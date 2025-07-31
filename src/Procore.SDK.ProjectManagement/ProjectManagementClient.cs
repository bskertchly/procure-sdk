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
using Procore.SDK.Core.TypeMapping;
using CoreModels = Procore.SDK.Core.Models;
using ProjectModels = Procore.SDK.ProjectManagement.Models;
using Procore.SDK.ProjectManagement.Models;
using Procore.SDK.ProjectManagement.TypeMapping;
using GeneratedProject = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse;

namespace Procore.SDK.ProjectManagement;

/// <summary>
/// Implementation of the ProjectManagement client wrapper that provides domain-specific 
/// convenience methods over the generated Kiota client.
/// </summary>
public class ProcoreProjectManagementClient : ProjectModels.IProjectManagementClient
{
    private readonly Procore.SDK.ProjectManagement.ProjectManagementClient _generatedClient;
    private readonly IRequestAdapter _requestAdapter;
    private readonly ILogger<ProcoreProjectManagementClient>? _logger;
    private readonly StructuredLogger? _structuredLogger;
    private readonly ITypeMapper<Project, GeneratedProject>? _projectMapper;
    private bool _disposed;

    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    public object RawClient => _generatedClient;

    /// <summary>
    /// Initializes a new instance of the ProcoreProjectManagementClient.
    /// </summary>
    /// <param name="requestAdapter">The request adapter to use for HTTP communication.</param>
    /// <param name="logger">Optional logger for diagnostic information.</param>
    /// <param name="structuredLogger">Optional structured logger for correlation tracking.</param>
    /// <param name="projectMapper">Optional type mapper for Project conversions.</param>
    public ProcoreProjectManagementClient(
        IRequestAdapter requestAdapter, 
        ILogger<ProcoreProjectManagementClient>? logger = null,
        StructuredLogger? structuredLogger = null,
        ITypeMapper<Project, GeneratedProject>? projectMapper = null)
    {
        _generatedClient = new Procore.SDK.ProjectManagement.ProjectManagementClient(requestAdapter);
        _requestAdapter = requestAdapter;
        _logger = logger;
        _structuredLogger = structuredLogger;
        _projectMapper = projectMapper ?? new ProjectTypeMapper();
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
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
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

    /// <summary>
    /// Maps a Core project model to a ProjectManagement project model.
    /// </summary>
    /// <param name="coreProject">The Core project model.</param>
    /// <param name="companyId">The company ID to set in the mapped project.</param>
    /// <returns>The ProjectManagement project model.</returns>
    private ProjectModels.Project MapCoreProjectToProjectManagement(Procore.SDK.Core.Rest.V10.Companies.Item.Projects.Projects coreProject, int companyId)
    {
        return new ProjectModels.Project
        {
            Id = coreProject.Id ?? 0,
            Name = coreProject.Name ?? string.Empty,
            Description = string.Empty, // Core model doesn't include description
            Status = ProjectModels.ProjectStatus.Active, // Default status as Core model doesn't map directly
            StartDate = null, // Core model doesn't include dates in this response
            EndDate = null,
            CompanyId = companyId,
            Budget = null, // Core model doesn't include budget in list response
            ProjectType = string.Empty, // Core model doesn't include type in list response
            Phase = ProjectModels.ProjectPhase.Construction, // Default phase
            IsActive = true, // Assume active since it's returned in the list
            CreatedAt = DateTime.UtcNow, // Core model doesn't include timestamps in list response
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Maps generated budget change status to wrapper domain model status.
    /// </summary>
    /// <param name="status">The generated budget change status.</param>
    /// <returns>The mapped wrapper domain model status.</returns>
    private static ProjectModels.BudgetChangeStatus MapGeneratedBudgetChangeStatusToWrapper(
        Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesGetResponse_data_status? status)
    {
        return status switch
        {
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesGetResponse_data_status.Draft => ProjectModels.BudgetChangeStatus.Draft,
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesGetResponse_data_status.Approved => ProjectModels.BudgetChangeStatus.Approved,
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesGetResponse_data_status.Under_review => ProjectModels.BudgetChangeStatus.UnderReview,
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesGetResponse_data_status.Void => ProjectModels.BudgetChangeStatus.Void,
            _ => ProjectModels.BudgetChangeStatus.Draft
        };
    }

    /// <summary>
    /// Maps generated budget change POST response status to wrapper domain model status.
    /// </summary>
    /// <param name="status">The generated budget change POST response status.</param>
    /// <returns>The mapped wrapper domain model status.</returns>
    private static ProjectModels.BudgetChangeStatus MapGeneratedBudgetChangePostStatusToWrapper(
        Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostResponse_data_status? status)
    {
        return status switch
        {
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostResponse_data_status.Draft => ProjectModels.BudgetChangeStatus.Draft,
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostResponse_data_status.Approved => ProjectModels.BudgetChangeStatus.Approved,
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostResponse_data_status.Under_review => ProjectModels.BudgetChangeStatus.UnderReview,
            Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostResponse_data_status.Void => ProjectModels.BudgetChangeStatus.Void,
            _ => ProjectModels.BudgetChangeStatus.Draft
        };
    }

    #endregion

    #region Project Operations

    /// <summary>
    /// Gets all projects for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of projects.</returns>
    public async Task<IEnumerable<ProjectModels.Project>> GetProjectsAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting projects for company {CompanyId}", companyId);
                
                // Use Core client to access the company projects endpoint since
                // ProjectManagement client doesn't have a projects listing endpoint
                var coreClient = new Procore.SDK.Core.CoreClient(_requestAdapter);
                var coreProjects = await coreClient.Rest.V10.Companies[companyId].Projects
                    .GetAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                
                if (coreProjects == null || !coreProjects.Any())
                {
                    _logger?.LogWarning("No projects returned for company {CompanyId}", companyId);
                    return Enumerable.Empty<ProjectModels.Project>();
                }
                
                // Map from Core generated models to ProjectManagement domain models
                return coreProjects.Select(p => MapCoreProjectToProjectManagement(p, companyId));
            },
            $"GetProjects-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific project by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The project.</returns>
    public async Task<ProjectModels.Project> GetProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting project {ProjectId} for company {CompanyId}", projectId, companyId);
                
                // Call the generated client
                var generatedProject = await _generatedClient.Rest.V10.Projects[projectId]
                    .GetAsync(cancellationToken: cancellationToken);
                
                if (generatedProject == null)
                {
                    throw new CoreModels.ProcoreCoreException(
                        $"Project {projectId} not found in company {companyId}",
                        "PROJECT_NOT_FOUND",
                        null);
                }

                // Map to wrapper domain model using type mapper
                return _projectMapper!.MapToWrapper(generatedProject);
            },
            $"GetProject-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The project creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created project.</returns>
    public Task<ProjectModels.Project> CreateProjectAsync(int companyId, ProjectModels.CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger?.LogDebug("Attempting to create project {ProjectName} for company {CompanyId}", request.Name, companyId);
        
        // Note: This is a placeholder implementation as the V1.0 API does not
        // provide a project creation endpoint. This would typically be handled
        // through the Procore web interface or higher version APIs.
        return Task.FromException<ProjectModels.Project>(new NotImplementedException(
            "Project creation is not supported in the V1.0 API. " +
            "Please use the Procore web interface or contact your administrator."));
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The project update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated project.</returns>
    public async Task<ProjectModels.Project> UpdateProjectAsync(int companyId, int projectId, ProjectModels.UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Updating project {ProjectId} for company {CompanyId}", projectId, companyId);
                
                // Create the PATCH request body for the generated client
                var patchRequestBody = new Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.PatchRequestBody
                {
                    CompanyId = companyId,
                    Project = new Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.PatchRequestBody_project()
                };

                // Map from domain request to generated request
                if (!string.IsNullOrEmpty(request.Name))
                {
                    patchRequestBody.Project.Name = request.Name;
                }

                if (!string.IsNullOrEmpty(request.Description))
                {
                    patchRequestBody.Project.Description = request.Description;
                }

                if (request.StartDate.HasValue)
                {
                    patchRequestBody.Project.StartDate = DateOnly.FromDateTime(request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    patchRequestBody.Project.CompletionDate = DateOnly.FromDateTime(request.EndDate.Value);
                }

                if (request.Budget.HasValue)
                {
                    patchRequestBody.Project.TotalValue = (float)request.Budget.Value;
                }

                // Map status - for now just set active flag based on status
                if (request.Status.HasValue)
                {
                    patchRequestBody.Project.Active = request.Status.Value == ProjectStatus.Active;
                }
                
                // Call the generated client
                var patchResponse = await _generatedClient.Rest.V10.Projects[projectId]
                    .PatchAsync(patchRequestBody, cancellationToken: cancellationToken);
                
                if (patchResponse == null)
                {
                    throw new CoreModels.ProcoreCoreException(
                        $"Failed to update project {projectId} in company {companyId}",
                        "PROJECT_UPDATE_FAILED",
                        null);
                }

                // Map the response back to our domain model using the patch response overload
                return ((ProjectTypeMapper)_projectMapper!).MapToWrapper(patchResponse);
            },
            $"UpdateProject-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Deletes a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotImplementedException">Thrown because project deletion is not supported in V1.0 API.</exception>
    public Task DeleteProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to delete project {ProjectId} for company {CompanyId}", projectId, companyId);
        
        // Note: Project deletion is not supported in the V1.0 API
        // This would typically be handled through administrative functions
        return Task.FromException(new NotImplementedException(
            "Project deletion is not supported in the V1.0 API. " +
            "Please use the Procore web interface or contact your administrator."));
    }

    #endregion

    #region Budget Operations

    /// <summary>
    /// Gets all budget line items for a project.
    /// Note: V1.0 API only provides budget lock status. For detailed budget line items, 
    /// V2.0 API would be required but is not currently supported by this client.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of budget line items.</returns>
    public async Task<IEnumerable<ProjectModels.BudgetLineItem>> GetBudgetLineItemsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting budget information for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Call the generated V1.0 budget endpoint
                var budgetResponse = await _generatedClient.Rest.V10.Projects[projectId].Budget
                    .GetAsync(cancellationToken: cancellationToken);
                
                if (budgetResponse == null)
                {
                    _logger?.LogWarning("No budget information returned for project {ProjectId} in company {CompanyId}", projectId, companyId);
                    return Enumerable.Empty<ProjectModels.BudgetLineItem>();
                }

                // V1.0 API only provides budget lock status, not detailed line items
                // Create a single placeholder line item to indicate budget exists but details require V2.0 API
                if (budgetResponse.Locked.HasValue)
                {
                    var budgetLineItem = new BudgetLineItem
                    {
                        Id = 1,
                        ProjectId = projectId,
                        WbsCode = "N/A",
                        Description = budgetResponse.Locked.Value ? "Budget (Locked)" : "Budget (Unlocked)",
                        BudgetAmount = 0m, // V1.0 doesn't provide amounts
                        ActualAmount = 0m,
                        VarianceAmount = 0m,
                        CostCode = "N/A",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    return new[] { budgetLineItem };
                }

                return Enumerable.Empty<ProjectModels.BudgetLineItem>();
            },
            $"GetBudgetLineItems-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific budget line item by ID.
    /// Note: V1.0 API does not provide detailed budget line item endpoints.
    /// This method is included for interface completeness but requires V2.0 API for full functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="lineItemId">The budget line item ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The budget line item.</returns>
    /// <exception cref="NotImplementedException">Thrown because detailed budget line items are not supported in V1.0 API.</exception>
    public Task<ProjectModels.BudgetLineItem> GetBudgetLineItemAsync(int companyId, int projectId, int lineItemId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to get budget line item {LineItemId} for project {ProjectId} in company {CompanyId}", lineItemId, projectId, companyId);
        
        // Note: V1.0 API does not provide detailed budget line item endpoints
        // For detailed budget information, V2.0 API would be required
        return Task.FromException<ProjectModels.BudgetLineItem>(new NotImplementedException(
            "Detailed budget line item retrieval is not supported in the V1.0 API. " +
            "Use GetBudgetLineItemsAsync for available budget information or upgrade to V2.0 API."));
    }

    /// <summary>
    /// Creates a new budget change.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The budget change creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created budget change.</returns>
    public async Task<ProjectModels.BudgetChange> CreateBudgetChangeAsync(int companyId, int projectId, ProjectModels.CreateBudgetChangeRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Creating budget change for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Create the POST request body for the generated client
                var postRequestBody = new Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostRequestBody
                {
                    Description = request.Description,
                    Status = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostRequestBody_status.Draft,
                };

                // Map line items from domain request to generated request format
                if (request.LineItems?.Count > 0)
                {
                    postRequestBody.AdjustmentLineItems = request.LineItems.Select(lineItem => 
                        new Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostRequestBody_adjustment_line_items
                        {
                            Amount = (double)lineItem.Amount,
                            Description = lineItem.Reason,
                            Type = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Budget_changesPostRequestBody_adjustment_line_items_type.Budget_change,
                            AdjustmentNumber = 1 // Default adjustment number
                        }).ToList();
                }

                // Call the generated client
                var postResponse = await _generatedClient.Rest.V10.Projects[projectId].Budget_changes
                    .PostAsync(postRequestBody, cancellationToken: cancellationToken);
                
                if (postResponse?.Data == null)
                {
                    throw new CoreModels.ProcoreCoreException(
                        $"Failed to create budget change for project {projectId} in company {companyId}",
                        "BUDGET_CHANGE_CREATE_FAILED",
                        null);
                }

                // Map the response back to our domain model
                return new BudgetChange
                {
                    Id = postResponse.Data.Id ?? 0,
                    ProjectId = projectId,
                    Description = postResponse.Data.Description ?? string.Empty,
                    Amount = request.Amount, // Use the original request amount since POST response doesn't include amount
                    Status = MapGeneratedBudgetChangePostStatusToWrapper(postResponse.Data.Status),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0, // POST response doesn't include created_by info
                    LineItems = request.LineItems ?? new List<BudgetLineItemChange>()
                };
            },
            $"CreateBudgetChange-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets all budget changes for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of budget changes.</returns>
    public async Task<IEnumerable<ProjectModels.BudgetChange>> GetBudgetChangesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting budget changes for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Call the generated client
                var budgetChangesResponse = await _generatedClient.Rest.V10.Projects[projectId].Budget_changes
                    .GetAsync(cancellationToken: cancellationToken);
                
                if (budgetChangesResponse?.Data == null || budgetChangesResponse.Data.Count == 0)
                {
                    _logger?.LogWarning("No budget changes returned for project {ProjectId} in company {CompanyId}", projectId, companyId);
                    return Enumerable.Empty<ProjectModels.BudgetChange>();
                }

                // Map from generated response to domain models
                return budgetChangesResponse.Data.Select(data => new BudgetChange
                {
                    Id = data.Id ?? 0,
                    ProjectId = projectId,
                    Description = data.Description ?? string.Empty,
                    Amount = (decimal)(data.Amount ?? 0.0),
                    Status = MapGeneratedBudgetChangeStatusToWrapper(data.Status),
                    CreatedAt = DateTime.UtcNow, // Generated model doesn't include created_at in GET response
                    CreatedBy = (int)(data.CreatedBy?.Id ?? 0),
                    LineItems = new List<BudgetLineItemChange>() // Line items would need additional API calls
                });
            },
            $"GetBudgetChanges-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Contract Operations

    /// <summary>
    /// Gets all commitment contracts for a project.
    /// Note: V1.0 API does not provide commitment contract endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of commitment contracts.</returns>
    public async Task<IEnumerable<ProjectModels.CommitmentContract>> GetCommitmentContractsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting commitment contracts for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Note: V1.0 API does not provide commitment contract endpoints
                // Return empty collection for now - would require V2.0 API implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<CommitmentContract>();
            },
            $"GetCommitmentContracts-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific commitment contract by ID.
    /// Note: V1.0 API does not provide commitment contract endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="contractId">The contract ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The commitment contract.</returns>
    /// <exception cref="NotImplementedException">Thrown because commitment contracts are not supported in V1.0 API.</exception>
    public Task<ProjectModels.CommitmentContract> GetCommitmentContractAsync(int companyId, int projectId, int contractId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to get commitment contract {ContractId} for project {ProjectId} in company {CompanyId}", contractId, projectId, companyId);
        
        // Note: V1.0 API does not provide commitment contract endpoints
        return Task.FromException<ProjectModels.CommitmentContract>(new NotImplementedException(
            "Commitment contract retrieval is not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for commitment contract functionality."));
    }

