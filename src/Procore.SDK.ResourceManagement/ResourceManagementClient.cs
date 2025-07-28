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
using Procore.SDK.ResourceManagement.TypeMapping;
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
    private readonly StructuredLogger? _structuredLogger;
    private readonly ResourceTypeMapper _resourceTypeMapper;
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
    /// <param name="structuredLogger">Optional structured logger for correlation tracking.</param>
    public ProcoreResourceManagementClient(
        IRequestAdapter requestAdapter, 
        ILogger<ProcoreResourceManagementClient>? logger = null,
        StructuredLogger? structuredLogger = null)
    {
        _generatedClient = new Procore.SDK.ResourceManagement.ResourceManagementClient(requestAdapter);
        _logger = logger;
        _structuredLogger = structuredLogger;
        _resourceTypeMapper = new ResourceTypeMapper();
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
            var mappedException = ErrorMapper.MapHttpException(ex, correlationId);
            
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
                new Dictionary<string, object> { { "inner_exception", ex.GetType().Name } }, 
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
    /// Gets a specific resource by ID (legacy interface compatibility).
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The resource.</returns>
    public async Task<Resource> GetResourceAsync(int companyId, int resourceId, CancellationToken cancellationToken = default)
    {
        // For legacy compatibility, we need a project ID but don't have one
        // This would require additional API calls to discover the project
        throw new NotSupportedException("GetResourceAsync requires projectId parameter. Use GetResourceAsync(companyId, projectId, resourceId) instead.");
    }

    /// <summary>
    /// Gets a specific resource by ID from a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The resource.</returns>
    public async Task<Resource> GetResourceAsync(int companyId, int projectId, int resourceId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Getting resource {ResourceId} for project {ProjectId} in company {CompanyId} using generated Kiota client", resourceId, projectId, companyId);
            
            // Use the generated Kiota client to get the specific resource
            var resourceResponse = await _generatedClient.Rest.V11.Projects[projectId].Schedule.Resources[resourceId].GetAsync(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (resourceResponse == null)
            {
                throw new CoreModels.ProcoreCoreException($"Resource {resourceId} not found in project {projectId}", "RESOURCE_NOT_FOUND");
            }
            
            // Map from generated response to our domain model using type mapper
            return _resourceTypeMapper.MapToWrapper(resourceResponse);
        }, "GetResourceAsync", null, cancellationToken).ConfigureAwait(false);
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
    /// Updates an existing resource (legacy interface compatibility).
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="request">The resource update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated resource.</returns>
    public async Task<Resource> UpdateResourceAsync(int companyId, int resourceId, CreateResourceRequest request, CancellationToken cancellationToken = default)
    {
        // For legacy compatibility, we need a project ID but don't have one
        throw new NotSupportedException("UpdateResourceAsync requires projectId parameter. Use UpdateResourceAsync(companyId, projectId, resourceId, request) instead.");
    }

    /// <summary>
    /// Updates an existing resource in a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="request">The resource update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated resource.</returns>
    public async Task<Resource> UpdateResourceAsync(int companyId, int projectId, int resourceId, CreateResourceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Updating resource {ResourceId} for project {ProjectId} in company {CompanyId} using generated Kiota client", resourceId, projectId, companyId);
            
            // Create the request body for V1.1 Resources PATCH endpoint
            var requestBody = new global::Procore.SDK.ResourceManagement.Rest.V11.Projects.Item.Schedule.Resources.Item.ResourcesPatchRequestBody
            {
                ProjectId = projectId,
                Resource = new global::Procore.SDK.ResourceManagement.Rest.V11.Projects.Item.Schedule.Resources.Item.ResourcesPatchRequestBody_resource
                {
                    Name = request.Name,
                    SourceUid = request.Name?.Replace(" ", "_") // Generate a basic source UID from name
                }
            };
            
            // Use the generated Kiota client to update the resource
            var resourceResponse = await _generatedClient.Rest.V11.Projects[projectId].Schedule.Resources[resourceId].PatchAsync(
                requestBody, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (resourceResponse == null)
            {
                throw new CoreModels.ProcoreCoreException($"Failed to update resource {resourceId} in project {projectId}", "RESOURCE_UPDATE_FAILED");
            }
            
            // The patch response has a different structure for ScheduleAttributes than the get response
            // We'll create a minimal get response structure without ScheduleAttributes for now
            var getResponse = new global::Procore.SDK.ResourceManagement.Rest.V11.Projects.Item.Schedule.Resources.Item.ResourcesGetResponse
            {
                Id = resourceResponse.Id,
                Name = resourceResponse.Name,
                CompanyId = resourceResponse.CompanyId,
                ProjectId = resourceResponse.ProjectId,
                SourceUid = resourceResponse.SourceUid,
                DeletedAt = resourceResponse.DeletedAt
                // Note: ScheduleAttributes has different types between PATCH and GET responses
            };
            
            return _resourceTypeMapper.MapToWrapper(getResponse);
        }, "UpdateResourceAsync", null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a resource (legacy interface compatibility).
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteResourceAsync(int companyId, int resourceId, CancellationToken cancellationToken = default)
    {
        // For legacy compatibility, we need a project ID but don't have one
        throw new NotSupportedException("DeleteResourceAsync requires projectId parameter. Use DeleteResourceAsync(companyId, projectId, resourceId) instead.");
    }

    /// <summary>
    /// Deletes a resource from a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="resourceId">The resource ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteResourceAsync(int companyId, int projectId, int resourceId, CancellationToken cancellationToken = default)
    {
        await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Deleting resource {ResourceId} for project {ProjectId} in company {CompanyId} using generated Kiota client", resourceId, projectId, companyId);
            
            // Use the generated Kiota client to delete the resource
            await _generatedClient.Rest.V11.Projects[projectId].Schedule.Resources[resourceId].DeleteAsync(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            
            _logger?.LogDebug("Successfully deleted resource {ResourceId} for project {ProjectId} in company {CompanyId}", resourceId, projectId, companyId);
        }, "DeleteResourceAsync", null, cancellationToken).ConfigureAwait(false);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting resource allocations for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<ResourceAllocation>();
            },
            $"GetResourceAllocations-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(async () =>
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
        }, $"GetResourceAllocation-{allocationId}-Project-{projectId}-Company-{companyId}", null, cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(async () =>
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
        }, $"UpdateAllocation-{allocationId}-Project-{projectId}-Company-{companyId}", null, cancellationToken);
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
        await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Releasing resource allocation {AllocationId} for project {ProjectId} in company {CompanyId}", allocationId, projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
        }, $"ReleaseResource-{allocationId}-Project-{projectId}-Company-{companyId}", null, cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting workforce assignments for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<WorkforceAssignment>();
            },
            $"GetWorkforceAssignments-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(async () =>
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
        }, $"GetWorkforceAssignment-{assignmentId}-Project-{projectId}-Company-{companyId}", null, cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(
            async () =>
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
            },
            $"CreateWorkforceAssignment-Worker-{request.WorkerId}-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(async () =>
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
        }, $"UpdateWorkforceAssignment-{assignmentId}-Project-{projectId}-Company-{companyId}", null, cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting capacity plans for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<CapacityPlan>();
            },
            $"GetCapacityPlans-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(
            async () =>
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
            },
            $"CreateCapacityPlan-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting available resources for company {CompanyId} from {StartDate} to {EndDate}", companyId, startDate, endDate);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<Resource>();
            },
            $"GetAvailableResources-Company-{companyId}-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets over-allocated resources.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of over-allocated resources.</returns>
    public async Task<IEnumerable<Resource>> GetOverAllocatedResourcesAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting over-allocated resources for company {CompanyId}", companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<Resource>();
            },
            $"GetOverAllocatedResources-Company-{companyId}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting utilization rate for resource {ResourceId} in company {CompanyId}", resourceId, companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return 78.5m;
            },
            $"GetResourceUtilizationRate-Resource-{resourceId}-Company-{companyId}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
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
            },
            $"GetCapacityAnalysis-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Optimizing resource allocation for project {ProjectId} in company {CompanyId} from {StartDate} to {EndDate}", projectId, companyId, startDate, endDate);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<Resource>();
            },
            $"OptimizeResourceAllocation-Project-{projectId}-Company-{companyId}-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}",
            null,
            cancellationToken);
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
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting optimal workforce assignments for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<WorkforceAssignment>();
            },
            $"GetOptimalWorkforceAssignments-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(
            async () =>
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
            },
            $"GetResourcesPaged-Company-{companyId}-Page-{options.Page}",
            null,
            cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(
            async () =>
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
            },
            $"GetResourceAllocationsPaged-Project-{projectId}-Company-{companyId}-Page-{options.Page}",
            null,
            cancellationToken);
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
        
        return await ExecuteWithResilienceAsync(
            async () =>
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
            },
            $"GetWorkforceAssignmentsPaged-Project-{projectId}-Company-{companyId}-Page-{options.Page}",
            null,
            cancellationToken);
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