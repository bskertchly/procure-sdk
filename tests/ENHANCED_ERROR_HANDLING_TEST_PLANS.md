# Enhanced Error Handling and Resilience Patterns Test Plans

## Overview

This document outlines comprehensive test plans for enhanced error handling and resilience patterns in the Procore SDK. The test plans focus on five key areas:

1. **Polly Integration Tests** - Retry policies, circuit breakers, and timeout policies
2. **Custom Exception Tests** - Domain-specific exception types and error handling
3. **Resilience Pattern Tests** - Failure scenarios and recovery behaviors
4. **Structured Logging Tests** - Error logging and monitoring with correlation IDs
5. **Performance Impact Tests** - Ensuring resilience patterns don't degrade performance

## Test Architecture Philosophy

### Core Principles
- **Realistic Failure Scenarios**: Tests must simulate real-world production failures
- **Deterministic Testing**: Use controllable failure injection rather than random failures
- **Comprehensive Coverage**: Test all failure modes and recovery paths
- **Performance Validation**: Measure resilience pattern overhead
- **Production Readiness**: Tests should reflect actual Procore API behaviors

### Test Categories
- **Unit Tests**: Individual resilience components in isolation
- **Integration Tests**: End-to-end failure and recovery scenarios
- **Performance Tests**: Measure overhead of resilience patterns
- **Chaos Engineering**: Controlled failure injection testing

## 1. Polly Integration Test Plans

### 1.1 Retry Policy Tests

#### 1.1.1 Exponential Backoff Retry Tests

**Test Class**: `ExponentialBackoffRetryTests`

```csharp
/// <summary>
/// Tests exponential backoff retry policies for transient failures.
/// Validates that retries occur with proper intervals and eventually succeed or fail.
/// </summary>
public class ExponentialBackoffRetryTests
{
    [Fact]
    public async Task RetryPolicy_Should_Retry_On_Transient_HttpRequestException()
    {
        // Arrange
        var failureCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            failureCount++;
            if (failureCount <= 2) // Fail first 2 attempts
            {
                throw new HttpRequestException("Temporary network error");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogInformation("Retry {RetryCount} after {Delay}ms", retryCount, timespan.TotalMilliseconds);
                });

        var httpClient = new HttpClient(mockHandler);
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = await retryPolicy.ExecuteAsync(async () =>
            await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies"));
        stopwatch.Stop();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        failureCount.Should().Be(3); // 2 failures + 1 success
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(300); // At least 100 + 200ms delays
    }

    [Fact]
    public async Task RetryPolicy_Should_Not_Retry_On_Non_Transient_Errors()
    {
        // Arrange
        var attemptCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            attemptCount++;
            return new HttpResponseMessage(HttpStatusCode.Forbidden); // Non-transient error
        });

        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(100));

        var httpClient = new HttpClient(mockHandler);
        
        // Act
        var response = await retryPolicy.ExecuteAsync(async () =>
            await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        attemptCount.Should().Be(1); // Only one attempt, no retries
    }

    [Fact]
    public async Task RetryPolicy_Should_Handle_Timeout_Scenarios()
    {
        // Arrange
        var timeoutCount = 0;
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            timeoutCount++;
            if (timeoutCount <= 2)
            {
                await Task.Delay(5000); // Simulate timeout
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(1));
        var retryPolicy = Policy
            .Handle<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(50));

        var combinedPolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy);
        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await combinedPolicy.ExecuteAsync(async () =>
            await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies"));

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        timeoutCount.Should().Be(3); // 2 timeouts + 1 success
    }
}
```

#### 1.1.2 Jittered Retry Tests

```csharp
/// <summary>
/// Tests jittered retry policies to prevent thundering herd problems.
/// </summary>
public class JitteredRetryTests
{
    [Fact]
    public async Task JitteredRetry_Should_Vary_Delays_Between_Concurrent_Requests()
    {
        // Arrange
        var delays = new ConcurrentBag<TimeSpan>();
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: retryAttempt =>
                {
                    var baseDelay = TimeSpan.FromMilliseconds(100 * retryAttempt);
                    var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 50));
                    var totalDelay = baseDelay.Add(jitter);
                    delays.Add(totalDelay);
                    return totalDelay;
                });

        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Temporary failure"));

        var httpClient = new HttpClient(mockHandler);

        // Act
        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                    await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies"));
            }
            catch (HttpRequestException)
            {
                // Expected to fail after retries
            }
        });

        await Task.WhenAll(tasks);

        // Assert
        delays.Should().HaveCount(20); // 10 requests × 2 retries each
        delays.Select(d => d.TotalMilliseconds).Distinct().Should().HaveCountGreaterThan(5); // Should have jitter variation
    }
}
```

