using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.ResourceManagement.Models;

/// <summary>
/// Defines the contract for the ResourceManagement client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
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