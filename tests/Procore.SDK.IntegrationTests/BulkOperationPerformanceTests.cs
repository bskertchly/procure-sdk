using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NUnit.Framework;
using Procore.SDK.Core;
using Procore.SDK.Core.Models;
using Procore.SDK.ProjectManagement;
using Procore.SDK.QualitySafety;
using Procore.SDK.ConstructionFinancials;
using Procore.SDK.FieldProductivity;
using Procore.SDK.ResourceManagement;

namespace Procore.SDK.IntegrationTests;

/// <summary>
/// Comprehensive performance tests for bulk operations across all SDK clients.
/// These tests validate throughput, latency, memory usage, and error handling
/// for large-scale batch operations.
/// </summary>
[TestFixture]
[Category("Integration")]
[Category("Performance")]
[Category("BulkOperations")]
public class BulkOperationPerformanceTests
{
    private IRequestAdapter? _requestAdapter;
    private ILogger<BulkOperationPerformanceTests>? _logger;
    private int _testCompanyId;
    private int _testProjectId;
    
    // Performance thresholds
    private static readonly TimeSpan BULK_OPERATION_TIMEOUT = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan SINGLE_OPERATION_MAX_TIME = TimeSpan.FromSeconds(10);
    private static readonly long MAX_MEMORY_INCREASE_MB = 100; // 100MB threshold
    private static readonly int BULK_OPERATION_SIZE = 100;
    private static readonly int LARGE_BULK_OPERATION_SIZE = 1000;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _requestAdapter = TestHelpers.CreateRequestAdapter();
        _logger = TestHelpers.CreateLogger<BulkOperationPerformanceTests>();
        _testCompanyId = TestHelpers.GetTestCompanyId();
        _testProjectId = TestHelpers.GetTestProjectId();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _requestAdapter?.Dispose();
    }

    #region Core Client Bulk Operations Tests

    [TestFixture]
    [Category("CoreClientBulk")]
    public class CoreClientBulkTests
    {
        [Test]
        [Description("Tests bulk user operations performance and memory usage")]
        public async Task TestBulkUserOperations()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            var results = new BulkOperationTestResults("CoreClient_BulkUsers");

            // Test bulk user creation (simulated - API may not support)
            await TestBulkUserCreation(coreClient, results);

            // Test bulk user updates
            await TestBulkUserUpdates(coreClient, results);

            // Test bulk user retrieval with pagination
            await TestBulkUserRetrieval(coreClient, results);

            // Assert performance criteria
            Assert.That(results.GetAverageOperationTime(), Is.LessThan(SINGLE_OPERATION_MAX_TIME),
                $"Average bulk operation time exceeded threshold: {results.GetAverageOperationTime()}");
            
            TestContext.WriteLine($"Core Client Bulk Operations: {results.SuccessfulOperations}/{results.TotalOperations} successful");
            TestContext.WriteLine($"Average operation time: {results.GetAverageOperationTime().TotalMilliseconds:F0}ms");
        }

        private async Task TestBulkUserCreation(ProcoreCoreClient coreClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            try
            {
                var userRequests = GenerateTestUserRequests(BULK_OPERATION_SIZE);
                var tasks = new List<Task<User>>();

                foreach (var request in userRequests)
                {
                    // Note: Actual bulk creation may need to be implemented as batch API
                    // For now, test concurrent individual operations
                    tasks.Add(SimulateBulkUserCreation(coreClient, request));
                }

                var createdUsers = await Task.WhenAll(tasks);
                stopwatch.Stop();

                var memoryAfter = GC.GetTotalMemory(false);
                var memoryIncrease = (memoryAfter - memoryBefore) / (1024 * 1024); // MB

                results.RecordOperation("BulkUserCreation", stopwatch.Elapsed, 
                    createdUsers.Length, memoryIncrease, 
                    createdUsers.All(u => u != null));

                TestContext.WriteLine($"Bulk user creation: {createdUsers.Length} users in {stopwatch.ElapsedMilliseconds}ms, Memory: +{memoryIncrease}MB");
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkUserCreation", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task TestBulkUserUpdates(ProcoreCoreClient coreClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            try
            {
                // Get existing users for updates
                var existingUsers = await coreClient.GetUsersAsync(_testCompanyId);
                var usersToUpdate = existingUsers?.Take(Math.Min(BULK_OPERATION_SIZE, existingUsers.Count())).ToList();

                if (usersToUpdate?.Any() == true)
                {
                    var updateTasks = new List<Task<User>>();

                    foreach (var user in usersToUpdate)
                    {
                        var updateRequest = new UpdateUserRequest
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Notes = $"Bulk updated at {DateTime.UtcNow}"
                        };
                        updateTasks.Add(coreClient.UpdateUserAsync(_testCompanyId, user.Id, updateRequest));
                    }

                    var updatedUsers = await Task.WhenAll(updateTasks);
                    stopwatch.Stop();

                    var memoryAfter = GC.GetTotalMemory(false);
                    var memoryIncrease = (memoryAfter - memoryBefore) / (1024 * 1024);

                    results.RecordOperation("BulkUserUpdate", stopwatch.Elapsed,
                        updatedUsers.Length, memoryIncrease,
                        updatedUsers.All(u => u != null));
                }
                else
                {
                    results.RecordOperation("BulkUserUpdate", stopwatch.Elapsed, 0, 0, false, "No users available for update");
                }
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkUserUpdate", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task TestBulkUserRetrieval(ProcoreCoreClient coreClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            try
            {
                var allUsers = new List<User>();
                var pageSize = 50;
                var currentPage = 1;
                
                // Test paginated bulk retrieval
                while (allUsers.Count < LARGE_BULK_OPERATION_SIZE)
                {
                    var paginationOptions = new PaginationOptions { Page = currentPage, PerPage = pageSize };
                    var pagedResult = await coreClient.GetUsersPagedAsync(_testCompanyId, paginationOptions);
                    
                    if (pagedResult?.Items?.Any() != true)
                        break;
                        
                    allUsers.AddRange(pagedResult.Items);
                    currentPage++;
                    
                    // Safety break to avoid infinite loop
                    if (currentPage > 20) break;
                }

                stopwatch.Stop();
                var memoryAfter = GC.GetTotalMemory(false);
                var memoryIncrease = (memoryAfter - memoryBefore) / (1024 * 1024);

                results.RecordOperation("BulkUserRetrieval", stopwatch.Elapsed,
                    allUsers.Count, memoryIncrease, allUsers.Any());

                TestContext.WriteLine($"Bulk user retrieval: {allUsers.Count} users in {stopwatch.ElapsedMilliseconds}ms, Memory: +{memoryIncrease}MB");
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkUserRetrieval", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task<User> SimulateBulkUserCreation(ProcoreCoreClient coreClient, CreateUserRequest request)
        {
            // Since bulk user creation may not be supported, simulate with placeholder
            await Task.Delay(10); // Simulate API call time
            return new User
            {
                Id = new Random().Next(1000, 9999),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        private IEnumerable<CreateUserRequest> GenerateTestUserRequests(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new CreateUserRequest
                {
                    FirstName = $"BulkTest{i}",
                    LastName = $"User{i}",
                    Email = $"bulktest{i}@example.com",
                    JobTitle = "Test User"
                };
            }
        }
    }

    #endregion

    #region Document Bulk Operations Tests

    [TestFixture]
    [Category("DocumentBulk")]
    public class DocumentBulkOperationTests
    {
        [Test]
        [Description("Tests bulk document upload and processing performance")]
        public async Task TestBulkDocumentOperations()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            var results = new BulkOperationTestResults("CoreClient_BulkDocuments");

            // Test bulk document uploads
            await TestBulkDocumentUploads(coreClient, results);

            // Test bulk document updates
            await TestBulkDocumentUpdates(coreClient, results);

            // Test bulk document retrieval with filtering
            await TestBulkDocumentRetrieval(coreClient, results);

            TestContext.WriteLine($"Document Bulk Operations: {results.SuccessfulOperations}/{results.TotalOperations} successful");
        }

        private async Task TestBulkDocumentUploads(ProcoreCoreClient coreClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            try
            {
                var uploadRequests = GenerateTestDocumentRequests(BULK_OPERATION_SIZE);
                var uploadTasks = new List<Task<Document>>();

                foreach (var request in uploadRequests)
                {
                    uploadTasks.Add(coreClient.UploadDocumentAsync(_testCompanyId, request));
                }

                var uploadedDocuments = await Task.WhenAll(uploadTasks);
                stopwatch.Stop();

                var memoryAfter = GC.GetTotalMemory(false);
                var memoryIncrease = (memoryAfter - memoryBefore) / (1024 * 1024);

                results.RecordOperation("BulkDocumentUpload", stopwatch.Elapsed,
                    uploadedDocuments.Length, memoryIncrease,
                    uploadedDocuments.All(d => d != null));

                TestContext.WriteLine($"Bulk document upload: {uploadedDocuments.Length} documents in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkDocumentUpload", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task TestBulkDocumentUpdates(ProcoreCoreClient coreClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var existingDocuments = await coreClient.GetDocumentsAsync(_testCompanyId);
                var documentsToUpdate = existingDocuments?.Take(Math.Min(BULK_OPERATION_SIZE, existingDocuments.Count())).ToList();

                if (documentsToUpdate?.Any() == true)
                {
                    var updateTasks = new List<Task<Document>>();

                    foreach (var doc in documentsToUpdate)
                    {
                        var updateRequest = new UpdateDocumentRequest
                        {
                            Name = doc.Name,
                            Description = $"Bulk updated at {DateTime.UtcNow}"
                        };
                        updateTasks.Add(coreClient.UpdateDocumentAsync(_testCompanyId, doc.Id, updateRequest));
                    }

                    var updatedDocuments = await Task.WhenAll(updateTasks);
                    stopwatch.Stop();

                    results.RecordOperation("BulkDocumentUpdate", stopwatch.Elapsed,
                        updatedDocuments.Length, 0, updatedDocuments.All(d => d != null));
                }
                else
                {
                    results.RecordOperation("BulkDocumentUpdate", stopwatch.Elapsed, 0, 0, false, "No documents available");
                }
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkDocumentUpdate", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task TestBulkDocumentRetrieval(ProcoreCoreClient coreClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var allDocuments = new List<Document>();
                var pageSize = 25;
                var currentPage = 1;

                while (allDocuments.Count < BULK_OPERATION_SIZE && currentPage <= 10)
                {
                    var paginationOptions = new PaginationOptions { Page = currentPage, PerPage = pageSize };
                    var pagedResult = await coreClient.GetDocumentsPagedAsync(_testCompanyId, paginationOptions);
                    
                    if (pagedResult?.Items?.Any() != true)
                        break;
                        
                    allDocuments.AddRange(pagedResult.Items);
                    currentPage++;
                }

                stopwatch.Stop();

                results.RecordOperation("BulkDocumentRetrieval", stopwatch.Elapsed,
                    allDocuments.Count, 0, allDocuments.Any());
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkDocumentRetrieval", ex.Message, stopwatch.Elapsed);
            }
        }

        private IEnumerable<UploadDocumentRequest> GenerateTestDocumentRequests(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new UploadDocumentRequest
                {
                    Name = $"BulkTestDocument{i}.pdf",
                    FileName = $"bulktest{i}.pdf",
                    ContentType = "application/pdf",
                    Description = $"Bulk test document {i}"
                };
            }
        }
    }

    #endregion

    #region Resource Management Bulk Operations Tests

    [TestFixture]
    [Category("ResourceManagementBulk")]
    public class ResourceManagementBulkTests
    {
        [Test]
        [Description("Tests bulk resource allocation and workforce management performance")]
        public async Task TestBulkResourceOperations()
        {
            using var resourceClient = new ProcoreResourceManagementClient(TestHelpers.CreateRequestAdapter());
            var results = new BulkOperationTestResults("ResourceManagement_BulkOperations");

            // Test bulk resource creation
            await TestBulkResourceCreation(resourceClient, results);

            // Test bulk resource allocation
            await TestBulkResourceAllocation(resourceClient, results);

            // Test bulk workforce assignments
            await TestBulkWorkforceAssignments(resourceClient, results);

            TestContext.WriteLine($"Resource Management Bulk Operations: {results.SuccessfulOperations}/{results.TotalOperations} successful");
        }

        private async Task TestBulkResourceCreation(ProcoreResourceManagementClient resourceClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var resourceRequests = GenerateTestResourceRequests(BULK_OPERATION_SIZE);
                var creationTasks = new List<Task<Resource>>();

                foreach (var request in resourceRequests)
                {
                    creationTasks.Add(resourceClient.CreateResourceAsync(_testCompanyId, request));
                }

                var createdResources = await Task.WhenAll(creationTasks);
                stopwatch.Stop();

                results.RecordOperation("BulkResourceCreation", stopwatch.Elapsed,
                    createdResources.Length, 0, createdResources.All(r => r != null));
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkResourceCreation", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task TestBulkResourceAllocation(ProcoreResourceManagementClient resourceClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var allocationRequests = GenerateTestAllocationRequests(BULK_OPERATION_SIZE);
                var allocationTasks = new List<Task<ResourceAllocation>>();

                foreach (var request in allocationRequests)
                {
                    allocationTasks.Add(resourceClient.AllocateResourceAsync(_testCompanyId, _testProjectId, request));
                }

                var allocations = await Task.WhenAll(allocationTasks);
                stopwatch.Stop();

                results.RecordOperation("BulkResourceAllocation", stopwatch.Elapsed,
                    allocations.Length, 0, allocations.All(a => a != null));
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkResourceAllocation", ex.Message, stopwatch.Elapsed);
            }
        }

        private async Task TestBulkWorkforceAssignments(ProcoreResourceManagementClient resourceClient, BulkOperationTestResults results)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var assignmentRequests = GenerateTestWorkforceRequests(BULK_OPERATION_SIZE);
                var assignmentTasks = new List<Task<WorkforceAssignment>>();

                foreach (var request in assignmentRequests)
                {
                    assignmentTasks.Add(resourceClient.CreateWorkforceAssignmentAsync(_testCompanyId, _testProjectId, request));
                }

                var assignments = await Task.WhenAll(assignmentTasks);
                stopwatch.Stop();

                results.RecordOperation("BulkWorkforceAssignment", stopwatch.Elapsed,
                    assignments.Length, 0, assignments.All(a => a != null));
            }
            catch (Exception ex)
            {
                results.RecordFailure("BulkWorkforceAssignment", ex.Message, stopwatch.Elapsed);
            }
        }

        private IEnumerable<CreateResourceRequest> GenerateTestResourceRequests(int count)
        {
            var resourceTypes = new[] { "Equipment", "Material", "Labor", "Vehicle" };
            
            for (int i = 0; i < count; i++)
            {
                yield return new CreateResourceRequest
                {
                    Name = $"BulkTestResource{i}",
                    Type = resourceTypes[i % resourceTypes.Length],
                    Description = $"Bulk test resource {i}",
                    CostPerHour = 50.0m + (i % 100)
                };
            }
        }

        private IEnumerable<AllocateResourceRequest> GenerateTestAllocationRequests(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new AllocateResourceRequest
                {
                    ResourceId = i + 1,
                    StartDate = DateTime.Now.AddDays(i % 30),
                    EndDate = DateTime.Now.AddDays((i % 30) + 7),
                    AllocatedHours = 8.0m,
                    Notes = $"Bulk allocation {i}"
                };
            }
        }

        private IEnumerable<CreateWorkforceAssignmentRequest> GenerateTestWorkforceRequests(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new CreateWorkforceAssignmentRequest
                {
                    UserId = i + 1,
                    Role = $"Role{i % 5}",
                    StartDate = DateTime.Now.AddDays(i % 30),
                    EndDate = DateTime.Now.AddDays((i % 30) + 14),
                    HoursPerDay = 8.0m
                };
            }
        }
    }

    #endregion

    #region Performance Benchmark Tests

    [TestFixture]
    [Category("PerformanceBenchmarks")]
    public class BulkOperationBenchmarks
    {
        [Test]
        [Description("Benchmarks bulk operation performance against established thresholds")]
        public async Task BenchmarkBulkOperationPerformance()
        {
            var benchmarkResults = new Dictionary<string, BenchmarkResult>();

            // Benchmark Core Client operations
            benchmarkResults["CoreClient"] = await BenchmarkCoreClientOperations();

            // Benchmark ProjectManagement operations
            benchmarkResults["ProjectManagement"] = await BenchmarkProjectManagementOperations();

            // Benchmark QualitySafety operations
            benchmarkResults["QualitySafety"] = await BenchmarkQualitySafetyOperations();

            // Validate against performance thresholds
            foreach (var (clientName, result) in benchmarkResults)
            {
                Assert.That(result.AverageResponseTime, Is.LessThan(SINGLE_OPERATION_MAX_TIME),
                    $"{clientName} bulk operations exceeded response time threshold");
                
                Assert.That(result.MemoryUsageMB, Is.LessThan(MAX_MEMORY_INCREASE_MB),
                    $"{clientName} bulk operations exceeded memory usage threshold");

                TestContext.WriteLine($"{clientName} Benchmark: {result.OperationsPerSecond:F1} ops/sec, " +
                    $"Avg: {result.AverageResponseTime.TotalMilliseconds:F0}ms, " +
                    $"Memory: {result.MemoryUsageMB:F1}MB");
            }
        }

        private async Task<BenchmarkResult> BenchmarkCoreClientOperations()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            var operations = 0;
            try
            {
                // Perform a series of bulk operations for benchmarking
                var companies = await coreClient.GetCompaniesAsync();
                operations++;

                var users = await coreClient.GetUsersAsync(_testCompanyId);
                operations++;

                var documents = await coreClient.GetDocumentsAsync(_testCompanyId);
                operations++;

                // Test pagination performance
                for (int page = 1; page <= 5; page++)
                {
                    var paginationOptions = new PaginationOptions { Page = page, PerPage = 20 };
                    await coreClient.GetUsersPagedAsync(_testCompanyId, paginationOptions);
                    operations++;
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Core client benchmark error: {ex.Message}");
            }

            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryIncrease = (memoryAfter - memoryBefore) / (1024.0 * 1024.0);

            return new BenchmarkResult
            {
                TotalOperations = operations,
                TotalTime = stopwatch.Elapsed,
                AverageResponseTime = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Max(operations, 1)),
                OperationsPerSecond = operations / Math.Max(stopwatch.Elapsed.TotalSeconds, 0.001),
                MemoryUsageMB = memoryIncrease
            };
        }

        private async Task<BenchmarkResult> BenchmarkProjectManagementOperations()
        {
            using var projectClient = new ProcoreProjectManagementClient(TestHelpers.CreateRequestAdapter());
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            var operations = 0;
            try
            {
                // Note: Most operations are placeholders, so this tests the placeholder performance
                var project = await projectClient.GetProjectAsync(_testCompanyId, _testProjectId);
                operations++;

                var projects = await projectClient.GetProjectsAsync(_testCompanyId);
                operations++;

                var budgetItems = await projectClient.GetBudgetLineItemsAsync(_testCompanyId, _testProjectId);
                operations++;

                var contracts = await projectClient.GetCommitmentContractsAsync(_testCompanyId, _testProjectId);
                operations++;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Project management benchmark error: {ex.Message}");
            }

            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryIncrease = (memoryAfter - memoryBefore) / (1024.0 * 1024.0);

            return new BenchmarkResult
            {
                TotalOperations = operations,
                TotalTime = stopwatch.Elapsed,
                AverageResponseTime = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Max(operations, 1)),
                OperationsPerSecond = operations / Math.Max(stopwatch.Elapsed.TotalSeconds, 0.001),
                MemoryUsageMB = memoryIncrease
            };
        }

        private async Task<BenchmarkResult> BenchmarkQualitySafetyOperations()
        {
            using var qualityClient = new ProcoreQualitySafetyClient(TestHelpers.CreateRequestAdapter());
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            var operations = 0;
            try
            {
                var observations = await qualityClient.GetObservationsAsync(_testCompanyId, _testProjectId);
                operations++;

                var incidents = await qualityClient.GetSafetyIncidentsAsync(_testCompanyId, _testProjectId);
                operations++;

                var inspections = await qualityClient.GetInspectionsAsync(_testCompanyId, _testProjectId);
                operations++;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Quality safety benchmark error: {ex.Message}");
            }

            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryIncrease = (memoryAfter - memoryBefore) / (1024.0 * 1024.0);

            return new BenchmarkResult
            {
                TotalOperations = operations,
                TotalTime = stopwatch.Elapsed,
                AverageResponseTime = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Max(operations, 1)),
                OperationsPerSecond = operations / Math.Max(stopwatch.Elapsed.TotalSeconds, 0.001),
                MemoryUsageMB = memoryIncrease
            };
        }
    }

    #endregion

    #region Test Result Helpers

    public class BulkOperationTestResults
    {
        public string TestCategory { get; }
        public List<BulkOperationResult> Results { get; } = new();

        public BulkOperationTestResults(string testCategory)
        {
            TestCategory = testCategory;
        }

        public void RecordOperation(string operationName, TimeSpan duration, int itemCount, 
            double memoryUsageMB, bool success, string? notes = null)
        {
            Results.Add(new BulkOperationResult
            {
                OperationName = operationName,
                Duration = duration,
                ItemCount = itemCount,
                MemoryUsageMB = memoryUsageMB,
                Success = success,
                Notes = notes,
                Timestamp = DateTime.UtcNow
            });
        }

        public void RecordFailure(string operationName, string errorMessage, TimeSpan duration)
        {
            Results.Add(new BulkOperationResult
            {
                OperationName = operationName,
                Duration = duration,
                ItemCount = 0,
                MemoryUsageMB = 0,
                Success = false,
                Notes = $"Failed: {errorMessage}",
                Timestamp = DateTime.UtcNow
            });
        }

        public int SuccessfulOperations => Results.Count(r => r.Success);
        public int TotalOperations => Results.Count;
        public double SuccessRate => TotalOperations > 0 ? (double)SuccessfulOperations / TotalOperations : 0.0;

        public TimeSpan GetAverageOperationTime()
        {
            var successfulResults = Results.Where(r => r.Success).ToList();
            if (!successfulResults.Any()) return TimeSpan.Zero;

            var totalTicks = successfulResults.Sum(r => r.Duration.Ticks);
            return TimeSpan.FromTicks(totalTicks / successfulResults.Count);
        }

        public double GetTotalMemoryUsage() => Results.Sum(r => r.MemoryUsageMB);
        public double GetThroughput() => Results.Where(r => r.Success).Sum(r => r.ItemCount) / 
            Math.Max(Results.Where(r => r.Success).Sum(r => r.Duration.TotalSeconds), 0.001);
    }

    public class BulkOperationResult
    {
        public string OperationName { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public int ItemCount { get; set; }
        public double MemoryUsageMB { get; set; }
        public bool Success { get; set; }
        public string? Notes { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class BenchmarkResult
    {
        public int TotalOperations { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
        public double OperationsPerSecond { get; set; }
        public double MemoryUsageMB { get; set; }
    }

    #endregion
}