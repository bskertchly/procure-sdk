using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Procore.SDK.IntegrationTests.Live.Infrastructure;

/// <summary>
/// Base class for all live integration tests
/// Provides common functionality, logging, and performance tracking
/// </summary>
[Collection("LiveSandboxTests")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly LiveSandboxFixture Fixture;
    protected readonly ILogger Logger;
    protected readonly ITestOutputHelper Output;
    protected readonly PerformanceMetricsCollector PerformanceMetrics;
    protected readonly TestDataBuilder<object> TestDataBuilder;

    // Common test properties
    protected int TestCompanyId => Fixture.TestCompanyId;
    protected string TestUserEmail => Fixture.TestUserEmail;
    protected TestConfiguration TestConfig => Fixture.TestConfig;

    // Client shortcuts
    protected ProcoreCoreClient CoreClient => Fixture.CoreClient;
    protected ProjectManagementClient ProjectManagementClient => Fixture.ProjectManagementClient;  
    protected QualitySafetyClient QualitySafetyClient => Fixture.QualitySafetyClient;
    protected ConstructionFinancialsClient ConstructionFinancialsClient => Fixture.ConstructionFinancialsClient;
    protected FieldProductivityClient FieldProductivityClient => Fixture.FieldProductivityClient;
    protected ResourceManagementClient ResourceManagementClient => Fixture.ResourceManagementClient;

    protected IntegrationTestBase(LiveSandboxFixture fixture, ITestOutputHelper output)
    {
        Fixture = fixture;
        Output = output;
        Logger = Fixture.CreateLogger(GetType());
        PerformanceMetrics = new PerformanceMetricsCollector();
        TestDataBuilder = new TestDataBuilder<object>(Fixture.CreateLogger<TestDataBuilder<object>>());
    }

    public virtual Task InitializeAsync()
    {
        Logger.LogInformation("Initializing integration test: {TestName}", GetType().Name);
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        Logger.LogInformation("Disposing integration test: {TestName}", GetType().Name);
        
        // Generate test-specific performance report
        var report = PerformanceMetrics.GenerateReport();
        if (report.TotalOperations > 0)
        {
            Output.WriteLine($"Test Performance Summary:");
            Output.WriteLine($"  Operations: {report.TotalOperations}");
            Output.WriteLine($"  Errors: {report.TotalErrors}");
            Output.WriteLine($"  Success Rate: {report.SuccessRate:F2}%");
            
            // Log detailed metrics
            foreach (var metric in report.MetricSummaries.Where(m => m.Count > 0))
            {
                Output.WriteLine($"  {metric.MetricName}: Avg={metric.Average:F2}ms, P95={metric.P95:F2}ms");
            }
        }
        
        PerformanceMetrics?.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes an operation with performance tracking and error handling
    /// </summary>
    protected async Task<T> ExecuteWithTrackingAsync<T>(string operationName, Func<Task<T>> operation)
    {
        Logger.LogDebug("Starting operation: {OperationName}", operationName);
        
        return await PerformanceMetrics.RecordOperationAsync(operationName, async () =>
        {
            try
            {
                var result = await operation();
                Logger.LogDebug("Completed operation: {OperationName}", operationName);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed operation: {OperationName}", operationName);
                throw;
            }
        });
    }

    /// <summary>
    /// Validates performance against configured thresholds
    /// </summary>
    protected void ValidatePerformance(string operationName, int thresholdMs)
    {
        var summary = PerformanceMetrics.GetMetricSummary($"{operationName}_Duration_Ms");
        if (summary.Count > 0)
        {
            summary.P95.Should().BeLessOrEqualTo(thresholdMs, 
                $"Operation {operationName} P95 response time should be under {thresholdMs}ms");
            
            Output.WriteLine($"Performance validation passed for {operationName}: P95={summary.P95:F2}ms (threshold={thresholdMs}ms)");
        }
    }

    /// <summary>
    /// Validates that no errors occurred during test execution
    /// </summary>
    protected void ValidateNoErrors()
    {
        var report = PerformanceMetrics.GenerateReport();
        report.TotalErrors.Should().Be(0, "No errors should occur during test execution");
    }

    /// <summary>
    /// Creates a test project for the current test
    /// </summary>
    protected async Task<Project> CreateTestProjectAsync()
    {
        return await ExecuteWithTrackingAsync("CreateTestProject", async () =>
        {
            var projectRequest = TestDataBuilder<CreateProjectRequest>.CreateRealisticProject();
            var project = await ProjectManagementClient.CreateProjectAsync(TestCompanyId, projectRequest);
            
            Logger.LogInformation("Created test project: {ProjectId} - {ProjectName}", project.Id, project.Name);
            return project;
        });
    }

    /// <summary>
    /// Creates test data with automatic cleanup
    /// </summary>
    protected async Task<T> CreateTestDataAsync<T>(string dataKey, Func<Task<T>> factory) where T : class
    {
        return await Fixture.GetOrCreateTestDataAsync(dataKey, factory);
    }

    /// <summary>
    /// Asserts that an operation completes within the specified time
    /// </summary>
    protected async Task AssertCompletesWithinAsync(TimeSpan timeout, Func<Task> operation, string operationName)
    {
        var stopwatch = Stopwatch.StartNew();
        var timeoutTask = Task.Delay(timeout);
        var operationTask = operation();

        var completedTask = await Task.WhenAny(operationTask, timeoutTask);
        stopwatch.Stop();

        if (completedTask == timeoutTask)
        {
            throw new TimeoutException($"Operation '{operationName}' did not complete within {timeout.TotalSeconds} seconds");
        }

        await operationTask; // Re-await to get any exceptions
        
        Output.WriteLine($"Operation '{operationName}' completed in {stopwatch.ElapsedMilliseconds}ms");
    }

    /// <summary>
    /// Executes concurrent operations and validates results
    /// </summary>
    protected async Task<T[]> ExecuteConcurrentOperationsAsync<T>(
        string operationName, 
        Func<int, Task<T>> operation, 
        int concurrencyLevel)
    {
        Logger.LogInformation("Starting {ConcurrencyLevel} concurrent {OperationName} operations", 
            concurrencyLevel, operationName);

        var tasks = Enumerable.Range(0, concurrencyLevel)
            .Select(i => ExecuteWithTrackingAsync($"{operationName}_Concurrent_{i}", () => operation(i)))
            .ToArray();

        var results = await Task.WhenAll(tasks);
        
        Logger.LogInformation("Completed {ConcurrencyLevel} concurrent {OperationName} operations", 
            concurrencyLevel, operationName);

        return results;
    }

    /// <summary>
    /// Validates API response structure and data quality
    /// </summary>
    protected void ValidateApiResponse<T>(T response, string operationName) where T : class
    {
        response.Should().NotBeNull($"{operationName} should return a valid response");
        
        // Additional validation based on response type
        if (response is IEnumerable<object> collection)
        {
            collection.Should().NotBeNull($"{operationName} collection should not be null");
            Output.WriteLine($"{operationName} returned {collection.Count()} items");
        }
        
        if (response is IIdentifiable identifiable)
        {
            identifiable.Id.Should().BeGreaterThan(0, "Response should have a valid ID");
        }
    }

    /// <summary>
    /// Executes a batch of operations with rate limiting
    /// </summary>
    protected async Task<List<T>> ExecuteBatchOperationsAsync<T>(
        string operationName,
        IEnumerable<Func<Task<T>>> operations,
        int maxConcurrency = 5,
        TimeSpan? delayBetweenBatches = null)
    {
        var results = new List<T>();
        var operationsList = operations.ToList();
        
        Logger.LogInformation("Starting batch execution of {TotalOperations} {OperationName} operations with max concurrency {MaxConcurrency}",
            operationsList.Count, operationName, maxConcurrency);

        for (int i = 0; i < operationsList.Count; i += maxConcurrency)
        {
            var batch = operationsList.Skip(i).Take(maxConcurrency);
            var batchTasks = batch.Select((op, index) => 
                ExecuteWithTrackingAsync($"{operationName}_Batch_{i + index}", op));

            var batchResults = await Task.WhenAll(batchTasks);
            results.AddRange(batchResults);

            if (delayBetweenBatches.HasValue && i + maxConcurrency < operationsList.Count)
            {
                await Task.Delay(delayBetweenBatches.Value);
            }
        }

        Logger.LogInformation("Completed batch execution of {TotalOperations} {OperationName} operations",
            operationsList.Count, operationName);

        return results;
    }
}

/// <summary>
/// Marker interface for objects with an ID property
/// </summary>
public interface IIdentifiable
{
    int Id { get; }
}

/// <summary>
/// Collection name for live sandbox tests to ensure sequential execution
/// </summary>
[CollectionDefinition("LiveSandboxTests")]
public class LiveSandboxTestCollection : ICollectionFixture<LiveSandboxFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}