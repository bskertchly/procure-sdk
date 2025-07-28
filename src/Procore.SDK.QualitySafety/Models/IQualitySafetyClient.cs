using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.QualitySafety.Models;

/// <summary>
/// Defines the contract for the QualitySafety client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
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
    Task<CoreModels.PagedResult<Observation>> GetObservationsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    Task<CoreModels.PagedResult<InspectionTemplate>> GetInspectionTemplatesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    Task<CoreModels.PagedResult<SafetyIncident>> GetSafetyIncidentsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
}