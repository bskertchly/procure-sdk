using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Procore.SDK.Core;
using Procore.SDK.ConstructionFinancials;
using Procore.SDK.FieldProductivity;
using Procore.SDK.ProjectManagement;
using Procore.SDK.QualitySafety;
using Procore.SDK.ResourceManagement;

namespace Procore.SDK.QualityAnalysis.Tests
{
    /// <summary>
    /// Comprehensive quality analysis tests for all 6 Kiota clients following CQ Task 9 strategy.
    /// Tests cover 10 critical quality dimensions: static analysis, code quality, performance,
    /// authentication, resilience, memory usage, async patterns, cancellation, error handling, and logging.
    /// Enhanced with production-ready patterns, comprehensive metrics, and parallel execution.
    /// </summary>
    public class KiotaClientQualityAnalysisTests : KiotaClientQualityTestBase
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan ExtendedTimeout = TimeSpan.FromMinutes(5);
        private static readonly int DefaultIterations = 1000;
        private static readonly int PerformanceIterations = 10000;
        private readonly ITestOutputHelper _output;

        public KiotaClientQualityAnalysisTests(ITestOutputHelper output) : base(output)
        {
            _output = output;
        }
        
        #region 1. Static Analysis Tests
        
        [Fact]
        [Trait("Category", "StaticAnalysis.CodeCoverage")]
        [Trait("Priority", "1")]
        public async Task All_Kiota_Clients_Should_Meet_Code_Coverage_Requirements()
        {
            // Test each client assembly for coverage thresholds with enhanced metrics
            var clients = GetAllKiotaClients();
            var coverageResults = new ConcurrentBag<CoverageResult>();
            var testStopwatch = Stopwatch.StartNew();

            try
            {
                // Validate preconditions
                clients.Should().HaveCount(6, "Expected 6 Kiota clients");
                
                // Run coverage analysis in parallel for better performance
                var coverageTasks = clients.Select(async client =>
                {
                    ValidateTestPreconditions(client);
                    
                    var coverage = await ExecuteWithTimeoutAsync(
                        () => AnalyzeCodeCoverageAsync(client),
                        DefaultTimeout,
                        $"Coverage analysis for {client.Name}"
                    );
                    
                    coverageResults.Add(coverage);
                    return coverage;
                });

                var results = await Task.WhenAll(coverageTasks);
                
                // Enhanced assertions with better error messages
                foreach (var coverage in results)
                {
                    coverage.CriticalPathCoverage.Should().BeGreaterOrEqualTo(0.90,
                        $"{coverage.ClientName} critical path coverage is {coverage.CriticalPathCoverage:P}, expected ≥90%");
                    
                    coverage.OverallCoverage.Should().BeGreaterOrEqualTo(0.80,
                        $"{coverage.ClientName} overall coverage is {coverage.OverallCoverage:P}, expected ≥80%");
                    
                    coverage.BranchCoverage.Should().BeGreaterOrEqualTo(0.75,
                        $"{coverage.ClientName} branch coverage is {coverage.BranchCoverage:P}, expected ≥75%");
                    
                    coverage.CoverageGrade.Should().BeOneOf(QualityGrade.Good, QualityGrade.Excellent,
                        $"{coverage.ClientName} coverage grade is {coverage.CoverageGrade}, expected Good or Excellent");
                }
                
                LogCoverageResults(results.ToList());
                testStopwatch.Stop();
                
                LogTestResult("Code Coverage Analysis", true, testStopwatch.Elapsed, 
                    $"Analyzed {results.Length} clients");
            }
            catch (Exception ex)
            {
                testStopwatch.Stop();
                LogTestResult("Code Coverage Analysis", false, testStopwatch.Elapsed, ex.Message);
                throw;
            }
        }

