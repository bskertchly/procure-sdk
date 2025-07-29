using System;
using System.Collections.Generic;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ResourceManagement.Models;
using Generated = Procore.SDK.ResourceManagement.Rest.V11.Projects.Item.Schedule.Resources.Item;
using V10Resources = Procore.SDK.ResourceManagement.Rest.V10.Resources.Item;
using V10Workforce = Procore.SDK.ResourceManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Assignments.Current;

namespace Procore.SDK.ResourceManagement.TypeMapping;

/// <summary>
/// Type mapper for converting between Resource domain models and generated Kiota client types.
/// Handles mapping between ResourceManagement.Models.Resource and generated ResourcesGetResponse.
/// </summary>
public class ResourceTypeMapper : BaseTypeMapper<Resource, Generated.ResourcesGetResponse>
{
    /// <summary>
    /// Maps from generated Kiota client type to Resource domain model.
    /// </summary>
    /// <param name="source">The generated ResourcesGetResponse to map from</param>
    /// <returns>The mapped Resource domain model</returns>
    protected override Resource DoMapToWrapper(Generated.ResourcesGetResponse source)
    {
        return new Resource
        {
            Id = source.Id ?? 0,
            Name = source.Name ?? string.Empty,
            Type = ResourceType.Equipment, // Default since API doesn't provide type field
            Status = ResourceStatus.Available, // Default since API doesn't provide status field
            Description = source.Name ?? string.Empty, // Use name as description fallback
            CostPerHour = 0m, // Not available in generated model
            Location = string.Empty, // Not available in generated model
            AvailableFrom = DateTime.UtcNow, // Default availability
            AvailableTo = DateTime.UtcNow.AddYears(1), // Default availability
            CreatedAt = DateTime.UtcNow, // Not available in generated model
            UpdatedAt = DateTime.UtcNow, // Not available in generated model
            CompanyId = source.CompanyId,
            ProjectId = source.ProjectId,
            SourceUid = source.SourceUid,
            ScheduleAttributes = source.ScheduleAttributes?.AdditionalData,
            DeletedAt = MapNullableDateTime(source.DeletedAt)
        };
    }

    /// <summary>
    /// Maps from Resource domain model to generated Kiota client type.
    /// </summary>
    /// <param name="source">The Resource domain model to map from</param>
    /// <returns>The mapped generated ResourcesGetResponse</returns>
    protected override Generated.ResourcesGetResponse DoMapToGenerated(Resource source)
    {
        return new Generated.ResourcesGetResponse
        {
            Id = source.Id,
            Name = source.Name,
            CompanyId = source.CompanyId,
            ProjectId = source.ProjectId,
            SourceUid = source.SourceUid,
            DeletedAt = source.DeletedAt.HasValue ? new DateTimeOffset(source.DeletedAt.Value) : null
        };
    }

