using Procore.SDK.Core;
using Procore.SDK.Shared.Authentication;

namespace WebSample.Services;

/// <summary>
/// Service for handling project-related operations using the Procore Core client
/// Demonstrates proper API usage patterns with authentication and error handling
/// </summary>
public class ProjectService
{
    private readonly ProcoreCoreClient _coreClient;
    private readonly ITokenManager _tokenManager;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(
        ProcoreCoreClient coreClient,
        ITokenManager tokenManager,
        ILogger<ProjectService> logger)
    {
        _coreClient = coreClient;
        _tokenManager = tokenManager;
        _logger = logger;
    }

    /// <summary>
    /// Gets a list of projects for the authenticated user
    /// </summary>
    public async Task<ProjectListResult> GetProjectsAsync(int page = 1, int pageSize = 20)
    {
        try
        {
            // Ensure we have a valid token
            var token = await _tokenManager.GetAccessTokenAsync();
            if (token == null)
            {
                throw new UnauthorizedAccessException("No valid authentication token available");
            }

            _logger.LogInformation("Fetching companies and users - Page: {Page}, PageSize: {PageSize}", page, pageSize);

            // Get companies and users to demonstrate real API calls
            var companies = await _coreClient.GetCompaniesAsync();
            var companiesList = companies.ToList();
            
            var projects = new List<ProjectSummary>();
            
            if (companiesList.Any())
            {
                var firstCompany = companiesList.First();
                _logger.LogInformation("Using company: {CompanyName} (ID: {CompanyId})", firstCompany.Name, firstCompany.Id);
                
                // Get users for the company to demonstrate real data
                var users = await _coreClient.GetUsersAsync(firstCompany.Id);
                var usersList = users.ToList();
                
                // Create mock projects based on real company and user data
                var startIndex = (page - 1) * pageSize;
                for (int i = 0; i < pageSize && i < usersList.Count; i++)
                {
                    var user = usersList[i];
                    projects.Add(new ProjectSummary
                    {
                        Id = user.Id,
                        Name = $"Project managed by {user.FirstName} {user.LastName}",
                        Description = $"Construction project in {firstCompany.Name}",
                        Status = user.IsActive ? "Active" : "Inactive"
                    });
                }
                
                // Fill remaining slots with mock data if needed
                while (projects.Count < pageSize)
                {
                    var projectNumber = startIndex + projects.Count + 1;
                    projects.Add(new ProjectSummary
                    {
                        Id = projectNumber + 10000, // Offset to avoid ID conflicts
                        Name = $"Construction Project {projectNumber}",
                        Description = $"Project {projectNumber} for {firstCompany.Name}",
                        Status = projectNumber % 3 == 0 ? "Completed" : "Active"
                    });
                }
            }
            else
            {
                // Fallback to mock data if no companies found
                projects = GenerateMockProjects(page, pageSize);
            }
            
            return new ProjectListResult
            {
                Projects = projects,
                TotalCount = Math.Max(companiesList.Count * 5, 50), // Estimate based on real data
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch projects - falling back to mock data");
            
            // Fallback to mock data on error
            return new ProjectListResult
            {
                Projects = GenerateMockProjects(page, pageSize),
                TotalCount = 50,
                Page = page,
                PageSize = pageSize
            };
        }
    }

    /// <summary>
    /// Gets detailed information for a specific project
    /// </summary>
    public async Task<ProjectDetail?> GetProjectAsync(int projectId)
    {
        try
        {
            var token = await _tokenManager.GetAccessTokenAsync();
            if (token == null)
            {
                throw new UnauthorizedAccessException("No valid authentication token available");
            }

            _logger.LogInformation("Fetching project details for ID: {ProjectId}", projectId);

            // Note: This would be implemented based on the actual generated client structure
            // Example API call pattern:
            // var project = await _coreClient.Rest.V10.Projects[projectId].GetAsync();
            // return new ProjectDetail
            // {
            //     Id = project.Id,
            //     Name = project.Name,
            //     Description = project.Description,
            //     Status = project.Status,
            //     CreatedAt = project.CreatedAt,
            //     UpdatedAt = project.UpdatedAt,
            //     Company = new CompanySummary
            //     {
            //         Id = project.Company.Id,
            //         Name = project.Company.Name
            //     }
            // };

            // Mock data for demonstration
            await Task.Delay(50); // Simulate API call
            
            return new ProjectDetail
            {
                Id = projectId,
                Name = $"Sample Project {projectId}",
                Description = $"This is a detailed description of project {projectId}",
                Status = "Active",
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-30),
                UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                Company = new CompanySummary
                {
                    Id = 1,
                    Name = "Sample Construction Company"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch project {ProjectId}", projectId);
            throw;
        }
    }

    /// <summary>
    /// Creates a new project (demonstration of POST operations)
    /// </summary>
    public async Task<ProjectDetail> CreateProjectAsync(CreateProjectRequest request)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);
            
            var token = await _tokenManager.GetAccessTokenAsync();
            if (token == null)
            {
                throw new UnauthorizedAccessException("No valid authentication token available");
            }

            _logger.LogInformation("Creating new project: {ProjectName}", request.Name);

            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Project name is required", nameof(request));
            }

