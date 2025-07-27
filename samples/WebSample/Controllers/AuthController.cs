using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSample.Models;
using WebSample.Services;

namespace WebSample.Controllers;

/// <summary>
/// Controller handling OAuth authentication flows for the web application
/// Demonstrates proper OAuth PKCE implementation with state validation and error handling
/// </summary>
public class AuthController : Controller
{
    private readonly AuthenticationService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthenticationService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Displays the login page and initiates OAuth flow if requested
    /// </summary>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// Initiates the OAuth PKCE flow by redirecting to Procore's authorization server
    /// </summary>
    [HttpPost]
    public IActionResult StartOAuth(string? returnUrl = null)
    {
        try
        {
            // Store return URL in session for post-auth redirect
            if (!string.IsNullOrEmpty(returnUrl))
            {
                HttpContext.Session.SetString("ReturnUrl", returnUrl);
            }

            // Generate authorization URL and redirect
            var authUrl = _authService.InitiateOAuthFlow();
            
            _logger.LogInformation("Redirecting user to OAuth authorization URL");
            return Redirect(authUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initiate OAuth flow");
            TempData["Error"] = "Failed to start authentication process. Please try again.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }
    }

    /// <summary>
    /// Handles the OAuth callback from Procore's authorization server
    /// Validates state parameter and exchanges authorization code for access token
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Callback(string? code, string? state, string? error, string? error_description)
    {
        try
        {
            // Check for OAuth error response
            if (!string.IsNullOrEmpty(error))
            {
                _logger.LogWarning("OAuth error received: {Error} - {Description}", error, error_description);
                
                var errorModel = new AuthErrorViewModel
                {
                    Error = error,
                    ErrorDescription = error_description ?? "An authentication error occurred"
                };
                
                return View("Error", errorModel);
            }

            // Validate callback parameters
            if (!_authService.ValidateCallbackParameters(code, state, error))
            {
                _logger.LogWarning("Invalid callback parameters received");
                return View("Error", new AuthErrorViewModel
                {
                    Error = "invalid_request",
                    ErrorDescription = "Invalid authentication response received"
                });
            }

            // Handle the callback and exchange code for token
            var success = await _authService.HandleCallbackAsync(code!, state!);
            
            if (!success)
            {
                _logger.LogWarning("Failed to handle OAuth callback");
                return View("Error", new AuthErrorViewModel
                {
                    Error = "authentication_failed",
                    ErrorDescription = "Failed to complete authentication process"
                });
            }

            _logger.LogInformation("OAuth authentication completed successfully");

            // Redirect to original requested URL or dashboard
            var returnUrl = HttpContext.Session.GetString("ReturnUrl");
            HttpContext.Session.Remove("ReturnUrl");
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Dashboard", "Projects");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during OAuth callback handling");
            return View("Error", new AuthErrorViewModel
            {
                Error = "server_error",
                ErrorDescription = "An unexpected error occurred during authentication"
            });
        }
    }

    /// <summary>
    /// Signs out the current user and clears authentication state
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _authService.SignOutAsync();
            _logger.LogInformation("User signed out successfully");
            
            TempData["Message"] = "You have been signed out successfully";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sign out user");
            TempData["Error"] = "Failed to sign out. Please try again.";
            return RedirectToAction("Index", "Home");
        }
    }

    /// <summary>
    /// Displays access denied page
    /// </summary>
    public IActionResult AccessDenied(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// API endpoint to check authentication status (for AJAX calls)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Status()
    {
        try
        {
            var isAuthenticated = await _authService.IsAuthenticatedAsync();
            var token = isAuthenticated ? await _authService.GetCurrentTokenAsync() : null;
            
            return Json(new
            {
                authenticated = isAuthenticated,
                expires_at = token?.ExpiresAt.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
                scopes = token?.Scopes ?? Array.Empty<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get authentication status");
            return Json(new { authenticated = false, error = "Failed to check authentication status" });
        }
    }

    /// <summary>
    /// Endpoint to refresh authentication token (for AJAX calls)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var isAuthenticated = await _authService.IsAuthenticatedAsync();
            if (!isAuthenticated)
            {
                return Json(new { success = false, error = "Not authenticated" });
            }

            // Token refresh would be handled automatically by the TokenManager
            // This endpoint is for explicit refresh requests
            var token = await _authService.GetCurrentTokenAsync();
            
            return Json(new
            {
                success = true,
                expires_at = token?.ExpiresAt.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
                scopes = token?.Scopes ?? Array.Empty<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh token");
            return Json(new { success = false, error = "Failed to refresh authentication" });
        }
    }
}