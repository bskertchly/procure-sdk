using System;
using System.Collections.Generic;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedDocumentsGetResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper FinancialTransaction domain model and generated response types.
/// Provides bidirectional mapping with performance tracking and data integrity validation.
/// </summary>
public class FinancialTransactionTypeMapper : BaseTypeMapper<FinancialTransaction, GeneratedDocumentsGetResponse>
{
    /// <summary>
    /// Maps from generated DocumentsGetResponse to wrapper FinancialTransaction domain model.
    /// This is a simplified example mapper demonstrating the pattern.
    /// </summary>
    /// <param name="source">The generated DocumentsGetResponse to map from</param>
    /// <returns>The mapped FinancialTransaction domain model</returns>
    protected override FinancialTransaction DoMapToWrapper(GeneratedDocumentsGetResponse source)
    {
        try
        {
            // This is a placeholder implementation - in a real scenario, you would map
            // from the appropriate generated financial transaction response type
            var additionalData = source.AdditionalData ?? new Dictionary<string, object>();
            
            return new FinancialTransaction
            {
                Id = MapId(additionalData.ContainsKey("id") ? additionalData["id"] as int? : null),
                ProjectId = MapId(additionalData.ContainsKey("project_id") ? additionalData["project_id"] as int? : null),
                Type = MapEnum<string, TransactionType>(
                    additionalData.ContainsKey("type") ? additionalData["type"]?.ToString() : null, 
                    TransactionType.Payment),
                Amount = MapNumeric(additionalData.ContainsKey("amount") ? additionalData["amount"] as decimal? : null),
                TransactionDate = MapDateTime(additionalData.ContainsKey("transaction_date") ? additionalData["transaction_date"] as DateTimeOffset? : null),
                Description = MapString(additionalData.ContainsKey("description") ? additionalData["description"]?.ToString() : null),
                InvoiceId = additionalData.ContainsKey("invoice_id") ? additionalData["invoice_id"] as int? : null,
                Reference = MapString(additionalData.ContainsKey("reference") ? additionalData["reference"]?.ToString() : null),
                CreatedAt = MapDateTime(additionalData.ContainsKey("created_at") ? additionalData["created_at"] as DateTimeOffset? : null),
                UpdatedAt = MapDateTime(additionalData.ContainsKey("updated_at") ? additionalData["updated_at"] as DateTimeOffset? : null)
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map DocumentsGetResponse to FinancialTransaction: {ex.Message}",
                ex,
                typeof(GeneratedDocumentsGetResponse),
                typeof(FinancialTransaction));
        }
    }

    /// <summary>
    /// Maps from wrapper FinancialTransaction domain model to generated DocumentsGetResponse.
    /// This reverse mapping is primarily for testing scenarios and API integration.
    /// </summary>
    /// <param name="source">The FinancialTransaction domain model to map from</param>
    /// <returns>The mapped DocumentsGetResponse</returns>
    protected override GeneratedDocumentsGetResponse DoMapToGenerated(FinancialTransaction source)
    {
        try
        {
            var response = new GeneratedDocumentsGetResponse();
            
            // Map core properties to AdditionalData since we're using a placeholder generated type
            response.AdditionalData = new Dictionary<string, object>
            {
                ["id"] = source.Id,
                ["project_id"] = source.ProjectId,
                ["type"] = source.Type.ToString().ToLowerInvariant(),
                ["amount"] = source.Amount,
                ["transaction_date"] = source.TransactionDate,
                ["description"] = source.Description,
                ["reference"] = source.Reference,
                ["created_at"] = source.CreatedAt,
                ["updated_at"] = source.UpdatedAt
            };

            if (source.InvoiceId.HasValue)
            {
                response.AdditionalData["invoice_id"] = source.InvoiceId.Value;
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map FinancialTransaction to DocumentsGetResponse: {ex.Message}",
                ex,
                typeof(FinancialTransaction),
                typeof(GeneratedDocumentsGetResponse));
        }
    }

    /// <summary>
    /// Maps string transaction type to TransactionType enum with fallback.
    /// </summary>
    private static TransactionType MapEnum<TSource, TTarget>(TSource? source, TTarget defaultValue)
        where TSource : class
        where TTarget : struct, Enum
    {
        if (source == null)
            return (TransactionType)(object)defaultValue;

        var sourceString = source.ToString();
        if (string.IsNullOrEmpty(sourceString))
            return (TransactionType)(object)defaultValue;

        return sourceString.ToLowerInvariant() switch
        {
            "payment" => TransactionType.Payment,
            "receipt" => TransactionType.Receipt,
            "adjustment" => TransactionType.Adjustment,
            "transfer" => TransactionType.Transfer,
            "accrual" => TransactionType.Accrual,
            _ => (TransactionType)(object)defaultValue
        };
    }
}