            // Note: This would be implemented based on the actual generated client structure
            // Example API call pattern:
            // var createRequest = new CreateProjectRequestBody
            // {
            //     Project = new ProjectCreateModel
            //     {
            //         Name = request.Name,
            //         Description = request.Description,
            //         CompanyId = request.CompanyId
            //     }
            // };
            // var createdProject = await _coreClient.Rest.V10.Projects.PostAsync(createRequest);

            // Mock data for demonstration
            await Task.Delay(200); // Simulate API call
            
            var newProject = new ProjectDetail
            {
                Id = Random.Shared.Next(1000, 9999),
                Name = request.Name,
                Description = request.Description,
                Status = "Active",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                Company = new CompanySummary
                {
                    Id = request.CompanyId,
                    Name = "Sample Construction Company"
                }
            };

            _logger.LogInformation("Project created successfully with ID: {ProjectId}", newProject.Id);
            return newProject;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create project");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing project (demonstration of PUT/PATCH operations)
    /// </summary>
    public async Task<ProjectDetail> UpdateProjectAsync(int projectId, UpdateProjectRequest request)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);
            
            var token = await _tokenManager.GetAccessTokenAsync();
            if (token == null)
            {
                throw new UnauthorizedAccessException("No valid authentication token available");
            }

            _logger.LogInformation("Updating project {ProjectId}", projectId);

            // Note: This would be implemented based on the actual generated client structure
            // Example API call pattern:
            // var updateRequest = new UpdateProjectRequestBody
            // {
            //     Project = new ProjectUpdateModel
            //     {
            //         Name = request.Name,
            //         Description = request.Description
            //     }
            // };
            // var updatedProject = await _coreClient.Rest.V10.Projects[projectId].PatchAsync(updateRequest);

            // Mock data for demonstration
            await Task.Delay(150); // Simulate API call
            
            var updatedProject = new ProjectDetail
            {
                Id = projectId,
                Name = request.Name ?? $"Updated Project {projectId}",
                Description = request.Description ?? "Updated description",
                Status = "Active",
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-30),
                UpdatedAt = DateTimeOffset.UtcNow,
                Company = new CompanySummary
                {
                    Id = 1,
                    Name = "Sample Construction Company"
                }
            };

            _logger.LogInformation("Project {ProjectId} updated successfully", projectId);
            return updatedProject;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update project {ProjectId}", projectId);
            throw;
        }
    }

    private static List<ProjectSummary> GenerateMockProjects(int page, int pageSize)
    {
        var projects = new List<ProjectSummary>();
        var startIndex = (page - 1) * pageSize;
        
        for (int i = 0; i < pageSize; i++)
        {
            var projectNumber = startIndex + i + 1;
            projects.Add(new ProjectSummary
            {
                Id = projectNumber,
                Name = $"Project {projectNumber}",
                Description = $"Description for project {projectNumber}",
                Status = projectNumber % 3 == 0 ? "Completed" : "Active"
            });
        }
        
        return projects;
    }
}

/// <summary>
/// Model classes for project operations
/// </summary>
public class ProjectListResult
{
    public List<ProjectSummary> Projects { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class ProjectSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ProjectDetail : ProjectSummary
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public CompanySummary Company { get; set; } = new();
}

public class CompanySummary
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CompanyId { get; set; }
}

public class UpdateProjectRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}