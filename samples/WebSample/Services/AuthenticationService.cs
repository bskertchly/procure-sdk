using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Procore.SDK.Shared.Authentication;
using System.Security.Claims;

namespace WebSample.Services;

/// <summary>
/// Service for handling OAuth authentication flows in web applications
/// Manages the complete OAuth PKCE flow with proper state validation and error handling
/// </summary>
public class AuthenticationService
{
    private readonly OAuthFlowHelper _oauthHelper;
    private readonly ITokenManager _tokenManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthenticationService> _logger;
    private const string StateSessionKey = "oauth_state";
    private const string CodeVerifierSessionKey = "oauth_code_verifier";

    public AuthenticationService(
        OAuthFlowHelper oauthHelper,
        ITokenManager tokenManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthenticationService> logger)
    {
        _oauthHelper = oauthHelper;
        _tokenManager = tokenManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Initiates the OAuth PKCE flow by generating authorization URL and storing state
    /// </summary>
    /// <returns>Authorization URL to redirect the user to</returns>
    public string InitiateOAuthFlow()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session == null)
            {
                throw new InvalidOperationException("No HTTP context or session available");
            }

            // Generate state and code verifier
            var state = GenerateSecureState();
            var (authUrl, codeVerifier) = _oauthHelper.GenerateAuthorizationUrl(state);

            // Store state and code verifier in session for validation
            httpContext.Session.SetString(StateSessionKey, state);
            httpContext.Session.SetString(CodeVerifierSessionKey, codeVerifier);

            _logger.LogInformation("OAuth flow initiated with state: {State}", state);
            return authUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initiate OAuth flow");
            throw;
        }
    }

    /// <summary>
    /// Handles the OAuth callback with authorization code and state validation
    /// </summary>
    /// <param name="code">Authorization code from OAuth provider</param>
    /// <param name="state">State parameter for CSRF protection</param>
    /// <returns>True if authentication was successful</returns>
    public async Task<bool> HandleCallbackAsync(string code, string state)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session == null)
            {
                _logger.LogError("No HTTP context or session available for callback handling");
                return false;
            }

            // Validate state parameter (CSRF protection)
            var storedState = httpContext.Session.GetString(StateSessionKey);
            if (string.IsNullOrEmpty(storedState) || storedState != state)
            {
                _logger.LogWarning("Invalid or missing state parameter. Stored: {StoredState}, Received: {ReceivedState}", 
                    storedState, state);
                return false;
            }

            // Get code verifier from session
            var codeVerifier = httpContext.Session.GetString(CodeVerifierSessionKey);
            if (string.IsNullOrEmpty(codeVerifier))
            {
                _logger.LogError("Code verifier not found in session");
                return false;
            }

            // Exchange authorization code for access token
            var token = await _oauthHelper.ExchangeCodeForTokenAsync(code, codeVerifier);
            
            // Store token
            await _tokenManager.StoreTokenAsync(token);

            // Create authentication claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "Procore User"),
                new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new("access_token", token.Token),
                new("token_expires", token.ExpiresAt.ToString("O")),
                new("scopes", string.Join(",", token.Scopes))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = token.ExpiresAt,
                IsPersistent = false,
                AllowRefresh = true
            };

            // Sign in the user
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Clean up session state
            httpContext.Session.Remove(StateSessionKey);
            httpContext.Session.Remove(CodeVerifierSessionKey);

            _logger.LogInformation("OAuth callback handled successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to handle OAuth callback");
            return false;
        }
    }

    /// <summary>
    /// Signs out the current user and clears authentication state
    /// </summary>
    public async Task SignOutAsync()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Clear stored token
                await _tokenManager.ClearTokenAsync();

                // Sign out of cookie authentication
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Clear session
                httpContext.Session.Clear();

                _logger.LogInformation("User signed out successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sign out user");
            throw;
        }
    }

    /// <summary>
    /// Checks if the current user is authenticated and has a valid token
    /// </summary>
    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return false;
            }

            // Check if we have a valid token
            var token = await _tokenManager.GetAccessTokenAsync();
            return token != null && token.ExpiresAt > DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check authentication status");
            return false;
        }
    }

    /// <summary>
    /// Gets the current access token for the authenticated user
    /// </summary>
    public async Task<AccessToken?> GetCurrentTokenAsync()
    {
        try
        {
            if (!await IsAuthenticatedAsync())
            {
                return null;
            }

            return await _tokenManager.GetAccessTokenAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get current token");
            return null;
        }
    }

    /// <summary>
    /// Validates that the callback contains required parameters
    /// </summary>
    public bool ValidateCallbackParameters(string? code, string? state, string? error)
    {
        // Check for OAuth error response
        if (!string.IsNullOrEmpty(error))
        {
            _logger.LogWarning("OAuth error received: {Error}", error);
            return false;
        }

        // Check required parameters
        if (string.IsNullOrEmpty(code))
        {
            _logger.LogWarning("Missing authorization code in callback");
            return false;
        }

        if (string.IsNullOrEmpty(state))
        {
            _logger.LogWarning("Missing state parameter in callback");
            return false;
        }

        return true;
    }

    private static string GenerateSecureState()
    {
        return $"web-{Guid.NewGuid():N}-{DateTimeOffset.UtcNow.Ticks}";
    }
}