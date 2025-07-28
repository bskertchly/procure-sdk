using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.FieldProductivity.Models;
using GeneratedTimecardEntryResponse = Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timecard_entries.Item.Timecard_entriesGetResponse;

namespace Procore.SDK.FieldProductivity.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper ProductivityReport domain model and generated timecard entry response.
/// Note: This maps timecard entry data to productivity report domain model for consistency with the API.
/// </summary>
public class TimecardEntryTypeMapper : BaseTypeMapper<ProductivityReport, GeneratedTimecardEntryResponse>
{
    /// <summary>
    /// Maps from generated timecard entry response to wrapper ProductivityReport domain model.
    /// </summary>
    /// <param name="source">The generated timecard entry response to map from</param>
    /// <returns>The mapped ProductivityReport domain model</returns>
    protected override ProductivityReport DoMapToWrapper(GeneratedTimecardEntryResponse source)
    {
        try
        {
            // Extract basic information from the timecard entry
            var hoursWorked = ExtractHoursWorked(source.AdditionalData);
            var unitsCompleted = ExtractUnitsCompleted(source.AdditionalData);
            
            return new ProductivityReport
            {
                Id = ExtractIdFromAdditionalData(source.AdditionalData),
                ProjectId = ExtractProjectIdFromAdditionalData(source.AdditionalData),
                ReportDate = ExtractDateFromAdditionalData(source.AdditionalData, "date") ?? DateTime.UtcNow,
                ActivityType = ExtractStringFromAdditionalData(source.AdditionalData, "activity_type") ?? "Field Work",
                UnitsCompleted = unitsCompleted,
                HoursWorked = hoursWorked,
                ProductivityRate = hoursWorked > 0 ? unitsCompleted / hoursWorked : 0,
                CrewSize = ExtractIntFromAdditionalData(source.AdditionalData, "crew_size") ?? 1,
                Weather = ExtractStringFromAdditionalData(source.AdditionalData, "weather") ?? "Unknown",
                CreatedAt = ExtractDateFromAdditionalData(source.AdditionalData, "created_at") ?? DateTime.UtcNow,
                UpdatedAt = ExtractDateFromAdditionalData(source.AdditionalData, "updated_at") ?? DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Timecard_entriesGetResponse to ProductivityReport: {ex.Message}",
                ex,
                typeof(GeneratedTimecardEntryResponse),
                typeof(ProductivityReport));
        }
    }

    /// <summary>
    /// Maps from wrapper ProductivityReport domain model to generated timecard entry response.
    /// Note: This reverse mapping is primarily for testing scenarios and is limited.
    /// </summary>
    /// <param name="source">The ProductivityReport domain model to map from</param>
    /// <returns>The mapped Timecard_entriesGetResponse</returns>
    protected override GeneratedTimecardEntryResponse DoMapToGenerated(ProductivityReport source)
    {
        try
        {
            return new GeneratedTimecardEntryResponse
            {
                AdditionalData = new Dictionary<string, object>
                {
                    ["id"] = source.Id,
                    ["project_id"] = source.ProjectId,
                    ["date"] = source.ReportDate.ToString("yyyy-MM-dd"),
                    ["activity_type"] = source.ActivityType,
                    ["units_completed"] = source.UnitsCompleted.ToString("F2"),
                    ["hours_worked"] = source.HoursWorked.ToString("F2"),
                    ["productivity_rate"] = source.ProductivityRate.ToString("F2"),
                    ["crew_size"] = source.CrewSize,
                    ["weather"] = source.Weather,
                    ["created_at"] = source.CreatedAt.ToString("O"),
                    ["updated_at"] = source.UpdatedAt.ToString("O")
                }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map ProductivityReport to Timecard_entriesGetResponse: {ex.Message}",
                ex,
                typeof(ProductivityReport),
                typeof(GeneratedTimecardEntryResponse));
        }
    }

    /// <summary>
    /// Attempts to extract an ID from additional data if available.
    /// </summary>
    private static int ExtractIdFromAdditionalData(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null)
            return 0;

        foreach (var key in new[] { "id", "timecard_entry_id", "entry_id" })
        {
            if (additionalData.TryGetValue(key, out var value) && value != null)
            {
                if (int.TryParse(value.ToString(), out var id))
                    return id;
            }
        }

        return 0;
    }

    /// <summary>
    /// Attempts to extract a project ID from additional data if available.
    /// </summary>
    private static int ExtractProjectIdFromAdditionalData(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null)
            return 0;

        foreach (var key in new[] { "project_id", "project" })
        {
            if (additionalData.TryGetValue(key, out var value) && value != null)
            {
                // Handle nested project object
                if (value is Dictionary<string, object> projectObj)
                {
                    if (projectObj.TryGetValue("id", out var projectId) && projectId != null)
                    {
                        if (int.TryParse(projectId.ToString(), out var id))
                            return id;
                    }
                }
                else if (int.TryParse(value.ToString(), out var id))
                {
                    return id;
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// Attempts to extract hours worked from additional data if available.
    /// </summary>
    private static decimal ExtractHoursWorked(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null)
            return 0;

        foreach (var key in new[] { "hours", "hours_worked", "total_hours", "duration" })
        {
            if (additionalData.TryGetValue(key, out var value) && value != null)
            {
                if (decimal.TryParse(value.ToString(), out var hours))
                    return hours;
            }
        }

        return 8.0m; // Default to 8 hours
    }

    /// <summary>
    /// Attempts to extract units completed from additional data if available.
    /// </summary>
    private static decimal ExtractUnitsCompleted(IDictionary<string, object>? additionalData)
    {
        if (additionalData == null)
            return 0;

        foreach (var key in new[] { "units", "units_completed", "quantity", "amount" })
        {
            if (additionalData.TryGetValue(key, out var value) && value != null)
            {
                if (decimal.TryParse(value.ToString(), out var units))
                    return units;
            }
        }

        return 100.0m; // Default to 100 units
    }

    /// <summary>
    /// Attempts to extract a string value from additional data if available.
    /// </summary>
    private static string? ExtractStringFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData == null)
            return null;

        if (additionalData.TryGetValue(key, out var value) && value != null)
        {
            return value.ToString();
        }

        return null;
    }

    /// <summary>
    /// Attempts to extract an integer value from additional data if available.
    /// </summary>
    private static int? ExtractIntFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData == null)
            return null;

        if (additionalData.TryGetValue(key, out var value) && value != null)
        {
            if (int.TryParse(value.ToString(), out var intValue))
                return intValue;
        }

        return null;
    }

    /// <summary>
    /// Attempts to extract a DateTime value from additional data if available.
    /// </summary>
    private static DateTime? ExtractDateFromAdditionalData(IDictionary<string, object>? additionalData, string key)
    {
        if (additionalData == null)
            return null;

        if (additionalData.TryGetValue(key, out var value) && value != null)
        {
            if (DateTime.TryParse(value.ToString(), out var dateValue))
                return dateValue;
        }

        return null;
    }
}