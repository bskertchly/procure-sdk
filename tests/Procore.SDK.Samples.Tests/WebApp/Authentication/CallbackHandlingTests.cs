using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Procore.SDK.Shared.Authentication;
using System.Security.Claims;

namespace Procore.SDK.Samples.Tests.WebApp.Authentication;

/// <summary>
/// Tests for web application OAuth callback handling
/// Validates proper handling of authorization callbacks, state validation, and error scenarios
/// </summary>
public class CallbackHandlingTests : IClassFixture<WebTestFixture>
{
    private readonly WebTestFixture _fixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CallbackHandlingTests> _logger;

    public CallbackHandlingTests(WebTestFixture fixture)
    {
        _fixture = fixture;
        _serviceProvider = _fixture.ServiceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<CallbackHandlingTests>>();
    }

    [Fact]
    public async Task WebApp_ValidCallback_ShouldExchangeCodeAndSetAuthentication()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "valid-authorization-code";
        var state = "web-app-state-12345";
        var codeVerifier = "stored-code-verifier-for-session";

        // Store code verifier in session (simulated)
        _fixture.MockSessionStorage(state, codeVerifier);

        // Setup mock token response
        _fixture.MockTokenResponse(new
        {
            access_token = "web-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            refresh_token = "web-refresh-token",
            scope = "read write admin"
        });

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={state}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue("Callback should process successfully");
        
        // Verify redirect to home page
        response.Headers.Location?.ToString().Should().Contain("/", "Should redirect to home after auth");
        
