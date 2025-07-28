using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;
using ObservationItems = Procore.SDK.QualitySafety.Rest.V10.Observations.Items;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between Observation domain models and generated Kiota client types.
/// Enhanced to use real generated client types for comprehensive mapping.
/// </summary>
public class ObservationTypeMapper : BaseTypeMapper<Observation, ObservationItems.Items>
{
    /// <summary>
    /// Maps from generated type to Observation domain model.
    /// Enhanced to map real properties from the generated Kiota client types.
    /// </summary>
    /// <param name="source">The generated type to map from</param>
    /// <returns>The mapped Observation domain model</returns>
    protected override Observation DoMapToWrapper(ObservationItems.Items source)
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
            Category = ExtractCategory(source.Type),
            CreatedAt = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
            CreatedBy = source.CreatedBy?.Id ?? 0,
            AssignedTo = source.Assignee?.Id,
            DueDate = null, // DueDate may not be available in Items type
            Location = ExtractLocation(source.Location),
            UpdatedAt = source.UpdatedAt?.DateTime ?? DateTime.UtcNow
        };
    }

    /// <summary>
    /// Maps from Observation domain model to generated type.
    /// Enhanced to create proper generated client types for POST/PATCH operations.
    /// </summary>
    /// <param name="source">The Observation domain model to map from</param>
    /// <returns>The mapped generated type</returns>
    protected override ObservationItems.Items DoMapToGenerated(Observation source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new ObservationItems.Items
        {
            Id = source.Id,
            Name = source.Title,
            Description = source.Description,
            Priority = MapPriorityToGenerated(source.Priority),
            Status = MapStatusToGenerated(source.Status),
            DueDate = source.DueDate.HasValue ? DateOnly.FromDateTime(source.DueDate.Value) : null,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
    }
    
    /// <summary>
    /// Maps domain priority to generated priority enum.
    /// </summary>
    private static ObservationPriority MapPriority(ObservationItems.Items_priority? priority)
    {
        return priority switch
        {
            ObservationItems.Items_priority.Low => ObservationPriority.Low,
            ObservationItems.Items_priority.Medium => ObservationPriority.Medium,
            ObservationItems.Items_priority.High => ObservationPriority.High,
            ObservationItems.Items_priority.Urgent => ObservationPriority.Critical,
            _ => ObservationPriority.Medium
        };
    }
    
    /// <summary>
    /// Maps domain status to generated status enum.
    /// </summary>
    private static ObservationStatus MapStatus(ObservationItems.Items_status? status)
    {
        return status switch
        {
            ObservationItems.Items_status.Initiated => ObservationStatus.Open,
            ObservationItems.Items_status.Ready_for_review => ObservationStatus.InProgress,
            ObservationItems.Items_status.Not_accepted => ObservationStatus.Open,
            ObservationItems.Items_status.Closed => ObservationStatus.Closed,
            _ => ObservationStatus.Open
        };
    }
    
    /// <summary>
    /// Maps domain priority enum to generated priority enum.
    /// </summary>
    private static ObservationItems.Items_priority MapPriorityToGenerated(ObservationPriority priority)
    {
        return priority switch
        {
            ObservationPriority.Low => ObservationItems.Items_priority.Low,
            ObservationPriority.Medium => ObservationItems.Items_priority.Medium,
            ObservationPriority.High => ObservationItems.Items_priority.High,
            ObservationPriority.Critical => ObservationItems.Items_priority.Urgent,
            _ => ObservationItems.Items_priority.Medium
        };
    }
    
    /// <summary>
    /// Maps domain status enum to generated status enum.
    /// </summary>
    private static ObservationItems.Items_status MapStatusToGenerated(ObservationStatus status)
    {
        return status switch
        {
            ObservationStatus.Open => ObservationItems.Items_status.Initiated,
            ObservationStatus.InProgress => ObservationItems.Items_status.Ready_for_review,
            ObservationStatus.Resolved => ObservationItems.Items_status.Ready_for_review,
            ObservationStatus.Closed => ObservationItems.Items_status.Closed,
            ObservationStatus.Cancelled => ObservationItems.Items_status.Not_accepted,
            _ => ObservationItems.Items_status.Initiated
        };
    }
    
    /// <summary>
    /// Extracts project ID from the source object or returns 0 if not available.
    /// </summary>
    private static int ExtractProjectIdFromSource(ObservationItems.Items source)
    {
        // The project ID is typically available in the context but not in the item itself
        // This would need to be set from the calling context
        return 0;
    }
    
    /// <summary>
    /// Extracts category from the observation type.
    /// </summary>
    private static string ExtractCategory(ObservationItems.Items_type? type)
    {
        return type?.Category?.ToString() ?? "Quality"; // Convert enum to string
    }
    
    /// <summary>
    /// Extracts location information from the location object.
    /// </summary>
    private static string ExtractLocation(ObservationItems.Items_location? location)
    {
        return location?.Name ?? "Site";
    }
}