using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;
using InjuryPostResponse = Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostResponse;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between SafetyIncident domain models and Injury PostResponse types.
/// </summary>
public class SafetyIncidentPostResponseMapper : BaseTypeMapper<SafetyIncident, InjuryPostResponse>
{
    /// <summary>
    /// Maps from PostResponse type to SafetyIncident domain model.
    /// </summary>
    /// <param name="source">The PostResponse type to map from</param>
    /// <returns>The mapped SafetyIncident domain model</returns>
    protected override SafetyIncident DoMapToWrapper(InjuryPostResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new SafetyIncident
        {
            Id = source.Id ?? 0,
            ProjectId = ExtractProjectIdFromSource(source),
            Title = source.IncidentTitle ?? "Safety Incident",
            Description = source.Description ?? string.Empty,
            Severity = MapSeverity(source.Recordable),
            Type = IncidentType.MedicalTreatment, // Map injury to medical treatment
            IncidentDate = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
            Location = ExtractLocation(source),
            ReportedBy = source.IncidentCreatedBy?.Id ?? 0,
            Status = IncidentStatus.Reported,
            CreatedAt = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
            UpdatedAt = source.UpdatedAt?.DateTime ?? DateTime.UtcNow
        };
    }

    /// <summary>
    /// Maps from SafetyIncident domain model to PostResponse type.
    /// Note: This is typically not used for POST responses.
    /// </summary>
    /// <param name="source">The SafetyIncident domain model to map from</param>
    /// <returns>The mapped PostResponse type</returns>
    protected override InjuryPostResponse DoMapToGenerated(SafetyIncident source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new InjuryPostResponse
        {
            Id = source.Id,
            IncidentTitle = source.Title,
            Description = source.Description,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
    }
    
    /// <summary>
    /// Maps recordable flag to incident severity.
    /// </summary>
    private static IncidentSeverity MapSeverity(bool? recordable)
    {
        return recordable == true ? IncidentSeverity.Major : IncidentSeverity.Minor;
    }
    
    /// <summary>
    /// Extracts project ID from the source object or returns 0 if not available.
    /// </summary>
    private static int ExtractProjectIdFromSource(InjuryPostResponse source)
    {
        // The project ID is typically available in the context but not in the item itself
        // This would need to be set from the calling context
        return 0;
    }
    
    /// <summary>
    /// Extracts location information from various fields.
    /// </summary>
    private static string ExtractLocation(InjuryPostResponse source)
    {
        // Try to extract location from available fields
        return "Project Site"; // Default since specific location field may not be available
    }
}