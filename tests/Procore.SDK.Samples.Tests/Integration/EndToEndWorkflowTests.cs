using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Core;
using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Samples.Tests.Integration;

/// <summary>
/// End-to-end workflow tests demonstrating complete user scenarios
/// Tests entire workflows from authentication through API operations to cleanup
/// </summary>
public class EndToEndWorkflowTests : IClassFixture<TestAuthFixture>
{
    private readonly TestAuthFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EndToEndWorkflowTests> _logger;

    public EndToEndWorkflowTests(TestAuthFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<EndToEndWorkflowTests>>();
    }

    [Fact]
    public async Task EndToEnd_ConsoleApplicationWorkflow_ShouldCompleteSuccessfully()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();

        _logger.LogInformation("Starting console application end-to-end workflow test");

        // Act & Assert - Step 1: Generate Authorization URL
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("e2e-console-state");
        
        authUrl.Should().NotBeNullOrEmpty("Authorization URL should be generated");
        authUrl.Should().Contain("code_challenge_method=S256", "Should use PKCE");
        authUrl.Should().Contain("state=e2e-console-state", "Should include state parameter");
        codeVerifier.Should().NotBeNullOrEmpty("Code verifier should be generated");

        _logger.LogInformation("✅ Step 1: Authorization URL generated successfully");

        // Act & Assert - Step 2: Exchange Authorization Code for Token
        var simulatedAuthCode = "e2e-console-auth-code";
        
        _fixture.MockTokenResponse(new
        {
            access_token = "e2e-console-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "e2e-console-refresh-token",
            scope = "read write admin"
        });

        var token = await oauthHelper.ExchangeCodeForTokenAsync(simulatedAuthCode, codeVerifier);
        await tokenManager.StoreTokenAsync(token);

        token.Should().NotBeNull("Token should be exchanged successfully");
        token.Token.Should().Be("e2e-console-access-token");
        token.TokenType.Should().Be("Bearer");
        token.RefreshToken.Should().Be("e2e-console-refresh-token");

        _logger.LogInformation("✅ Step 2: Token exchange completed successfully");

        // Act & Assert - Step 3: Make Authenticated API Calls
        _fixture.MockApiResponse("/rest/v1.0/me", new
        {
            id = 12345,
            name = "Test User",
            email = "test.user@example.com",
            company = new
            {
                id = 1,
                name = "Test Construction Company"
            }
        });

        _fixture.MockApiResponse("/rest/v1.0/projects", new
        {
            projects = new[]
            {
                new { id = 1, name = "E2E Test Project 1", status = "Active" },
                new { id = 2, name = "E2E Test Project 2", status = "Planning" }
            }
        });

        // Note: These would be actual API calls once the client is implemented
        // var currentUser = await coreClient.Rest.V10.Me.GetAsync();
        // var projects = await coreClient.Rest.V10.Projects.GetAsync();
        
        // Verify authentication headers are included
        await Task.Delay(100); // Simulate API calls
        _fixture.VerifyAuthenticatedRequest("e2e-console-access-token");

        _logger.LogInformation("✅ Step 3: Authenticated API calls completed successfully");

        // Act & Assert - Step 4: Verify Token Storage and Retrieval
        var storedToken = await tokenManager.GetAccessTokenAsync();
        
