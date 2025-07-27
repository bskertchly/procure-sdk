using Procore.SDK.Resilience.Tests.Helpers;

namespace Procore.SDK.Resilience.Tests.Performance;

/// <summary>
/// Performance tests to ensure resilience patterns don't significantly impact performance during normal operations.
/// Measures overhead of retry policies, circuit breakers, timeouts, and combined policies.
/// </summary>
public class ResilienceOverheadTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<ResilienceOverheadTests> _logger;

    public ResilienceOverheadTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<ResilienceOverheadTests>();
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Retry_Policy_Should_Have_Minimal_Overhead_For_Successful_Requests()
    {
        // Arrange
        const int requestCount = 200;
        const int warmupRequests = 50;
        
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 3, logger: _logger);

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": true}")
            });

        var clientWithRetry = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);
        var clientWithoutRetry = PolicyFactory.CreateHttpClient(
            new TestHttpMessageHandler(request => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": true}")
            }));

        // Warmup
        for (int i = 0; i < warmupRequests; i++)
        {
            await clientWithRetry.GetAsync("/rest/v1.0/companies");
            await clientWithoutRetry.GetAsync("/rest/v1.0/companies");
        }

        // Act - Measure performance
        var withRetryTimes = new List<long>();
        var withoutRetryTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            // Test with retry policy
            var sw1 = Stopwatch.StartNew();
            var response1 = await clientWithRetry.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withRetryTimes.Add(sw1.ElapsedTicks);
            response1.Dispose();

            // Test without retry policy
            var sw2 = Stopwatch.StartNew();
            var response2 = await clientWithoutRetry.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutRetryTimes.Add(sw2.ElapsedTicks);
            response2.Dispose();
        }

        // Assert
        var avgWithRetry = withRetryTimes.Average();
        var avgWithoutRetry = withoutRetryTimes.Average();
        var overhead = avgWithoutRetry > 0 ? (avgWithRetry - avgWithoutRetry) / avgWithoutRetry * 100 : 0;
        
        var medianWithRetry = GetMedian(withRetryTimes);
        var medianWithoutRetry = GetMedian(withoutRetryTimes);
        var medianOverhead = medianWithoutRetry > 0 ? (medianWithRetry - medianWithoutRetry) / medianWithoutRetry * 100 : 0;

        // Performance assertions
        overhead.Should().BeLessThan(15, "average overhead should be less than 15%");
        medianOverhead.Should().BeLessThan(10, "median overhead should be less than 10%");
        
        _logger.LogInformation("Retry policy overhead - Average: {AvgOverhead:F2}%, Median: {MedianOverhead:F2}%", 
            overhead, medianOverhead);
        
        // Verify no performance warnings were logged
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Circuit_Breaker_Should_Have_Minimal_Overhead_When_Closed()
    {
        // Arrange
        const int requestCount = 200;
        const int warmupRequests = 50;
        
        var circuitBreaker = PolicyFactory.CreateCircuitBreakerPolicy(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            logger: _logger);

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\": \"test\"}")
            });

        var clientWithCircuitBreaker = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);
        var clientWithoutCircuitBreaker = PolicyFactory.CreateHttpClient(
            new TestHttpMessageHandler(request => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\": \"test\"}")
            }));

        // Warmup
        for (int i = 0; i < warmupRequests; i++)
        {
            await clientWithCircuitBreaker.GetAsync("/rest/v1.0/companies");
            await clientWithoutCircuitBreaker.GetAsync("/rest/v1.0/companies");
        }

        // Act
        var withCircuitBreakerTimes = new List<long>();
        var withoutCircuitBreakerTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            var sw1 = Stopwatch.StartNew();
            var response1 = await clientWithCircuitBreaker.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withCircuitBreakerTimes.Add(sw1.ElapsedTicks);
            response1.Dispose();

            var sw2 = Stopwatch.StartNew();
            var response2 = await clientWithoutCircuitBreaker.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutCircuitBreakerTimes.Add(sw2.ElapsedTicks);
            response2.Dispose();
        }

        // Assert
        var avgWithCircuitBreaker = withCircuitBreakerTimes.Average();
        var avgWithoutCircuitBreaker = withoutCircuitBreakerTimes.Average();
        var overhead = avgWithoutCircuitBreaker > 0 ? 
            (avgWithCircuitBreaker - avgWithoutCircuitBreaker) / avgWithoutCircuitBreaker * 100 : 0;

        var p95WithCircuitBreaker = GetPercentile(withCircuitBreakerTimes, 0.95);
        var p95WithoutCircuitBreaker = GetPercentile(withoutCircuitBreakerTimes, 0.95);
        var p95Overhead = p95WithoutCircuitBreaker > 0 ? 
            (p95WithCircuitBreaker - p95WithoutCircuitBreaker) / p95WithoutCircuitBreaker * 100 : 0;

        overhead.Should().BeLessThan(8, "average overhead should be less than 8%");
        p95Overhead.Should().BeLessThan(15, "95th percentile overhead should be less than 15%");
        
        _logger.LogInformation("Circuit breaker overhead - Average: {AvgOverhead:F2}%, P95: {P95Overhead:F2}%", 
            overhead, p95Overhead);
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Timeout_Policy_Should_Have_Minimal_Overhead_For_Fast_Requests()
    {
        // Arrange
        const int requestCount = 200;
        const int warmupRequests = 50;
        
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromSeconds(30), _logger);

        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(10); // Simulate small processing time
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"result\": \"success\"}")
            };
        });

        var clientWithTimeout = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);
        var clientWithoutTimeout = PolicyFactory.CreateHttpClient(
            new TestHttpMessageHandler(async request =>
            {
                await Task.Delay(10); // Same processing time
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"result\": \"success\"}")
                };
            }));

        // Warmup
        for (int i = 0; i < warmupRequests; i++)
        {
            await clientWithTimeout.GetAsync("/rest/v1.0/companies");
            await clientWithoutTimeout.GetAsync("/rest/v1.0/companies");
        }

        // Act
        var withTimeoutTimes = new List<long>();
        var withoutTimeoutTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            var sw1 = Stopwatch.StartNew();
            var response1 = await clientWithTimeout.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withTimeoutTimes.Add(sw1.ElapsedTicks);
            response1.Dispose();

            var sw2 = Stopwatch.StartNew();
            var response2 = await clientWithoutTimeout.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutTimeoutTimes.Add(sw2.ElapsedTicks);
            response2.Dispose();
        }

        // Assert
        var avgWithTimeout = withTimeoutTimes.Average();
        var avgWithoutTimeout = withoutTimeoutTimes.Average();
        var overhead = avgWithoutTimeout > 0 ? (avgWithTimeout - avgWithoutTimeout) / avgWithoutTimeout * 100 : 0;

        overhead.Should().BeLessThan(12, "timeout policy should add less than 12% overhead");
        
        // Verify all requests completed successfully (no timeouts)
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).WithMessage("timed out").Should().BeEmpty();
        
        _logger.LogInformation("Timeout policy overhead: {Overhead:F2}%", overhead);
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Combined_Policies_Should_Have_Acceptable_Overhead()
    {
        // Arrange
        const int requestCount = 150;
        const int warmupRequests = 30;
        
        var combinedPolicy = PolicyFactory.CreateCombinedPolicy(
            retryCount: 3,
            retryBaseDelay: TimeSpan.FromMilliseconds(100),
            circuitBreakerFailureThreshold: 5,
            circuitBreakerDuration: TimeSpan.FromSeconds(30),
            timeout: TimeSpan.FromSeconds(10),
            logger: _logger);

        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(5); // Small processing delay
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"message\": \"processed\"}")
            };
        });

        var clientWithPolicies = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);
        var clientWithoutPolicies = PolicyFactory.CreateHttpClient(
            new TestHttpMessageHandler(async request =>
            {
                await Task.Delay(5); // Same processing delay
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"message\": \"processed\"}")
                };
            }));

        // Warmup
        for (int i = 0; i < warmupRequests; i++)
        {
            await clientWithPolicies.GetAsync("/rest/v1.0/companies");
            await clientWithoutPolicies.GetAsync("/rest/v1.0/companies");
        }

        // Act
        var withPoliciesTimes = new List<long>();
        var withoutPoliciesTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            var sw1 = Stopwatch.StartNew();
            var response1 = await clientWithPolicies.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withPoliciesTimes.Add(sw1.ElapsedTicks);
            response1.Dispose();

            var sw2 = Stopwatch.StartNew();
            var response2 = await clientWithoutPolicies.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutPoliciesTimes.Add(sw2.ElapsedTicks);
            response2.Dispose();
        }

        // Assert
        var avgWithPolicies = withPoliciesTimes.Average();
        var avgWithoutPolicies = withoutPoliciesTimes.Average();
        var overhead = avgWithoutPolicies > 0 ? 
            (avgWithPolicies - avgWithoutPolicies) / avgWithoutPolicies * 100 : 0;

        var maxWithPolicies = withPoliciesTimes.Max();
        var maxWithoutPolicies = withoutPoliciesTimes.Max();
        
        overhead.Should().BeLessThan(25, "combined policies should add less than 25% overhead");
        
        // Ensure we're not degrading significantly
        var avgDurationMs = TimeSpan.FromTicks((long)avgWithPolicies).TotalMilliseconds;
        avgDurationMs.Should().BeLessThan(100, "average request time should still be reasonable");
        
        _logger.LogInformation("Combined policies overhead: {Overhead:F2}%, Avg duration: {AvgDurationMs:F1}ms", 
            overhead, avgDurationMs);
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Memory_Usage_Should_Not_Increase_Significantly_With_Resilience_Patterns()
    {
        // Arrange
        const int iterationCount = 1000;
        const int measurementInterval = 100;
        
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, logger: _logger);
        var circuitBreakerPolicy = PolicyFactory.CreateCircuitBreakerPolicy(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            logger: _logger);
        var combinedPolicy = Policy.WrapAsync(circuitBreakerPolicy, retryPolicy);

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\": 12345}")
            });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act
        var memoryMeasurements = new List<long>();
        var initialMemory = GC.GetTotalMemory(true);
        memoryMeasurements.Add(initialMemory);

        for (int i = 0; i < iterationCount; i++)
        {
            var response = await client.GetAsync("/rest/v1.0/companies");
            await response.Content.ReadAsStringAsync();
            response.Dispose();

            if (i % measurementInterval == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                
                var currentMemory = GC.GetTotalMemory(false);
                memoryMeasurements.Add(currentMemory);
            }
        }

        var finalMemory = GC.GetTotalMemory(true);
        memoryMeasurements.Add(finalMemory);

        // Assert
        var memoryIncrease = finalMemory - initialMemory;
        var memoryIncreasePerRequest = memoryIncrease / (double)iterationCount;

        memoryIncreasePerRequest.Should().BeLessThan(2048, "memory increase per request should be less than 2KB");
        
        // Check for memory leaks - final memory shouldn't be much higher than initial
        var memoryGrowthRatio = (double)finalMemory / initialMemory;
        memoryGrowthRatio.Should().BeLessThan(1.5, "total memory growth should be less than 50%");
        
        _logger.LogInformation(
            "Memory usage - Initial: {InitialKB}KB, Final: {FinalKB}KB, Increase per request: {IncreasePerRequest:F1} bytes",
            initialMemory / 1024, 
            finalMemory / 1024, 
            memoryIncreasePerRequest);

        // Verify no memory-related warnings
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Concurrent_Requests_Should_Not_Degrade_Performance_Significantly()
    {
        // Arrange
        const int concurrentRequests = 50;
        const int requestsPerClient = 20;
        
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, logger: _logger);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(Random.Shared.Next(5, 15)); // Simulate variable processing time
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"{{\"timestamp\": \"{DateTimeOffset.UtcNow:O}\"}}")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act - Sequential baseline
        var sequentialTimes = new List<long>();
        for (int i = 0; i < concurrentRequests; i++)
        {
            var sw = Stopwatch.StartNew();
            var response = await client.GetAsync("/rest/v1.0/companies");
            sw.Stop();
            sequentialTimes.Add(sw.ElapsedTicks);
            response.Dispose();
        }

        // Act - Concurrent execution
        var concurrentTimes = new ConcurrentBag<long>();
        var concurrentTasks = Enumerable.Range(0, concurrentRequests).Select(async _ =>
        {
            var sw = Stopwatch.StartNew();
            var response = await client.GetAsync("/rest/v1.0/companies");
            sw.Stop();
            concurrentTimes.Add(sw.ElapsedTicks);
            response.Dispose();
        });

        var overallStopwatch = Stopwatch.StartNew();
        await Task.WhenAll(concurrentTasks);
        overallStopwatch.Stop();

        // Assert
        var avgSequential = sequentialTimes.Average();
        var avgConcurrent = concurrentTimes.Average();
        var concurrentOverhead = (avgConcurrent - avgSequential) / avgSequential * 100;

        // Concurrent requests shouldn't be significantly slower per request
        concurrentOverhead.Should().BeLessThan(50, "concurrent execution shouldn't add more than 50% overhead per request");
        
        // Total concurrent execution should be much faster than sequential
        var totalSequentialTime = sequentialTimes.Sum();
        var totalConcurrentTime = overallStopwatch.ElapsedTicks;
        var concurrentSpeedup = (double)totalSequentialTime / totalConcurrentTime;
        
        concurrentSpeedup.Should().BeGreaterThan(5, "concurrent execution should be at least 5x faster than sequential");
        
        _logger.LogInformation(
            "Concurrent performance - Sequential avg: {SeqMs:F1}ms, Concurrent avg: {ConcMs:F1}ms, Speedup: {Speedup:F1}x",
            TimeSpan.FromTicks((long)avgSequential).TotalMilliseconds,
            TimeSpan.FromTicks((long)avgConcurrent).TotalMilliseconds,
            concurrentSpeedup);
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task Policy_Evaluation_Should_Be_Fast_For_High_Frequency_Operations()
    {
        // Arrange
        const int operationCount = 10000;
        
        var policy = PolicyFactory.CreateRetryPolicy(retryCount: 1, baseDelay: TimeSpan.Zero, logger: _logger);
        
        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var client = PolicyFactory.CreateHttpClient(mockHandler, policy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < operationCount; i++)
        {
            var response = await client.GetAsync("/rest/v1.0/test");
            response.Dispose();
        }
        
        stopwatch.Stop();

        // Assert
        var avgOperationTime = stopwatch.Elapsed.TotalMicroseconds / operationCount;
        var operationsPerSecond = operationCount / stopwatch.Elapsed.TotalSeconds;
        
        avgOperationTime.Should().BeLessThan(1000, "average operation time should be less than 1ms");
        operationsPerSecond.Should().BeGreaterThan(1000, "should handle more than 1000 operations per second");
        
        _logger.LogInformation(
            "High frequency performance - {OperationsPerSec:F0} ops/sec, {AvgMicroseconds:F1} μs per operation",
            operationsPerSecond, avgOperationTime);
    }

    #region Helper Methods

    private static double GetMedian(IList<long> values)
    {
        var sorted = values.OrderBy(x => x).ToArray();
        var mid = sorted.Length / 2;
        
        return sorted.Length % 2 == 0 
            ? (sorted[mid - 1] + sorted[mid]) / 2.0
            : sorted[mid];
    }

    private static double GetPercentile(IList<long> values, double percentile)
    {
        var sorted = values.OrderBy(x => x).ToArray();
        var index = (int)Math.Ceiling(percentile * sorted.Length) - 1;
        return sorted[Math.Max(0, Math.Min(index, sorted.Length - 1))];
    }

    #endregion
}