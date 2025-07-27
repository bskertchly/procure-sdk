using System;
using System.Collections.Generic;

namespace Procore.SDK.Core.Resilience;

/// <summary>
/// Context information for resilience policies including retry attempts and correlation tracking.
/// </summary>
public class ResilienceContext
{
    /// <summary>
    /// Unique correlation ID for tracking requests across systems.
    /// </summary>
    public string CorrelationId { get; }
    
    /// <summary>
    /// The current retry attempt number (0 for initial attempt).
    /// </summary>
    public int AttemptNumber { get; set; }
    
    /// <summary>
    /// The operation being performed (e.g., "GET /rest/v1.0/companies").
    /// </summary>
    public string Operation { get; }
    
    /// <summary>
    /// Timestamp when the operation started.
    /// </summary>
    public DateTimeOffset StartTime { get; }
    
    /// <summary>
    /// Additional context properties for tracking and debugging.
    /// </summary>
    public Dictionary<string, object> Properties { get; }
    
    /// <summary>
    /// The last exception that occurred (if any).
    /// </summary>
    public Exception? LastException { get; set; }
    
    /// <summary>
    /// Total time elapsed since the operation started.
    /// </summary>
    public TimeSpan ElapsedTime => DateTimeOffset.UtcNow - StartTime;

    public ResilienceContext(string operation, string? correlationId = null)
    {
        Operation = operation;
        CorrelationId = correlationId ?? Guid.NewGuid().ToString();
        StartTime = DateTimeOffset.UtcNow;
        Properties = new Dictionary<string, object>();
        AttemptNumber = 0;
    }

    /// <summary>
    /// Increments the attempt number for retry scenarios.
    /// </summary>
    public void IncrementAttempt()
    {
        AttemptNumber++;
    }

    /// <summary>
    /// Adds or updates a property in the context.
    /// </summary>
    public void SetProperty(string key, object value)
    {
        Properties[key] = value;
    }

    /// <summary>
    /// Gets a property from the context.
    /// </summary>
    public T? GetProperty<T>(string key)
    {
        if (Properties.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    /// <summary>
    /// Creates a copy of the context for a new attempt.
    /// </summary>
    public ResilienceContext Clone()
    {
        var cloned = new ResilienceContext(Operation, CorrelationId)
        {
            AttemptNumber = AttemptNumber,
            LastException = LastException
        };
        
        foreach (var kvp in Properties)
        {
            cloned.Properties[kvp.Key] = kvp.Value;
        }
        
        return cloned;
    }
}