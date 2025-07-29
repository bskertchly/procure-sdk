using System;

namespace Procore.SDK.FieldProductivity.Models;

/// <summary>
/// Domain models for FieldProductivity client
/// </summary>

// Core Productivity Models

/// <summary>
/// Represents a productivity report for field activities.
/// </summary>
public class ProductivityReport
{
    /// <summary>
    /// Gets or sets the unique identifier for the productivity report.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this report.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the date of the productivity report.
    /// </summary>
    public DateTime ReportDate { get; set; }

    /// <summary>
    /// Gets or sets the type of activity being measured.
    /// </summary>
    public string ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of units completed during the reporting period.
    /// </summary>
    public decimal UnitsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the total hours worked during the reporting period.
    /// </summary>
    public decimal HoursWorked { get; set; }

    /// <summary>
    /// Gets or sets the productivity rate (units per hour).
    /// </summary>
    public decimal ProductivityRate { get; set; }

    /// <summary>
    /// Gets or sets the size of the crew working on the activity.
    /// </summary>
    public int CrewSize { get; set; }

    /// <summary>
    /// Gets or sets the weather conditions during the reporting period.
    /// </summary>
    public string Weather { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the report was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the report was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a field activity in a construction project.
/// </summary>
public class FieldActivity
{
    /// <summary>
    /// Gets or sets the unique identifier for the field activity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this activity.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the name of the activity.
    /// </summary>
    public string ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the activity.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the activity.
    /// </summary>
    public ActivityStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the start date of the activity.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the completion date of the activity.
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    /// <summary>
    /// Gets or sets the percentage of completion for the activity.
    /// </summary>
    public decimal PercentComplete { get; set; }

    /// <summary>
    /// Gets or sets the location where the activity is taking place.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the activity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the activity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Defines the status of a field activity.
/// </summary>
public enum ActivityStatus
{
    /// <summary>
    /// Activity has not been started yet.
    /// </summary>
    NotStarted,
    
    /// <summary>
    /// Activity is currently in progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Activity has been completed.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Activity is temporarily on hold.
    /// </summary>
    OnHold,
    
    /// <summary>
    /// Activity has been cancelled.
    /// </summary>
    Cancelled
}

/// <summary>
/// Represents resource utilization metrics for field operations.
/// </summary>
public class ResourceUtilization
{
    /// <summary>
    /// Gets or sets the unique identifier for the resource utilization record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this resource utilization.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the type of resource (e.g., equipment, labor, material).
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the specific resource.
    /// </summary>
    public string ResourceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the utilization rate as a percentage (0.0 to 1.0).
    /// </summary>
    public decimal UtilizationRate { get; set; }

    /// <summary>
    /// Gets or sets the total hours the resource was available.
    /// </summary>
    public decimal AvailableHours { get; set; }

    /// <summary>
    /// Gets or sets the actual hours the resource was used.
    /// </summary>
    public decimal UsedHours { get; set; }

    /// <summary>
    /// Gets or sets the date of the utilization report.
    /// </summary>
    public DateTime ReportDate { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a performance metric measurement.
/// </summary>
public class PerformanceMetric
{
    /// <summary>
    /// Gets or sets the unique identifier for the performance metric.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this metric.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the name of the metric being measured.
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the measured value of the metric.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement for the metric.
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the measurement was taken.
    /// </summary>
    public DateTime MeasurementDate { get; set; }

    /// <summary>
    /// Gets or sets the category this metric belongs to.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the metric was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the metric was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}