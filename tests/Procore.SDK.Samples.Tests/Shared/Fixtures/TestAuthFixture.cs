using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Procore.SDK.Samples.Tests.Shared.Fixtures;

/// <summary>
/// Test fixture for authentication testing with mock HTTP responses and dependency injection setup
/// Provides a complete testing environment for OAuth flows with controlled HTTP responses
/// </summary>
public class TestAuthFixture : IDisposable
{
    private readonly TestableHttpMessageHandler _mockHttpHandler;
    private readonly IHost _host;
    private bool _disposed = false;

    public TestAuthFixture()
    {
        _mockHttpHandler = new TestableHttpMessageHandler();
        _host = CreateHost();
    }

    public IServiceProvider ServiceProvider => _host.Services;

    private IHost CreateHost()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ProcoreAuth:ClientId"] = "test-client-id",
                ["ProcoreAuth:ClientSecret"] = "test-client-secret", 
                ["ProcoreAuth:RedirectUri"] = "https://localhost:5001/auth/callback",
                ["ProcoreAuth:Scopes:0"] = "read",
                ["ProcoreAuth:Scopes:1"] = "write", 
                ["ProcoreAuth:Scopes:2"] = "admin",
                ["ProcoreAuth:AuthorizationEndpoint"] = "https://app.procore.com/oauth/authorize",
                ["ProcoreAuth:TokenEndpoint"] = "https://api.procore.com/oauth/token",
                ["Logging:LogLevel:Default"] = "Information",
                ["Logging:LogLevel:Microsoft"] = "Warning"
            })
            .Build();

        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register Procore SDK services with test configuration
                services.AddProcoreSDK(configuration);
                
                // Override HTTP client with mock handler
                services.Configure<HttpClientOptions>(options =>
                {
                    options.BaseAddress = new Uri("https://api.procore.com");
                });

                // Replace HTTP client factory with test version
                services.AddSingleton<IHttpClientFactory>(provider =>
                    new TestHttpClientFactory(_mockHttpHandler));

                // Add test-specific services
                services.AddSingleton<TestTokenStorage>();
                services.AddSingleton<ITokenStorage>(provider => 
                    provider.GetRequiredService<TestTokenStorage>());
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .Build();
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
    /// Mocks a successful refresh token response
    /// </summary>
    public void MockRefreshTokenResponse(object refreshResponse)
    {
        var json = JsonSerializer.Serialize(refreshResponse);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpHandler.SetResponse(request =>
        {
            if (request.RequestUri?.AbsolutePath.Contains("/oauth/token") != true ||
                request.Method != HttpMethod.Post)
                return false;

            // Check if it's a refresh token request
            var content = request.Content?.ReadAsStringAsync().Result ?? "";
            return content.Contains("grant_type=refresh_token");
        }, response);
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
    /// Mocks an error response for refresh token requests
    /// </summary>
    public void MockRefreshTokenErrorResponse(HttpStatusCode statusCode, object errorResponse)
    {
        var json = JsonSerializer.Serialize(errorResponse);
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpHandler.SetResponse(request =>
        {
            if (request.RequestUri?.AbsolutePath.Contains("/oauth/token") != true ||
                request.Method != HttpMethod.Post)
                return false;

            var content = request.Content?.ReadAsStringAsync().Result ?? "";
            return content.Contains("grant_type=refresh_token");
        }, response);
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
    /// Mocks API responses for CRUD operations
    /// </summary>
    public void MockApiResponse(string endpoint, object response, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var json = JsonSerializer.Serialize(response);
        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpHandler.SetResponse(request =>
            request.RequestUri?.AbsolutePath.Contains(endpoint) == true, httpResponse);
    }

    /// <summary>
    /// Gets the test token storage for verification
    /// </summary>
    public TestTokenStorage GetTokenStorage()
    {
        return ServiceProvider.GetRequiredService<TestTokenStorage>();
    }

    /// <summary>
    /// Clears all stored tokens for clean test state
    /// </summary>
    public async Task ClearTokensAsync()
    {
        var tokenStorage = GetTokenStorage();
        await tokenStorage.ClearTokenAsync();
    }

    /// <summary>
    /// Sets up a pre-authenticated state with valid token
    /// </summary>
    public async Task SetupAuthenticatedStateAsync(
        string accessToken = "test-access-token",
        int expiresInSeconds = 3600,
        string? refreshToken = "test-refresh-token",
        string[]? scopes = null)
    {
        var token = new AccessToken(
            accessToken,
            "Bearer",
            DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds),
            refreshToken,
            scopes ?? new[] { "read", "write" });

        var tokenManager = ServiceProvider.GetRequiredService<ITokenManager>();
        await tokenManager.StoreTokenAsync(token);
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
    /// Gets all captured HTTP requests for verification
    /// </summary>
    public IReadOnlyList<HttpRequestMessage> GetCapturedRequests()
    {
        return _mockHttpHandler.GetCapturedRequests();
    }

    /// <summary>
    /// Resets all mock configurations and captured data
    /// </summary>
    public void Reset()
    {
        _mockHttpHandler.Reset();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _host?.Dispose();
            _mockHttpHandler?.Dispose();
            _disposed = true;
        }
    }
}

/// <summary>
/// Test implementation of token storage for verification in tests
/// </summary>
public class TestTokenStorage : ITokenStorage
{
    private AccessToken? _storedToken;
    private readonly object _lock = new();

    public Task<AccessToken?> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_storedToken);
        }
    }

    public Task StoreTokenAsync(AccessToken token, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        lock (_lock)
        {
            _storedToken = token;
        }
        return Task.CompletedTask;
    }

    public Task ClearTokenAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _storedToken = null;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Test helper to check if token is stored
    /// </summary>
    public bool HasToken()
    {
        lock (_lock)
        {
            return _storedToken != null;
        }
    }

    /// <summary>
    /// Test helper to get stored token for verification
    /// </summary>
    public AccessToken? GetStoredToken()
    {
        lock (_lock)
        {
            return _storedToken;
        }
    }
}

/// <summary>
/// Test HTTP client factory that uses the mock handler
/// </summary>
public class TestHttpClientFactory : IHttpClientFactory
{
    private readonly HttpMessageHandler _handler;

    public TestHttpClientFactory(HttpMessageHandler handler)
    {
        _handler = handler;
    }

    public HttpClient CreateClient(string name)
    {
        return new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.procore.com")
        };
    }
}