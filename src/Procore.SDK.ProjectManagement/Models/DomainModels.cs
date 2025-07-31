using System;
using System.Collections.Generic;

namespace Procore.SDK.ProjectManagement.Models;

/// <summary>
/// Domain models for ProjectManagement client.
/// </summary>

// Core Project Models

/// <summary>
/// Represents a construction project in the Procore system.
/// </summary>
public class Project
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the current status of the project.
    /// </summary>
    public ProjectStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the planned start date of the project.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the planned end date of the project.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the company that owns this project.
    /// </summary>
    public int CompanyId { get; set; }
    
    /// <summary>
    /// Gets or sets the total budget allocated for the project.
    /// </summary>
    public decimal? Budget { get; set; }
    
    /// <summary>
    /// Gets or sets the type or category of the project.
    /// </summary>
    public string ProjectType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the current phase of the project lifecycle.
    /// </summary>
    public ProjectPhase Phase { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the project is currently active.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the project was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the project was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents the current status of a project.
/// </summary>
public enum ProjectStatus
{
    /// <summary>
    /// Project is in the planning phase.
    /// </summary>
    Planning,
    
    /// <summary>
    /// Project is currently active and ongoing.
    /// </summary>
    Active,
    
    /// <summary>
    /// Project is temporarily on hold.
    /// </summary>
    OnHold,
    
    /// <summary>
    /// Project has been completed successfully.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Project has been cancelled.
    /// </summary>
    Cancelled
}

/// <summary>
/// Represents the current phase of a project lifecycle.
/// </summary>
public enum ProjectPhase
{
    /// <summary>
    /// Pre-construction phase including planning and design.
    /// </summary>
    PreConstruction,
    
    /// <summary>
    /// Active construction phase.
    /// </summary>
    Construction,
    
    /// <summary>
    /// Post-construction phase including final inspections.
    /// </summary>
    PostConstruction,
    
    /// <summary>
    /// Project closeout phase including final documentation.
    /// </summary>
    Closeout
}

// Budget Models

/// <summary>
/// Represents a budget line item within a project's budget.
/// </summary>
public class BudgetLineItem
{
    /// <summary>
    /// Gets or sets the unique identifier for the budget line item.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this budget line item belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the Work Breakdown Structure (WBS) code for this line item.
    /// </summary>
    public string WbsCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the budget line item.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the budgeted amount for this line item.
    /// </summary>
    public decimal BudgetAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the actual amount spent for this line item.
    /// </summary>
    public decimal ActualAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the variance amount (actual - budget) for this line item.
    /// </summary>
    public decimal VarianceAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the cost code associated with this budget line item.
    /// </summary>
    public string CostCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date and time when this budget line item was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this budget line item was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a budget change request within a project.
/// </summary>
public class BudgetChange
{
    /// <summary>
    /// Gets or sets the unique identifier for the budget change.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this budget change belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the budget change.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total amount of the budget change.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the budget change.
    /// </summary>
    public BudgetChangeStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this budget change was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user who created this budget change.
    /// </summary>
    public int CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the list of individual line item changes that make up this budget change.
    /// </summary>
    public List<BudgetLineItemChange> LineItems { get; set; } = new();
}

/// <summary>
/// Represents a budget variance analysis for a specific budget line item.
/// </summary>
public class BudgetVariance
{
    /// <summary>
    /// Gets or sets the ID of the budget line item this variance relates to.
    /// </summary>
    public int BudgetLineItemId { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this variance belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the Work Breakdown Structure (WBS) code for this variance.
    /// </summary>
    public string WbsCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the budget line item with variance.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the originally budgeted amount.
    /// </summary>
    public decimal BudgetAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the actual amount spent.
    /// </summary>
    public decimal ActualAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the variance amount (actual - budget).
    /// </summary>
    public decimal VarianceAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the variance percentage ((actual - budget) / budget * 100).
    /// </summary>
    public decimal VariancePercentage { get; set; }
}

/// <summary>
/// Represents the approval status of a budget change.
/// </summary>
public enum BudgetChangeStatus
{
    /// <summary>
    /// Budget change is in draft status and not yet submitted for review.
    /// </summary>
    Draft,
    
    /// <summary>
    /// Budget change has been approved and is in effect.
    /// </summary>
    Approved,
    
    /// <summary>
    /// Budget change is currently under review for approval.
    /// </summary>
    UnderReview,
    
    /// <summary>
    /// Budget change has been voided and is no longer valid.
    /// </summary>
    Void
}

// Contract Models

/// <summary>
/// Represents a commitment contract between the project owner and a vendor/subcontractor.
/// </summary>
public class CommitmentContract
{
    /// <summary>
    /// Gets or sets the unique identifier for the commitment contract.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this contract belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the title or name of the commitment contract.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the contract number or reference identifier.
    /// </summary>
    public string ContractNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total contract amount.
    /// </summary>
    public decimal ContractAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the contract.
    /// </summary>
    public ContractStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the contract was executed/signed.
    /// </summary>
    public DateTime? ExecutedDate { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the vendor/subcontractor for this contract.
    /// </summary>
    public int VendorId { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this contract was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this contract was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a change order that modifies the scope, cost, or timeline of a contract.
/// </summary>
public class ChangeOrder
{
    /// <summary>
    /// Gets or sets the unique identifier for the change order.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this change order belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the contract this change order modifies, if applicable.
    /// </summary>
    public int? ContractId { get; set; }
    
    /// <summary>
    /// Gets or sets the title or name of the change order.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the change order number or reference identifier.
    /// </summary>
    public string Number { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the financial impact amount of the change order.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the current approval status of the change order.
    /// </summary>
    public ChangeOrderStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the type of change order (Prime or Commitment).
    /// </summary>
    public ChangeOrderType Type { get; set; }
    
    /// <summary>
    /// Gets or sets the detailed description of the change order.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date and time when this change order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this change order was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents the current status of a commitment contract.
/// </summary>
public enum ContractStatus
{
    /// <summary>
    /// Contract is in draft status and not yet finalized.
    /// </summary>
    Draft,
    
    /// <summary>
    /// Contract is pending approval or execution.
    /// </summary>
    Pending,
    
    /// <summary>
    /// Contract has been executed/signed by all parties.
    /// </summary>
    Executed,
    
    /// <summary>
    /// Contract work has been completed successfully.
    /// </summary>
    Complete,
    
    /// <summary>
    /// Contract has been terminated before completion.
    /// </summary>
    Terminated
}

/// <summary>
/// Represents the approval status of a change order.
/// </summary>
public enum ChangeOrderStatus
{
    /// <summary>
    /// Change order is in draft status and not yet submitted.
    /// </summary>
    Draft,
    
    /// <summary>
    /// Change order is pending approval.
    /// </summary>
    Pending,
    
    /// <summary>
    /// Change order has been approved and is in effect.
    /// </summary>
    Approved,
    
    /// <summary>
    /// Change order has been rejected.
    /// </summary>
    Rejected,
    
    /// <summary>
    /// Change order has been voided and is no longer valid.
    /// </summary>
    Void
}

/// <summary>
/// Represents the type of change order based on contract relationship.
/// </summary>
public enum ChangeOrderType
{
    /// <summary>
    /// Change order for the prime contract (between owner and main contractor).
    /// </summary>
    Prime,
    
    /// <summary>
    /// Change order for a commitment contract (between main contractor and subcontractor).
    /// </summary>
    Commitment
}

// Workflow Models

/// <summary>
/// Represents an instance of a workflow process within a project.
/// </summary>
public class WorkflowInstance
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow instance.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this workflow instance belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the title or name of the workflow instance.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the current status of the workflow instance.
    /// </summary>
    public WorkflowStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this workflow instance was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user who created this workflow instance.
    /// </summary>
    public int CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user currently assigned to this workflow, if any.
    /// </summary>
    public int? AssignedTo { get; set; }
}

/// <summary>
/// Represents the current status of a workflow instance.
/// </summary>
public enum WorkflowStatus
{
    /// <summary>
    /// Workflow is currently active and in progress.
    /// </summary>
    Active,
    
    /// <summary>
    /// Workflow has been completed successfully.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Workflow has been terminated before completion.
    /// </summary>
    Terminated,
    
    /// <summary>
    /// Workflow is temporarily on hold.
    /// </summary>
    OnHold
}

// Meeting Models

/// <summary>
/// Represents a project meeting or conference.
/// </summary>
public class Meeting
{
    /// <summary>
    /// Gets or sets the unique identifier for the meeting.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the project this meeting belongs to.
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the title or name of the meeting.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the scheduled date and time for the meeting.
    /// </summary>
    public DateTime ScheduledDate { get; set; }
    
    /// <summary>
    /// Gets or sets the location where the meeting will be held.
    /// </summary>
    public string Location { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the current status of the meeting.
    /// </summary>
    public MeetingStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the list of user IDs for meeting attendees.
    /// </summary>
    public List<int> AttendeeIds { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the date and time when this meeting was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when this meeting was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents the current status of a meeting.
/// </summary>
public enum MeetingStatus
{
    /// <summary>
    /// Meeting is scheduled but has not yet started.
    /// </summary>
    Scheduled,
    
    /// <summary>
    /// Meeting is currently in progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Meeting has been completed.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Meeting has been cancelled.
    /// </summary>
    Cancelled
}