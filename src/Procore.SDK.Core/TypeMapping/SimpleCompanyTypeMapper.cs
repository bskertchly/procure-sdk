using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.Core.Models;
using GeneratedCompany = Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.Item.InstancesGetResponse_data_workflow_manager_company;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper Company domain model and simple generated company types.
/// This is designed for simple company representations that contain minimal fields (name only).
/// For more comprehensive company mappings, specialized mappers should be created.
/// </summary>
public class SimpleCompanyTypeMapper : BaseTypeMapper<Company, GeneratedCompany>
{
    /// <summary>
    /// Maps from generated simple company type to wrapper Company domain model.
    /// </summary>
    /// <param name="source">The generated company type to map from</param>
    /// <returns>The mapped Company domain model</returns>
    protected override Company DoMapToWrapper(GeneratedCompany source)
    {
        try
        {
            return new Company
            {
                Id = 0, // Simple company types typically don't include ID
                Name = source.Name ?? string.Empty,
                Description = null,
                IsActive = true, // Default assumption for referenced companies
                CreatedAt = DateTime.UtcNow, // Not available in simple types
                UpdatedAt = DateTime.UtcNow,
                LogoUrl = null,
                Address = null,
                CustomFields = ExtractCustomFields(source.AdditionalData)
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map simple company type to Company: {ex.Message}",
                ex,
                typeof(GeneratedCompany),
                typeof(Company));
        }
    }

    /// <summary>
    /// Maps from wrapper Company domain model to generated simple company type.
    /// Note: Only name is preserved in simple company types.
    /// </summary>
    /// <param name="source">The Company domain model to map from</param>
    /// <returns>The mapped simple company type</returns>
    protected override GeneratedCompany DoMapToGenerated(Company source)
    {
        try
        {
            return new GeneratedCompany
            {
                Name = source.Name
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Company to simple company type: {ex.Message}",
                ex,
                typeof(Company),
                typeof(GeneratedCompany));
        }
    }

    /// <summary>
    /// Extracts custom fields from additional data, filtering out known system properties.
    /// </summary>
    private static Dictionary<string, object>? ExtractCustomFields(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null || additionalData.Count == 0)
            return null;

        // For simple company types, only "name" is a known system property
        var systemProperties = new HashSet<string> { "name" };

        var customFields = additionalData
            .Where(kvp => !systemProperties.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return customFields.Count > 0 ? customFields : null;
    }
}