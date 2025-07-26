using System;
using System.Collections.Generic;

namespace Procore.SDK.ProjectManagement.Models;

/// <summary>
/// Request models for ProjectManagement client operations
/// </summary>

// Project Request Models
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

// Budget Request Models
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

// Contract Request Models
public class CreateChangeOrderRequest
{
    public string Title { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ChangeOrderType Type { get; set; }
    public int? ContractId { get; set; }
    public string Description { get; set; } = string.Empty;
}

// Meeting Request Models
public class CreateMeetingRequest
{
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public List<int> AttendeeIds { get; set; } = new();
}