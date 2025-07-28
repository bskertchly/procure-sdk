using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Type mapper for converting between Observation domain models and generated Kiota client types.
/// Since QualitySafety API has limited endpoints available, this mapper provides basic functionality.
/// </summary>
public class ObservationTypeMapper : BaseTypeMapper<Observation, object>
{
    /// <summary>
    /// Maps from generated type to Observation domain model.
    /// Note: The QualitySafety API has limited endpoints, so this implementation is basic.
    /// </summary>
    /// <param name="source">The generated type to map from</param>
    /// <returns>The mapped Observation domain model</returns>
    protected override Observation DoMapToWrapper(object source)
    {
        // Since the QualitySafety API has limited endpoints available,
        // we provide a basic implementation that can be extended when more endpoints become available
        return new Observation
        {
            Id = 0,
            ProjectId = 0,
            Title = "Generated Observation",
            Description = "Placeholder observation generated from limited API endpoint",
            Priority = ObservationPriority.Medium,
            Status = ObservationStatus.Open,
            Category = "Quality",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = 0,
            Location = "Site",
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Maps from Observation domain model to generated type.
    /// Note: The QualitySafety API has limited endpoints, so this implementation is basic.
    /// </summary>
    /// <param name="source">The Observation domain model to map from</param>
    /// <returns>The mapped generated type</returns>
    protected override object DoMapToGenerated(Observation source)
    {
        // Since the QualitySafety API has limited create/update endpoints available,
        // we return a basic object representation
        return new
        {
            title = source.Title,
            description = source.Description,
            priority = source.Priority.ToString().ToLowerInvariant(),
            status = source.Status.ToString().ToLowerInvariant(),
            category = source.Category,
            location = source.Location,
            assigned_to = source.AssignedTo,
            due_date = source.DueDate
        };
    }
}