### 1.2 Circuit Breaker Tests

#### 1.2.1 Circuit Breaker State Transition Tests

**Test Class**: `CircuitBreakerStateTests`

```csharp
/// <summary>
/// Tests circuit breaker state transitions (Closed → Open → Half-Open → Closed/Open).
/// </summary>
public class CircuitBreakerStateTests
{
    [Fact]
    public async Task CircuitBreaker_Should_Transition_From_Closed_To_Open_After_Consecutive_Failures()
    {
        // Arrange
        var callCount = 0;
        var circuitState = CircuitBreakerState.Closed;
        
        var circuitBreaker = Policy
            .Handle<HttpRequestException>()
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

        var mockHandler = new TestHttpMessageHandler(request =>
        {
            callCount++;
            throw new HttpRequestException("Service unavailable");
        });

        var httpClient = new HttpClient(mockHandler);

        // Act & Assert - Closed state, failures accumulate
        for (int i = 1; i <= 3; i++)
        {
            circuitState.Should().Be(CircuitBreakerState.Closed);
            
            await circuitBreaker.Invoking(async cb => 
                await cb.ExecuteAsync(async () => 
                    await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies")))
                .Should().ThrowAsync<HttpRequestException>();
                
            callCount.Should().Be(i);
        }

        // Circuit should now be open
        circuitState.Should().Be(CircuitBreakerState.Open);

        // Subsequent calls should fail fast without hitting the service
        await circuitBreaker.Invoking(async cb => 
            await cb.ExecuteAsync(async () => 
                await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies")))
            .Should().ThrowAsync<CircuitBreakerOpenException>();
            
        callCount.Should().Be(3); // No additional calls made
    }

    [Fact]
    public async Task CircuitBreaker_Should_Transition_To_HalfOpen_After_Break_Duration()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var breakDuration = TimeSpan.FromMilliseconds(100);
        
        var circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: breakDuration,
                onBreak: (exception, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var callCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            callCount++;
            if (callCount == 1)
                throw new HttpRequestException("Initial failure");
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var httpClient = new HttpClient(mockHandler);

        // Act - Trigger circuit breaker
        await circuitBreaker.Invoking(async cb => 
            await cb.ExecuteAsync(async () => 
                await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies")))
            .Should().ThrowAsync<HttpRequestException>();

        circuitState.Should().Be(CircuitBreakerState.Open);

        // Wait for break duration
        await Task.Delay(breakDuration.Add(TimeSpan.FromMilliseconds(50)));

        // Next call should transition to half-open and succeed
        var result = await circuitBreaker.ExecuteAsync(async () =>
            await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies"));

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        circuitState.Should().Be(CircuitBreakerState.Closed); // Reset after successful call
        callCount.Should().Be(2);
    }

    [Fact]
    public async Task CircuitBreaker_Should_Reopen_If_HalfOpen_Call_Fails()
    {
        // Arrange
        var circuitState = CircuitBreakerState.Closed;
        var breakDuration = TimeSpan.FromMilliseconds(100);
        
        var circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: breakDuration,
                onBreak: (exception, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Persistent failure"));

        var httpClient = new HttpClient(mockHandler);

        // Act - Trigger circuit breaker
        await circuitBreaker.Invoking(async cb => 
            await cb.ExecuteAsync(async () => 
                await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies")))
            .Should().ThrowAsync<HttpRequestException>();

        circuitState.Should().Be(CircuitBreakerState.Open);

        // Wait for break duration
        await Task.Delay(breakDuration.Add(TimeSpan.FromMilliseconds(50)));

        // Next call should fail and reopen circuit
        await circuitBreaker.Invoking(async cb => 
            await cb.ExecuteAsync(async () => 
                await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies")))
            .Should().ThrowAsync<HttpRequestException>();

        // Assert
        circuitState.Should().Be(CircuitBreakerState.Open); // Should be open again
    }
}
```

### 1.3 Timeout Policy Tests

#### 1.3.1 Request Timeout Tests

**Test Class**: `TimeoutPolicyTests`

