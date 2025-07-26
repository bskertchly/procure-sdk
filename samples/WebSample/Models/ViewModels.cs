using System.ComponentModel.DataAnnotations;

namespace WebSample.Models;

/// <summary>
/// View model for the home page showing authentication status
/// </summary>
public class HomeViewModel
{
    public bool IsAuthenticated { get; set; }
    public DateTimeOffset? TokenExpiresAt { get; set; }
    public IEnumerable<string> TokenScopes { get; set; } = Array.Empty<string>();
}

/// <summary>
/// View model for authentication errors
/// </summary>
public class AuthErrorViewModel
{
    public string Error { get; set; } = string.Empty;
    public string ErrorDescription { get; set; } = string.Empty;
    
    public string GetFriendlyErrorMessage()
    {
        return Error?.ToLowerInvariant() switch
        {
            "access_denied" => "Access was denied. You need to authorize the application to continue.",
            "invalid_request" => "The authentication request was invalid. Please try again.",
            "unauthorized_client" => "This application is not authorized to perform this action.",
            "unsupported_response_type" => "The authentication method is not supported.",
            "invalid_scope" => "The requested permissions are not valid.",
            "server_error" => "An error occurred on the authentication server. Please try again later.",
            "temporarily_unavailable" => "The authentication service is temporarily unavailable. Please try again later.",
            _ => !string.IsNullOrEmpty(ErrorDescription) ? ErrorDescription : "An unknown authentication error occurred."
        };
    }
}

/// <summary>
/// View model for the projects dashboard
/// </summary>
public class DashboardViewModel
{
    public DateTimeOffset? TokenExpiresAt { get; set; }
    public IEnumerable<string> TokenScopes { get; set; } = Array.Empty<string>();
    public List<ProjectSummary> RecentProjects { get; set; } = new();
    public int TotalProjectCount { get; set; }
    
    public bool IsTokenExpiringSoon => TokenExpiresAt.HasValue && 
        TokenExpiresAt.Value.Subtract(DateTimeOffset.UtcNow).TotalMinutes < 30;
}

/// <summary>
/// View model for the projects list page
/// </summary>
public class ProjectListViewModel
{
    public List<ProjectSummary> Projects { get; set; } = new();
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
    public int PreviousPage => Math.Max(1, Page - 1);
    public int NextPage => Math.Min(TotalPages, Page + 1);
}

/// <summary>
/// View model for creating a new project
/// </summary>
public class CreateProjectViewModel
{
    [Required(ErrorMessage = "Project name is required")]
    [StringLength(200, ErrorMessage = "Project name cannot exceed 200 characters")]
    [Display(Name = "Project Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company is required")]
    [Display(Name = "Company")]
    public int CompanyId { get; set; } = 1; // Default to first company for demo
    
    // For dropdown population
    public List<CompanySummary> AvailableCompanies { get; set; } = new()
    {
        new CompanySummary { Id = 1, Name = "Sample Construction Company" }
    };
}

/// <summary>
/// View model for editing an existing project
/// </summary>
public class EditProjectViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Project name is required")]
    [StringLength(200, ErrorMessage = "Project name cannot exceed 200 characters")]
    [Display(Name = "Project Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Standard error view model
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

/// <summary>
/// View model for displaying API operation results
/// </summary>
public class ApiResultViewModel<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// View model for pagination controls
/// </summary>
public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Controller { get; set; } = string.Empty;
    public object? RouteValues { get; set; }
    
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int StartItem => ((CurrentPage - 1) * PageSize) + 1;
    public int EndItem => Math.Min(CurrentPage * PageSize, TotalItems);
    
    public IEnumerable<int> GetPageNumbers()
    {
        var start = Math.Max(1, CurrentPage - 2);
        var end = Math.Min(TotalPages, CurrentPage + 2);
        
        for (int i = start; i <= end; i++)
        {
            yield return i;
        }
    }
}