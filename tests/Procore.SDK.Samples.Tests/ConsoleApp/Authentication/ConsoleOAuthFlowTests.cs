using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using System.Diagnostics;

namespace Procore.SDK.Samples.Tests.ConsoleApp.Authentication;

/// <summary>
/// Unit tests for console application OAuth PKCE flow
/// Tests the complete authentication workflow from URL generation to token exchange
/// </summary>
public class ConsoleOAuthFlowTests : IClassFixture<TestAuthFixture>
{
    private readonly TestAuthFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConsoleOAuthFlowTests> _logger;

    public ConsoleOAuthFlowTests(TestAuthFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<ConsoleOAuthFlowTests>>();
    }

    [Fact]
    public async Task ConsoleApp_InitialAuthentication_ShouldGenerateValidAuthUrl()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var expectedState = "console-app-state-12345";

        // Act
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(expectedState);

        // Assert
        authUrl.Should().NotBeNullOrEmpty("Authorization URL should be generated");
        codeVerifier.Should().NotBeNullOrEmpty("Code verifier should be generated");
        
        authUrl.Should().Contain("response_type=code", "Should use authorization code flow");
        authUrl.Should().Contain($"state={Uri.EscapeDataString(expectedState)}", "Should include state parameter");
        authUrl.Should().Contain("code_challenge_method=S256", "Should use PKCE with SHA256");
        
        _logger.LogInformation("Generated auth URL: {AuthUrl}", authUrl);
        _logger.LogInformation("Code verifier length: {Length}", codeVerifier.Length);
    }

    [Fact]
    public async Task ConsoleApp_UserProvidesAuthCode_ShouldExchangeForToken()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("test-state");
        var simulatedAuthCode = "simulated-auth-code-from-callback";

        // Setup mock HTTP response for token exchange
        _fixture.MockTokenResponse(new
        {
            access_token = "console-access-token",
            token_type = "Bearer", 
            expires_in = 3600,
            refresh_token = "console-refresh-token",
            scope = "read write admin"
        });

        // Act
        var token = await oauthHelper.ExchangeCodeForTokenAsync(simulatedAuthCode, codeVerifier);
        await tokenManager.StoreTokenAsync(token);

        // Assert
        token.Should().NotBeNull("Token should be exchanged successfully");
        token.Token.Should().Be("console-access-token");
        token.TokenType.Should().Be("Bearer");
        token.RefreshToken.Should().Be("console-refresh-token");
        token.Scopes.Should().ContainInOrder("read", "write", "admin");

        var storedToken = await tokenManager.GetAccessTokenAsync();
        storedToken.Should().NotBeNull("Token should be stored and retrievable");
        storedToken.Token.Should().Be(token.Token);
    }

    [Fact]
    public async Task ConsoleApp_InvalidAuthCode_ShouldThrowMeaningfulException()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();
        var invalidAuthCode = "invalid-authorization-code";

        // Setup mock HTTP error response
        _fixture.MockTokenErrorResponse(HttpStatusCode.BadRequest, new
        {
            error = "invalid_grant",
            error_description = "The provided authorization grant is invalid"
        });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync(invalidAuthCode, codeVerifier));

        exception.Message.Should().Contain("400", "Should include HTTP status code");
        _logger.LogWarning("Expected exception caught: {Message}", exception.Message);
    }

    [Fact]
    public async Task ConsoleApp_ExpiredToken_ShouldAutomaticallyRefresh()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        
        // Store an expired token
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer", 
            DateTimeOffset.UtcNow.AddMinutes(-30), // Expired 30 minutes ago
            "valid-refresh-token",
            new[] { "read", "write" });
        
        await tokenManager.StoreTokenAsync(expiredToken);

        // Setup mock refresh response
        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-refresh-token",
            scope = "read write"
        });

        // Act
        var currentToken = await tokenManager.GetAccessTokenAsync();

        // Assert
        currentToken.Should().NotBeNull("Should return refreshed token");
        currentToken.Token.Should().Be("refreshed-access-token", "Should be the new token");
        currentToken.RefreshToken.Should().Be("new-refresh-token", "Should update refresh token");
        currentToken.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow, "New token should not be expired");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task ConsoleApp_EmptyAuthCode_ShouldRejectGracefully(string? invalidCode)
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();

        // Act & Assert
        if (string.IsNullOrWhiteSpace(invalidCode))
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => oauthHelper.ExchangeCodeForTokenAsync(invalidCode!, codeVerifier));
        }
    }

    [Fact]
    public async Task ConsoleApp_NetworkFailure_ShouldHandleGracefully()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();
        var authCode = "valid-auth-code";

        // Setup network failure simulation
        _fixture.MockNetworkFailure();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier));

        exception.Should().NotBeNull("Should handle network failures gracefully");
        _logger.LogWarning("Network failure handled: {Message}", exception.Message);
    }

    [Fact]
    public async Task ConsoleApp_TokenRefreshFailure_ShouldClearTokenAndRequireReauth()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "invalid-refresh-token", // This will fail to refresh
            new[] { "read" });
        
        await tokenManager.StoreTokenAsync(expiredToken);

        // Setup mock refresh failure
        _fixture.MockRefreshTokenErrorResponse(HttpStatusCode.Unauthorized, new
        {
            error = "invalid_grant",
            error_description = "Refresh token is invalid or expired"
        });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => tokenManager.GetAccessTokenAsync());

        // Verify token was cleared
        var clearedToken = await tokenManager.GetAccessTokenAsync();
        clearedToken.Should().BeNull("Token should be cleared after refresh failure");
    }

    [Fact]
    public async Task ConsoleApp_ConcurrentTokenAccess_ShouldHandleSafely()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var tasks = new List<Task<AccessToken?>>();

        // Setup a valid token
        var validToken = new AccessToken(
            "concurrent-test-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "refresh-token",
            new[] { "read", "write" });
        
        await tokenManager.StoreTokenAsync(validToken);

        // Act - Multiple concurrent access attempts
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(tokenManager.GetAccessTokenAsync());
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().AllSatisfy(token => 
        {
            token.Should().NotBeNull("All concurrent calls should succeed");
            token!.Token.Should().Be("concurrent-test-token");
        });
    }

    [Fact]
    public void ConsoleApp_DependencyInjection_ShouldConfigureCorrectly()
    {
        // Act & Assert - Verify all required services are registered
        var oauthHelper = _serviceProvider.GetService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetService<ITokenManager>();
        var authOptions = _serviceProvider.GetService<IOptions<ProcoreAuthOptions>>();

        oauthHelper.Should().NotBeNull("OAuthFlowHelper should be registered");
        tokenManager.Should().NotBeNull("ITokenManager should be registered");
        authOptions.Should().NotBeNull("ProcoreAuthOptions should be configured");

        var options = authOptions!.Value;
        options.ClientId.Should().NotBeNullOrEmpty("ClientId should be configured");
        options.RedirectUri.Should().NotBeNullOrEmpty("RedirectUri should be configured");
        options.Scopes.Should().NotBeEmpty("Scopes should be configured");
    }
}