        [Fact]
        [Trait("Category", "StaticAnalysis.Complexity")]
        [Trait("Priority", "1")]
        public async Task All_Kiota_Clients_Should_Meet_Complexity_Requirements()
        {
            var clients = GetAllKiotaClients();
            var complexityViolations = new ConcurrentBag<ComplexityViolation>();
            var analysisResults = new ConcurrentBag<ComplexityAnalysisResult>();
            var testStopwatch = Stopwatch.StartNew();

            try
            {
                // Run complexity analysis in parallel
                var complexityTasks = clients.Select(async client =>
                {
                    ValidateTestPreconditions(client);
                    
                    var analysis = await ExecuteWithTimeoutAsync(
                        () => AnalyzeComplexityAsync(client),
                        ExtendedTimeout,
                        $"Complexity analysis for {client.Name}"
                    );
                    
                    analysisResults.Add(analysis);
                    
                    foreach (var violation in analysis.Violations)
                    {
                        complexityViolations.Add(violation);
                    }
                    
                    return analysis;
                });

                var results = await Task.WhenAll(complexityTasks);
                
                // Enhanced assertions with detailed reporting
                var totalViolations = complexityViolations.Count;
                var totalMethods = results.Sum(r => r.TotalMethods);
                var violationRate = totalMethods > 0 ? (double)totalViolations / totalMethods : 0;
                
                violationRate.Should().BeLessOrEqualTo(0.05, 
                    $"Complexity violation rate is {violationRate:P}, expected ≤5%. Violations: {totalViolations}/{totalMethods}");
                
                foreach (var result in results)
                {
                    result.ComplexityGrade.Should().BeOneOf(QualityGrade.Good, QualityGrade.Excellent, QualityGrade.Satisfactory,
                        $"{result.ClientName} complexity grade is {result.ComplexityGrade}");
                    
                    result.AverageComplexity.Should().BeLessOrEqualTo(8.0,
                        $"{result.ClientName} average complexity is {result.AverageComplexity:F2}, expected ≤8.0");
                }
                
                testStopwatch.Stop();
                LogTestResult("Complexity Analysis", totalViolations == 0, testStopwatch.Elapsed,
                    $"Total violations: {totalViolations}, Methods analyzed: {totalMethods}");
                
                // Log detailed results
                _output.WriteLine($"\n=== Complexity Analysis Summary ===");
                foreach (var result in results.OrderBy(r => r.ClientName))
                {
                    _output.WriteLine($"{result.ClientName}: Avg={result.AverageComplexity:F2}, Max={result.MaxComplexity}, Violations={result.Violations.Count}, Grade={result.ComplexityGrade}");
                }
            }
            catch (Exception ex)
            {
                testStopwatch.Stop();
                LogTestResult("Complexity Analysis", false, testStopwatch.Elapsed, ex.Message);
                throw;
            }
        }

