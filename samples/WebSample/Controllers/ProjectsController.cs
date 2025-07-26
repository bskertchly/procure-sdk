using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSample.Services;
using WebSample.Models;

namespace WebSample.Controllers;

/// <summary>
/// Controller demonstrating API operations with proper authentication and error handling
/// Shows CRUD operations using the Procore Core client in a web application context
/// </summary>
[Authorize]
public class ProjectsController : Controller
{
    private readonly ProjectService _projectService;
    private readonly AuthenticationService _authService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(
        ProjectService projectService,
        AuthenticationService authService,
        ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Dashboard showing authenticated user's projects
    /// </summary>
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            // Verify authentication
            if (!await _authService.IsAuthenticatedAsync())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new DashboardViewModel();
            
            // Get current token info for display
            var token = await _authService.GetCurrentTokenAsync();
            if (token != null)
            {
                model.TokenExpiresAt = token.ExpiresAt;
                model.TokenScopes = token.Scopes;
            }

            // Get projects (first page)
            var projects = await _projectService.GetProjectsAsync(page: 1, pageSize: 10);
            model.RecentProjects = projects.Projects;
            model.TotalProjectCount = projects.TotalCount;

            return View(model);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning("Unauthorized access to dashboard");
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load dashboard");
            TempData["Error"] = "Failed to load dashboard. Please try again.";
            return View(new DashboardViewModel());
        }
    }

    /// <summary>
    /// Lists all projects with pagination
    /// </summary>
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _projectService.GetProjectsAsync(page, pageSize);
            
            var model = new ProjectListViewModel
            {
                Projects = result.Projects,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };

            return View(model);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load projects list");
            TempData["Error"] = "Failed to load projects. Please try again.";
            return View(new ProjectListViewModel());
        }
    }

    /// <summary>
    /// Shows details for a specific project
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return RedirectToAction("Login", "Auth");
            }

            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load project {ProjectId}", id);
            TempData["Error"] = "Failed to load project details. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Shows the create project form
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return RedirectToAction("Login", "Auth");
        }

        return View(new CreateProjectViewModel());
    }

    /// <summary>
    /// Handles project creation
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProjectViewModel model)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new CreateProjectRequest
            {
                Name = model.Name,
                Description = model.Description,
                CompanyId = model.CompanyId
            };

            var createdProject = await _projectService.CreateProjectAsync(request);
            
            TempData["Success"] = $"Project '{createdProject.Name}' created successfully";
            return RedirectToAction(nameof(Details), new { id = createdProject.Id });
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create project");
            ModelState.AddModelError(string.Empty, "Failed to create project. Please try again.");
            return View(model);
        }
    }

    /// <summary>
    /// Shows the edit project form
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return RedirectToAction("Login", "Auth");
            }

            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var model = new EditProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            };

            return View(model);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load project {ProjectId} for editing", id);
            TempData["Error"] = "Failed to load project for editing. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Handles project updates
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProjectViewModel model)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new UpdateProjectRequest
            {
                Name = model.Name,
                Description = model.Description
            };

            var updatedProject = await _projectService.UpdateProjectAsync(model.Id, request);
            
            TempData["Success"] = $"Project '{updatedProject.Name}' updated successfully";
            return RedirectToAction(nameof(Details), new { id = updatedProject.Id });
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update project {ProjectId}", model.Id);
            ModelState.AddModelError(string.Empty, "Failed to update project. Please try again.");
            return View(model);
        }
    }

    /// <summary>
    /// API endpoint for AJAX project operations
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ApiList(int page = 1, int pageSize = 10)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return Json(new { error = "Not authenticated" });
            }

            var result = await _projectService.GetProjectsAsync(page, pageSize);
            
            return Json(new
            {
                projects = result.Projects,
                page = result.Page,
                pageSize = result.PageSize,
                totalCount = result.TotalCount,
                totalPages = result.TotalPages
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load projects via API");
            return Json(new { error = "Failed to load projects" });
        }
    }

    /// <summary>
    /// API endpoint for getting project details
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ApiDetails(int id)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
            {
                return Json(new { error = "Not authenticated" });
            }

            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
            {
                return Json(new { error = "Project not found" });
            }

            return Json(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load project {ProjectId} via API", id);
            return Json(new { error = "Failed to load project details" });
        }
    }
}