```csharp
/// <summary>
/// Tests timeout policies for long-running requests.
/// </summary>
public class TimeoutPolicyTests
{
    [Fact]
    public async Task TimeoutPolicy_Should_Cancel_Long_Running_Requests()
    {
        // Arrange
        var timeoutDuration = TimeSpan.FromMilliseconds(500);
        var timeoutPolicy = Policy.TimeoutAsync(timeoutDuration);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(1000); // Longer than timeout
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var httpClient = new HttpClient(mockHandler);

        // Act & Assert
        var stopwatch = Stopwatch.StartNew();
        
        await timeoutPolicy.Invoking(async p =>
            await p.ExecuteAsync(async () =>
                await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies")))
            .Should().ThrowAsync<TimeoutRejectedException>();
            
        stopwatch.Stop();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(timeoutDuration.TotalMilliseconds + 100);
    }

    [Fact]
    public async Task TimeoutPolicy_Should_Allow_Fast_Requests()
    {
        // Arrange
        var timeoutDuration = TimeSpan.FromSeconds(1);
        var timeoutPolicy = Policy.TimeoutAsync(timeoutDuration);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(100); // Faster than timeout
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await timeoutPolicy.ExecuteAsync(async () =>
            await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies"));

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TimeoutPolicy_Should_Respect_CancellationToken()
    {
        // Arrange
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(10));
        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(1000, request.GetCancellationToken());
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var httpClient = new HttpClient(mockHandler);

        // Act & Assert
        await timeoutPolicy.Invoking(async p =>
            await p.ExecuteAsync(async ct =>
                await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies", ct), 
                cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}
```

### 1.4 Combined Policy Tests

#### 1.4.1 Policy Wrap Tests

**Test Class**: `PolicyWrapTests`

```csharp
/// <summary>
/// Tests combinations of retry, circuit breaker, and timeout policies.
/// </summary>
public class PolicyWrapTests
{
    [Fact]
    public async Task CombinedPolicies_Should_Apply_Timeout_Then_Retry_Then_CircuitBreaker()
    {
        // Arrange
        var executionResults = new List<string>();
        
        var timeoutPolicy = Policy
            .TimeoutAsync(TimeSpan.FromMilliseconds(100))
            .AsAsyncPolicy();
            
        var retryPolicy = Policy
            .Handle<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(50),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    executionResults.Add($"Retry {retryCount}");
                });
                
        var circuitBreakerPolicy = Policy
            .Handle<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(1),
                onBreak: (exception, duration) => executionResults.Add("Circuit Opened"),
                onReset: () => executionResults.Add("Circuit Reset"));

        var combinedPolicy = Policy.WrapAsync(circuitBreakerPolicy, retryPolicy, timeoutPolicy);
        
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            executionResults.Add("Request Started");
            await Task.Delay(200); // Will timeout
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var httpClient = new HttpClient(mockHandler);

        // Act
        await combinedPolicy.Invoking(async p =>
            await p.ExecuteAsync(async () =>
            {
                executionResults.Add("Policy Execution");
                return await httpClient.GetAsync("https://api.procore.com/rest/v1.0/companies");
            }))
            .Should().ThrowAsync<TimeoutRejectedException>();

        // Assert
        executionResults.Should().Contain("Policy Execution");
        executionResults.Should().Contain("Request Started");
        executionResults.Should().Contain("Retry 1");
        executionResults.Should().Contain("Retry 2");
        executionResults.Count(r => r == "Request Started").Should().Be(3); // Initial + 2 retries
    }
}
```

## 2. Custom Exception Tests

### 2.1 Domain-Specific Exception Types

**Test Class**: `CustomExceptionTests`

