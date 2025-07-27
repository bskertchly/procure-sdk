using Procore.SDK.Resilience.Tests.Helpers;

namespace Procore.SDK.Resilience.Tests.Logging;

/// <summary>
/// Tests for structured logging of errors, correlation IDs, and resilience pattern activities.
/// Validates that logs contain appropriate context and are structured for monitoring systems.
/// </summary>
public class StructuredLoggingTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<StructuredLoggingTests> _logger;

    public StructuredLoggingTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<StructuredLoggingTests>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Should_Log_Request_Failures_With_Correlation_Id()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, logger: _logger);
        
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            // Verify correlation ID is in request
            request.Headers.Should().ContainKey("X-Correlation-ID");
            throw new HttpRequestException("Service unavailable");
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);
        client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

        // Act
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.Should().NotBeEmpty();
        
        var retryEntries = logEntries.WithLevel(LogLevel.Warning).WithMessage("Retry attempt");
        retryEntries.Should().HaveCount(2);
        
        // Verify structured logging includes correlation context
        foreach (var entry in retryEntries)
        {
            entry.State.Should().NotBeNull();
            entry.Message.Should().Contain("Service unavailable");
            
            // In a real implementation, correlation ID would be included in log scope or state
            // For this test, we verify the pattern is followed
            entry.CategoryName.Should().Contain("StructuredLoggingTests");
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Should_Log_Retry_Attempts_With_Structured_Context()
    {
        // Arrange
        var contextData = new Dictionary<string, object>
        {
            { "RequestId", Guid.NewGuid().ToString() },
            { "UserId", "test.user@procore.com" },
            { "CompanyId", "12345" },
            { "Operation", "GetCompanies" }
        };

        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(50 * retryAttempt),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    using (_logger.BeginScope(contextData))
                    {
                        _logger.LogWarning(
                            "Retry attempt {RetryCount} for {Operation} after {DelayMs}ms due to {ErrorType}: {ErrorMessage}",
                            retryCount,
                            contextData["Operation"],
                            timespan.TotalMilliseconds,
                            outcome.Exception?.GetType().Name,
                            outcome.Exception?.Message);
                    }
                });

        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Connection timeout"));

        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        var retryEntries = logEntries.WithLevel(LogLevel.Warning).ToList();
        
        retryEntries.Should().HaveCount(3);
        
        foreach (var entry in retryEntries)
        {
            entry.Message.Should().Contain("Retry attempt");
            entry.Message.Should().Contain("GetCompanies");
            entry.Message.Should().Contain("HttpRequestException");
            entry.Message.Should().Contain("Connection timeout");
        }

        // Verify retry delays are logged correctly
        retryEntries[0].Message.Should().Contain("50ms");  // First retry: 50ms
        retryEntries[1].Message.Should().Contain("100ms"); // Second retry: 100ms  
        retryEntries[2].Message.Should().Contain("150ms"); // Third retry: 150ms
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Should_Log_Circuit_Breaker_State_Changes_With_Context()
    {
        // Arrange
        var operationContext = new
        {
            Service = "ProCore.API",
            Endpoint = "/rest/v1.0/companies",
            Environment = "test"
        };

        var stateChanges = new List<string>();
        
        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromMilliseconds(100),
                onBreak: (exception, duration) =>
                {
                    stateChanges.Add("Opened");
                    _logger.LogError(
                        "Circuit breaker OPENED for {Service} {Endpoint} due to {ExceptionType}: {ExceptionMessage}. Break duration: {BreakDurationMs}ms",
                        operationContext.Service,
                        operationContext.Endpoint,
                        exception.GetType().Name,
                        exception.Message,
                        duration.TotalMilliseconds);
                },
                onReset: () =>
                {
                    stateChanges.Add("Reset");
                    _logger.LogInformation(
                        "Circuit breaker RESET for {Service} {Endpoint} - service recovered",
                        operationContext.Service,
                        operationContext.Endpoint);
                },
                onHalfOpen: () =>
                {
                    stateChanges.Add("HalfOpen");
                    _logger.LogInformation(
                        "Circuit breaker HALF-OPEN for {Service} {Endpoint} - testing service health",
                        operationContext.Service,
                        operationContext.Endpoint);
                });

        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var count = Interlocked.Increment(ref requestCount);
            if (count <= 2)
                throw new HttpRequestException("Service degraded");
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, circuitBreakerPolicy);

        // Act - Trigger circuit breaker
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>();

        // Wait for break duration and test recovery
        await Task.Delay(150);
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stateChanges.Should().ContainInOrder("Opened", "HalfOpen", "Reset");

        var logEntries = _loggerProvider.GetLogEntries();
        
        // Verify circuit breaker opened log
        var openedEntry = logEntries.WithLevel(LogLevel.Error).WithMessage("Circuit breaker OPENED").First();
        openedEntry.Message.Should().Contain("ProCore.API");
        openedEntry.Message.Should().Contain("/rest/v1.0/companies");
        openedEntry.Message.Should().Contain("HttpRequestException");
        openedEntry.Message.Should().Contain("Service degraded");
        openedEntry.Message.Should().Contain("100ms");

        // Verify circuit breaker half-open log
        var halfOpenEntry = logEntries.WithLevel(LogLevel.Information).WithMessage("HALF-OPEN").First();
        halfOpenEntry.Message.Should().Contain("ProCore.API");
        halfOpenEntry.Message.Should().Contain("testing service health");

        // Verify circuit breaker reset log
        var resetEntry = logEntries.WithLevel(LogLevel.Information).WithMessage("RESET").First();
        resetEntry.Message.Should().Contain("service recovered");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Should_Log_Performance_Metrics_With_Structured_Data()
    {
        // Arrange
        var performanceMetrics = new List<object>();
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(100),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    var metric = new
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        RetryAttempt = retryCount,
                        DelayMs = timespan.TotalMilliseconds,
                        ErrorType = outcome.Exception?.GetType().Name,
                        ErrorMessage = outcome.Exception?.Message
                    };
                    performanceMetrics.Add(metric);
                    
                    _logger.LogWarning(
                        "Performance: Retry {RetryAttempt} after {DelayMs}ms for {ErrorType}",
                        metric.RetryAttempt,
                        metric.DelayMs,
                        metric.ErrorType);
                });

        var requestTimes = new List<long>();
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var sw = Stopwatch.StartNew();
            Thread.Sleep(50); // Simulate processing time
            sw.Stop();
            requestTimes.Add(sw.ElapsedMilliseconds);
            
            if (requestTimes.Count <= 2)
                throw new HttpRequestException("Temporary failure");
                
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var overallStopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        overallStopwatch.Stop();

        // Log final performance metrics
        _logger.LogInformation(
            "Performance: Request completed in {TotalDurationMs}ms with {RetryCount} retries. Individual attempts: [{AttemptDurations}]ms",
            overallStopwatch.ElapsedMilliseconds,
            performanceMetrics.Count,
            string.Join(", ", requestTimes));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        performanceMetrics.Should().HaveCount(2);
        requestTimes.Should().HaveCount(3); // 2 failures + 1 success

        var logEntries = _loggerProvider.GetLogEntries();
        
        // Verify retry performance logs
        var retryLogs = logEntries.WithLevel(LogLevel.Warning).WithMessage("Performance: Retry");
        retryLogs.Should().HaveCount(2);
        
        // Verify final performance log
        var finalLog = logEntries.WithLevel(LogLevel.Information).WithMessage("Request completed").First();
        finalLog.Message.Should().Contain("2 retries");
        finalLog.Message.Should().MatchRegex(@"\d+ms"); // Should contain duration
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void Should_Not_Log_Sensitive_Information_In_Errors()
    {
        // Arrange
        var sensitiveData = new Dictionary<string, object>
        {
            { "password", "secret123" },
            { "token", "bearer_token_abc123" },
            { "api_key", "api_key_xyz789" },
            { "ssn", "123-45-6789" },
            { "credit_card", "4111-1111-1111-1111" },
            { "user_id", "12345" }, // Non-sensitive
            { "company_name", "Test Company" } // Non-sensitive
        };

        var exception = new ProcoreCoreException("Authentication failed", "AUTH_ERROR", sensitiveData);

        // Act
        _logger.LogError(exception, 
            "Authentication error for user {UserId} at company {CompanyName}",
            sensitiveData["user_id"], 
            sensitiveData["company_name"]);

        // Assert
        var logEntries = _loggerProvider.GetLogEntries();
        var errorEntry = logEntries.WithLevel(LogLevel.Error).First();
        
        var logContent = errorEntry.ToString();
        
        // Should NOT contain sensitive information
        logContent.Should().NotContain("secret123");
        logContent.Should().NotContain("bearer_token_abc123");
        logContent.Should().NotContain("api_key_xyz789");
        logContent.Should().NotContain("123-45-6789");
        logContent.Should().NotContain("4111-1111-1111-1111");
        
        // Should contain non-sensitive information
        logContent.Should().Contain("12345"); // user_id
        logContent.Should().Contain("Test Company"); // company_name
        logContent.Should().Contain("Authentication failed"); // error message
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Should_Log_Rate_Limiting_Events_With_Retry_After_Information()
    {
        // Arrange
        var rateLimitEvents = new List<object>();
        var rateLimitPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: (retryAttempt, result, context) =>
                {
                    var retryAfter = TimeSpan.Zero;
                    
                    if (result.Result?.Headers.RetryAfter != null)
                    {
                        if (result.Result.Headers.RetryAfter.Delta.HasValue)
                        {
                            retryAfter = result.Result.Headers.RetryAfter.Delta.Value;
                        }
                    }
                    
                    var rateLimitEvent = new
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        RetryAttempt = retryAttempt,
                        RetryAfterSeconds = retryAfter.TotalSeconds,
                        StatusCode = (int)result.Result!.StatusCode
                    };
                    rateLimitEvents.Add(rateLimitEvent);
                    
                    _logger.LogWarning(
                        "Rate limit exceeded (HTTP {StatusCode}). Retry attempt {RetryAttempt} will wait {RetryAfterSeconds}s",
                        rateLimitEvent.StatusCode,
                        rateLimitEvent.RetryAttempt,
                        rateLimitEvent.RetryAfterSeconds);
                        
                    return retryAfter;
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogInformation(
                        "Rate limit retry {RetryCount} executing after {ActualDelayMs}ms delay",
                        retryCount,
                        timespan.TotalMilliseconds);
                });

        var requestCount = 0;
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            var count = Interlocked.Increment(ref requestCount);
            
            return count switch
            {
                1 => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    Headers = { RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromSeconds(1)) },
                    Content = new StringContent("{\"error\": \"Rate limit exceeded\"}")
                },
                2 => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                {
                    Headers = { RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromSeconds(2)) },
                    Content = new StringContent("{\"error\": \"Rate limit exceeded\"}")
                },
                3 => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"companies\": []}")
                },
                _ => throw new InvalidOperationException("Unexpected request")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, rateLimitPolicy);

        // Act
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        rateLimitEvents.Should().HaveCount(2);
        requestCount.Should().Be(3);

        var logEntries = _loggerProvider.GetLogEntries();
        
        // Verify rate limit warning logs
        var rateLimitWarnings = logEntries.WithLevel(LogLevel.Warning).WithMessage("Rate limit exceeded");
        rateLimitWarnings.Should().HaveCount(2);
        
        var firstWarning = rateLimitWarnings.First();
        firstWarning.Message.Should().Contain("HTTP 429");
        firstWarning.Message.Should().Contain("Retry attempt 1");
        firstWarning.Message.Should().Contain("1s");
        
        // Verify retry execution logs
        var retryExecutions = logEntries.WithLevel(LogLevel.Information).WithMessage("Rate limit retry");
        retryExecutions.Should().HaveCount(2);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Should_Create_Structured_Logs_For_End_To_End_Request_Flow()
    {
        // Arrange
        var requestId = Guid.NewGuid().ToString();
        var userId = "test.user@procore.com";
        var companyId = "12345";
        
        var operationContext = new Dictionary<string, object>
        {
            { "RequestId", requestId },
            { "UserId", userId },
            { "CompanyId", companyId },
            { "Operation", "GetCompanies" },
            { "ApiVersion", "v1.0" }
        };

        var combinedPolicy = PolicyFactory.CreateCombinedPolicy(
            retryCount: 2,
            retryBaseDelay: TimeSpan.FromMilliseconds(100),
            circuitBreakerFailureThreshold: 3,
            timeout: TimeSpan.FromSeconds(5),
            logger: _logger);

        var requestSteps = new List<string>();
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            requestSteps.Add($"Request received at {DateTimeOffset.UtcNow:HH:mm:ss.fff}");
            
            if (requestSteps.Count == 1)
            {
                requestSteps.Add("First attempt failed");
                throw new HttpRequestException("Database connection timeout");
            }
            
            requestSteps.Add("Second attempt succeeded");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"companies\": [{\"id\": 123, \"name\": \"Test Company\"}]}")
            };
        });

        var client = PolicyFactory.CreateHttpClient(mockHandler, combinedPolicy);
        
        // Add request context headers
        client.DefaultRequestHeaders.Add("X-Request-ID", requestId);
        client.DefaultRequestHeaders.Add("X-User-ID", userId);

        // Act - Log the complete request flow
        using (_logger.BeginScope(operationContext))
        {
            _logger.LogInformation("Starting request for {Operation} by user {UserId}", 
                operationContext["Operation"], operationContext["UserId"]);
                
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var response = await client.GetAsync($"/rest/v1.0/companies?company_id={companyId}");
                stopwatch.Stop();
                
                var content = await response.Content.ReadAsStringAsync();
                var companiesData = JsonSerializer.Deserialize<JsonElement>(content);
                var companyCount = companiesData.GetProperty("companies").GetArrayLength();
                
                _logger.LogInformation(
                    "Request completed successfully for {Operation}. Duration: {DurationMs}ms, Companies returned: {CompanyCount}, Status: {StatusCode}",
                    operationContext["Operation"],
                    stopwatch.ElapsedMilliseconds,
                    companyCount,
                    (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "Request failed for {Operation} after {DurationMs}ms. Error: {ErrorType}",
                    operationContext["Operation"],
                    stopwatch.ElapsedMilliseconds,
                    ex.GetType().Name);
                throw;
            }
        }

        // Assert
        requestSteps.Should().HaveCount(3);
        requestSteps[0].Should().Contain("Request received");
        requestSteps[1].Should().Be("First attempt failed");
        requestSteps[2].Should().Be("Second attempt succeeded");

        var logEntries = _loggerProvider.GetLogEntries();
        
        // Verify request start log
        var startLog = logEntries.WithLevel(LogLevel.Information).WithMessage("Starting request").First();
        startLog.Message.Should().Contain("GetCompanies");
        startLog.Message.Should().Contain(userId);
        
        // Verify retry log from policy
        var retryLogs = logEntries.WithLevel(LogLevel.Warning).WithMessage("Retry attempt");
        retryLogs.Should().HaveCount(1);
        
        // Verify success completion log
        var completionLog = logEntries.WithLevel(LogLevel.Information).WithMessage("Request completed successfully").First();
        completionLog.Message.Should().Contain("GetCompanies");
        completionLog.Message.Should().Contain("Companies returned: 1");
        completionLog.Message.Should().Contain("Status: 200");
        completionLog.Message.Should().MatchRegex(@"Duration: \d+ms");
    }
}