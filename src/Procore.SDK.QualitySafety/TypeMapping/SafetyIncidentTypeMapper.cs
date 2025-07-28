using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;
using InjuriesTypes = Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between SafetyIncident domain models and generated Kiota client types.
/// Maps primarily from injury incident types.
/// </summary>
public class SafetyIncidentTypeMapper : BaseTypeMapper<SafetyIncident, InjuriesTypes.Injuries>
{
    /// <summary>
    /// Maps from generated type to SafetyIncident domain model.
    /// </summary>
    /// <param name="source">The generated type to map from</param>
    /// <returns>The mapped SafetyIncident domain model</returns>
    protected override SafetyIncident DoMapToWrapper(InjuriesTypes.Injuries source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new SafetyIncident
        {
            Id = source.Id ?? 0,
            ProjectId = ExtractProjectIdFromSource(source),
            Title = source.IncidentTitle ?? "Safety Incident",
            Description = source.Description ?? string.Empty,
            Severity = MapSeverity(source.Recordable),
            Type = IncidentType.MedicalTreatment,
            IncidentDate = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
            Location = "Project Site", // Location not directly available in injury type
            ReportedBy = source.IncidentCreatedBy?.Id ?? 0,
            Status = IncidentStatus.Reported,
            CreatedAt = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
            UpdatedAt = source.UpdatedAt?.DateTime ?? DateTime.UtcNow
        };
    }

    /// <summary>
    /// Maps from SafetyIncident domain model to generated type.
    /// </summary>
    /// <param name="source">The SafetyIncident domain model to map from</param>
    /// <returns>The mapped generated type</returns>
    protected override InjuriesTypes.Injuries DoMapToGenerated(SafetyIncident source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new InjuriesTypes.Injuries
        {
            Id = source.Id,
            IncidentTitle = source.Title,
            Description = source.Description,
            Recordable = source.Severity == IncidentSeverity.Major || source.Severity == IncidentSeverity.Serious || source.Severity == IncidentSeverity.Critical || source.Severity == IncidentSeverity.Fatal,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
    }
    
    /// <summary>
    /// Maps recordable status to incident severity.
    /// </summary>
    private static IncidentSeverity MapSeverity(bool? recordable)
    {
        return recordable == true ? IncidentSeverity.Major : IncidentSeverity.Minor;
    }
    
    /// <summary>
    /// Extracts project ID from the source object or returns 0 if not available.
    /// </summary>
    private static int ExtractProjectIdFromSource(InjuriesTypes.Injuries source)
    {
        // The project ID is typically available in the context but not in the item itself
        // This would need to be set from the calling context
        return 0;
    }
}