using Procore.SDK.Resilience.Tests.Helpers;

namespace Procore.SDK.Resilience.Tests.Polly;

/// <summary>
/// Tests for combining multiple policies (retry, circuit breaker, timeout) to validate their interaction
/// and ensure they work together effectively for comprehensive resilience.
/// </summary>
public class PolicyWrapTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<PolicyWrapTests> _logger;

    public PolicyWrapTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<PolicyWrapTests>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CombinedPolicies_Should_Apply_Timeout_Then_Retry_Then_CircuitBreaker()
    {
        // Arrange
        var executionResults = new List<string>();
        
        var timeoutPolicy = Policy
            .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(100));
            
        var retryPolicy = Policy
            .Handle<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(50),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    executionResults.Add($"Retry {retryCount}");
                    _logger.LogWarning("Retry attempt {RetryCount} after timeout", retryCount);
                });
                
        var circuitBreakerPolicy = Policy
            .Handle<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(1),
                onBreak: (exception, duration) => 
                {
                    executionResults.Add("Circuit Opened");
                    _logger.LogError("Circuit breaker opened due to timeouts");
                },
                onReset: () => 
                {
                    executionResults.Add("Circuit Reset");
                    _logger.LogInformation("Circuit breaker reset");
                });

        var combinedPolicy = Policy.WrapAsync(circuitBreakerPolicy, retryPolicy, timeoutPolicy);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            executionResults.Add("Request Started");
            await Task.Delay(200); // Will timeout
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act
        await client.Invoking(async c =>
        {
            executionResults.Add("Policy Execution");
            return await c.GetAsync("/rest/v1.0/companies");
        })
        .Should().ThrowAsync<TimeoutRejectedException>();

        // Assert
        executionResults.Should().Contain("Policy Execution");
        executionResults.Should().Contain("Request Started");
        executionResults.Should().Contain("Retry 1");
        executionResults.Should().Contain("Retry 2");
        executionResults.Count(r => r == "Request Started").Should().Be(3); // Initial + 2 retries

        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).Should().HaveCount(2); // Two retry attempts
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CombinedPolicies_Should_Handle_Rate_Limiting_With_Exponential_Backoff()
    {
        // Arrange
        var requestAttempts = new List<DateTimeOffset>();
        var rateLimitPolicy = PolicyFactory.CreateRateLimitPolicy(_logger);
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromSeconds(2), _logger);
        var combinedPolicy = Policy.WrapAsync(rateLimitPolicy, timeoutPolicy);
        
        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestAttempts.Add(DateTimeOffset.UtcNow);
            var count = Interlocked.Increment(ref requestCount);
            
            return count switch
            {
                1 => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    Headers = { RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMilliseconds(100)) }
                },
                2 => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    Headers = { RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMilliseconds(200)) }
                },
                3 => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"success\": true}")
                },
                _ => throw new InvalidOperationException("Unexpected request")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(3);
        requestAttempts.Should().HaveCount(3);
        
        // Should respect retry-after headers
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(300); // At least 100 + 200ms delays
        
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => entry.Message.Contains("Rate limit exceeded"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CombinedPolicies_Should_Short_Circuit_When_Circuit_Breaker_Opens()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(100), _logger);
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 1, baseDelay: TimeSpan.FromMilliseconds(50), logger: _logger);
        var circuitBreakerPolicy = PolicyFactory.CreateCircuitBreakerPolicy(
            handledEventsAllowedBeforeBreaking: 2,
            durationOfBreak: TimeSpan.FromMilliseconds(500),
            logger: _logger);
            
        var combinedPolicy = Policy.WrapAsync(circuitBreakerPolicy, retryPolicy, timeoutPolicy);
        
        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            Interlocked.Increment(ref requestCount);
            await Task.Delay(200); // Always times out
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act - Make requests to trigger circuit breaker
        // First request: timeout + retry = 2 timeouts
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();

        // Second request: timeout + retry = 2 more timeouts (total 4, circuit should open)
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();

        // Third request: should hit open circuit breaker immediately
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<CircuitBreakerOpenException>();

        // Assert
        requestCount.Should().Be(4); // 2 requests × 2 attempts each (initial + retry)
        
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Error && entry.Message.Contains("Circuit breaker opened"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CombinedPolicies_Should_Recover_After_Circuit_Breaker_Reset()
    {
        // Arrange
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromMilliseconds(200), _logger);
        var circuitBreakerPolicy = PolicyFactory.CreateCircuitBreakerPolicy(
            handledEventsAllowedBeforeBreaking: 1,
            durationOfBreak: TimeSpan.FromMilliseconds(100),
            logger: _logger);
            
        var combinedPolicy = Policy.WrapAsync(circuitBreakerPolicy, timeoutPolicy);
        
        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            var count = Interlocked.Increment(ref requestCount);
            
            if (count == 1)
            {
                await Task.Delay(500); // First request times out
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            
            // Subsequent requests succeed quickly
            await Task.Delay(50);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"recovered\": true}")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

        // Act
        // 1. Trigger circuit breaker with timeout
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<TimeoutRejectedException>();

        // 2. Verify circuit is open
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<CircuitBreakerOpenException>();

        // 3. Wait for circuit breaker to allow test request
        await Task.Delay(150);

        // 4. Make successful request to reset circuit
        var response = await client.GetAsync("/rest/v1.0/companies");

        // 5. Verify subsequent requests work
        var response2 = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("recovered");
        
        requestCount.Should().Be(3); // Initial timeout + recovery + subsequent
        
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Information && entry.Message.Contains("Circuit breaker reset"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task CombinedPolicies_Should_Handle_Authentication_Token_Refresh_With_Retries()
    {
        // Arrange
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, baseDelay: TimeSpan.FromMilliseconds(100), logger: _logger);
        var timeoutPolicy = PolicyFactory.CreateTimeoutPolicy(TimeSpan.FromSeconds(5), _logger);
        var combinedPolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy);
        
        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var count = Interlocked.Increment(ref requestCount);
            var authHeader = request.Headers.Authorization?.Parameter;
            
            return count switch
            {
                1 => new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("{\"error\": \"Token expired\"}")
                },
                2 => authHeader?.Contains("refreshed") == true 
                    ? new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{\"data\": \"success with refreshed token\"}")
                    }
                    : new HttpResponseMessage(HttpStatusCode.Unauthorized),
                _ => new HttpResponseMessage(HttpStatusCode.OK)
            };
        });

        // Simulate auth handler that refreshes token on retry
        var authHandler = new TestAuthHandler(mockHandler);
        var client = PolicyFactory.CreateHttpClient(authHandler, combinedPolicy);

        // Act
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("success with refreshed token");
        
        requestCount.Should().Be(2); // Initial unauthorized + retry with refreshed token
        
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => entry.Message.Contains("Retry attempt 1"));
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task CombinedPolicies_Should_Handle_Complex_Failure_Scenarios()
    {
        // Arrange - Simulate complex real-world failure scenarios
        var scenarios = new[]
        {
            new { Name = "Cascading Timeouts", FailureType = "timeout", RecoveryAfter = 3 },
            new { Name = "Intermittent Rate Limits", FailureType = "rate_limit", RecoveryAfter = 2 },
            new { Name = "Service Degradation", FailureType = "mixed", RecoveryAfter = 4 }
        };

        foreach (var scenario in scenarios)
        {
            _loggerProvider.ClearLogs();
            
            var combinedPolicy = PolicyFactory.CreateCombinedPolicy(
                retryCount: 3,
                retryBaseDelay: TimeSpan.FromMilliseconds(100),
                circuitBreakerFailureThreshold: 4,
                circuitBreakerDuration: TimeSpan.FromMilliseconds(200),
                timeout: TimeSpan.FromMilliseconds(300),
                logger: _logger);
            
            var requestCount = 0;
            var mockHandler = new TestHttpMessageHandler(async request =>
            {
                var count = Interlocked.Increment(ref requestCount);
                
                if (count <= scenario.RecoveryAfter)
                {
                    // Simulate different failure types
                    switch (scenario.FailureType)
                    {
                        case "timeout":
                            await Task.Delay(500); // Will timeout
                            break;
                        case "rate_limit":
                            return new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                            {
                                Headers = { RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMilliseconds(50)) }
                            };
                        case "mixed":
                            if (count % 2 == 0)
                                await Task.Delay(500); // Timeout
                            else
                                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                            break;
                    }
                }
                
                // Eventually succeed
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"scenario\": \"{scenario.Name}\", \"recovered\": true}}")
                };
            });

            var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);

            // Act
            try
            {
                var response = await client.GetAsync("/rest/v1.0/companies");
                
                // Assert success scenarios
                response.StatusCode.Should().Be(HttpStatusCode.OK, 
                    $"should eventually succeed for {scenario.Name}");
                    
                var content = await response.Content.ReadAsStringAsync();
                content.Should().Contain(scenario.Name);
            }
            catch (Exception ex)
            {
                // Assert failure scenarios (if circuit breaker opens)
                ex.Should().BeOfType<CircuitBreakerOpenException>(
                    $"should fail with circuit breaker open for persistent failures in {scenario.Name}");
            }
            
            requestCount.Should().BeGreaterThan(0, $"should have made requests for {scenario.Name}");
            
            var logEntries = _loggerProvider.GetLogEntries();
            logEntries.Should().NotBeEmpty($"should have logged activity for {scenario.Name}");
        }
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task CombinedPolicies_Should_Have_Acceptable_Performance_Overhead()
    {
        // Arrange
        const int requestCount = 50;
        
        var combinedPolicy = PolicyFactory.CreateCombinedPolicy(
            retryCount: 3,
            retryBaseDelay: TimeSpan.FromMilliseconds(100),
            circuitBreakerFailureThreshold: 5,
            circuitBreakerDuration: TimeSpan.FromSeconds(30),
            timeout: TimeSpan.FromSeconds(5),
            logger: _logger);

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": true}")
            });

        var clientWithPolicies = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);
        var clientWithoutPolicies = PolicyFactory.CreateHttpClient(
            new TestHttpMessageHandler(request => new HttpResponseMessage(HttpStatusCode.OK)));

        // Act
        var withPoliciesTimes = new List<long>();
        var withoutPoliciesTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            var sw1 = Stopwatch.StartNew();
            await clientWithPolicies.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withPoliciesTimes.Add(sw1.ElapsedMilliseconds);

            var sw2 = Stopwatch.StartNew();
            await clientWithoutPolicies.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutPoliciesTimes.Add(sw2.ElapsedMilliseconds);
        }

        // Assert
        var avgWithPolicies = withPoliciesTimes.Average();
        var avgWithoutPolicies = withoutPoliciesTimes.Average();
        var overhead = avgWithoutPolicies > 0 ? 
            (avgWithPolicies - avgWithoutPolicies) / avgWithoutPolicies * 100 : 0;

        overhead.Should().BeLessThan(25); // Less than 25% overhead for combined policies
        _logger.LogInformation("Combined policies overhead: {Overhead:F2}%", overhead);
        
        // Ensure we're not degrading significantly
        avgWithPolicies.Should().BeLessThan(100); // Should still be reasonably fast
    }
}

/// <summary>
/// Test authentication handler that simulates token refresh on authentication failures.
/// </summary>
public class TestAuthHandler : DelegatingHandler
{
    private bool _tokenRefreshed = false;

    public TestAuthHandler(HttpMessageHandler innerHandler)
    {
        InnerHandler = innerHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        // Add initial auth header
        if (request.Headers.Authorization == null)
        {
            var token = _tokenRefreshed ? "Bearer refreshed_token_12345" : "Bearer initial_token_12345";
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Split(' ')[1]);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // If we get unauthorized and haven't refreshed yet, refresh token
        if (response.StatusCode == HttpStatusCode.Unauthorized && !_tokenRefreshed)
        {
            _tokenRefreshed = true;
            
            // Clone request with new token
            var newRequest = new HttpRequestMessage(request.Method, request.RequestUri);
            newRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "refreshed_token_12345");
            
            // Copy content if present
            if (request.Content != null)
            {
                var content = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                newRequest.Content = new ByteArrayContent(content);
                
                foreach (var header in request.Content.Headers)
                {
                    newRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            response.Dispose();
            return await base.SendAsync(newRequest, cancellationToken);
        }

        return response;
    }
}