```csharp
/// <summary>
/// Tests for domain-specific exception types and their properties.
/// </summary>
public class CustomExceptionTests
{
    [Fact]
    public void ProcoreCoreException_Should_Include_ErrorCode_And_Details()
    {
        // Arrange
        var details = new Dictionary<string, object>
        {
            { "field", "name" },
            { "error", "is required" }
        };

        // Act
        var exception = new ProcoreCoreException("Validation failed", "VALIDATION_ERROR", details);

        // Assert
        exception.Message.Should().Be("Validation failed");
        exception.ErrorCode.Should().Be("VALIDATION_ERROR");
        exception.Details.Should().NotBeNull();
        exception.Details!["field"].Should().Be("name");
        exception.Details!["error"].Should().Be("is required");
    }

    [Fact]
    public void RateLimitExceededException_Should_Include_RetryAfter_TimeSpan()
    {
        // Arrange
        var retryAfter = TimeSpan.FromMinutes(5);

        // Act
        var exception = new RateLimitExceededException(retryAfter);

        // Assert
        exception.ErrorCode.Should().Be("RATE_LIMIT_EXCEEDED");
        exception.RetryAfter.Should().Be(retryAfter);
        exception.Message.Should().Contain("300 seconds");
    }

    [Fact]
    public void ResourceNotFoundException_Should_Include_ResourceType_And_Id()
    {
        // Arrange & Act
        var exception = new ResourceNotFoundException("Company", 12345);

        // Assert
        exception.ErrorCode.Should().Be("RESOURCE_NOT_FOUND");
        exception.Message.Should().Contain("Company");
        exception.Message.Should().Contain("12345");
    }

    [Fact]
    public void ProcoreException_Should_Be_Serializable()
    {
        // Arrange
        var originalException = new ProcoreCoreException(
            "Test error", 
            "TEST_ERROR", 
            new Dictionary<string, object> { { "key", "value" } });

        // Act
        var json = JsonSerializer.Serialize(originalException, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        var deserializedException = JsonSerializer.Deserialize<ProcoreCoreException>(json);

        // Assert
        deserializedException.Should().NotBeNull();
        deserializedException!.Message.Should().Be(originalException.Message);
        deserializedException.ErrorCode.Should().Be(originalException.ErrorCode);
    }
}
```

### 2.2 Exception Context Preservation

**Test Class**: `ExceptionContextTests`

```csharp
/// <summary>
/// Tests that exceptions preserve important context information.
/// </summary>
public class ExceptionContextTests
{
    [Fact]
    public void Exception_Should_Preserve_CorrelationId_In_Context()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var context = new Dictionary<string, object>
        {
            { "CorrelationId", correlationId },
            { "RequestPath", "/rest/v1.0/companies/123" },
            { "UserId", "user@procore.com" }
        };

        // Act
        var exception = new ProcoreCoreException("Test error", "TEST_ERROR", context);

        // Assert
        exception.Details.Should().ContainKey("CorrelationId");
        exception.Details!["CorrelationId"].Should().Be(correlationId);
        exception.Details.Should().ContainKey("RequestPath");
        exception.Details.Should().ContainKey("UserId");
    }

    [Fact]
    public void Exception_Should_Sanitize_Sensitive_Information()
    {
        // Arrange
        var sensitiveData = new Dictionary<string, object>
        {
            { "Password", "secret123" },
            { "AccessToken", "bearer_token_value" },
            { "ApiKey", "api_key_value" },
            { "Name", "John Doe" } // Non-sensitive
        };

        // Act
        var exception = new ProcoreCoreException("Auth failed", "AUTH_ERROR", sensitiveData);

        // Assert
        exception.Details.Should().ContainKey("Name");
        exception.Details.Should().NotContainKey("Password");
        exception.Details.Should().NotContainKey("AccessToken");
        exception.Details.Should().NotContainKey("ApiKey");
    }
}
```

## 3. Resilience Pattern Tests

### 3.1 Failure Scenario Tests

**Test Class**: `FailureScenarioTests`

