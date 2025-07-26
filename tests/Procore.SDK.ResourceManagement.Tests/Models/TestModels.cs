using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.ResourceManagement.Tests.Models;

/// <summary>
/// Test domain models for ResourceManagement client testing
/// </summary>

// Core Resource Models
public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public ResourceStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? CostPerHour { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }
}

public enum ResourceType
{
    Equipment,
    Labor,
    Material,
    Vehicle,
    Tool
}

public enum ResourceStatus
{
    Available,
    Allocated,
    InUse,
    Maintenance,
    Unavailable
}

public class ResourceAllocation
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public int ProjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AllocationPercentage { get; set; }
    public AllocationStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public enum AllocationStatus
{
    Planned,
    Active,
    Completed,
    Cancelled,
    OnHold
}

public class WorkforceAssignment
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public int ProjectId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HoursPerWeek { get; set; }
    public AssignmentStatus Status { get; set; }
}

public enum AssignmentStatus
{
    Assigned,
    Active,
    Completed,
    Terminated,
    OnLeave
}

public class CapacityPlan
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public DateTime PlanDate { get; set; }
    public string ResourceCategory { get; set; } = string.Empty;
    public decimal RequiredCapacity { get; set; }
    public decimal AvailableCapacity { get; set; }
    public decimal UtilizationRate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

// Request Models
public class CreateResourceRequest
{
    public string Name { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? CostPerHour { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }
}

public class AllocateResourceRequest
{
    public int ResourceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AllocationPercentage { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreateWorkforceAssignmentRequest
{
    public int WorkerId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HoursPerWeek { get; set; }
}

public class CreateCapacityPlanRequest
{
    public DateTime PlanDate { get; set; }
    public string ResourceCategory { get; set; } = string.Empty;
    public decimal RequiredCapacity { get; set; }
    public decimal AvailableCapacity { get; set; }
    public string Notes { get; set; } = string.Empty;
}

// Pagination Models
public class PaginationOptions
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 100;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// Defines the contract for the ResourceManagement client wrapper
/// </summary>
public interface IResourceManagementClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }

    // Resource Operations
    Task<IEnumerable<Resource>> GetResourcesAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Resource> GetResourceAsync(int companyId, int resourceId, CancellationToken cancellationToken = default);
    Task<Resource> CreateResourceAsync(int companyId, CreateResourceRequest request, CancellationToken cancellationToken = default);
    Task<Resource> UpdateResourceAsync(int companyId, int resourceId, CreateResourceRequest request, CancellationToken cancellationToken = default);
    Task DeleteResourceAsync(int companyId, int resourceId, CancellationToken cancellationToken = default);

    // Resource Allocation
    Task<IEnumerable<ResourceAllocation>> GetResourceAllocationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<ResourceAllocation> GetResourceAllocationAsync(int companyId, int projectId, int allocationId, CancellationToken cancellationToken = default);
    Task<ResourceAllocation> AllocateResourceAsync(int companyId, int projectId, AllocateResourceRequest request, CancellationToken cancellationToken = default);
    Task<ResourceAllocation> UpdateAllocationAsync(int companyId, int projectId, int allocationId, AllocateResourceRequest request, CancellationToken cancellationToken = default);
    Task ReleaseResourceAsync(int companyId, int projectId, int allocationId, CancellationToken cancellationToken = default);

    // Workforce Management
    Task<IEnumerable<WorkforceAssignment>> GetWorkforceAssignmentsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<WorkforceAssignment> GetWorkforceAssignmentAsync(int companyId, int projectId, int assignmentId, CancellationToken cancellationToken = default);
    Task<WorkforceAssignment> CreateWorkforceAssignmentAsync(int companyId, int projectId, CreateWorkforceAssignmentRequest request, CancellationToken cancellationToken = default);
    Task<WorkforceAssignment> UpdateWorkforceAssignmentAsync(int companyId, int projectId, int assignmentId, CreateWorkforceAssignmentRequest request, CancellationToken cancellationToken = default);

    // Capacity Planning
    Task<IEnumerable<CapacityPlan>> GetCapacityPlansAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<CapacityPlan> CreateCapacityPlanAsync(int companyId, int projectId, CreateCapacityPlanRequest request, CancellationToken cancellationToken = default);

    // Resource Analytics
    Task<IEnumerable<Resource>> GetAvailableResourcesAsync(int companyId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Resource>> GetOverAllocatedResourcesAsync(int companyId, CancellationToken cancellationToken = default);
    Task<decimal> GetResourceUtilizationRateAsync(int companyId, int resourceId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, decimal>> GetCapacityAnalysisAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Optimization
    Task<IEnumerable<Resource>> OptimizeResourceAllocationAsync(int companyId, int projectId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkforceAssignment>> GetOptimalWorkforceAssignmentsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<PagedResult<Resource>> GetResourcesPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<ResourceAllocation>> GetResourceAllocationsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<WorkforceAssignment>> GetWorkforceAssignmentsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
}