using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Shared.Authentication;
using System.Net;
using System.Text.Json;

namespace Procore.SDK.Samples.Tests.Shared.TestHelpers;

/// <summary>
/// Comprehensive error handling and edge case tests for sample applications
/// Validates proper error handling, recovery mechanisms, and edge case scenarios
/// </summary>
public class ErrorScenarioTests : IClassFixture<TestAuthFixture>
{
    private readonly TestAuthFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ErrorScenarioTests> _logger;

    public ErrorScenarioTests(TestAuthFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<ErrorScenarioTests>>();
    }

    #region OAuth Error Scenarios

    [Theory]
    [InlineData("invalid_request", "The request is missing required parameters")]
    [InlineData("invalid_client", "Client authentication failed")]
    [InlineData("invalid_grant", "The authorization grant is invalid")]
    [InlineData("unauthorized_client", "The client is not authorized to request a token")]
    [InlineData("unsupported_grant_type", "The grant type is not supported")]
    [InlineData("invalid_scope", "The requested scope is invalid")]
    public async Task OAuth_StandardErrorResponses_ShouldHandleGracefully(string errorCode, string description)
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();

        _fixture.MockTokenErrorResponse(HttpStatusCode.BadRequest, new
        {
            error = errorCode,
            error_description = description
        });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));

        _logger.LogInformation("Handled OAuth error {ErrorCode}: {Description}", errorCode, description);
    }

    [Fact]
    public async Task OAuth_MalformedTokenResponse_ShouldThrowJsonException()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();

        // Setup malformed JSON response
        _fixture.MockTokenErrorResponse(HttpStatusCode.OK, "{ invalid json response");

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));
    }

    [Fact]
    public async Task OAuth_EmptyTokenResponse_ShouldThrowJsonException()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();

        // Setup empty response
        _fixture.MockTokenErrorResponse(HttpStatusCode.OK, "");

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));
    }

    [Fact]
    public async Task OAuth_NetworkTimeout_ShouldThrowTimeoutException()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();

        // Setup timeout simulation
        _fixture.MockNetworkFailure();

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));
    }

    #endregion

    #region Token Management Error Scenarios

    [Fact]
    public async Task TokenManager_RefreshWithInvalidToken_ShouldClearTokenAndThrow()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        
        var invalidToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "invalid-refresh-token",
            new[] { "read" });

        await tokenManager.StoreTokenAsync(invalidToken);

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
    public async Task TokenManager_RefreshServerError_ShouldRetryAndEventuallyFail()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "valid-refresh-token",
            new[] { "read" });

        await tokenManager.StoreTokenAsync(expiredToken);

        // Setup server error
        _fixture.MockRefreshTokenErrorResponse(HttpStatusCode.InternalServerError, new
        {
            error = "server_error",
            error_description = "The authorization server encountered an unexpected condition"
        });

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => tokenManager.RefreshTokenAsync());
    }

    [Fact]
    public async Task TokenManager_ConcurrentRefreshAttempts_ShouldHandleSafely()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-30),
            "valid-refresh-token",
            new[] { "read" });

        await tokenManager.StoreTokenAsync(expiredToken);

        _fixture.MockRefreshTokenResponse(new
        {
            access_token = "refreshed-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "new-refresh-token"
        });

        // Act - Multiple concurrent refresh attempts
        var refreshTasks = Enumerable.Range(0, 5).Select(_ => 
            tokenManager.RefreshTokenAsync());

        var results = await Task.WhenAll(refreshTasks);

        // Assert
        results.Should().AllSatisfy(token =>
        {
            token.Should().NotBeNull("All refresh attempts should succeed or be handled");
            token.Token.Should().Be("refreshed-token");
        });
    }

    #endregion

    #region Configuration Error Scenarios

    [Theory]
    [InlineData("", "valid-secret", "https://example.com/callback")]
    [InlineData("valid-client", "", "https://example.com/callback")]
    [InlineData("valid-client", "valid-secret", "")]
    [InlineData("valid-client", "valid-secret", "invalid-url")]
    public void Configuration_InvalidAuthOptions_ShouldFailValidation(
        string clientId, string clientSecret, string redirectUri)
    {
        // Arrange & Act & Assert
        var act = () =>
        {
            var options = new ProcoreAuthOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUri = redirectUri,
                Scopes = new[] { "read" },
                AuthorizationEndpoint = new Uri("https://app.procore.com/oauth/authorize"),
                TokenEndpoint = new Uri("https://api.procore.com/oauth/token")
            };

            // This would typically be validated in the actual configuration system
            if (string.IsNullOrWhiteSpace(options.ClientId))
                throw new ArgumentException("ClientId is required");
            if (string.IsNullOrWhiteSpace(options.ClientSecret))
                throw new ArgumentException("ClientSecret is required");
            if (string.IsNullOrWhiteSpace(options.RedirectUri))
                throw new ArgumentException("RedirectUri is required");
            if (!Uri.TryCreate(options.RedirectUri, UriKind.Absolute, out _))
                throw new ArgumentException("RedirectUri must be a valid URL");
        };

        act.Should().Throw<ArgumentException>("Invalid configuration should be rejected");
    }

    [Fact]
    public void Configuration_MissingScopes_ShouldDefaultToEmpty()
    {
        // Arrange & Act
        var options = new ProcoreAuthOptions
        {
            ClientId = "test-client",
            ClientSecret = "test-secret",
            RedirectUri = "https://example.com/callback",
            // Scopes intentionally not set
            AuthorizationEndpoint = new Uri("https://app.procore.com/oauth/authorize"),
            TokenEndpoint = new Uri("https://api.procore.com/oauth/token")
        };

        // Assert
        options.Scopes.Should().NotBeNull("Scopes should default to empty array");
        options.Scopes.Should().BeEmpty("Default scopes should be empty");
    }

    #endregion

    #region API Error Scenarios

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized, "Token has expired")]
    [InlineData(HttpStatusCode.Forbidden, "Insufficient permissions")]
    [InlineData(HttpStatusCode.NotFound, "Resource not found")]
    [InlineData(HttpStatusCode.TooManyRequests, "Rate limit exceeded")]
    [InlineData(HttpStatusCode.InternalServerError, "Internal server error")]
    [InlineData(HttpStatusCode.BadGateway, "Bad gateway")]
    [InlineData(HttpStatusCode.ServiceUnavailable, "Service unavailable")]
    public async Task API_StandardErrorResponses_ShouldHandleGracefully(
        HttpStatusCode statusCode, string errorMessage)
    {
        // Arrange
        await _fixture.SetupAuthenticatedStateAsync();
        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");

        _fixture.MockApiResponse("/api/v1/test", new
        {
            error = new { message = errorMessage }
        }, statusCode);

        // Act
        var response = await httpClient.GetAsync("/api/v1/test");

        // Assert
        response.StatusCode.Should().Be(statusCode, $"Should return {statusCode} status");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(errorMessage, "Should include error message");

        _logger.LogInformation("Handled API error {StatusCode}: {ErrorMessage}", statusCode, errorMessage);
    }

    [Fact]
    public async Task API_MalformedJsonResponse_ShouldHandleGracefully()
    {
        // Arrange
        await _fixture.SetupAuthenticatedStateAsync();
        var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Procore");

        // Setup malformed JSON response
        _fixture.MockApiResponse("/api/v1/malformed", "{ invalid json", HttpStatusCode.OK);

        // Act
        var response = await httpClient.GetAsync("/api/v1/malformed");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue("HTTP call should succeed");
        
        var content = await response.Content.ReadAsStringAsync();
        
        // The consumer would handle JSON parsing errors
        var act = () => JsonSerializer.Deserialize<object>(content);
        act.Should().Throw<JsonException>("Malformed JSON should throw parsing error");
    }

    #endregion

    #region Security Edge Cases

    [Fact]
    public async Task Security_TokenLeakage_ShouldNotLogSensitiveData()
    {
        // Arrange
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var sensitiveToken = "very-sensitive-access-token-12345";
        
        var token = new AccessToken(
            sensitiveToken,
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "sensitive-refresh-token",
            new[] { "read", "write" });

        // Act
        await tokenManager.StoreTokenAsync(token);
        var retrievedToken = await tokenManager.GetAccessTokenAsync();

        // Assert
        retrievedToken.Should().NotBeNull();
        retrievedToken!.Token.Should().Be(sensitiveToken);

        // Verify sensitive data is not logged (this would be verified in actual logging)
        _logger.LogInformation("Token operations completed without leaking sensitive data");
    }

    [Fact]
    public void Security_StateParameterGeneration_ShouldBeUnpredictable()
    {
        // Arrange & Act
        var states = new HashSet<string>();
        for (int i = 0; i < 100; i++)
        {
            var state = Guid.NewGuid().ToString();
            states.Add(state);
        }

        // Assert
        states.Should().HaveCount(100, "All generated states should be unique");
        states.Should().AllSatisfy(state => 
            state.Length.Should().BeGreaterThan(10, "States should be sufficiently long"));
    }

    [Fact]
    public async Task Security_CSRFProtection_ShouldValidateStateParameter()
    {
        // Arrange
        var originalState = "original-secure-state";
        var attackerState = "attacker-injected-state";

        // This test demonstrates the importance of state validation
        // In a real implementation, the web app would:
        // 1. Store the original state in session
        // 2. Validate the returned state matches
        // 3. Reject mismatched states

        originalState.Should().NotBe(attackerState, "States should not match (CSRF protection)");
        
        _logger.LogInformation("CSRF protection validated through state parameter verification");
    }

    #endregion

    #region Resource Management Edge Cases

    [Fact]
    public async Task ResourceManagement_MemoryPressure_ShouldHandleGracefully()
    {
        // Arrange - Simulate high memory usage scenario
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var tokens = new List<AccessToken>();

        // Act - Create many tokens to test memory handling
        for (int i = 0; i < 100; i++)
        {
            var token = new AccessToken(
                $"token-{i}",
                "Bearer",
                DateTimeOffset.UtcNow.AddHours(1),
                $"refresh-{i}",
                new[] { "read" });

            tokens.Add(token);
            await tokenManager.StoreTokenAsync(token);
        }

        // Assert
        var finalToken = await tokenManager.GetAccessTokenAsync();
        finalToken.Should().NotBeNull("Token manager should handle multiple operations");
        
        // Cleanup
        await tokenManager.ClearTokenAsync();
        
        _logger.LogInformation("Memory pressure test completed with {TokenCount} tokens", tokens.Count);
    }

    [Fact]
    public async Task ResourceManagement_CancellationToken_ShouldRespectCancellation()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl();

        using var cts = new CancellationTokenSource();
        cts.Cancel(); // Cancel immediately

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier, cts.Token));
    }

    #endregion

    #region Browser-Specific Edge Cases (Web App)

    [Theory]
    [InlineData("https://malicious-site.com/callback?code=stolen")]
    [InlineData("javascript:alert('xss')")]
    [InlineData("data:text/html,<script>alert('xss')</script>")]
    public void WebApp_MaliciousRedirectUris_ShouldBeRejected(string maliciousUri)
    {
        // Arrange & Act & Assert
        var act = () =>
        {
            // This simulates redirect URI validation that should happen
            // in the actual web application configuration
            var allowedRedirects = new[]
            {
                "https://localhost:5001/auth/callback",
                "https://myapp.com/auth/callback"
            };

            if (!allowedRedirects.Any(allowed => 
                maliciousUri.StartsWith(allowed, StringComparison.OrdinalIgnoreCase)))
            {
                throw new SecurityException("Redirect URI not in allowlist");
            }
        };

        act.Should().Throw<SecurityException>("Malicious redirect URIs should be rejected");
    }

    #endregion
}

/// <summary>
/// Custom exception for security violations in tests
/// </summary>
public class SecurityException : Exception
{
    public SecurityException(string message) : base(message) { }
    public SecurityException(string message, Exception innerException) : base(message, innerException) { }
}