```csharp
/// <summary>
/// Tests various failure scenarios that can occur when calling Procore APIs.
/// </summary>
public class FailureScenarioTests
{
    [Fact]
    public async Task Should_Handle_Intermittent_Network_Failures()
    {
        // Arrange
        var requestCount = 0;
        var policy = CreateRetryPolicy();
        
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            return requestCount switch
            {
                1 => throw new HttpRequestException("Network unreachable"),
                2 => throw new SocketException((int)SocketError.TimedOut),
                3 => new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("""{"companies": []}""")
                    },
                _ => throw new InvalidOperationException("Unexpected request")
            };
        });

        var client = CreateHttpClient(mockHandler, policy);

        // Act
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(3);
    }

    [Fact]
    public async Task Should_Handle_Service_Temporarily_Unavailable()
    {
        // Arrange
        var requestCount = 0;
        var policy = CreateRetryWithBackoffPolicy();
        
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            return requestCount switch
            {
                <= 2 => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Headers = { RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(1)) }
                },
                3 => new HttpResponseMessage(HttpStatusCode.OK),
                _ => throw new InvalidOperationException("Unexpected request")
            };
        });

        var client = CreateHttpClient(mockHandler, policy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(3);
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(1000); // Should respect retry-after
    }

    [Fact]
    public async Task Should_Handle_Authentication_Token_Expiry()
    {
        // Arrange
        var requestCount = 0;
        var tokenRefreshCount = 0;
        
        var mockTokenManager = Substitute.For<ITokenManager>();
        mockTokenManager.GetAccessTokenAsync(Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                tokenRefreshCount++;
                return new AccessToken($"token_{tokenRefreshCount}", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
            });

        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            var authHeader = request.Headers.Authorization?.Parameter;
            
            return authHeader switch
            {
                "token_1" => new HttpResponseMessage(HttpStatusCode.Unauthorized),
                "token_2" => new HttpResponseMessage(HttpStatusCode.OK),
                _ => new HttpResponseMessage(HttpStatusCode.BadRequest)
            };
        });

        var authHandler = new ProcoreAuthHandler(mockTokenManager, _logger);
        authHandler.InnerHandler = mockHandler;
        var client = new HttpClient(authHandler);

        // Act
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(2); // Initial request + retry with new token
        tokenRefreshCount.Should().Be(2); // Initial token + refresh
    }

    [Fact]
    public async Task Should_Handle_Rate_Limiting_With_Exponential_Backoff()
    {
        // Arrange
        var requestCount = 0;
        var policy = CreateRateLimitPolicy();
        
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            return requestCount switch
            {
                1 => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    Headers = { RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(100)) }
                },
                2 => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    Headers = { RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(200)) }
                },
                3 => new HttpResponseMessage(HttpStatusCode.OK),
                _ => throw new InvalidOperationException("Unexpected request")
            };
        });

        var client = CreateHttpClient(mockHandler, policy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(3);
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(300); // Should respect retry-after headers
    }
}
```

### 3.2 Recovery Behavior Tests

**Test Class**: `RecoveryBehaviorTests`

```csharp
/// <summary>
/// Tests recovery behaviors after various failure conditions.
/// </summary>
public class RecoveryBehaviorTests
{
    [Fact]
    public async Task Should_Recover_Gracefully_After_Circuit_Breaker_Reset()
    {
        // Arrange
        var requestCount = 0;
        var circuitState = CircuitBreakerState.Closed;
        
        var circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromMilliseconds(100),
                onBreak: (ex, duration) => circuitState = CircuitBreakerState.Open,
                onReset: () => circuitState = CircuitBreakerState.Closed,
                onHalfOpen: () => circuitState = CircuitBreakerState.HalfOpen);

        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            return requestCount switch
            {
                <= 2 => throw new HttpRequestException("Service failure"),
                > 2 => new HttpResponseMessage(HttpStatusCode.OK)
            };
        });

        var client = CreateHttpClient(mockHandler, circuitBreaker);

        // Act - Trigger circuit breaker
        for (int i = 0; i < 2; i++)
        {
            await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
                .Should().ThrowAsync<HttpRequestException>();
        }

        circuitState.Should().Be(CircuitBreakerState.Open);

        // Fast-fail while circuit is open
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<CircuitBreakerOpenException>();

        requestCount.Should().Be(2); // No additional requests while circuit is open

        // Wait for circuit to allow a test request
        await Task.Delay(150);

        // Recovery request should succeed
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        circuitState.Should().Be(CircuitBreakerState.Closed);
        requestCount.Should().Be(3);
    }

    [Fact]
    public async Task Should_Maintain_Performance_After_Recovery()
    {
        // Arrange
        var failurePhase = true;
        var requestTimes = new List<long>();
        
        var policy = CreateCombinedPolicy();
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var stopwatch = Stopwatch.StartNew();
            if (failurePhase)
            {
                failurePhase = false; // Only fail once
                Thread.Sleep(50); // Simulate slow failure
                stopwatch.Stop();
                requestTimes.Add(stopwatch.ElapsedMilliseconds);
                throw new HttpRequestException("Temporary failure");
            }
            
            Thread.Sleep(10); // Simulate normal response time
            stopwatch.Stop();
            requestTimes.Add(stopwatch.ElapsedMilliseconds);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = CreateHttpClient(mockHandler, policy);

        // Act - Initial request that fails and recovers
        var response = await client.GetAsync("/rest/v1.0/companies");
        
        // Subsequent requests should be fast
        for (int i = 0; i < 5; i++)
        {
            await client.GetAsync("/rest/v1.0/companies");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        requestTimes.Should().HaveCount(7); // 1 failure + 1 retry success + 5 subsequent
        
        // After recovery, requests should be consistently fast
        var recoveryTimes = requestTimes.Skip(2).ToList();
        recoveryTimes.Should().AllSatisfy(time => time.Should().BeLessThan(50));
    }
}
```

