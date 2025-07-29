using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedDocumentGetResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;
using GeneratedDocumentPostResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Type mapper for converting between Compliance Document domain models and generated V2.0 API responses.
/// Provides comprehensive mapping for compliance document management operations.
/// </summary>
public class ComplianceDocumentTypeMapper : BaseTypeMapper<ComplianceDocument, GeneratedDocumentGetResponse>
{
    /// <summary>
    /// Maps from generated V2.0 compliance document response to domain model.
    /// Extracts comprehensive document information including file attachments.
    /// </summary>
    /// <param name="source">The generated document response</param>
    /// <returns>The mapped ComplianceDocument domain model</returns>
    protected override ComplianceDocument DoMapToWrapper(GeneratedDocumentGetResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            var document = new ComplianceDocument
            {
                Id = ExtractIntFromData(source.Data?.AdditionalData, "id"),
                InvoiceId = ExtractIntFromData(source.Data?.AdditionalData, "invoice_id"),
                ProjectId = ExtractIntFromData(source.Data?.AdditionalData, "project_id"),
                Title = ExtractStringFromData(source.Data?.AdditionalData, "title") ?? "Compliance Document",
                Description = ExtractStringFromData(source.Data?.AdditionalData, "description") ?? string.Empty,
                DocumentType = ExtractStringFromData(source.Data?.AdditionalData, "document_type") ?? "invoice_compliance",
                Files = ExtractDocumentFiles(source.Data),
                CreatedAt = ExtractDateTimeFromData(source.Data?.AdditionalData, "created_at"),
                UpdatedAt = ExtractDateTimeFromData(source.Data?.AdditionalData, "updated_at")
            };

            return document;
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map DocumentsGetResponse to ComplianceDocument: {ex.Message}",
                ex,
                typeof(GeneratedDocumentGetResponse),
                typeof(ComplianceDocument));
        }
    }

    /// <summary>
    /// Maps from domain model to generated compliance document response.
    /// Primarily used for testing and reverse mapping scenarios.
    /// </summary>
    /// <param name="source">The ComplianceDocument domain model</param>
    /// <returns>The mapped generated response</returns>
    protected override GeneratedDocumentGetResponse DoMapToGenerated(ComplianceDocument source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new GeneratedDocumentGetResponse
            {
                Data = new Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse_data
                {
                    CreatedAt = source.CreatedAt,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["id"] = source.Id,
                        ["invoice_id"] = source.InvoiceId,
                        ["project_id"] = source.ProjectId,
                        ["title"] = source.Title,
                        ["description"] = source.Description,
                        ["document_type"] = source.DocumentType,
                        ["updated_at"] = source.UpdatedAt.ToString("O"),
                        ["file_count"] = source.Files.Count,
                        ["total_file_size"] = source.Files.Sum(f => f.FileSize)
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map ComplianceDocument to DocumentsGetResponse: {ex.Message}",
                ex,
                typeof(ComplianceDocument),
                typeof(GeneratedDocumentGetResponse));
        }
    }

    /// <summary>
    /// Maps from generated V2.0 document POST response to domain model.
    /// Used when creating new compliance documents.
    /// </summary>
    /// <param name="source">The generated document POST response</param>
    /// <returns>The mapped ComplianceDocument domain model</returns>
    public ComplianceDocument MapFromPostResponse(GeneratedDocumentPostResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new ComplianceDocument
            {
                Id = ExtractIntFromData(source.Data?.AdditionalData, "id"),
                InvoiceId = ExtractIntFromData(source.Data?.AdditionalData, "invoice_id"),
                ProjectId = ExtractIntFromData(source.Data?.AdditionalData, "project_id"),
                Title = ExtractStringFromData(source.Data?.AdditionalData, "title") ?? "Compliance Document",
                Description = ExtractStringFromData(source.Data?.AdditionalData, "description") ?? string.Empty,
                DocumentType = ExtractStringFromData(source.Data?.AdditionalData, "document_type") ?? "invoice_compliance",
                Files = ExtractDocumentFilesFromPost(source.Data),
                CreatedAt = ExtractDateTimeFromData(source.Data?.AdditionalData, "created_at"),
                UpdatedAt = ExtractDateTimeFromData(source.Data?.AdditionalData, "updated_at")
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map DocumentsPostResponse to ComplianceDocument: {ex.Message}",
                ex,
                typeof(GeneratedDocumentPostResponse),
                typeof(ComplianceDocument));
        }
    }

    /// <summary>
    /// Extracts document files from GET response data.
    /// </summary>
    private static List<ComplianceDocumentFile> ExtractDocumentFiles(
        Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse_data? data)
    {
        var files = new List<ComplianceDocumentFile>();
        
        if (data?.ComplianceDocumentProstoreFiles != null)
        {
            foreach (var fileData in data.ComplianceDocumentProstoreFiles)
            {
                if (fileData?.Data?.ProstoreFile != null)
                {
                    files.Add(new ComplianceDocumentFile
                    {
                        Id = ExtractIntFromData(fileData.Data.ProstoreFile.AdditionalData, "id"),
                        FileName = ExtractStringFromData(fileData.Data.ProstoreFile.AdditionalData, "filename") ?? "unknown.dat",
                        ContentType = ExtractStringFromData(fileData.Data.ProstoreFile.AdditionalData, "content_type") ?? "application/octet-stream",
                        FileSize = ExtractLongFromData(fileData.Data.ProstoreFile.AdditionalData, "file_size"),
                        DownloadUrl = ExtractStringFromData(fileData.Data.ProstoreFile.AdditionalData, "download_url") ?? string.Empty,
                        UploadedAt = ExtractDateTimeFromData(fileData.Data.ProstoreFile.AdditionalData, "uploaded_at")
                    });
                }
            }
        }

        return files;
    }

    /// <summary>
    /// Extracts document files from POST response data.
    /// </summary>
    private static List<ComplianceDocumentFile> ExtractDocumentFilesFromPost(
        Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse_data? data)
    {
        var files = new List<ComplianceDocumentFile>();
        
        if (data?.ComplianceDocumentProstoreFiles != null)
        {
            foreach (var fileData in data.ComplianceDocumentProstoreFiles)
            {
                if (fileData?.Data?.ProstoreFile != null)
                {
                    files.Add(new ComplianceDocumentFile
                    {
                        Id = ExtractIntFromData(fileData.Data.ProstoreFile.AdditionalData, "id"),
                        FileName = ExtractStringFromData(fileData.Data.ProstoreFile.AdditionalData, "filename") ?? "unknown.dat",
                        ContentType = ExtractStringFromData(fileData.Data.ProstoreFile.AdditionalData, "content_type") ?? "application/octet-stream",
                        FileSize = ExtractLongFromData(fileData.Data.ProstoreFile.AdditionalData, "file_size"),
                        DownloadUrl = ExtractStringFromData(fileData.Data.ProstoreFile.AdditionalData, "download_url") ?? string.Empty,
                        UploadedAt = ExtractDateTimeFromData(fileData.Data.ProstoreFile.AdditionalData, "uploaded_at")
                    });
                }
            }
        }

        return files;
    }

    /// <summary>
    /// Safely extracts a string value from additional data dictionary.
    /// </summary>
    private static string? ExtractStringFromData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true)
        {
            return value?.ToString();
        }
        return null;
    }

    /// <summary>
    /// Safely extracts an integer value from additional data dictionary.
    /// </summary>
    private static int ExtractIntFromData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (int.TryParse(value.ToString(), out var result))
                return result;
        }
        return 0;
    }

    /// <summary>
    /// Safely extracts a long value from additional data dictionary.
    /// </summary>
    private static long ExtractLongFromData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (long.TryParse(value.ToString(), out var result))
                return result;
        }
        return 0L;
    }

    /// <summary>
    /// Safely extracts a DateTime value from additional data dictionary.
    /// </summary>
    private static DateTime ExtractDateTimeFromData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (DateTime.TryParse(value.ToString(), out var result))
                return result;
        }
        return DateTime.UtcNow;
    }
}