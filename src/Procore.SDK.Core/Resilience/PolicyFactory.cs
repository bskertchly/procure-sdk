using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.Resilience;

/// <summary>
/// Factory for creating standardized resilience policies for Procore SDK operations.
/// </summary>
public class PolicyFactory : IDisposable
{
    private readonly ResilienceOptions _options;
    private readonly ILogger<PolicyFactory> _logger;
    private readonly RandomNumberGenerator _random;
    
    // Cache for policies to improve performance
    private readonly ConcurrentDictionary<string, IAsyncPolicy<HttpResponseMessage>> _policyCache;

    /// <summary>
    /// Initializes a new instance of the PolicyFactory class.
    /// </summary>
    /// <param name="options">The resilience configuration options.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    public PolicyFactory(IOptions<ResilienceOptions> options, ILogger<PolicyFactory> logger)
    {
        _options = options.Value;
        _logger = logger;
        _random = RandomNumberGenerator.Create();
        _policyCache = new ConcurrentDictionary<string, IAsyncPolicy<HttpResponseMessage>>();
    }

    /// <summary>
    /// Creates a comprehensive resilience policy combining retry, circuit breaker, and timeout policies.
    /// </summary>
    /// <param name="context">The resilience context for the operation.</param>
    /// <returns>A combined async policy.</returns>
    public IAsyncPolicy<HttpResponseMessage> CreateHttpPolicy(ResilienceContext context)
    {
        // Use cached policy for better performance
        var cacheKey = GeneratePolicyKey(context.Operation);
        return _policyCache.GetOrAdd(cacheKey, _ => CreateCombinedPolicy(context.Operation));
    }

    /// <summary>
    /// Creates a combined policy with all configured resilience strategies.
    /// </summary>
    /// <param name="operation">The operation name for logging context.</param>
    /// <returns>A combined async policy.</returns>
    private IAsyncPolicy<HttpResponseMessage> CreateCombinedPolicy(string operation)
    {
        var policies = new List<IAsyncPolicy<HttpResponseMessage>>();

        // Add timeout policy (innermost)
        if (_options.Timeout.Enabled)
        {
            policies.Add(CreateTimeoutPolicy(operation));
        }

        // Add retry policy
        if (_options.Retry.MaxAttempts > 0)
        {
            policies.Add(CreateRetryPolicy(operation));
        }

        // Add circuit breaker policy (outermost)
        if (_options.CircuitBreaker.Enabled)
        {
            policies.Add(CreateCircuitBreakerPolicy(operation));
        }

        return policies.Count switch
        {
            0 => Policy.NoOpAsync<HttpResponseMessage>(),
            1 => policies[0],
            _ => Policy.WrapAsync(policies.ToArray())
        };
    }