## 4. Structured Logging Tests

### 4.1 Error Logging Tests

**Test Class**: `ErrorLoggingTests`

```csharp
/// <summary>
/// Tests that errors are logged with proper structure and correlation IDs.
/// </summary>
public class ErrorLoggingTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<CoreClient> _logger;

    public ErrorLoggingTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<CoreClient>();
    }

    [Fact]
    public async Task Should_Log_Request_Failures_With_Correlation_Id()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var policy = CreateLoggingPolicy();
        
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            throw new HttpRequestException("Service unavailable");
        });

        var client = CreateHttpClient(mockHandler, policy);
        client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

        // Act
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Error &&
            entry.Message.Contains("Service unavailable") &&
            entry.State.ToString().Contains(correlationId));
    }

    [Fact]
    public async Task Should_Log_Retry_Attempts_With_Context()
    {
        // Arrange
        var policy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(10),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning("Retry attempt {RetryCount} after {Delay}ms due to {Exception}", 
                        retryCount, timespan.TotalMilliseconds, outcome.Exception?.Message);
                });

        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Temporary failure"));

        var client = CreateHttpClient(mockHandler, policy);

        // Act
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Where(e => e.Level == LogLevel.Warning).Should().HaveCount(2);
        logEntries.Should().Contain(entry => entry.Message.Contains("Retry attempt 1"));
        logEntries.Should().Contain(entry => entry.Message.Contains("Retry attempt 2"));
    }

    [Fact]
    public async Task Should_Log_Circuit_Breaker_State_Changes()
    {
        // Arrange
        var policy = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: TimeSpan.FromMilliseconds(100),
                onBreak: (exception, duration) =>
                {
                    _logger.LogError("Circuit breaker opened due to {Exception} for {Duration}", 
                        exception.Message, duration);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker reset - service recovered");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker half-open - testing service");
                });

        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            return requestCount switch
            {
                1 => throw new HttpRequestException("Service failure"),
                2 => new HttpResponseMessage(HttpStatusCode.OK)
            };
        });

        var client = CreateHttpClient(mockHandler, policy);

        // Act
        // Trigger circuit breaker
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Wait for break duration and test recovery
        await Task.Delay(150);
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Error && entry.Message.Contains("Circuit breaker opened"));
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Information && entry.Message.Contains("Circuit breaker half-open"));
        logEntries.Should().Contain(entry => 
            entry.Level == LogLevel.Information && entry.Message.Contains("Circuit breaker reset"));
    }

    [Fact]
    public void Should_Not_Log_Sensitive_Information()
    {
        // Arrange
        var sensitiveData = new Dictionary<string, object>
        {
            { "password", "secret123" },
            { "token", "bearer_token" },
            { "api_key", "api_key_value" }
        };

        var exception = new ProcoreCoreException("Authentication failed", "AUTH_ERROR", sensitiveData);

        // Act
        _logger.LogError(exception, "Authentication error occurred");

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        var errorEntry = logEntries.First(e => e.Level == LogLevel.Error);
        
        var logContent = errorEntry.ToString();
        logContent.Should().NotContain("secret123");
        logContent.Should().NotContain("bearer_token");
        logContent.Should().NotContain("api_key_value");
    }
}
```

### 4.2 Performance Logging Tests

**Test Class**: `PerformanceLoggingTests`

