using Procore.SDK.Resilience.Tests.Helpers;

namespace Procore.SDK.Resilience.Tests.Polly;

/// <summary>
/// Tests for circuit breaker patterns including state transitions and recovery behaviors.
/// Validates circuit breaker transitions: Closed → Open → Half-Open → Closed/Open.
/// </summary>
public class CircuitBreakerTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<CircuitBreakerTests> _logger;

    public CircuitBreakerTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<CircuitBreakerTests>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CircuitBreaker_Should_Transition_From_Closed_To_Open_After_Consecutive_Failures()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Service unavailable"));

        var circuitBreaker = Policy
            .HandleResult<HttpResponseMessage>(r => false) // Will not handle successful responses
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (exception, duration) =>
                {
                    circuitState = CircuitBreakerState.Open;
                    _logger.LogWarning("Circuit breaker opened for {Duration}", duration);
                },
                onReset: () =>
                {
                    circuitState = CircuitBreakerState.Closed;
                    _logger.LogInformation("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    circuitState = CircuitBreakerState.HalfOpen;
                    _logger.LogInformation("Circuit breaker half-open");
                });

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

        // Act & Assert - Closed state, failures accumulate
        for (int i = 1; i <= 3; i++)
        {
            circuitState.Should().Be(CircuitBreakerState.Closed);
            
            await client.Invoking(async c => 
                await c.GetAsync("/rest/v1.0/companies"))
                .Should().ThrowAsync<HttpRequestException>();
                
            mockHandler.SentRequests.Should().HaveCount(i);
        }

        // Circuit should now be open
        circuitState.Should().Be(CircuitBreakerState.Open);

        // Subsequent calls should fail fast without hitting the service
        await client.Invoking(async c => 
            await c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<CircuitBreakerOpenException>();
            
        mockHandler.SentRequests.Should().HaveCount(3); // No additional calls made

        // Verify logging
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Warning && entry.Message.Contains("Circuit breaker opened"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CircuitBreaker_Should_Transition_To_HalfOpen_After_Break_Duration()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var breakDuration = TimeSpan.FromMilliseconds(100);
        
        var callCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            Interlocked.Increment(ref callCount);
            if (callCount == 1)
                throw new HttpRequestException("Initial failure");
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var circuitBreaker = Policy
            .HandleResult<HttpResponseMessage>(r => false)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: breakDuration,
                onBreak: (exception, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

        // Act - Trigger circuit breaker
        await client.Invoking(async c => 
            await c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        circuitState.Should().Be(CircuitBreakerState.Open);

        // Wait for break duration
        await Task.Delay(breakDuration.Add(TimeSpan.FromMilliseconds(50)));

        // Next call should transition to half-open and succeed
        var result = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        circuitState.Should().Be(CircuitBreakerState.Closed); // Reset after successful call
        callCount.Should().Be(2);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CircuitBreaker_Should_Reopen_If_HalfOpen_Call_Fails()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var breakDuration = TimeSpan.FromMilliseconds(100);
        
        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Persistent failure"));

        var circuitBreaker = Policy
            .HandleResult<HttpResponseMessage>(r => false)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: breakDuration,
                onBreak: (exception, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

        // Act - Trigger circuit breaker
        await client.Invoking(async c => 
            await c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        circuitState.Should().Be(CircuitBreakerState.Open);

        // Wait for break duration
        await Task.Delay(breakDuration.Add(TimeSpan.FromMilliseconds(50)));

        // Next call should fail and reopen circuit
        await client.Invoking(async c => 
            await c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Assert
        circuitState.Should().Be(CircuitBreakerState.Open); // Should be open again
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CircuitBreaker_Should_Reset_After_Successful_HalfOpen_Call()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var breakDuration = TimeSpan.FromMilliseconds(50);
        var stateTransitions = new List<string>();
        
        var callCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var currentCall = Interlocked.Increment(ref callCount);
            
            // Fail first call to trigger circuit breaker, succeed on subsequent calls
            if (currentCall == 1)
                throw new HttpRequestException("Initial failure");
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var circuitBreaker = Policy
            .HandleResult<HttpResponseMessage>(r => false)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: breakDuration,
                onBreak: (exception, duration) =>
                {
                    circuitState = CircuitBreakerState.Open;
                    stateTransitions.Add("Open");
                },
                onReset: () =>
                {
                    circuitState = CircuitBreakerState.Closed;
                    stateTransitions.Add("Closed");
                },
                onHalfOpen: () =>
                {
                    circuitState = CircuitBreakerState.HalfOpen;
                    stateTransitions.Add("HalfOpen");
                });

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

        // Act
        // 1. Trigger circuit breaker
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // 2. Wait for break duration
        await Task.Delay(breakDuration.Add(TimeSpan.FromMilliseconds(25)));

        // 3. Make successful call to reset circuit
        var response = await client.GetAsync("/rest/v1.0/companies");

        // 4. Make another call to verify circuit stays closed
        var response2 = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);
        circuitState.Should().Be(CircuitBreakerState.Closed);
        
        stateTransitions.Should().ContainInOrder("Open", "HalfOpen", "Closed");
        callCount.Should().Be(3); // Initial failure + half-open success + subsequent success
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CircuitBreaker_Should_Handle_Multiple_Concurrent_Requests_When_Open()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Service failure"));

        var circuitBreaker = Policy
            .HandleResult<HttpResponseMessage>(r => false)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: TimeSpan.FromSeconds(1),
                onBreak: (exception, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

        // Act - Trigger circuit breaker
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        circuitState.Should().Be(CircuitBreakerState.Open);

        // Make multiple concurrent requests while circuit is open
        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
                .Should().ThrowAsync<CircuitBreakerOpenException>();
        });

        await Task.WhenAll(tasks);

        // Assert
        mockHandler.SentRequests.Should().HaveCount(1); // Only the initial request that triggered the circuit breaker
        circuitState.Should().Be(CircuitBreakerState.Open);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CircuitBreaker_Should_Track_Success_Rate_Correctly()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var successCount = 0;
        var failureCount = 0;
        
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var total = successCount + failureCount;
            
            // Alternate between success and failure
            if (total % 2 == 0)
            {
                Interlocked.Increment(ref successCount);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                Interlocked.Increment(ref failureCount);
                throw new HttpRequestException("Intermittent failure");
            }
        });

        var circuitBreaker = Policy
            .HandleResult<HttpResponseMessage>(r => false)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3, // Allow 3 failures before breaking
                durationOfBreak: TimeSpan.FromMilliseconds(100),
                onBreak: (exception, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

        // Act - Make alternating successful/failed requests
        var responses = new List<HttpResponseMessage?>();
        var exceptions = new List<Exception>();

        for (int i = 0; i < 8; i++)
        {
            try
            {
                var response = await client.GetAsync("/rest/v1.0/companies");
                responses.Add(response);
            }
            catch (Exception ex)
            {
                responses.Add(null);
                exceptions.Add(ex);
            }
        }

        // Assert
        // Should have made successful requests and failed requests before circuit opens
        successCount.Should().BeGreaterThan(0);
        failureCount.Should().BeGreaterOrEqualTo(3); // At least 3 failures to trigger circuit breaker
        
        // Circuit should eventually be open due to failures
        exceptions.Should().Contain(ex => ex is CircuitBreakerOpenException);
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task CircuitBreaker_Should_Have_Minimal_Overhead_When_Closed()
    {
        // Arrange
        const int requestCount = 100;
        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var circuitBreaker = PolicyFactory.CreateCircuitBreakerPolicy(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            logger: _logger);
            
        var clientWithCircuitBreaker = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);
        var clientWithoutCircuitBreaker = PolicyFactory.CreateHttpClient(
            new TestHttpMessageHandler(request => new HttpResponseMessage(HttpStatusCode.OK)));

        // Act
        var withCircuitBreakerTimes = new List<long>();
        var withoutCircuitBreakerTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            var sw1 = Stopwatch.StartNew();
            await clientWithCircuitBreaker.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withCircuitBreakerTimes.Add(sw1.ElapsedMilliseconds);

            var sw2 = Stopwatch.StartNew();
            await clientWithoutCircuitBreaker.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutCircuitBreakerTimes.Add(sw2.ElapsedMilliseconds);
        }

        // Assert
        var avgWithCircuitBreaker = withCircuitBreakerTimes.Average();
        var avgWithoutCircuitBreaker = withoutCircuitBreakerTimes.Average();
        var overhead = avgWithoutCircuitBreaker > 0 ? 
            (avgWithCircuitBreaker - avgWithoutCircuitBreaker) / avgWithoutCircuitBreaker * 100 : 0;

        overhead.Should().BeLessThan(10); // Less than 10% overhead
        _logger.LogInformation("Circuit breaker overhead: {Overhead:F2}%", overhead);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task CircuitBreaker_Should_Handle_Realistic_Failure_Patterns()
    {
        // Arrange - Simulate realistic service degradation patterns
        var failurePatterns = new[]
        {
            "Cascading failures",
            "Database connection timeouts", 
            "Memory pressure",
            "Network partitions"
        };

        foreach (var pattern in failurePatterns)
        {
            _loggerProvider.ClearLogs();
            var circuitState = CircuitBreakerState.Closed;
            var stateHistory = new List<CircuitBreakerState>();
            
            // Simulate degrading service - increasing failure rate
            var requestCount = 0;
            var mockHandler = new TestHttpMessageHandler(request =>
            {
                var count = Interlocked.Increment(ref requestCount);
                
                // Gradually increase failure rate: 0%, 25%, 50%, 75%, 100%
                var failureThreshold = Math.Min(100, (count - 1) * 25);
                var shouldFail = (count * 17) % 100 < failureThreshold; // Pseudo-random but deterministic
                
                if (shouldFail)
                    throw new HttpRequestException($"Service degradation: {pattern}");
                    
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

            var circuitBreaker = Policy
                .HandleResult<HttpResponseMessage>(r => false)
                .Or<HttpRequestException>()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMilliseconds(50),
                    onBreak: (ex, duration) =>
                    {
                        circuitState = CircuitBreakerState.Open;
                        stateHistory.Add(CircuitBreakerState.Open);
                    },
                    onReset: () =>
                    {
                        circuitState = CircuitBreakerState.Closed;
                        stateHistory.Add(CircuitBreakerState.Closed);
                    },
                    onHalfOpen: () =>
                    {
                        circuitState = CircuitBreakerState.HalfOpen;
                        stateHistory.Add(CircuitBreakerState.HalfOpen);
                    });

            var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreaker);

            // Act - Make requests until circuit opens
            var successCount = 0;
            var failureCount = 0;
            var circuitOpenCount = 0;

            for (int i = 0; i < 20 && circuitOpenCount < 5; i++)
            {
                try
                {
                    var response = await client.GetAsync("/rest/v1.0/companies");
                    if (response.IsSuccessStatusCode) successCount++;
                }
                catch (HttpRequestException)
                {
                    failureCount++;
                }
                catch (CircuitBreakerOpenException)
                {
                    circuitOpenCount++;
                }
                
                await Task.Delay(10); // Small delay between requests
            }

            // Assert - Circuit breaker should have activated for this failure pattern
            stateHistory.Should().Contain(CircuitBreakerState.Open, 
                $"circuit breaker should activate for {pattern}");
                
            (successCount + failureCount).Should().BeGreaterThan(0, 
                $"should have made some requests for {pattern}");
        }
    }
}