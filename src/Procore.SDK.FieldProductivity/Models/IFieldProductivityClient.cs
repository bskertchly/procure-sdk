using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.FieldProductivity.Models;

/// <summary>
/// Defines the contract for the FieldProductivity client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
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
    Task<CoreModels.PagedResult<ProductivityReport>> GetProductivityReportsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    Task<CoreModels.PagedResult<FieldActivity>> GetFieldActivitiesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
}