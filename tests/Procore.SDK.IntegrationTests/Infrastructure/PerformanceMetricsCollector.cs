using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace Procore.SDK.IntegrationTests.Infrastructure;

/// <summary>
/// Collects and analyzes performance metrics from integration tests
/// </summary>
public class PerformanceMetricsCollector : IDisposable
{
    private readonly ConcurrentDictionary<string, List<double>> _metrics = new();
    private readonly ConcurrentDictionary<string, Stopwatch> _activeOperations = new();
    private readonly object _lock = new();

    /// <summary>
    /// Starts timing an operation
    /// </summary>
    public void StartOperation(string operationName)
    {
        var stopwatch = Stopwatch.StartNew();
        _activeOperations.TryAdd(operationName, stopwatch);
    }

    /// <summary>
    /// Stops timing an operation and records the duration
    /// </summary>
    public TimeSpan StopOperation(string operationName)
    {
        if (_activeOperations.TryRemove(operationName, out var stopwatch))
        {
            stopwatch.Stop();
            RecordMetric($"{operationName}_Duration_Ms", stopwatch.ElapsedMilliseconds);
            return stopwatch.Elapsed;
        }

        return TimeSpan.Zero;
    }

    /// <summary>
    /// Records a metric value
    /// </summary>
    public void RecordMetric(string metricName, double value)
    {
        _metrics.AddOrUpdate(metricName, 
            new List<double> { value },
            (key, existing) =>
            {
                lock (_lock)
                {
                    existing.Add(value);
                    return existing;
                }
            });
    }

