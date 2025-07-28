using System;
using System.Diagnostics;
using System.Threading;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Performance and usage metrics for type mappers to ensure they meet performance requirements.
/// </summary>
public class TypeMapperMetrics
{
    private long _toWrapperCalls;
    private long _toGeneratedCalls;
    private long _toWrapperTimeMs;
    private long _toGeneratedTimeMs;
    private long _toWrapperErrors;
    private long _toGeneratedErrors;

    /// <summary>
    /// Gets the total number of calls to MapToWrapper.
    /// </summary>
    public long ToWrapperCalls => _toWrapperCalls;

    /// <summary>
    /// Gets the total number of calls to MapToGenerated.
    /// </summary>
    public long ToGeneratedCalls => _toGeneratedCalls;

    /// <summary>
    /// Gets the total time spent in MapToWrapper operations (milliseconds).
    /// </summary>
    public long ToWrapperTimeMs => _toWrapperTimeMs;

    /// <summary>
    /// Gets the total time spent in MapToGenerated operations (milliseconds).
    /// </summary>
    public long ToGeneratedTimeMs => _toGeneratedTimeMs;

    /// <summary>
    /// Gets the number of errors in MapToWrapper operations.
    /// </summary>
    public long ToWrapperErrors => _toWrapperErrors;

    /// <summary>
    /// Gets the number of errors in MapToGenerated operations.
    /// </summary>
    public long ToGeneratedErrors => _toGeneratedErrors;

    /// <summary>
    /// Gets the average time per MapToWrapper operation (milliseconds).
    /// </summary>
    public double AverageToWrapperTimeMs => _toWrapperCalls > 0 ? (double)_toWrapperTimeMs / _toWrapperCalls : 0;

    /// <summary>
    /// Gets the average time per MapToGenerated operation (milliseconds).
    /// </summary>
    public double AverageToGeneratedTimeMs => _toGeneratedCalls > 0 ? (double)_toGeneratedTimeMs / _toGeneratedCalls : 0;

    /// <summary>
    /// Gets the error rate for MapToWrapper operations.
    /// </summary>
    public double ToWrapperErrorRate => _toWrapperCalls > 0 ? (double)_toWrapperErrors / _toWrapperCalls : 0;

    /// <summary>
    /// Gets the error rate for MapToGenerated operations.
    /// </summary>
    public double ToGeneratedErrorRate => _toGeneratedCalls > 0 ? (double)_toGeneratedErrors / _toGeneratedCalls : 0;

    /// <summary>
    /// Records timing and success for a MapToWrapper operation.
    /// </summary>
    /// <param name="elapsedMs">The elapsed time in milliseconds</param>
    /// <param name="success">Whether the operation succeeded</param>
    public void RecordToWrapper(long elapsedMs, bool success)
    {
        Interlocked.Increment(ref _toWrapperCalls);
        Interlocked.Add(ref _toWrapperTimeMs, elapsedMs);
        
        if (!success)
        {
            Interlocked.Increment(ref _toWrapperErrors);
        }
    }

    /// <summary>
    /// Records timing and success for a MapToGenerated operation.
    /// </summary>
    /// <param name="elapsedMs">The elapsed time in milliseconds</param>
    /// <param name="success">Whether the operation succeeded</param>
    public void RecordToGenerated(long elapsedMs, bool success)
    {
        Interlocked.Increment(ref _toGeneratedCalls);
        Interlocked.Add(ref _toGeneratedTimeMs, elapsedMs);
        
        if (!success)
        {
            Interlocked.Increment(ref _toGeneratedErrors);
        }
    }

    /// <summary>
    /// Resets all metrics to zero.
    /// </summary>
    public void Reset()
    {
        Interlocked.Exchange(ref _toWrapperCalls, 0);
        Interlocked.Exchange(ref _toGeneratedCalls, 0);
        Interlocked.Exchange(ref _toWrapperTimeMs, 0);
        Interlocked.Exchange(ref _toGeneratedTimeMs, 0);
        Interlocked.Exchange(ref _toWrapperErrors, 0);
        Interlocked.Exchange(ref _toGeneratedErrors, 0);
    }

    /// <summary>
    /// Validates that performance targets are being met.
    /// </summary>
    /// <param name="targetAverageMs">The target average time per operation in milliseconds (default 1ms)</param>
    /// <param name="maxErrorRate">The maximum acceptable error rate (default 1%)</param>
    /// <returns>A validation result indicating whether targets are met</returns>
    public MetricsValidationResult ValidatePerformance(double targetAverageMs = 1.0, double maxErrorRate = 0.01)
    {
        var result = new MetricsValidationResult
        {
            TargetAverageMs = targetAverageMs,
            MaxErrorRate = maxErrorRate,
            ActualToWrapperAverageMs = AverageToWrapperTimeMs,
            ActualToGeneratedAverageMs = AverageToGeneratedTimeMs,
            ActualToWrapperErrorRate = ToWrapperErrorRate,
            ActualToGeneratedErrorRate = ToGeneratedErrorRate
        };

        result.ToWrapperPerformanceOk = AverageToWrapperTimeMs <= targetAverageMs;
        result.ToGeneratedPerformanceOk = AverageToGeneratedTimeMs <= targetAverageMs;
        result.ToWrapperErrorRateOk = ToWrapperErrorRate <= maxErrorRate;
        result.ToGeneratedErrorRateOk = ToGeneratedErrorRate <= maxErrorRate;

        result.OverallValid = result.ToWrapperPerformanceOk && 
                             result.ToGeneratedPerformanceOk && 
                             result.ToWrapperErrorRateOk && 
                             result.ToGeneratedErrorRateOk;

        return result;
    }
}

/// <summary>
/// Result of performance validation for type mapper metrics.
/// </summary>
public class MetricsValidationResult
{
    public double TargetAverageMs { get; set; }
    public double MaxErrorRate { get; set; }
    public double ActualToWrapperAverageMs { get; set; }
    public double ActualToGeneratedAverageMs { get; set; }
    public double ActualToWrapperErrorRate { get; set; }
    public double ActualToGeneratedErrorRate { get; set; }
    public bool ToWrapperPerformanceOk { get; set; }
    public bool ToGeneratedPerformanceOk { get; set; }
    public bool ToWrapperErrorRateOk { get; set; }
    public bool ToGeneratedErrorRateOk { get; set; }
    public bool OverallValid { get; set; }
}