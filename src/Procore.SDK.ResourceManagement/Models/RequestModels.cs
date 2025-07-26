using System;

namespace Procore.SDK.ResourceManagement.Models;

/// <summary>
/// Request models for ResourceManagement client operations
/// </summary>

// Resource Request Models
public class CreateResourceRequest
{
    public string Name { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? CostPerHour { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }
}

// Allocation Request Models
public class AllocateResourceRequest
{
    public int ResourceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AllocationPercentage { get; set; }
    public string Notes { get; set; } = string.Empty;
}

// Workforce Request Models
public class CreateWorkforceAssignmentRequest
{
    public int WorkerId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HoursPerWeek { get; set; }
}

// Capacity Planning Request Models
public class CreateCapacityPlanRequest
{
    public DateTime PlanDate { get; set; }
    public string ResourceCategory { get; set; } = string.Empty;
    public decimal RequiredCapacity { get; set; }
    public decimal AvailableCapacity { get; set; }
    public string Notes { get; set; } = string.Empty;
}