using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ResourceManagement.Models;
using Generated = Procore.SDK.ResourceManagement.Rest.V11.Projects.Item.Schedule.Resources.Item;

namespace Procore.SDK.ResourceManagement.TypeMapping;

/// <summary>
/// Type mapper for converting between Resource domain models and generated Kiota client types.
/// Handles mapping between ResourceManagement.Models.Resource and generated ResourcesGetResponse.
/// </summary>
public class ResourceTypeMapper : BaseTypeMapper<Resource, Generated.ResourcesGetResponse>
{
    /// <summary>
    /// Maps from generated Kiota client type to Resource domain model.
    /// </summary>
    /// <param name="source">The generated ResourcesGetResponse to map from</param>
    /// <returns>The mapped Resource domain model</returns>
    protected override Resource DoMapToWrapper(Generated.ResourcesGetResponse source)
    {
        return new Resource
        {
            Id = source.Id ?? 0,
            Name = source.Name ?? string.Empty,
            Type = ResourceType.Equipment, // Default since API doesn't provide type field
            Status = ResourceStatus.Available, // Default since API doesn't provide status field
            Description = source.Name ?? string.Empty, // Use name as description fallback
            CostPerHour = 0m, // Not available in generated model
            Location = string.Empty, // Not available in generated model
            AvailableFrom = DateTime.UtcNow, // Default availability
            AvailableTo = DateTime.UtcNow.AddYears(1), // Default availability
            CreatedAt = DateTime.UtcNow, // Not available in generated model
            UpdatedAt = DateTime.UtcNow, // Not available in generated model
            CompanyId = source.CompanyId,
            ProjectId = source.ProjectId,
            SourceUid = source.SourceUid,
            ScheduleAttributes = source.ScheduleAttributes?.AdditionalData,
            DeletedAt = MapNullableDateTime(source.DeletedAt)
        };
    }

    /// <summary>
    /// Maps from Resource domain model to generated Kiota client type.
    /// </summary>
    /// <param name="source">The Resource domain model to map from</param>
    /// <returns>The mapped generated ResourcesGetResponse</returns>
    protected override Generated.ResourcesGetResponse DoMapToGenerated(Resource source)
    {
        return new Generated.ResourcesGetResponse
        {
            Id = source.Id,
            Name = source.Name,
            CompanyId = source.CompanyId,
            ProjectId = source.ProjectId,
            SourceUid = source.SourceUid,
            DeletedAt = source.DeletedAt.HasValue ? new DateTimeOffset(source.DeletedAt.Value) : null
        };
    }
}