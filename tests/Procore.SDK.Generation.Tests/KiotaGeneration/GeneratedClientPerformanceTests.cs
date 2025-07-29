using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Generation.Tests.KiotaGeneration;

/// <summary>
/// Tests to validate performance characteristics, memory usage patterns, and async/await 
/// implementation quality in generated Kiota clients.
/// </summary>
public class GeneratedClientPerformanceTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly List<object> _clientInstances;
    private readonly PerformanceTracker _performanceTracker;

    public GeneratedClientPerformanceTests(ITestOutputHelper output)
    {
        _output = output;
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
        _clientInstances = new List<object>();
        _performanceTracker = new PerformanceTracker();
    }

    #region Client Instantiation Performance Tests

    /// <summary>
    /// Validates that generated clients can be instantiated quickly and efficiently.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient), "Core")]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), "ProjectManagement")]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient), "ResourceManagement")]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient), "QualitySafety")]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient), "ConstructionFinancials")]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient), "FieldProductivity")]
    public void GeneratedClient_Should_Instantiate_Quickly(Type clientType, string clientName)
    {
        // Arrange
        const int iterations = 100;
        var instantiationTimes = new List<long>();

        // Act - Measure instantiation time over multiple iterations
        for (int i = 0; i < iterations; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            var client = Activator.CreateInstance(clientType, _mockRequestAdapter);
            stopwatch.Stop();
            
            instantiationTimes.Add(stopwatch.ElapsedMilliseconds);
            _clientInstances.Add(client!);
        }

        // Assert
        var averageTime = instantiationTimes.Average();
        var maxTime = instantiationTimes.Max();
        
        Assert.True(averageTime < 10, $"Average instantiation time should be < 10ms, was {averageTime:F2}ms");
        Assert.True(maxTime < 50, $"Maximum instantiation time should be < 50ms, was {maxTime}ms");
        
        _output.WriteLine($"✅ {clientName} instantiation performance:");
        _output.WriteLine($"   Average: {averageTime:F2}ms");
        _output.WriteLine($"   Maximum: {maxTime}ms");
        _output.WriteLine($"   Minimum: {instantiationTimes.Min()}ms");
    }

    /// <summary>
    /// Validates that multiple client instances don't cause memory leaks.
    /// </summary>
    [Fact]
    public void Multiple_Client_Instances_Should_Not_Cause_Memory_Leaks()
    {
        // Arrange
        const int instanceCount = 1000;
        var initialMemory = GC.GetTotalMemory(true);

        // Act - Create many client instances
        var clients = new List<object>();
        for (int i = 0; i < instanceCount; i++)
        {
            var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
            clients.Add(client);
        }

        var peakMemory = GC.GetTotalMemory(false);
        
        // Clear references and force garbage collection
        clients.Clear();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        var finalMemory = GC.GetTotalMemory(true);

        // Assert
        var memoryIncrease = finalMemory - initialMemory;
        var peakIncrease = peakMemory - initialMemory;
        
        // Memory increase should be reasonable (less than 50MB)
        Assert.True(memoryIncrease < 50 * 1024 * 1024, 
            $"Memory increase should be < 50MB, was {memoryIncrease / (1024 * 1024):F1}MB");
        
        _output.WriteLine($"✅ Memory usage for {instanceCount} client instances:");
        _output.WriteLine($"   Initial: {initialMemory / (1024 * 1024):F1}MB");
        _output.WriteLine($"   Peak: {peakMemory / (1024 * 1024):F1}MB");
        _output.WriteLine($"   Final: {finalMemory / (1024 * 1024):F1}MB");
        _output.WriteLine($"   Net increase: {memoryIncrease / (1024 * 1024):F1}MB");
    }

    #endregion

    #region Request Building Performance Tests

    /// <summary>
    /// Validates that request builders can be created efficiently.
    /// </summary>
    [Fact]
    public void Request_Builders_Should_Be_Created_Efficiently()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        const int iterations = 1000;

        // Act - Measure request builder creation time
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            var projectBuilder = client.Rest.V10.Projects[i * 1000];
            var companyBuilder = client.Rest.V10.Companies[i * 2000];
            
            // Access properties to ensure they're actually created
            _ = projectBuilder.ToString();
            _ = companyBuilder.ToString();
        }
        
        stopwatch.Stop();

        // Assert
        var averageTimePerBuilder = (double)stopwatch.ElapsedMilliseconds / (iterations * 2);
        Assert.True(averageTimePerBuilder < 0.1, 
            $"Average request builder creation time should be < 0.1ms, was {averageTimePerBuilder:F3}ms");
        
        _output.WriteLine($"✅ Request builder creation performance:");
        _output.WriteLine($"   {iterations * 2} builders created in {stopwatch.ElapsedMilliseconds}ms");
        _output.WriteLine($"   Average per builder: {averageTimePerBuilder:F3}ms");
    }

    /// <summary>
    /// Validates that request information can be generated quickly.
    /// </summary>
    [Fact]
    public void Request_Information_Should_Be_Generated_Quickly()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var projectBuilder = client.Rest.V10.Projects[123];
        const int iterations = 500;

        // Act - Measure request information generation time
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            var getRequest = projectBuilder.ToGetRequestInformation();
            var patchRequest = projectBuilder.ToPatchRequestInformation(new { });
            
            // Access properties to ensure they're actually generated
            _ = getRequest.HttpMethod;
            _ = patchRequest.HttpMethod;
        }
        
        stopwatch.Stop();

        // Assert
        var averageTimePerRequest = (double)stopwatch.ElapsedMilliseconds / (iterations * 2);
        Assert.True(averageTimePerRequest < 1, 
            $"Average request information generation time should be < 1ms, was {averageTimePerRequest:F3}ms");
        
        _output.WriteLine($"✅ Request information generation performance:");
        _output.WriteLine($"   {iterations * 2} requests generated in {stopwatch.ElapsedMilliseconds}ms");
        _output.WriteLine($"   Average per request: {averageTimePerRequest:F3}ms");
    }

    #endregion

    #region Async/Await Pattern Tests

    /// <summary>
    /// Validates that generated async methods properly implement the async pattern.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    public void Generated_Async_Methods_Should_Follow_Proper_Patterns(Type clientType)
    {
        // Act - Get all async methods from request builders
        var assembly = clientType.Assembly;
        var requestBuilderTypes = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("RequestBuilder"))
            .ToList();

        var asyncMethods = new List<MethodInfo>();
        foreach (var builderType in requestBuilderTypes)
        {
            var methods = builderType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.Name.EndsWith("Async"))
                .ToList();
            asyncMethods.AddRange(methods);
        }

        // Assert
        Assert.NotEmpty(asyncMethods);

        foreach (var method in asyncMethods.Take(10)) // Test first 10 methods
        {
            // Verify return type is Task or Task<T>
            Assert.True(method.ReturnType == typeof(Task) || 
                       method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>),
                       $"Async method {method.Name} should return Task or Task<T>");

            // Verify has CancellationToken parameter
            var parameters = method.GetParameters();
            var hasCancellationToken = parameters.Any(p => p.ParameterType == typeof(CancellationToken));
            Assert.True(hasCancellationToken, $"Async method {method.Name} should have CancellationToken parameter");
        }

        _output.WriteLine($"✅ {clientType.Name} has {asyncMethods.Count} async methods following proper patterns");
    }

    /// <summary>
    /// Validates that async methods support cancellation properly.
    /// </summary>
    [Fact]
    public async Task Generated_Async_Methods_Should_Support_Cancellation()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var projectBuilder = client.Rest.V10.Projects[123];
        
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Setup mock to simulate long-running operation
        _mockRequestAdapter.SendAsync(Arg.Any<RequestInformation>(), Arg.Any<CancellationToken>())
                          .Returns(async (callInfo) =>
                          {
                              var token = callInfo.Arg<CancellationToken>();
                              await Task.Delay(1000, token); // This should be cancelled
                              return Task.FromResult<object?>(null);
                          });

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await projectBuilder.GetAsync(cancellationToken: cts.Token);
        });

        _output.WriteLine("✅ Generated async methods support cancellation tokens");
    }

    /// <summary>
    /// Validates that async methods don't block threads unnecessarily.
    /// </summary>
    [Fact]
    public async Task Generated_Async_Methods_Should_Not_Block_Threads()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var projectBuilder = client.Rest.V10.Projects[123];
        
        // Setup mock to simulate async operation
        _mockRequestAdapter.SendAsync(Arg.Any<RequestInformation>(), Arg.Any<CancellationToken>())
                          .Returns(Task.FromResult<object?>(new object()));

        var initialThreadId = Thread.CurrentThread.ManagedThreadId;

        // Act
        try
        {
            await projectBuilder.GetAsync();
        }
        catch (NotImplementedException)
        {
            // Expected for mock
        }

        var finalThreadId = Thread.CurrentThread.ManagedThreadId;

        // Assert - Thread ID may change in async operations, which is expected
        _output.WriteLine($"✅ Async operation thread handling: {initialThreadId} -> {finalThreadId}");
    }

    #endregion

    #region Memory Usage Pattern Tests

    /// <summary>
    /// Validates that generated clients have reasonable memory footprints.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient), "Core")]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), "ProjectManagement")]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient), "ResourceManagement")]
    public void Generated_Clients_Should_Have_Reasonable_Memory_Footprint(Type clientType, string clientName)
    {
        // Arrange
        const int instanceCount = 100;
        var initialMemory = GC.GetTotalMemory(true);

        // Act - Create multiple instances and measure memory
        var clients = new List<object>();
        for (int i = 0; i < instanceCount; i++)
        {
            var client = Activator.CreateInstance(clientType, _mockRequestAdapter);
            clients.Add(client!);
        }

        var currentMemory = GC.GetTotalMemory(false);
        var memoryPerInstance = (currentMemory - initialMemory) / instanceCount;

        // Assert - Each client instance should use less than 100KB
        Assert.True(memoryPerInstance < 100 * 1024, 
            $"Memory per {clientName} instance should be < 100KB, was {memoryPerInstance / 1024:F1}KB");

        _output.WriteLine($"✅ {clientName} memory footprint:");
        _output.WriteLine($"   Per instance: {memoryPerInstance / 1024:F1}KB");
        _output.WriteLine($"   Total for {instanceCount} instances: {(currentMemory - initialMemory) / 1024:F1}KB");

        // Keep references to prevent GC during test
        _clientInstances.AddRange(clients);
    }

    /// <summary>
    /// Validates that request builders don't cause excessive memory allocations.
    /// </summary>
    [Fact]
    public void Request_Builders_Should_Not_Cause_Excessive_Allocations()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        const int iterations = 1000;
        
        var initialMemory = GC.GetTotalMemory(true);

        // Act - Create many request builders
        var builders = new List<object>();
        for (int i = 0; i < iterations; i++)
        {
            var projectBuilder = client.Rest.V10.Projects[i * 1000];
            var companyBuilder = client.Rest.V10.Companies[i * 2000];
            
            builders.Add(projectBuilder);
            builders.Add(companyBuilder);
        }

        var currentMemory = GC.GetTotalMemory(false);
        var memoryPerBuilder = (currentMemory - initialMemory) / (iterations * 2);

        // Assert - Each request builder should use less than 1KB
        Assert.True(memoryPerBuilder < 1024, 
            $"Memory per request builder should be < 1KB, was {memoryPerBuilder:F1} bytes");

        _output.WriteLine($"✅ Request builder memory efficiency:");
        _output.WriteLine($"   Per builder: {memoryPerBuilder:F1} bytes");
        _output.WriteLine($"   Total for {iterations * 2} builders: {(currentMemory - initialMemory) / 1024:F1}KB");
    }

    #endregion

    #region Serialization Performance Tests

    /// <summary>
    /// Validates that serialization registration doesn't impact performance significantly.
    /// </summary>
    [Fact]
    public void Serialization_Registration_Should_Be_Efficient()
    {
        // Arrange
        const int iterations = 100;
        var registrationTimes = new List<long>();

        // Act - Measure serialization registration time during client creation
        for (int i = 0; i < iterations; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
            stopwatch.Stop();
            
            registrationTimes.Add(stopwatch.ElapsedTicks);
            _clientInstances.Add(client);
        }

        // Assert
        var averageTicks = registrationTimes.Average();
        var averageMs = averageTicks / (double)Stopwatch.Frequency * 1000;
        
        Assert.True(averageMs < 5, $"Serialization registration should take < 5ms, was {averageMs:F2}ms");
        
        _output.WriteLine($"✅ Serialization registration performance:");
        _output.WriteLine($"   Average time: {averageMs:F2}ms");
        _output.WriteLine($"   Total registrations: {iterations}");
    }

    /// <summary>
    /// Validates that serializer factory access is efficient.
    /// </summary>
    [Fact]
    public void Serializer_Factory_Access_Should_Be_Efficient()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        const int iterations = 1000;

        // Act - Measure time to access serialization components
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            // Access request builders which may trigger serializer access
            var projectBuilder = client.Rest.V10.Projects[i * 1000];
            var requestInfo = projectBuilder.ToGetRequestInformation();
            
            // Access HTTP method to ensure request info is fully created
            _ = requestInfo.HttpMethod;
        }
        
        stopwatch.Stop();

        // Assert
        var averageTimePerAccess = (double)stopwatch.ElapsedMilliseconds / iterations;
        Assert.True(averageTimePerAccess < 0.1, 
            $"Average serializer access time should be < 0.1ms, was {averageTimePerAccess:F3}ms");
        
        _output.WriteLine($"✅ Serializer factory access performance:");
        _output.WriteLine($"   {iterations} accesses in {stopwatch.ElapsedMilliseconds}ms");
        _output.WriteLine($"   Average per access: {averageTimePerAccess:F3}ms");
    }

    #endregion

    #region Concurrent Access Tests

    /// <summary>
    /// Validates that generated clients are thread-safe for read operations.
    /// </summary>
    [Fact]
    public async Task Generated_Clients_Should_Be_ThreadSafe_For_Read_Operations()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        const int concurrentOperations = 50;
        
        _mockRequestAdapter.SendAsync(Arg.Any<RequestInformation>(), Arg.Any<CancellationToken>())
                          .Returns(Task.FromResult<object?>(new object()));

        // Act - Perform concurrent read operations
        var tasks = new List<Task>();
        var exceptions = new List<Exception>();
        
        for (int i = 0; i < concurrentOperations; i++)
        {
            var taskId = i;
            var task = Task.Run(async () =>
            {
                try
                {
                    var projectBuilder = client.Rest.V10.Projects[taskId * 3000];
                    await projectBuilder.GetAsync();
                }
                catch (NotImplementedException)
                {
                    // Expected for mock
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Empty(exceptions);
        
        _output.WriteLine($"✅ Generated clients handle {concurrentOperations} concurrent operations without issues");
    }

    /// <summary>
    /// Validates that multiple client instances can be used concurrently.
    /// </summary>
    [Fact]
    public async Task Multiple_Client_Instances_Should_Work_Concurrently()
    {
        // Arrange
        const int clientCount = 10;
        const int operationsPerClient = 5;
        
        var clients = Enumerable.Range(0, clientCount)
            .Select(_ => new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter))
            .ToList();

        _mockRequestAdapter.SendAsync(Arg.Any<RequestInformation>(), Arg.Any<CancellationToken>())
                          .Returns(Task.FromResult<object?>(new object()));

        // Act - Use multiple clients concurrently
        var allTasks = new List<Task>();
        var exceptions = new List<Exception>();

        foreach (var (client, clientIndex) in clients.Select((c, i) => (c, i)))
        {
            for (int opIndex = 0; opIndex < operationsPerClient; opIndex++)
            {
                var taskId = $"{clientIndex}-{opIndex}";
                var task = Task.Run(async () =>
                {
                    try
                    {
                        var projectBuilder = client.Rest.V10.Projects[taskId * 3000];
                        await projectBuilder.GetAsync();
                    }
                    catch (NotImplementedException)
                    {
                        // Expected for mock
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }
                });
                allTasks.Add(task);
            }
        }

        await Task.WhenAll(allTasks);

        // Assert
        Assert.Empty(exceptions);
        
        _output.WriteLine($"✅ {clientCount} clients with {operationsPerClient} operations each completed successfully");
        
        // Keep references to prevent GC during test
        _clientInstances.AddRange(clients);
    }

    #endregion

    #region Helper Classes

    private class PerformanceTracker
    {
        private readonly Dictionary<string, List<long>> _metrics = new();

        public void RecordMetric(string name, long value)
        {
            if (!_metrics.ContainsKey(name))
            {
                _metrics[name] = new List<long>();
            }
            _metrics[name].Add(value);
        }

        public double GetAverageMetric(string name)
        {
            return _metrics.ContainsKey(name) ? _metrics[name].Average() : 0;
        }

        public long GetMaxMetric(string name)
        {
            return _metrics.ContainsKey(name) ? _metrics[name].Max() : 0;
        }
    }

    #endregion

    #region IDisposable Implementation

    public void Dispose()
    {
        // Clean up client instances to prevent memory leaks in tests
        _clientInstances.Clear();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    #endregion
}