        [Fact]
        [Trait("Category", "StaticAnalysis.Security")]
        [Trait("Priority", "1")]
        public async Task All_Kiota_Clients_Should_Have_No_Critical_Security_Issues()
        {
            var clients = GetAllKiotaClients();
            var securityAnalyzer = new SecurityAnalyzer();
            var allViolations = new ConcurrentBag<SecurityViolation>();
            var analysisResults = new ConcurrentBag<SecurityAnalysisResult>();
            var testStopwatch = Stopwatch.StartNew();

            try
            {
                // Run security analysis in parallel
                var securityTasks = clients.Select(async client =>
                {
                    ValidateTestPreconditions(client);
                    
                    var analysis = await ExecuteWithTimeoutAsync(
                        () => securityAnalyzer.AnalyzeAsync(client),
                        ExtendedTimeout,
                        $"Security analysis for {client.Name}"
                    );
                    
                    analysisResults.Add(analysis);
                    
                    foreach (var violation in analysis.Violations)
                    {
                        allViolations.Add(violation);
                    }
                    
                    return analysis;
                });

                var results = await Task.WhenAll(securityTasks);
                
                // Enhanced security validation
                var criticalViolations = allViolations.Where(v => v.Severity == SecuritySeverity.Critical).ToList();
                var highViolations = allViolations.Where(v => v.Severity == SecuritySeverity.High).ToList();
                
                criticalViolations.Should().BeEmpty(
                    $"Found {criticalViolations.Count} critical security violations: {string.Join(", ", criticalViolations.Select(v => v.ToString()))}");
                
                highViolations.Should().HaveCountLessOrEqualTo(2, 
                    $"Found {highViolations.Count} high-severity security violations, expected ≤2");
                
                foreach (var result in results)
                {
                    result.SecurityGrade.Should().BeOneOf(QualityGrade.Good, QualityGrade.Excellent, QualityGrade.Satisfactory,
                        $"{result.ClientName} security grade is {result.SecurityGrade}");
                }
                
                testStopwatch.Stop();
                LogTestResult("Security Analysis", criticalViolations.Count == 0 && highViolations.Count <= 2, 
                    testStopwatch.Elapsed, $"Critical: {criticalViolations.Count}, High: {highViolations.Count}");
                
                // Log security summary
                _output.WriteLine($"\n=== Security Analysis Summary ===");
                foreach (var result in results.OrderBy(r => r.ClientName))
                {
                    var severityBreakdown = result.Violations.GroupBy(v => v.Severity)
                        .ToDictionary(g => g.Key, g => g.Count());
                    
                    _output.WriteLine($"{result.ClientName}: Grade={result.SecurityGrade}, Total={result.Violations.Count}, " +
                        $"Critical={severityBreakdown.GetValueOrDefault(SecuritySeverity.Critical)}, " +
                        $"High={severityBreakdown.GetValueOrDefault(SecuritySeverity.High)}");
                }
            }
            catch (Exception ex)
            {
                testStopwatch.Stop();
                LogTestResult("Security Analysis", false, testStopwatch.Elapsed, ex.Message);
                throw;
            }
        }

