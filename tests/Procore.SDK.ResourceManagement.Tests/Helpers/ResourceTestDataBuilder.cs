using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.ResourceManagement.Models;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.ResourceManagement.Tests.Helpers;

/// <summary>
/// Test data builder for ResourceManagement domain models and generated client responses.
/// Provides consistent test data creation patterns for all ResourceManagement tests.
/// </summary>
public static class ResourceTestDataBuilder
{
    #region Domain Model Builders

    /// <summary>
    /// Creates a test Resource with default or specified values.
    /// </summary>
    public static Resource CreateTestResource(
        int id = 1, 
        string name = "Test Resource", 
        ResourceType type = ResourceType.Equipment,
        ResourceStatus status = ResourceStatus.Available,
        decimal costPerHour = 100.00m,
        string location = "Test Location")
    {
        return new Resource
        {
            Id = id,
            Name = name,
            Type = type,
            Status = status,
            Description = $"Test description for {name}",
            CostPerHour = costPerHour,
            Location = location,
            AvailableFrom = DateTime.UtcNow,
            AvailableTo = DateTime.UtcNow.AddMonths(6),
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a collection of test Resources.
    /// </summary>
    public static IEnumerable<Resource> CreateTestResourceCollection(int count = 5)
    {
        var resourceTypes = Enum.GetValues<ResourceType>();
        var statuses = Enum.GetValues<ResourceStatus>();
        
        return Enumerable.Range(1, count).Select(i => CreateTestResource(
            id: i,
            name: $"Test Resource {i}",
            type: resourceTypes[i % resourceTypes.Length],
            status: statuses[i % statuses.Length],
            costPerHour: 50m + (i * 25m),
            location: $"Location {(char)('A' + (i % 26))}"
        ));
    }

    /// <summary>
    /// Creates a test ResourceAllocation with default or specified values.
    /// </summary>
    public static ResourceAllocation CreateTestResourceAllocation(
        int id = 1,
        int resourceId = 1,
        int projectId = 100,
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal allocationPercentage = 75.0m,
        AllocationStatus status = AllocationStatus.Planned)
    {
        return new ResourceAllocation
        {
            Id = id,
            ResourceId = resourceId,
            ProjectId = projectId,
            StartDate = startDate ?? DateTime.UtcNow.AddDays(1),
            EndDate = endDate ?? DateTime.UtcNow.AddDays(30),
            AllocationPercentage = allocationPercentage,
            Status = status,
            Notes = $"Test allocation for resource {resourceId}",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a test WorkforceAssignment with default or specified values.
    /// </summary>
    public static WorkforceAssignment CreateTestWorkforceAssignment(
        int id = 1,
        int workerId = 1,
        int projectId = 100,
        string role = "Project Manager",
        decimal hoursPerWeek = 40.0m,
        AssignmentStatus status = AssignmentStatus.Assigned)
    {
        return new WorkforceAssignment
        {
            Id = id,
            WorkerId = workerId,
            ProjectId = projectId,
            Role = role,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(6),
            HoursPerWeek = hoursPerWeek,
            Status = status,
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a test CapacityPlan with default or specified values.
    /// </summary>
    public static CapacityPlan CreateTestCapacityPlan(
        int id = 1,
        int projectId = 100,
        DateTime? planDate = null,
        string resourceCategory = "Equipment",
        decimal requiredCapacity = 100.0m,
        decimal availableCapacity = 120.0m)
    {
        var utilization = availableCapacity > 0 ? (requiredCapacity / availableCapacity) * 100 : 0;
        
        return new CapacityPlan
        {
            Id = id,
            ProjectId = projectId,
            PlanDate = planDate ?? DateTime.UtcNow.Date,
            ResourceCategory = resourceCategory,
            RequiredCapacity = requiredCapacity,
            AvailableCapacity = availableCapacity,
            UtilizationRate = utilization,
            Notes = $"Test capacity plan for {resourceCategory}",
            CreatedAt = DateTime.UtcNow.AddHours(-3),
            UpdatedAt = DateTime.UtcNow
        };
    }

    #endregion

    #region Request Model Builders

    /// <summary>
    /// Creates a test CreateResourceRequest.
    /// </summary>
    public static CreateResourceRequest CreateResourceRequest(
        string name = "Test Equipment Request",
        ResourceType type = ResourceType.Equipment,
        string description = "Test resource description",
        decimal costPerHour = 150.00m,
        string location = "Equipment Yard A",
        DateTime? availableFrom = null,
        DateTime? availableTo = null)
    {
        return new CreateResourceRequest
        {
            Name = name,
            Type = type,
            Description = description,
            CostPerHour = costPerHour,
            Location = location,
            AvailableFrom = availableFrom ?? DateTime.UtcNow,
            AvailableTo = availableTo ?? DateTime.UtcNow.AddMonths(6)
        };
    }

    /// <summary>
    /// Creates a test AllocateResourceRequest.
    /// </summary>
    public static AllocateResourceRequest CreateAllocationRequest(
        int resourceId = 1,
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal allocationPercentage = 75.0m,
        string notes = "Test allocation request")
    {
        return new AllocateResourceRequest
        {
            ResourceId = resourceId,
            StartDate = startDate ?? DateTime.UtcNow.AddDays(1),
            EndDate = endDate ?? DateTime.UtcNow.AddDays(30),
            AllocationPercentage = allocationPercentage,
            Notes = notes
        };
    }

    /// <summary>
    /// Creates a test CreateWorkforceAssignmentRequest.
    /// </summary>
    public static CreateWorkforceAssignmentRequest CreateWorkforceAssignmentRequest(
        int workerId = 1,
        string role = "Project Manager",
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal hoursPerWeek = 40.0m)
    {
        return new CreateWorkforceAssignmentRequest
        {
            WorkerId = workerId,
            Role = role,
            StartDate = startDate ?? DateTime.UtcNow.AddDays(1),
            EndDate = endDate ?? DateTime.UtcNow.AddMonths(6),
            HoursPerWeek = hoursPerWeek
        };
    }

    /// <summary>
    /// Creates a test CreateCapacityPlanRequest.
    /// </summary>
    public static CreateCapacityPlanRequest CreateCapacityPlanRequest(
        DateTime? planDate = null,
        string resourceCategory = "Equipment",
        decimal requiredCapacity = 100.0m,
        decimal availableCapacity = 120.0m,
        string notes = "Test capacity plan request")
    {
        return new CreateCapacityPlanRequest
        {
            PlanDate = planDate ?? DateTime.UtcNow.Date,
            ResourceCategory = resourceCategory,
            RequiredCapacity = requiredCapacity,
            AvailableCapacity = availableCapacity,
            Notes = notes
        };
    }

    #endregion

    #region Generated Client Response Builders

    /// <summary>
    /// Creates a mock generated Resource response.
    /// TODO: Replace with actual generated types when available.
    /// </summary>
    public static object CreateGeneratedResource(
        int id = 1,
        string name = "Generated Resource",
        string type = "Equipment",
        string status = "Available")
    {
        // TODO: Replace with actual generated Resource type
        return new
        {
            Id = id,
            Name = name,
            Type = type,
            Status = status,
            Description = $"Generated description for {name}",
            CostPerHour = 100.00m,
            Location = "Generated Location",
            AvailableFrom = DateTime.UtcNow,
            AvailableTo = DateTime.UtcNow.AddMonths(6),
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a collection of mock generated Resource responses.
    /// </summary>
    public static IEnumerable<object> CreateGeneratedResourceCollection(int count = 5)
    {
        return Enumerable.Range(1, count).Select(i => CreateGeneratedResource(
            id: i,
            name: $"Generated Resource {i}",
            type: i % 2 == 0 ? "Equipment" : "Labor",
            status: i % 3 == 0 ? "Available" : "Allocated"
        ));
    }

    /// <summary>
    /// Creates a mock generated ResourceAllocation response.
    /// TODO: Replace with actual generated types when available.
    /// </summary>
    public static object CreateGeneratedAllocation(
        int id = 1,
        AllocateResourceRequest? request = null,
        int projectId = 100)
    {
        var allocationRequest = request ?? CreateAllocationRequest();
        
        return new
        {
            Id = id,
            ResourceId = allocationRequest.ResourceId,
            ProjectId = projectId,
            StartDate = allocationRequest.StartDate,
            EndDate = allocationRequest.EndDate,
            AllocationPercentage = allocationRequest.AllocationPercentage,
            Status = "Planned",
            Notes = allocationRequest.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a collection of mock generated ResourceAllocation responses.
    /// </summary>
    public static IEnumerable<object> CreateGeneratedAllocationCollection(int count = 3, int projectId = 100)
    {
        return Enumerable.Range(1, count).Select(i => CreateGeneratedAllocation(
            id: i,
            request: CreateAllocationRequest(
                resourceId: i,
                startDate: DateTime.UtcNow.AddDays(i),
                endDate: DateTime.UtcNow.AddDays(30 + i),
                allocationPercentage: 50m + (i * 10m)
            ),
            projectId: projectId
        ));
    }

    #endregion

    #region Error Response Builders

    /// <summary>
    /// Creates a test HTTP error response.
    /// </summary>
    public static System.Net.Http.HttpRequestException CreateHttpException(
        System.Net.HttpStatusCode statusCode,
        string message = "Test error",
        string responseBody = null)
    {
        var exception = new System.Net.Http.HttpRequestException(message);
        exception.Data["StatusCode"] = statusCode;
        if (!string.IsNullOrEmpty(responseBody))
        {
            exception.Data["ResponseBody"] = responseBody;
        }
        return exception;
    }

    /// <summary>
    /// Creates a validation error response for resource operations.
    /// </summary>
    public static object CreateValidationErrorResponse(string field, string error)
    {
        return new
        {
            Error = "ValidationError",
            Message = "Validation failed",
            Errors = new[]
            {
                new { Field = field, Message = error }
            }
        };
    }

    /// <summary>
    /// Creates a conflict error response for resource allocation.
    /// </summary>
    public static object CreateConflictErrorResponse(int resourceId, string reason)
    {
        return new
        {
            Error = "ResourceConflict",
            Message = $"Resource {resourceId} allocation conflict: {reason}",
            ConflictDetails = new
            {
                ResourceId = resourceId,
                Reason = reason,
                ConflictType = "OverAllocation"
            }
        };
    }

    #endregion

    #region Performance Test Data

    /// <summary>
    /// Creates large dataset for performance testing.
    /// </summary>
    public static IEnumerable<Resource> CreateLargeResourceDataset(int count = 1000)
    {
        return CreateTestResourceCollection(count);
    }

    /// <summary>
    /// Creates complex resource allocation scenario for testing.
    /// </summary>
    public static IEnumerable<ResourceAllocation> CreateComplexAllocationScenario(
        int resourceCount = 10,
        int projectCount = 5,
        int allocationsPerResource = 3)
    {
        var allocations = new List<ResourceAllocation>();
        var allocationId = 1;
        
        for (int resourceId = 1; resourceId <= resourceCount; resourceId++)
        {
            for (int allocationIndex = 0; allocationIndex < allocationsPerResource; allocationIndex++)
            {
                var projectId = (allocationIndex % projectCount) + 100;
                var startDate = DateTime.UtcNow.AddDays(allocationIndex * 30);
                var endDate = startDate.AddDays(25); // 5-day gap between allocations
                
                allocations.Add(CreateTestResourceAllocation(
                    id: allocationId++,
                    resourceId: resourceId,
                    projectId: projectId,
                    startDate: startDate,
                    endDate: endDate,
                    allocationPercentage: 60m + (allocationIndex * 10m)
                ));
            }
        }
        
        return allocations;
    }

    #endregion

    #region Analytics Test Data

    /// <summary>
    /// Creates test data for resource utilization analytics.
    /// </summary>
    public static Dictionary<string, decimal> CreateUtilizationAnalyticsData()
    {
        return new Dictionary<string, decimal>
        {
            ["Equipment"] = 85.5m,
            ["Labor"] = 92.3m,
            ["Materials"] = 67.8m,
            ["Vehicles"] = 75.2m,
            ["Tools"] = 88.9m
        };
    }

    /// <summary>
    /// Creates test data for capacity analysis reporting.
    /// </summary>
    public static Dictionary<string, object> CreateCapacityAnalysisData()
    {
        return new Dictionary<string, object>
        {
            ["TotalCapacity"] = 1000.0m,
            ["UsedCapacity"] = 750.0m,
            ["AvailableCapacity"] = 250.0m,
            ["UtilizationRate"] = 75.0m,
            ["OverAllocatedResources"] = 3,
            ["UnderUtilizedResources"] = 7,
            ["OptimalAllocationTarget"] = 85.0m
        };
    }

    /// <summary>
    /// Creates trend data for analytics testing.
    /// </summary>
    public static IEnumerable<object> CreateUtilizationTrendData(int days = 30)
    {
        return Enumerable.Range(1, days).Select(day => new
        {
            Date = DateTime.UtcNow.AddDays(-days + day),
            UtilizationRate = 70m + (decimal)(Math.Sin(day * 0.2) * 15m), // Simulated trend
            ResourceCount = 50 + (day % 10),
            ActiveAllocations = 35 + (day % 15)
        });
    }

    #endregion

    #region Pagination Test Data

    /// <summary>
    /// Creates pagination options for testing.
    /// </summary>
    public static CoreModels.PaginationOptions CreatePaginationOptions(
        int page = 1,
        int perPage = 25)
    {
        return new CoreModels.PaginationOptions
        {
            Page = page,
            PerPage = perPage
        };
    }

    /// <summary>
    /// Creates a paged result for testing.
    /// </summary>
    public static CoreModels.PagedResult<T> CreatePagedResult<T>(
        IEnumerable<T> items,
        int totalCount,
        int page = 1,
        int perPage = 25)
    {
        var itemsList = items.ToList();
        var totalPages = (int)Math.Ceiling((double)totalCount / perPage);
        
        return new CoreModels.PagedResult<T>
        {
            Items = itemsList,
            TotalCount = totalCount,
            Page = page,
            PerPage = perPage,
            TotalPages = totalPages,
            HasNextPage = page < totalPages,
            HasPreviousPage = page > 1
        };
    }

    #endregion
}