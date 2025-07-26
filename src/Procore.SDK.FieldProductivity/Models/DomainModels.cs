using System;

namespace Procore.SDK.FieldProductivity.Models;

/// <summary>
/// Domain models for FieldProductivity client
/// </summary>

// Core Productivity Models
public class ProductivityReport
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public DateTime ReportDate { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public decimal UnitsCompleted { get; set; }
    public decimal HoursWorked { get; set; }
    public decimal ProductivityRate { get; set; }
    public int CrewSize { get; set; }
    public string Weather { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class FieldActivity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ActivityStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public decimal PercentComplete { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum ActivityStatus
{
    NotStarted,
    InProgress,
    Completed,
    OnHold,
    Cancelled
}

public class ResourceUtilization
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ResourceType { get; set; } = string.Empty;
    public string ResourceName { get; set; } = string.Empty;
    public decimal UtilizationRate { get; set; }
    public decimal AvailableHours { get; set; }
    public decimal UsedHours { get; set; }
    public DateTime ReportDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PerformanceMetric
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime MeasurementDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}