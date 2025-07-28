using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.ErrorHandling;
using Procore.SDK.Core.Logging;
using Procore.SDK.FieldProductivity.Models;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.FieldProductivity;

/// <summary>
/// Implementation of the FieldProductivity client wrapper that provides domain-specific 
/// convenience methods over the generated Kiota client.
/// </summary>
public class ProcoreFieldProductivityClient : IFieldProductivityClient
{
    private readonly Procore.SDK.FieldProductivity.FieldProductivityClient _generatedClient;
    private readonly ILogger<ProcoreFieldProductivityClient>? _logger;
    private readonly ErrorMapper? _errorMapper;
    private readonly StructuredLogger? _structuredLogger;
    private bool _disposed;

    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    public object RawClient => _generatedClient;

    /// <summary>
    /// Initializes a new instance of the ProcoreFieldProductivityClient.
    /// </summary>
    /// <param name="requestAdapter">The request adapter to use for HTTP communication.</param>
    /// <param name="logger">Optional logger for diagnostic information.</param>
    /// <param name="errorMapper">Optional error mapper for exception handling.</param>
    /// <param name="structuredLogger">Optional structured logger for correlation tracking.</param>
    public ProcoreFieldProductivityClient(
        IRequestAdapter requestAdapter, 
        ILogger<ProcoreFieldProductivityClient>? logger = null,
        ErrorMapper? errorMapper = null,
        StructuredLogger? structuredLogger = null)
    {
        _generatedClient = new Procore.SDK.FieldProductivity.FieldProductivityClient(requestAdapter);
        _logger = logger;
        _errorMapper = errorMapper;
        _structuredLogger = structuredLogger;
    }

    #region Private Helper Methods

    /// <summary>
    /// Executes an operation with proper error handling and logging.
    /// </summary>
    private async Task<T> ExecuteWithResilienceAsync<T>(
        Func<Task<T>> operation,
        string operationName,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        correlationId ??= Guid.NewGuid().ToString();
        
        using var operationScope = _structuredLogger?.BeginOperation(operationName, correlationId);
        
        try
        {
            _logger?.LogDebug("Executing operation {Operation} with correlation ID {CorrelationId}", operationName, correlationId);
            
            return await operation().ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            var mappedException = _errorMapper?.MapHttpException(ex, correlationId) ?? 
                new CoreModels.ProcoreCoreException(ex.Message, "HTTP_ERROR", null, correlationId);
            
            _structuredLogger?.LogError(mappedException, operationName, correlationId, 
                "HTTP error in operation {Operation}", operationName);
            
            throw mappedException;
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            _structuredLogger?.LogWarning(operationName, correlationId,
                "Operation {Operation} was cancelled", operationName);
            throw;
        }
        catch (Exception ex)
        {
            var wrappedException = new CoreModels.ProcoreCoreException(
                $"Unexpected error in {operationName}: {ex.Message}", 
                "UNEXPECTED_ERROR", 
                null, 
                correlationId);
            
            _structuredLogger?.LogError(wrappedException, operationName, correlationId,
                "Unexpected error in operation {Operation}", operationName);
            
            throw wrappedException;
        }
    }

    /// <summary>
    /// Executes an operation with proper error handling and logging (void return).
    /// </summary>
    private async Task ExecuteWithResilienceAsync(
        Func<Task> operation,
        string operationName,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        await ExecuteWithResilienceAsync(async () =>
        {
            await operation();
            return true; // Return a dummy value
        }, operationName, correlationId, cancellationToken);
    }

    #endregion

    #region Productivity Reporting

