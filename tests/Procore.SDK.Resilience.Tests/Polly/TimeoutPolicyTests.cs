using Procore.SDK.Resilience.Tests.Helpers;

namespace Procore.SDK.Resilience.Tests.Polly;

/// <summary>
/// Tests for timeout policies covering request cancellation, timeout handling, and integration with other policies.
/// </summary>
public class TimeoutPolicyTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<TimeoutPolicyTests> _logger;

    public TimeoutPolicyTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<TimeoutPolicyTests>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Cancel_Long_Running_Requests()
    {
        // Arrange
        var timeoutDuration = TimeSpan.FromMilliseconds(500);
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(timeoutDuration, _logger);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(1000); // Longer than timeout
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

        // Act & Assert
        var stopwatch = Stopwatch.StartNew();
        
        await client.Invoking(async c =>
            await c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();
            
        stopwatch.Stop();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(timeoutDuration.TotalMilliseconds + 200);

        // Verify timeout was logged
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Warning && entry.Message.Contains("Request timed out"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Allow_Fast_Requests()
    {
        // Arrange
        var timeoutDuration = TimeSpan.FromSeconds(2);
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(timeoutDuration, _logger);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(100); // Faster than timeout
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": true}")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("success");

        // Should not have timeout warnings
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).WithMessage("timed out").Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Respect_CancellationToken()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromSeconds(10), _logger);
        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            // Simulate a request that could be cancelled
            await Task.Delay(1000, TestHttpMessageHandler.GetCancellationToken(request));
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

        // Act & Assert
        var stopwatch = Stopwatch.StartNew();
        
        await client.Invoking(async c =>
            await c.GetAsync("/rest/v1.0/companies", cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
            
        stopwatch.Stop();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(500);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Handle_Different_Timeout_Durations()
    {
        // Arrange
        var timeoutDurations = new[]
        {
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2)
        };

        foreach (var timeout in timeoutDurations)
        {
            var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(timeout, _logger);
            var requestDuration = timeout.Add(TimeSpan.FromMilliseconds(100)); // Slightly longer than timeout
            
            var mockHandler = new TestHttpMessageHandler(async request =>
            {
                await Task.Delay(requestDuration);
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

            var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

            // Act & Assert
            var stopwatch = Stopwatch.StartNew();
            
            await client.Invoking(async c =>
                await c.GetAsync("/rest/v1.0/companies"))
                .Should().ThrowAsync<TimeoutRejectedException>(
                    $"request should timeout with {timeout.TotalMilliseconds}ms timeout");
                    
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(timeout.TotalMilliseconds + 200,
                $"actual timeout should be close to configured {timeout.TotalMilliseconds}ms");
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Work_With_Retry_Policy()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(200), _logger);
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, baseDelay: TimeSpan.FromMilliseconds(50), logger: _logger);
        var combinedPolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy);
        
        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            var count = Interlocked.Increment(ref requestCount);
            
            if (count <= 2)
            {
                // First two requests timeout
                await Task.Delay(500);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            
            // Third request succeeds quickly
            await Task.Delay(50);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(3); // 2 timeouts + 1 success
        
        // Should have taken time for 2 timeouts plus 2 retry delays plus final successful request
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(500); // 2 × 200ms timeout + 2 × 50ms delay + execution time

        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).WithMessage("Retry attempt").Should().HaveCount(2);
        logEntries.WithLevel(LogLevel.Warning).WithMessage("timed out").Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Work_With_Circuit_Breaker()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(100), _logger);
        var circuitBreakerPolicy = PolicyFactory.CreateCircuitBreakerPolicy(
            handledEventsAllowedBeforeBreaking: 2,
            durationOfBreak: TimeSpan.FromMilliseconds(500),
            logger: _logger);
        var combinedPolicy = Policy.WrapAsync(circuitBreakerPolicy, timeoutPolicy);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(300); // Always times out
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act - First two requests should timeout and trigger circuit breaker
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();
            
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();

        // Third request should hit open circuit breaker
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<CircuitBreakerOpenException>();

        // Assert
        mockHandler.SentRequests.Should().HaveCount(2); // Circuit breaker prevents third request
        
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Error && entry.Message.Contains("Circuit breaker opened"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Handle_Concurrent_Requests_Independently()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(300), _logger);
        
        var requestCounter = 0;
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            var requestId = Interlocked.Increment(ref requestCounter);
            
            // Alternate between fast and slow requests
            var delay = requestId % 2 == 0 ? 100 : 500; // Even requests fast, odd requests slow
            await Task.Delay(delay);
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"{{\"requestId\": {requestId}}}")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

        // Act - Make concurrent requests
        var tasks = Enumerable.Range(1, 6).Select(async i =>
        {
            try
            {
                var response = await client.GetAsync($"/rest/v1.0/companies/{i}");
                var content = await response.Content.ReadAsStringAsync();
                return (Success: true, Content: content, Exception: (Exception?)null);
            }
            catch (TimeoutRejectedException ex)
            {
                return (Success: false, Content: (string?)null, Exception: ex);
            }
        });

        var results = await Task.WhenAll(tasks);

        // Assert
        var successfulRequests = results.Where(r => r.Success).ToList();
        var timeoutRequests = results.Where(r => !r.Success).ToList();

        successfulRequests.Should().HaveCount(3); // Even-numbered requests (fast)
        timeoutRequests.Should().HaveCount(3);    // Odd-numbered requests (slow)
        
        // All successful requests should have content
        successfulRequests.Should().AllSatisfy(r => r.Content.Should().NotBeNullOrEmpty());
        
        // All timeout requests should have TimeoutRejectedException
        timeoutRequests.Should().AllSatisfy(r => r.Exception.Should().BeOfType<TimeoutRejectedException>());
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TimeoutPolicy_Should_Clean_Up_Resources_On_Timeout()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(100), _logger);
        var disposedResources = new ConcurrentBag<string>();
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            var resource = $"Resource-{Guid.NewGuid():N}";
            
            try
            {
                await Task.Delay(500, TestHttpMessageHandler.GetCancellationToken(request));
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (OperationCanceledException)
            {
                disposedResources.Add(resource);
                throw;
            }
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

        // Act
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();

        // Give some time for cleanup
        await Task.Delay(50);

        // Assert
        disposedResources.Should().HaveCount(1);
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task TimeoutPolicy_Should_Have_Accurate_Timeout_Precision()
    {
        // Arrange
        var timeoutDurations = new[]
        {
            TimeSpan.FromMilliseconds(50),
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromMilliseconds(500)
        };

        foreach (var expectedTimeout in timeoutDurations)
        {
            var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(expectedTimeout, _logger);
            
            var mockHandler = new TestHttpMessageHandler(async request =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10)); // Much longer than any timeout
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

            var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

            // Act
            var stopwatch = Stopwatch.StartNew();
            
            await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
                .Should().ThrowAsync<TimeoutRejectedException>();
                
            stopwatch.Stop();

            // Assert - Timeout should be accurate within reasonable bounds
            var actualTimeout = stopwatch.Elapsed;
            var toleranceMs = 100; // 100ms tolerance for timing precision
            
            actualTimeout.Should().BeCloseTo(expectedTimeout, TimeSpan.FromMilliseconds(toleranceMs),
                $"timeout of {expectedTimeout.TotalMilliseconds}ms should be accurate within {toleranceMs}ms");
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TimeoutPolicy_Should_Handle_Realistic_Network_Delays()
    {
        // Arrange - Simulate various network conditions
        var networkConditions = new[]
        {
            (Name: "Fast Connection", Delay: TimeSpan.FromMilliseconds(50), ShouldTimeout: false),
            (Name: "Normal Connection", Delay: TimeSpan.FromMilliseconds(200), ShouldTimeout: false),
            (Name: "Slow Connection", Delay: TimeSpan.FromMilliseconds(400), ShouldTimeout: true),
            (Name: "Very Slow Connection", Delay: TimeSpan.FromMilliseconds(800), ShouldTimeout: true)
        };

        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(300), _logger);

        foreach (var condition in networkConditions)
        {
            _loggerProvider.ClearLogs();
            
            var mockHandler = new TestHttpMessageHandler(async request =>
            {
                await Task.Delay(condition.Delay);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"condition\": \"{condition.Name}\"}}")
                };
            });

            var client = PolicyFactory.CreateHttpClient(mockHandler, timeoutPolicy);

            // Act & Assert
            if (condition.ShouldTimeout)
            {
                await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
                    .Should().ThrowAsync<TimeoutRejectedException>(
                        $"{condition.Name} should timeout with {condition.Delay.TotalMilliseconds}ms delay");
                        
                var logEntries = _loggerProvider.GetLogEntries();
                logEntries.Should().Contain(entry => 
                    entry.Level == LogLevel.Warning && entry.Message.Contains("timed out"));
            }
            else
            {
                var response = await client.GetAsync("/rest/v1.0/companies");
                response.StatusCode.Should().Be(HttpStatusCode.OK,
                    $"{condition.Name} should succeed with {condition.Delay.TotalMilliseconds}ms delay");
                    
                var content = await response.Content.ReadAsStringAsync();
                content.Should().Contain(condition.Name);
            }
        }
    }
}