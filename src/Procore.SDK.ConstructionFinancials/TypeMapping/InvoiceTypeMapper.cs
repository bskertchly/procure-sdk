using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedDocumentResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper Invoice domain model and generated compliance document response.
/// Note: This is a simplified mapping as the available endpoints primarily deal with compliance documents
/// rather than full invoice data. In a complete implementation, this would map to proper invoice endpoints.
/// </summary>
public class InvoiceTypeMapper : BaseTypeMapper<Invoice, GeneratedDocumentResponse>
{
    /// <summary>
    /// Maps from generated compliance document response to wrapper Invoice domain model.
    /// Note: This is a limited mapping due to available endpoint constraints.
    /// </summary>
    /// <param name="source">The generated document response to map from</param>
    /// <returns>The mapped Invoice domain model</returns>
    protected override Invoice DoMapToWrapper(GeneratedDocumentResponse source)
    {
        try
        {
            // This is a placeholder mapping since compliance documents don't contain full invoice data
            // In a real implementation, this would map from a proper invoice endpoint response
            return new Invoice
            {
                Id = ExtractIdFromAdditionalData(source.AdditionalData),
                ProjectId = 0, // Would need to be extracted from context or additional data
                InvoiceNumber = "PLACEHOLDER", // Not available in compliance document response
                Amount = 0m, // Not available in compliance document response
                Status = InvoiceStatus.Submitted, // Default status assumption
                InvoiceDate = DateTime.UtcNow, // Default to current date
                DueDate = null,
                VendorId = 0, // Not available in compliance document response
                Description = "Invoice with compliance documents",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map DocumentsGetResponse to Invoice: {ex.Message}",
                ex,
                typeof(GeneratedDocumentResponse),
                typeof(Invoice));
        }
    }

    /// <summary>
    /// Maps from wrapper Invoice domain model to generated compliance document response.
    /// Note: This reverse mapping is primarily for testing scenarios and is limited.
    /// </summary>
    /// <param name="source">The Invoice domain model to map from</param>
    /// <returns>The mapped DocumentsGetResponse</returns>
    protected override GeneratedDocumentResponse DoMapToGenerated(Invoice source)
    {
        try
        {
            // This is a placeholder mapping as we can't create meaningful compliance document data
            // from invoice data without additional context
            return new GeneratedDocumentResponse
            {
                AdditionalData = new Dictionary<string, object>
                {
                    ["invoice_id"] = source.Id,
                    ["invoice_number"] = source.InvoiceNumber,
                    ["amount"] = source.Amount.ToString("F2"),
                    ["status"] = source.Status.ToString()
                }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Invoice to DocumentsGetResponse: {ex.Message}",
                ex,
                typeof(Invoice),
                typeof(GeneratedDocumentResponse));
        }
    }

    /// <summary>
    /// Attempts to extract an ID from additional data if available.
    /// </summary>
    private static int ExtractIdFromAdditionalData(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null)
            return 0;

        // Try to find an ID field in various possible formats
        foreach (var key in new[] { "id", "invoice_id", "document_id" })
        {
            if (additionalData.TryGetValue(key, out var value) && value != null)
            {
                if (int.TryParse(value.ToString(), out var id))
                    return id;
            }
        }

        return 0;
    }
}