    /// <summary>
    /// Gets all productivity reports for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of productivity reports.</returns>
    public async Task<IEnumerable<ProductivityReport>> GetProductivityReportsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting productivity reports for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Placeholder implementation
                await Task.CompletedTask.ConfigureAwait(false);
                return Enumerable.Empty<ProductivityReport>();
            },
            $"GetProductivityReports-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific productivity report by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="reportId">The report ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The productivity report.</returns>
    public async Task<ProductivityReport> GetProductivityReportAsync(int companyId, int projectId, int reportId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting productivity report {ReportId} for project {ProjectId} in company {CompanyId}", reportId, projectId, companyId);
            
            // Placeholder implementation
            return new ProductivityReport 
            { 
                Id = reportId,
                ProjectId = projectId,
                ReportDate = DateTime.UtcNow.AddDays(-1),
                ActivityType = "Concrete Pouring",
                UnitsCompleted = 150.5m,
                HoursWorked = 8.0m,
                ProductivityRate = 18.8m,
                CrewSize = 5,
                Weather = "Clear",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get productivity report {ReportId} for project {ProjectId} in company {CompanyId}", reportId, projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Creates a new productivity report.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The productivity report creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created productivity report.</returns>
    public async Task<ProductivityReport> CreateProductivityReportAsync(int companyId, int projectId, CreateProductivityReportRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Creating productivity report for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Calculate productivity rate
                var productivityRate = request.HoursWorked > 0 ? request.UnitsCompleted / request.HoursWorked : 0;
                
                // Placeholder implementation
                return new ProductivityReport 
                { 
                    Id = 1,
                    ProjectId = projectId,
                    ReportDate = request.ReportDate,
                    ActivityType = request.ActivityType,
                    UnitsCompleted = request.UnitsCompleted,
                    HoursWorked = request.HoursWorked,
                    ProductivityRate = productivityRate,
                    CrewSize = request.CrewSize,
                    Weather = request.Weather,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            },
            $"CreateProductivityReport-{request.ActivityType}-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Field Activity Management

    /// <summary>
    /// Gets all field activities for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of field activities.</returns>
    public async Task<IEnumerable<FieldActivity>> GetFieldActivitiesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting field activities for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<FieldActivity>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get field activities for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Gets a specific field activity by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="activityId">The activity ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The field activity.</returns>
    public async Task<FieldActivity> GetFieldActivityAsync(int companyId, int projectId, int activityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting field activity {ActivityId} for project {ProjectId} in company {CompanyId}", activityId, projectId, companyId);
            
            // Placeholder implementation
            return new FieldActivity 
            { 
                Id = activityId,
                ProjectId = projectId,
                ActivityName = "Foundation Work",
                Description = "Pour foundation concrete",
                Status = ActivityStatus.InProgress,
                StartDate = DateTime.UtcNow.AddDays(-5),
                PercentComplete = 65.0m,
                Location = "Building A - Foundation",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get field activity {ActivityId} for project {ProjectId} in company {CompanyId}", activityId, projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing field activity.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="activityId">The activity ID.</param>
    /// <param name="request">The activity update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated field activity.</returns>
    public async Task<FieldActivity> UpdateFieldActivityAsync(int companyId, int projectId, int activityId, UpdateFieldActivityRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Updating field activity {ActivityId} for project {ProjectId} in company {CompanyId}", activityId, projectId, companyId);
            
            // Placeholder implementation
            return new FieldActivity 
            { 
                Id = activityId,
                ProjectId = projectId,
                ActivityName = request.ActivityName ?? "Updated Activity",
                Status = request.Status ?? ActivityStatus.InProgress,
                PercentComplete = request.PercentComplete ?? 50.0m,
                CompletionDate = request.CompletionDate,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update field activity {ActivityId} for project {ProjectId} in company {CompanyId}", activityId, projectId, companyId);
            throw;
        }
    }

    #endregion

    #region Resource Utilization

    /// <summary>
    /// Gets resource utilization data for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of resource utilization records.</returns>
    public async Task<IEnumerable<ResourceUtilization>> GetResourceUtilizationAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting resource utilization for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<ResourceUtilization>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get resource utilization for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Records resource utilization data.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The resource utilization recording request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The recorded resource utilization.</returns>
    public async Task<ResourceUtilization> RecordResourceUtilizationAsync(int companyId, int projectId, RecordResourceUtilizationRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Recording resource utilization for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // Calculate utilization rate
                var utilizationRate = request.AvailableHours > 0 ? (request.UsedHours / request.AvailableHours) * 100 : 0;
                
                // Placeholder implementation
                return new ResourceUtilization 
                { 
                    Id = 1,
                    ProjectId = projectId,
                    ResourceType = request.ResourceType,
                    ResourceName = request.ResourceName,
                    UtilizationRate = utilizationRate,
                    AvailableHours = request.AvailableHours,
                    UsedHours = request.UsedHours,
                    ReportDate = request.ReportDate,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            },
            $"RecordResourceUtilization-{request.ResourceType}-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    #endregion

    #region Performance Metrics

    /// <summary>
    /// Gets all performance metrics for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of performance metrics.</returns>
    public async Task<IEnumerable<PerformanceMetric>> GetPerformanceMetricsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting performance metrics for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<PerformanceMetric>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get performance metrics for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Records a performance metric.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="metricName">The metric name.</param>
    /// <param name="value">The metric value.</param>
    /// <param name="unit">The metric unit.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The recorded performance metric.</returns>
    public async Task<PerformanceMetric> RecordPerformanceMetricAsync(int companyId, int projectId, string metricName, decimal value, string unit, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(metricName)) throw new ArgumentException("Metric name cannot be null or empty", nameof(metricName));
        if (string.IsNullOrEmpty(unit)) throw new ArgumentException("Unit cannot be null or empty", nameof(unit));
        
        try
        {
            _logger?.LogDebug("Recording performance metric {MetricName} for project {ProjectId} in company {CompanyId}", metricName, projectId, companyId);
            
            // Placeholder implementation
            return new PerformanceMetric 
            { 
                Id = 1,
                ProjectId = projectId,
                MetricName = metricName,
                Value = value,
                Unit = unit,
                MeasurementDate = DateTime.UtcNow,
                Category = "Performance",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to record performance metric {MetricName} for project {ProjectId} in company {CompanyId}", metricName, projectId, companyId);
            throw;
        }
    }

    #endregion

    #region Analytics and Reporting

    /// <summary>
    /// Gets the average productivity rate for a specific activity type.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="activityType">The activity type.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The average productivity rate.</returns>
    public async Task<decimal> GetAverageProductivityRateAsync(int companyId, int projectId, string activityType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(activityType)) throw new ArgumentException("Activity type cannot be null or empty", nameof(activityType));
        
        try
        {
            _logger?.LogDebug("Getting average productivity rate for activity type {ActivityType} in project {ProjectId} in company {CompanyId}", activityType, projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return 15.5m;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get average productivity rate for activity type {ActivityType} in project {ProjectId} in company {CompanyId}", activityType, projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Gets a productivity summary for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with productivity summary by activity type.</returns>
    public async Task<Dictionary<string, decimal>> GetProductivitySummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting productivity summary for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return new Dictionary<string, decimal>
            {
                ["Concrete Pouring"] = 18.5m,
                ["Steel Installation"] = 12.3m,
                ["Framing"] = 22.1m,
                ["Electrical"] = 8.7m,
                ["Plumbing"] = 9.2m
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get productivity summary for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Gets resources that are under-utilized based on a threshold.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="threshold">The utilization threshold percentage.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of under-utilized resources.</returns>
    public async Task<IEnumerable<ResourceUtilization>> GetUnderUtilizedResourcesAsync(int companyId, int projectId, decimal threshold, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting under-utilized resources (threshold: {Threshold}%) for project {ProjectId} in company {CompanyId}", threshold, projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<ResourceUtilization>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get under-utilized resources for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    #endregion

    #region Pagination Support

    /// <summary>
    /// Gets productivity reports with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of productivity reports.</returns>
    public async Task<CoreModels.PagedResult<ProductivityReport>> GetProductivityReportsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        
        try
        {
            _logger?.LogDebug("Getting productivity reports with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<ProductivityReport>
            {
                Items = Enumerable.Empty<ProductivityReport>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get productivity reports with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    /// <summary>
    /// Gets field activities with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of field activities.</returns>
    public async Task<CoreModels.PagedResult<FieldActivity>> GetFieldActivitiesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        
        try
        {
            _logger?.LogDebug("Getting field activities with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<FieldActivity>
            {
                Items = Enumerable.Empty<FieldActivity>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get field activities with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw;
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Disposes of the ProcoreFieldProductivityClient and its resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the ProcoreFieldProductivityClient and its resources.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // The generated client doesn't implement IDisposable, so we don't dispose it
            _disposed = true;
        }
    }

    #endregion
}