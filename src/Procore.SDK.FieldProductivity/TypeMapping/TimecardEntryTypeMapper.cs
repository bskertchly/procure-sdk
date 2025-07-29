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
    /// Enhanced with comprehensive field mapping and validation.
    /// </summary>
    /// <param name="source">The generated timecard entry response to map from</param>
    /// <returns>The mapped ProductivityReport domain model</returns>
    protected override ProductivityReport DoMapToWrapper(GeneratedTimecardEntryResponse source)
    {
        try
        {
            // Extract comprehensive information from the timecard entry using both structured and additional data
            var hoursWorked = ExtractDecimalFromSource(source, "hours", "hours_worked", "total_hours", "duration") ?? 
                             (decimal.TryParse(source.Hours, out var parsedHours) ? parsedHours : 8.0m);
            
            var unitsCompleted = ExtractDecimalFromSource(source, "units", "units_completed", "quantity", "amount") ?? 100.0m;
            
            var projectId = ExtractProjectId(source);
            var activityType = ExtractActivityType(source);
            var crewSize = ExtractCrewSize(source);
            
            return new ProductivityReport
            {
                Id = ExtractId(source),
                ProjectId = projectId,
                ReportDate = ConvertDateToDateTime(source.Date) ?? source.CreatedAt?.DateTime ?? DateTime.UtcNow,
                ActivityType = activityType,
                UnitsCompleted = unitsCompleted,
                HoursWorked = hoursWorked,
                ProductivityRate = hoursWorked > 0 ? unitsCompleted / hoursWorked : 0,
                CrewSize = crewSize,
                Weather = ExtractWeather(source),
                CreatedAt = source.CreatedAt?.DateTime ?? DateTime.UtcNow,
                UpdatedAt = source.UpdatedAt?.DateTime ?? DateTime.UtcNow
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
    /// Extracts ID from timecard entry response with fallback strategies.
    /// </summary>
    private static int ExtractId(GeneratedTimecardEntryResponse source)
    {
        if (source.Id.HasValue)
            return source.Id.Value;
            
        return ExtractIntFromAdditionalData(source.AdditionalData, "id") ?? 0;
    }
    
    /// <summary>
    /// Extracts project ID from timecard entry with multiple fallback strategies.
    /// </summary>
    private static int ExtractProjectId(GeneratedTimecardEntryResponse source)
    {
        // Try project object first
        if (source.Project?.Id.HasValue == true)
            return source.Project.Id.Value;
            
        // Sub job doesn't directly have project reference in this API version
            
        // Fallback to additional data
        return ExtractProjectIdFromAdditionalData(source.AdditionalData);
    }
    
    /// <summary>
    /// Extracts activity type from cost code or timecard time type.
    /// </summary>
    private static string ExtractActivityType(GeneratedTimecardEntryResponse source)
    {
        // Try cost code name first
        if (!string.IsNullOrEmpty(source.CostCode?.Name))
            return source.CostCode.Name;
            
        // Try timecard time type
        if (!string.IsNullOrEmpty(source.TimecardTimeType?.TimeType))
            return source.TimecardTimeType.TimeType;
            
        // Try description
        if (!string.IsNullOrEmpty(source.Description))
            return source.Description;
            
        return "Field Work";
    }
    
    /// <summary>
    /// Extracts crew size from crew information or defaults.
    /// </summary>
    private static int ExtractCrewSize(GeneratedTimecardEntryResponse source)
    {
        // Try crew size from crew object
        if (source.Crew?.Id.HasValue == true)
            return 1; // At least one person if crew is specified
            
        return ExtractIntFromAdditionalData(source.AdditionalData, "crew_size") ?? 1;
    }
    
    /// <summary>
    /// Extracts weather information from custom fields or additional data.
    /// </summary>
    private static string ExtractWeather(GeneratedTimecardEntryResponse source)
    {
        // Try custom fields for weather information
        if (source.CustomFields?.AdditionalData != null)
        {
            var weather = ExtractStringFromAdditionalData(source.CustomFields.AdditionalData, "weather");
            if (!string.IsNullOrEmpty(weather))
                return weather;
        }
        
        // Fallback to additional data
        return ExtractStringFromAdditionalData(source.AdditionalData, "weather") ?? "Unknown";
    }
    
    /// <summary>
    /// Extracts decimal values from timecard entry with multiple key attempts.
    /// </summary>
    private static decimal? ExtractDecimalFromSource(GeneratedTimecardEntryResponse source, params string[] keys)
    {
        if (source.AdditionalData == null)
            return null;
            
        foreach (var key in keys)
        {
            if (source.AdditionalData.TryGetValue(key, out var value) && value != null)
            {
                if (decimal.TryParse(value.ToString(), out var result))
                    return result;
            }
        }
        
        return null;
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

    /// <summary>
    /// Converts a Kiota Date to DateTime.
    /// </summary>
    private static DateTime? ConvertDateToDateTime(Microsoft.Kiota.Abstractions.Date? date)
    {
        if (!date.HasValue) return null;
        var dateValue = date.Value;
        return new DateTime(dateValue.Year, dateValue.Month, dateValue.Day);
    }
}