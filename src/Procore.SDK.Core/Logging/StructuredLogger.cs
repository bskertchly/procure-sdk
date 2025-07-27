using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Procore.SDK.Core.Logging;

/// <summary>
/// Structured logger for Procore SDK operations with correlation tracking and performance metrics.
/// </summary>
public class StructuredLogger
{
    private readonly ILogger<StructuredLogger> _logger;

    /// <summary>
    /// Initializes a new instance of the StructuredLogger class.
    /// </summary>
    /// <param name="logger">The logger instance for structured logging operations.</param>
    public StructuredLogger(ILogger<StructuredLogger> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs the start of an operation with correlation tracking.
    /// </summary>
    public IDisposable BeginOperation(string operation, string correlationId, IDictionary<string, object>? properties = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var context = LogContext.PushProperty("CorrelationId", correlationId);
        
        if (properties != null)
        {
            foreach (var kvp in properties)
            {
                LogContext.PushProperty(kvp.Key, kvp.Value);
            }
        }

        _logger.LogInformation("Starting operation {Operation} (CorrelationId: {CorrelationId})", 
            operation, correlationId);

        return new OperationScope(_logger, operation, correlationId, stopwatch, context);
    }

    /// <summary>
    /// Logs an error with structured context and correlation tracking.
    /// </summary>
    public void LogError(
        Exception exception,
        string operation,
        string correlationId,
        string message,
        params object[] args)
    {
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogError(exception, $"Operation: {operation} - " + message, args);
        }
    }

    /// <summary>
    /// Logs a warning with structured context.
    /// </summary>
    public void LogWarning(
        string operation,
        string correlationId,
        string message,
        params object[] args)
    {
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogWarning($"Operation: {operation} - " + message, args);
        }
    }

    /// <summary>
    /// Logs performance metrics for an operation.
    /// </summary>
    public void LogPerformanceMetrics(
        string operation,
        string correlationId,
        TimeSpan duration,
        bool success,
        IDictionary<string, object>? metrics = null)
    {
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            var logLevel = success ? LogLevel.Information : LogLevel.Warning;
            var status = success ? "completed" : "failed";

            _logger.Log(logLevel,
                "Operation {Operation} {Status} in {DurationMs}ms (CorrelationId: {CorrelationId})",
                operation, status, duration.TotalMilliseconds, correlationId);

            if (metrics != null)
            {
                foreach (var kvp in metrics)
                {
                    using (LogContext.PushProperty(kvp.Key, kvp.Value))
                    {
                        // Properties are pushed to the context
                    }
                }
            }
        }
    }

    /// <summary>
    /// Logs retry attempt information.
    /// </summary>
    public void LogRetryAttempt(
        string operation,
        string correlationId,
        int attemptNumber,
        TimeSpan delay,
        Exception? exception = null)
    {
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            if (exception != null)
            {
                _logger.LogWarning(exception,
                    "Retry attempt {AttemptNumber} for operation {Operation} after {DelayMs}ms delay (CorrelationId: {CorrelationId})",
                    attemptNumber, operation, delay.TotalMilliseconds, correlationId);
            }
            else
            {
                _logger.LogWarning(
                    "Retry attempt {AttemptNumber} for operation {Operation} after {DelayMs}ms delay (CorrelationId: {CorrelationId})",
                    attemptNumber, operation, delay.TotalMilliseconds, correlationId);
            }
        }
    }

    /// <summary>
    /// Logs circuit breaker state changes.
    /// </summary>
    public void LogCircuitBreakerStateChange(
        string operation,
        string correlationId,
        string state,
        string? reason = null)
    {
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogWarning(
                "Circuit breaker {State} for operation {Operation} (CorrelationId: {CorrelationId}). Reason: {Reason}",
                state, operation, correlationId, reason ?? "Not specified");
        }
    }

    /// <summary>
    /// Creates a scoped operation logger that automatically tracks duration and completion.
    /// </summary>
    private class OperationScope : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _operation;
        private readonly string _correlationId;
        private readonly Stopwatch _stopwatch;
        private readonly IDisposable _logContext;
        private bool _disposed;

        public OperationScope(
            ILogger logger,
            string operation,
            string correlationId,
            Stopwatch stopwatch,
            IDisposable logContext)
        {
            _logger = logger;
            _operation = operation;
            _correlationId = correlationId;
            _stopwatch = stopwatch;
            _logContext = logContext;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _stopwatch.Stop();
            
            _logger.LogInformation(
                "Completed operation {Operation} in {DurationMs}ms (CorrelationId: {CorrelationId})",
                _operation, _stopwatch.ElapsedMilliseconds, _correlationId);

            _logContext.Dispose();
            _disposed = true;
        }
    }
}