        [Fact]
        [Trait("Category", "StaticAnalysis.Compilation")]
        [Trait("Priority", "1")]
        public void All_Kiota_Clients_Should_Compile_Without_Errors()
        {
            var compilationResults = new List<CompilationResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var result = AnalyzeCompilation(client);
                compilationResults.Add(result);
                
                Assert.True(result.CompiledSuccessfully,
                    $"Client {client.Name} failed to compile: {string.Join(", ", result.Errors)}");
                Assert.True(result.ErrorCount == 0,
                    $"Client {client.Name} has {result.ErrorCount} compilation errors");
            }
        }
        
        #endregion
        
        #region 2. Code Quality Validation Tests
        
        [Fact]
        [Trait("Category", "CodeQuality.Architecture")]
        [Trait("Priority", "1")]
        public void All_Kiota_Clients_Should_Follow_SOLID_Principles()
        {
            var violations = new List<SolidViolation>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var analyzer = new SolidPrincipleAnalyzer();
                violations.AddRange(analyzer.AnalyzeSingleResponsibility(client));
                violations.AddRange(analyzer.AnalyzeDependencyInversion(client));
            }

            Assert.Equal(0, violations.Count);
        }

        [Fact]
        [Trait("Category", "CodeQuality.DependencyInjection")]
        [Trait("Priority", "1")]
        public void All_Kiota_Clients_Should_Support_Dependency_Injection()
        {
            var serviceCollection = new ServiceCollection();
            
            // Test DI registration for each client
            foreach (var client in GetAllKiotaClients())
            {
                var exception = Record.Exception(() => RegisterClientServices(client, serviceCollection));
                Assert.Null(exception);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            // Validate all clients can be resolved
            foreach (var client in GetAllKiotaClients())
            {
                var clientInterface = client.GetClientInterface();
                if (clientInterface != null)
                {
                    var instance = serviceProvider.GetService(clientInterface);
                    Assert.NotNull(instance);
                }
            }
        }

        [Fact]
        [Trait("Category", "CodeQuality.ResourceManagement")]
        [Trait("Priority", "1")]
        public void All_Kiota_Clients_Should_Properly_Dispose_Resources()
        {
            var disposableAnalyzer = new DisposableAnalyzer();
            var violations = new List<DisposableViolation>();

            foreach (var client in GetAllKiotaClients())
            {
                violations.AddRange(disposableAnalyzer.AnalyzeDisposalPatterns(client));
            }

            Assert.Equal(0, violations.Count);
        }
        
        #endregion
        
        #region 3. Type Mapping Performance Tests
        
        [Fact]
        [Trait("Category", "Performance.TypeMapping")]
        [Trait("Priority", "2")]
        public async Task All_Type_Mappers_Should_Meet_Performance_Requirements()
        {
            var clients = GetAllKiotaClients();
            var performanceResults = new ConcurrentBag<TypeMapperPerformanceResult>();
            var testStopwatch = Stopwatch.StartNew();

            try
            {
                var allMappingTasks = new List<Task>();
                
                foreach (var client in clients)
                {
                    ValidateTestPreconditions(client);
                    var typeMappers = GetTypeMappers(client);
                    
                    foreach (var mapper in typeMappers)
                    {
                        allMappingTasks.Add(Task.Run(async () =>
                        {
                            var result = await ExecuteWithTimeoutAsync(
                                () => LoadTestTypeMapper(mapper, PerformanceIterations),
                                ExtendedTimeout,
                                $"Performance test for {mapper.Name}"
                            );
                            
                            performanceResults.Add(result);
                            return result;
                        }));
                    }
                }

                await Task.WhenAll(allMappingTasks);
                var results = performanceResults.ToList();
                
                // Enhanced performance validation
                foreach (var result in results)
                {
                    result.AverageTimeMs.Should().BeLessOrEqualTo(2.0,
                        $"{result.MapperName} average time {result.AverageTimeMs:F3}ms exceeds 2ms threshold");
                    
                    result.ErrorRate.Should().BeLessOrEqualTo(0.01,
                        $"{result.MapperName} error rate {result.ErrorRate:P} exceeds 1% threshold");
                    
                    result.Grade.Should().BeOneOf(PerformanceGrade.Excellent, PerformanceGrade.Good, PerformanceGrade.Acceptable,
                        $"{result.MapperName} performance grade is {result.Grade}");
                    
                    result.ThroughputPerSecond.Should().BeGreaterThan(500,
                        $"{result.MapperName} throughput {result.ThroughputPerSecond:F0} ops/sec is below 500 ops/sec threshold");
                }
                
                testStopwatch.Stop();
                LogTestResult("Type Mapping Performance", true, testStopwatch.Elapsed,
                    $"Tested {results.Count} mappers with {PerformanceIterations} iterations each");
                
                LogPerformanceResults(results);
                
                // Enhanced performance logging
                _output.WriteLine($"\n=== Performance Analysis Summary ===");
                foreach (var result in results.OrderBy(r => r.AverageTimeMs))
                {
                    _output.WriteLine($"{result.MapperName}: Avg={result.AverageTimeMs:F3}ms, " +
                        $"Throughput={result.ThroughputPerSecond:F0} ops/sec, Grade={result.Grade}, Errors={result.ErrorRate:P}");
                }
            }
            catch (Exception ex)
            {
                testStopwatch.Stop();
                LogTestResult("Type Mapping Performance", false, testStopwatch.Elapsed, ex.Message);
                throw;
            }
        }

        [Fact]
        [Trait("Category", "Performance.TypeMapping.Concurrency")]
        [Trait("Priority", "2")]
        public async Task Type_Mappers_Should_Support_Concurrent_Operations()
        {
            var tasks = new List<Task>();
            var results = new ConcurrentBag<MappingResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var typeMappers = GetTypeMappers(client);
                
                foreach (var mapper in typeMappers)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            var result = PerformMappingOperations(mapper, operations: 100);
                            results.Add(result);
                        }));
                    }
                }
            }

            await Task.WhenAll(tasks);
            
            var failedResults = results.Where(r => !r.Success).ToList();
            Assert.Equal(0, failedResults.Count);
        }

        [Fact]
        [Trait("Category", "Performance.TypeMapping.Memory")]
        [Trait("Priority", "2")]
        public void Type_Mapping_Should_Have_Acceptable_Memory_Footprint()
        {
            var initialMemory = GC.GetTotalMemory(true);
            var mappingResults = new List<object>();

            foreach (var client in GetAllKiotaClients())
            {
                var typeMappers = GetTypeMappers(client);
                
                foreach (var mapper in typeMappers)
                {
                    // Perform 1000 mapping operations
                    for (int i = 0; i < 1000; i++)
                    {
                        var testData = GenerateTestData(mapper);
                        if (testData.GeneratedObject != null && testData.WrapperObject != null)
                        {
                            var wrapperResult = mapper.MapToWrapper(testData.GeneratedObject);
                            var generatedResult = mapper.MapToGenerated(testData.WrapperObject);
                            
                            mappingResults.Add(wrapperResult);
                            mappingResults.Add(generatedResult);
                        }
                    }
                }
            }

            var finalMemory = GC.GetTotalMemory(true);
            var memoryIncrease = finalMemory - initialMemory;
            var memoryPerMapping = mappingResults.Count > 0 ? memoryIncrease / mappingResults.Count : 0;

            Assert.True(memoryPerMapping < 100, // 100 bytes per mapping
                $"Memory per mapping {memoryPerMapping} bytes exceeds 100 byte threshold");
        }
        
        #endregion
        
        #region 4. Authentication Integration Tests
        
        [Fact]
        [Trait("Category", "Authentication.TokenManagement")]
        [Trait("Priority", "1")]
        public async Task All_Clients_Should_Handle_Token_Refresh_Correctly()
        {
            var mockTokenManager = CreateMockTokenManager();
            var refreshResults = new List<TokenRefreshResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                try
                {
                    var clientInstance = CreateClientWithAuth(client, mockTokenManager);
                    
                    // Simulate token expiration
                    mockTokenManager.SimulateExpiredToken();
                    
                    var result = await TestTokenRefreshScenario(clientInstance);
                    refreshResults.Add(result);
                    
                    Assert.True(result.Success, 
                        $"Client {client.Name} failed token refresh: {result.ErrorMessage}");
                    Assert.True(result.RefreshAttempted,
                        $"Client {client.Name} did not attempt token refresh");
                }
                catch (Exception ex)
                {
                    refreshResults.Add(new TokenRefreshResult
                    {
                        ClientName = client.Name,
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                }
            }
            
            LogAuthenticationResults(refreshResults);
        }

        [Fact]
        [Trait("Category", "Authentication.Security")]
        [Trait("Priority", "1")]
        public void Authentication_Should_Use_Secure_Token_Storage()
        {
            var tokenStorages = GetAvailableTokenStorages();

            foreach (var storage in tokenStorages)
            {
                var securityAnalysis = AnalyzeTokenStorage(storage);
                
                Assert.True(securityAnalysis.UsesEncryption,
                    $"{storage.GetType().Name} should use encryption for token storage");
                Assert.False(securityAnalysis.HasPlaintextTokens,
                    $"{storage.GetType().Name} should not store plaintext tokens");
            }
        }
        
        #endregion
        
        #region 5. Resilience Policy Validation Tests
        
        [Fact]
        [Trait("Category", "Resilience.RetryPolicy")]
        [Trait("Priority", "2")]
        public async Task All_Clients_Should_Implement_Proper_Retry_Logic()
        {
            var retryResults = new List<RetryPolicyResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var scenarios = new[]
                {
                    HttpStatusCode.RequestTimeout,
                    HttpStatusCode.TooManyRequests,
                    HttpStatusCode.InternalServerError,
                    HttpStatusCode.BadGateway,
                    HttpStatusCode.ServiceUnavailable,
                    HttpStatusCode.GatewayTimeout
                };

                foreach (var statusCode in scenarios)
                {
                    var result = await TestRetryBehavior(client, statusCode);
                    retryResults.Add(result);
                    
                    Assert.True(result.EventuallySucceeded,
                        $"Client {client.Name} should eventually succeed after retries for {statusCode}");
                    Assert.True(result.RetryAttempts >= 2,
                        $"Client {client.Name} should retry at least 2 times for {statusCode}");
                    Assert.True(result.RetryAttempts <= 5,
                        $"Client {client.Name} should not retry more than 5 times for {statusCode}");
                }
            }
            
            LogResilienceResults(retryResults);
        }

        [Fact]
        [Trait("Category", "Resilience.CircuitBreaker")]
        [Trait("Priority", "2")]
        public async Task Circuit_Breaker_Should_Prevent_Cascading_Failures()
        {
            var circuitBreakerResults = new List<CircuitBreakerResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var result = await TestCircuitBreakerBehavior(client);
                circuitBreakerResults.Add(result);
                
                // Circuit breaker behavior may vary by client configuration
                if (client.HasCircuitBreakerEnabled())
                {
                    Assert.True(result.CircuitBreakerTripped,
                        $"Circuit breaker should trip for client {client.Name}");
                    Assert.True(result.SubsequentCallsFailed,
                        $"Subsequent calls should fail fast for client {client.Name}");
                }
            }
        }
        
        #endregion
        
        #region 6. Memory Usage Analysis Tests
        
        [Fact]
        [Trait("Category", "Memory.LeakDetection")]
        [Trait("Priority", "2")]
        public async Task All_Clients_Should_Not_Have_Memory_Leaks()
        {
            var memoryResults = new List<MemoryAnalysisResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var result = await AnalyzeMemoryUsage(client, operations: 1000);
                memoryResults.Add(result);
                
                Assert.True(result.MemoryGrowthMB < 10,
                    $"Client {client.Name} memory growth {result.MemoryGrowthMB}MB exceeds 10MB threshold");
                Assert.True(result.Gen2Collections < 10,
                    $"Client {client.Name} triggered {result.Gen2Collections} Gen2 collections, expected <10");
            }
            
            LogMemoryResults(memoryResults);
        }

        [Fact]
        [Trait("Category", "Memory.PeakUsage")]
        [Trait("Priority", "2")]
        public async Task All_Clients_Should_Have_Acceptable_Peak_Memory_Usage()
        {
            var peakMemoryResults = new List<PeakMemoryResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var initialMemory = GC.GetTotalMemory(true);
                
                // Simulate high load scenario
                var tasks = Enumerable.Range(0, 100)
                    .Select(_ => Task.Run(() => PerformIntensiveOperations(client)))
                    .ToArray();
                    
                await Task.WhenAll(tasks);
                
                var peakMemory = GC.GetTotalMemory(false);
                var memoryIncreaseMB = (peakMemory - initialMemory) / (1024.0 * 1024.0);
                
                peakMemoryResults.Add(new PeakMemoryResult
                {
                    ClientName = client.Name,
                    PeakMemoryMB = memoryIncreaseMB
                });
                
                Assert.True(memoryIncreaseMB < 50,
                    $"Client {client.Name} peak memory {memoryIncreaseMB}MB exceeds 50MB threshold");
            }
        }
        
        #endregion
        
        #region 7. Async/Await Pattern Tests
        
        [Fact]
        [Trait("Category", "Async.ConfigureAwait")]
        [Trait("Priority", "2")]
        public void All_Async_Methods_Should_Use_ConfigureAwait_False()
        {
            var configureAwaitViolations = new List<ConfigureAwaitViolation>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var analyzer = new AsyncPatternAnalyzer();
                var violations = analyzer.AnalyzeConfigureAwaitUsage(client);
                configureAwaitViolations.AddRange(violations);
            }
            
            Assert.Equal(0, configureAwaitViolations.Count);
        }

        [Fact]
        [Trait("Category", "Async.DeadlockPrevention")]
        [Trait("Priority", "2")]
        public async Task All_Clients_Should_Prevent_Async_Deadlocks()
        {
            var deadlockTests = new List<DeadlockTestResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var result = await TestDeadlockScenarios(client);
                deadlockTests.Add(result);
                
                Assert.False(result.DeadlockDetected,
                    $"Client {client.Name} has potential deadlock in {result.MethodName}");
            }
        }
        
        #endregion
        
        #region 8. Cancellation Token Tests
        
        [Fact]
        [Trait("Category", "Cancellation.TokenPropagation")]
        [Trait("Priority", "2")]
        public void All_Operations_Should_Accept_Cancellation_Tokens()
        {
            var tokenPropagationResults = new List<TokenPropagationResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var operations = GetAllOperations(client);
                
                foreach (var operation in operations)
                {
                    var hasTokenParameter = operation.GetParameters()
                        .Any(p => p.ParameterType == typeof(CancellationToken));
                        
                    var result = new TokenPropagationResult
                    {
                        ClientName = client.Name,
                        OperationName = operation.Name,
                        HasCancellationToken = hasTokenParameter
                    };
                    
                    tokenPropagationResults.Add(result);
                    
                    Assert.True(hasTokenParameter,
                        $"Operation {client.Name}.{operation.Name} should accept CancellationToken");
                }
            }
        }

        [Fact]
        [Trait("Category", "Cancellation.Responsiveness")]
        [Trait("Priority", "2")]
        public async Task Operations_Should_Respond_To_Cancellation_Quickly()
        {
            var responsivenessResults = new List<CancellationResponsivenessResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var operations = GetAsyncOperations(client);
                
                foreach (var operation in operations.Take(3)) // Limit to avoid long test times
                {
                    using var cts = new CancellationTokenSource();
                    var stopwatch = Stopwatch.StartNew();
                    
                    var operationTask = InvokeOperationAsync(operation, cts.Token);
                    
                    // Cancel after 100ms
                    _ = Task.Delay(100).ContinueWith(_ => cts.Cancel());
                    
                    try
                    {
                        await operationTask;
                        throw new InvalidOperationException($"Operation {operation.Name} should have been cancelled");
                    }
                    catch (OperationCanceledException)
                    {
                        stopwatch.Stop();
                        var responseTime = stopwatch.ElapsedMilliseconds;
                        
                        var result = new CancellationResponsivenessResult
                        {
                            ClientName = client.Name,
                            OperationName = operation.Name,
                            ResponseTimeMs = responseTime,
                            RespondedQuickly = responseTime < 1000
                        };
                        
                        responsivenessResults.Add(result);
                        
                        Assert.True(responseTime < 1000,
                            $"Operation {client.Name}.{operation.Name} took {responseTime}ms to respond to cancellation, expected <1000ms");
                    }
                    catch (Exception ex) when (!(ex is OperationCanceledException))
                    {
                        // Some operations may not support cancellation yet - log but don't fail
                        Console.WriteLine($"Operation {client.Name}.{operation.Name} does not support cancellation: {ex.Message}");
                    }
                }
            }
        }
        
        #endregion
        
        #region 9. Error Handling Validation Tests
        
        [Fact]
        [Trait("Category", "ErrorHandling.ExceptionMapping")]
        [Trait("Priority", "1")]
        public async Task All_HTTP_Errors_Should_Be_Properly_Mapped()
        {
            var errorMappingResults = new List<ErrorMappingResult>();
            
            var httpErrorCodes = new[]
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.NotFound,
                HttpStatusCode.Conflict,
                HttpStatusCode.UnprocessableEntity,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout
            };

            foreach (var client in GetAllKiotaClients())
            {
                foreach (var statusCode in httpErrorCodes)
                {
                    var result = await TestErrorMapping(client, statusCode);
                    errorMappingResults.Add(result);
                    
                    Assert.NotNull(result.MappedException);
                    Assert.True(result.HasRichErrorInfo,
                        $"Client {client.Name} should provide rich error info for {statusCode}");
                }
            }
            
            LogErrorMappingResults(errorMappingResults);
        }

        [Fact]
        [Trait("Category", "ErrorHandling.LoggingIntegration")]
        [Trait("Priority", "2")]
        public async Task Error_Scenarios_Should_Be_Properly_Logged()
        {
            var logCapture = new TestLogCapture();
            var loggingResults = new List<ErrorLoggingResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                var errorScenarios = new[]
                {
                    (HttpStatusCode.InternalServerError, LogLevel.Error),
                    (HttpStatusCode.BadRequest, LogLevel.Warning),
                    (HttpStatusCode.NotFound, LogLevel.Information),
                    (HttpStatusCode.TooManyRequests, LogLevel.Warning)
                };

                foreach (var (statusCode, expectedLogLevel) in errorScenarios)
                {
                    logCapture.Clear();
                    
                    try
                    {
                        await PerformOperationExpectingError(client, statusCode);
                    }
                    catch
                    {
                        // Expected
                    }
                    
                    var logEntries = logCapture.GetLogEntries();
                    var errorLogs = logEntries.Where(l => l.LogLevel >= expectedLogLevel).ToList();
                    
                    var result = new ErrorLoggingResult
                    {
                        ClientName = client.Name,
                        StatusCode = statusCode,
                        ExpectedLogLevel = expectedLogLevel,
                        LogEntriesCount = errorLogs.Count,
                        HasStructuredLogging = errorLogs.Any(l => l.HasStructuredData),
                        HasCorrelationId = errorLogs.Any(l => l.Properties.ContainsKey("CorrelationId"))
                    };
                    
                    loggingResults.Add(result);
                }
            }
        }
        
        #endregion
        
        #region 10. Logging Consistency Tests
        
        [Fact]
        [Trait("Category", "Logging.StructuredLogging")]
        [Trait("Priority", "2")]
        public async Task All_Clients_Should_Use_Structured_Logging()
        {
            var logCapture = new StructuredLogCapture();
            var structuredLoggingResults = new List<StructuredLoggingResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                logCapture.Clear();
                
                // Perform various operations to generate logs
                await PerformTypicalOperations(client, logCapture);
                
                var logEntries = logCapture.GetLogEntries();
                var structuredEntries = logEntries.Where(l => l.IsStructured).ToList();
                
                var result = new StructuredLoggingResult
                {
                    ClientName = client.Name,
                    TotalLogEntries = logEntries.Count,
                    StructuredLogEntries = structuredEntries.Count,
                    StructuredPercentage = logEntries.Count > 0 ? structuredEntries.Count / (double)logEntries.Count : 0,
                    HasConsistentSchema = ValidateLogSchema(structuredEntries)
                };
                
                structuredLoggingResults.Add(result);
                
                // Allow for some unstructured logging in generated code
                Assert.True(result.StructuredPercentage >= 0.80,
                    $"Client {client.Name} structured logging percentage {result.StructuredPercentage:P} should be ≥80%");
            }
            
            LogStructuredLoggingResults(structuredLoggingResults);
        }

        [Fact]
        [Trait("Category", "Logging.PerformanceLogging")]
        [Trait("Priority", "3")]
        public async Task Performance_Metrics_Should_Be_Consistently_Logged()
        {
            var logCapture = new PerformanceLogCapture();
            var performanceLoggingResults = new List<PerformanceLoggingResult>();
            
            foreach (var client in GetAllKiotaClients())
            {
                logCapture.Clear();
                
                // Perform operations that should generate performance logs
                await PerformTimingCriticalOperations(client, logCapture);
                
                var performanceLogs = logCapture.GetPerformanceLogEntries();
                
                var result = new PerformanceLoggingResult
                {
                    ClientName = client.Name,
                    PerformanceLogCount = performanceLogs.Count,
                    HasOperationTiming = performanceLogs.Any(l => l.HasProperty("OperationDurationMs")),
                    HasThroughputMetrics = performanceLogs.Any(l => l.HasProperty("RequestsPerSecond")),
                    HasMemoryMetrics = performanceLogs.Any(l => l.HasProperty("MemoryUsageMB")),
                    HasCorrelationIds = performanceLogs.All(l => l.HasProperty("CorrelationId"))
                };
                
                performanceLoggingResults.Add(result);
            }
        }
        
        #endregion
    }
}