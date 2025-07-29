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

    // Productivity Reporting (Core Timecard Operations)
    /// <summary>
    /// Gets all productivity reports for a company and optionally a specific project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier (0 for all projects).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of productivity reports.</returns>
    Task<IEnumerable<ProductivityReport>> GetProductivityReportsAsync(int companyId, int projectId = 0, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets multiple timecard entries by their identifiers.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="timecardEntryIds">The collection of timecard entry identifiers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of productivity reports for the specified timecard entries.</returns>
    Task<IEnumerable<ProductivityReport>> GetTimecardEntriesAsync(int companyId, IEnumerable<int> timecardEntryIds, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a single timecard entry by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="timecardEntryId">The timecard entry identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The productivity report for the specified timecard entry.</returns>
    Task<ProductivityReport> GetTimecardEntryAsync(int companyId, int timecardEntryId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates a timecard entry with new productivity data.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="timecardEntryId">The timecard entry identifier.</param>
    /// <param name="request">The update request containing new productivity data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated productivity report.</returns>
    Task<ProductivityReport> UpdateTimecardEntryAsync(int companyId, int timecardEntryId, UpdateProductivityReportRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes a timecard entry.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="timecardEntryId">The timecard entry identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The deleted productivity report.</returns>
    Task<ProductivityReport> DeleteTimecardEntryAsync(int companyId, int timecardEntryId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific productivity report by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="reportId">The report identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The productivity report.</returns>
    Task<ProductivityReport> GetProductivityReportAsync(int companyId, int projectId, int reportId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates a new productivity report for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="request">The request containing productivity data for the new report.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created productivity report.</returns>
    Task<ProductivityReport> CreateProductivityReportAsync(int companyId, int projectId, CreateProductivityReportRequest request, CancellationToken cancellationToken = default);

    // Bulk Operations
    /// <summary>
    /// Updates multiple timecard entries in a single operation.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="updates">The collection of timecard entry updates.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of bulk operation results for the updated timecard entries.</returns>
    Task<IEnumerable<BulkOperationResult<ProductivityReport>>> BulkUpdateTimecardEntriesAsync(int companyId, IEnumerable<TimecardEntryUpdate> updates, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes multiple timecard entries in a single operation.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="timecardEntryIds">The collection of timecard entry identifiers to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of bulk operation results for the deleted timecard entries.</returns>
    Task<IEnumerable<BulkOperationResult<ProductivityReport>>> BulkDeleteTimecardEntriesAsync(int companyId, IEnumerable<int> timecardEntryIds, CancellationToken cancellationToken = default);

    // Field Activity Management
    /// <summary>
    /// Gets all field activities for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of field activities.</returns>
    Task<IEnumerable<FieldActivity>> GetFieldActivitiesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a specific field activity by its identifier.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="activityId">The activity identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The field activity.</returns>
    Task<FieldActivity> GetFieldActivityAsync(int companyId, int projectId, int activityId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates a field activity with new data.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="activityId">The activity identifier.</param>
    /// <param name="request">The update request containing new activity data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated field activity.</returns>
    Task<FieldActivity> UpdateFieldActivityAsync(int companyId, int projectId, int activityId, UpdateFieldActivityRequest request, CancellationToken cancellationToken = default);

    // Resource Utilization
    /// <summary>
    /// Gets resource utilization data for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of resource utilization data.</returns>
    Task<IEnumerable<ResourceUtilization>> GetResourceUtilizationAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Records new resource utilization data for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="request">The request containing resource utilization data to record.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The recorded resource utilization data.</returns>
    Task<ResourceUtilization> RecordResourceUtilizationAsync(int companyId, int projectId, RecordResourceUtilizationRequest request, CancellationToken cancellationToken = default);

    // Performance Metrics
    /// <summary>
    /// Gets performance metrics for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of performance metrics.</returns>
    Task<IEnumerable<PerformanceMetric>> GetPerformanceMetricsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Records a new performance metric for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="metricName">The name of the performance metric.</param>
    /// <param name="value">The metric value.</param>
    /// <param name="unit">The unit of measurement for the metric.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The recorded performance metric.</returns>
    Task<PerformanceMetric> RecordPerformanceMetricAsync(int companyId, int projectId, string metricName, decimal value, string unit, CancellationToken cancellationToken = default);

    // Analytics and Reporting  
    /// <summary>
    /// Gets the average productivity rate for a specific activity type.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="activityType">The type of activity to calculate the average rate for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The average productivity rate.</returns>
    Task<decimal> GetAverageProductivityRateAsync(int companyId, int projectId, string activityType, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the average productivity rate for a specific activity type, optionally filtered by timecard entries.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="activityType">The type of activity to calculate the average rate for.</param>
    /// <param name="timecardEntryIds">Optional collection of timecard entry identifiers to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The average productivity rate.</returns>
    Task<decimal> GetAverageProductivityRateAsync(int companyId, int projectId, string activityType, IEnumerable<int>? timecardEntryIds = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a productivity summary with metrics by activity type for a project.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dictionary containing productivity metrics by activity type.</returns>
    Task<Dictionary<string, decimal>> GetProductivitySummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a productivity summary with metrics by activity type, optionally filtered by timecard entries.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="timecardEntryIds">Optional collection of timecard entry identifiers to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dictionary containing productivity metrics by activity type.</returns>
    Task<Dictionary<string, decimal>> GetProductivitySummaryAsync(int companyId, int projectId, IEnumerable<int>? timecardEntryIds = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets resources that are under-utilized below the specified threshold.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="threshold">The utilization threshold below which resources are considered under-utilized.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of under-utilized resources.</returns>
    Task<IEnumerable<ResourceUtilization>> GetUnderUtilizedResourcesAsync(int companyId, int projectId, decimal threshold, CancellationToken cancellationToken = default);

    // Pagination Support
    /// <summary>
    /// Gets productivity reports with pagination support.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="options">The pagination options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged result containing productivity reports.</returns>
    Task<CoreModels.PagedResult<ProductivityReport>> GetProductivityReportsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets field activities with pagination support.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="options">The pagination options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged result containing field activities.</returns>
    Task<CoreModels.PagedResult<FieldActivity>> GetFieldActivitiesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
}