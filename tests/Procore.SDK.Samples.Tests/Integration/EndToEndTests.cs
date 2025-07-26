using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Samples.Tests.Integration;

/// <summary>
/// End-to-end integration tests that validate complete workflows
/// Tests full OAuth flow, token management, and API interactions
/// </summary>
public class EndToEndTests : IClassFixture<TestAuthFixture>
{
    private readonly TestAuthFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EndToEndTests> _logger;

    public EndToEndTests(TestAuthFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<EndToEndTests>>();
    }

    [Fact]
    public async Task E2E_CompleteAuthenticationWorkflow_ShouldSucceed()
    {
        // Arrange
        _fixture.Reset();
        await _fixture.ClearTokensAsync();

        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        // Step 1: Generate authorization URL
        var state = Guid.NewGuid().ToString();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);

        _logger.LogInformation("Generated authorization URL: {AuthUrl}", authUrl);

        // Step 2: Simulate user authorization and callback
        var simulatedAuthCode = "e2e-authorization-code";
        
        _fixture.MockTokenResponse(new
        {
            access_token = "e2e-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "e2e-refresh-token",
            scope = "read write admin"
        });

        // Step 3: Exchange code for token
        var token = await oauthHelper.ExchangeCodeForTokenAsync(simulatedAuthCode, codeVerifier);
        await tokenManager.StoreTokenAsync(token);

        // Step 4: Use token for API calls
        _fixture.MockApiResponse("/api/v1/projects", new
        {
            projects = new[]
            {
                new { id = 1, name = "Test Project 1", status = "active" },
                new { id = 2, name = "Test Project 2", status = "active" }
            }
        });

        // Simulate API call with authenticated client
        // Note: This would use actual SDK clients in real implementation
        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");
        var apiResponse = await httpClient.GetAsync("/api/v1/projects");

        // Assert
        token.Should().NotBeNull("Token should be obtained");
        token.Token.Should().Be("e2e-access-token");
        
        var storedToken = await tokenManager.GetAccessTokenAsync();
        storedToken.Should().NotBeNull("Token should be stored and retrievable");
        
        apiResponse.IsSuccessStatusCode.Should().BeTrue("API call should succeed with valid token");
        
        _fixture.VerifyAuthenticatedRequest("e2e-access-token");
        
