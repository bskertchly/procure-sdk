using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.QualitySafety.Tests.Models;

/// <summary>
/// Test domain models for QualitySafety client testing
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
}

public enum IncidentSeverity
{
    Minor,
    Moderate,
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
}

public enum ComplianceStatus
{
    Scheduled,
    InProgress,
    Passed,
    Failed,
    Rescheduled
}

// Request Models
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

public class CreateSafetyIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentSeverity Severity { get; set; }
    public IncidentType Type { get; set; }
    public DateTime IncidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class CreateComplianceCheckRequest
{
    public string CheckType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public int? InspectorId { get; set; }
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
/// Defines the contract for the QualitySafety client wrapper
/// </summary>
public interface IQualitySafetyClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }

    // Observation Operations
    Task<IEnumerable<Observation>> GetObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<Observation> GetObservationAsync(int companyId, int projectId, int observationId, CancellationToken cancellationToken = default);
    Task<Observation> CreateObservationAsync(int companyId, int projectId, CreateObservationRequest request, CancellationToken cancellationToken = default);
    Task<Observation> UpdateObservationAsync(int companyId, int projectId, int observationId, UpdateObservationRequest request, CancellationToken cancellationToken = default);
    Task DeleteObservationAsync(int companyId, int projectId, int observationId, CancellationToken cancellationToken = default);

    // Inspection Template Operations
    Task<IEnumerable<InspectionTemplate>> GetInspectionTemplatesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<InspectionTemplate> GetInspectionTemplateAsync(int companyId, int projectId, int templateId, CancellationToken cancellationToken = default);
    Task<InspectionTemplate> CreateInspectionTemplateAsync(int companyId, int projectId, CreateInspectionTemplateRequest request, CancellationToken cancellationToken = default);
    Task<InspectionTemplate> UpdateInspectionTemplateAsync(int companyId, int projectId, int templateId, CreateInspectionTemplateRequest request, CancellationToken cancellationToken = default);

    // Inspection Item Operations
    Task<IEnumerable<InspectionItem>> GetInspectionItemsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<InspectionItem> GetInspectionItemAsync(int companyId, int projectId, int itemId, CancellationToken cancellationToken = default);
    Task<InspectionItem> UpdateInspectionItemAsync(int companyId, int projectId, int itemId, string response, InspectionItemStatus status, CancellationToken cancellationToken = default);

    // Safety Incident Operations
    Task<IEnumerable<SafetyIncident>> GetSafetyIncidentsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<SafetyIncident> GetSafetyIncidentAsync(int companyId, int projectId, int incidentId, CancellationToken cancellationToken = default);
    Task<SafetyIncident> CreateSafetyIncidentAsync(int companyId, int projectId, CreateSafetyIncidentRequest request, CancellationToken cancellationToken = default);
    Task<SafetyIncident> UpdateSafetyIncidentAsync(int companyId, int projectId, int incidentId, IncidentStatus status, CancellationToken cancellationToken = default);

    // Compliance Operations
    Task<IEnumerable<ComplianceCheck>> GetComplianceChecksAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<ComplianceCheck> GetComplianceCheckAsync(int companyId, int projectId, int checkId, CancellationToken cancellationToken = default);
    Task<ComplianceCheck> CreateComplianceCheckAsync(int companyId, int projectId, CreateComplianceCheckRequest request, CancellationToken cancellationToken = default);
    Task<ComplianceCheck> CompleteComplianceCheckAsync(int companyId, int projectId, int checkId, ComplianceStatus status, string notes, CancellationToken cancellationToken = default);

    // Convenience Methods
    Task<IEnumerable<Observation>> GetOpenObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Observation>> GetCriticalObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Observation>> GetOverdueObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SafetyIncident>> GetRecentIncidentsAsync(int companyId, int projectId, int days, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetObservationSummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<PagedResult<Observation>> GetObservationsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<InspectionTemplate>> GetInspectionTemplatesPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<SafetyIncident>> GetSafetyIncidentsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
}