using System;
using System.Collections.Generic;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedDocumentsPostResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper CostCode domain model and generated response types.
/// Provides high-performance bidirectional mapping with comprehensive validation.
/// </summary>
public class CostCodeTypeMapper : BaseTypeMapper<CostCode, GeneratedDocumentsPostResponse>
{
    /// <summary>
    /// Maps from generated DocumentsPostResponse to wrapper CostCode domain model.
    /// This demonstrates mapping financial data with precision requirements.
    /// </summary>
    /// <param name="source">The generated DocumentsPostResponse to map from</param>
    /// <returns>The mapped CostCode domain model</returns>
    protected override CostCode DoMapToWrapper(GeneratedDocumentsPostResponse source)
    {
        try
        {
            // Extract cost code data from the response's additional data
            var additionalData = source.AdditionalData ?? new Dictionary<string, object>();

            return new CostCode
            {
                Id = MapId(additionalData.ContainsKey("id") ? additionalData["id"] as int? : null),
                Code = MapString(additionalData.ContainsKey("code") ? additionalData["code"]?.ToString() : null),
                Description = MapString(additionalData.ContainsKey("description") ? additionalData["description"]?.ToString() : null),
                BudgetAmount = MapDecimalWithPrecision(additionalData.ContainsKey("budget_amount") ? additionalData["budget_amount"] : null),
                ActualAmount = MapDecimalWithPrecision(additionalData.ContainsKey("actual_amount") ? additionalData["actual_amount"] : null),
                CommittedAmount = MapDecimalWithPrecision(additionalData.ContainsKey("committed_amount") ? additionalData["committed_amount"] : null),
                CreatedAt = MapDateTime(additionalData.ContainsKey("created_at") ? additionalData["created_at"] as DateTimeOffset? : null),
                UpdatedAt = MapDateTime(additionalData.ContainsKey("updated_at") ? additionalData["updated_at"] as DateTimeOffset? : null)
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map DocumentsPostResponse to CostCode: {ex.Message}",
                ex,
                typeof(GeneratedDocumentsPostResponse),
                typeof(CostCode));
        }
    }

    /// <summary>
    /// Maps from wrapper CostCode domain model to generated DocumentsPostResponse.
    /// Preserves financial precision and handles decimal rounding appropriately.
    /// </summary>
    /// <param name="source">The CostCode domain model to map from</param>
    /// <returns>The mapped DocumentsPostResponse</returns>
    protected override GeneratedDocumentsPostResponse DoMapToGenerated(CostCode source)
    {
        try
        {
            var response = new GeneratedDocumentsPostResponse
            {
                AdditionalData = new Dictionary<string, object>
                {
                    ["id"] = source.Id,
                    ["code"] = source.Code,
                    ["description"] = source.Description,
                    ["budget_amount"] = source.BudgetAmount,
                    ["actual_amount"] = source.ActualAmount,
                    ["committed_amount"] = source.CommittedAmount,
                    ["created_at"] = source.CreatedAt,
                    ["updated_at"] = source.UpdatedAt
                }
            };

            return response;
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map CostCode to DocumentsPostResponse: {ex.Message}",
                ex,
                typeof(CostCode),
                typeof(GeneratedDocumentsPostResponse));
        }
    }

    /// <summary>
    /// Maps decimal values with financial precision requirements.
    /// Ensures proper handling of currency and financial calculations.
    /// </summary>
    private static decimal MapDecimalWithPrecision(object? source)
    {
        if (source == null)
            return 0m;

        return source switch
        {
            decimal d => Math.Round(d, 2, MidpointRounding.AwayFromZero),
            double dbl => Math.Round((decimal)dbl, 2, MidpointRounding.AwayFromZero),
            float f => Math.Round((decimal)f, 2, MidpointRounding.AwayFromZero),
            int i => (decimal)i,
            long l => (decimal)l,
            string s when decimal.TryParse(s, out var parsed) => Math.Round(parsed, 2, MidpointRounding.AwayFromZero),
            _ => 0m
        };
    }
}