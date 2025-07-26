using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Shared.Authentication;
using System.Diagnostics;

namespace Procore.SDK.Samples.Tests.Performance;

/// <summary>
/// Performance benchmarks and load tests for authentication components
/// Validates performance characteristics and scalability of OAuth flows
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class AuthenticationPerformanceTests : IClassFixture<TestAuthFixture>
{
    private readonly TestAuthFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly OAuthFlowHelper _oauthHelper;
    private readonly ITokenManager _tokenManager;
    private readonly ILogger<AuthenticationPerformanceTests> _logger;

    public AuthenticationPerformanceTests(TestAuthFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        _tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        _logger = _serviceProvider.GetRequiredService<ILogger<AuthenticationPerformanceTests>>();
    }

    #region BenchmarkDotNet Benchmarks

    [Benchmark]
    public (string AuthUrl, string CodeVerifier) GenerateAuthorizationUrl()
    {
        return _oauthHelper.GenerateAuthorizationUrl("benchmark-state");
    }

    [Benchmark]
    public async Task<AccessToken> TokenExchange()
    {
        _fixture.MockTokenResponse(new
        {
            access_token = "benchmark-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "benchmark-refresh",
            scope = "read write"
        });

        var (_, codeVerifier) = _oauthHelper.GenerateAuthorizationUrl();
        return await _oauthHelper.ExchangeCodeForTokenAsync("benchmark-code", codeVerifier);
    }

    [Benchmark]
    public async Task TokenStorageRetrival()
    {
        var token = new AccessToken(
            "benchmark-stored-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "benchmark-refresh",
            new[] { "read", "write" });

        await _tokenManager.StoreTokenAsync(token);
        await _tokenManager.GetAccessTokenAsync();
    }

    [Benchmark]
    public async Task TokenRefresh()
    {
        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-benchmark-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-benchmark-refresh"
        });

        return await _tokenManager.RefreshTokenAsync();
    }

    #endregion

    #region Load Tests

    [Fact]
    public async Task LoadTest_ConcurrentAuthorizationUrlGeneration_ShouldMaintainPerformance()
    {
        // Arrange
        const int concurrentRequests = 100;
        var stopwatch = Stopwatch.StartNew();
        var tasks = new List<Task<(string, string)>>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks.Add(Task.Run(() => _oauthHelper.GenerateAuthorizationUrl($"load-test-{i}")));
        }

        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        results.Should().HaveCount(concurrentRequests, "All requests should complete");
        results.Should().AllSatisfy(result =>
        {
            result.Item1.Should().NotBeNullOrEmpty("Auth URL should be generated");
            result.Item2.Should().NotBeNullOrEmpty("Code verifier should be generated");
        });

        var averageTimeMs = stopwatch.ElapsedMilliseconds / (double)concurrentRequests;
        averageTimeMs.Should().BeLessThan(10, "Average time per request should be under 10ms");

        _logger.LogInformation(
            "Load test: {Requests} concurrent auth URL generations in {Elapsed}ms (avg: {Average}ms)",
            concurrentRequests, stopwatch.ElapsedMilliseconds, averageTimeMs);
    }

    [Fact]
    public async Task LoadTest_ConcurrentTokenExchange_ShouldHandleLoad()
    {
        // Arrange
        const int concurrentRequests = 50;
        var stopwatch = Stopwatch.StartNew();

        _fixture.MockTokenResponse(new
        {
            access_token = "load-test-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "load-test-refresh",
            scope = "read write admin"
        });

        var tasks = new List<Task<AccessToken>>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            var (_, codeVerifier) = _oauthHelper.GenerateAuthorizationUrl();
            tasks.Add(_oauthHelper.ExchangeCodeForTokenAsync($"load-test-code-{i}", codeVerifier));
        }

        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        results.Should().HaveCount(concurrentRequests, "All token exchanges should complete");
        results.Should().AllSatisfy(token =>
        {
            token.Should().NotBeNull("Token should be obtained");
            token.Token.Should().Be("load-test-token");
        });

        var averageTimeMs = stopwatch.ElapsedMilliseconds / (double)concurrentRequests;
        averageTimeMs.Should().BeLessThan(100, "Average time per token exchange should be under 100ms");

        _logger.LogInformation(
            "Load test: {Requests} concurrent token exchanges in {Elapsed}ms (avg: {Average}ms)",
            concurrentRequests, stopwatch.ElapsedMilliseconds, averageTimeMs);
    }

    [Fact]
    public async Task LoadTest_ConcurrentTokenRetrieval_ShouldMaintainConsistency()
    {
        // Arrange
        const int concurrentRequests = 200;
        
        var testToken = new AccessToken(
            "concurrent-retrieval-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "concurrent-refresh",
            new[] { "read", "write" });

        await _tokenManager.StoreTokenAsync(testToken);

        var stopwatch = Stopwatch.StartNew();
        var tasks = new List<Task<AccessToken?>>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks.Add(_tokenManager.GetAccessTokenAsync());
        }

        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        results.Should().HaveCount(concurrentRequests, "All retrievals should complete");
        results.Should().AllSatisfy(token =>
        {
            token.Should().NotBeNull("All requests should get the token");
            token!.Token.Should().Be("concurrent-retrieval-token");
        });

        var averageTimeMs = stopwatch.ElapsedMilliseconds / (double)concurrentRequests;
        averageTimeMs.Should().BeLessThan(5, "Average time per retrieval should be under 5ms");

        _logger.LogInformation(
            "Load test: {Requests} concurrent token retrievals in {Elapsed}ms (avg: {Average}ms)",
            concurrentRequests, stopwatch.ElapsedMilliseconds, averageTimeMs);
    }

    [Fact]
    public async Task LoadTest_TokenRefreshUnderLoad_ShouldHandleGracefully()
    {
        // Arrange
        const int concurrentRequests = 20; // Lower count for refresh operations
        
        var expiredToken = new AccessToken(
            "expired-load-test-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "valid-refresh-token",
            new[] { "read", "write" });

        await _tokenManager.StoreTokenAsync(expiredToken);

        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-load-test-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-refresh-token",
            scope = "read write"
        });

        var stopwatch = Stopwatch.StartNew();
        var tasks = new List<Task<AccessToken?>>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks.Add(_tokenManager.GetAccessTokenAsync());
        }

        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        results.Should().HaveCount(concurrentRequests, "All refresh operations should complete");
        results.Should().AllSatisfy(token =>
        {
            token.Should().NotBeNull("All requests should get refreshed token");
            token!.Token.Should().Be("refreshed-load-test-token");
        });

        var averageTimeMs = stopwatch.ElapsedMilliseconds / (double)concurrentRequests;
        averageTimeMs.Should().BeLessThan(500, "Average time per refresh should be under 500ms");

        _logger.LogInformation(
            "Load test: {Requests} concurrent token refreshes in {Elapsed}ms (avg: {Average}ms)",
            concurrentRequests, stopwatch.ElapsedMilliseconds, averageTimeMs);
    }

    #endregion

    #region Memory Performance Tests

    [Fact]
    public async Task MemoryTest_RepeatedTokenOperations_ShouldNotLeak()
    {
        // Arrange
        const int iterations = 1000;
        var initialMemory = GC.GetTotalMemory(true);

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var token = new AccessToken(
                $"memory-test-token-{i}",
                "Bearer",
                DateTimeOffset.UtcNow.AddHours(1),
                $"refresh-{i}",
                new[] { "read", "write" });

            await _tokenManager.StoreTokenAsync(token);
            await _tokenManager.GetAccessTokenAsync();
            
            if (i % 100 == 0)
            {
                await _tokenManager.ClearTokenAsync();
            }
        }

        // Force garbage collection and measure memory
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        var finalMemory = GC.GetTotalMemory(false);
        var memoryIncrease = finalMemory - initialMemory;

        // Assert
        memoryIncrease.Should().BeLessThan(1024 * 1024, "Memory increase should be less than 1MB");

        _logger.LogInformation(
            "Memory test: {Iterations} operations, memory increase: {MemoryIncrease} bytes",
            iterations, memoryIncrease);
    }

    [Fact]
    public void PerformanceTest_PKCECodeGeneration_ShouldBeFast()
    {
        // Arrange
        const int iterations = 10000;
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var (authUrl, codeVerifier) = _oauthHelper.GenerateAuthorizationUrl($"perf-test-{i}");
            
            // Verify the generated values are valid
            authUrl.Should().NotBeNullOrEmpty();
            codeVerifier.Should().NotBeNullOrEmpty();
        }

        stopwatch.Stop();

        // Assert
        var averageTimeMs = stopwatch.ElapsedMilliseconds / (double)iterations;
        averageTimeMs.Should().BeLessThan(1, "Average PKCE generation should be under 1ms");

        _logger.LogInformation(
            "Performance test: {Iterations} PKCE generations in {Elapsed}ms (avg: {Average}ms)",
            iterations, stopwatch.ElapsedMilliseconds, averageTimeMs);
    }

    #endregion

    #region Stress Tests

    [Fact]
    public async Task StressTest_RapidTokenStorageOperations_ShouldRemainStable()
    {
        // Arrange
        const int rapidOperations = 5000;
        var tasks = new List<Task>();
        var random = new Random();

        // Act - Mix of store, retrieve, and clear operations
        for (int i = 0; i < rapidOperations; i++)
        {
            var operationType = random.Next(3);
            
            switch (operationType)
            {
                case 0: // Store
                    tasks.Add(Task.Run(async () =>
                    {
                        var token = new AccessToken(
                            $"stress-token-{i}",
                            "Bearer",
                            DateTimeOffset.UtcNow.AddHours(1),
                            $"stress-refresh-{i}",
                            new[] { "read" });
                        await _tokenManager.StoreTokenAsync(token);
                    }));
                    break;
                    
                case 1: // Retrieve
                    tasks.Add(Task.Run(async () =>
                    {
                        await _tokenManager.GetAccessTokenAsync();
                    }));
                    break;
                    
                case 2: // Clear (less frequent)
                    if (i % 50 == 0)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            await _tokenManager.ClearTokenAsync();
                        }));
                    }
                    break;
            }
        }

        // Wait for all operations to complete
        await Task.WhenAll(tasks);

        // Assert - No exceptions should have been thrown
        _logger.LogInformation("Stress test: {Operations} rapid token operations completed successfully", rapidOperations);
    }

    [Fact]
    public async Task StressTest_LongRunningTokenRefresh_ShouldMaintainPerformance()
    {
        // Arrange
        const int refreshCycles = 100;
        var refreshTimes = new List<long>();

        // Act
        for (int i = 0; i < refreshCycles; i++)
        {
            var expiredToken = new AccessToken(
                $"long-running-token-{i}",
                "Bearer",
                DateTimeOffset.UtcNow.AddMinutes(-30),
                $"refresh-token-{i}",
                new[] { "read", "write" });

            await _tokenManager.StoreTokenAsync(expiredToken);

            _fixture.MockRefreshTokenResponse(new
            {
                access_token = $"refreshed-token-{i}",
                token_type = "Bearer",
                expires_in = 3600,
                refresh_token = $"new-refresh-{i}",
                scope = "read write"
            });

            var stopwatch = Stopwatch.StartNew();
            await _tokenManager.GetAccessTokenAsync();
            stopwatch.Stop();
            
            refreshTimes.Add(stopwatch.ElapsedMilliseconds);
        }

        // Assert
        var averageRefreshTime = refreshTimes.Average();
        var maxRefreshTime = refreshTimes.Max();
        
        averageRefreshTime.Should().BeLessThan(200, "Average refresh time should remain under 200ms");
        maxRefreshTime.Should().BeLessThan(1000, "Max refresh time should remain under 1000ms");

        _logger.LogInformation(
            "Long-running test: {Cycles} refresh cycles, avg: {Average}ms, max: {Max}ms",
            refreshCycles, averageRefreshTime, maxRefreshTime);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Runs BenchmarkDotNet benchmarks (for manual execution)
    /// </summary>
    [Fact(Skip = "Manual execution only - requires BenchmarkDotNet runner")]
    public void RunBenchmarks()
    {
        var summary = BenchmarkRunner.Run<AuthenticationPerformanceTests>();
        _logger.LogInformation("Benchmark summary: {Summary}", summary);
    }

    #endregion
}