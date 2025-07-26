using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.FieldProductivity.Tests.Models;

/// <summary>
/// Test domain models for FieldProductivity client testing
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
}

// Request Models
public class CreateProductivityReportRequest
{
    public DateTime ReportDate { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public decimal UnitsCompleted { get; set; }
    public decimal HoursWorked { get; set; }
    public int CrewSize { get; set; }
    public string Weather { get; set; } = string.Empty;
}

public class UpdateFieldActivityRequest
{
    public string? ActivityName { get; set; }
    public ActivityStatus? Status { get; set; }
    public decimal? PercentComplete { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class RecordResourceUtilizationRequest
{
    public string ResourceType { get; set; } = string.Empty;
    public string ResourceName { get; set; } = string.Empty;
    public decimal UsedHours { get; set; }
    public decimal AvailableHours { get; set; }
    public DateTime ReportDate { get; set; }
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
/// Defines the contract for the FieldProductivity client wrapper
/// </summary>
public interface IFieldProductivityClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }

    // Productivity Reporting
    Task<IEnumerable<ProductivityReport>> GetProductivityReportsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<ProductivityReport> GetProductivityReportAsync(int companyId, int projectId, int reportId, CancellationToken cancellationToken = default);
    Task<ProductivityReport> CreateProductivityReportAsync(int companyId, int projectId, CreateProductivityReportRequest request, CancellationToken cancellationToken = default);

    // Field Activity Management
    Task<IEnumerable<FieldActivity>> GetFieldActivitiesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<FieldActivity> GetFieldActivityAsync(int companyId, int projectId, int activityId, CancellationToken cancellationToken = default);
    Task<FieldActivity> UpdateFieldActivityAsync(int companyId, int projectId, int activityId, UpdateFieldActivityRequest request, CancellationToken cancellationToken = default);

    // Resource Utilization
    Task<IEnumerable<ResourceUtilization>> GetResourceUtilizationAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<ResourceUtilization> RecordResourceUtilizationAsync(int companyId, int projectId, RecordResourceUtilizationRequest request, CancellationToken cancellationToken = default);

    // Performance Metrics
    Task<IEnumerable<PerformanceMetric>> GetPerformanceMetricsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<PerformanceMetric> RecordPerformanceMetricAsync(int companyId, int projectId, string metricName, decimal value, string unit, CancellationToken cancellationToken = default);

    // Analytics and Reporting
    Task<decimal> GetAverageProductivityRateAsync(int companyId, int projectId, string activityType, CancellationToken cancellationToken = default);
    Task<Dictionary<string, decimal>> GetProductivitySummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ResourceUtilization>> GetUnderUtilizedResourcesAsync(int companyId, int projectId, decimal threshold, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<PagedResult<ProductivityReport>> GetProductivityReportsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<FieldActivity>> GetFieldActivitiesPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
}