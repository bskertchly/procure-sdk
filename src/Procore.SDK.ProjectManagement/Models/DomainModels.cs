using System;
using System.Collections.Generic;

namespace Procore.SDK.ProjectManagement.Models;

/// <summary>
/// Domain models for ProjectManagement client
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
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public List<BudgetLineItemChange> LineItems { get; set; } = new();
}

public class BudgetVariance
{
    public int LineItemId { get; set; }
    public string WbsCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal VarianceAmount { get; set; }
    public decimal VariancePercentage { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum MeetingStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled
}