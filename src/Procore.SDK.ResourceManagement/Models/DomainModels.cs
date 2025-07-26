using System;

namespace Procore.SDK.ResourceManagement.Models;

/// <summary>
/// Domain models for ResourceManagement client
/// </summary>

// Core Resource Models
public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public ResourceStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? CostPerHour { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum ResourceType
{
    Equipment,
    Labor,
    Material,
    Vehicle,
    Tool
}

public enum ResourceStatus
{
    Available,
    Allocated,
    InUse,
    Maintenance,
    Unavailable
}

public class ResourceAllocation
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public int ProjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AllocationPercentage { get; set; }
    public AllocationStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum AllocationStatus
{
    Planned,
    Active,
    Completed,
    Cancelled,
    OnHold
}

public class WorkforceAssignment
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public int ProjectId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HoursPerWeek { get; set; }
    public AssignmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum AssignmentStatus
{
    Assigned,
    Active,
    Completed,
    Terminated,
    OnLeave
}

public class CapacityPlan
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public DateTime PlanDate { get; set; }
    public string ResourceCategory { get; set; } = string.Empty;
    public decimal RequiredCapacity { get; set; }
    public decimal AvailableCapacity { get; set; }
    public decimal UtilizationRate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}