        _logger.LogInformation("E2E authentication workflow completed successfully");
    }

    [Fact]
    public async Task E2E_TokenRefreshWorkflow_ShouldMaintainAuthentication()
    {
        // Arrange
        _fixture.Reset();
        await _fixture.ClearTokensAsync();

        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        // Store an expired token
        var expiredToken = new AccessToken(
            "expired-access-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30), // Expired 30 minutes ago
            "valid-refresh-token",
            new[] { "read", "write", "admin" });

        await tokenManager.StoreTokenAsync(expiredToken);

        // Setup refresh token response
        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-refresh-token",
            scope = "read write admin"
        });

        // Setup API response for authenticated call
        _fixture.MockApiResponse("/api/v1/companies", new
        {
            companies = new[]
            {
                new { id = 1, name = "Test Company", status = "active" }
            }
        });

        // Act
        var currentToken = await tokenManager.GetAccessTokenAsync();
        
        // Simulate API call that triggers refresh
        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");
        var apiResponse = await httpClient.GetAsync("/api/v1/companies");

        // Assert
        currentToken.Should().NotBeNull("Should get refreshed token");
        currentToken!.Token.Should().Be("refreshed-access-token", "Should be new token");
        currentToken.RefreshToken.Should().Be("new-refresh-token", "Should update refresh token");
        
        apiResponse.IsSuccessStatusCode.Should().BeTrue("API call should succeed after refresh");
        
        _fixture.VerifyAuthenticatedRequest("refreshed-access-token");
        
        _logger.LogInformation("E2E token refresh workflow completed successfully");
    }

    [Fact]
    public async Task E2E_AuthenticationFailureRecovery_ShouldHandleGracefully()
    {
        // Arrange
        _fixture.Reset();
        await _fixture.ClearTokensAsync();

        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        // Store a token with invalid refresh token
        var tokenWithBadRefresh = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "invalid-refresh-token",
            new[] { "read" });

        await tokenManager.StoreTokenAsync(tokenWithBadRefresh);

        // Setup refresh failure
        _fixture.MockRefreshTokenErrorResponse(HttpStatusCode.Unauthorized, new
        {
            error = "invalid_grant",
            error_description = "Refresh token is invalid or expired"
        });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => tokenManager.GetAccessTokenAsync());

        // Verify clean state after failure
        var clearedToken = await tokenManager.GetAccessTokenAsync();
        clearedToken.Should().BeNull("Token should be cleared after refresh failure");

        _logger.LogInformation("E2E authentication failure recovery completed");
    }

    [Fact]
    public async Task E2E_ConcurrentOperations_ShouldMaintainConsistency()
    {
        // Arrange
        _fixture.Reset();
        await _fixture.SetupAuthenticatedStateAsync("concurrent-token", 3600);

        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        // Setup API responses
        _fixture.MockApiResponse("/api/v1/projects", new { projects = new[] { new { id = 1, name = "Project 1" } } });
        _fixture.MockApiResponse("/api/v1/companies", new { companies = new[] { new { id = 1, name = "Company 1" } } });
        _fixture.MockApiResponse("/api/v1/users", new { users = new[] { new { id = 1, name = "User 1" } } });

        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");

        // Act - Multiple concurrent API calls
        var tasks = new[]
        {
            httpClient.GetAsync("/api/v1/projects"),
            httpClient.GetAsync("/api/v1/companies"),
            httpClient.GetAsync("/api/v1/users"),
            tokenManager.GetAccessTokenAsync(),
            tokenManager.GetAccessTokenAsync()
        };

        var results = await Task.WhenAll(tasks.Take(3).Cast<Task<HttpResponseMessage>>());
        var tokenResults = await Task.WhenAll(tasks.Skip(3).Cast<Task<AccessToken?>>());

        // Assert
        results.Should().AllSatisfy(response => 
            response.IsSuccessStatusCode.Should().BeTrue("All API calls should succeed"));

        tokenResults.Should().AllSatisfy(token =>
        {
            token.Should().NotBeNull("All token requests should succeed");
            token!.Token.Should().Be("concurrent-token");
        });

        _logger.LogInformation("E2E concurrent operations completed successfully");
    }

    [Fact]
    public async Task E2E_ErrorHandlingAcrossComponents_ShouldPropagateProperly()
    {
        // Arrange
        _fixture.Reset();
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        
        // Setup network failure
        _fixture.MockNetworkFailure();

        // Act & Assert - Test error propagation through components
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();
        
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));

        exception.Message.Should().Contain("network failure", "Should propagate network error");
        
        _logger.LogInformation("E2E error handling validation completed");
    }

    [Fact]
    public async Task E2E_FullConsoleAppWorkflow_ShouldDemonstrateUsage()
    {
        // Arrange - Simulate complete console app workflow
        _fixture.Reset();
        await _fixture.ClearTokensAsync();

        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();

        _logger.LogInformation("Starting console app workflow simulation");

        // Step 1: App starts, user needs to authenticate
        var initialToken = await tokenManager.GetAccessTokenAsync();
        initialToken.Should().BeNull("No token should exist initially");

        // Step 2: Generate auth URL and display to user
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("console-app-state");
        authUrl.Should().NotBeNullOrEmpty("Auth URL should be generated");
        
        _logger.LogInformation("User would visit: {AuthUrl}", authUrl);

        // Step 3: User authorizes and provides auth code
        var userProvidedCode = "console-authorization-code";
        
        _fixture.MockTokenResponse(new
        {
            access_token = "console-app-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "console-refresh-token",
            scope = "read write admin"
        });

        // Step 4: Exchange code for token
        var token = await oauthHelper.ExchangeCodeForTokenAsync(userProvidedCode, codeVerifier);
        await tokenManager.StoreTokenAsync(token);

        // Step 5: App can now make authenticated API calls
        _fixture.MockApiResponse("/api/v1/me", new
        {
            user = new { id = 1, name = "Console User", email = "user@example.com" }
        });

        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");
        var userResponse = await httpClient.GetAsync("/api/v1/me");

        // Assert
        userResponse.IsSuccessStatusCode.Should().BeTrue("Authenticated API call should succeed");
        
        var finalToken = await tokenManager.GetAccessTokenAsync();
        finalToken.Should().NotBeNull("Token should be available for future API calls");
        
        _logger.LogInformation("Console app workflow simulation completed successfully");
    }

    [Fact]
    public async Task E2E_FullWebAppWorkflow_ShouldDemonstrateUsage()
    {
        // Arrange - Simulate complete web app workflow
        _fixture.Reset();
        await _fixture.ClearTokensAsync();

        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        
        _logger.LogInformation("Starting web app workflow simulation");

        // Step 1: User accesses protected resource, gets redirected to auth
        var sessionState = "web-session-" + Guid.NewGuid();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(sessionState);
        
        // Step 2: User returns from authorization with code
        var authCode = "web-authorization-code";
        
        _fixture.MockTokenResponse(new
        {
            access_token = "web-app-token",
            token_type = "Bearer", 
            expires_in = 3600,
            refresh_token = "web-refresh-token",
            scope = "read write admin"
        });

        // Step 3: Web app processes callback
        var token = await oauthHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier);
        
        // Step 4: Web app makes API calls on behalf of user
        _fixture.MockApiResponse("/api/v1/projects", new
        {
            projects = new[]
            {
                new { id = 1, name = "Web Project 1" },
                new { id = 2, name = "Web Project 2" }
            }
        });

        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");
        var projectsResponse = await httpClient.GetAsync("/api/v1/projects");

        // Assert
        token.Should().NotBeNull("Token should be obtained from callback");
        projectsResponse.IsSuccessStatusCode.Should().BeTrue("API call should succeed");
        
        _fixture.VerifyAuthenticatedRequest("web-app-token");
        
        _logger.LogInformation("Web app workflow simulation completed successfully");
    }
}