```csharp
/// <summary>
/// Tests performance logging for requests and resilience patterns.
/// </summary>
public class PerformanceLoggingTests
{
    [Fact]
    public async Task Should_Log_Request_Performance_Metrics()
    {
        // Arrange
        var telemetryData = new List<(string Operation, TimeSpan Duration, bool Success)>();
        
        var policy = Policy.WrapAsync(
            Policy.Handle<Exception>().WaitAndRetryAsync(0, _ => TimeSpan.Zero),
            Policy.TimeoutAsync(TimeSpan.FromSeconds(10)));

        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            await Task.Delay(100); // Simulate request time
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = CreateHttpClient(mockHandler, policy, telemetryData);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        telemetryData.Should().ContainSingle();
        
        var (operation, duration, success) = telemetryData.First();
        operation.Should().Be("GET /rest/v1.0/companies");
        duration.Should().BeCloseTo(stopwatch.Elapsed, TimeSpan.FromMilliseconds(50));
        success.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Log_Retry_Policy_Performance_Impact()
    {
        // Arrange
        var performanceMetrics = new Dictionary<string, TimeSpan>();
        var requestCount = 0;
        
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(50),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    performanceMetrics[$"Retry_{retryCount}_Delay"] = timespan;
                });

        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            return requestCount switch
            {
                <= 2 => throw new HttpRequestException("Temporary failure"),
                3 => new HttpResponseMessage(HttpStatusCode.OK)
            };
        });

        var client = CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(100); // Should include retry delays
        
        performanceMetrics.Should().ContainKey("Retry_1_Delay");
        performanceMetrics.Should().ContainKey("Retry_2_Delay");
        performanceMetrics.Values.Should().AllSatisfy(delay => 
            delay.Should().BeCloseTo(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(10)));
    }
}
```

## 5. Performance Impact Tests

### 5.1 Resilience Pattern Overhead Tests

**Test Class**: `ResilienceOverheadTests`

```csharp
/// <summary>
/// Tests that resilience patterns don't significantly impact performance during normal operations.
/// </summary>
public class ResilienceOverheadTests
{
    [Fact]
    public async Task Retry_Policy_Should_Have_Minimal_Overhead_For_Successful_Requests()
    {
        // Arrange
        const int requestCount = 100;
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100));

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var clientWithRetry = CreateHttpClient(mockHandler, retryPolicy);
        var clientWithoutRetry = CreateHttpClient(new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)));

        // Act
        var withRetryTimes = new List<long>();
        var withoutRetryTimes = new List<long>();

        for (int i = 0; i < requestCount; i++)
        {
            // Test with retry policy
            var sw1 = Stopwatch.StartNew();
            await clientWithRetry.GetAsync("/rest/v1.0/companies");
            sw1.Stop();
            withRetryTimes.Add(sw1.ElapsedMilliseconds);

            // Test without retry policy
            var sw2 = Stopwatch.StartNew();
            await clientWithoutRetry.GetAsync("/rest/v1.0/companies");
            sw2.Stop();
            withoutRetryTimes.Add(sw2.ElapsedMilliseconds);
        }

        // Assert
        var avgWithRetry = withRetryTimes.Average();
        var avgWithoutRetry = withoutRetryTimes.Average();
        var overhead = (avgWithRetry - avgWithoutRetry) / avgWithoutRetry * 100;

        overhead.Should().BeLessThan(10); // Less than 10% overhead
        _logger.LogInformation("Retry policy overhead: {Overhead:F2}%", overhead);
    }

    [Fact]
    public async Task Circuit_Breaker_Should_Have_Minimal_Overhead_When_Closed()
    {
        // Arrange
        const int requestCount = 100;
        var circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var clientWithCircuitBreaker = CreateHttpClient(mockHandler, circuitBreaker);
        var clientWithoutCircuitBreaker = CreateHttpClient(new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)));

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
        var overhead = (avgWithCircuitBreaker - avgWithoutCircuitBreaker) / avgWithoutCircuitBreaker * 100;

        overhead.Should().BeLessThan(5); // Less than 5% overhead
        _logger.LogInformation("Circuit breaker overhead: {Overhead:F2}%", overhead);
    }

    [Fact]
    public async Task Combined_Policies_Should_Have_Acceptable_Overhead()
    {
        // Arrange
        const int requestCount = 50;
        
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(30));
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100));
        var circuitBreaker = Policy.Handle<Exception>().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        var combinedPolicy = Policy.WrapAsync(circuitBreaker, retryPolicy, timeoutPolicy);

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var clientWithPolicies = CreateHttpClient(mockHandler, combinedPolicy);
        var clientWithoutPolicies = CreateHttpClient(new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK)));

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
        var overhead = (avgWithPolicies - avgWithoutPolicies) / avgWithoutPolicies * 100;

        overhead.Should().BeLessThan(20); // Less than 20% overhead for combined policies
        _logger.LogInformation("Combined policies overhead: {Overhead:F2}%", overhead);
    }
}
```

### 5.2 Memory Usage Tests

**Test Class**: `MemoryUsageTests`

