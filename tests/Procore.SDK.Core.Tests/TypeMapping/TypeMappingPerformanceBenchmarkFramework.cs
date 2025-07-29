using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive performance benchmarking framework for type mapping operations.
/// Provides detailed performance analysis, stress testing, and concurrent access validation.
/// </summary>
public class TypeMappingPerformanceBenchmarkFramework
{
    private readonly ITestOutputHelper _output;
    private readonly PerformanceConfiguration _config;

    public TypeMappingPerformanceBenchmarkFramework(ITestOutputHelper output, PerformanceConfiguration? config = null)
    {
        _output = output;
        _config = config ?? PerformanceConfiguration.Default;
    }

    #region Single Operation Benchmarks

    /// <summary>
    /// Measures single operation performance with detailed metrics.
    /// </summary>
    public SingleOperationBenchmarkResult BenchmarkSingleOperation<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperSource,
        TGenerated generatedSource,
        string operationName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new SingleOperationBenchmarkResult
        {
            OperationName = operationName,
            MapperType = mapper.GetType().Name
        };

        // Warm up
        for (int i = 0; i < _config.WarmupIterations; i++)
        {
            mapper.MapToWrapper(generatedSource);
            mapper.MapToGenerated(wrapperSource);
        }

        // Measure ToWrapper operation
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var initialMemory = GC.GetTotalMemory(false);
        var stopwatch = Stopwatch.StartNew();

        var wrapperResult = mapper.MapToWrapper(generatedSource);
        
        stopwatch.Stop();
        var finalMemory = GC.GetTotalMemory(false);

        result.ToWrapperMetrics = new OperationMetrics
        {
            ElapsedTicks = stopwatch.ElapsedTicks,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            ElapsedMicroseconds = (stopwatch.ElapsedTicks * 1000000.0) / Stopwatch.Frequency,
            MemoryAllocated = Math.Max(0, finalMemory - initialMemory),
            Success = wrapperResult != null
        };

        // Measure ToGenerated operation
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        initialMemory = GC.GetTotalMemory(false);
        stopwatch.Restart();

        var generatedResult = mapper.MapToGenerated(wrapperSource);
        
        stopwatch.Stop();
        finalMemory = GC.GetTotalMemory(false);

        result.ToGeneratedMetrics = new OperationMetrics
        {
            ElapsedTicks = stopwatch.ElapsedTicks,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            ElapsedMicroseconds = (stopwatch.ElapsedTicks * 1000000.0) / Stopwatch.Frequency,
            MemoryAllocated = Math.Max(0, finalMemory - initialMemory),
            Success = generatedResult != null
        };

