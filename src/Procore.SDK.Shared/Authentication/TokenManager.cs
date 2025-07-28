using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// Manages OAuth 2.0 access tokens including automatic refresh and storage
/// </summary>
public class TokenManager : ITokenManager
{
    private readonly ITokenStorage _storage;
    private readonly ProcoreAuthOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenManager> _logger;
    private readonly string _storageKey;
    private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);

    /// <inheritdoc />
    public event EventHandler<TokenRefreshedEventArgs>? TokenRefreshed;

    /// <summary>
    /// Creates a new TokenManager instance
    /// </summary>
    /// <param name="storage">Token storage implementation</param>
    /// <param name="options">Procore authentication options</param>
    /// <param name="httpClient">HTTP client for making token requests</param>
    /// <param name="logger">Logger for diagnostic information</param>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null</exception>
    public TokenManager(
        ITokenStorage storage,
        IOptions<ProcoreAuthOptions> options,
        HttpClient httpClient,
        ILogger<TokenManager> logger)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _storageKey = $"procore_token_{_options.ClientId}";
    }

    /// <inheritdoc />
    public async Task<AccessToken?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        AccessToken? token;
        try
        {
            token = await _storage.GetTokenAsync(_storageKey, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve token from storage");
            return null;
        }

        if (token == null)
        {
            _logger.LogDebug("No access token found in storage");
            return null;
        }

        // Check if token needs refresh
        if (token.ExpiresAt <= DateTimeOffset.UtcNow.Add(_options.TokenRefreshMargin))
        {
            _logger.LogDebug("Access token is expired or near expiration, attempting refresh");

            try
            {
                token = await RefreshTokenAsync(cancellationToken);
                _logger.LogDebug("Successfully refreshed access token");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to refresh token, returning expired token");
                return token;
            }
        }

        return token;
    }

    /// <inheritdoc />
    public async Task<AccessToken> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        await _refreshSemaphore.WaitAsync(cancellationToken);
        try
        {
            var currentToken = await _storage.GetTokenAsync(_storageKey, cancellationToken);

            if (currentToken?.RefreshToken == null)
            {
                throw new InvalidOperationException("No refresh token available");
            }

            _logger.LogDebug("Refreshing access token using refresh token");

            var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", currentToken.RefreshToken),
                    new KeyValuePair<string, string>("client_id", _options.ClientId),
                    new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                })
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json)
                ?? throw new InvalidOperationException("Failed to deserialize token response");

            // Validate required fields
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new InvalidOperationException("Token response missing required 'access_token' field");
            }

            if (string.IsNullOrEmpty(tokenResponse.TokenType))
            {
                throw new InvalidOperationException("Token response missing required 'token_type' field");
            }

            var newToken = new AccessToken(
                tokenResponse.AccessToken,
                tokenResponse.TokenType,
                DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                tokenResponse.RefreshToken ?? currentToken.RefreshToken,
                tokenResponse.Scope?.Split(' '));

            await StoreTokenAsync(newToken, cancellationToken);

            TokenRefreshed?.Invoke(this, new TokenRefreshedEventArgs(newToken, currentToken));

            _logger.LogInformation("Access token refreshed successfully");

            return newToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh access token");
            throw;
        }
        finally
        {
            _refreshSemaphore.Release();
        }
    }

    /// <inheritdoc />
    public async Task StoreTokenAsync(AccessToken token, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(token);

        await _storage.StoreTokenAsync(_storageKey, token, cancellationToken);
        _logger.LogDebug("Access token stored successfully");
    }

    /// <inheritdoc />
    public async Task ClearTokenAsync(CancellationToken cancellationToken = default)
    {
        await _storage.DeleteTokenAsync(_storageKey, cancellationToken);
        _logger.LogDebug("Access token cleared from storage");
    }

    /// <summary>
    /// Internal record for deserializing token responses
    /// </summary>
    private sealed record TokenResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expires_in")] int ExpiresIn,
        [property: JsonPropertyName("refresh_token")] string? RefreshToken,
        [property: JsonPropertyName("scope")] string? Scope);
}