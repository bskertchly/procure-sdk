using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// HttpMessageHandler that automatically adds OAuth 2.0 Bearer tokens to requests
/// and handles token refresh on 401 Unauthorized responses
/// </summary>
public class ProcoreAuthHandler : DelegatingHandler
{
    private readonly ITokenManager _tokenManager;
    private readonly ILogger<ProcoreAuthHandler> _logger;
    private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);

    /// <summary>
    /// Creates a new ProcoreAuthHandler instance
    /// </summary>
    /// <param name="tokenManager">Token manager for retrieving and refreshing tokens</param>
    /// <param name="logger">Logger for diagnostic information</param>
    public ProcoreAuthHandler(ITokenManager tokenManager, ILogger<ProcoreAuthHandler> logger)
    {
        _tokenManager = tokenManager;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Add authorization header if not already present
        await AddAuthorizationHeaderAsync(request, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);

        // Handle 401 Unauthorized - attempt token refresh and retry once
        if (response.StatusCode == HttpStatusCode.Unauthorized && !HasBeenRetried(request))
        {
            _logger.LogDebug("Received 401 Unauthorized, attempting to refresh token and retry");

            await _refreshSemaphore.WaitAsync(cancellationToken);
            try
            {
                var newToken = await _tokenManager.RefreshTokenAsync(cancellationToken);

                // Clone the request and add new token
                var retryRequest = await CloneRequestAsync(request, cancellationToken);
                retryRequest.Headers.Authorization = new AuthenticationHeaderValue(
                    newToken.TokenType, newToken.Token);

                // Mark this request as a retry to prevent infinite loops
                MarkAsRetried(retryRequest);

                response.Dispose();
                response = await base.SendAsync(retryRequest, cancellationToken);

                _logger.LogDebug("Token refresh and retry completed with status: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to refresh token on 401 response");
                // Return the original 401 response if refresh fails
            }
            finally
            {
                _refreshSemaphore.Release();
            }
        }

        return response;
    }

    /// <summary>
    /// Adds the authorization header to the request if not already present
    /// </summary>
    private async Task AddAuthorizationHeaderAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization != null)
        {
            _logger.LogTrace("Authorization header already present, skipping token injection");
            return; // Already has authorization header
        }

        var token = await _tokenManager.GetAccessTokenAsync(cancellationToken);
        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                token.TokenType, token.Token);
            _logger.LogTrace("Added authorization header to request");
        }
        else
        {
            _logger.LogDebug("No access token available, request will be sent without authorization");
        }
    }

    /// <summary>
    /// Clones an HTTP request message for retry scenarios
    /// </summary>
    private static async Task<HttpRequestMessage> CloneRequestAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        // Copy headers (except authorization which will be set separately)
        foreach (var header in request.Headers)
        {
            if (header.Key != "Authorization")
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Copy content
        if (request.Content != null)
        {
            var content = await request.Content.ReadAsByteArrayAsync(cancellationToken);
            clone.Content = new ByteArrayContent(content);

            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return clone;
    }

    /// <summary>
    /// Option key for tracking request retry status
    /// </summary>
    private static readonly HttpRequestOptionsKey<bool> RetriedOptionKey = new("ProcoreAuthHandler.Retried");

    /// <summary>
    /// Checks if a request has already been retried to prevent infinite retry loops
    /// </summary>
    private static bool HasBeenRetried(HttpRequestMessage request)
    {
        return request.Options.TryGetValue(RetriedOptionKey, out var retried) && retried;
    }

    /// <summary>
    /// Marks a request as having been retried
    /// </summary>
    private static void MarkAsRetried(HttpRequestMessage request)
    {
        request.Options.Set(RetriedOptionKey, true);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _refreshSemaphore.Dispose();
        }
        base.Dispose(disposing);
    }
}