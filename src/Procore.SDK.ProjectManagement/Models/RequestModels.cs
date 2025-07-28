using System;
using System.Collections.Generic;

namespace Procore.SDK.ProjectManagement.Models;

/// <summary>
/// Request models for ProjectManagement client operations
/// </summary>

// Project Request Models

/// <summary>
/// Request model for creating a new project.
/// </summary>
public class CreateProjectRequest
{
    /// <summary>
    /// Gets or sets the name of the project to be created.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the planned start date for the project.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the planned end date for the project.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Gets or sets the initial budget allocation for the project.
    /// </summary>
    public decimal? Budget { get; set; }
    
    /// <summary>
    /// Gets or sets the type or category of the project.
    /// </summary>
    public string ProjectType { get; set; } = string.Empty;
}

/// <summary>
/// Request model for updating an existing project. All properties are optional.
/// </summary>
public class UpdateProjectRequest
{
    /// <summary>
    /// Gets or sets the new name for the project, if provided.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the new description for the project, if provided.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the new status for the project, if provided.
    /// </summary>
    public ProjectStatus? Status { get; set; }
    
    /// <summary>
    /// Gets or sets the new start date for the project, if provided.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the new end date for the project, if provided.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Gets or sets the new budget allocation for the project, if provided.
    /// </summary>
    public decimal? Budget { get; set; }
}

// Budget Request Models

/// <summary>
/// Request model for creating a new budget change.
/// </summary>
public class CreateBudgetChangeRequest
{
    /// <summary>
    /// Gets or sets the description of the budget change.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total amount of the budget change.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the list of individual line item changes that make up this budget change.
    /// </summary>
    public List<BudgetLineItemChange> LineItems { get; set; } = new();
}

/// <summary>
/// Represents a change to a specific budget line item within a budget change request.
/// </summary>
public class BudgetLineItemChange
{
    /// <summary>
    /// Gets or sets the ID of the budget line item being changed.
    /// </summary>
    public int LineItemId { get; set; }
    
    /// <summary>
    /// Gets or sets the amount of change for this line item.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the reason or justification for this line item change.
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}

// Contract Request Models

/// <summary>
/// Request model for creating a new change order.
/// </summary>
public class CreateChangeOrderRequest
{
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
    /// Gets or sets the type of change order (Prime or Commitment).
    /// </summary>
    public ChangeOrderType Type { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the contract this change order modifies, if applicable.
    /// </summary>
    public int? ContractId { get; set; }
    
    /// <summary>
    /// Gets or sets the detailed description of the change order.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

// Meeting Request Models

/// <summary>
/// Request model for creating a new meeting.
/// </summary>
public class CreateMeetingRequest
{
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
    /// Gets or sets the list of user IDs for meeting attendees.
    /// </summary>
    public List<int> AttendeeIds { get; set; } = new();
}