namespace Procore.SDK.Resilience.Tests.Helpers;

/// <summary>
/// Factory for creating test policies with predefined configurations.
/// </summary>
public static class PolicyFactory
{
    /// <summary>
    /// Creates a basic retry policy for testing.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(
        int retryCount = 3,
        TimeSpan? baseDelay = null,
        ILogger? logger = null)
    {
        var delay = baseDelay ?? TimeSpan.FromMilliseconds(100);
        
        return Policy
            .HandleResult<HttpResponseMessage>(r => !IsSuccessStatusCode(r.StatusCode))
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: retryCount,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(delay.TotalMilliseconds * Math.Pow(2, retryAttempt - 1)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger?.LogWarning("Retry attempt {RetryCount} after {Delay}ms due to {Reason}",
                        retryCount, timespan.TotalMilliseconds, GetFailureReason(outcome));
                });
    }

    /// <summary>
    /// Creates a circuit breaker policy for testing.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(
        int handledEventsAllowedBeforeBreaking = 3,
        TimeSpan? durationOfBreak = null,
        ILogger? logger = null)
    {
        var breakDuration = durationOfBreak ?? TimeSpan.FromSeconds(30);
        
        return Policy
            .HandleResult<HttpResponseMessage>(r => !IsSuccessStatusCode(r.StatusCode))
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: handledEventsAllowedBeforeBreaking,
                durationOfBreak: breakDuration,
                onBreak: (result, duration) =>
                {
                    logger?.LogError("Circuit breaker opened for {Duration} due to {Reason}",
                        duration, GetFailureReason(result));
                },
                onReset: () =>
                {
                    logger?.LogInformation("Circuit breaker reset - service recovered");
                },
                onHalfOpen: () =>
                {
                    logger?.LogInformation("Circuit breaker half-open - testing service");
                });
    }

    /// <summary>
    /// Creates a timeout policy for testing.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(
        TimeSpan timeout,
        ILogger? logger = null)
    {
        return Policy
            .TimeoutAsync<HttpResponseMessage>(
                timeout: timeout,
                onTimeout: (context, timeout, task) =>
                {
                    logger?.LogWarning("Request timed out after {Timeout}", timeout);
                    return Task.CompletedTask;
                });
    }

    /// <summary>
    /// Creates a combined policy with retry, circuit breaker, and timeout.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateCombinedPolicy(
        int retryCount = 3,
        TimeSpan? retryBaseDelay = null,
        int circuitBreakerFailureThreshold = 5,
        TimeSpan? circuitBreakerDuration = null,
        TimeSpan? timeout = null,
        ILogger? logger = null)
    {
        var timeoutPolicy = timeout.HasValue 
            ? CreateTimeoutPolicy(timeout.Value, logger)
            : null;
            
        var retryPolicy = CreateRetryPolicy(retryCount, retryBaseDelay, logger);
        
        var circuitBreakerPolicy = CreateCircuitBreakerPolicy(
            circuitBreakerFailureThreshold, 
            circuitBreakerDuration, 
            logger);

        if (timeoutPolicy != null)
        {
            return Policy.WrapAsync(circuitBreakerPolicy, retryPolicy, timeoutPolicy);
        }
        
        return Policy.WrapAsync(circuitBreakerPolicy, retryPolicy);
    }

    /// <summary>
    /// Creates a rate limiting retry policy that respects Retry-After headers.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateRateLimitPolicy(ILogger? logger = null)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: (retryAttempt, result, context) =>
                {
                    // Try to get Retry-After header
                    if (result.Result?.Headers.RetryAfter != null)
                    {
                        if (result.Result.Headers.RetryAfter.Delta.HasValue)
                        {
                            return result.Result.Headers.RetryAfter.Delta.Value;
                        }
                        
                        if (result.Result.Headers.RetryAfter.Date.HasValue)
                        {
                            var retryAfter = result.Result.Headers.RetryAfter.Date.Value - DateTimeOffset.UtcNow;
                            return retryAfter > TimeSpan.Zero ? retryAfter : TimeSpan.FromSeconds(1);
                        }
                    }
                    
                    // Fallback to exponential backoff
                    return TimeSpan.FromMilliseconds(1000 * Math.Pow(2, retryAttempt - 1));
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger?.LogWarning("Rate limit exceeded, retrying in {Delay}ms (attempt {RetryCount})",
                        timespan.TotalMilliseconds, retryCount);
                });
    }

    /// <summary>
    /// Creates a jittered retry policy to prevent thundering herd problems.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CreateJitteredRetryPolicy(
        int retryCount = 3,
        TimeSpan? baseDelay = null,
        double jitterFactor = 0.1,
        ILogger? logger = null)
    {
        var delay = baseDelay ?? TimeSpan.FromMilliseconds(1000);
        var random = new Random();
        
        return Policy
            .HandleResult<HttpResponseMessage>(r => !IsSuccessStatusCode(r.StatusCode))
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: retryCount,
                sleepDurationProvider: retryAttempt =>
                {
                    var baseDelayMs = delay.TotalMilliseconds * Math.Pow(2, retryAttempt - 1);
                    var jitterMs = baseDelayMs * jitterFactor * (random.NextDouble() * 2 - 1); // ±jitterFactor
                    return TimeSpan.FromMilliseconds(Math.Max(0, baseDelayMs + jitterMs));
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger?.LogWarning("Jittered retry attempt {RetryCount} after {Delay}ms",
                        retryCount, timespan.TotalMilliseconds);
                });
    }

    /// <summary>
    /// Creates an HTTP client with the specified handler and policy.
    /// </summary>
    public static HttpClient CreateHttpClient(
        HttpMessageHandler handler, 
        IAsyncPolicy<HttpResponseMessage>? policy = null)
    {
        var client = policy != null 
            ? new HttpClient(new PolicyHttpMessageHandler(policy) { InnerHandler = handler })
            : new HttpClient(handler);
            
        client.BaseAddress = new Uri("https://api.procore.com");
        return client;
    }

    private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
    {
        return ((int)statusCode >= 200) && ((int)statusCode <= 299);
    }

    private static string GetFailureReason(DelegateResult<HttpResponseMessage> outcome)
    {
        if (outcome.Exception != null)
        {
            return outcome.Exception.Message;
        }
        
        if (outcome.Result != null)
        {
            return $"HTTP {(int)outcome.Result.StatusCode} {outcome.Result.StatusCode}";
        }
        
        return "Unknown failure";
    }
}

/// <summary>
/// HTTP message handler that applies a Polly policy to requests.
/// </summary>
public class PolicyHttpMessageHandler : DelegatingHandler
{
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public PolicyHttpMessageHandler(IAsyncPolicy<HttpResponseMessage> policy)
    {
        _policy = policy ?? throw new ArgumentNullException(nameof(policy));
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        return await _policy.ExecuteAsync(async () =>
        {
            return await base.SendAsync(request, cancellationToken);
        });
    }
}