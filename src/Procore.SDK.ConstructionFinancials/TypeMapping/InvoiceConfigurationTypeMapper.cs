using System;
using System.Collections.Generic;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedInvoiceConfigResponse = Procore.SDK.ConstructionFinancials.Rest.V10.Projects.Item.Contracts.Item.Invoice_configuration.Invoice_configurationGetResponse;
using GeneratedInvoiceConfigPatchRequest = Procore.SDK.ConstructionFinancials.Rest.V10.Projects.Item.Contracts.Item.Invoice_configuration.Invoice_configurationPatchRequestBody;
using GeneratedInvoiceConfigPatchResponse = Procore.SDK.ConstructionFinancials.Rest.V10.Projects.Item.Contracts.Item.Invoice_configuration.Invoice_configurationPatchResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Type mapper for converting between Invoice Configuration domain models and generated V1.0 API responses.
/// Provides comprehensive mapping for invoice configuration management operations.
/// </summary>
public class InvoiceConfigurationTypeMapper : BaseTypeMapper<InvoiceConfiguration, GeneratedInvoiceConfigResponse>
{
    /// <summary>
    /// Maps from generated V1.0 invoice configuration response to domain model.
    /// Extracts comprehensive invoice configuration settings including retainage rules.
    /// </summary>
    /// <param name="source">The generated invoice configuration response</param>
    /// <returns>The mapped InvoiceConfiguration domain model</returns>
    protected override InvoiceConfiguration DoMapToWrapper(GeneratedInvoiceConfigResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new InvoiceConfiguration
            {
                Id = source.Id ?? 0,
                CompanyId = source.CompanyId ?? 0,
                ProjectId = source.ProjectId ?? 0,
                ContractId = source.ContractId ?? 0,
                MoveMaterialsToPreviousWorkCompleted = source.MoveMaterialsToPreviousWorkCompleted ?? false,
                SeparateBillingForStoredMaterials = source.SeparateBillingForStoredMaterials ?? false,
                StoredMaterialsBillingMethod = source.StoredMaterialsBillingMethod ?? string.Empty,
                SlidingScaleRetainageEnabled = source.SsrEnabled ?? false,
                RetainageRuleSet = MapRetainageRuleSet(source.RetainageRuleSet),
                CreatedAt = DateTime.UtcNow, // Not available in response, using current time
                UpdatedAt = DateTime.UtcNow  // Not available in response, using current time
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Invoice_configurationGetResponse to InvoiceConfiguration: {ex.Message}",
                ex,
                typeof(GeneratedInvoiceConfigResponse),
                typeof(InvoiceConfiguration));
        }
    }

    /// <summary>
    /// Maps from domain model to generated invoice configuration response.
    /// Primarily used for testing and reverse mapping scenarios.
    /// </summary>
    /// <param name="source">The InvoiceConfiguration domain model</param>
    /// <returns>The mapped generated response</returns>
    protected override GeneratedInvoiceConfigResponse DoMapToGenerated(InvoiceConfiguration source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new GeneratedInvoiceConfigResponse
            {
                Id = source.Id,
                CompanyId = source.CompanyId,
                ProjectId = source.ProjectId,
                ContractId = source.ContractId,
                MoveMaterialsToPreviousWorkCompleted = source.MoveMaterialsToPreviousWorkCompleted,
                SeparateBillingForStoredMaterials = source.SeparateBillingForStoredMaterials,
                StoredMaterialsBillingMethod = source.StoredMaterialsBillingMethod,
                SsrEnabled = source.SlidingScaleRetainageEnabled,
                AdditionalData = new Dictionary<string, object>
                {
                    ["created_at"] = source.CreatedAt.ToString("O"),
                    ["updated_at"] = source.UpdatedAt.ToString("O")
                }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map InvoiceConfiguration to Invoice_configurationGetResponse: {ex.Message}",
                ex,
                typeof(InvoiceConfiguration),
                typeof(GeneratedInvoiceConfigResponse));
        }
    }

    /// <summary>
    /// Maps domain model to V1.0 PATCH request body for invoice configuration updates.
    /// </summary>
    /// <param name="source">The InvoiceConfiguration domain model to map from</param>
    /// <returns>The mapped PATCH request body</returns>
    public GeneratedInvoiceConfigPatchRequest MapToPatchRequest(InvoiceConfiguration source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            var patchRequestBody = new GeneratedInvoiceConfigPatchRequest
            {
                AdditionalData = new Dictionary<string, object>()
            };

            // Create the invoice_configuration nested object
            var invoiceConfigData = new Dictionary<string, object>
            {
                ["move_materials_to_previous_work_completed"] = source.MoveMaterialsToPreviousWorkCompleted,
                ["separate_billing_for_stored_materials"] = source.SeparateBillingForStoredMaterials,
                ["stored_materials_billing_method"] = source.StoredMaterialsBillingMethod,
                ["ssr_enabled"] = source.SlidingScaleRetainageEnabled
            };

            patchRequestBody.AdditionalData["invoice_configuration"] = invoiceConfigData;
            
            return patchRequestBody;
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map InvoiceConfiguration to Invoice_configurationPatchRequestBody: {ex.Message}",
                ex,
                typeof(InvoiceConfiguration),
                typeof(GeneratedInvoiceConfigPatchRequest));
        }
    }

    /// <summary>
    /// Maps from generated V1.0 PATCH response to domain model.
    /// </summary>
    /// <param name="source">The generated PATCH response</param>
    /// <returns>The mapped InvoiceConfiguration domain model</returns>
    public InvoiceConfiguration MapFromPatchResponse(GeneratedInvoiceConfigPatchResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new InvoiceConfiguration
            {
                Id = source.Id ?? 0,
                CompanyId = source.CompanyId ?? 0,
                ProjectId = source.ProjectId ?? 0,
                ContractId = source.ContractId ?? 0,
                MoveMaterialsToPreviousWorkCompleted = source.MoveMaterialsToPreviousWorkCompleted ?? false,
                SeparateBillingForStoredMaterials = source.SeparateBillingForStoredMaterials ?? false,
                StoredMaterialsBillingMethod = source.StoredMaterialsBillingMethod ?? string.Empty,
                SlidingScaleRetainageEnabled = source.SsrEnabled ?? false,
                RetainageRuleSet = MapRetainageRuleSetFromPatch(source.RetainageRuleSet),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Invoice_configurationPatchResponse to InvoiceConfiguration: {ex.Message}",
                ex,
                typeof(GeneratedInvoiceConfigPatchResponse),
                typeof(InvoiceConfiguration));
        }
    }

    /// <summary>
    /// Maps retainage rule set from GET response.
    /// </summary>
    private static RetainageRuleSet? MapRetainageRuleSet(
        Procore.SDK.ConstructionFinancials.Rest.V10.Projects.Item.Contracts.Item.Invoice_configuration.Invoice_configurationGetResponse_retainage_rule_set? source)
    {
        if (source == null)
            return null;

        var rules = new List<RetainageRule>();
        if (source.RetainageRules != null)
        {
            foreach (var rule in source.RetainageRules)
            {
                if (rule != null)
                {
                    rules.Add(new RetainageRule
                    {
                        Id = ExtractIntFromAdditionalData(rule.AdditionalData, "id"),
                        RuleType = rule.RuleType?.ToString() ?? string.Empty,
                        Percentage = ExtractDecimalFromAdditionalData(rule.AdditionalData, "percentage"),
                        ThresholdAmount = ExtractDecimalFromAdditionalData(rule.AdditionalData, "threshold_amount"),
                        IsActive = ExtractBoolFromAdditionalData(rule.AdditionalData, "is_active")
                    });
                }
            }
        }

        return new RetainageRuleSet
        {
            Id = ExtractIntFromAdditionalData(source.AdditionalData, "id"),
            Rules = rules,
            IsActive = ExtractBoolFromAdditionalData(source.AdditionalData, "is_active")
        };
    }

    /// <summary>
    /// Maps retainage rule set from PATCH response.
    /// </summary>
    private static RetainageRuleSet? MapRetainageRuleSetFromPatch(
        Procore.SDK.ConstructionFinancials.Rest.V10.Projects.Item.Contracts.Item.Invoice_configuration.Invoice_configurationPatchResponse_retainage_rule_set? source)
    {
        if (source == null)
            return null;

        var rules = new List<RetainageRule>();
        if (source.RetainageRules != null)
        {
            foreach (var rule in source.RetainageRules)
            {
                if (rule != null)
                {
                    rules.Add(new RetainageRule
                    {
                        Id = ExtractIntFromAdditionalData(rule.AdditionalData, "id"),
                        RuleType = rule.RuleType?.ToString() ?? string.Empty,
                        Percentage = ExtractDecimalFromAdditionalData(rule.AdditionalData, "percentage"),
                        ThresholdAmount = ExtractDecimalFromAdditionalData(rule.AdditionalData, "threshold_amount"),
                        IsActive = ExtractBoolFromAdditionalData(rule.AdditionalData, "is_active")
                    });
                }
            }
        }

        return new RetainageRuleSet
        {
            Id = ExtractIntFromAdditionalData(source.AdditionalData, "id"),
            Rules = rules,
            IsActive = ExtractBoolFromAdditionalData(source.AdditionalData, "is_active")
        };
    }

    /// <summary>
    /// Safely extracts an integer value from additional data dictionary.
    /// </summary>
    private static int ExtractIntFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (int.TryParse(value.ToString(), out var result))
                return result;
        }
        return 0;
    }

    /// <summary>
    /// Safely extracts a decimal value from additional data dictionary.
    /// </summary>
    private static decimal ExtractDecimalFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (decimal.TryParse(value.ToString(), out var result))
                return result;
        }
        return 0m;
    }

    /// <summary>
    /// Safely extracts a boolean value from additional data dictionary.
    /// </summary>
    private static bool ExtractBoolFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (bool.TryParse(value.ToString(), out var result))
                return result;
        }
        return false;
    }
}