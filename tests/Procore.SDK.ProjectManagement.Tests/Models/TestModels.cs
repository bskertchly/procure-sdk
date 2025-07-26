using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.ProjectManagement.Tests.Models;

/// <summary>
/// Test domain models for ProjectManagement client testing
/// </summary>

// Core Project Models
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int CompanyId { get; set; }
    public decimal? Budget { get; set; }
    public string ProjectType { get; set; } = string.Empty;
    public ProjectPhase Phase { get; set; }
}

public enum ProjectStatus
{
    Planning,
    Active,
    OnHold,
    Completed,
    Cancelled
}

public enum ProjectPhase
{
    PreConstruction,
    Construction,
    PostConstruction,
    Closeout
}

// Budget Models
public class BudgetLineItem
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string WbsCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal VarianceAmount { get; set; }
    public string CostCode { get; set; } = string.Empty;
}

public class BudgetChange
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public BudgetChangeStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
}

public enum BudgetChangeStatus
{
    Draft,
    Pending,
    Approved,
    Rejected
}

// Contract Models
public class CommitmentContract
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public decimal ContractAmount { get; set; }
    public ContractStatus Status { get; set; }
    public DateTime? ExecutedDate { get; set; }
    public int VendorId { get; set; }
}

public class ChangeOrder
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int? ContractId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ChangeOrderStatus Status { get; set; }
    public ChangeOrderType Type { get; set; }
}

public enum ContractStatus
{
    Draft,
    Pending,
    Executed,
    Complete,
    Terminated
}

public enum ChangeOrderStatus
{
    Draft,
    Pending,
    Approved,
    Rejected,
    Void
}

public enum ChangeOrderType
{
    Prime,
    Commitment
}

// Workflow Models
public class WorkflowInstance
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public WorkflowStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int? AssignedTo { get; set; }
}

public enum WorkflowStatus
{
    Active,
    Completed,
    Terminated,
    OnHold
}

// Meeting Models
public class Meeting
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public MeetingStatus Status { get; set; }
    public List<int> AttendeeIds { get; set; } = new();
}

public enum MeetingStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled
}

// Request Models
public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string ProjectType { get; set; } = string.Empty;
}

public class UpdateProjectRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ProjectStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
}

public class CreateBudgetChangeRequest
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public List<BudgetLineItemChange> LineItems { get; set; } = new();
}

public class BudgetLineItemChange
{
    public int LineItemId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class CreateChangeOrderRequest
{
    public string Title { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ChangeOrderType Type { get; set; }
    public int? ContractId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class CreateMeetingRequest
{
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public List<int> AttendeeIds { get; set; } = new();
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
/// Defines the contract for the ProjectManagement client wrapper
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
    Task<WorkflowInstance> GetWorkflowInstanceAsync(int companyId, int projectId, int instanceId, CancellationToken cancellationToken = default);
    Task<WorkflowInstance> RestartWorkflowAsync(int companyId, int projectId, int instanceId, CancellationToken cancellationToken = default);
    Task TerminateWorkflowAsync(int companyId, int projectId, int instanceId, CancellationToken cancellationToken = default);

    // Meeting Operations
    Task<IEnumerable<Meeting>> GetMeetingsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<Meeting> GetMeetingAsync(int companyId, int projectId, int meetingId, CancellationToken cancellationToken = default);
    Task<Meeting> CreateMeetingAsync(int companyId, int projectId, CreateMeetingRequest request, CancellationToken cancellationToken = default);
    Task<Meeting> UpdateMeetingAsync(int companyId, int projectId, int meetingId, CreateMeetingRequest request, CancellationToken cancellationToken = default);

    // Convenience Methods
    Task<IEnumerable<Project>> GetActiveProjectsAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Project> GetProjectByNameAsync(int companyId, string projectName, CancellationToken cancellationToken = default);
    Task<decimal> GetProjectBudgetTotalAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BudgetLineItem>> GetBudgetVariancesAsync(int companyId, int projectId, decimal thresholdAmount, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<PagedResult<Project>> GetProjectsPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<BudgetLineItem>> GetBudgetLineItemsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<CommitmentContract>> GetCommitmentContractsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
}