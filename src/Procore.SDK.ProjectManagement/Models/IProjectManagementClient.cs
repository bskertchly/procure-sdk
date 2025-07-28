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
    Task<IEnumerable<Project>> GetProjectsAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Project> GetProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<Project> CreateProjectAsync(int companyId, CreateProjectRequest request, CancellationToken cancellationToken = default);
    Task<Project> UpdateProjectAsync(int companyId, int projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default);
    Task DeleteProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Budget Operations
    Task<IEnumerable<BudgetLineItem>> GetBudgetLineItemsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<BudgetLineItem> GetBudgetLineItemAsync(int companyId, int projectId, int lineItemId, CancellationToken cancellationToken = default);
    Task<BudgetChange> CreateBudgetChangeAsync(int companyId, int projectId, CreateBudgetChangeRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<BudgetChange>> GetBudgetChangesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Contract Operations
    Task<IEnumerable<CommitmentContract>> GetCommitmentContractsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<CommitmentContract> GetCommitmentContractAsync(int companyId, int projectId, int contractId, CancellationToken cancellationToken = default);
    Task<ChangeOrder> CreateChangeOrderAsync(int companyId, int projectId, CreateChangeOrderRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChangeOrder>> GetChangeOrdersAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Workflow Operations
    Task<IEnumerable<WorkflowInstance>> GetWorkflowInstancesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<WorkflowInstance> GetWorkflowInstanceAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default);
    Task RestartWorkflowAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default);
    Task TerminateWorkflowAsync(int companyId, int projectId, int workflowId, CancellationToken cancellationToken = default);

    // Meeting Operations
    Task<IEnumerable<Meeting>> GetMeetingsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<Meeting> GetMeetingAsync(int companyId, int projectId, int meetingId, CancellationToken cancellationToken = default);
    Task<Meeting> CreateMeetingAsync(int companyId, int projectId, CreateMeetingRequest request, CancellationToken cancellationToken = default);
    Task<Meeting> UpdateMeetingAsync(int companyId, int projectId, int meetingId, CreateMeetingRequest request, CancellationToken cancellationToken = default);

    // RFI Operations - TODO: Implement when RFI models are available
    // Task<IEnumerable<Rfi>> GetRfisAsync(int projectId, CancellationToken cancellationToken = default);
    // Task<Rfi> GetRfiAsync(int projectId, int rfiId, CancellationToken cancellationToken = default);
    // Task<Rfi> CreateRfiAsync(int projectId, CreateRfiRequest request, CancellationToken cancellationToken = default);
    // Task<Rfi> UpdateRfiAsync(int projectId, int rfiId, UpdateRfiRequest request, CancellationToken cancellationToken = default);
    // Task DeleteRfiAsync(int projectId, int rfiId, CancellationToken cancellationToken = default);

    // Drawing Management Operations - TODO: Implement when Drawing models are available
    // Task<IEnumerable<Drawing>> GetDrawingsAsync(int projectId, CancellationToken cancellationToken = default);
    // Task<Drawing> GetDrawingAsync(int projectId, int drawingId, CancellationToken cancellationToken = default);

    // Convenience Methods
    Task<IEnumerable<Project>> GetActiveProjectsAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Project> GetProjectByNameAsync(int companyId, string projectName, CancellationToken cancellationToken = default);
    Task<decimal> GetProjectBudgetTotalAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BudgetVariance>> GetBudgetVariancesAsync(int companyId, int projectId, decimal thresholdPercentage, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<CoreModels.PagedResult<Project>> GetProjectsPagedAsync(int companyId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    Task<CoreModels.PagedResult<BudgetLineItem>> GetBudgetLineItemsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    Task<CoreModels.PagedResult<CommitmentContract>> GetCommitmentContractsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
}