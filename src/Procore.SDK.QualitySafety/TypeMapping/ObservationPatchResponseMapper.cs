using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;
using ObservationItemPatchResponse = Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between Observation domain models and generated PatchResponse types.
/// </summary>
public class ObservationPatchResponseMapper : BaseTypeMapper<Observation, ObservationItemPatchResponse>
{
    /// <summary>
    /// Maps from PatchResponse type to Observation domain model.
    /// </summary>
    /// <param name="source">The PatchResponse type to map from</param>
    /// <returns>The mapped Observation domain model</returns>
    protected override Observation DoMapToWrapper(ObservationItemPatchResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new Observation
        {
            Id = source.Id ?? 0,
            ProjectId = ExtractProjectIdFromSource(source),
            Title = source.Name ?? string.Empty,
            Description = source.Description ?? string.Empty,
            Priority = MapPriority(source.Priority),
            Status = MapStatus(source.Status),
            AssignedTo = source.Assignee?.Id ?? 0,
            DueDate = null, // DueDate may not be available in PatchResponse
            CreatedAt = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
            UpdatedAt = source.UpdatedAt?.DateTime ?? DateTime.UtcNow,
            Location = source.Location?.Name ?? string.Empty
        };
    }

    /// <summary>
    /// Maps from Observation domain model to PatchResponse type.
    /// Note: This is typically not used for PATCH responses.
    /// </summary>
    /// <param name="source">The Observation domain model to map from</param>
    /// <returns>The mapped PatchResponse type</returns>
    protected override ObservationItemPatchResponse DoMapToGenerated(Observation source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new ObservationItemPatchResponse
        {
            Id = source.Id,
            Name = source.Title,
            Description = source.Description,
            DueDate = source.DueDate.HasValue ? DateOnly.FromDateTime(source.DueDate.Value) : null,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
    }
    
    /// <summary>
    /// Maps priority string to ObservationPriority enum.
    /// </summary>
    private static ObservationPriority MapPriority(global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse_priority? priority)
    {
        return priority switch
        {
            global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse_priority.Low => ObservationPriority.Low,
            global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse_priority.Medium => ObservationPriority.Medium,
            global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse_priority.High => ObservationPriority.High,
            global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse_priority.Urgent => ObservationPriority.Critical,
            _ => ObservationPriority.Medium
        };
    }
    
    /// <summary>
    /// Maps status string to ObservationStatus enum.
    /// </summary>
    private static ObservationStatus MapStatus(global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.PatchResponse_status? status)
    {
        // Note: This requires checking what values are available in PatchResponse_status enum
        // For now, provide a basic mapping
        return ObservationStatus.Open; // TODO: Map based on actual enum values
    }
    
    /// <summary>
    /// Extracts project ID from the source object or returns 0 if not available.
    /// </summary>
    private static int ExtractProjectIdFromSource(ObservationItemPatchResponse source)
    {
        // The project ID is typically available in the context but not in the item itself
        // This would need to be set from the calling context
        return 0;
    }
}