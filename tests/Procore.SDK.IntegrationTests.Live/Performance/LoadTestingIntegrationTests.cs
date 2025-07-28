using NBomber.Contracts;
using NBomber.CSharp;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Procore.SDK.IntegrationTests.Live.Performance;

/// <summary>
/// Performance and load testing integration tests using real Procore API
/// Tests response times, concurrent operations, and API rate limit compliance
/// </summary>
public class LoadTestingIntegrationTests : IntegrationTestBase
{
    public LoadTestingIntegrationTests(LiveSandboxFixture fixture, ITestOutputHelper output) 
        : base(fixture, output) { }

    #region Response Time Performance Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "ResponseTime")]
    public async Task API_Operations_Should_Meet_Response_Time_Targets()
    {
        await ExecuteWithTrackingAsync("ResponseTimeTargets", async () =>
        {
            var performanceResults = new Dictionary<string, double>();

            // Test authentication performance
            var authStopwatch = Stopwatch.StartNew();
            await Fixture.GetValidTokenAsync();
            authStopwatch.Stop();
            performanceResults["Authentication"] = authStopwatch.ElapsedMilliseconds;

            // Test Core API operations
            var coreStopwatch = Stopwatch.StartNew();
            var companies = await CoreClient.GetCompaniesAsync();
            coreStopwatch.Stop();
            performanceResults["GetCompanies"] = coreStopwatch.ElapsedMilliseconds;

            var userStopwatch = Stopwatch.StartNew();
            var currentUser = await CoreClient.GetCurrentUserAsync();
            userStopwatch.Stop();
            performanceResults["GetCurrentUser"] = userStopwatch.ElapsedMilliseconds;

            // Test QualitySafety operations
            var project = await CreateTestProjectAsync();
            
            var obsStopwatch = Stopwatch.StartNew();
            var observations = await QualitySafetyClient.GetObservationsAsync(TestCompanyId, project.Id);
            obsStopwatch.Stop();
            performanceResults["GetObservations"] = obsStopwatch.ElapsedMilliseconds;

            // Validate against performance thresholds
            foreach (var result in performanceResults)
            {
                var threshold = result.Key == "Authentication" 
                    ? TestConfig.PerformanceThresholds.AuthenticationMs
                    : TestConfig.PerformanceThresholds.ApiOperationMs;

                result.Value.Should().BeLessOrEqualTo(threshold, 
                    $"{result.Key} should complete within {threshold}ms, but took {result.Value}ms");

                Output.WriteLine($"✓ {result.Key}: {result.Value}ms (threshold: {threshold}ms)");
            }

            return performanceResults.Count;
        });
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "BulkOperations")]
    public async Task Bulk_Operations_Should_Complete_Within_Time_Limits()
    {
        var project = await CreateTestProjectAsync();

        await ExecuteWithTrackingAsync("BulkOperationsPerformance", async () =>
        {
            // Test bulk observation creation
            const int observationCount = 20;
            var observationRequests = Enumerable.Range(0, observationCount)
                .Select(_ => TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(project.Id))
                .ToList();

            var bulkStopwatch = Stopwatch.StartNew();
            
            var createdObservations = await ExecuteBatchOperationsAsync(
                "BulkObservationCreation",
                observationRequests.Select(req => 
                    new Func<Task<Observation>>(() => QualitySafetyClient.CreateObservationAsync(TestCompanyId, req))),
                maxConcurrency: 5,
                delayBetweenBatches: TimeSpan.FromMilliseconds(100));

            bulkStopwatch.Stop();

            // Validate bulk operation performance
            createdObservations.Should().HaveCount(observationCount, "All observations should be created");
            bulkStopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(TestConfig.PerformanceThresholds.BulkOperationMs,
                $"Bulk operation should complete within {TestConfig.PerformanceThresholds.BulkOperationMs}ms");

            Output.WriteLine($"✓ Created {observationCount} observations in {bulkStopwatch.ElapsedMilliseconds}ms");
            Output.WriteLine($"  Average per operation: {bulkStopwatch.ElapsedMilliseconds / observationCount}ms");

            return createdObservations.Count;
        });
    }

    #endregion

    #region Concurrent Operations Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "Concurrency")]
    public async Task Concurrent_API_Calls_Should_Handle_Multiple_Users()
    {
        await ExecuteWithTrackingAsync("ConcurrentApiCalls", async () =>
        {
            const int concurrentUsers = 10;
            const int operationsPerUser = 3;
            
            Output.WriteLine($"Starting concurrent test with {concurrentUsers} users, {operationsPerUser} operations each");

            var concurrentTasks = Enumerable.Range(0, concurrentUsers).Select(async userId =>
            {
                var userResults = new List<double>();
                
                for (int opIndex = 0; opIndex < operationsPerUser; opIndex++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    
                    try
                    {
                        // Vary the operations to simulate real usage
                        switch (opIndex % 3)
                        {
                            case 0:
                                await CoreClient.GetCompaniesAsync();
                                break;
                            case 1:
                                await CoreClient.GetCurrentUserAsync();
                                break;
                            case 2:
                                var users = await CoreClient.GetUsersAsync(TestCompanyId);
                                break;
                        }
                        
                        stopwatch.Stop();
                        userResults.Add(stopwatch.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        stopwatch.Stop();
                        Output.WriteLine($"User {userId}, Operation {opIndex} failed: {ex.Message}");
                        throw;
                    }

                    // Small delay to prevent overwhelming the API
                    await Task.Delay(50);
                }

                return new
                {
                    UserId = userId,
                    AverageResponseTime = userResults.Average(),
                    MaxResponseTime = userResults.Max(),
                    TotalOperations = userResults.Count
                };
            });

            var results = await Task.WhenAll(concurrentTasks);

            // Analyze concurrent performance
            var overallAverageMs = results.Average(r => r.AverageResponseTime);
            var maxResponseTimeMs = results.Max(r => r.MaxResponseTime);
            var totalSuccessfulOperations = results.Sum(r => r.TotalOperations);

            // Validate concurrent performance
            totalSuccessfulOperations.Should().Be(concurrentUsers * operationsPerUser, 
                "All concurrent operations should succeed");
            
            overallAverageMs.Should().BeLessOrEqualTo(TestConfig.PerformanceThresholds.ApiOperationMs * 1.5,
                "Average response time under load should be reasonable");

            Output.WriteLine($"✓ Concurrent test completed successfully:");
            Output.WriteLine($"  Total operations: {totalSuccessfulOperations}");
            Output.WriteLine($"  Average response time: {overallAverageMs:F2}ms");
            Output.WriteLine($"  Max response time: {maxResponseTimeMs:F2}ms");
            Output.WriteLine($"  Concurrent users: {concurrentUsers}");

            return totalSuccessfulOperations;
        });
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "Concurrency")]
    public async Task High_Concurrency_Operations_Should_Not_Cause_Resource_Exhaustion()
    {
        await ExecuteWithTrackingAsync("HighConcurrencyResourceTest", async () =>
        {
            const int highConcurrency = 25;
            
            // Monitor resource usage
            var initialMemory = GC.GetTotalMemory(false);
            var memoryReadings = new ConcurrentBag<long>();
            
            var concurrentOperations = Enumerable.Range(0, highConcurrency).Select(async index =>
            {
                try
                {
                    // Stagger the start times to simulate real-world usage
                    await Task.Delay(index * 20);
                    
                    var companies = await CoreClient.GetCompaniesAsync();
                    
                    // Record memory after operation
                    memoryReadings.Add(GC.GetTotalMemory(false));
                    
                    return new { Index = index, Success = true, CompanyCount = companies.Count() };
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"Operation {index} failed: {ex.Message}");
                    return new { Index = index, Success = false, CompanyCount = 0 };
                }
            });

            var results = await Task.WhenAll(concurrentOperations);
            
            // Force garbage collection and check final memory
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var finalMemory = GC.GetTotalMemory(true);

            // Analyze results
            var successfulOperations = results.Count(r => r.Success);
            var memoryIncreaseMB = (finalMemory - initialMemory) / (1024.0 * 1024.0);
            var maxMemoryDuringTestMB = memoryReadings.Any() ? memoryReadings.Max() / (1024.0 * 1024.0) : 0;

            // Validate resource usage
            successfulOperations.Should().BeGreaterThan(highConcurrency * 0.9, 
                "At least 90% of high concurrency operations should succeed");
            
            memoryIncreaseMB.Should().BeLessOrEqualTo(100, 
                "Memory increase should be reasonable (less than 100MB)");

            Output.WriteLine($"✓ High concurrency test completed:");
            Output.WriteLine($"  Successful operations: {successfulOperations}/{highConcurrency}");
            Output.WriteLine($"  Memory increase: {memoryIncreaseMB:F2}MB");
            Output.WriteLine($"  Max memory during test: {maxMemoryDuringTestMB:F2}MB");

            return successfulOperations;
        });
    }

    #endregion

    #region Rate Limiting Compliance Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "RateLimiting")]
    public async Task API_Rate_Limits_Should_Be_Respected()
    {
        await ExecuteWithTrackingAsync("RateLimitCompliance", async () =>
        {
            const int requestCount = 50;
            const int maxRequestsPerSecond = 10; // Conservative estimate
            
            var requestTimes = new List<DateTime>();
            var responses = new List<bool>();
            var rateLimitHits = 0;

            Output.WriteLine($"Testing rate limit compliance with {requestCount} requests");

            for (int i = 0; i < requestCount; i++)
            {
                var requestStart = DateTime.UtcNow;
                requestTimes.Add(requestStart);

                try
                {
                    await CoreClient.GetCompaniesAsync();
                    responses.Add(true);
                }
                catch (ProcoreApiException ex) when (ex.StatusCode == 429)
                {
                    // Rate limited - this is expected behavior
                    rateLimitHits++;
                    responses.Add(false);
                    
                    Output.WriteLine($"Request {i + 1}: Rate limited (429) - backing off");
                    
                    // Back off when rate limited
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"Request {i + 1}: Unexpected error - {ex.Message}");
                    responses.Add(false);
                }

                // Prevent overwhelming the API
                if (i % maxRequestsPerSecond == 0 && i > 0)
                {
                    await Task.Delay(1000);
                }
            }

            // Analyze rate limiting behavior
            var successfulRequests = responses.Count(r => r);
            var failedRequests = responses.Count(r => !r);
            var totalDuration = requestTimes.Last() - requestTimes.First();
            var averageRequestsPerSecond = requestCount / totalDuration.TotalSeconds;

            Output.WriteLine($"✓ Rate limit compliance test completed:");
            Output.WriteLine($"  Successful requests: {successfulRequests}");
            Output.WriteLine($"  Failed requests: {failedRequests}");
            Output.WriteLine($"  Rate limit hits (429): {rateLimitHits}");
            Output.WriteLine($"  Average rate: {averageRequestsPerSecond:F2} requests/second");
            Output.WriteLine($"  Total duration: {totalDuration.TotalSeconds:F2} seconds");

            // Validate rate limiting behavior
            successfulRequests.Should().BeGreaterThan(0, "Some requests should succeed");
            
            if (rateLimitHits > 0)
            {
                Output.WriteLine("Rate limiting detected and handled appropriately");
            }

            return successfulRequests;
        });
    }

    #endregion

    #region Memory and Resource Efficiency Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "MemoryEfficiency")]
    public async Task Memory_Usage_Should_Remain_Stable_During_Extended_Operations()
    {
        await ExecuteWithTrackingAsync("MemoryStabilityTest", async () =>
        {
            const int operationCycles = 10;
            const int operationsPerCycle = 20;
            
            var memoryReadings = new List<long>();
            var initialMemory = GC.GetTotalMemory(true); // Force GC first
            memoryReadings.Add(initialMemory);

            Output.WriteLine($"Starting memory stability test: {operationCycles} cycles of {operationsPerCycle} operations");
            Output.WriteLine($"Initial memory: {initialMemory / (1024.0 * 1024.0):F2}MB");

            for (int cycle = 0; cycle < operationCycles; cycle++)
            {
                // Perform a batch of operations
                var cycleTasks = Enumerable.Range(0, operationsPerCycle).Select(async opIndex =>
                {
                    try
                    {
                        // Mix different types of operations
                        switch (opIndex % 4)
                        {
                            case 0:
                                await CoreClient.GetCompaniesAsync();
                                break;
                            case 1:
                                await CoreClient.GetCurrentUserAsync();
                                break;
                            case 2:
                                await CoreClient.GetUsersAsync(TestCompanyId);
                                break;
                            case 3:
                                await CoreClient.GetDocumentsAsync(TestCompanyId);
                                break;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine($"Cycle {cycle}, Operation {opIndex} failed: {ex.Message}");
                        return false;
                    }
                });

                var cycleResults = await Task.WhenAll(cycleTasks);
                var successfulInCycle = cycleResults.Count(r => r);

                // Record memory after each cycle
                var memoryAfterCycle = GC.GetTotalMemory(false);
                memoryReadings.Add(memoryAfterCycle);

                Output.WriteLine($"Cycle {cycle + 1}: {successfulInCycle}/{operationsPerCycle} successful, " +
                               $"Memory: {memoryAfterCycle / (1024.0 * 1024.0):F2}MB");

                // Small delay between cycles
                await Task.Delay(100);
            }

            // Force final garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var finalMemory = GC.GetTotalMemory(true);
            memoryReadings.Add(finalMemory);

            // Analyze memory usage patterns
            var maxMemory = memoryReadings.Max();
            var memoryGrowth = finalMemory - initialMemory;
            var memoryGrowthMB = memoryGrowth / (1024.0 * 1024.0);
            var maxMemoryMB = maxMemory / (1024.0 * 1024.0);

            Output.WriteLine($"✓ Memory stability test completed:");
            Output.WriteLine($"  Initial memory: {initialMemory / (1024.0 * 1024.0):F2}MB");
            Output.WriteLine($"  Final memory: {finalMemory / (1024.0 * 1024.0):F2}MB");
            Output.WriteLine($"  Max memory: {maxMemoryMB:F2}MB");
            Output.WriteLine($"  Memory growth: {memoryGrowthMB:F2}MB");

            // Validate memory stability
            memoryGrowthMB.Should().BeLessOrEqualTo(50, "Memory growth should be reasonable (less than 50MB)");
            maxMemoryMB.Should().BeLessOrEqualTo(200, "Peak memory usage should be reasonable (less than 200MB)");

            return memoryReadings.Count;
        });
    }

    #endregion

    #region NBomber Load Testing Integration

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "LoadTesting")]
    [Trait("External", "NBomber")]
    public async Task NBomber_Load_Test_Should_Validate_API_Under_Realistic_Load()
    {
        // This test uses NBomber for more sophisticated load testing
        await ExecuteWithTrackingAsync("NBomberLoadTest", async () =>
        {
            var scenario = Scenario.Create("procore_api_load_test", async context =>
            {
                try
                {
                    // Simulate realistic user behavior
                    var operation = context.Data % 3;
                    
                    switch (operation)
                    {
                        case 0:
                            var companies = await CoreClient.GetCompaniesAsync();
                            return companies.Any() ? Response.Ok(statusCode: 200, sizeBytes: companies.Count() * 100) 
                                                  : Response.Fail("No companies returned");
                        
                        case 1:
                            var currentUser = await CoreClient.GetCurrentUserAsync();
                            return currentUser != null ? Response.Ok(statusCode: 200, sizeBytes: 500)
                                                       : Response.Fail("No current user returned");
                        
                        case 2:
                            var users = await CoreClient.GetUsersAsync(TestCompanyId);
                            return users.Any() ? Response.Ok(statusCode: 200, sizeBytes: users.Count() * 200)
                                               : Response.Fail("No users returned");
                        
                        default:
                            return Response.Fail("Invalid operation");
                    }
                }
                catch (ProcoreApiException ex) when (ex.StatusCode == 429)
                {
                    // Rate limiting is acceptable
                    return Response.Ok(statusCode: 429, message: "Rate limited");
                }
                catch (Exception ex)
                {
                    return Response.Fail(ex.Message);
                }
            })
            .WithLoadSimulations(
                Simulation.InjectPerSec(rate: 5, during: TimeSpan.FromSeconds(30)), // Ramp up slowly
                Simulation.KeepConstant(copies: 3, during: TimeSpan.FromSeconds(60))  // Sustained light load
            );

            var stats = NBomberRunner
                .RegisterScenarios(scenario)
                .WithReportFolder("performance-reports")
                .WithReportFormats(ReportFormat.Html, ReportFormat.Csv)
                .Run();

            // Validate load test results
            var scStats = stats.AllScenarios.First();
            
            Output.WriteLine($"✓ NBomber load test completed:");
            Output.WriteLine($"  Total requests: {scStats.Ok.Request.Count + scStats.Fail.Request.Count}");
            Output.WriteLine($"  Successful requests: {scStats.Ok.Request.Count}");
            Output.WriteLine($"  Failed requests: {scStats.Fail.Request.Count}");
            Output.WriteLine($"  Average response time: {scStats.Ok.Response.Mean}ms");
            Output.WriteLine($"  95th percentile: {scStats.Ok.Response.Percentile95}ms");
            Output.WriteLine($"  Success rate: {(scStats.Ok.Request.Count / (double)(scStats.Ok.Request.Count + scStats.Fail.Request.Count)) * 100:F2}%");

            // Assert load test success criteria
            var successRate = scStats.Ok.Request.Count / (double)(scStats.Ok.Request.Count + scStats.Fail.Request.Count);
            successRate.Should().BeGreaterThan(0.8, "Success rate should be above 80% under load");
            
            scStats.Ok.Response.Mean.Should().BeLessOrEqualTo(TestConfig.PerformanceThresholds.ApiOperationMs * 2,
                "Average response time under load should be reasonable");

            await Task.CompletedTask; // Ensure async signature
            return scStats.Ok.Request.Count;
        });
    }

    #endregion

    #region Performance Regression Detection

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "Performance")]
    [Trait("TestType", "Regression")]
    public async Task Performance_Should_Not_Regress_From_Baseline()
    {
        await ExecuteWithTrackingAsync("PerformanceRegressionTest", async () =>
        {
            // Define baseline performance expectations (these would typically come from previous test runs)
            var baselineMetrics = new Dictionary<string, double>
            {
                ["GetCompanies"] = TestConfig.PerformanceThresholds.ApiOperationMs * 0.8, // 80% of threshold
                ["GetCurrentUser"] = TestConfig.PerformanceThresholds.ApiOperationMs * 0.6, // 60% of threshold
                ["GetUsers"] = TestConfig.PerformanceThresholds.ApiOperationMs * 0.9 // 90% of threshold
            };

            var actualMetrics = new Dictionary<string, List<double>>();
            const int measurements = 5;

            // Take multiple measurements for statistical significance
            for (int i = 0; i < measurements; i++)
            {
                // GetCompanies performance
                var companiesStopwatch = Stopwatch.StartNew();
                await CoreClient.GetCompaniesAsync();
                companiesStopwatch.Stop();
                
                actualMetrics.GetOrAddValue("GetCompanies", []).Add(companiesStopwatch.ElapsedMilliseconds);

                // GetCurrentUser performance
                var userStopwatch = Stopwatch.StartNew();
                await CoreClient.GetCurrentUserAsync();
                userStopwatch.Stop();
                
                actualMetrics.GetOrAddValue("GetCurrentUser", []).Add(userStopwatch.ElapsedMilliseconds);

                // GetUsers performance
                var usersStopwatch = Stopwatch.StartNew();
                await CoreClient.GetUsersAsync(TestCompanyId);
                usersStopwatch.Stop();
                
                actualMetrics.GetOrAddValue("GetUsers", []).Add(usersStopwatch.ElapsedMilliseconds);

                // Small delay between measurements
                await Task.Delay(100);
            }

            // Analyze performance against baseline
            var regressionResults = new List<string>();
            
            foreach (var baselineMetric in baselineMetrics)
            {
                var operation = baselineMetric.Key;
                var baselineMs = baselineMetric.Value;
                var actualMeasurements = actualMetrics[operation];
                var averageActualMs = actualMeasurements.Average();
                var p95ActualMs = actualMeasurements.OrderBy(x => x).Skip((int)(measurements * 0.95)).First();

                var regressionThreshold = baselineMs * 1.2; // Allow 20% degradation
                
                Output.WriteLine($"{operation}:");
                Output.WriteLine($"  Baseline: {baselineMs:F2}ms");
                Output.WriteLine($"  Average: {averageActualMs:F2}ms");
                Output.WriteLine($"  P95: {p95ActualMs:F2}ms");
                Output.WriteLine($"  Regression threshold: {regressionThreshold:F2}ms");

                if (averageActualMs > regressionThreshold)
                {
                    regressionResults.Add($"{operation}: {averageActualMs:F2}ms > {regressionThreshold:F2}ms (baseline: {baselineMs:F2}ms)");
                }
                else
                {
                    Output.WriteLine($"  ✓ No regression detected");
                }
            }

            // Validate no significant performance regression
            regressionResults.Should().BeEmpty($"Performance regressions detected: {string.Join(", ", regressionResults)}");

            Output.WriteLine($"✓ Performance regression test passed - no significant degradation detected");

            return actualMetrics.Count;
        });
    }

    #endregion
}

// Extension method for dictionary operations
public static class DictionaryExtensions
{
    public static TValue GetOrAddValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) 
        where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = defaultValue;
            dictionary[key] = value;
        }
        return value;
    }
}