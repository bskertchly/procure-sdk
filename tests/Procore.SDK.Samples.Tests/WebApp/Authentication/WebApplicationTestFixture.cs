using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using Procore.SDK.Samples.Tests.Shared.Fixtures;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Procore.SDK.Samples.Tests.WebApp.Authentication;

/// <summary>
/// Test fixture for web application testing with OAuth flows
/// Provides a complete ASP.NET Core test environment with mock services
/// </summary>
public class WebApplicationTestFixture : WebApplicationFactory<Program>, IDisposable
{
    private readonly TestableHttpMessageHandler _mockHttpHandler;
    private bool _disposed = false;

    public WebApplicationTestFixture()
    {
        _mockHttpHandler = new TestableHttpMessageHandler();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ProcoreAuth:ClientId"] = "test-web-client-id",
                ["ProcoreAuth:ClientSecret"] = "test-web-client-secret",
                ["ProcoreAuth:RedirectUri"] = "https://localhost:5001/auth/callback",
                ["ProcoreAuth:Scopes:0"] = "read",
                ["ProcoreAuth:Scopes:1"] = "write",
                ["ProcoreAuth:Scopes:2"] = "admin",
                ["ProcoreAuth:AuthorizationEndpoint"] = "https://app.procore.com/oauth/authorize",
                ["ProcoreAuth:TokenEndpoint"] = "https://api.procore.com/oauth/token",
                ["Logging:LogLevel:Default"] = "Debug"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove existing HTTP client registrations
            var httpClientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHttpClientFactory));
            if (httpClientDescriptor != null)
            {
                services.Remove(httpClientDescriptor);
            }

            // Register test HTTP client factory
            services.AddSingleton<IHttpClientFactory>(provider =>
                new TestHttpClientFactory(_mockHttpHandler));

            // Add test-specific services
            services.AddSingleton<TestSessionStorage>();
            services.AddScoped<IHttpContextAccessor, TestHttpContextAccessor>();
        });

        builder.UseEnvironment("Testing");
    }

    /// <summary>
    /// Mocks a successful token response for OAuth code exchange
    /// </summary>
    public void MockTokenResponse(object tokenResponse)
    {
        var json = JsonSerializer.Serialize(tokenResponse);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpHandler.SetResponse(request =>
            request.RequestUri?.AbsolutePath.Contains("/oauth/token") == true &&
            request.Method == HttpMethod.Post, response);
    }

    /// <summary>
    /// Mocks an error response for token requests
    /// </summary>
    public void MockTokenErrorResponse(HttpStatusCode statusCode, object errorResponse)
    {
        var json = JsonSerializer.Serialize(errorResponse);
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpHandler.SetResponse(request =>
            request.RequestUri?.AbsolutePath.Contains("/oauth/token") == true &&
            request.Method == HttpMethod.Post, response);
    }

    /// <summary>
    /// Mocks a network failure for HTTP requests
    /// </summary>
    public void MockNetworkFailure()
    {
        _mockHttpHandler.SetException(request => true,
            new HttpRequestException("Simulated network failure"));
    }

    /// <summary>
    /// Sets up session state for testing OAuth callback
    /// </summary>
    public async Task SetupSessionStateAsync(string state, string codeVerifier)
    {
        var sessionStorage = Services.GetRequiredService<TestSessionStorage>();
        await sessionStorage.SetStateAsync(state);
        await sessionStorage.SetCodeVerifierAsync(codeVerifier);
    }

    /// <summary>
    /// Sets up return URL in session for post-auth redirect testing
    /// </summary>
    public async Task SetupReturnUrlAsync(string returnUrl)
    {
        var sessionStorage = Services.GetRequiredService<TestSessionStorage>();
        await sessionStorage.SetReturnUrlAsync(returnUrl);
    }

    /// <summary>
    /// Clears all session storage for clean test state
    /// </summary>
    public void ClearSessionStorage()
    {
        var sessionStorage = Services.GetRequiredService<TestSessionStorage>();
        sessionStorage.Clear();
    }

    /// <summary>
    /// Sets up a pre-authenticated state with valid token
    /// </summary>
    public async Task SetupAuthenticatedStateAsync(
        string accessToken = "test-web-access-token",
        int expiresInSeconds = 3600,
        string? refreshToken = "test-web-refresh-token",
        string[]? scopes = null)
    {
        var token = new AccessToken(
            accessToken,
            "Bearer",
            DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds),
            refreshToken,
            scopes ?? new[] { "read", "write", "admin" });

        var tokenManager = Services.GetRequiredService<ITokenManager>();
        await tokenManager.StoreTokenAsync(token);
    }

    /// <summary>
    /// Gets all captured HTTP requests for verification
    /// </summary>
    public IReadOnlyList<HttpRequestMessage> GetCapturedRequests()
    {
        return _mockHttpHandler.GetCapturedRequests();
    }

    /// <summary>
    /// Verifies HTTP requests were made with proper authentication
    /// </summary>
    public void VerifyAuthenticatedRequest(string expectedToken)
    {
        var requests = _mockHttpHandler.GetCapturedRequests();
        var authenticatedRequest = requests.FirstOrDefault(r =>
            r.Headers.Authorization?.Scheme == "Bearer");

        authenticatedRequest.Should().NotBeNull("Should include authenticated requests");
        authenticatedRequest!.Headers.Authorization!.Parameter.Should().Be(expectedToken,
            "Should use correct access token");
    }

    /// <summary>
    /// Resets all mock configurations and captured data
    /// </summary>
    public void Reset()
    {
        _mockHttpHandler.Reset();
        ClearSessionStorage();
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _mockHttpHandler?.Dispose();
            _disposed = true;
        }
        base.Dispose(disposing);
    }
}

/// <summary>
/// Test implementation of session storage for web application testing
/// </summary>
public class TestSessionStorage
{
    private readonly Dictionary<string, string> _sessionData = new();
    private readonly object _lock = new();

    public Task SetStateAsync(string state)
    {
        lock (_lock)
        {
            _sessionData["oauth_state"] = state;
        }
        return Task.CompletedTask;
    }

    public Task SetCodeVerifierAsync(string codeVerifier)
    {
        lock (_lock)
        {
            _sessionData["oauth_code_verifier"] = codeVerifier;
        }
        return Task.CompletedTask;
    }

    public Task SetReturnUrlAsync(string returnUrl)
    {
        lock (_lock)
        {
            _sessionData["return_url"] = returnUrl;
        }
        return Task.CompletedTask;
    }

    public Task<string?> GetStateAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(_sessionData.TryGetValue("oauth_state", out var state) ? state : null);
        }
    }

    public Task<string?> GetCodeVerifierAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(_sessionData.TryGetValue("oauth_code_verifier", out var verifier) ? verifier : null);
        }
    }

    public Task<string?> GetReturnUrlAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(_sessionData.TryGetValue("return_url", out var url) ? url : null);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _sessionData.Clear();
        }
    }

    public bool HasState(string state)
    {
        lock (_lock)
        {
            return _sessionData.TryGetValue("oauth_state", out var storedState) && storedState == state;
        }
    }
}

/// <summary>
/// Test implementation of IHttpContextAccessor for dependency injection
/// </summary>
public class TestHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; }
}