```csharp
/// <summary>
/// Tests that resilience patterns don't cause memory leaks or excessive allocations.
/// </summary>
public class MemoryUsageTests
{
    [Fact]
    public async Task Retry_Policy_Should_Not_Cause_Memory_Leaks()
    {
        // Arrange
        const int iterationCount = 1000;
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(2, _ => TimeSpan.FromMilliseconds(1));

        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var client = CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var initialMemory = GC.GetTotalMemory(true);
        
        for (int i = 0; i < iterationCount; i++)
        {
            await client.GetAsync("/rest/v1.0/companies");
            
            if (i % 100 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        var finalMemory = GC.GetTotalMemory(true);

        // Assert
        var memoryIncrease = finalMemory - initialMemory;
        var memoryIncreasePerRequest = memoryIncrease / (double)iterationCount;

        memoryIncreasePerRequest.Should().BeLessThan(1024); // Less than 1KB per request
        _logger.LogInformation("Memory increase per request: {MemoryIncrease} bytes", memoryIncreasePerRequest);
    }

    [Fact]
    public async Task Circuit_Breaker_Should_Dispose_Resources_Properly()
    {
        // Arrange
        var circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(1, TimeSpan.FromMilliseconds(100));

        var disposableHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Test failure"));

        var client = CreateHttpClient(disposableHandler, circuitBreaker);

        // Act
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Trigger circuit breaker to open state
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<CircuitBreakerOpenException>();

        // Dispose client
        client.Dispose();

        // Assert
        disposableHandler.IsDisposed.Should().BeTrue();
    }
}
```

## Implementation Guidelines

### Test Helper Classes

**TestHttpMessageHandler**:
```csharp
public class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handlerFunc;
    public bool IsDisposed { get; private set; }

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handlerFunc)
        : this(request => Task.FromResult(handlerFunc(request)))
    {
    }

    public TestHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
    {
        _handlerFunc = handlerFunc;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        return await _handlerFunc(request);
    }

    protected override void Dispose(bool disposing)
    {
        IsDisposed = true;
        base.Dispose(disposing);
    }
}
```

**TestLoggerProvider**:
```csharp
public class TestLoggerProvider : ILoggerProvider
{
    private readonly List<LogEntry> _logEntries = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(_logEntries);
    }

    public List<LogEntry> GetLogEntries() => _logEntries.ToList();

    public void Dispose() { }
}

public class LogEntry
{
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? State { get; set; }
    public Exception? Exception { get; set; }
}
```

### Test Configuration

**Recommended Test Project Structure**:
```
tests/Procore.SDK.Resilience.Tests/
├── Polly/
│   ├── RetryPolicyTests.cs
│   ├── CircuitBreakerTests.cs
│   ├── TimeoutPolicyTests.cs
│   └── PolicyWrapTests.cs
├── Exceptions/
│   ├── CustomExceptionTests.cs
│   └── ExceptionContextTests.cs
├── Resilience/
│   ├── FailureScenarioTests.cs
│   └── RecoveryBehaviorTests.cs
├── Logging/
│   ├── ErrorLoggingTests.cs
│   └── PerformanceLoggingTests.cs
├── Performance/
│   ├── ResilienceOverheadTests.cs
│   └── MemoryUsageTests.cs
└── Helpers/
    ├── TestHttpMessageHandler.cs
    ├── TestLoggerProvider.cs
    └── PolicyFactory.cs
```

### Performance Benchmarks

**Target Performance Metrics**:
- Retry policy overhead: < 10% for successful requests
- Circuit breaker overhead: < 5% when closed
- Combined policies overhead: < 20%
- Memory increase: < 1KB per request
- Timeout accuracy: ±50ms

### Integration with CI/CD

**Test Categories**:
- `Unit`: Fast-running unit tests (< 1 second each)
- `Integration`: Integration tests with real failures (< 30 seconds each)
- `Performance`: Performance and memory tests (< 5 minutes each)
- `Chaos`: Controlled chaos engineering tests (manual execution)

**Example CI Configuration**:
```yaml
- name: Run Unit Tests
  run: dotnet test --filter Category=Unit --logger trx --collect:"XPlat Code Coverage"
  
- name: Run Integration Tests
  run: dotnet test --filter Category=Integration --logger trx
  
- name: Run Performance Tests
  run: dotnet test --filter Category=Performance --logger trx
  if: github.event_name == 'pull_request'
```

This comprehensive test plan ensures that the enhanced error handling and resilience patterns are thoroughly validated, perform well under load, and maintain production readiness while providing clear feedback about failures and recovery behaviors.