        // Verify authentication cookie is set
        var cookies = response.Headers.GetValues("Set-Cookie");
        cookies.Should().Contain(cookie => cookie.Contains("auth"), "Should set authentication cookie");
    }

    [Fact]
    public async Task WebApp_CallbackWithInvalidState_ShouldRejectAndRedirectToError()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "valid-authorization-code";
        var invalidState = "tampered-state-value";
        var expectedState = "original-state-value";

        // Store different state in session
        _fixture.MockSessionStorage(expectedState, "code-verifier");

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={invalidState}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should reject invalid state");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Invalid state parameter", "Should show state validation error");
    }

    [Fact]
    public async Task WebApp_CallbackWithMissingCode_ShouldShowAuthorizationError()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var state = "valid-state";
        var error = "access_denied";
        var errorDescription = "User denied the authorization request";

        // Act
        var response = await client.GetAsync(
            $"/auth/callback?error={error}&error_description={Uri.EscapeDataString(errorDescription)}&state={state}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should handle authorization errors");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("access_denied", "Should display the authorization error");
        content.Should().Contain("User denied", "Should display the error description");
    }

    [Fact]
    public async Task WebApp_CallbackTokenExchangeFailure_ShouldHandleGracefully()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "invalid-authorization-code";
        var state = "valid-state";
        var codeVerifier = "valid-code-verifier";

        _fixture.MockSessionStorage(state, codeVerifier);

        // Setup mock token error response
        _fixture.MockTokenErrorResponse(HttpStatusCode.BadRequest, new
        {
            error = "invalid_grant",
            error_description = "Authorization code is invalid or expired"
        });

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={state}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should handle token exchange errors");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("invalid_grant", "Should display token exchange error");
    }

    [Fact]
    public async Task WebApp_CallbackWithExpiredSession_ShouldRequireNewAuthentication()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "valid-authorization-code";
        var state = "expired-session-state";

        // Don't store any session data (simulates expired session)
        _fixture.ClearSessionStorage();

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={state}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should reject callback without session");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Session expired", "Should indicate session expiration");
    }

    [Fact]
    public async Task WebApp_ConcurrentCallbacks_ShouldHandleSafely()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "concurrent-auth-code";
        var state = "concurrent-state";
        var codeVerifier = "concurrent-code-verifier";

        _fixture.MockSessionStorage(state, codeVerifier);
        _fixture.MockTokenResponse(new
        {
            access_token = "concurrent-token",
            token_type = "Bearer",
            expires_in = 3600
        });

        // Act - Multiple concurrent callback requests
        var tasks = Enumerable.Range(0, 5).Select(_ => 
            client.GetAsync($"/auth/callback?code={authCode}&state={state}"));
        
        var responses = await Task.WhenAll(tasks);

        // Assert
        // Only one should succeed, others should be handled gracefully
        var successfulResponses = responses.Count(r => r.IsSuccessStatusCode);
        successfulResponses.Should().BeLessOrEqualTo(1, "Only one concurrent callback should succeed");
        
        var failedResponses = responses.Where(r => !r.IsSuccessStatusCode).ToArray();
        failedResponses.Should().AllSatisfy(response =>
        {
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest, 
                HttpStatusCode.Conflict,
                "Failed responses should have appropriate status codes");
        });
    }

    [Fact]
    public async Task WebApp_CallbackStoresUserClaims_ShouldIncludeTokenInformation()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "claims-auth-code";
        var state = "claims-state";
        var codeVerifier = "claims-code-verifier";

        _fixture.MockSessionStorage(state, codeVerifier);
        _fixture.MockTokenResponse(new
        {
            access_token = "claims-access-token",
            token_type = "Bearer",
            expires_in = 3600,
            scope = "read write admin"
        });

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={state}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        // Verify claims are set properly
        var authenticationTicket = _fixture.GetAuthenticationTicket();
        authenticationTicket.Should().NotBeNull("Authentication ticket should be created");
        
        var claims = authenticationTicket!.Principal.Claims.ToArray();
        claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier, "Should include user identifier");
        claims.Should().Contain(c => c.Type == "scope" && c.Value.Contains("read"), "Should include scopes");
        claims.Should().Contain(c => c.Type == "token_type" && c.Value == "Bearer", "Should include token type");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task WebApp_CallbackWithEmptyParameters_ShouldRejectGracefully(string? invalidParam)
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act & Assert
        var response = await client.GetAsync($"/auth/callback?code={invalidParam}&state=valid-state");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should reject empty/null code");

        response = await client.GetAsync($"/auth/callback?code=valid-code&state={invalidParam}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should reject empty/null state");
    }

    [Fact]
    public async Task WebApp_CallbackWithMaliciousState_ShouldPreventCSRFAttacks()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var maliciousCode = "<script>alert('xss')</script>";
        var maliciousState = "javascript:alert('csrf')";

        // Act
        var response = await client.GetAsync(
            $"/auth/callback?code={Uri.EscapeDataString(maliciousCode)}&state={Uri.EscapeDataString(maliciousState)}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Should reject malicious state");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotContain("<script>", "Should not render malicious scripts");
        content.Should().NotContain("javascript:", "Should not execute malicious JavaScript");
    }

    [Fact]
    public async Task WebApp_CallbackLogging_ShouldLogSecurityEvents()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "logged-auth-code";
        var invalidState = "invalid-state";

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={invalidState}");

        // Assert
        var logEntries = _fixture.GetLogEntries<CallbackHandlingTests>();
        logEntries.Should().Contain(entry => 
            entry.LogLevel == LogLevel.Warning && 
            entry.Message.Contains("Invalid state parameter"),
            "Should log security warning for invalid state");
    }

    [Fact]
    public async Task WebApp_SuccessfulCallback_ShouldCleanupSession()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var authCode = "cleanup-auth-code";
        var state = "cleanup-state";
        var codeVerifier = "cleanup-code-verifier";

        _fixture.MockSessionStorage(state, codeVerifier);
        _fixture.MockTokenResponse(new
        {
            access_token = "cleanup-token",
            token_type = "Bearer",
            expires_in = 3600
        });

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={state}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        // Verify session cleanup
        _fixture.VerifySessionCleanup(state).Should().BeTrue(
            "Session should be cleaned up after successful authentication");
    }
}