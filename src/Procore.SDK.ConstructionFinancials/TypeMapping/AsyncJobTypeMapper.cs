using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedAsyncJobResponse = Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Type mapper for converting between Async Job domain models and generated V1.0 API responses.
/// Provides comprehensive mapping for async job monitoring and processing operations.
/// </summary>
public class AsyncJobTypeMapper : BaseTypeMapper<AsyncJob, GeneratedAsyncJobResponse>
{
    /// <summary>
    /// Maps from generated V1.0 async job response to domain model.
    /// Extracts comprehensive job status, progress, and result information.
    /// </summary>
    /// <param name="source">The generated async job response</param>
    /// <returns>The mapped AsyncJob domain model</returns>
    protected override AsyncJob DoMapToWrapper(GeneratedAsyncJobResponse source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new AsyncJob
            {
                Uuid = ExtractStringFromAdditionalData(source.AdditionalData, "uuid") ?? string.Empty,
                CompanyId = ExtractIntFromAdditionalData(source.AdditionalData, "company_id"),
                Status = MapAsyncJobStatus(source.Status),
                JobType = ExtractStringFromAdditionalData(source.AdditionalData, "job_type") ?? "invoice_processing",
                Result = MapAsyncJobResult(source.Result),
                CreatedAt = ExtractDateTimeFromAdditionalData(source.AdditionalData, "created_at"),
                UpdatedAt = ExtractDateTimeFromAdditionalData(source.AdditionalData, "updated_at"),
                ErrorMessage = ExtractStringFromAdditionalData(source.AdditionalData, "error_message"),
                ProgressPercentage = ExtractNullableIntFromAdditionalData(source.AdditionalData, "progress_percentage")
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map WithUuGetResponse to AsyncJob: {ex.Message}",
                ex,
                typeof(GeneratedAsyncJobResponse),
                typeof(AsyncJob));
        }
    }

    /// <summary>
    /// Maps from domain model to generated async job response.
    /// Primarily used for testing and reverse mapping scenarios.
    /// </summary>
    /// <param name="source">The AsyncJob domain model</param>
    /// <returns>The mapped generated response</returns>
    protected override GeneratedAsyncJobResponse DoMapToGenerated(AsyncJob source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        try
        {
            return new GeneratedAsyncJobResponse
            {
                Status = MapAsyncJobStatusToGenerated(source.Status),
                Result = MapAsyncJobResultToGenerated(source.Result),
                AdditionalData = new Dictionary<string, object>
                {
                    ["uuid"] = source.Uuid,
                    ["company_id"] = source.CompanyId,
                    ["job_type"] = source.JobType,
                    ["created_at"] = source.CreatedAt.ToString("O"),
                    ["updated_at"] = source.UpdatedAt.ToString("O"),
                    ["error_message"] = source.ErrorMessage ?? string.Empty,
                    ["progress_percentage"] = source.ProgressPercentage ?? 0
                }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map AsyncJob to WithUuGetResponse: {ex.Message}",
                ex,
                typeof(AsyncJob),
                typeof(GeneratedAsyncJobResponse));
        }
    }

    /// <summary>
    /// Maps async job status from generated enum to domain enum.
    /// </summary>
    private static AsyncJobStatus MapAsyncJobStatus(
        Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status? status)
    {
        return status switch
        {
            Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Pending => AsyncJobStatus.Pending,
            Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.In_progress => AsyncJobStatus.InProgress,
            Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Completed => AsyncJobStatus.Completed,
            Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Failed => AsyncJobStatus.Failed,
            _ => AsyncJobStatus.Pending
        };
    }

    /// <summary>
    /// Maps domain async job status to generated enum.
    /// </summary>
    private static Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status MapAsyncJobStatusToGenerated(AsyncJobStatus status)
    {
        return status switch
        {
            AsyncJobStatus.Pending => Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Pending,
            AsyncJobStatus.InProgress => Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.In_progress,
            AsyncJobStatus.Completed => Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Completed,
            AsyncJobStatus.Failed => Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Failed,
            AsyncJobStatus.Cancelled => Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Failed, // Map cancelled to failed
            _ => Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_status.Pending
        };
    }

    /// <summary>
    /// Maps async job result from generated type to domain model.
    /// </summary>
    private static AsyncJobResult? MapAsyncJobResult(
        Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_result? result)
    {
        if (result == null || result.AdditionalData == null)
            return null;

        var domainResult = new AsyncJobResult
        {
            TotalRecords = ExtractIntFromAdditionalData(result.AdditionalData, "total_records"),
            ProcessedRecords = ExtractIntFromAdditionalData(result.AdditionalData, "processed_records"),
            FailedRecords = ExtractIntFromAdditionalData(result.AdditionalData, "failed_records"),
            Errors = new List<string>(),
            Data = new Dictionary<string, object>()
        };

        // Extract errors if present
        if (result.AdditionalData.TryGetValue("errors", out var errorsObj) && errorsObj is IEnumerable<object> errors)
        {
            domainResult.Errors = errors.Select(e => e?.ToString() ?? string.Empty).ToList();
        }

        // Extract additional data
        foreach (var kvp in result.AdditionalData.Where(kvp => !IsKnownResultField(kvp.Key)))
        {
            domainResult.Data[kvp.Key] = kvp.Value;
        }

        return domainResult;
    }

    /// <summary>
    /// Maps domain async job result to generated type.
    /// </summary>
    private static Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_result? MapAsyncJobResultToGenerated(AsyncJobResult? result)
    {
        if (result == null)
            return null;

        var additionalData = new Dictionary<string, object>
        {
            ["total_records"] = result.TotalRecords,
            ["processed_records"] = result.ProcessedRecords,
            ["failed_records"] = result.FailedRecords,
            ["errors"] = result.Errors
        };

        // Add additional data
        foreach (var kvp in result.Data)
        {
            additionalData[kvp.Key] = kvp.Value;
        }

        return new Procore.SDK.ConstructionFinancials.Rest.V10.Companies.Item.Invoices.Async_jobs.Item.WithUuGetResponse_result
        {
            AdditionalData = additionalData
        };
    }

    /// <summary>
    /// Checks if a field is a known result field that shouldn't be added to additional data.
    /// </summary>
    private static bool IsKnownResultField(string key)
    {
        return key switch
        {
            "total_records" or "processed_records" or "failed_records" or "errors" => true,
            _ => false
        };
    }

    /// <summary>
    /// Safely extracts a string value from additional data dictionary.
    /// </summary>
    private static string? ExtractStringFromAdditionalData(IDictionary<string, object>? additionalData, string key)
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
    /// Safely extracts a nullable integer value from additional data dictionary.
    /// </summary>
    private static int? ExtractNullableIntFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (int.TryParse(value.ToString(), out var result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Safely extracts a DateTime value from additional data dictionary.
    /// </summary>
    private static DateTime ExtractDateTimeFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData?.TryGetValue(key, out var value) == true && value != null)
        {
            if (DateTime.TryParse(value.ToString(), out var result))
                return result;
        }
        return DateTime.UtcNow;
    }
}