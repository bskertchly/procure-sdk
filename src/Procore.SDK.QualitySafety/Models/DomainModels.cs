using System;
using System.Collections.Generic;

namespace Procore.SDK.QualitySafety.Models;

/// <summary>
/// Domain models for QualitySafety client
/// </summary>

// Core Observation Models
public class Observation
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservationPriority Priority { get; set; }
    public ObservationStatus Status { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public enum ObservationPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum ObservationStatus
{
    Open,
    InProgress,
    Resolved,
    Closed,
    Cancelled
}

// Inspection Models
public class InspectionTemplate
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public InspectionTemplateStatus Status { get; set; }
    public List<InspectionTemplateItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class InspectionTemplateItem
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public InspectionItemType Type { get; set; }
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }
}

public class InspectionItem
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int TemplateItemId { get; set; }
    public string Response { get; set; } = string.Empty;
    public InspectionItemStatus Status { get; set; }
    public DateTime? InspectedAt { get; set; }
    public int? InspectedBy { get; set; }
    public List<string> EvidenceUrls { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum InspectionTemplateStatus
{
    Draft,
    Active,
    Archived
}

public enum InspectionItemType
{
    Text,
    CheckBox,
    Checkbox, // Alias for CheckBox to handle various casing  
    MultipleChoice,
    Photo,
    Signature,
    Numeric,
    Date
}

public enum InspectionItemStatus
{
    NotStarted,
    InProgress,
    Complete,
    Passed,
    Failed,
    NotApplicable
}

// Safety Models
public class SafetyIncident
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentSeverity Severity { get; set; }
    public IncidentType Type { get; set; }
    public DateTime IncidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int ReportedBy { get; set; }
    public IncidentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum IncidentSeverity
{
    Minor,
    Moderate,
    Major,
    Serious,
    Critical,
    Fatal
}

public enum IncidentType
{
    NearMiss,
    FirstAid,
    MedicalTreatment,
    LostTime,
    Fatality,
    PropertyDamage
}

public enum IncidentStatus
{
    Reported,
    UnderInvestigation,
    Resolved,
    Closed
}

// Compliance Models
public class ComplianceCheck
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string CheckType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ComplianceStatus Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int? InspectorId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum ComplianceStatus
{
    Scheduled,
    InProgress,
    Passed,
    Failed,
    Rescheduled
}