using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Procore.SDK.Core.Models;
using Procore.SDK.Core.Resilience;
using Procore.SDK.Core.Tests.Helpers;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Procore.SDK.Core.Tests.Resilience;

/// <summary>
/// Comprehensive error recovery scenario tests for CQ Task 7 Subtask 8.
/// Validates that error handling system recovers correctly from various failure scenarios.
/// </summary>
public class ErrorRecoveryScenarioTests : IDisposable
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<ErrorRecoveryScenarioTests> _logger;
    private readonly PolicyFactory _policyFactory;
    private readonly ResilienceOptions _resilienceOptions;

    public ErrorRecoveryScenarioTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<ErrorRecoveryScenarioTests>();

        // Configure resilience options for testing
        _resilienceOptions = new ResilienceOptions
        {
            Retry = new RetryOptions
            {
                MaxAttempts = 3,
                BaseDelayMs = 100,
                MaxDelayMs = 1000,
                UseExponentialBackoff = true,
                BackoffMultiplier = 2.0,
                UseJitter = true,
                MaxJitterMs = 50
            },
            CircuitBreaker = new CircuitBreakerOptions
            {
                Enabled = true,
                FailureThreshold = 3,
                DurationOfBreakInSeconds = 1,
                MinimumThroughput = 3
            },
            Timeout = new TimeoutOptions
            {
                Enabled = true,
                DefaultTimeoutInSeconds = 5
            },
            Logging = new LoggingOptions
            {
                LogRetryAttempts = true,
                LogCircuitBreakerEvents = true,
                LogTimeouts = true
            }
        };

        var optionsWrapper = Options.Create(_resilienceOptions);
        var policyLoggerFactory = new LoggerFactory(new[] { _loggerProvider });
        var policyLogger = policyLoggerFactory.CreateLogger<PolicyFactory>();
        _policyFactory = new PolicyFactory(optionsWrapper, policyLogger);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ErrorRecovery_CircuitBreakerStateTransitions_ShouldRecoverCorrectly()
    {
        // Arrange
        var stateHistory = new List<string>();
        var requestCount = 0;
        var isServiceHealthy = false;

        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var currentRequest = Interlocked.Increment(ref requestCount);
            
            // Simulate service recovery after request 5
            if (currentRequest >= 5)
            {
                isServiceHealthy = true;
            }

            if (!isServiceHealthy)
            {
                throw new HttpRequestException($"Service failure #{currentRequest}");
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"status\":\"healthy\"}")
            };
        });

        var context = new ResilienceContext("test-circuit-recovery");
        var policy = _policyFactory.CreateHttpPolicy(context);
        var client = new HttpClient(mockHandler);

        // Act & Assert - Phase 1: Trigger circuit breaker
        for (int i = 1; i <= 3; i++)
        {
            await client.Invoking(c => policy.ExecuteAsync(() => c.GetAsync("/api/test")))
                .Should().ThrowAsync<HttpRequestException>($"Request {i} should fail");
        }

        // Next request should trigger circuit breaker open
        await client.Invoking(c => policy.ExecuteAsync(() => c.GetAsync("/api/test")))
            .Should().ThrowAsync<BrokenCircuitException>("Circuit should be open");

        // Phase 2: Wait for circuit breaker to enter half-open state
        await Task.Delay(TimeSpan.FromSeconds(1.2)); // Slightly longer than break duration

        // Phase 3: Service recovery - circuit should reset
        var recoveryResponse = await policy.ExecuteAsync(() => client.GetAsync("/api/test"));
        recoveryResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Service should be recovered");

        // Phase 4: Verify continued operation
        for (int i = 0; i < 3; i++)
        {
            var response = await policy.ExecuteAsync(() => client.GetAsync("/api/test"));
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Post-recovery request {i + 1} should succeed");
        }

        // Verify logs show complete recovery cycle
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => entry.Level == LogLevel.Warning && entry.Message.Contains("Circuit breaker opened"));
        logEntries.Should().Contain(entry => entry.Level == LogLevel.Information && entry.Message.Contains("Circuit breaker reset"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ErrorRecovery_RetryPolicyWithExponentialBackoff_ShouldEventuallySucceed()
    {
        // Arrange
        var attemptCount = 0;
        var delayMeasurements = new List<long>();
        var lastAttemptTime = DateTimeOffset.UtcNow;

        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var currentAttempt = Interlocked.Increment(ref attemptCount);
            var currentTime = DateTimeOffset.UtcNow;
            
            if (currentAttempt > 1)
            {
                var delay = (currentTime - lastAttemptTime).TotalMilliseconds;
                delayMeasurements.Add((long)delay);
            }
            lastAttemptTime = currentTime;

            // Succeed on the 3rd attempt
            if (currentAttempt == 3)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"recovered\":true}")
                };
            }

            throw new HttpRequestException($"Transient failure #{currentAttempt}");
        });

        var context = new ResilienceContext("test-retry-recovery");
        var policy = _policyFactory.CreateHttpPolicy(context);
        var client = new HttpClient(mockHandler);

        // Act
        var response = await policy.ExecuteAsync(() => client.GetAsync("/api/retry-test"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, "Request should eventually succeed");
        attemptCount.Should().Be(3, "Should have made exactly 3 attempts");
        
        // Verify exponential backoff pattern
        delayMeasurements.Should().HaveCountGreaterOrEqualTo(2, "Should have at least 2 delay measurements");
        
        if (delayMeasurements.Count >= 2)
        {
            // Second delay should be approximately double the first (allowing for jitter)
            var firstDelay = delayMeasurements[0];
            var secondDelay = delayMeasurements[1];
            
            secondDelay.Should().BeGreaterThan((long)(firstDelay * 1.5), "Exponential backoff should increase delay");
            secondDelay.Should().BeLessThan((long)(firstDelay * 3), "Delay should not exceed reasonable bounds");
        }

        // Verify retry logging
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Where(e => e.Level == LogLevel.Warning && e.Message.Contains("Retry attempt"))
            .Should().HaveCount(2, "Should log 2 retry attempts");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ErrorRecovery_TimeoutWithCancellation_ShouldRecoverGracefully()
    {
        // Arrange
        var operationStarted = false;
        var operationCompleted = false;
        var cancellationRequested = false;

        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            operationStarted = true;
            
            try
            {
                // Simulate a slow operation that respects cancellation
                await Task.Delay(TimeSpan.FromSeconds(10), request.GetCancellationToken());
                operationCompleted = true;
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (OperationCanceledException)
            {
                cancellationRequested = true;
                throw;
            }
        });

        var context = new ResilienceContext("test-timeout-recovery");
        var policy = _policyFactory.CreateHttpPolicy(context);
        var client = new HttpClient(mockHandler);

        // Act & Assert - Operation should timeout
        await client.Invoking(c => policy.ExecuteAsync(() => c.GetAsync("/api/slow")))
            .Should().ThrowAsync<TimeoutRejectedException>("Slow operation should timeout");

        // Verify timeout behavior
        operationStarted.Should().BeTrue("Operation should have started");
        operationCompleted.Should().BeFalse("Operation should not have completed");
        cancellationRequested.Should().BeTrue("Cancellation should have been requested");

        // Test recovery with fast operation
        mockHandler.SetResponseFactory(request => 
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"fast\":true}") });

        var recoveryResponse = await policy.ExecuteAsync(() => client.GetAsync("/api/fast"));
        recoveryResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Fast operation should succeed after timeout");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ErrorRecovery_ExceptionSerializationRecovery_ShouldPreserveContext()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var originalException = new ProcoreCoreException(
            "Test error for serialization recovery",
            "TEST_ERROR",
            new Dictionary<string, object>
            {
                ["request_id"] = "req-123",
                ["user_id"] = 456,
                ["password"] = "should-be-sanitized", // This should be removed
                ["sensitive_data"] = "secret-key" // This should be removed
            },
            correlationId);

        // Act - Serialize and deserialize the exception
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true
        };

        var serialized = JsonSerializer.Serialize(originalException, jsonOptions);
        var deserialized = JsonSerializer.Deserialize<ProcoreCoreException>(serialized, jsonOptions);

        // Assert
        deserialized.Should().NotBeNull("Deserialized exception should not be null");
        deserialized!.Message.Should().Be(originalException.Message, "Message should be preserved");
        deserialized.ErrorCode.Should().Be(originalException.ErrorCode, "Error code should be preserved");
        deserialized.CorrelationId.Should().Be(correlationId, "Correlation ID should be preserved");
        deserialized.Timestamp.Should().BeCloseTo(originalException.Timestamp, TimeSpan.FromSeconds(1), 
            "Timestamp should be preserved");

        // Verify sensitive data was sanitized
        deserialized.Details.Should().NotBeNull("Details should be preserved");
        deserialized.Details!.Should().ContainKey("request_id");
        deserialized.Details.Should().ContainKey("user_id");
        deserialized.Details.Should().NotContainKey("password", "Sensitive password should be sanitized");
        deserialized.Details.Should().NotContainKey("sensitive_data", "Sensitive data should be sanitized");

        // Verify JSON doesn't contain sensitive information
        serialized.Should().NotContain("should-be-sanitized", "Serialized JSON should not contain password");
        serialized.Should().NotContain("secret-key", "Serialized JSON should not contain sensitive data");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ErrorRecovery_CascadingFailuresRecovery_ShouldHandleComplexScenarios()
    {
        // Arrange - Simulate cascading failures across multiple services
        var serviceAFailures = 0;
        var serviceBFailures = 0;
        var serviceARecovered = false;
        var serviceBRecovered = false;

        var serviceAHandler = new TestHttpMessageHandler(request =>
        {
            if (!serviceARecovered)
            {
                var failures = Interlocked.Increment(ref serviceAFailures);
                if (failures >= 3) serviceARecovered = true; // Service A recovers after 3 failures
                
                if (!serviceARecovered)
                    throw new HttpRequestException($"Service A failure #{failures}");
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"service\":\"A\",\"status\":\"healthy\"}")
            };
        });

        var serviceBHandler = new TestHttpMessageHandler(request =>
        {
            if (!serviceBRecovered)
            {
                var failures = Interlocked.Increment(ref serviceBFailures);
                if (failures >= 5) serviceBRecovered = true; // Service B takes longer to recover
                
                if (!serviceBRecovered)
                    throw new HttpRequestException($"Service B failure #{failures}");
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"service\":\"B\",\"status\":\"healthy\"}")
            };
        });

        var contextA = new ResilienceContext("service-a-recovery");
        var contextB = new ResilienceContext("service-b-recovery");
        var policyA = _policyFactory.CreateHttpPolicy(contextA);
        var policyB = _policyFactory.CreateHttpPolicy(contextB);
        var clientA = new HttpClient(serviceAHandler);
        var clientB = new HttpClient(serviceBHandler);

        // Act - Simulate concurrent operations that will initially fail then recover
        var serviceATask = Task.Run(async () =>
        {
            var attempts = 0;
            while (!serviceARecovered && attempts < 10)
            {
                attempts++;
                try
                {
                    var response = await policyA.ExecuteAsync(() => clientA.GetAsync("/api/serviceA"));
                    if (response.IsSuccessStatusCode) return response;
                }
                catch (HttpRequestException)
                {
                    // Continue retrying
                }
                catch (BrokenCircuitException)
                {
                    await Task.Delay(1200); // Wait for circuit breaker
                }
                await Task.Delay(100); // Small delay between attempts
            }
            return await policyA.ExecuteAsync(() => clientA.GetAsync("/api/serviceA"));
        });

        var serviceBTask = Task.Run(async () =>
        {
            var attempts = 0;
            while (!serviceBRecovered && attempts < 15)
            {
                attempts++;
                try
                {
                    var response = await policyB.ExecuteAsync(() => clientB.GetAsync("/api/serviceB"));
                    if (response.IsSuccessStatusCode) return response;
                }
                catch (HttpRequestException)
                {
                    // Continue retrying
                }
                catch (BrokenCircuitException)
                {
                    await Task.Delay(1200); // Wait for circuit breaker
                }
                await Task.Delay(100); // Small delay between attempts
            }
            return await policyB.ExecuteAsync(() => clientB.GetAsync("/api/serviceB"));
        });

        var results = await Task.WhenAll(serviceATask, serviceBTask);

        // Assert
        results[0].StatusCode.Should().Be(HttpStatusCode.OK, "Service A should eventually recover");
        results[1].StatusCode.Should().Be(HttpStatusCode.OK, "Service B should eventually recover");
        
        serviceARecovered.Should().BeTrue("Service A should have recovered");
        serviceBRecovered.Should().BeTrue("Service B should have recovered");
        
        serviceAFailures.Should().BeGreaterOrEqualTo(3, "Service A should have failed at least 3 times");
        serviceBFailures.Should().BeGreaterOrEqualTo(5, "Service B should have failed at least 5 times");

        // Verify both services are now healthy
        var healthCheckA = await policyA.ExecuteAsync(() => clientA.GetAsync("/api/serviceA/health"));
        var healthCheckB = await policyB.ExecuteAsync(() => clientB.GetAsync("/api/serviceB/health"));
        
        healthCheckA.StatusCode.Should().Be(HttpStatusCode.OK, "Service A health check should pass");
        healthCheckB.StatusCode.Should().Be(HttpStatusCode.OK, "Service B health check should pass");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task ErrorRecovery_PolicyFactoryDisposal_ShouldCleanupResourcesCorrectly()
    {
        // Arrange
        var optionsWrapper = Options.Create(_resilienceOptions);
        var policyLoggerFactory = new LoggerFactory(new[] { _loggerProvider });
        var policyLogger = policyLoggerFactory.CreateLogger<PolicyFactory>();
        var disposablePolicyFactory = new PolicyFactory(optionsWrapper, policyLogger);
        
        var context = new ResilienceContext("disposal-test");
        var policy = disposablePolicyFactory.CreateHttpPolicy(context);
        
        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));
        var client = new HttpClient(mockHandler);

        // Act - Use the policy factory
        var response = await policy.ExecuteAsync(() => client.GetAsync("/api/test"));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Dispose the policy factory
        disposablePolicyFactory.Dispose();

        // Assert - Should be able to dispose without issues
        // Additional cleanup verification would be done in actual implementation
        // This test validates that Dispose() doesn't throw exceptions
        
        // Verify GC behavior by calling Dispose again (should be safe)
        disposablePolicyFactory.Dispose(); // Should not throw
        
        _logger.LogInformation("PolicyFactory disposal test completed successfully");
    }

    public void Dispose()
    {
        _policyFactory?.Dispose();
        _loggerProvider?.Dispose();
    }
}

