using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.Core.Models;
using GeneratedUser = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse;
using GeneratedVendor = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse_vendor;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper User domain model and generated Kiota UsersGetResponse type.
/// Maps comprehensive user data including contact information, permissions, and vendor associations.
/// </summary>
public class UserTypeMapper : BaseTypeMapper<User, GeneratedUser>
{
    /// <summary>
    /// Maps from generated UsersGetResponse to wrapper User domain model.
    /// </summary>
    /// <param name="source">The generated UsersGetResponse to map from</param>
    /// <returns>The mapped User domain model</returns>
    protected override User DoMapToWrapper(GeneratedUser source)
    {
        try
        {
            return new User
            {
                Id = source.Id ?? 0,
                Email = source.EmailAddress ?? string.Empty,
                FirstName = source.FirstName ?? string.Empty,
                LastName = source.LastName ?? string.Empty,
                JobTitle = source.JobTitle,
                IsActive = source.IsActive ?? false,
                CreatedAt = MapDateTime(source.CreatedAt),
                UpdatedAt = MapDateTime(source.UpdatedAt),
                LastSignInAt = source.LastLoginAt?.DateTime,
                AvatarUrl = string.IsNullOrEmpty(source.Avatar) ? null : new Uri(source.Avatar),
                PhoneNumber = MapPhoneNumber(source.BusinessPhone, source.MobilePhone),
                Company = MapVendorToCompany(source.Vendor),
                CustomFields = ExtractCustomFields(source.AdditionalData)
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map UsersGetResponse to User: {ex.Message}",
                ex,
                typeof(GeneratedUser),
                typeof(User));
        }
    }

    /// <summary>
    /// Maps from wrapper User domain model to generated UsersGetResponse.
    /// Note: This reverse mapping is primarily for testing scenarios.
    /// </summary>
    /// <param name="source">The User domain model to map from</param>
    /// <returns>The mapped UsersGetResponse</returns>
    protected override GeneratedUser DoMapToGenerated(User source)
    {
        try
        {
            return new GeneratedUser
            {
                Id = source.Id,
                EmailAddress = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Name = $"{source.FirstName} {source.LastName}".Trim(),
                JobTitle = source.JobTitle,
                IsActive = source.IsActive,
                CreatedAt = source.CreatedAt != DateTime.MinValue ? new DateTimeOffset(source.CreatedAt) : null,
                UpdatedAt = source.UpdatedAt != DateTime.MinValue ? new DateTimeOffset(source.UpdatedAt) : null,
                LastLoginAt = source.LastSignInAt.HasValue ? new DateTimeOffset(source.LastSignInAt.Value) : null,
                Avatar = source.AvatarUrl?.ToString(),
                BusinessPhone = source.PhoneNumber, // Primary phone mapping
                MobilePhone = null, // Would need additional field in domain model to separate
                Vendor = MapCompanyToVendor(source.Company),
                Address = source.Company?.Address?.Street1,
                City = source.Company?.Address?.City,
                StateCode = source.Company?.Address?.State,
                CountryCode = source.Company?.Address?.Country,
                Zip = source.Company?.Address?.PostalCode
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map User to UsersGetResponse: {ex.Message}",
                ex,
                typeof(User),
                typeof(GeneratedUser));
        }
    }

    /// <summary>
    /// Maps phone numbers, prioritizing business phone over mobile.
    /// </summary>
    private static string? MapPhoneNumber(string? businessPhone, string? mobilePhone)
    {
        return !string.IsNullOrEmpty(businessPhone) ? businessPhone : mobilePhone;
    }

    /// <summary>
    /// Maps generated vendor to wrapper Company (representing the user's company association).
    /// </summary>
    private static Company? MapVendorToCompany(GeneratedVendor? vendor)
    {
        if (vendor == null)
            return null;

        return new Company
        {
            Id = vendor.Id ?? 0,
            Name = vendor.Name ?? string.Empty,
            IsActive = true, // Default assumption for vendor companies
            CreatedAt = DateTime.UtcNow, // Not available in vendor data
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Maps wrapper Company to generated vendor structure.
    /// </summary>
    private static GeneratedVendor? MapCompanyToVendor(Company? company)
    {
        if (company == null)
            return null;

        return new GeneratedVendor
        {
            Id = company.Id,
            Name = company.Name
        };
    }

    /// <summary>
    /// Extracts custom fields from additional data, filtering out system properties.
    /// </summary>
    private static Dictionary<string, object>? ExtractCustomFields(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null || additionalData.Count == 0)
            return null;

        // Filter out known system properties, keeping only actual custom fields
        var systemProperties = new HashSet<string>
        {
            "address", "avatar", "business_id", "business_phone", "business_phone_extension",
            "city", "company_permission_template_id", "country_code", "created_at",
            "default_permission_template_id", "email_address", "email_signature",
            "employee_id", "fax_number", "first_name", "id", "initials", "is_active",
            "is_employee", "is_insurance_manager", "job_title", "last_login_at",
            "last_name", "locale", "mobile_phone", "name", "notes", "origin_data",
            "origin_id", "state_code", "updated_at", "vendor", "work_classification_id", "zip"
        };

        var customFields = additionalData
            .Where(kvp => !systemProperties.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return customFields.Count > 0 ? customFields : null;
    }
}