using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// Helper class for OAuth 2.0 authorization code flow with PKCE (Proof Key for Code Exchange)
/// </summary>
public class OAuthFlowHelper
{
    private readonly ProcoreAuthOptions _options;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates a new OAuthFlowHelper instance
    /// </summary>
    /// <param name="options">Procore authentication options</param>
    /// <param name="httpClient">HTTP client for making token requests</param>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null</exception>
    public OAuthFlowHelper(IOptions<ProcoreAuthOptions> options, HttpClient httpClient)
    {
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Generates an OAuth 2.0 authorization URL with PKCE parameters
    /// </summary>
    /// <param name="state">Optional state parameter for security</param>
    /// <returns>A tuple containing the authorization URL and the code verifier for later use</returns>
    public (string AuthorizeUrl, string CodeVerifier) GenerateAuthorizationUrl(string? state = null)
    {
        var codeVerifier = GenerateCodeVerifier();
        var codeChallenge = GenerateCodeChallenge(codeVerifier);

        var queryParams = new Dictionary<string, string>
        {
            ["response_type"] = "code",
            ["client_id"] = _options.ClientId,
            ["redirect_uri"] = _options.RedirectUri,
            ["scope"] = string.Join(" ", _options.Scopes),
            ["code_challenge"] = codeChallenge,
            ["code_challenge_method"] = "S256"
        };

        if (!string.IsNullOrEmpty(state))
            queryParams["state"] = state;

        var query = string.Join("&", queryParams.Select(kvp =>
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

        var authorizeUrl = $"{_options.AuthorizationEndpoint}?{query}";

        return (authorizeUrl, codeVerifier);
    }

    /// <summary>
    /// Exchanges an authorization code for an access token
    /// </summary>
    /// <param name="code">The authorization code received from the authorization server</param>
    /// <param name="codeVerifier">The code verifier that was used to generate the code challenge</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>The access token received from the token endpoint</returns>
    /// <exception cref="ArgumentException">Thrown when code or codeVerifier is null or whitespace</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails</exception>
    /// <exception cref="JsonException">Thrown when the response cannot be parsed</exception>
    public async Task<AccessToken> ExchangeCodeForTokenAsync(
        string code,
        string codeVerifier,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Authorization code cannot be null or whitespace", nameof(code));
        if (string.IsNullOrWhiteSpace(codeVerifier))
            throw new ArgumentException("Code verifier cannot be null or whitespace", nameof(codeVerifier));
        var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _options.RedirectUri),
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                new KeyValuePair<string, string>("code_verifier", codeVerifier),
            })
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json)
            ?? throw new JsonException("Failed to deserialize token response");

        return new AccessToken(
            tokenResponse.AccessToken,
            tokenResponse.TokenType,
            DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
            tokenResponse.RefreshToken,
            tokenResponse.Scope?.Split(' '));
    }

    /// <summary>
    /// Generates a cryptographically secure code verifier for PKCE
    /// </summary>
    /// <returns>A URL-safe base64 encoded code verifier</returns>
    private static string GenerateCodeVerifier()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    /// <summary>
    /// Generates a code challenge from a code verifier using SHA256
    /// </summary>
    /// <param name="codeVerifier">The code verifier</param>
    /// <returns>A URL-safe base64 encoded SHA256 hash of the code verifier</returns>
    private static string GenerateCodeChallenge(string codeVerifier)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
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