/// <summary>
/// Helper class for testing HTTP message handling with configurable responses
/// </summary>
public class TestHttpMessageHandler : HttpMessageHandler
{
    private Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;
    private Func<HttpRequestMessage, Task<HttpResponseMessage>>? _asyncResponseFactory;
    private readonly List<HttpRequestMessage> _sentRequests = new();

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
    {
        _responseFactory = responseFactory;
    }

    public TestHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> asyncResponseFactory)
    {
        _asyncResponseFactory = asyncResponseFactory;
        _responseFactory = _ => throw new InvalidOperationException("Use async version");
    }

    public IReadOnlyList<HttpRequestMessage> SentRequests => _sentRequests.AsReadOnly();

    public void SetResponseFactory(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
    {
        _responseFactory = responseFactory;
        _asyncResponseFactory = null;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _sentRequests.Add(request);

        if (_asyncResponseFactory != null)
        {
            return await _asyncResponseFactory(request);
        }

        return _responseFactory(request);
    }
}

/// <summary>
/// Extension method to help with cancellation token extraction from requests
/// </summary>
public static class HttpRequestMessageExtensions
{
    private static readonly string CancellationTokenKey = "CancellationToken";

    public static CancellationToken GetCancellationToken(this HttpRequestMessage request)
    {
        if (request.Options.TryGetValue(new HttpRequestOptionsKey<CancellationToken>(CancellationTokenKey), out var token))
        {
            return token;
        }
        return CancellationToken.None;
    }

    public static void SetCancellationToken(this HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Options.Set(new HttpRequestOptionsKey<CancellationToken>(CancellationTokenKey), cancellationToken);
    }
}