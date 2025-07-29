using System;

namespace Procore.SDK.FieldProductivity.Models;

/// <summary>
/// Request models for FieldProductivity client operations
/// </summary>

// Productivity Report Request Models
public class CreateProductivityReportRequest
{
    public DateTime ReportDate { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public decimal UnitsCompleted { get; set; }
    public decimal HoursWorked { get; set; }
    public int CrewSize { get; set; }
    public string Weather { get; set; } = string.Empty;
}

public class UpdateProductivityReportRequest
{
    public DateTime? ReportDate { get; set; }
    public string? ActivityType { get; set; }
    public decimal? UnitsCompleted { get; set; }
    public decimal? HoursWorked { get; set; }
    public int? CrewSize { get; set; }
    public string? Weather { get; set; }
}

// Field Activity Request Models
public class UpdateFieldActivityRequest
{
    public string? ActivityName { get; set; }
    public ActivityStatus? Status { get; set; }
    public decimal? PercentComplete { get; set; }
    public DateTime? CompletionDate { get; set; }
}

// Resource Utilization Request Models
public class RecordResourceUtilizationRequest
{
    public string ResourceType { get; set; } = string.Empty;
    public string ResourceName { get; set; } = string.Empty;
    public decimal UsedHours { get; set; }
    public decimal AvailableHours { get; set; }
    public DateTime ReportDate { get; set; }
}

// Bulk Operation Models
public class TimecardEntryUpdate
{
    public int TimecardEntryId { get; set; }
    public UpdateProductivityReportRequest UpdateRequest { get; set; } = new();
}

public class BulkOperationResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string Id { get; set; } = string.Empty;
}