    /// <summary>
    /// Creates a new change order.
    /// Note: V1.0 API does not provide change order creation endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The change order creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created change order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="NotImplementedException">Thrown because change order creation is not supported in V1.0 API.</exception>
    public Task<ProjectModels.ChangeOrder> CreateChangeOrderAsync(int companyId, int projectId, ProjectModels.CreateChangeOrderRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger?.LogDebug("Attempting to create change order for project {ProjectId} in company {CompanyId}", projectId, companyId);
        
        // Note: V1.0 API does not provide change order creation endpoints
        return Task.FromException<ProjectModels.ChangeOrder>(new NotImplementedException(
            "Change order creation is not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for change order functionality."));
    }

    /// <summary>
    /// Gets all change orders for a project.
    /// Note: V1.0 API does not provide change order endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of change orders.</returns>
    public async Task<IEnumerable<ProjectModels.ChangeOrder>> GetChangeOrdersAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting change orders for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Note: V1.0 API does not provide change order endpoints
                // Return empty collection for now - would require V2.0 API implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<ChangeOrder>();
            },
            $"GetChangeOrders-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Workflow Operations

    /// <summary>
    /// Gets all workflow instances for a project.
    /// Note: V1.0 API does not provide workflow endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of workflow instances.</returns>
    public async Task<IEnumerable<ProjectModels.WorkflowInstance>> GetWorkflowInstancesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting workflow instances for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Note: V1.0 API does not provide workflow endpoints
                // Return empty collection for now - would require V2.0 API implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<WorkflowInstance>();
            },
            $"GetWorkflowInstances-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific workflow instance by ID.
    /// Note: V1.0 API does not provide workflow endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="workflowId">The workflow instance ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The workflow instance.</returns>
    /// <exception cref="NotImplementedException">Thrown because workflow instances are not supported in V1.0 API.</exception>
    public Task<ProjectModels.WorkflowInstance> GetWorkflowInstanceAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to get workflow instance {WorkflowId} for project {ProjectId} in company {CompanyId}", workflowId, projectId, companyId);
        
        // Note: V1.0 API does not provide workflow endpoints
        return Task.FromException<ProjectModels.WorkflowInstance>(new NotImplementedException(
            "Workflow instance retrieval is not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for workflow functionality."));
    }

    /// <summary>
    /// Restarts a workflow instance.
    /// Note: V1.0 API does not provide workflow endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="workflowId">The workflow instance ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotImplementedException">Thrown because workflow operations are not supported in V1.0 API.</exception>
    public Task RestartWorkflowAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to restart workflow instance {WorkflowId} for project {ProjectId} in company {CompanyId}", workflowId, projectId, companyId);
        
        // Note: V1.0 API does not provide workflow endpoints
        return Task.FromException(new NotImplementedException(
            "Workflow operations are not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for workflow functionality."));
    }

    /// <summary>
    /// Terminates a workflow instance.
    /// Note: V1.0 API does not provide workflow endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="workflowId">The workflow instance ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotImplementedException">Thrown because workflow operations are not supported in V1.0 API.</exception>
    public Task TerminateWorkflowAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to terminate workflow instance {WorkflowId} for project {ProjectId} in company {CompanyId}", workflowId, projectId, companyId);
        
        // Note: V1.0 API does not provide workflow endpoints
        return Task.FromException(new NotImplementedException(
            "Workflow operations are not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for workflow functionality."));
    }

    #endregion

    #region Meeting Operations

    /// <summary>
    /// Gets all meetings for a project.
    /// Note: V1.0 API does not provide meeting endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of meetings.</returns>
    public async Task<IEnumerable<ProjectModels.Meeting>> GetMeetingsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting meetings for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Note: V1.0 API does not provide meeting endpoints
                // Return empty collection for now - would require V2.0 API implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<Meeting>();
            },
            $"GetMeetings-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific meeting by ID.
    /// Note: V1.0 API does not provide meeting endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="meetingId">The meeting ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The meeting.</returns>
    /// <exception cref="NotImplementedException">Thrown because meeting operations are not supported in V1.0 API.</exception>
    public Task<ProjectModels.Meeting> GetMeetingAsync(int companyId, int projectId, int meetingId, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Attempting to get meeting {MeetingId} for project {ProjectId} in company {CompanyId}", meetingId, projectId, companyId);
        
        // Note: V1.0 API does not provide meeting endpoints
        return Task.FromException<ProjectModels.Meeting>(new NotImplementedException(
            "Meeting operations are not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for meeting functionality."));
    }

    /// <summary>
    /// Creates a new meeting.
    /// Note: V1.0 API does not provide meeting endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The meeting creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created meeting.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="NotImplementedException">Thrown because meeting operations are not supported in V1.0 API.</exception>
    public Task<ProjectModels.Meeting> CreateMeetingAsync(int companyId, int projectId, ProjectModels.CreateMeetingRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger?.LogDebug("Attempting to create meeting for project {ProjectId} in company {CompanyId}", projectId, companyId);
        
        // Note: V1.0 API does not provide meeting endpoints
        return Task.FromException<ProjectModels.Meeting>(new NotImplementedException(
            "Meeting creation is not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for meeting functionality."));
    }

    /// <summary>
    /// Updates an existing meeting.
    /// Note: V1.0 API does not provide meeting endpoints.
    /// This method is included for interface completeness but requires V2.0 API for functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="meetingId">The meeting ID.</param>
    /// <param name="request">The meeting update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated meeting.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="NotImplementedException">Thrown because meeting operations are not supported in V1.0 API.</exception>
    public Task<ProjectModels.Meeting> UpdateMeetingAsync(int companyId, int projectId, int meetingId, ProjectModels.CreateMeetingRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger?.LogDebug("Attempting to update meeting {MeetingId} for project {ProjectId} in company {CompanyId}", meetingId, projectId, companyId);
        
        // Note: V1.0 API does not provide meeting endpoints
        return Task.FromException<ProjectModels.Meeting>(new NotImplementedException(
            "Meeting updates are not supported in the V1.0 API. " +
            "Please upgrade to V2.0 API for meeting functionality."));
    }

    #endregion

    #region Convenience Methods

    /// <summary>
    /// Gets all active projects for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of active projects.</returns>
    public async Task<IEnumerable<ProjectModels.Project>> GetActiveProjectsAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting active projects for company {CompanyId}", companyId);
                
                // Use the existing GetProjectsAsync method and filter for active projects
                var allProjects = await GetProjectsAsync(companyId, cancellationToken);
                
                return allProjects.Where(p => p.IsActive && p.Status == ProjectStatus.Active);
            },
            $"GetActiveProjects-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a project by name.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectName">The project name.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The project.</returns>
    public async Task<ProjectModels.Project> GetProjectByNameAsync(int companyId, string projectName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(projectName))
        {
            throw new ArgumentException("Project name cannot be null or empty", nameof(projectName));
        }
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting project by name {ProjectName} for company {CompanyId}", projectName, companyId);
                
                // Use the existing GetProjectsAsync method and find by name
                var allProjects = await GetProjectsAsync(companyId, cancellationToken);
                
                var matchingProject = allProjects.FirstOrDefault(p => 
                    string.Equals(p.Name, projectName, StringComparison.OrdinalIgnoreCase));
                
                if (matchingProject == null)
                {
                    throw new CoreModels.ProcoreCoreException(
                        $"Project with name '{projectName}' not found in company {companyId}",
                        "PROJECT_NOT_FOUND",
                        null);
                }
                
                return matchingProject;
            },
            $"GetProjectByName-{projectName}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets the total budget amount for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The total budget amount.</returns>
    public async Task<decimal> GetProjectBudgetTotalAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting budget total for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Get the project first to see if it has a budget amount
                var project = await GetProjectAsync(companyId, projectId, cancellationToken);
                
                if (project.Budget.HasValue)
                {
                    return project.Budget.Value;
                }

                // If no budget in project, try to get from budget changes
                var budgetChanges = await GetBudgetChangesAsync(companyId, projectId, cancellationToken);
                
                return budgetChanges.Where(bc => bc.Status == BudgetChangeStatus.Approved)
                                  .Sum(bc => bc.Amount);
            },
            $"GetProjectBudgetTotal-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets budget variances that exceed a threshold percentage.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="thresholdPercentage">The variance threshold percentage.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of budget variances.</returns>
    public async Task<IEnumerable<ProjectModels.BudgetVariance>> GetBudgetVariancesAsync(int companyId, int projectId, decimal thresholdPercentage, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting budget variances for project {ProjectId} in company {CompanyId} with threshold {Threshold}%", projectId, companyId, thresholdPercentage);
                
                // Get budget line items (limited info available from V1.0)
                var budgetLineItems = await GetBudgetLineItemsAsync(companyId, projectId, cancellationToken);
                
                var variances = new List<BudgetVariance>();
                
                foreach (var lineItem in budgetLineItems)
                {
                    if (lineItem.BudgetAmount > 0)
                    {
                        var varianceAmount = lineItem.VarianceAmount;
                        var variancePercentage = Math.Abs((varianceAmount / lineItem.BudgetAmount) * 100);
                        
                        if (variancePercentage >= thresholdPercentage)
                        {
                            variances.Add(new BudgetVariance
                            {
                                BudgetLineItemId = lineItem.Id,
                                WbsCode = lineItem.WbsCode,
                                Description = lineItem.Description,
                                BudgetAmount = lineItem.BudgetAmount,
                                ActualAmount = lineItem.ActualAmount,
                                VarianceAmount = varianceAmount,
                                VariancePercentage = variancePercentage,
                                ProjectId = projectId
                            });
                        }
                    }
                }
                
                return variances.AsEnumerable();
            },
            $"GetBudgetVariances-Project-{projectId}-Company-{companyId}-Threshold-{thresholdPercentage}",
            null,
            cancellationToken);
    }

    #endregion

    #region Pagination Support

    /// <summary>
    /// Gets projects with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of projects.</returns>
    public async Task<CoreModels.PagedResult<ProjectModels.Project>> GetProjectsPagedAsync(int companyId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting projects with pagination for company {CompanyId} (page {Page}, per page {PerPage})", companyId, options.Page, options.PerPage);
                
                // Get all projects first (since V1.0 API doesn't support server-side pagination)
                var allProjects = await GetProjectsAsync(companyId, cancellationToken);
                var projectsList = allProjects.ToList();
                
                var totalCount = projectsList.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / options.PerPage);
                
                // Apply client-side pagination
                var skip = (options.Page - 1) * options.PerPage;
                var pagedItems = projectsList.Skip(skip).Take(options.PerPage);
                
                return new CoreModels.PagedResult<ProjectModels.Project>
                {
                    Items = pagedItems,
                    TotalCount = totalCount,
                    Page = options.Page,
                    PerPage = options.PerPage,
                    TotalPages = totalPages,
                    HasNextPage = options.Page < totalPages,
                    HasPreviousPage = options.Page > 1
                };
            },
            $"GetProjectsPaged-Company-{companyId}-Page-{options.Page}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets budget line items with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of budget line items.</returns>
    public async Task<CoreModels.PagedResult<ProjectModels.BudgetLineItem>> GetBudgetLineItemsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting budget line items with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
                
                // Get all budget line items first
                var allBudgetLineItems = await GetBudgetLineItemsAsync(companyId, projectId, cancellationToken);
                var budgetLineItemsList = allBudgetLineItems.ToList();
                
                var totalCount = budgetLineItemsList.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / options.PerPage);
                
                // Apply client-side pagination
                var skip = (options.Page - 1) * options.PerPage;
                var pagedItems = budgetLineItemsList.Skip(skip).Take(options.PerPage);
                
                return new CoreModels.PagedResult<ProjectModels.BudgetLineItem>
                {
                    Items = pagedItems,
                    TotalCount = totalCount,
                    Page = options.Page,
                    PerPage = options.PerPage,
                    TotalPages = totalPages,
                    HasNextPage = options.Page < totalPages,
                    HasPreviousPage = options.Page > 1
                };
            },
            $"GetBudgetLineItemsPaged-Project-{projectId}-Company-{companyId}-Page-{options.Page}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets commitment contracts with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of commitment contracts.</returns>
    public async Task<CoreModels.PagedResult<ProjectModels.CommitmentContract>> GetCommitmentContractsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting commitment contracts with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
                
                // Get all commitment contracts first
                var allCommitmentContracts = await GetCommitmentContractsAsync(companyId, projectId, cancellationToken);
                var commitmentContractsList = allCommitmentContracts.ToList();
                
                var totalCount = commitmentContractsList.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / options.PerPage);
                
                // Apply client-side pagination
                var skip = (options.Page - 1) * options.PerPage;
                var pagedItems = commitmentContractsList.Skip(skip).Take(options.PerPage);
                
                return new CoreModels.PagedResult<ProjectModels.CommitmentContract>
                {
                    Items = pagedItems,
                    TotalCount = totalCount,
                    Page = options.Page,
                    PerPage = options.PerPage,
                    TotalPages = totalPages,
                    HasNextPage = options.Page < totalPages,
                    HasPreviousPage = options.Page > 1
                };
            },
            $"GetCommitmentContractsPaged-Project-{projectId}-Company-{companyId}-Page-{options.Page}",
            null,
            cancellationToken);
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Disposes of the ProcoreProjectManagementClient and its resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the ProcoreProjectManagementClient and its resources.
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