    /// <summary>
    /// Creates a retry policy with exponential backoff and jitter.
    /// </summary>
    private IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(string operation)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(response => ShouldRetry(response.StatusCode))
            .WaitAndRetryAsync(
                retryCount: _options.Retry.MaxAttempts,
                sleepDurationProvider: retryAttempt => CalculateDelay(retryAttempt),
                onRetry: (outcome, timespan, retryCount, pollyContext) =>
                {
                    if (_options.Logging.LogRetryAttempts)
                    {
                        LogRetryAttempt(operation, retryCount, timespan, outcome.Exception);
                    }
                });
    }

    /// <summary>
    /// Creates a circuit breaker policy to prevent cascading failures.
    /// </summary>
    private IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(string operation)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(response => ShouldTripCircuitBreaker(response.StatusCode))
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: _options.CircuitBreaker.FailureThreshold,
                durationOfBreak: TimeSpan.FromSeconds(_options.CircuitBreaker.DurationOfBreakInSeconds),
                onBreak: (result, duration) =>
                {
                    if (_options.Logging.LogCircuitBreakerEvents)
                    {
                        LogCircuitBreakerOpened(operation, result.Exception ?? new Exception("Circuit breaker opened"), duration);
                    }
                },
                onReset: () =>
                {
                    if (_options.Logging.LogCircuitBreakerEvents)
                    {
                        LogCircuitBreakerReset(operation);
                    }
                },
                onHalfOpen: () =>
                {
                    if (_options.Logging.LogCircuitBreakerEvents)
                    {
                        LogCircuitBreakerHalfOpen(operation);
                    }
                });
    }

    /// <summary>
    /// Creates a timeout policy for operations.
    /// </summary>
    private IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(string operation)
    {
        var timeout = TimeSpan.FromSeconds(_options.Timeout.DefaultTimeoutInSeconds);

        return Policy.TimeoutAsync<HttpResponseMessage>(timeout);
    }

    /// <summary>
    /// Generates a cache key for the policy based on operation characteristics.
    /// </summary>
    /// <param name="operation">The operation name.</param>
    /// <returns>A cache key for the policy.</returns>
    private string GeneratePolicyKey(string operation)
    {
        // For now, use a simple hash of the operation and current options
        // This ensures different configurations get different cached policies
        var optionsHash = _options.GetHashCode();
        return $"{operation}_{optionsHash}";
    }

    /// <summary>
    /// Calculates the delay for a retry attempt using exponential backoff and jitter.
    /// </summary>
    private TimeSpan CalculateDelay(int retryAttempt)
    {
        var baseDelay = TimeSpan.FromMilliseconds(_options.Retry.BaseDelayMs);

        if (_options.Retry.UseExponentialBackoff)
        {
            var exponentialDelay = TimeSpan.FromMilliseconds(
                _options.Retry.BaseDelayMs * Math.Pow(_options.Retry.BackoffMultiplier, retryAttempt - 1));

            baseDelay = exponentialDelay;
        }

        // Apply maximum delay constraint
        var maxDelay = TimeSpan.FromMilliseconds(_options.Retry.MaxDelayMs);
        if (baseDelay > maxDelay)
        {
            baseDelay = maxDelay;
        }

        // Add jitter to prevent thundering herd
        if (_options.Retry.UseJitter && _options.Retry.MaxJitterMs > 0)
        {
            byte[] randomBytes = new byte[4];
            _random.GetBytes(randomBytes);
            int jitterValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % _options.Retry.MaxJitterMs;
            TimeSpan jitter = TimeSpan.FromMilliseconds(jitterValue);
            baseDelay = baseDelay.Add(jitter);
        }

        return baseDelay;
    }

    /// <summary>
    /// Determines if an HTTP status code should trigger a retry.
    /// </summary>
    private static bool ShouldRetry(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.RequestTimeout => true,
            HttpStatusCode.TooManyRequests => true,
            HttpStatusCode.InternalServerError => true,
            HttpStatusCode.BadGateway => true,
            HttpStatusCode.ServiceUnavailable => true,
            HttpStatusCode.GatewayTimeout => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if an HTTP status code should trip the circuit breaker.
    /// </summary>
    private static bool ShouldTripCircuitBreaker(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.InternalServerError => true,
            HttpStatusCode.BadGateway => true,
            HttpStatusCode.ServiceUnavailable => true,
            HttpStatusCode.GatewayTimeout => true,
            _ => false
        };
    }

    private void LogRetryAttempt(string operation, int retryCount, TimeSpan delay, Exception? exception)
    {
        _logger.LogWarning(
            "Retry attempt {RetryCount} for operation {Operation} " +
            "after {DelayMs}ms delay. Exception: {ExceptionType}: {ExceptionMessage}",
            retryCount,
            operation,
            delay.TotalMilliseconds,
            exception?.GetType().Name,
            exception?.Message);
    }

    private void LogCircuitBreakerOpened(string operation, Exception exception, TimeSpan duration)
    {
        _logger.LogError(
            "Circuit breaker opened for operation {Operation} " +
            "due to {ExceptionType}: {ExceptionMessage}. Duration: {DurationSeconds}s",
            operation,
            exception.GetType().Name,
            exception.Message,
            duration.TotalSeconds);
    }

    private void LogCircuitBreakerReset(string operation)
    {
        _logger.LogInformation(
            "Circuit breaker reset for operation {Operation} - service recovered",
            operation);
    }

    private void LogCircuitBreakerHalfOpen(string operation)
    {
        _logger.LogInformation(
            "Circuit breaker half-open for operation {Operation} - testing service",
            operation);
    }

    /// <summary>
    /// Disposes the RandomNumberGenerator to free resources.
    /// </summary>
    public void Dispose()
    {
        _random?.Dispose();
        GC.SuppressFinalize(this);
    }
}