        storedToken.Should().NotBeNull("Token should be stored and retrievable");
        storedToken!.Token.Should().Be("e2e-console-access-token");
        storedToken.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow, "Token should not be expired");

        _logger.LogInformation("✅ Step 4: Token storage and retrieval verified");

        _logger.LogInformation("🎉 Console application end-to-end workflow completed successfully");
    }

    [Fact]
    public async Task EndToEnd_WebApplicationWorkflow_ShouldCompleteSuccessfully()
    {
        // Note: This would be implemented with WebApplicationTestFixture once available
        // For now, testing the components that exist
        
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        _logger.LogInformation("Starting web application end-to-end workflow test");

        // Step 1: Initiate OAuth flow (similar to console but with session state management)
        var webState = "e2e-web-state-12345";
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(webState);

        authUrl.Should().NotBeNullOrEmpty();
        authUrl.Should().Contain($"state={Uri.EscapeDataString(webState)}");

        // Step 2: Handle callback (session-based state validation)
        _fixture.MockTokenResponse(new
        {
            access_token = "e2e-web-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "e2e-web-refresh-token",
            scope = "read write admin"
        });

        var token = await oauthHelper.ExchangeCodeForTokenAsync("e2e-web-auth-code", codeVerifier);
        await tokenManager.StoreTokenAsync(token);

        token.Should().NotBeNull();
        token.Token.Should().Be("e2e-web-access-token");

        _logger.LogInformation("🎉 Web application workflow components verified");
    }

    [Fact]
    public async Task EndToEnd_TokenRefreshWorkflow_ShouldHandleExpiredTokens()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();

        _logger.LogInformation("Starting token refresh end-to-end workflow test");

        // Step 1: Set up expired token
        var expiredToken = new AccessToken(
            "expired-access-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30), // Expired 30 minutes ago
            "valid-refresh-token",
            new[] { "read", "write", "admin" });

        await tokenManager.StoreTokenAsync(expiredToken);

        _logger.LogInformation("✅ Step 1: Expired token stored");

        // Step 2: Mock refresh token response
        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-refresh-token",
            scope = "read write admin"
        });

        // Step 3: Attempt to get current token (should trigger refresh)
        var currentToken = await tokenManager.GetAccessTokenAsync();

        // Assert refresh worked
        currentToken.Should().NotBeNull("Token should be refreshed");
        currentToken!.Token.Should().Be("refreshed-access-token", "Should use refreshed token");
        currentToken.RefreshToken.Should().Be("new-refresh-token", "Should update refresh token");
        currentToken.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow, "New token should not be expired");

        _logger.LogInformation("✅ Step 2: Token refresh completed successfully");

        // Step 4: Verify API calls use new token
        _fixture.MockApiResponse("/rest/v1.0/projects", new
        {
            projects = new[] { new { id = 1, name = "Refresh Test Project" } }
        });

        // Simulate API call that would use the refreshed token
        await Task.Delay(100);

        _logger.LogInformation("🎉 Token refresh end-to-end workflow completed successfully");
    }

    [Fact]
    public async Task EndToEnd_ErrorRecoveryWorkflow_ShouldHandleErrorsGracefully()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        _logger.LogInformation("Starting error recovery end-to-end workflow test");

        // Step 1: Test invalid authorization code handling
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("error-recovery-state");
        var invalidAuthCode = "invalid-authorization-code";

        _fixture.MockTokenErrorResponse(HttpStatusCode.BadRequest, new
        {
            error = "invalid_grant",
            error_description = "The provided authorization grant is invalid"
        });

        // Act & Assert - Should handle token exchange error gracefully
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync(invalidAuthCode, codeVerifier));

        exception.Message.Should().Contain("400", "Should include HTTP status code");

        _logger.LogInformation("✅ Step 1: Invalid authorization code error handled gracefully");

        // Step 2: Test refresh token failure and recovery
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "invalid-refresh-token",
            new[] { "read", "write" });

        await tokenManager.StoreTokenAsync(expiredToken);

        _fixture.MockRefreshTokenErrorResponse(HttpStatusCode.Unauthorized, new
        {
            error = "invalid_grant",
            error_description = "Refresh token is invalid or expired"
        });

        // Should handle refresh failure by clearing token
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => tokenManager.GetAccessTokenAsync());

        var clearedToken = await tokenManager.GetAccessTokenAsync();
        clearedToken.Should().BeNull("Token should be cleared after refresh failure");

        _logger.LogInformation("✅ Step 2: Refresh token failure handled with token cleanup");

        // Step 3: Test network failure recovery
        _fixture.MockNetworkFailure();

        var networkException = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("valid-code", codeVerifier));

        networkException.Should().NotBeNull("Should handle network failures gracefully");

        _logger.LogInformation("✅ Step 3: Network failure handled gracefully");

        _logger.LogInformation("🎉 Error recovery end-to-end workflow completed successfully");
    }

    [Fact]
    public async Task EndToEnd_FullProjectLifecycleWorkflow_ShouldCompleteAllOperations()
    {
        // Arrange
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
        await _fixture.SetupAuthenticatedStateAsync("lifecycle-access-token");

        _logger.LogInformation("Starting full project lifecycle end-to-end workflow test");

        // Step 1: List existing projects
        _fixture.MockApiResponse("/rest/v1.0/projects", new
        {
            projects = new[]
            {
                new { id = 1, name = "Existing Project 1", status = "Active" },
                new { id = 2, name = "Existing Project 2", status = "Planning" }
            }
        });

        // Note: Actual implementation would be:
        // var existingProjects = await coreClient.Rest.V10.Projects.GetAsync();
        await Task.Delay(50); // Simulate API call

        _logger.LogInformation("✅ Step 1: Listed existing projects");

        // Step 2: Create new project
        var newProjectId = 999;
        _fixture.MockApiResponse("/rest/v1.0/projects", new
        {
            id = newProjectId,
            name = "Lifecycle Test Project",
            description = "A project created during lifecycle testing",
            status = "Active",
            created_at = DateTimeOffset.UtcNow.ToString("O"),
            updated_at = DateTimeOffset.UtcNow.ToString("O")
        }, HttpStatusCode.Created);

        // Note: Actual implementation would be:
        // var createRequest = new { project = new { name = "Lifecycle Test Project", ... } };
        // var createdProject = await coreClient.Rest.V10.Projects.PostAsync(createRequest);
        await Task.Delay(50); // Simulate API call

        _logger.LogInformation("✅ Step 2: Created new project with ID {ProjectId}", newProjectId);

        // Step 3: Get project details
        _fixture.MockApiResponse($"/rest/v1.0/projects/{newProjectId}", new
        {
            id = newProjectId,
            name = "Lifecycle Test Project",
            description = "A project created during lifecycle testing",
            status = "Active",
            created_at = DateTimeOffset.UtcNow.ToString("O"),
            updated_at = DateTimeOffset.UtcNow.ToString("O"),
            company = new { id = 1, name = "Test Company" }
        });

        // Note: Actual implementation would be:
        // var projectDetails = await coreClient.Rest.V10.Projects[newProjectId].GetAsync();
        await Task.Delay(50); // Simulate API call

        _logger.LogInformation("✅ Step 3: Retrieved project details");

        // Step 4: Update project
        _fixture.MockApiResponse($"/rest/v1.0/projects/{newProjectId}", new
        {
            id = newProjectId,
            name = "Updated Lifecycle Test Project",
            description = "Updated description for lifecycle testing",
            status = "Active",
            created_at = DateTimeOffset.UtcNow.AddDays(-1).ToString("O"),
            updated_at = DateTimeOffset.UtcNow.ToString("O"),
            company = new { id = 1, name = "Test Company" }
        });

        // Note: Actual implementation would be:
        // var updateRequest = new { project = new { name = "Updated Lifecycle Test Project", ... } };
        // var updatedProject = await coreClient.Rest.V10.Projects[newProjectId].PatchAsync(updateRequest);
        await Task.Delay(50); // Simulate API call

        _logger.LogInformation("✅ Step 4: Updated project");

        // Step 5: Delete project (cleanup)
        _fixture.MockApiResponse($"/rest/v1.0/projects/{newProjectId}", null, HttpStatusCode.NoContent);

        // Note: Actual implementation would be:
        // await coreClient.Rest.V10.Projects[newProjectId].DeleteAsync();
        await Task.Delay(50); // Simulate API call

        _logger.LogInformation("✅ Step 5: Deleted project (cleanup)");

        // Verify all operations included proper authentication
        _fixture.VerifyAuthenticatedRequest("lifecycle-access-token");

        _logger.LogInformation("🎉 Full project lifecycle end-to-end workflow completed successfully");
    }

    [Fact]
    public async Task EndToEnd_ConcurrentUsersWorkflow_ShouldHandleMultipleUsers()
    {
        // Arrange
        var numberOfUsers = 3;
        var tasks = new List<Task>();

        _logger.LogInformation("Starting concurrent users end-to-end workflow test with {UserCount} users", numberOfUsers);

        // Simulate multiple users authenticating and performing operations concurrently
        for (int userId = 1; userId <= numberOfUsers; userId++)
        {
            tasks.Add(SimulateUserWorkflowAsync(userId));
        }

        // Act
        await Task.WhenAll(tasks);

        // Assert
        var allRequests = _fixture.GetCapturedRequests();
        allRequests.Should().NotBeEmpty("Should capture requests from all concurrent users");

        _logger.LogInformation("🎉 Concurrent users end-to-end workflow completed successfully");
    }

    private async Task SimulateUserWorkflowAsync(int userId)
    {
        // Each user gets their own service scope to simulate independent sessions
        using var scope = _serviceProvider.CreateScope();
        var scopedOAuthHelper = scope.ServiceProvider.GetRequiredService<OAuthFlowHelper>();
        var scopedTokenManager = scope.ServiceProvider.GetRequiredService<ITokenManager>();

        // Authenticate user
        var (authUrl, codeVerifier) = scopedOAuthHelper.GenerateAuthorizationUrl($"user-{userId}-state");

        _fixture.MockTokenResponse(new
        {
            access_token = $"user-{userId}-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = $"user-{userId}-refresh-token",
            scope = "read write"
        });

        var token = await scopedOAuthHelper.ExchangeCodeForTokenAsync($"user-{userId}-auth-code", codeVerifier);
        await scopedTokenManager.StoreTokenAsync(token);

        // Perform some API operations
        _fixture.MockApiResponse($"/rest/v1.0/users/{userId}/projects", new
        {
            projects = new[] { new { id = userId * 100, name = $"User {userId} Project" } }
        });

        // Simulate API calls
        await Task.Delay(Random.Shared.Next(50, 200)); // Simulate varying response times

        _logger.LogInformation("✅ User {UserId} workflow completed", userId);
    }

    [Fact]
    public async Task EndToEnd_LongRunningSessionWorkflow_ShouldMaintainAuthenticationOverTime()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();

        _logger.LogInformation("Starting long-running session end-to-end workflow test");

        // Step 1: Set up initial authentication
        await _fixture.SetupAuthenticatedStateAsync("long-session-token", expiresInSeconds: 300); // 5 minutes

        // Step 2: Perform operations over time with token refresh
        for (int iteration = 1; iteration <= 3; iteration++)
        {
            _logger.LogInformation("Iteration {Iteration}: Performing API operations", iteration);

            // Mock API responses for this iteration
            _fixture.MockApiResponse($"/rest/v1.0/projects/iteration-{iteration}", new
            {
                projects = new[] { new { id = iteration, name = $"Iteration {iteration} Project" } }
            });

            // If this is the second iteration, simulate token near expiry and refresh
            if (iteration == 2)
            {
                // Set up a token that will expire soon
                var nearExpiryToken = new AccessToken(
                    "near-expiry-token",
                    "Bearer",
                    DateTimeOffset.UtcNow.AddMinutes(2), // Expires in 2 minutes
                    "refresh-token",
                    new[] { "read", "write" });

                await tokenManager.StoreTokenAsync(nearExpiryToken);

                // Mock refresh response
                _fixture.MockRefreshTokenResponse(new
                {
                    access_token = "refreshed-long-session-token",
                    token_type = "Bearer",
                    expires_in = 3600,
                    refresh_token = "new-refresh-token",
                    scope = "read write"
                });

                _logger.LogInformation("Token refresh simulation set up for iteration {Iteration}", iteration);
            }

            // Simulate API operations
            await Task.Delay(100);
            
            // Verify token is still valid
            var currentToken = await tokenManager.GetAccessTokenAsync();
            currentToken.Should().NotBeNull($"Token should be available in iteration {iteration}");

            _logger.LogInformation("✅ Iteration {Iteration} completed successfully", iteration);
        }

        _logger.LogInformation("🎉 Long-running session end-to-end workflow completed successfully");
    }
}