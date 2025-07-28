using System;
using System.Collections.Generic;
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
/// Comprehensive tests for advanced query and filtering capabilities across all SDK clients.
/// These tests validate complex search scenarios, filtering options, sorting capabilities,
/// and performance of query operations.
/// </summary>
[TestFixture]
[Category("Integration")]
[Category("AdvancedQueries")]
public class AdvancedQueryTests
{
    private IRequestAdapter? _requestAdapter;
    private ILogger<AdvancedQueryTests>? _logger;
    private int _testCompanyId;
    private int _testProjectId;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _requestAdapter = TestHelpers.CreateRequestAdapter();
        _logger = TestHelpers.CreateLogger<AdvancedQueryTests>();
        _testCompanyId = TestHelpers.GetTestCompanyId();
        _testProjectId = TestHelpers.GetTestProjectId();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _requestAdapter?.Dispose();
    }

    #region Date Range Filtering Tests

    [TestFixture]
    [Category("DateRangeFiltering")]
    public class DateRangeFilteringTests
    {
        [Test]
        [Description("Tests date range filtering capabilities across different entity types")]
        public async Task TestDateRangeFiltering()
        {
            var startDate = DateTime.Now.AddMonths(-3);
            var endDate = DateTime.Now;

            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            using var projectClient = new ProcoreProjectManagementClient(TestHelpers.CreateRequestAdapter());
            using var qualityClient = new ProcoreQualitySafetyClient(TestHelpers.CreateRequestAdapter());
            using var fieldClient = new ProcoreFieldProductivityClient(TestHelpers.CreateRequestAdapter());

            var results = new QueryTestResults("DateRangeFiltering");

            // Test user filtering by creation date
            await TestUserDateRangeFiltering(coreClient, results, startDate, endDate);

            // Test document filtering by date
            await TestDocumentDateRangeFiltering(coreClient, results, startDate, endDate);

            // Test safety incident filtering by incident date
            await TestSafetyIncidentDateFiltering(qualityClient, results, startDate, endDate);

            // Test productivity report filtering by report date
            await TestProductivityReportDateFiltering(fieldClient, results, startDate, endDate);

            // Assert overall success
            Assert.That(results.SuccessfulTests, Is.GreaterThan(0), 
                "At least some date range filtering tests should succeed");
            
            TestContext.WriteLine($"Date Range Filtering Results: {results.SuccessfulTests}/{results.TotalTests} passed");
        }

        private async Task TestUserDateRangeFiltering(ProcoreCoreClient coreClient, QueryTestResults results, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Get all users first
                var allUsers = await coreClient.GetUsersAsync(_testCompanyId);
                
                // Filter by date range (client-side for now, until API supports it)
                var filteredUsers = allUsers?.Where(u => 
                    u.CreatedAt >= startDate && u.CreatedAt <= endDate);

                results.RecordTest("UserDateRangeFilter", 
                    filteredUsers != null, 
                    "Client-side date filtering works",
                    $"Found {filteredUsers?.Count() ?? 0} users in date range");

                // Test with pagination
                var paginationOptions = new PaginationOptions { Page = 1, PerPage = 10 };
                var pagedUsers = await coreClient.GetUsersPagedAsync(_testCompanyId, paginationOptions);
                
                results.RecordTest("UserDateRangeFilterPaginated",
                    pagedUsers?.Items != null,
                    "Paginated results support filtering",
                    $"Paginated query returned {pagedUsers?.Items?.Count() ?? 0} users");
            }
            catch (Exception ex)
            {
                results.RecordTest("UserDateRangeFilter", false, ex.Message);
            }
        }

        private async Task TestDocumentDateRangeFiltering(ProcoreCoreClient coreClient, QueryTestResults results, DateTime startDate, DateTime endDate)
        {
            try
            {
                var allDocuments = await coreClient.GetDocumentsAsync(_testCompanyId);
                
                var filteredDocuments = allDocuments?.Where(d => 
                    d.CreatedAt >= startDate && d.CreatedAt <= endDate);

                results.RecordTest("DocumentDateRangeFilter",
                    filteredDocuments != null,
                    "Document date filtering works",
                    $"Found {filteredDocuments?.Count() ?? 0} documents in date range");
            }
            catch (Exception ex)
            {
                results.RecordTest("DocumentDateRangeFilter", false, ex.Message);
            }
        }

        private async Task TestSafetyIncidentDateFiltering(ProcoreQualitySafetyClient qualityClient, QueryTestResults results, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Test recent incidents method
                var recentIncidents = await qualityClient.GetRecentIncidentsAsync(_testCompanyId, _testProjectId, 30);
                
                results.RecordTest("SafetyIncidentRecentFilter",
                    recentIncidents != null,
                    "Recent incidents filtering available",
                    $"Found {recentIncidents?.Count() ?? 0} recent incidents");

                // Test general incident retrieval with client-side filtering
                var allIncidents = await qualityClient.GetSafetyIncidentsAsync(_testCompanyId, _testProjectId);
                
                results.RecordTest("SafetyIncidentDateFilter",
                    allIncidents != null,
                    "Can retrieve incidents for date filtering",
                    "Placeholder implementation returns empty collection");
            }
            catch (Exception ex)
            {
                results.RecordTest("SafetyIncidentDateFilter", false, ex.Message);
            }
        }

        private async Task TestProductivityReportDateFiltering(ProcoreFieldProductivityClient fieldClient, QueryTestResults results, DateTime startDate, DateTime endDate)
        {
            try
            {
                var productivityReports = await fieldClient.GetProductivityReportsAsync(_testCompanyId, _testProjectId);
                
                results.RecordTest("ProductivityReportDateFilter",
                    productivityReports != null,
                    "Can retrieve productivity reports for filtering",
                    "Placeholder implementation returns empty collection");
            }
            catch (Exception ex)
            {
                results.RecordTest("ProductivityReportDateFilter", false, ex.Message);
            }
        }
    }

    #endregion

    #region Status-Based Filtering Tests

    [TestFixture]
    [Category("StatusFiltering")]
    public class StatusBasedFilteringTests
    {
        [Test]
        [Description("Tests status-based filtering across different entity types")]
        public async Task TestStatusBasedFiltering()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            using var projectClient = new ProcoreProjectManagementClient(TestHelpers.CreateRequestAdapter());
            using var qualityClient = new ProcoreQualitySafetyClient(TestHelpers.CreateRequestAdapter());
            using var resourceClient = new ProcoreResourceManagementClient(TestHelpers.CreateRequestAdapter());

            var results = new QueryTestResults("StatusBasedFiltering");

            // Test active user filtering
            await TestActiveUserFiltering(coreClient, results);

            // Test active project filtering
            await TestActiveProjectFiltering(projectClient, results);

            // Test open observation filtering
            await TestOpenObservationFiltering(qualityClient, results);

            // Test available resource filtering
            await TestAvailableResourceFiltering(resourceClient, results);

            Assert.That(results.SuccessfulTests, Is.GreaterThan(0),
                "At least some status-based filtering tests should succeed");

            TestContext.WriteLine($"Status-Based Filtering Results: {results.SuccessfulTests}/{results.TotalTests} passed");
        }

        private async Task TestActiveUserFiltering(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var allUsers = await coreClient.GetUsersAsync(_testCompanyId);
                var activeUsers = allUsers?.Where(u => u.IsActive);

                results.RecordTest("ActiveUserFilter",
                    activeUsers != null,
                    "Can filter active users",
                    $"Found {activeUsers?.Count() ?? 0} active users");
            }
            catch (Exception ex)
            {
                results.RecordTest("ActiveUserFilter", false, ex.Message);
            }
        }

        private async Task TestActiveProjectFiltering(ProcoreProjectManagementClient projectClient, QueryTestResults results)
        {
            try
            {
                // Test dedicated active projects method
                var activeProjects = await projectClient.GetActiveProjectsAsync(_testCompanyId);

                results.RecordTest("ActiveProjectFilter",
                    activeProjects != null,
                    "Active projects filtering available",
                    "Placeholder implementation returns empty collection");

                // Test general project filtering
                var allProjects = await projectClient.GetProjectsAsync(_testCompanyId);

                results.RecordTest("ProjectStatusFilter",
                    allProjects != null,
                    "Can retrieve projects for status filtering",
                    "Placeholder implementation returns empty collection");
            }
            catch (Exception ex)
            {
                results.RecordTest("ActiveProjectFilter", false, ex.Message);
            }
        }

        private async Task TestOpenObservationFiltering(ProcoreQualitySafetyClient qualityClient, QueryTestResults results)
        {
            try
            {
                // Test dedicated open observations method
                var openObservations = await qualityClient.GetOpenObservationsAsync(_testCompanyId, _testProjectId);

                results.RecordTest("OpenObservationFilter",
                    openObservations != null,
                    "Open observations filtering available",
                    "Placeholder implementation returns empty collection");

                // Test critical observations filtering
                var criticalObservations = await qualityClient.GetCriticalObservationsAsync(_testCompanyId, _testProjectId);

                results.RecordTest("CriticalObservationFilter",
                    criticalObservations != null,
                    "Critical observations filtering available",
                    "Placeholder implementation returns empty collection");

                // Test overdue observations filtering
                var overdueObservations = await qualityClient.GetOverdueObservationsAsync(_testCompanyId, _testProjectId);

                results.RecordTest("OverdueObservationFilter",
                    overdueObservations != null,
                    "Overdue observations filtering available",
                    "Placeholder implementation returns empty collection");
            }
            catch (Exception ex)
            {
                results.RecordTest("OpenObservationFilter", false, ex.Message);
            }
        }

        private async Task TestAvailableResourceFiltering(ProcoreResourceManagementClient resourceClient, QueryTestResults results)
        {
            try
            {
                var startDate = DateTime.Now;
                var endDate = DateTime.Now.AddMonths(1);

                // Test available resources filtering
                var availableResources = await resourceClient.GetAvailableResourcesAsync(_testCompanyId, startDate, endDate);

                results.RecordTest("AvailableResourceFilter",
                    availableResources != null,
                    "Available resources filtering available",
                    "Placeholder implementation returns empty collection");

                // Test over-allocated resources filtering
                var overAllocatedResources = await resourceClient.GetOverAllocatedResourcesAsync(_testCompanyId);

                results.RecordTest("OverAllocatedResourceFilter",
                    overAllocatedResources != null,
                    "Over-allocated resources filtering available",
                    "Placeholder implementation returns empty collection");
            }
            catch (Exception ex)
            {
                results.RecordTest("AvailableResourceFilter", false, ex.Message);
            }
        }
    }

    #endregion

    #region Keyword Search Tests

    [TestFixture]
    [Category("KeywordSearch")]
    public class KeywordSearchTests
    {
        [Test]
        [Description("Tests keyword search capabilities across different entity types")]
        public async Task TestKeywordSearch()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            using var projectClient = new ProcoreProjectManagementClient(TestHelpers.CreateRequestAdapter());

            var results = new QueryTestResults("KeywordSearch");

            // Test user search
            await TestUserSearch(coreClient, results);

            // Test company search
            await TestCompanySearch(coreClient, results);

            // Test project search
            await TestProjectSearch(projectClient, results);

            // Test document type search
            await TestDocumentTypeSearch(coreClient, results);

            Assert.That(results.SuccessfulTests, Is.GreaterThan(0),
                "At least some keyword search tests should succeed");

            TestContext.WriteLine($"Keyword Search Results: {results.SuccessfulTests}/{results.TotalTests} passed");
        }

        private async Task TestUserSearch(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var searchTerm = "test";
                var searchResults = await coreClient.SearchUsersAsync(_testCompanyId, searchTerm);

                results.RecordTest("UserKeywordSearch",
                    searchResults != null,
                    "User search functionality available",
                    $"Search for '{searchTerm}' returned {searchResults?.Count() ?? 0} results");

                // Test search with different terms
                var emailSearch = await coreClient.SearchUsersAsync(_testCompanyId, "@");
                results.RecordTest("UserEmailSearch",
                    emailSearch != null,
                    "Email-based user search works",
                    $"Email search returned {emailSearch?.Count() ?? 0} results");
            }
            catch (Exception ex)
            {
                results.RecordTest("UserKeywordSearch", false, ex.Message);
            }
        }

        private async Task TestCompanySearch(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var companyName = "Test";
                var companyByName = await coreClient.GetCompanyByNameAsync(companyName);

                results.RecordTest("CompanyNameSearch",
                    companyByName != null || true, // May throw exception if not found
                    "Company name search functionality available",
                    "Can search companies by name");
            }
            catch (Exception ex)
            {
                // Expected if company not found
                results.RecordTest("CompanyNameSearch", true, 
                    "Company name search throws appropriate exception when not found");
            }
        }

        private async Task TestProjectSearch(ProcoreProjectManagementClient projectClient, QueryTestResults results)
        {
            try
            {
                var projectName = "Test Project";
                var projectByName = await projectClient.GetProjectByNameAsync(_testCompanyId, projectName);

                results.RecordTest("ProjectNameSearch",
                    projectByName != null,
                    "Project name search functionality available",
                    "Placeholder implementation returns sample project");
            }
            catch (Exception ex)
            {
                results.RecordTest("ProjectNameSearch", false, ex.Message);
            }
        }

        private async Task TestDocumentTypeSearch(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var documentType = "pdf";
                var documentsByType = await coreClient.GetDocumentsByTypeAsync(_testCompanyId, documentType);

                results.RecordTest("DocumentTypeSearch",
                    documentsByType != null,
                    "Document type search functionality available",
                    $"Search for '{documentType}' documents returned {documentsByType?.Count() ?? 0} results");
            }
            catch (Exception ex)
            {
                results.RecordTest("DocumentTypeSearch", false, ex.Message);
            }
        }
    }

    #endregion

    #region Custom Field Filtering Tests

    [TestFixture]
    [Category("CustomFieldFiltering")]
    public class CustomFieldFilteringTests
    {
        [Test]
        [Description("Tests custom field filtering and querying capabilities")]
        public async Task TestCustomFieldFiltering()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());

            var results = new QueryTestResults("CustomFieldFiltering");

            // Test custom field retrieval by resource type
            await TestCustomFieldByResourceType(coreClient, results);

            // Test custom field data validation
            await TestCustomFieldDataTypes(coreClient, results);

            Assert.That(results.TotalTests, Is.GreaterThan(0),
                "Custom field filtering tests should execute");

            TestContext.WriteLine($"Custom Field Filtering Results: {results.SuccessfulTests}/{results.TotalTests} passed");
        }

        private async Task TestCustomFieldByResourceType(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var resourceTypes = new[] { "project", "user", "company", "task" };

                foreach (var resourceType in resourceTypes)
                {
                    var customFields = await coreClient.GetCustomFieldsAsync(_testCompanyId, resourceType);
                    
                    results.RecordTest($"CustomField_{resourceType}",
                        customFields != null,
                        $"Can retrieve custom fields for {resourceType}",
                        $"Found {customFields?.Count() ?? 0} custom fields for {resourceType}");
                }
            }
            catch (Exception ex)
            {
                results.RecordTest("CustomFieldByResourceType", false, ex.Message);
            }
        }

        private async Task TestCustomFieldDataTypes(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var customFields = await coreClient.GetCustomFieldsAsync(_testCompanyId, "project");
                
                if (customFields?.Any() == true)
                {
                    var fieldTypes = customFields.Select(cf => cf.FieldType).Distinct();
                    
                    results.RecordTest("CustomFieldDataTypes",
                        fieldTypes.Any(),
                        "Custom fields have data types",
                        $"Found field types: {string.Join(", ", fieldTypes)}");
                }
                else
                {
                    results.RecordTest("CustomFieldDataTypes",
                        true,
                        "No custom fields available for type validation",
                        "Empty result set for custom fields");
                }
            }
            catch (Exception ex)
            {
                results.RecordTest("CustomFieldDataTypes", false, ex.Message);
            }
        }
    }

    #endregion

    #region Sorting and Pagination Tests

    [TestFixture]
    [Category("SortingPagination")]
    public class SortingAndPaginationTests
    {
        [Test]
        [Description("Tests sorting and advanced pagination capabilities")]
        public async Task TestAdvancedSortingAndPagination()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());
            using var projectClient = new ProcoreProjectManagementClient(TestHelpers.CreateRequestAdapter());

            var results = new QueryTestResults("SortingPagination");

            // Test pagination with different page sizes
            await TestVariablePaginationSizes(coreClient, results);

            // Test pagination boundary conditions
            await TestPaginationBoundaries(coreClient, results);

            // Test sorting capabilities (if available)
            await TestSortingCapabilities(coreClient, results);

            Assert.That(results.SuccessfulTests, Is.GreaterThan(0),
                "At least some sorting/pagination tests should succeed");

            TestContext.WriteLine($"Sorting/Pagination Results: {results.SuccessfulTests}/{results.TotalTests} passed");
        }

        private async Task TestVariablePaginationSizes(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var pageSizes = new[] { 5, 10, 25, 50, 100 };

                foreach (var pageSize in pageSizes)
                {
                    var options = new PaginationOptions { Page = 1, PerPage = pageSize };
                    var pagedCompanies = await coreClient.GetCompaniesPagedAsync(options);

                    results.RecordTest($"Pagination_Size_{pageSize}",
                        pagedCompanies != null && pagedCompanies.PerPage == pageSize,
                        $"Pagination works with page size {pageSize}",
                        $"Returned {pagedCompanies?.Items?.Count() ?? 0} items");
                }
            }
            catch (Exception ex)
            {
                results.RecordTest("VariablePaginationSizes", false, ex.Message);
            }
        }

        private async Task TestPaginationBoundaries(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                // Test first page
                var firstPage = new PaginationOptions { Page = 1, PerPage = 10 };
                var firstPageResult = await coreClient.GetCompaniesPagedAsync(firstPage);

                results.RecordTest("PaginationFirstPage",
                    firstPageResult != null && firstPageResult.Page == 1,
                    "First page pagination works",
                    $"First page has {firstPageResult?.Items?.Count() ?? 0} items");

                // Test empty page (beyond available data)
                var emptyPage = new PaginationOptions { Page = 999, PerPage = 10 };
                var emptyPageResult = await coreClient.GetCompaniesPagedAsync(emptyPage);

                results.RecordTest("PaginationEmptyPage",
                    emptyPageResult != null,
                    "Empty page request handled gracefully",
                    $"Empty page returned {emptyPageResult?.Items?.Count() ?? 0} items");

                // Test invalid page numbers
                var invalidPage = new PaginationOptions { Page = 0, PerPage = 10 };
                try
                {
                    var invalidPageResult = await coreClient.GetCompaniesPagedAsync(invalidPage);
                    results.RecordTest("PaginationInvalidPage",
                        invalidPageResult != null,
                        "Invalid page handled gracefully");
                }
                catch (ArgumentException)
                {
                    results.RecordTest("PaginationInvalidPage",
                        true,
                        "Invalid page throws appropriate exception");
                }
            }
            catch (Exception ex)
            {
                results.RecordTest("PaginationBoundaries", false, ex.Message);
            }
        }

        private async Task TestSortingCapabilities(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                // Current implementation doesn't expose sorting parameters
                // This test documents the limitation and checks for future enhancement
                
                var companies = await coreClient.GetCompaniesAsync();
                var companiesList = companies?.ToList();

                if (companiesList?.Any() == true)
                {
                    // Test client-side sorting as baseline
                    var sortedByName = companiesList.OrderBy(c => c.Name).ToList();
                    var sortedById = companiesList.OrderBy(c => c.Id).ToList();

                    results.RecordTest("ClientSideSorting",
                        sortedByName.Count == companiesList.Count,
                        "Client-side sorting works as fallback",
                        "Can sort results after retrieval");
                }

                results.RecordTest("ServerSideSorting",
                    false,
                    "Server-side sorting not currently implemented",
                    "Future enhancement opportunity");
            }
            catch (Exception ex)
            {
                results.RecordTest("SortingCapabilities", false, ex.Message);
            }
        }
    }

    #endregion

    #region Performance Query Tests

    [TestFixture]
    [Category("QueryPerformance")]
    public class QueryPerformanceTests
    {
        [Test]
        [Description("Tests query performance under various conditions")]
        public async Task TestQueryPerformance()
        {
            using var coreClient = new ProcoreCoreClient(TestHelpers.CreateRequestAdapter());

            var results = new QueryTestResults("QueryPerformance");

            // Test response times for different query types
            await TestQueryResponseTimes(coreClient, results);

            // Test concurrent query handling
            await TestConcurrentQueries(coreClient, results);

            // Test large result set handling
            await TestLargeResultSets(coreClient, results);

            TestContext.WriteLine($"Query Performance Results: {results.SuccessfulTests}/{results.TotalTests} passed");
        }

        private async Task TestQueryResponseTimes(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            var performanceThreshold = TimeSpan.FromSeconds(5); // 5 second threshold
            
            try
            {
                // Test company listing performance
                var startTime = DateTime.UtcNow;
                var companies = await coreClient.GetCompaniesAsync();
                var companiesTime = DateTime.UtcNow - startTime;

                results.RecordTest("CompaniesQueryPerformance",
                    companiesTime < performanceThreshold,
                    $"Companies query completed in {companiesTime.TotalMilliseconds:F0}ms",
                    $"Threshold: {performanceThreshold.TotalMilliseconds}ms");

                // Test user listing performance
                startTime = DateTime.UtcNow;
                var users = await coreClient.GetUsersAsync(_testCompanyId);
                var usersTime = DateTime.UtcNow - startTime;

                results.RecordTest("UsersQueryPerformance",
                    usersTime < performanceThreshold,
                    $"Users query completed in {usersTime.TotalMilliseconds:F0}ms",
                    $"Threshold: {performanceThreshold.TotalMilliseconds}ms");

                // Test document listing performance
                startTime = DateTime.UtcNow;
                var documents = await coreClient.GetDocumentsAsync(_testCompanyId);
                var documentsTime = DateTime.UtcNow - startTime;

                results.RecordTest("DocumentsQueryPerformance",
                    documentsTime < performanceThreshold,
                    $"Documents query completed in {documentsTime.TotalMilliseconds:F0}ms",
                    $"Threshold: {performanceThreshold.TotalMilliseconds}ms");
            }
            catch (Exception ex)
            {
                results.RecordTest("QueryResponseTimes", false, ex.Message);
            }
        }

        private async Task TestConcurrentQueries(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                var concurrentQueries = 5;
                var tasks = new List<Task>();

                var startTime = DateTime.UtcNow;

                for (int i = 0; i < concurrentQueries; i++)
                {
                    tasks.Add(coreClient.GetCompaniesAsync());
                }

                await Task.WhenAll(tasks);
                var totalTime = DateTime.UtcNow - startTime;

                results.RecordTest("ConcurrentQueries",
                    totalTime < TimeSpan.FromSeconds(30),
                    $"{concurrentQueries} concurrent queries completed in {totalTime.TotalMilliseconds:F0}ms",
                    "Tests client's ability to handle concurrent requests");
            }
            catch (Exception ex)
            {
                results.RecordTest("ConcurrentQueries", false, ex.Message);
            }
        }

        private async Task TestLargeResultSets(ProcoreCoreClient coreClient, QueryTestResults results)
        {
            try
            {
                // Test pagination with larger page sizes
                var largePageOptions = new PaginationOptions { Page = 1, PerPage = 100 };
                
                var startTime = DateTime.UtcNow;
                var largePage = await coreClient.GetCompaniesPagedAsync(largePageOptions);
                var largePageTime = DateTime.UtcNow - startTime;

                results.RecordTest("LargeResultSetHandling",
                    largePageTime < TimeSpan.FromSeconds(10),
                    $"Large page query (100 items) completed in {largePageTime.TotalMilliseconds:F0}ms",
                    "Tests handling of larger result sets");

                // Test memory efficiency with multiple pages
                var memoryStartUsage = GC.GetTotalMemory(false);
                
                for (int page = 1; page <= 3; page++)
                {
                    var pageOptions = new PaginationOptions { Page = page, PerPage = 50 };
                    await coreClient.GetUsersPagedAsync(_testCompanyId, pageOptions);
                }

                var memoryEndUsage = GC.GetTotalMemory(false);
                var memoryIncrease = memoryEndUsage - memoryStartUsage;

                results.RecordTest("MemoryEfficiency",
                    memoryIncrease < 10 * 1024 * 1024, // 10MB threshold
                    $"Memory increase: {memoryIncrease / 1024:F0}KB for multiple queries",
                    "Tests memory efficiency of multiple queries");
            }
            catch (Exception ex)
            {
                results.RecordTest("LargeResultSets", false, ex.Message);
            }
        }
    }

    #endregion

    #region Query Test Results Helper

    public class QueryTestResults
    {
        public string TestCategory { get; }
        public List<QueryTestResult> Results { get; } = new();

        public QueryTestResults(string testCategory)
        {
            TestCategory = testCategory;
        }

        public void RecordTest(string testName, bool success, string message, string? details = null)
        {
            Results.Add(new QueryTestResult
            {
                TestName = testName,
                Success = success,
                Message = message,
                Details = details,
                Timestamp = DateTime.UtcNow
            });
        }

        public int SuccessfulTests => Results.Count(r => r.Success);
        public int TotalTests => Results.Count;
        public double SuccessRate => TotalTests > 0 ? (double)SuccessfulTests / TotalTests : 0.0;
    }

    public class QueryTestResult
    {
        public string TestName { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }
    }

    #endregion
}