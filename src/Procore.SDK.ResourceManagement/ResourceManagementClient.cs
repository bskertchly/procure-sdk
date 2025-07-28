using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.ErrorHandling;
using Procore.SDK.Core.Logging;
using Procore.SDK.ResourceManagement.Models;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.ResourceManagement;

/// <summary>
/// Implementation of the ResourceManagement client wrapper that provides domain-specific 
/// convenience methods over the generated Kiota client.
/// </summary>
public class ProcoreResourceManagementClient : IResourceManagementClient
{
    private readonly Procore.SDK.ResourceManagement.ResourceManagementClient _generatedClient;
    private readonly ILogger<ProcoreResourceManagementClient>? _logger;
    private readonly ErrorMapper? _errorMapper;
    private readonly StructuredLogger? _structuredLogger;
    private bool _disposed;

    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    public object RawClient => _generatedClient;

    /// <summary>
    /// Initializes a new instance of the ProcoreResourceManagementClient.
    /// </summary>
    /// <param name="requestAdapter">The request adapter to use for HTTP communication.</param>
    /// <param name="logger">Optional logger for diagnostic information.</param>
    /// <param name="errorMapper">Optional error mapper for exception handling.</param>
    /// <param name="structuredLogger">Optional structured logger for correlation tracking.</param>
    public ProcoreResourceManagementClient(
        IRequestAdapter requestAdapter, 
        ILogger<ProcoreResourceManagementClient>? logger = null,
        ErrorMapper? errorMapper = null,
        StructuredLogger? structuredLogger = null)
    {
        _generatedClient = new Procore.SDK.ResourceManagement.ResourceManagementClient(requestAdapter);
        _logger = logger;
        _errorMapper = errorMapper;
        _structuredLogger = structuredLogger;
    }

    #region Private Helper Methods

    /// <summary>
    /// Executes an operation with proper error handling and logging.
    /// </summary>
    private async Task<T> ExecuteWithResilienceAsync<T>(
        Func<Task<T>> operation,
        string operationName,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        correlationId ??= Guid.NewGuid().ToString();
        
        using var operationScope = _structuredLogger?.BeginOperation(operationName, correlationId);
        
        try
        {
            _logger?.LogDebug("Executing operation {Operation} with correlation ID {CorrelationId}", operationName, correlationId);
            
            return await operation().ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            var mappedException = _errorMapper?.MapHttpException(ex, correlationId) ?? 
                new CoreModels.ProcoreCoreException(ex.Message, "HTTP_ERROR", null, correlationId);
            
            _structuredLogger?.LogError(mappedException, operationName, correlationId, 
                "HTTP error in operation {Operation}", operationName);
            
            throw mappedException;
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            _structuredLogger?.LogWarning(operationName, correlationId,
                "Operation {Operation} was cancelled", operationName);
            throw;
        }
        catch (Exception ex)
        {
            var wrappedException = new CoreModels.ProcoreCoreException(
                $"Unexpected error in {operationName}: {ex.Message}", 
                "UNEXPECTED_ERROR", 
                null, 
                correlationId);
            
            _structuredLogger?.LogError(wrappedException, operationName, correlationId,
                "Unexpected error in operation {Operation}", operationName);
            
            throw wrappedException;
        }
    }

    /// <summary>
    /// Executes an operation with proper error handling and logging (void return).
    /// </summary>
    private async Task ExecuteWithResilienceAsync(
        Func<Task> operation,
        string operationName,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        await ExecuteWithResilienceAsync(async () =>
        {
            await operation();
            return true; // Return a dummy value
        }, operationName, correlationId, cancellationToken);
    }

    #endregion

    #region Resource Operations

