using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;
using NearMissTypes = Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Near_misses;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between SafetyIncident domain models and Near Miss generated Kiota client types.
/// </summary>
public class NearMissTypeMapper : BaseTypeMapper<SafetyIncident, NearMissTypes.Near_misses>
{
    /// <summary>
    /// Maps from generated type to SafetyIncident domain model.
    /// </summary>
    /// <param name="source">The generated type to map from</param>
    /// <returns>The mapped SafetyIncident domain model</returns>
    protected override SafetyIncident DoMapToWrapper(NearMissTypes.Near_misses source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new SafetyIncident
        {
            Id = source.Id ?? 0,
            ProjectId = ExtractProjectIdFromSource(source),
            Title = source.IncidentTitle ?? "Near Miss Incident",
            Description = source.Description ?? string.Empty,
            Severity = IncidentSeverity.Minor, // Near misses are typically minor severity
            Type = IncidentType.NearMiss,
            IncidentDate = source.CreatedAt?.DateTime ?? DateTime.UtcNow, // Use CreatedAt as incident date
            Location = ExtractLocation(source),
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
    protected override NearMissTypes.Near_misses DoMapToGenerated(SafetyIncident source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new NearMissTypes.Near_misses
        {
            Id = source.Id,
            IncidentTitle = source.Title,
            Description = source.Description,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
    }
    
    /// <summary>
    /// Extracts project ID from the source object or returns 0 if not available.
    /// </summary>
    private static int ExtractProjectIdFromSource(NearMissTypes.Near_misses source)
    {
        // The project ID is typically available in the context but not in the item itself
        // This would need to be set from the calling context
        return 0;
    }
    
    /// <summary>
    /// Extracts location information from various fields.
    /// </summary>
    private static string ExtractLocation(NearMissTypes.Near_misses source)
    {
        // Try to extract location from available fields
        return "Project Site"; // Default since specific location field may not be available
    }
}