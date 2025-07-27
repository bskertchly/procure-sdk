using Procore.SDK.Resilience.Tests.Helpers;

namespace Procore.SDK.Resilience.Tests.Polly;

/// <summary>
/// Tests for retry policies including exponential backoff and jittered retry patterns.
/// These tests validate that retries occur with proper intervals and eventually succeed or fail appropriately.
/// </summary>
public class RetryPolicyTests
{
    private readonly TestLoggerProvider _loggerProvider;
    private readonly ILogger<RetryPolicyTests> _logger;

    public RetryPolicyTests()
    {
        _loggerProvider = new TestLoggerProvider();
        var loggerFactory = new LoggerFactory(new[] { _loggerProvider });
        _logger = loggerFactory.CreateLogger<RetryPolicyTests>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetryPolicy_Should_Retry_On_Transient_HttpRequestException()
    {
        // Arrange
        var mockHandler = TestHttpMessageHandler.CreateFailThenSucceedHandler(2, HttpStatusCode.InternalServerError);
        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 3, baseDelay: TimeSpan.FromMilliseconds(100), logger: _logger);
        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await client.GetAsync("/rest/v1.0/companies");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mockHandler.SentRequests.Should().HaveCount(3); // 2 failures + 1 success
        stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(300); // At least 100 + 200ms delays

        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).Should().HaveCount(2);
        logEntries.Should().Contain(entry => entry.Message.Contains("Retry attempt 1"));
        logEntries.Should().Contain(entry => entry.Message.Contains("Retry attempt 2"));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetryPolicy_Should_Not_Retry_On_Non_Transient_Errors()
    {
        // Arrange
        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.Forbidden)); // Non-transient error

        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 3, logger: _logger);
        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var response = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        mockHandler.SentRequests.Should().HaveCount(1); // Only one attempt, no retries

        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).Should().BeEmpty(); // No retry warnings
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetryPolicy_Should_Handle_Timeout_Scenarios()
    {
        // Arrange
        var timeoutCount = 0;
        var mockHandler = new TestHttpMessageHandler(async request =>
        {
            Interlocked.Increment(ref timeoutCount);
            if (timeoutCount <= 2)
            {
                await Task.Delay(2000); // Simulate slow response
                throw new TaskCanceledException("Request timeout");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 3, baseDelay: TimeSpan.FromMilliseconds(50), logger: _logger);
        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var result = await client.GetAsync("/rest/v1.0/companies");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        timeoutCount.Should().Be(3); // 2 timeouts + 1 success
        mockHandler.SentRequests.Should().HaveCount(3);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetryPolicy_Should_Respect_Maximum_Retry_Count()
    {
        // Arrange
        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Persistent failure"));

        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, baseDelay: TimeSpan.FromMilliseconds(10), logger: _logger);
        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act & Assert
        await client.Invoking(c => c.GetAsync("/rest/v1.0/companies"))
            .Should().ThrowAsync<HttpRequestException>()
            .WithMessage("Persistent failure");

        mockHandler.SentRequests.Should().HaveCount(3); // Initial + 2 retries
        
        var logEntries = _loggerProvider.GetLogEntries();
        logEntries.WithLevel(LogLevel.Warning).Should().HaveCount(2); // Two retry attempts
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task JitteredRetry_Should_Vary_Delays_Between_Concurrent_Requests()
    {
        // Arrange
        var delays = new ConcurrentBag<TimeSpan>();
        var retryPolicy = PolicyFactory.CreateJitteredRetryPolicy(
            retryCount: 2, 
            baseDelay: TimeSpan.FromMilliseconds(100), 
            jitterFactor: 0.2,
            logger: _logger);

        var mockHandler = new TestHttpMessageHandler(request =>
            throw new HttpRequestException("Temporary failure"));

        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                await client.GetAsync("/rest/v1.0/companies");
                stopwatch.Stop();
                delays.Add(stopwatch.Elapsed);
            }
            catch (HttpRequestException)
            {
                // Expected to fail after retries
            }
        });

        await Task.WhenAll(tasks);

        // Assert
        delays.Should().HaveCount(10);
        
        // Should have jitter variation - delays shouldn't all be identical
        var uniqueDelays = delays.Select(d => (long)(d.TotalMilliseconds / 10) * 10).Distinct().Count();
        uniqueDelays.Should().BeGreaterThan(3); // Should have some variation due to jitter
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetryPolicy_Should_Use_Exponential_Backoff()
    {
        // Arrange
        var attemptTimes = new List<DateTimeOffset>();
        var mockHandler = new TestHttpMessageHandler(request =>
        {
            attemptTimes.Add(DateTimeOffset.UtcNow);
            throw new HttpRequestException("Failure for backoff test");
        });

        var retryPolicy = PolicyFactory.CreateRetryPolicy(
            retryCount: 3, 
            baseDelay: TimeSpan.FromMilliseconds(100), 
            logger: _logger);
        var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

        // Act
        try
        {
            await client.GetAsync("/rest/v1.0/companies");
        }
        catch (HttpRequestException)
        {
            // Expected to fail
        }

        // Assert
        attemptTimes.Should().HaveCount(4); // Initial + 3 retries
        
        // Check exponential backoff intervals
        var interval1 = attemptTimes[1] - attemptTimes[0];
        var interval2 = attemptTimes[2] - attemptTimes[1];
        var interval3 = attemptTimes[3] - attemptTimes[2];

        interval1.Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(50));
        interval2.Should().BeCloseTo(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(50));
        interval3.Should().BeCloseTo(TimeSpan.FromMilliseconds(400), TimeSpan.FromMilliseconds(50));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task RetryPolicy_Should_Preserve_Successful_Response_Content()
    {
        // Arrange
        var expectedContent = "{\"companies\": [{\"id\": 123, \"name\": \"Test Company\"}]}";
        var mockHandler = TestHttpMessageHandler.CreateFailThenSucceedHandler(1);
        
        // Override the success response to include specific content
        var requestCount = 0;
        var contentHandler = new TestHttpMessageHandler(request =>
        {
            requestCount++;
            if (requestCount == 1)
            {
                throw new HttpRequestException("First attempt fails");
            }
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedContent, System.Text.Encoding.UTF8, "application/json")
            };
        });

        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 2, logger: _logger);
        var client = PolicyFactory.CreateHttpClient(contentHandler, retryPolicy);

        // Act
        var response = await client.GetAsync("/rest/v1.0/companies");
        var actualContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actualContent.Should().Be(expectedContent);
        requestCount.Should().Be(2); // One failure + one success
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task RetryPolicy_Should_Handle_Real_Network_Conditions()
    {
        // Arrange - Simulate realistic network conditions
        var networkConditions = new[]
        {
            (HttpStatusCode.RequestTimeout, "Request timeout"),
            (HttpStatusCode.BadGateway, "Bad gateway"),
            (HttpStatusCode.ServiceUnavailable, "Service unavailable"),
            (HttpStatusCode.GatewayTimeout, "Gateway timeout")
        };

        foreach (var (statusCode, reason) in networkConditions)
        {
            var mockHandler = TestHttpMessageHandler.CreateStatusCodeSequenceHandler(
                statusCode, statusCode, HttpStatusCode.OK);
            
            var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 3, logger: _logger);
            var client = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);

            // Act
            var response = await client.GetAsync("/rest/v1.0/companies");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, 
                $"should recover from {statusCode} ({reason})");
            mockHandler.SentRequests.Should().HaveCount(3, 
                $"should make 3 attempts for {statusCode}");
        }
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task RetryPolicy_Should_Have_Minimal_Overhead_For_Successful_Requests()
    {
        // Arrange
        const int requestCount = 100;
        var mockHandler = new TestHttpMessageHandler(request =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var retryPolicy = PolicyFactory.CreateRetryPolicy(retryCount: 3, logger: _logger);
        var clientWithRetry = PolicyFactory.CreateHttpClient(mockHandler, retryPolicy);
        var clientWithoutRetry = PolicyFactory.CreateHttpClient(new TestHttpMessageHandler(request =>
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
        var overhead = avgWithoutRetry > 0 ? (avgWithRetry - avgWithoutRetry) / avgWithoutRetry * 100 : 0;

        overhead.Should().BeLessThan(15); // Less than 15% overhead
        _logger.LogInformation("Retry policy overhead: {Overhead:F2}%", overhead);
    }
}