    /// <summary>
    /// Gets all resources for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of resources.</returns>
    public async Task<IEnumerable<Resource>> GetResourcesAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting resources for company {CompanyId}", companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<Resource>();
            },
            $"GetResources-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific resource by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The resource.</returns>
    public async Task<Resource> GetResourceAsync(int companyId, int resourceId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting resource {ResourceId} for company {CompanyId}", resourceId, companyId);
            
            // Placeholder implementation
            return new Resource 
            { 
                Id = resourceId,
                Name = "Excavator CAT 320",
                Type = ResourceType.Equipment,
                Status = ResourceStatus.Available,
                Description = "Heavy construction excavator",
                CostPerHour = 150.00m,
                Location = "Equipment Yard A",
                AvailableFrom = DateTime.UtcNow,
                AvailableTo = DateTime.UtcNow.AddMonths(6),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get resource {ResourceId} for company {CompanyId}", resourceId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The resource creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created resource.</returns>
    public async Task<Resource> CreateResourceAsync(int companyId, CreateResourceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Creating resource for company {CompanyId}", companyId);
                
                // Placeholder implementation
                return new Resource 
                { 
                    Id = 1,
                    Name = request.Name,
                    Type = request.Type,
                    Status = ResourceStatus.Available,
                    Description = request.Description,
                    CostPerHour = request.CostPerHour,
                    Location = request.Location,
                    AvailableFrom = request.AvailableFrom,
                    AvailableTo = request.AvailableTo,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            },
            $"CreateResource-{request.Name}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing resource.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="request">The resource update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated resource.</returns>
    public async Task<Resource> UpdateResourceAsync(int companyId, int resourceId, CreateResourceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Updating resource {ResourceId} for company {CompanyId}", resourceId, companyId);
            
            // Placeholder implementation
            return new Resource 
            { 
                Id = resourceId,
                Name = request.Name,
                Type = request.Type,
                Description = request.Description,
                CostPerHour = request.CostPerHour,
                Location = request.Location,
                AvailableFrom = request.AvailableFrom,
                AvailableTo = request.AvailableTo,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update resource {ResourceId} for company {CompanyId}", resourceId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Deletes a resource.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteResourceAsync(int companyId, int resourceId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Deleting resource {ResourceId} for company {CompanyId}", resourceId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to delete resource {ResourceId} for company {CompanyId}", resourceId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Resource Allocation

    /// <summary>
    /// Gets all resource allocations for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of resource allocations.</returns>
    public async Task<IEnumerable<ResourceAllocation>> GetResourceAllocationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting resource allocations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<ResourceAllocation>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get resource allocations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific resource allocation by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="allocationId">The allocation ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The resource allocation.</returns>
    public async Task<ResourceAllocation> GetResourceAllocationAsync(int companyId, int projectId, int allocationId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            
            // Placeholder implementation
            return new ResourceAllocation 
            { 
                Id = allocationId,
                ResourceId = 1,
                ProjectId = projectId,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(30),
                AllocationPercentage = 75.0m,
                Status = AllocationStatus.Planned,
                Notes = "Allocated for foundation work",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Allocates a resource to a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The resource allocation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created resource allocation.</returns>
    public async Task<ResourceAllocation> AllocateResourceAsync(int companyId, int projectId, AllocateResourceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Allocating resource {ResourceId} to project {ProjectId} in company {CompanyId}", request.ResourceId, projectId, companyId);
                
                // Placeholder implementation
                return new ResourceAllocation 
                { 
                    Id = 1,
                    ResourceId = request.ResourceId,
                    ProjectId = projectId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    AllocationPercentage = request.AllocationPercentage,
                    Status = AllocationStatus.Planned,
                    Notes = request.Notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            },
            $"AllocateResource-{request.ResourceId}-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing resource allocation.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="allocationId">The allocation ID.</param>
    /// <param name="request">The allocation update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated resource allocation.</returns>
    public async Task<ResourceAllocation> UpdateAllocationAsync(int companyId, int projectId, int allocationId, AllocateResourceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Updating resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            
            // Placeholder implementation
            return new ResourceAllocation 
            { 
                Id = allocationId,
                ResourceId = request.ResourceId,
                ProjectId = projectId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AllocationPercentage = request.AllocationPercentage,
                Notes = request.Notes,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Releases a resource allocation.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="allocationId">The allocation ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task ReleaseResourceAsync(int companyId, int projectId, int allocationId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Releasing resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to release resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Workforce Management

    /// <summary>
    /// Gets all workforce assignments for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of workforce assignments.</returns>
    public async Task<IEnumerable<WorkforceAssignment>> GetWorkforceAssignmentsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting workforce assignments for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<WorkforceAssignment>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get workforce assignments for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific workforce assignment by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="assignmentId">The assignment ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The workforce assignment.</returns>
    public async Task<WorkforceAssignment> GetWorkforceAssignmentAsync(int companyId, int projectId, int assignmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting workforce assignment {AssignmentId} for project {ProjectId} in company {CompanyId}", assignmentId, projectId, companyId);
            
            // Placeholder implementation
            return new WorkforceAssignment 
            { 
                Id = assignmentId,
                WorkerId = 1,
                ProjectId = projectId,
                Role = "Project Manager",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddMonths(6),
                HoursPerWeek = 40.0m,
                Status = AssignmentStatus.Assigned,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get workforce assignment {AssignmentId} for project {ProjectId} in company {CompanyId}", assignmentId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Creates a new workforce assignment.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The workforce assignment creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created workforce assignment.</returns>
    public async Task<WorkforceAssignment> CreateWorkforceAssignmentAsync(int companyId, int projectId, CreateWorkforceAssignmentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Creating workforce assignment for worker {WorkerId} to project {ProjectId} in company {CompanyId}", request.WorkerId, projectId, companyId);
            
            // Placeholder implementation
            return new WorkforceAssignment 
            { 
                Id = 1,
                WorkerId = request.WorkerId,
                ProjectId = projectId,
                Role = request.Role,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                HoursPerWeek = request.HoursPerWeek,
                Status = AssignmentStatus.Assigned,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create workforce assignment for worker {WorkerId} to project {ProjectId} in company {CompanyId}", request.WorkerId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Updates an existing workforce assignment.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="assignmentId">The assignment ID.</param>
    /// <param name="request">The assignment update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated workforce assignment.</returns>
    public async Task<WorkforceAssignment> UpdateWorkforceAssignmentAsync(int companyId, int projectId, int assignmentId, CreateWorkforceAssignmentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Updating workforce assignment {AssignmentId} for project {ProjectId} in company {CompanyId}", assignmentId, projectId, companyId);
            
            // Placeholder implementation
            return new WorkforceAssignment 
            { 
                Id = assignmentId,
                WorkerId = request.WorkerId,
                ProjectId = projectId,
                Role = request.Role,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                HoursPerWeek = request.HoursPerWeek,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update workforce assignment {AssignmentId} for project {ProjectId} in company {CompanyId}", assignmentId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Capacity Planning

    /// <summary>
    /// Gets all capacity plans for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of capacity plans.</returns>
    public async Task<IEnumerable<CapacityPlan>> GetCapacityPlansAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting capacity plans for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<CapacityPlan>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get capacity plans for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Creates a new capacity plan.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The capacity plan creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created capacity plan.</returns>
    public async Task<CapacityPlan> CreateCapacityPlanAsync(int companyId, int projectId, CreateCapacityPlanRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Creating capacity plan for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Calculate utilization rate
            var utilizationRate = request.AvailableCapacity > 0 ? (request.RequiredCapacity / request.AvailableCapacity) * 100 : 0;
            
            // Placeholder implementation
            return new CapacityPlan 
            { 
                Id = 1,
                ProjectId = projectId,
                PlanDate = request.PlanDate,
                ResourceCategory = request.ResourceCategory,
                RequiredCapacity = request.RequiredCapacity,
                AvailableCapacity = request.AvailableCapacity,
                UtilizationRate = utilizationRate,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create capacity plan for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Resource Analytics

    /// <summary>
    /// Gets available resources within a date range.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of available resources.</returns>
    public async Task<IEnumerable<Resource>> GetAvailableResourcesAsync(int companyId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting available resources for company {CompanyId} from {StartDate} to {EndDate}", companyId, startDate, endDate);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<Resource>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get available resources for company {CompanyId}", companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets over-allocated resources.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of over-allocated resources.</returns>
    public async Task<IEnumerable<Resource>> GetOverAllocatedResourcesAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting over-allocated resources for company {CompanyId}", companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<Resource>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get over-allocated resources for company {CompanyId}", companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets the utilization rate for a specific resource.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The utilization rate as a percentage.</returns>
    public async Task<decimal> GetResourceUtilizationRateAsync(int companyId, int resourceId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting utilization rate for resource {ResourceId} in company {CompanyId}", resourceId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return 78.5m;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get utilization rate for resource {ResourceId} in company {CompanyId}", resourceId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets capacity analysis for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with capacity analysis by resource category.</returns>
    public async Task<Dictionary<string, decimal>> GetCapacityAnalysisAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting capacity analysis for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return new Dictionary<string, decimal>
            {
                ["Equipment"] = 85.5m,
                ["Labor"] = 92.3m,
                ["Materials"] = 67.8m,
                ["Vehicles"] = 75.2m,
                ["Tools"] = 88.9m
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get capacity analysis for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Optimization

    /// <summary>
    /// Optimizes resource allocation for a project within a date range.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of optimized resource allocations.</returns>
    public async Task<IEnumerable<Resource>> OptimizeResourceAllocationAsync(int companyId, int projectId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Optimizing resource allocation for project {ProjectId} in company {CompanyId} from {StartDate} to {EndDate}", projectId, companyId, startDate, endDate);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<Resource>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to optimize resource allocation for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets optimal workforce assignments for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of optimal workforce assignments.</returns>
    public async Task<IEnumerable<WorkforceAssignment>> GetOptimalWorkforceAssignmentsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting optimal workforce assignments for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<WorkforceAssignment>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get optimal workforce assignments for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Pagination Support

    /// <summary>
    /// Gets resources with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of resources.</returns>
    public async Task<CoreModels.PagedResult<Resource>> GetResourcesPagedAsync(int companyId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting resources with pagination for company {CompanyId} (page {Page}, per page {PerPage})", companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<Resource>
            {
                Items = Enumerable.Empty<Resource>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get resources with pagination for company {CompanyId}", companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets resource allocations with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of resource allocations.</returns>
    public async Task<CoreModels.PagedResult<ResourceAllocation>> GetResourceAllocationsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting resource allocations with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<ResourceAllocation>
            {
                Items = Enumerable.Empty<ResourceAllocation>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get resource allocations with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets workforce assignments with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of workforce assignments.</returns>
    public async Task<CoreModels.PagedResult<WorkforceAssignment>> GetWorkforceAssignmentsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting workforce assignments with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<WorkforceAssignment>
            {
                Items = Enumerable.Empty<WorkforceAssignment>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get workforce assignments with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Disposes of the ProcoreResourceManagementClient and its resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the ProcoreResourceManagementClient and its resources.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // The generated client doesn't implement IDisposable, so we don't dispose it
            _disposed = true;
        }
    }

    #endregion
}