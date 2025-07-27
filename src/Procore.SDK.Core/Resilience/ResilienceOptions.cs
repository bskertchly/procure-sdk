using System;
using System.ComponentModel.DataAnnotations;

namespace Procore.SDK.Core.Resilience;

/// <summary>
/// Configuration options for resilience policies.
/// </summary>
public class ResilienceOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "Procore:Resilience";

    /// <summary>
    /// Retry policy configuration.
    /// </summary>
    public RetryOptions Retry { get; set; } = new();
    
    /// <summary>
    /// Circuit breaker policy configuration.
    /// </summary>
    public CircuitBreakerOptions CircuitBreaker { get; set; } = new();
    
    /// <summary>
    /// Timeout policy configuration.
    /// </summary>
    public TimeoutOptions Timeout { get; set; } = new();
    
    /// <summary>
    /// Logging configuration for resilience events.
    /// </summary>
    public LoggingOptions Logging { get; set; } = new();
}

/// <summary>
/// Retry policy configuration options.
/// </summary>
public class RetryOptions
{
    /// <summary>
    /// Maximum number of retry attempts (default: 3).
    /// </summary>
    [Range(0, 10)]
    public int MaxAttempts { get; set; } = 3;
    
    /// <summary>
    /// Base delay between retries in milliseconds (default: 1000ms).
    /// </summary>
    [Range(100, 60000)]
    public int BaseDelayMs { get; set; } = 1000;
    
    /// <summary>
    /// Maximum delay between retries in milliseconds (default: 30000ms).
    /// </summary>
    [Range(1000, 300000)]
    public int MaxDelayMs { get; set; } = 30000;
    
    /// <summary>
    /// Exponential backoff multiplier (default: 2.0).
    /// </summary>
    [Range(1.0, 5.0)]
    public double BackoffMultiplier { get; set; } = 2.0;
    
    /// <summary>
    /// Maximum jitter in milliseconds to add to delays (default: 1000ms).
    /// </summary>
    [Range(0, 10000)]
    public int MaxJitterMs { get; set; } = 1000;
    
    /// <summary>
    /// Whether to use exponential backoff (default: true).
    /// </summary>
    public bool UseExponentialBackoff { get; set; } = true;
    
    /// <summary>
    /// Whether to add jitter to prevent thundering herd (default: true).
    /// </summary>
    public bool UseJitter { get; set; } = true;
}

/// <summary>
/// Circuit breaker policy configuration options.
/// </summary>
public class CircuitBreakerOptions
{
    /// <summary>
    /// Number of consecutive failures before opening the circuit (default: 5).
    /// </summary>
    [Range(1, 50)]
    public int FailureThreshold { get; set; } = 5;
    
    /// <summary>
    /// Duration to keep the circuit open before attempting recovery in seconds (default: 30).
    /// </summary>
    [Range(1, 3600)]
    public int DurationOfBreakInSeconds { get; set; } = 30;
    
    /// <summary>
    /// Minimum number of requests required before circuit breaker can trip (default: 10).
    /// </summary>
    [Range(1, 100)]
    public int MinimumThroughput { get; set; } = 10;
    
    /// <summary>
    /// Whether the circuit breaker is enabled (default: true).
    /// </summary>
    public bool Enabled { get; set; } = true;
}

/// <summary>
/// Timeout policy configuration options.
/// </summary>
public class TimeoutOptions
{
    /// <summary>
    /// Default request timeout in seconds (default: 30).
    /// </summary>
    [Range(1, 3600)]
    public int DefaultTimeoutInSeconds { get; set; } = 30;
    
    /// <summary>
    /// Timeout for long-running operations in seconds (default: 300).
    /// </summary>
    [Range(30, 3600)]
    public int LongRunningTimeoutInSeconds { get; set; } = 300;
    
    /// <summary>
    /// Whether timeout policies are enabled (default: true).
    /// </summary>
    public bool Enabled { get; set; } = true;
}

/// <summary>
/// Logging configuration for resilience events.
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// Whether to log retry attempts (default: true).
    /// </summary>
    public bool LogRetryAttempts { get; set; } = true;
    
    /// <summary>
    /// Whether to log circuit breaker state changes (default: true).
    /// </summary>
    public bool LogCircuitBreakerEvents { get; set; } = true;
    
    /// <summary>
    /// Whether to log timeout events (default: true).
    /// </summary>
    public bool LogTimeouts { get; set; } = true;
    
    /// <summary>
    /// Whether to log performance metrics (default: true).
    /// </summary>
    public bool LogPerformanceMetrics { get; set; } = true;
    
    /// <summary>
    /// Whether to include request/response details in logs (default: false for security).
    /// </summary>
    public bool IncludeRequestDetails { get; set; } = false;
}