        return result;
    }

    #endregion

    #region Batch Operation Benchmarks

    /// <summary>
    /// Measures batch operation performance with statistical analysis.
    /// </summary>
    public BatchOperationBenchmarkResult BenchmarkBatchOperations<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperSource,
        TGenerated generatedSource,
        int operationCount,
        string operationName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new BatchOperationBenchmarkResult
        {
            OperationName = operationName,
            MapperType = mapper.GetType().Name,
            OperationCount = operationCount
        };

        // Warm up
        for (int i = 0; i < Math.Min(_config.WarmupIterations, 10); i++)
        {
            mapper.MapToWrapper(generatedSource);
            mapper.MapToGenerated(wrapperSource);
        }

        // Measure ToWrapper batch operations
        var toWrapperTimes = new List<long>();
        var toWrapperMemory = new List<long>();

        var initialTotalMemory = GC.GetTotalMemory(true);
        var overallStopwatch = Stopwatch.StartNew();

        for (int i = 0; i < operationCount; i++)
        {
            var preOpMemory = GC.GetTotalMemory(false);
            var opStopwatch = Stopwatch.StartNew();
            
            var wrapperResult = mapper.MapToWrapper(generatedSource);
            
            opStopwatch.Stop();
            var postOpMemory = GC.GetTotalMemory(false);

            toWrapperTimes.Add(opStopwatch.ElapsedMilliseconds);
            toWrapperMemory.Add(Math.Max(0, postOpMemory - preOpMemory));

            wrapperResult.Should().NotBeNull();
        }

        overallStopwatch.Stop();
        var finalTotalMemory = GC.GetTotalMemory(true);

        result.ToWrapperBatchMetrics = new BatchMetrics
        {
            TotalElapsedMilliseconds = overallStopwatch.ElapsedMilliseconds,
            MinTimeMilliseconds = toWrapperTimes.Min(),
            MaxTimeMilliseconds = toWrapperTimes.Max(),
            AverageTimeMilliseconds = toWrapperTimes.Average(),
            MedianTimeMilliseconds = CalculateMedian(toWrapperTimes),
            Percentile95Milliseconds = CalculatePercentile(toWrapperTimes, 0.95),
            Percentile99Milliseconds = CalculatePercentile(toWrapperTimes, 0.99),
            TotalMemoryAllocated = Math.Max(0, finalTotalMemory - initialTotalMemory),
            AverageMemoryPerOperation = toWrapperMemory.Average(),
            StandardDeviation = CalculateStandardDeviation(toWrapperTimes.Select(x => (double)x))
        };

        // Reset for ToGenerated operations
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Measure ToGenerated batch operations
        var toGeneratedTimes = new List<long>();
        var toGeneratedMemory = new List<long>();

        initialTotalMemory = GC.GetTotalMemory(true);
        overallStopwatch.Restart();

        for (int i = 0; i < operationCount; i++)
        {
            var preOpMemory = GC.GetTotalMemory(false);
            var opStopwatch = Stopwatch.StartNew();
            
            var generatedResult = mapper.MapToGenerated(wrapperSource);
            
            opStopwatch.Stop();
            var postOpMemory = GC.GetTotalMemory(false);

            toGeneratedTimes.Add(opStopwatch.ElapsedMilliseconds);
            toGeneratedMemory.Add(Math.Max(0, postOpMemory - preOpMemory));

            generatedResult.Should().NotBeNull();
        }

        overallStopwatch.Stop();
        finalTotalMemory = GC.GetTotalMemory(true);

        result.ToGeneratedBatchMetrics = new BatchMetrics
        {
            TotalElapsedMilliseconds = overallStopwatch.ElapsedMilliseconds,
            MinTimeMilliseconds = toGeneratedTimes.Min(),
            MaxTimeMilliseconds = toGeneratedTimes.Max(),
            AverageTimeMilliseconds = toGeneratedTimes.Average(),
            MedianTimeMilliseconds = CalculateMedian(toGeneratedTimes),
            Percentile95Milliseconds = CalculatePercentile(toGeneratedTimes, 0.95),
            Percentile99Milliseconds = CalculatePercentile(toGeneratedTimes, 0.99),
            TotalMemoryAllocated = Math.Max(0, finalTotalMemory - initialTotalMemory),
            AverageMemoryPerOperation = toGeneratedMemory.Average(),
            StandardDeviation = CalculateStandardDeviation(toGeneratedTimes.Select(x => (double)x))
        };

        return result;
    }

    #endregion

    #region Concurrent Access Benchmarks

    /// <summary>
    /// Measures concurrent access performance and thread safety.
    /// </summary>
    public ConcurrentBenchmarkResult BenchmarkConcurrentAccess<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperSource,
        TGenerated generatedSource,
        int threadCount,
        int operationsPerThread,
        string operationName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new ConcurrentBenchmarkResult
        {
            OperationName = operationName,
            MapperType = mapper.GetType().Name,
            ThreadCount = threadCount,
            OperationsPerThread = operationsPerThread,
            TotalOperations = threadCount * operationsPerThread
        };

        var operationTimes = new ConcurrentBag<long>();
        var exceptions = new ConcurrentBag<Exception>();
        var successfulOperations = 0;

        var barrier = new Barrier(threadCount);
        var overallStopwatch = new Stopwatch();

        var tasks = Enumerable.Range(0, threadCount).Select(threadId =>
            Task.Run(() =>
            {
                try
                {
                    // Wait for all threads to be ready
                    barrier.SignalAndWait();

                    // Start timing after all threads are synchronized
                    if (threadId == 0)
                    {
                        overallStopwatch.Start();
                    }

                    for (int i = 0; i < operationsPerThread; i++)
                    {
                        var stopwatch = Stopwatch.StartNew();

                        // Alternate between operation types
                        if (i % 2 == 0)
                        {
                            var wrapperResult = mapper.MapToWrapper(generatedSource);
                            wrapperResult.Should().NotBeNull();
                        }
                        else
                        {
                            var generatedResult = mapper.MapToGenerated(wrapperSource);
                            generatedResult.Should().NotBeNull();
                        }

                        stopwatch.Stop();
                        operationTimes.Add(stopwatch.ElapsedMilliseconds);
                        Interlocked.Increment(ref successfulOperations);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            })
        ).ToArray();

        Task.WaitAll(tasks);
        overallStopwatch.Stop();

        var times = operationTimes.ToList();
        
        result.TotalElapsedMilliseconds = overallStopwatch.ElapsedMilliseconds;
        result.SuccessfulOperations = successfulOperations;
        result.FailedOperations = result.TotalOperations - successfulOperations;
        result.Exceptions = exceptions.ToList();
        
        if (times.Any())
        {
            result.MinTimeMilliseconds = times.Min();
            result.MaxTimeMilliseconds = times.Max();
            result.AverageTimeMilliseconds = times.Average();
            result.MedianTimeMilliseconds = CalculateMedian(times);
            result.Percentile95Milliseconds = CalculatePercentile(times, 0.95);
            result.Percentile99Milliseconds = CalculatePercentile(times, 0.99);
            result.StandardDeviation = CalculateStandardDeviation(times.Select(x => (double)x));
        }

        result.OperationsPerSecond = result.TotalOperations / (overallStopwatch.ElapsedMilliseconds / 1000.0);

        return result;
    }

    #endregion

    #region Stress Testing

    /// <summary>
    /// Performs stress testing with high operation counts to detect performance degradation.
    /// </summary>
    public StressTestResult PerformStressTest<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperSource,
        TGenerated generatedSource,
        int totalOperations,
        int batchSize = 100,
        string operationName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new StressTestResult
        {
            OperationName = operationName,
            MapperType = mapper.GetType().Name,
            TotalOperations = totalOperations,
            BatchSize = batchSize
        };

        var batchResults = new List<BatchPerformanceSnapshot>();
        var exceptions = new List<Exception>();
        var totalBatches = (int)Math.Ceiling((double)totalOperations / batchSize);

        var overallStopwatch = Stopwatch.StartNew();
        var initialMemory = GC.GetTotalMemory(true);

        for (int batch = 0; batch < totalBatches; batch++)
        {
            var currentBatchSize = Math.Min(batchSize, totalOperations - (batch * batchSize));
            var batchSnapshot = new BatchPerformanceSnapshot
            {
                BatchNumber = batch + 1,
                OperationCount = currentBatchSize
            };

            try
            {
                var batchStopwatch = Stopwatch.StartNew();
                var batchStartMemory = GC.GetTotalMemory(false);

                for (int i = 0; i < currentBatchSize; i++)
                {
                    if (i % 2 == 0)
                    {
                        mapper.MapToWrapper(generatedSource);
                    }
                    else
                    {
                        mapper.MapToGenerated(wrapperSource);
                    }
                }

                batchStopwatch.Stop();
                var batchEndMemory = GC.GetTotalMemory(false);

                batchSnapshot.ElapsedMilliseconds = batchStopwatch.ElapsedMilliseconds;
                batchSnapshot.AverageTimePerOperation = (double)batchStopwatch.ElapsedMilliseconds / currentBatchSize;
                batchSnapshot.MemoryIncrease = Math.Max(0, batchEndMemory - batchStartMemory);
                batchSnapshot.Success = true;

                // Force GC every 10 batches to prevent memory buildup
                if (batch % 10 == 9)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                batchSnapshot.Success = false;
                batchSnapshot.Exception = ex;
                exceptions.Add(ex);
            }

            batchResults.Add(batchSnapshot);

            // Report progress for large stress tests
            if (totalBatches > 50 && batch % (totalBatches / 10) == 0)
            {
                _output.WriteLine($"Stress test progress: {batch + 1}/{totalBatches} batches completed");
            }
        }

        overallStopwatch.Stop();
        var finalMemory = GC.GetTotalMemory(true);

        result.TotalElapsedMilliseconds = overallStopwatch.ElapsedMilliseconds;
        result.BatchResults = batchResults;
        result.Exceptions = exceptions;
        result.TotalMemoryIncrease = Math.Max(0, finalMemory - initialMemory);
        result.SuccessfulBatches = batchResults.Count(b => b.Success);
        result.FailedBatches = batchResults.Count(b => !b.Success);

        var successfulBatches = batchResults.Where(b => b.Success).ToList();
        if (successfulBatches.Any())
        {
            result.AverageTimePerOperation = successfulBatches.Average(b => b.AverageTimePerOperation);
            result.MinBatchTime = successfulBatches.Min(b => b.ElapsedMilliseconds);
            result.MaxBatchTime = successfulBatches.Max(b => b.ElapsedMilliseconds);
            
            var firstBatch = successfulBatches.Take(5).Average(b => b.AverageTimePerOperation);
            var lastBatch = successfulBatches.TakeLast(5).Average(b => b.AverageTimePerOperation);
            
            result.PerformanceDegradation = ((lastBatch - firstBatch) / firstBatch) * 100;
        }

        result.OperationsPerSecond = totalOperations / (overallStopwatch.ElapsedMilliseconds / 1000.0);

        return result;
    }

    #endregion

    #region Validation Methods

    /// <summary>
    /// Validates that benchmark results meet performance requirements.
    /// </summary>
    public BenchmarkValidationResult ValidateBenchmarkResults(
        SingleOperationBenchmarkResult singleOp,
        BatchOperationBenchmarkResult batchOp,
        ConcurrentBenchmarkResult concurrentOp,
        StressTestResult stressTest)
    {
        var validation = new BenchmarkValidationResult
        {
            MapperType = singleOp.MapperType,
            ValidationTimestamp = DateTime.UtcNow
        };

        var issues = new List<string>();

        // Single operation validation
        if (singleOp.ToWrapperMetrics.ElapsedMilliseconds > _config.SingleOperationMaxMs)
        {
            issues.Add($"Single ToWrapper operation ({singleOp.ToWrapperMetrics.ElapsedMilliseconds}ms) exceeds target ({_config.SingleOperationMaxMs}ms)");
        }

        if (singleOp.ToGeneratedMetrics.ElapsedMilliseconds > _config.SingleOperationMaxMs)
        {
            issues.Add($"Single ToGenerated operation ({singleOp.ToGeneratedMetrics.ElapsedMilliseconds}ms) exceeds target ({_config.SingleOperationMaxMs}ms)");
        }

        // Batch operation validation
        if (batchOp.ToWrapperBatchMetrics.AverageTimeMilliseconds > _config.BatchAverageMaxMs)
        {
            issues.Add($"Batch ToWrapper average ({batchOp.ToWrapperBatchMetrics.AverageTimeMilliseconds:F3}ms) exceeds target ({_config.BatchAverageMaxMs}ms)");
        }

        if (batchOp.ToGeneratedBatchMetrics.AverageTimeMilliseconds > _config.BatchAverageMaxMs)
        {
            issues.Add($"Batch ToGenerated average ({batchOp.ToGeneratedBatchMetrics.AverageTimeMilliseconds:F3}ms) exceeds target ({_config.BatchAverageMaxMs}ms)");
        }

        // Concurrent operation validation
        if (concurrentOp.AverageTimeMilliseconds > _config.ConcurrentAverageMaxMs)
        {
            issues.Add($"Concurrent average ({concurrentOp.AverageTimeMilliseconds:F3}ms) exceeds target ({_config.ConcurrentAverageMaxMs}ms)");
        }

        if (concurrentOp.FailedOperations > 0)
        {
            issues.Add($"Concurrent operations had {concurrentOp.FailedOperations} failures out of {concurrentOp.TotalOperations}");
        }

        // Stress test validation
        if (stressTest.PerformanceDegradation > _config.MaxPerformanceDegradationPercent)
        {
            issues.Add($"Performance degradation ({stressTest.PerformanceDegradation:F1}%) exceeds maximum ({_config.MaxPerformanceDegradationPercent}%)");
        }

        if (stressTest.FailedBatches > 0)
        {
            issues.Add($"Stress test had {stressTest.FailedBatches} failed batches out of {stressTest.BatchResults.Count}");
        }

        validation.IsValid = issues.Count == 0;
        validation.ValidationIssues = issues;

        return validation;
    }

    #endregion

    #region Helper Methods

    private static double CalculateMedian(IEnumerable<long> values)
    {
        var sortedValues = values.OrderBy(x => x).ToList();
        var count = sortedValues.Count;
        
        if (count % 2 == 0)
        {
            return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0;
        }
        else
        {
            return sortedValues[count / 2];
        }
    }

    private static double CalculatePercentile(IEnumerable<long> values, double percentile)
    {
        var sortedValues = values.OrderBy(x => x).ToList();
        var index = (int)Math.Ceiling(percentile * sortedValues.Count) - 1;
        return sortedValues[Math.Max(0, Math.Min(index, sortedValues.Count - 1))];
    }

    private static double CalculateStandardDeviation(IEnumerable<double> values)
    {
        var valuesList = values.ToList();
        var average = valuesList.Average();
        var sumOfSquares = valuesList.Sum(x => Math.Pow(x - average, 2));
        return Math.Sqrt(sumOfSquares / valuesList.Count);
    }

    #endregion
}

