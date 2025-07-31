using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.ProjectManagement.Models;

/// <summary>
/// Defines the contract for the ProjectManagement client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
/// </summary>
public interface IProjectManagementClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }

    // Project Operations
    /// <summary>
    /// Gets all projects for the specified company.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of projects for the company.</returns>
    Task<IEnumerable<Project>> GetProjectsAsync(int companyId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific project by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested project.</returns>
    Task<Project> GetProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates a new project in the specified company.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="request">The project creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created project.</returns>
    Task<Project> CreateProjectAsync(int companyId, CreateProjectRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates an existing project with new information.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="request">The project update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated project.</returns>
    Task<Project> UpdateProjectAsync(int companyId, int projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes a project from the specified company.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Budget Operations
    /// <summary>
    /// Gets all budget line items for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of budget line items for the project.</returns>
    Task<IEnumerable<BudgetLineItem>> GetBudgetLineItemsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific budget line item by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="lineItemId">The budget line item identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested budget line item.</returns>
    Task<BudgetLineItem> GetBudgetLineItemAsync(int companyId, int projectId, int lineItemId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates a new budget change for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="request">The budget change creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created budget change.</returns>
    Task<BudgetChange> CreateBudgetChangeAsync(int companyId, int projectId, CreateBudgetChangeRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets all budget changes for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of budget changes for the project.</returns>
    Task<IEnumerable<BudgetChange>> GetBudgetChangesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Contract Operations
    /// <summary>
    /// Gets all commitment contracts for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of commitment contracts for the project.</returns>
    Task<IEnumerable<CommitmentContract>> GetCommitmentContractsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific commitment contract by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested commitment contract.</returns>
    Task<CommitmentContract> GetCommitmentContractAsync(int companyId, int projectId, int contractId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates a new change order for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="request">The change order creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created change order.</returns>
    Task<ChangeOrder> CreateChangeOrderAsync(int companyId, int projectId, CreateChangeOrderRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets all change orders for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of change orders for the project.</returns>
    Task<IEnumerable<ChangeOrder>> GetChangeOrdersAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Workflow Operations
    /// <summary>
    /// Gets all workflow instances for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of workflow instances for the project.</returns>
    Task<IEnumerable<WorkflowInstance>> GetWorkflowInstancesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific workflow instance by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested workflow instance.</returns>
    Task<WorkflowInstance> GetWorkflowInstanceAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Restarts a workflow instance.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous restart operation.</returns>
    Task RestartWorkflowAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Terminates a workflow instance.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous terminate operation.</returns>
    Task TerminateWorkflowAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default);

    // Meeting Operations
    /// <summary>
    /// Gets all meetings for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of meetings for the project.</returns>
    Task<IEnumerable<Meeting>> GetMeetingsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific meeting by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="meetingId">The meeting identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested meeting.</returns>
    Task<Meeting> GetMeetingAsync(int companyId, int projectId, int meetingId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates a new meeting for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="request">The meeting creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created meeting.</returns>
    Task<Meeting> CreateMeetingAsync(int companyId, int projectId, CreateMeetingRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates an existing meeting with new information.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="meetingId">The meeting identifier.</param>
    /// <param name="request">The meeting update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated meeting.</returns>
    Task<Meeting> UpdateMeetingAsync(int companyId, int projectId, int meetingId, CreateMeetingRequest request, CancellationToken cancellationToken = default);

    // RFI Operations - Not implemented in V1.0 API
    // Future expansion: RFI operations would be available in V2.0 API
    // Task<IEnumerable<Rfi>> GetRfisAsync(int projectId, CancellationToken cancellationToken = default);
    // Task<Rfi> GetRfiAsync(int projectId, int rfiId, CancellationToken cancellationToken = default);
    // Task<Rfi> CreateRfiAsync(int projectId, CreateRfiRequest request, CancellationToken cancellationToken = default);
    // Task<Rfi> UpdateRfiAsync(int projectId, int rfiId, UpdateRfiRequest request, CancellationToken cancellationToken = default);
    // Task DeleteRfiAsync(int projectId, int rfiId, CancellationToken cancellationToken = default);

    // Drawing Management Operations - Not implemented in V1.0 API
    // Future expansion: Drawing operations would be available in V2.0 API
    // Task<IEnumerable<Drawing>> GetDrawingsAsync(int projectId, CancellationToken cancellationToken = default);
    // Task<Drawing> GetDrawingAsync(int projectId, int drawingId, CancellationToken cancellationToken = default);

    // Convenience Methods
    /// <summary>
    /// Gets all active projects for the specified company.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of active projects for the company.</returns>
    Task<IEnumerable<Project>> GetActiveProjectsAsync(int companyId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a project by its name within the specified company.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectName">The project name to search for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The project with the specified name, or null if not found.</returns>
    Task<Project> GetProjectByNameAsync(int companyId, string projectName, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the total budget amount for the specified project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The total budget amount for the project.</returns>
    Task<decimal> GetProjectBudgetTotalAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets budget variances that exceed the specified threshold percentage.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="thresholdPercentage">The variance threshold percentage (e.g., 0.1 for 10%).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of budget variances exceeding the threshold.</returns>
    Task<IEnumerable<BudgetVariance>> GetBudgetVariancesAsync(int companyId, int projectId, decimal thresholdPercentage, CancellationToken cancellationToken = default);

    // Pagination Support
    /// <summary>
    /// Gets projects for the specified company with pagination support.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="options">The pagination options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged result containing projects for the company.</returns>
    Task<CoreModels.PagedResult<Project>> GetProjectsPagedAsync(int companyId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets budget line items for the specified project with pagination support.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="options">The pagination options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged result containing budget line items for the project.</returns>
    Task<CoreModels.PagedResult<BudgetLineItem>> GetBudgetLineItemsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets commitment contracts for the specified project with pagination support.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="options">The pagination options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged result containing commitment contracts for the project.</returns>
    Task<CoreModels.PagedResult<CommitmentContract>> GetCommitmentContractsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
}