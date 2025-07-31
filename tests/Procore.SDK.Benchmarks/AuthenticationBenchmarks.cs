using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Procore.SDK.Benchmarks;

/// <summary>
/// Benchmarks for authentication operations
/// Target: Token refresh &lt;200ms, no memory leaks
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class AuthenticationBenchmarks
{
    private ServiceProvider _serviceProvider = null!;
    private ITokenManager _tokenManager = null!;
    private OAuthFlowHelper _oauthHelper = null!;
    private ITokenStorage _tokenStorage = null!;
    private AccessToken _testToken = null!;

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Configure authentication options
        services.Configure<ProcoreAuthOptions>(options =>
        {
            options.ClientId = "test-client-id";
            options.ClientSecret = "test-client-secret";
            options.RedirectUri = new Uri("http://localhost:8080/callback");
            options.Scopes = ["read", "write"];
            options.AuthorizationEndpoint = new Uri("https://app.procore.com/oauth/authorize");
            options.TokenEndpoint = new Uri("https://app.procore.com/oauth/token");
        });

        services.AddHttpClient();
        services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<OAuthFlowHelper>();

        _serviceProvider = services.BuildServiceProvider();
        _tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        _oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        _tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();

        // Create test token
        _testToken = new AccessToken(
            "test-access-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "test-refresh-token",
            ["read", "write"]);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _serviceProvider?.Dispose();
    }

    /// <summary>
    /// Benchmark token storage operations
    /// </summary>
    [Benchmark]
    public async Task StoreAndRetrieveToken()
    {
        const string key = "test-key";
        await _tokenStorage.StoreTokenAsync(key, _testToken);
        var retrieved = await _tokenStorage.GetTokenAsync(key);
        await _tokenStorage.DeleteTokenAsync(key);
    }

    /// <summary>
    /// Benchmark PKCE code generation
    /// </summary>
    [Benchmark]
    public void GenerateAuthorizationUrl()
    {
        var (url, verifier) = _oauthHelper.GenerateAuthorizationUrl("test-state");
        // Ensure the result is used
        GC.KeepAlive(url);
        GC.KeepAlive(verifier);
    }

    /// <summary>
    /// Benchmark token validation
    /// </summary>
    [Benchmark]
    public void ValidateToken()
    {
        var isValid = _testToken.ExpiresAt > DateTimeOffset.UtcNow;
        var willExpireSoon = _testToken.ExpiresAt < DateTimeOffset.UtcNow.AddMinutes(5);
        GC.KeepAlive(isValid);
        GC.KeepAlive(willExpireSoon);
    }

    /// <summary>
    /// Benchmark memory usage of token operations
    /// </summary>
    [Benchmark]
    public async Task TokenManagerOperations()
    {
        // Store token
        await _tokenManager.StoreTokenAsync(_testToken);

        // Get token
        var token = await _tokenManager.GetAccessTokenAsync();

        // Clear token
        await _tokenManager.ClearTokenAsync();

        GC.KeepAlive(token);
    }

    /// <summary>
    /// Stress test for memory leaks
    /// </summary>
    [Benchmark]
    [Arguments(100)]
    public async Task StressTestTokenOperations(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            var key = $"test-key-{i}";
            var token = new AccessToken(
                $"token-{i}",
                "Bearer",
                DateTimeOffset.UtcNow.AddHours(1),
                $"refresh-{i}",
                ["read"]);

            await _tokenStorage.StoreTokenAsync(key, token);
            var retrieved = await _tokenStorage.GetTokenAsync(key);
            await _tokenStorage.DeleteTokenAsync(key);
        }
    }
}