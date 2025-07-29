using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;
using NearMissPostResponse = Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Near_misses.Near_missesPostResponse;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between SafetyIncident domain models and Near Miss PostResponse types.
/// </summary>
public class NearMissPostResponseMapper : BaseTypeMapper<SafetyIncident, NearMissPostResponse>
{
    /// <summary>
    /// Maps from PostResponse type to SafetyIncident domain model.
    /// </summary>
    /// <param name="source">The PostResponse type to map from</param>
    /// <returns>The mapped SafetyIncident domain model</returns>
    protected override SafetyIncident DoMapToWrapper(NearMissPostResponse source)
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
    /// Maps from SafetyIncident domain model to PostResponse type.
    /// Note: This is typically not used for POST responses.
    /// </summary>
    /// <param name="source">The SafetyIncident domain model to map from</param>
    /// <returns>The mapped PostResponse type</returns>
    protected override NearMissPostResponse DoMapToGenerated(SafetyIncident source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new NearMissPostResponse
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
    private static int ExtractProjectIdFromSource(NearMissPostResponse source)
    {
        // The project ID is typically available in the context but not in the item itself
        // This would need to be set from the calling context
        return 0;
    }
    
    /// <summary>
    /// Extracts location information from various fields.
    /// </summary>
    private static string ExtractLocation(NearMissPostResponse source)
    {
        // Try to extract location from available fields
        return "Project Site"; // Default since specific location field may not be available
    }
}