    /// <summary>
    /// Gets statistical summary for a metric
    /// </summary>
    public MetricSummary GetMetricSummary(string metricName)
    {
        if (!_metrics.TryGetValue(metricName, out var values) || !values.Any())
        {
            return new MetricSummary(metricName, 0, 0, 0, 0, 0, 0, 0);
        }

        lock (_lock)
        {
            var sortedValues = values.OrderBy(v => v).ToArray();
            var count = sortedValues.Length;
            var sum = sortedValues.Sum();
            var average = sum / count;
            var min = sortedValues[0];
            var max = sortedValues[count - 1];
            var median = count % 2 == 0 
                ? (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0
                : sortedValues[count / 2];
            var p95 = sortedValues[(int)Math.Ceiling(count * 0.95) - 1];

            return new MetricSummary(metricName, count, sum, average, min, max, median, p95);
        }
    }

    /// <summary>
    /// Gets all metric summaries
    /// </summary>
    public IEnumerable<MetricSummary> GetAllMetricSummaries()
    {
        return _metrics.Keys.Select(GetMetricSummary);
    }

    /// <summary>
    /// Exports metrics to JSON format
    /// </summary>
    public string ExportToJson()
    {
        var summaries = GetAllMetricSummaries().ToArray();
        return JsonSerializer.Serialize(summaries, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
    }

    /// <summary>
    /// Validates metric against performance target
    /// </summary>
    public MetricValidationResult ValidateMetric(string metricName, double targetValue, 
        MetricValidationType validationType = MetricValidationType.Average)
    {
        var summary = GetMetricSummary(metricName);
        
        if (summary.Count == 0)
        {
            return new MetricValidationResult(metricName, false, 0, targetValue, 
                $"No data recorded for metric '{metricName}'");
        }

        var actualValue = validationType switch
        {
            MetricValidationType.Average => summary.Average,
            MetricValidationType.Median => summary.Median,
            MetricValidationType.P95 => summary.P95,
            MetricValidationType.Max => summary.Max,
            _ => summary.Average
        };

        var passed = actualValue <= targetValue;
        var message = passed 
            ? $"Metric '{metricName}' passed: {actualValue:F2} <= {targetValue:F2}"
            : $"Metric '{metricName}' failed: {actualValue:F2} > {targetValue:F2}";

        return new MetricValidationResult(metricName, passed, actualValue, targetValue, message);
    }

    /// <summary>
    /// Validates multiple metrics against targets
    /// </summary>
    public PerformanceValidationReport ValidatePerformanceTargets(
        Dictionary<string, double> targets,
        MetricValidationType validationType = MetricValidationType.P95)
    {
        var results = targets.Select(kvp => 
            ValidateMetric(kvp.Key, kvp.Value, validationType)).ToArray();

        var passedCount = results.Count(r => r.Passed);
        var overallPassed = passedCount == results.Length;

        return new PerformanceValidationReport(overallPassed, passedCount, results.Length, results);
    }

    /// <summary>
    /// Records operation execution with automatic timing
    /// </summary>
    public async Task<T> RecordOperationAsync<T>(string operationName, Func<Task<T>> operation)
    {
        StartOperation(operationName);
        try
        {
            var result = await operation();
            StopOperation(operationName);
            RecordMetric($"{operationName}_Success", 1);
            return result;
        }
        catch (Exception ex)
        {
            StopOperation(operationName);
            RecordMetric($"{operationName}_Error", 1);
            RecordMetric($"{operationName}_Error_{ex.GetType().Name}", 1);
            throw;
        }
    }

    /// <summary>
    /// Records synchronous operation execution with automatic timing
    /// </summary>
    public T RecordOperation<T>(string operationName, Func<T> operation)
    {
        StartOperation(operationName);
        try
        {
            var result = operation();
            StopOperation(operationName);
            RecordMetric($"{operationName}_Success", 1);
            return result;
        }
        catch (Exception ex)
        {
            StopOperation(operationName);
            RecordMetric($"{operationName}_Error", 1);
            RecordMetric($"{operationName}_Error_{ex.GetType().Name}", 1);
            throw;
        }
    }

    /// <summary>
    /// Generates a performance report
    /// </summary>
    public PerformanceReport GenerateReport()
    {
        var summaries = GetAllMetricSummaries().ToArray();
        var totalOperations = summaries.Where(s => s.MetricName.EndsWith("_Duration_Ms")).Sum(s => s.Count);
        var totalErrors = summaries.Where(s => s.MetricName.Contains("_Error")).Sum(s => s.Count);
        var successRate = totalOperations > 0 ? (totalOperations - totalErrors) / (double)totalOperations * 100 : 0;

        return new PerformanceReport
        {
            GeneratedAt = DateTime.UtcNow,
            TotalOperations = (int)totalOperations,
            TotalErrors = (int)totalErrors,
            SuccessRate = successRate,
            MetricSummaries = summaries
        };
    }

    public void Dispose()
    {
        // Stop any active operations
        foreach (var kvp in _activeOperations)
        {
            kvp.Value.Stop();
        }
        _activeOperations.Clear();
        _metrics.Clear();
    }
}

/// <summary>
/// Statistical summary of a metric
/// </summary>
/// <param name="MetricName">Name of the metric</param>
/// <param name="Count">Number of recorded values</param>
/// <param name="Sum">Sum of all values</param>
/// <param name="Average">Average value</param>
/// <param name="Min">Minimum value</param>
/// <param name="Max">Maximum value</param>
/// <param name="Median">Median value</param>
/// <param name="P95">95th percentile value</param>
public record MetricSummary(
    string MetricName, 
    int Count, 
    double Sum, 
    double Average, 
    double Min, 
    double Max, 
    double Median, 
    double P95);

/// <summary>
/// Result of metric validation against target
/// </summary>
/// <param name="MetricName">Name of the metric</param>
/// <param name="Passed">Whether the validation passed</param>
/// <param name="ActualValue">Actual metric value</param>
/// <param name="TargetValue">Target value</param>
/// <param name="Message">Validation message</param>
public record MetricValidationResult(
    string MetricName,
    bool Passed,
    double ActualValue,
    double TargetValue,
    string Message);

/// <summary>
/// Performance validation report
/// </summary>
/// <param name="OverallPassed">Whether all validations passed</param>
/// <param name="PassedCount">Number of validations that passed</param>
/// <param name="TotalCount">Total number of validations</param>
/// <param name="Results">Individual validation results</param>
public record PerformanceValidationReport(
    bool OverallPassed,
    int PassedCount,
    int TotalCount,
    MetricValidationResult[] Results);

/// <summary>
/// Type of metric validation
/// </summary>
public enum MetricValidationType
{
    Average,
    Median,
    P95,
    Max
}

/// <summary>
/// Performance test report
/// </summary>
public class PerformanceReport
{
    public DateTime GeneratedAt { get; set; }
    public int TotalOperations { get; set; }
    public int TotalErrors { get; set; }
    public double SuccessRate { get; set; }
    public MetricSummary[] MetricSummaries { get; set; } = Array.Empty<MetricSummary>();
}