    /// <summary>
    /// Maps from V1.0 company resources response to Resource domain model.
    /// </summary>
    /// <param name="source">The V1.0 resource response to map from</param>
    /// <param name="companyId">The company ID to associate with the resource</param>
    /// <returns>The mapped Resource domain model</returns>
    public Resource MapV10ResourceToWrapper(V10Resources.ResourcesGetResponse source, int? companyId = null)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        return new Resource
        {
            Id = source.Id ?? 0,
            Name = source.Name ?? string.Empty,
            Type = ResourceType.Equipment, // Default type for V1.0 resources
            Status = ResourceStatus.Available, // Default status
            Description = source.Name ?? string.Empty,
            CostPerHour = 0m, // Not available in V1.0 endpoint
            Location = string.Empty, // Not available in V1.0 endpoint
            AvailableFrom = DateTime.UtcNow,
            AvailableTo = DateTime.UtcNow.AddYears(1),
            CreatedAt = DateTime.UtcNow, // Not available in V1.0 endpoint
            UpdatedAt = DateTime.UtcNow, // Not available in V1.0 endpoint
            CompanyId = companyId,
            ProjectId = null, // V1.0 resources don't have project association
            SourceUid = source.SourceUid,
            ScheduleAttributes = null, // V1.0 doesn't have schedule attributes
            DeletedAt = null // V1.0 resources don't have DeletedAt
        };
    }

    /// <summary>
    /// Maps from V1.0 workforce assignment data to WorkforceAssignment domain model.
    /// </summary>
    /// <param name="assignment">The workforce assignment data</param>
    /// <param name="person">The person data containing the assignment</param>
    /// <param name="projectId">The project ID context</param>
    /// <returns>The mapped WorkforceAssignment domain model</returns>
    public WorkforceAssignment MapV10WorkforceAssignmentToWrapper(
        V10Workforce.Current_data_assignments assignment, 
        V10Workforce.Current_data person, 
        int projectId)
    {
        if (assignment == null)
            throw new ArgumentNullException(nameof(assignment));
        if (person == null)
            throw new ArgumentNullException(nameof(person));

        return new WorkforceAssignment
        {
            Id = int.TryParse(assignment.Id, out var assignmentId) ? assignmentId : 0,
            WorkerId = int.TryParse(person.PersonId, out var personId) ? personId : 0,
            ProjectId = projectId,
            Role = assignment.AssignmentStatus?.Name ?? "Unknown",
            StartDate = DateTime.UtcNow, // Default start date
            EndDate = DateTime.UtcNow.AddMonths(6), // Default 6-month assignment
            HoursPerWeek = CalculateWeeklyHours(assignment.WorkDays),
            Status = MapAssignmentStatus(assignment.AssignmentStatus?.Name),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Calculates weekly hours from work days data.
    /// Enhanced to provide more realistic hour calculations.
    /// </summary>
    /// <param name="workDays">The work days data</param>
    /// <returns>The calculated hours per week</returns>
    private decimal CalculateWeeklyHours(V10Workforce.Current_data_assignments_work_days? workDays)
    {
        if (workDays == null)
            return 40.0m; // Default full-time hours

        // Enhanced calculation based on available work days data
        // Since the structure contains additional data, we can make educated estimates
        try
        {
            // If additional data contains hour information, use it
            if (workDays.AdditionalData?.ContainsKey("hours_per_week") == true)
            {
                if (decimal.TryParse(workDays.AdditionalData["hours_per_week"]?.ToString(), out var hoursFromData))
                {
                    return Math.Min(60.0m, Math.Max(20.0m, hoursFromData)); // Reasonable bounds
                }
            }
            
            // If additional data contains day count, estimate hours
            if (workDays.AdditionalData?.ContainsKey("working_days") == true)
            {
                if (int.TryParse(workDays.AdditionalData["working_days"]?.ToString(), out var workingDays))
                {
                    return workingDays * 8.0m; // 8 hours per working day
                }
            }
            
            return 40.0m; // Default when no specific data available
        }
        catch
        {
            return 40.0m; // Fallback on any parsing errors
        }
    }

    /// <summary>
    /// Maps assignment status name to domain assignment status.
    /// Enhanced to handle more workforce planning status variations.
    /// </summary>
    /// <param name="statusName">The status name from API</param>
    /// <returns>The mapped assignment status</returns>
    private AssignmentStatus MapAssignmentStatus(string? statusName)
    {
        if (string.IsNullOrWhiteSpace(statusName))
            return AssignmentStatus.Assigned;
            
        var normalizedStatus = statusName.ToLowerInvariant().Replace("_", "").Replace("-", "").Replace(" ", "");
        
        return normalizedStatus switch
        {
            "assigned" or "assign" => AssignmentStatus.Assigned,
            "active" or "working" or "current" => AssignmentStatus.Active,
            "completed" or "finished" or "done" => AssignmentStatus.Completed,
            "terminated" or "ended" or "cancelled" => AssignmentStatus.Terminated,
            "onleave" or "leave" or "vacation" or "absent" => AssignmentStatus.OnLeave,
            "pending" or "planned" or "future" => AssignmentStatus.Assigned,
            "inactive" or "suspended" => AssignmentStatus.OnLeave,
            _ => AssignmentStatus.Assigned // Default status
        };
    }
    
    /// <summary>
    /// Enhanced mapping that handles additional workforce data fields.
    /// </summary>
    /// <param name="workforceData">The complete workforce data</param>
    /// <param name="companyId">The company ID for context</param>
    /// <returns>Collection of mapped workforce assignments</returns>
    public IEnumerable<WorkforceAssignment> MapV10WorkforceDataToWrappers(
        V10Workforce.Current workforceData,
        int companyId)
    {
        if (workforceData?.Data == null)
            return Enumerable.Empty<WorkforceAssignment>();
            
        var assignments = new List<WorkforceAssignment>();
        
        foreach (var person in workforceData.Data)
        {
            if (person.Assignments != null)
            {
                foreach (var assignment in person.Assignments)
                {
                    var mappedAssignment = MapV10WorkforceAssignmentToWrapper(
                        assignment, person, companyId);
                    assignments.Add(mappedAssignment);
                }
            }
        }
        
        return assignments;
    }
}