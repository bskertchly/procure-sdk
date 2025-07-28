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
using Procore.SDK.QualitySafety.Models;
using Procore.SDK.QualitySafety.TypeMapping;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.QualitySafety;

/// <summary>
/// Implementation of the QualitySafety client wrapper that provides domain-specific 
/// convenience methods over the generated Kiota client.
/// </summary>
public class ProcoreQualitySafetyClient : IQualitySafetyClient
{
    private readonly Procore.SDK.QualitySafety.QualitySafetyClient _generatedClient;
    private readonly ILogger<ProcoreQualitySafetyClient>? _logger;
    private readonly StructuredLogger? _structuredLogger;
    private readonly ObservationTypeMapper _observationTypeMapper;
    private bool _disposed;

    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    public object RawClient => _generatedClient;

    /// <summary>
    /// Initializes a new instance of the ProcoreQualitySafetyClient.
    /// </summary>
    /// <param name="requestAdapter">The request adapter to use for HTTP communication.</param>
    /// <param name="logger">Optional logger for diagnostic information.</param>
    /// <param name="structuredLogger">Optional structured logger for correlation tracking.</param>
    public ProcoreQualitySafetyClient(
        IRequestAdapter requestAdapter, 
        ILogger<ProcoreQualitySafetyClient>? logger = null,
        StructuredLogger? structuredLogger = null)
    {
        _generatedClient = new Procore.SDK.QualitySafety.QualitySafetyClient(requestAdapter);
        _logger = logger;
        _structuredLogger = structuredLogger;
        _observationTypeMapper = new ObservationTypeMapper();
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
            var mappedException = ErrorMapper.MapHttpException(ex, correlationId);
            
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
                new Dictionary<string, object> { { "inner_exception", ex.GetType().Name } }, 
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

    #region Observation Operations

    /// <summary>
    /// Gets all observations for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of observations.</returns>
    public async Task<IEnumerable<Observation>> GetObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Getting observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
                
                // TODO: Replace with actual implementation using generated client
                // This is currently a placeholder implementation
                return Enumerable.Empty<Observation>();
            },
            $"GetObservations-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Gets a specific observation by ID.
    /// Note: The QualitySafety API has limited read endpoints. This implementation provides placeholder functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="observationId">The observation ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The observation.</returns>
    public async Task<Observation> GetObservationAsync(int companyId, int projectId, int observationId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Getting observation {ObservationId} for project {ProjectId} in company {CompanyId} - limited API endpoint available", observationId, projectId, companyId);
            
            // Note: The QualitySafety API has limited GET endpoints for observations.
            // Most operations are focused on managing observations through the recycle bin system.
            // This implementation provides a consistent interface while highlighting API limitations.
            
            await Task.CompletedTask.ConfigureAwait(false);
            
            // Return a mapped observation using the type mapper
            var placeholderData = new object(); // Represents limited available data
            var observation = _observationTypeMapper.MapToWrapper(placeholderData);
            
            // Set the requested IDs
            observation.Id = observationId;
            observation.ProjectId = projectId;
            
            return observation;
        }, "GetObservationAsync", null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an observation (sends to recycle bin).
    /// This method uses the actual generated Kiota client.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="observationId">The observation ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteObservationAsync(int companyId, int projectId, int observationId, CancellationToken cancellationToken = default)
    {
        await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Deleting observation {ObservationId} for project {ProjectId} in company {CompanyId} using generated Kiota client", observationId, projectId, companyId);
            
            // Use the generated Kiota client to delete the observation (send to recycle bin)
            await _generatedClient.Rest.V10.Projects[projectId].Observations.Items[observationId].DeleteAsync(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            
            _logger?.LogDebug("Successfully deleted observation {ObservationId} for project {ProjectId} in company {CompanyId}", observationId, projectId, companyId);
        }, "DeleteObservationAsync", null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a new observation.
    /// Note: The QualitySafety API has limited creation endpoints. This provides placeholder functionality.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The observation creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created observation.</returns>
    public async Task<Observation> CreateObservationAsync(int companyId, int projectId, CreateObservationRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        return await ExecuteWithResilienceAsync(
            async () =>
            {
                _logger?.LogDebug("Creating observation for project {ProjectId} in company {CompanyId} - limited API endpoint available", projectId, companyId);
                
                // TODO: Replace with actual implementation using generated client
                // This is currently a placeholder implementation
                return new Observation 
                { 
                    Id = 1,
                    ProjectId = projectId,
                    Title = request.Title,
                    Description = request.Description,
                    Priority = request.Priority,
                    Status = ObservationStatus.Open,
                    Category = request.Category,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    AssignedTo = request.AssignedTo,
                    DueDate = request.DueDate,
                    Location = request.Location,
                    UpdatedAt = DateTime.UtcNow
                };
            },
            $"CreateObservation-{request.Title}-Project-{projectId}-Company-{companyId}",
            null,
            cancellationToken);
    }

    /// <summary>
    /// Updates an existing observation.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="observationId">The observation ID.</param>
    /// <param name="request">The observation update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated observation.</returns>
    public async Task<Observation> UpdateObservationAsync(int companyId, int projectId, int observationId, UpdateObservationRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Updating observation {ObservationId} for project {ProjectId} in company {CompanyId}", observationId, projectId, companyId);
            
            // Placeholder implementation
            return new Observation 
            { 
                Id = observationId,
                ProjectId = projectId,
                Title = request.Title ?? "Updated Observation",
                Description = request.Description ?? "Updated Description",
                Priority = request.Priority ?? ObservationPriority.Medium,
                Status = request.Status ?? ObservationStatus.InProgress,
                AssignedTo = request.AssignedTo,
                DueDate = request.DueDate,
                Location = request.Location ?? "Updated Location",
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update observation {ObservationId} for project {ProjectId} in company {CompanyId}", observationId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region Inspection Template Operations

    /// <summary>
    /// Gets all inspection templates for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of inspection templates.</returns>
    public async Task<IEnumerable<InspectionTemplate>> GetInspectionTemplatesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting inspection templates for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<InspectionTemplate>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get inspection templates for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific inspection template by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="templateId">The template ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The inspection template.</returns>
    public async Task<InspectionTemplate> GetInspectionTemplateAsync(int companyId, int projectId, int templateId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting inspection template {TemplateId} for project {ProjectId} in company {CompanyId}", templateId, projectId, companyId);
            
            // Placeholder implementation
            return new InspectionTemplate 
            { 
                Id = templateId,
                ProjectId = projectId,
                Name = "Placeholder Template",
                Description = "Placeholder Description",
                Status = InspectionTemplateStatus.Active,
                Items = new List<InspectionTemplateItem>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get inspection template {TemplateId} for project {ProjectId} in company {CompanyId}", templateId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Creates a new inspection template.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The template creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created inspection template.</returns>
    public async Task<InspectionTemplate> CreateInspectionTemplateAsync(int companyId, int projectId, CreateInspectionTemplateRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Creating inspection template for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            var templateItems = request.Items.Select((item, index) => new InspectionTemplateItem
            {
                Id = index + 1,
                TemplateId = 1,
                Title = item.Title,
                Description = item.Description,
                Type = item.Type,
                IsRequired = item.IsRequired,
                SortOrder = item.SortOrder
            }).ToList();

            return new InspectionTemplate 
            { 
                Id = 1,
                ProjectId = projectId,
                Name = request.Name,
                Description = request.Description,
                Status = InspectionTemplateStatus.Draft,
                Items = templateItems,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create inspection template for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Updates an existing inspection template.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="templateId">The template ID.</param>
    /// <param name="request">The template update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated inspection template.</returns>
    public async Task<InspectionTemplate> UpdateInspectionTemplateAsync(int companyId, int projectId, int templateId, CreateInspectionTemplateRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Updating inspection template {TemplateId} for project {ProjectId} in company {CompanyId}", templateId, projectId, companyId);
            
            // Placeholder implementation
            var templateItems = request.Items.Select((item, index) => new InspectionTemplateItem
            {
                Id = index + 1,
                TemplateId = templateId,
                Title = item.Title,
                Description = item.Description,
                Type = item.Type,
                IsRequired = item.IsRequired,
                SortOrder = item.SortOrder
            }).ToList();

            return new InspectionTemplate 
            { 
                Id = templateId,
                ProjectId = projectId,
                Name = request.Name,
                Description = request.Description,
                Status = InspectionTemplateStatus.Active,
                Items = templateItems,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update inspection template {TemplateId} for project {ProjectId} in company {CompanyId}", templateId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region Inspection Item Operations

    /// <summary>
    /// Gets all inspection items for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of inspection items.</returns>
    public async Task<IEnumerable<InspectionItem>> GetInspectionItemsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting inspection items for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<InspectionItem>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get inspection items for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific inspection item by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="itemId">The inspection item ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The inspection item.</returns>
    public async Task<InspectionItem> GetInspectionItemAsync(int companyId, int projectId, int itemId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting inspection item {ItemId} for project {ProjectId} in company {CompanyId}", itemId, projectId, companyId);
            
            // Placeholder implementation
            return new InspectionItem 
            { 
                Id = itemId,
                ProjectId = projectId,
                TemplateItemId = 1,
                Response = "",
                Status = InspectionItemStatus.NotStarted,
                EvidenceUrls = new List<string>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get inspection item {ItemId} for project {ProjectId} in company {CompanyId}", itemId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Updates an inspection item with response and status.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="itemId">The inspection item ID.</param>
    /// <param name="response">The inspection response.</param>
    /// <param name="status">The inspection status.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated inspection item.</returns>
    public async Task<InspectionItem> UpdateInspectionItemAsync(int companyId, int projectId, int itemId, string response, InspectionItemStatus status, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(response))
        {
            throw new ArgumentException("Response cannot be null or empty", nameof(response));
        }
        
        try
        {
            _logger?.LogDebug("Updating inspection item {ItemId} for project {ProjectId} in company {CompanyId}", itemId, projectId, companyId);
            
            // Placeholder implementation
            return new InspectionItem 
            { 
                Id = itemId,
                ProjectId = projectId,
                TemplateItemId = 1,
                Response = response,
                Status = status,
                InspectedAt = DateTime.UtcNow,
                InspectedBy = 1,
                EvidenceUrls = new List<string>(),
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update inspection item {ItemId} for project {ProjectId} in company {CompanyId}", itemId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region Safety Incident Operations

    /// <summary>
    /// Gets all safety incidents for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of safety incidents.</returns>
    public async Task<IEnumerable<SafetyIncident>> GetSafetyIncidentsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting safety incidents for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<SafetyIncident>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get safety incidents for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific safety incident by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="incidentId">The incident ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The safety incident.</returns>
    public async Task<SafetyIncident> GetSafetyIncidentAsync(int companyId, int projectId, int incidentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting safety incident {IncidentId} for project {ProjectId} in company {CompanyId}", incidentId, projectId, companyId);
            
            // Placeholder implementation
            return new SafetyIncident 
            { 
                Id = incidentId,
                ProjectId = projectId,
                Title = "Placeholder Incident",
                Description = "Placeholder Description",
                Severity = IncidentSeverity.Minor,
                Type = IncidentType.NearMiss,
                IncidentDate = DateTime.UtcNow.AddDays(-1),
                Location = "Site Location",
                ReportedBy = 1,
                Status = IncidentStatus.Reported,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get safety incident {IncidentId} for project {ProjectId} in company {CompanyId}", incidentId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Creates a new safety incident.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The incident creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created safety incident.</returns>
    public async Task<SafetyIncident> CreateSafetyIncidentAsync(int companyId, int projectId, CreateSafetyIncidentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Creating safety incident for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            return new SafetyIncident 
            { 
                Id = 1,
                ProjectId = projectId,
                Title = request.Title,
                Description = request.Description,
                Severity = request.Severity,
                Type = request.Type,
                IncidentDate = request.IncidentDate,
                Location = request.Location,
                ReportedBy = 1,
                Status = IncidentStatus.Reported,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create safety incident for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Updates the status of a safety incident.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="incidentId">The incident ID.</param>
    /// <param name="status">The new status.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated safety incident.</returns>
    public async Task<SafetyIncident> UpdateSafetyIncidentAsync(int companyId, int projectId, int incidentId, IncidentStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Updating safety incident {IncidentId} status to {Status} for project {ProjectId} in company {CompanyId}", incidentId, status, projectId, companyId);
            
            // Placeholder implementation
            return new SafetyIncident 
            { 
                Id = incidentId,
                ProjectId = projectId,
                Title = "Updated Incident",
                Status = status,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update safety incident {IncidentId} for project {ProjectId} in company {CompanyId}", incidentId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region Compliance Operations

    /// <summary>
    /// Gets all compliance checks for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of compliance checks.</returns>
    public async Task<IEnumerable<ComplianceCheck>> GetComplianceChecksAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting compliance checks for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<ComplianceCheck>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get compliance checks for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific compliance check by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="checkId">The compliance check ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The compliance check.</returns>
    public async Task<ComplianceCheck> GetComplianceCheckAsync(int companyId, int projectId, int checkId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting compliance check {CheckId} for project {ProjectId} in company {CompanyId}", checkId, projectId, companyId);
            
            // Placeholder implementation
            return new ComplianceCheck 
            { 
                Id = checkId,
                ProjectId = projectId,
                CheckType = "Safety Inspection",
                Description = "Placeholder Compliance Check",
                Status = ComplianceStatus.Scheduled,
                ScheduledDate = DateTime.UtcNow.AddDays(7),
                Notes = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get compliance check {CheckId} for project {ProjectId} in company {CompanyId}", checkId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Creates a new compliance check.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The compliance check creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created compliance check.</returns>
    public async Task<ComplianceCheck> CreateComplianceCheckAsync(int companyId, int projectId, CreateComplianceCheckRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Creating compliance check for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            return new ComplianceCheck 
            { 
                Id = 1,
                ProjectId = projectId,
                CheckType = request.CheckType,
                Description = request.Description,
                Status = ComplianceStatus.Scheduled,
                ScheduledDate = request.ScheduledDate,
                InspectorId = request.InspectorId,
                Notes = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create compliance check for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Completes a compliance check with status and notes.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="checkId">The compliance check ID.</param>
    /// <param name="status">The completion status.</param>
    /// <param name="notes">The completion notes.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The completed compliance check.</returns>
    public async Task<ComplianceCheck> CompleteComplianceCheckAsync(int companyId, int projectId, int checkId, ComplianceStatus status, string notes, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Completing compliance check {CheckId} with status {Status} for project {ProjectId} in company {CompanyId}", checkId, status, projectId, companyId);
            
            // Placeholder implementation
            return new ComplianceCheck 
            { 
                Id = checkId,
                ProjectId = projectId,
                CheckType = "Safety Inspection",
                Description = "Completed Compliance Check",
                Status = status,
                CompletedDate = DateTime.UtcNow,
                Notes = notes ?? "",
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to complete compliance check {CheckId} for project {ProjectId} in company {CompanyId}", checkId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region Convenience Methods

    /// <summary>
    /// Gets all open observations for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of open observations.</returns>
    public async Task<IEnumerable<Observation>> GetOpenObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting open observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<Observation>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get open observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets all critical priority observations for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of critical observations.</returns>
    public async Task<IEnumerable<Observation>> GetCriticalObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting critical observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<Observation>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get critical observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets all overdue observations for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of overdue observations.</returns>
    public async Task<IEnumerable<Observation>> GetOverdueObservationsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting overdue observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<Observation>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get overdue observations for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets recent safety incidents within specified days.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="days">Number of days to look back.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of recent safety incidents.</returns>
    public async Task<IEnumerable<SafetyIncident>> GetRecentIncidentsAsync(int companyId, int projectId, int days, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting recent incidents (last {Days} days) for project {ProjectId} in company {CompanyId}", days, projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return Enumerable.Empty<SafetyIncident>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get recent incidents for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a summary count of observations by status.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with observation counts by status.</returns>
    public async Task<Dictionary<string, int>> GetObservationSummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting observation summary for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask.ConfigureAwait(false);
            return new Dictionary<string, int>
            {
                ["Open"] = 5,
                ["InProgress"] = 3,
                ["Resolved"] = 10,
                ["Closed"] = 25,
                ["Cancelled"] = 2
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get observation summary for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region Pagination Support

    /// <summary>
    /// Gets observations with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of observations.</returns>
    public async Task<CoreModels.PagedResult<Observation>> GetObservationsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting observations with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<Observation>
            {
                Items = Enumerable.Empty<Observation>(),
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
            _logger?.LogError(ex, "Failed to get observations with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets inspection templates with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of inspection templates.</returns>
    public async Task<CoreModels.PagedResult<InspectionTemplate>> GetInspectionTemplatesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting inspection templates with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<InspectionTemplate>
            {
                Items = Enumerable.Empty<InspectionTemplate>(),
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
            _logger?.LogError(ex, "Failed to get inspection templates with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets safety incidents with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of safety incidents.</returns>
    public async Task<CoreModels.PagedResult<SafetyIncident>> GetSafetyIncidentsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting safety incidents with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<SafetyIncident>
            {
                Items = Enumerable.Empty<SafetyIncident>(),
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
            _logger?.LogError(ex, "Failed to get safety incidents with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for project {projectId} in company {companyId}", ex);
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Disposes of the ProcoreQualitySafetyClient and its resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the ProcoreQualitySafetyClient and its resources.
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