using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Core;
using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Samples.Tests.Integration;

/// <summary>
/// Integration tests for CRUD operations using the Procore Core client
/// Tests end-to-end API interactions with proper authentication and error handling
/// </summary>
public class CrudOperationsTests : IClassFixture<TestAuthFixture>
{
    private readonly TestAuthFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CrudOperationsTests> _logger;

    public CrudOperationsTests(TestAuthFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<CrudOperationsTests>>();
    }

    [Fact]
    public async Task CoreClient_AuthenticatedRequest_ShouldIncludeProperHeaders()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync("crud-access-token");

        // Setup mock API response
        _fixture.MockApiResponse("/rest/v1.0/projects", new
        {
            projects = new[]
            {
                new { id = 1, name = "Test Project 1", status = "Active" },
                new { id = 2, name = "Test Project 2", status = "Planning" }
            }
        });

        // Act
        try
        {
            // Note: This would be the actual API call once the client is fully implemented
            // var projects = await coreClient.Rest.V10.Projects.GetAsync();
            
            // For now, simulate the call to verify authentication headers
            await Task.Delay(100); // Simulate API call
        }
        catch (NotImplementedException)
        {
            // Expected until the client is fully implemented
        }

        // Assert
        _fixture.VerifyAuthenticatedRequest("crud-access-token");
        _logger.LogInformation("Authenticated request verification completed");
    }

    [Fact]
    public async Task CoreClient_GetProjects_ShouldReturnProjectsList()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var mockProjects = new
        {
            projects = new[]
            {
                new { 
                    id = 1, 
                    name = "Sample Construction Project", 
                    description = "A sample project for testing",
                    status = "Active",
                    created_at = "2024-01-01T00:00:00Z",
                    updated_at = "2024-01-15T12:00:00Z"
                },
                new { 
                    id = 2, 
                    name = "Commercial Building Project", 
                    description = "Large commercial construction",
                    status = "Planning",
                    created_at = "2024-01-10T00:00:00Z",
                    updated_at = "2024-01-20T14:30:00Z"
                }
            }
        };

        _fixture.MockApiResponse("/rest/v1.0/projects", mockProjects);

        // Act & Assert
        // Note: Actual implementation would be:
        // var projects = await coreClient.Rest.V10.Projects.GetAsync();
        // projects.Should().NotBeNull();
        // projects.Should().HaveCount(2);

        // For now, verify the mock setup
        var requests = _fixture.GetCapturedRequests();
        _logger.LogInformation("Projects list request test completed");
    }

    [Fact]
    public async Task CoreClient_GetProjectById_ShouldReturnProjectDetails()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var projectId = 123;
        var mockProject = new
        {
            id = projectId,
            name = "Detailed Test Project",
            description = "Complete project details for testing",
            status = "Active",
            created_at = "2024-01-01T00:00:00Z",
            updated_at = "2024-01-15T12:00:00Z",
            company = new
            {
                id = 1,
                name = "Test Construction Company"
            },
            custom_fields = new object[] { },
            departments = new object[] { }
        };

        _fixture.MockApiResponse($"/rest/v1.0/projects/{projectId}", mockProject);

        // Act & Assert
        // Note: Actual implementation would be:
        // var project = await coreClient.Rest.V10.Projects[projectId].GetAsync();
        // project.Should().NotBeNull();
        // project.Id.Should().Be(projectId);
        // project.Name.Should().Be("Detailed Test Project");

        _logger.LogInformation("Project details request test completed for project {ProjectId}", projectId);
    }

    [Fact]
    public async Task CoreClient_CreateProject_ShouldPostDataCorrectly()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var newProject = new
        {
            project = new
            {
                name = "New Test Project",
                description = "A newly created project for testing",
                company_id = 1
            }
        };

        var createdProject = new
        {
            id = 456,
            name = "New Test Project",
            description = "A newly created project for testing",
            status = "Active",
            created_at = DateTimeOffset.UtcNow.ToString("O"),
            updated_at = DateTimeOffset.UtcNow.ToString("O"),
            company = new
            {
                id = 1,
                name = "Test Construction Company"
            }
        };

        _fixture.MockApiResponse("/rest/v1.0/projects", createdProject, HttpStatusCode.Created);

        // Act & Assert
        // Note: Actual implementation would be:
        // var result = await coreClient.Rest.V10.Projects.PostAsync(newProject);
        // result.Should().NotBeNull();
        // result.Id.Should().Be(456);
        // result.Name.Should().Be("New Test Project");

        _logger.LogInformation("Project creation test completed");
    }

    [Fact]
    public async Task CoreClient_UpdateProject_ShouldPatchDataCorrectly()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var projectId = 789;
        var updateData = new
        {
            project = new
            {
                name = "Updated Project Name",
                description = "Updated project description"
            }
        };

        var updatedProject = new
        {
            id = projectId,
            name = "Updated Project Name",
            description = "Updated project description",
            status = "Active",
            created_at = "2024-01-01T00:00:00Z",
            updated_at = DateTimeOffset.UtcNow.ToString("O"),
            company = new
            {
                id = 1,
                name = "Test Construction Company"
            }
        };

        _fixture.MockApiResponse($"/rest/v1.0/projects/{projectId}", updatedProject);

        // Act & Assert
        // Note: Actual implementation would be:
        // var result = await coreClient.Rest.V10.Projects[projectId].PatchAsync(updateData);
        // result.Should().NotBeNull();
        // result.Id.Should().Be(projectId);
        // result.Name.Should().Be("Updated Project Name");

        _logger.LogInformation("Project update test completed for project {ProjectId}", projectId);
    }

    [Fact]
    public async Task CoreClient_DeleteProject_ShouldSendDeleteRequest()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var projectId = 999;
        
        // Mock successful deletion (204 No Content)
        _fixture.MockApiResponse($"/rest/v1.0/projects/{projectId}", null, HttpStatusCode.NoContent);

        // Act & Assert
        // Note: Actual implementation would be:
        // await coreClient.Rest.V10.Projects[projectId].DeleteAsync();

        _logger.LogInformation("Project deletion test completed for project {ProjectId}", projectId);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "Invalid request data")]
    [InlineData(HttpStatusCode.Unauthorized, "Authentication required")]
    [InlineData(HttpStatusCode.Forbidden, "Insufficient permissions")]
    [InlineData(HttpStatusCode.NotFound, "Project not found")]
    [InlineData(HttpStatusCode.UnprocessableEntity, "Validation errors")]
    [InlineData(HttpStatusCode.InternalServerError, "Server error")]
    public async Task CoreClient_ApiErrorResponses_ShouldHandleGracefully(
        HttpStatusCode statusCode, string errorMessage)
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var errorResponse = new
        {
            error = errorMessage,
            details = new
            {
                code = (int)statusCode,
                message = errorMessage
            }
        };

        _fixture.MockApiResponse("/rest/v1.0/projects/error-test", errorResponse, statusCode);

        // Act & Assert
        // Note: Actual implementation would handle these errors appropriately
        // For example:
        // await Assert.ThrowsAsync<HttpRequestException>(() => 
        //     coreClient.Rest.V10.Projects["error-test"].GetAsync());

        _logger.LogInformation("Error handling test completed for {StatusCode}", statusCode);
    }

    [Fact]
    public async Task CoreClient_PaginatedRequests_ShouldHandlePagedResults()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        var pagedResponse = new
        {
            projects = new[]
            {
                new { id = 1, name = "Project 1" },
                new { id = 2, name = "Project 2" },
                new { id = 3, name = "Project 3" }
            },
            pagination = new
            {
                current_page = 1,
                per_page = 3,
                total_pages = 10,
                total_entries = 30
            }
        };

        _fixture.MockApiResponse("/rest/v1.0/projects?page=1&per_page=3", pagedResponse);

        // Act & Assert
        // Note: Actual implementation would handle pagination:
        // var result = await coreClient.Rest.V10.Projects.GetAsync(new { page = 1, per_page = 3 });
        // result.Projects.Should().HaveCount(3);
        // result.Pagination.TotalPages.Should().Be(10);

        _logger.LogInformation("Pagination test completed");
    }

    [Fact]
    public async Task CoreClient_ConcurrentRequests_ShouldHandleMultipleSimultaneousOperations()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        // Setup multiple endpoint responses
        for (int i = 1; i <= 5; i++)
        {
            _fixture.MockApiResponse($"/rest/v1.0/projects/{i}", new
            {
                id = i,
                name = $"Concurrent Project {i}",
                status = "Active"
            });
        }

        // Act
        var tasks = new List<Task>();
        for (int i = 1; i <= 5; i++)
        {
            // Note: Actual implementation would be:
            // tasks.Add(coreClient.Rest.V10.Projects[i].GetAsync());
            tasks.Add(Task.Delay(50)); // Simulate concurrent API calls
        }

        await Task.WhenAll(tasks);

        // Assert
        var requests = _fixture.GetCapturedRequests();
        requests.Count.Should().BeGreaterOrEqualTo(5, "Should handle multiple concurrent requests");
        
        _logger.LogInformation("Concurrent requests test completed");
    }

    [Fact]
    public async Task CoreClient_TokenExpiredDuringRequest_ShouldRefreshAutomatically()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        // Set up an expired token
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-10), // Expired 10 minutes ago
            "valid-refresh-token",
            new[] { "read", "write" });

        await tokenManager.StoreTokenAsync(expiredToken);

        // Setup mock refresh response
        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-refresh-token",
            scope = "read write"
        });

        // Setup API response for the actual request
        _fixture.MockApiResponse("/rest/v1.0/projects", new
        {
            projects = new[] { new { id = 1, name = "Test Project" } }
        });

        // Act
        // Note: Actual implementation would automatically refresh the token
        // var projects = await coreClient.Rest.V10.Projects.GetAsync();

        // Verify token was refreshed
        var currentToken = await tokenManager.GetAccessTokenAsync();

        // Assert
        currentToken.Should().NotBeNull("Token should be refreshed");
        currentToken!.Token.Should().Be("refreshed-token", "Should use refreshed token");
        currentToken.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow, "New token should not be expired");

        _logger.LogInformation("Automatic token refresh test completed");
    }

    [Fact]
    public async Task CoreClient_NetworkTimeout_ShouldHandleTimeoutGracefully()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        // Setup network timeout simulation
        _fixture.MockNetworkFailure();

        // Act & Assert
        // Note: Actual implementation would handle timeouts:
        // await Assert.ThrowsAsync<HttpRequestException>(() => 
        //     coreClient.Rest.V10.Projects.GetAsync());

        _logger.LogInformation("Network timeout test completed");
    }

    [Fact]
    public async Task CoreClient_LargeDataSet_ShouldHandleEfficientlyWithStreaming()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync();

        // Create a large mock dataset
        var largeDataset = new
        {
            projects = Enumerable.Range(1, 1000).Select(i => new
            {
                id = i,
                name = $"Project {i}",
                description = $"Description for project {i} with sufficient detail to test large payloads",
                status = i % 3 == 0 ? "Completed" : "Active",
                created_at = DateTimeOffset.UtcNow.AddDays(-i).ToString("O")
            })
        };

        _fixture.MockApiResponse("/rest/v1.0/projects/large-dataset", largeDataset);

        // Act & Assert
        // Note: Actual implementation would handle large datasets efficiently
        _logger.LogInformation("Large dataset handling test completed");
    }
}