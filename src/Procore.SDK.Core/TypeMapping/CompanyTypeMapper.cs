using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.Core.Models;
using GeneratedCompany = Procore.SDK.Core.Rest.V10.Companies.Companies;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper Company domain model and generated Kiota Companies type.
/// Maps comprehensive company data including branding, activity status, and business experience settings.
/// </summary>
public class CompanyTypeMapper : BaseTypeMapper<Company, GeneratedCompany>
{
    /// <summary>
    /// Maps from generated Companies type to wrapper Company domain model.
    /// </summary>
    /// <param name="source">The generated Companies type to map from</param>
    /// <returns>The mapped Company domain model</returns>
    protected override Company DoMapToWrapper(GeneratedCompany source)
    {
        try
        {
            return new Company
            {
                Id = source.Id ?? 0,
                Name = source.Name ?? string.Empty,
                Description = null, // Not available in V1.0 Companies API
                IsActive = source.IsActive ?? false,
                LogoUrl = string.IsNullOrEmpty(source.LogoUrl) ? null : new Uri(source.LogoUrl),
                CreatedAt = DateTime.MinValue, // Not available in V1.0 Companies API
                UpdatedAt = DateTime.MinValue, // Not available in V1.0 Companies API
                Address = null, // Not available in V1.0 Companies API
                CustomFields = ExtractCustomFields(source.AdditionalData),
                
                // Additional properties from generated model that don't map to wrapper
                // MyCompany: source.MyCompany - indicates if this is the current user's company
                // PcnBusinessExperience: source.PcnBusinessExperience - business experience feature flag
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Companies to Company: {ex.Message}",
                ex,
                typeof(GeneratedCompany),
                typeof(Company));
        }
    }

    /// <summary>
    /// Maps from wrapper Company domain model to generated Companies type.
    /// Note: This reverse mapping is primarily for testing scenarios and create/update operations.
    /// </summary>
    /// <param name="source">The Company domain model to map from</param>
    /// <returns>The mapped Companies type</returns>
    protected override GeneratedCompany DoMapToGenerated(Company source)
    {
        try
        {
            return new GeneratedCompany
            {
                Id = source.Id != 0 ? source.Id : null,
                Name = source.Name,
                IsActive = source.IsActive,
                LogoUrl = source.LogoUrl?.ToString(),
                MyCompany = null, // This is a read-only computed field
                PcnBusinessExperience = null, // This is a system configuration field
                AdditionalData = source.CustomFields?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<string, object>()
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Company to Companies: {ex.Message}",
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

        // Filter out known system properties from the V1.0 Companies API
        var systemProperties = new HashSet<string>
        {
            "id", "name", "is_active", "logo_url", "my_company", "pcn_business_experience"
        };

        var customFields = additionalData
            .Where(kvp => !systemProperties.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return customFields.Count > 0 ? customFields : null;
    }
}