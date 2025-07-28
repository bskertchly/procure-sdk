using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.Core.Models;
using GeneratedFile = Procore.SDK.Core.Rest.V10.Companies.Item.Files.Item.FilesGetResponse;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper Document domain model and generated Kiota FilesGetResponse type.
/// Maps comprehensive file/document data including metadata, versions, and custom fields.
/// </summary>
public class DocumentTypeMapper : BaseTypeMapper<Document, GeneratedFile>
{
    /// <summary>
    /// Maps from generated FilesGetResponse to wrapper Document domain model.
    /// </summary>
    /// <param name="source">The generated FilesGetResponse to map from</param>
    /// <returns>The mapped Document domain model</returns>
    protected override Document DoMapToWrapper(GeneratedFile source)
    {
        try
        {
            return new Document
            {
                Id = source.Id ?? 0,
                Name = source.Name ?? string.Empty,
                FileName = source.Name ?? string.Empty, // Use Name as FileName since they're equivalent
                Description = source.Description,
                FileUrl = string.Empty, // Direct download URL not available in standard Files response
                ContentType = source.FileType ?? "application/octet-stream",
                FileSize = source.Size ?? 0,
                IsPrivate = source.Private ?? false,
                CreatedAt = MapDateTime(source.CreatedAt),
                UpdatedAt = MapDateTime(source.UpdatedAt),
                CustomFields = ExtractCustomFields(source.AdditionalData),
                
                // Additional properties from generated model that could be useful:
                // LegacyId: source.LegacyId - for migration scenarios
                // IsDeleted: source.IsDeleted - recycle bin status
                // IsTracked: source.IsTracked - document tracking status
                // CheckedOutBy: source.CheckedOutBy - check-out information
                // CheckedOutUntil: source.CheckedOutUntil - check-out expiration
                // FileVersions: source.FileVersions - version history
                // ParentId: source.ParentId - folder structure
                // NameWithPath: source.NameWithPath - full path context
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map FilesGetResponse to Document: {ex.Message}",
                ex,
                typeof(GeneratedFile),
                typeof(Document));
        }
    }

    /// <summary>
    /// Maps from wrapper Document domain model to generated FilesGetResponse.
    /// Note: This reverse mapping is primarily for testing scenarios.
    /// Most file operations use different request/response types (POST, PATCH).
    /// </summary>
    /// <param name="source">The Document domain model to map from</param>
    /// <returns>The mapped FilesGetResponse</returns>
    protected override GeneratedFile DoMapToGenerated(Document source)
    {
        try
        {
            return new GeneratedFile
            {
                Id = source.Id != 0 ? source.Id : null,
                Name = source.Name,
                Description = source.Description,
                FileType = source.ContentType,
                Size = source.FileSize != 0 ? (int)source.FileSize : null,
                Private = source.IsPrivate,
                CreatedAt = source.CreatedAt != DateTime.MinValue ? new DateTimeOffset(source.CreatedAt) : null,
                UpdatedAt = source.UpdatedAt != DateTime.MinValue ? new DateTimeOffset(source.UpdatedAt) : null,
                IsDeleted = false, // Documents in wrapper model are assumed to be active
                IsTracked = false, // Default tracking status
                LegacyId = null, // Not applicable for new documents
                ParentId = null, // Folder structure not modeled in wrapper
                NameWithPath = source.Name, // Use name as path for simple mapping
                CheckedOutBy = null, // Check-out state not modeled in wrapper
                CheckedOutUntil = null,
                FileVersions = null, // Version history not modeled in wrapper
                TrackedFolder = null, // Folder tracking not modeled in wrapper
                CustomFields = null, // Custom fields structure is complex in generated model
                AdditionalData = source.CustomFields?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<string, object>()
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Document to FilesGetResponse: {ex.Message}",
                ex,
                typeof(Document),
                typeof(GeneratedFile));
        }
    }

    /// <summary>
    /// Extracts custom fields from additional data, filtering out known system properties.
    /// </summary>
    private static Dictionary<string, object>? ExtractCustomFields(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null || additionalData.Count == 0)
            return null;

        // Filter out known system properties from the V1.0 Files API
        var systemProperties = new HashSet<string>
        {
            "id", "name", "description", "file_type", "size", "private", "created_at", 
            "updated_at", "is_deleted", "is_tracked", "legacy_id", "parent_id", 
            "name_with_path", "checked_out_by", "checked_out_until", "file_versions", 
            "tracked_folder", "custom_fields"
        };

        var customFields = additionalData
            .Where(kvp => !systemProperties.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return customFields.Count > 0 ? customFields : null;
    }
}