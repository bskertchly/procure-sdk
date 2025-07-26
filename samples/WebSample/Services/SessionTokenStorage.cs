using Microsoft.Extensions.Caching.Memory;
using Procore.SDK.Shared.Authentication;
using System.Text.Json;
using System.Security.Cryptography;

namespace WebSample.Services;

/// <summary>
/// Session-based token storage implementation for web applications
/// Stores tokens in user session with memory cache fallback
/// </summary>
public class SessionTokenStorage : ITokenStorage
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SessionTokenStorage> _logger;
    private const string TokenSessionKey = "procore_access_token";

    public SessionTokenStorage(
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache,
        ILogger<SessionTokenStorage> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<AccessToken?> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session == null)
            {
                _logger.LogWarning("No HTTP context or session available for token retrieval");
                return null;
            }

            // Try to get token from session
            var tokenJson = httpContext.Session.GetString(TokenSessionKey);
            if (string.IsNullOrEmpty(tokenJson))
            {
                _logger.LogDebug("No token found in session");
                return null;
            }

            // Deserialize token with proper error handling
            AccessToken? token;
            try
            {
                token = JsonSerializer.Deserialize<AccessToken>(tokenJson);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to deserialize token from session, clearing corrupted data");
                await ClearTokenAsync(cancellationToken);
                return null;
            }

            if (token != null)
            {
                _logger.LogDebug("Token retrieved from session successfully");
                
                // Check if token is expired
                if (token.ExpiresAt <= DateTimeOffset.UtcNow)
                {
                    _logger.LogInformation("Token found in session but is expired");
                    // Clear expired token
                    await ClearTokenAsync(cancellationToken);
                    return null;
                }
            }

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve token from session");
            return null;
        }
    }

    public Task StoreTokenAsync(AccessToken token, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(token);

        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session == null)
            {
                throw new InvalidOperationException("No HTTP context or session available for token storage");
            }

            // Serialize token to JSON
            var tokenJson = JsonSerializer.Serialize(token);
            
            // Store in session
            httpContext.Session.SetString(TokenSessionKey, tokenJson);

            // Also store in memory cache as backup with user session ID
            var sessionId = httpContext.Session.Id;
            if (!string.IsNullOrEmpty(sessionId))
            {
                var cacheKey = $"token_{sessionId}";
                _memoryCache.Set(cacheKey, token, token.ExpiresAt);
            }

            _logger.LogDebug("Token stored in session successfully");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to store token in session");
            throw;
        }
    }

    public Task ClearTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session != null)
            {
                // Remove from session
                httpContext.Session.Remove(TokenSessionKey);

                // Remove from memory cache
                var sessionId = httpContext.Session.Id;
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var cacheKey = $"token_{sessionId}";
                    _memoryCache.Remove(cacheKey);
                }

                _logger.LogDebug("Token cleared from session");
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clear token from session");
            throw;
        }
    }

    /// <summary>
    /// Gets the current user's session ID for debugging purposes
    /// </summary>
    public string? GetCurrentSessionId()
    {
        return _httpContextAccessor.HttpContext?.Session?.Id;
    }

    /// <summary>
    /// Checks if the current session has a token stored
    /// </summary>
    public bool HasToken()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session == null)
                return false;

            var tokenJson = httpContext.Session.GetString(TokenSessionKey);
            return !string.IsNullOrEmpty(tokenJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check if session has token");
            return false;
        }
    }
}