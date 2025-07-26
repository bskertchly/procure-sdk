using System;
using System.Collections.Generic;

namespace Procore.SDK.QualitySafety.Models;

/// <summary>
/// Request models for QualitySafety client operations
/// </summary>

// Observation Request Models
public class CreateObservationRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservationPriority Priority { get; set; }
    public string Category { get; set; } = string.Empty;
    public int? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class UpdateObservationRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ObservationPriority? Priority { get; set; }
    public ObservationStatus? Status { get; set; }
    public int? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Location { get; set; }
}

// Inspection Template Request Models
public class CreateInspectionTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CreateInspectionTemplateItemRequest> Items { get; set; } = new();
}

public class CreateInspectionTemplateItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public InspectionItemType Type { get; set; }
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }
}

// Safety Incident Request Models
public class CreateSafetyIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentSeverity Severity { get; set; }
    public IncidentType Type { get; set; }
    public DateTime IncidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
}

// Compliance Request Models
public class CreateComplianceCheckRequest
{
    public string CheckType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public int? InspectorId { get; set; }
}