#region Configuration and Result Classes

/// <summary>
/// Configuration for performance benchmarking.
/// </summary>
public class PerformanceConfiguration
{
    public int WarmupIterations { get; set; } = 5;
    public double SingleOperationMaxMs { get; set; } = 1.0;
    public double BatchAverageMaxMs { get; set; } = 1.0;
    public double ConcurrentAverageMaxMs { get; set; } = 2.0;
    public double MaxPerformanceDegradationPercent { get; set; } = 10.0;

    public static PerformanceConfiguration Default => new();
}

/// <summary>
/// Metrics for a single operation.
/// </summary>
public class OperationMetrics
{
    public long ElapsedTicks { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public double ElapsedMicroseconds { get; set; }
    public long MemoryAllocated { get; set; }
    public bool Success { get; set; }
}

/// <summary>
/// Result of single operation benchmark.
/// </summary>
public class SingleOperationBenchmarkResult
{
    public string OperationName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public OperationMetrics ToWrapperMetrics { get; set; } = new();
    public OperationMetrics ToGeneratedMetrics { get; set; } = new();
}

/// <summary>
/// Metrics for batch operations.
/// </summary>
public class BatchMetrics
{
    public long TotalElapsedMilliseconds { get; set; }
    public long MinTimeMilliseconds { get; set; }
    public long MaxTimeMilliseconds { get; set; }
    public double AverageTimeMilliseconds { get; set; }
    public double MedianTimeMilliseconds { get; set; }
    public double Percentile95Milliseconds { get; set; }
    public double Percentile99Milliseconds { get; set; }
    public long TotalMemoryAllocated { get; set; }
    public double AverageMemoryPerOperation { get; set; }
    public double StandardDeviation { get; set; }
}

/// <summary>
/// Result of batch operation benchmark.
/// </summary>
public class BatchOperationBenchmarkResult
{
    public string OperationName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public int OperationCount { get; set; }
    public BatchMetrics ToWrapperBatchMetrics { get; set; } = new();
    public BatchMetrics ToGeneratedBatchMetrics { get; set; } = new();
}

/// <summary>
/// Result of concurrent access benchmark.
/// </summary>
public class ConcurrentBenchmarkResult
{
    public string OperationName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public int ThreadCount { get; set; }
    public int OperationsPerThread { get; set; }
    public int TotalOperations { get; set; }
    public long TotalElapsedMilliseconds { get; set; }
    public int SuccessfulOperations { get; set; }
    public int FailedOperations { get; set; }
    public List<Exception> Exceptions { get; set; } = new();
    public long MinTimeMilliseconds { get; set; }
    public long MaxTimeMilliseconds { get; set; }
    public double AverageTimeMilliseconds { get; set; }
    public double MedianTimeMilliseconds { get; set; }
    public double Percentile95Milliseconds { get; set; }
    public double Percentile99Milliseconds { get; set; }
    public double StandardDeviation { get; set; }
    public double OperationsPerSecond { get; set; }
}

/// <summary>
/// Performance snapshot for a batch during stress testing.
/// </summary>
public class BatchPerformanceSnapshot
{
    public int BatchNumber { get; set; }
    public int OperationCount { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public double AverageTimePerOperation { get; set; }
    public long MemoryIncrease { get; set; }
    public bool Success { get; set; }
    public Exception? Exception { get; set; }
}

/// <summary>
/// Result of stress testing.
/// </summary>
public class StressTestResult
{
    public string OperationName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public int TotalOperations { get; set; }
    public int BatchSize { get; set; }
    public long TotalElapsedMilliseconds { get; set; }
    public List<BatchPerformanceSnapshot> BatchResults { get; set; } = new();
    public List<Exception> Exceptions { get; set; } = new();
    public long TotalMemoryIncrease { get; set; }
    public int SuccessfulBatches { get; set; }
    public int FailedBatches { get; set; }
    public double AverageTimePerOperation { get; set; }
    public long MinBatchTime { get; set; }
    public long MaxBatchTime { get; set; }
    public double PerformanceDegradation { get; set; }
    public double OperationsPerSecond { get; set; }
}

/// <summary>
/// Result of benchmark validation.
/// </summary>
public class BenchmarkValidationResult
{
    public string MapperType { get; set; } = string.Empty;
    public DateTime ValidationTimestamp { get; set; }
    public bool IsValid { get; set; }
    public List<string> ValidationIssues { get; set; } = new();
}

#endregion