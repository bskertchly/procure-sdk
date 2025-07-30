using Procore.SDK.Shared.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Procore.SDK.Shared.Tests.Authentication;

/// <summary>
/// TDD Tests for OAuthFlowHelper with PKCE implementation
/// These tests define the expected behavior for OAuth flow and PKCE code generation
/// </summary>
public class OAuthFlowHelperTests
{
    private readonly IOptions<ProcoreAuthOptions> _mockOptions;
    private readonly TestableHttpMessageHandler _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ProcoreAuthOptions _authOptions;
    private readonly OAuthFlowHelper _oAuthFlowHelper;

    public OAuthFlowHelperTests()
    {
        _mockOptions = Substitute.For<IOptions<ProcoreAuthOptions>>();
        _mockHttpMessageHandler = new TestableHttpMessageHandler();
        _httpClient = new HttpClient(_mockHttpMessageHandler);
        
        _authOptions = new ProcoreAuthOptions
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            RedirectUri = new Uri("https://myapp.com/oauth/callback"),
            Scopes = new[] { "read", "write", "admin" },
            AuthorizationEndpoint = new Uri("https://app.procore.com/oauth/authorize"),
            TokenEndpoint = new Uri("https://api.procore.com/oauth/token")
        };
        
        _mockOptions.Value.Returns(_authOptions);
        _oAuthFlowHelper = new OAuthFlowHelper(_mockOptions, _httpClient);
    }

    [Fact]
    public void GenerateAuthorizationUrl_ShouldReturnUrlAndCodeVerifier()
    {
        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl();

        // Assert
        authorizeUrl.Should().NotBeNullOrEmpty("Authorization URL should be generated");
        codeVerifier.Should().NotBeNullOrEmpty("Code verifier should be generated");
        
        authorizeUrl.Should().StartWith(_authOptions.AuthorizationEndpoint.ToString(),
            "Authorization URL should start with the configured endpoint");
    }

    [Fact]
    public void GenerateAuthorizationUrl_ShouldIncludeRequiredParameters()
    {
        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl();

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        query["response_type"].Should().Be("code", "Response type should be 'code' for authorization code flow");
        query["client_id"].Should().Be(_authOptions.ClientId, "Client ID should match configuration");
        query["redirect_uri"].Should().Be(_authOptions.RedirectUri.ToString(), "Redirect URI should match configuration");
        query["scope"].Should().Be("read write admin", "Scopes should be space-separated");
        query["code_challenge_method"].Should().Be("S256", "PKCE challenge method should be SHA256");
        query["code_challenge"].Should().NotBeNullOrEmpty("Code challenge should be present");
    }

    [Fact]
    public void GenerateAuthorizationUrl_WithState_ShouldIncludeStateParameter()
    {
        // Arrange
        var state = "random-state-value-123";

        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl(state);

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        query["state"].Should().Be(state, "State parameter should be included when provided");
    }

    [Fact]
    public void GenerateAuthorizationUrl_WithoutState_ShouldNotIncludeStateParameter()
    {
        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl();

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        query["state"].Should().BeNull("State parameter should not be included when not provided");
    }

    [Fact]
    public void GenerateAuthorizationUrl_ShouldGenerateValidCodeVerifier()
    {
        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl();

        // Assert
        codeVerifier.Should().NotBeNullOrEmpty("Code verifier should not be empty");
        codeVerifier.Length.Should().BeGreaterOrEqualTo(43, "Code verifier should be at least 43 characters per RFC 7636");
        codeVerifier.Length.Should().BeLessOrEqualTo(128, "Code verifier should be at most 128 characters per RFC 7636");
        
        // Should only contain URL-safe base64 characters
        var urlSafeBase64Pattern = @"^[A-Za-z0-9\-_]+$";
        Regex.IsMatch(codeVerifier, urlSafeBase64Pattern).Should().BeTrue(
            "Code verifier should only contain URL-safe base64 characters");
    }

    [Fact]
    public void GenerateAuthorizationUrl_ShouldGenerateValidCodeChallenge()
    {
        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl();

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var codeChallenge = query["code_challenge"];

        codeChallenge.Should().NotBeNullOrEmpty("Code challenge should be present");
        
        // Verify that the code challenge is the correct SHA256 hash of the verifier
        var expectedChallenge = GenerateExpectedCodeChallenge(codeVerifier);
        codeChallenge.Should().Be(expectedChallenge, "Code challenge should be SHA256 hash of code verifier");
    }

    [Fact]
    public void GenerateAuthorizationUrl_ShouldGenerateUniqueVerifiers()
    {
        // Act
        var (url1, verifier1) = _oAuthFlowHelper.GenerateAuthorizationUrl();
        var (url2, verifier2) = _oAuthFlowHelper.GenerateAuthorizationUrl();

        // Assert
        verifier1.Should().NotBe(verifier2, "Each call should generate a unique code verifier");
        url1.Should().NotBe(url2, "Each URL should contain a unique code challenge");
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_WithValidCode_ShouldReturnAccessToken()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";

        var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "new-access-token",
                token_type = "Bearer",
                expires_in = 3600,
                refresh_token = "new-refresh-token",
                scope = "read write"
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, ct) =>
        {
            req.Method.Should().Be(HttpMethod.Post);
            req.RequestUri.Should().Be(_authOptions.TokenEndpoint);
            return Task.FromResult(tokenResponse);
        };

        // Act
        var result = await _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("new-access-token");
        result.TokenType.Should().Be("Bearer");
        result.RefreshToken.Should().Be("new-refresh-token");
        result.Scopes.Should().BeEquivalentTo(new[] { "read", "write" });
        result.ExpiresAt.Should().BeCloseTo(DateTimeOffset.UtcNow.AddSeconds(3600), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_ShouldSendCorrectRequestParameters()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";

        var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "test-token",
                token_type = "Bearer",
                expires_in = 3600
            }), Encoding.UTF8, "application/json")
        };

        HttpRequestMessage? capturedRequest = null;
        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
        {
            capturedRequest = req;
            return Task.FromResult(tokenResponse);
        };

        // Act
        await _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri.Should().Be(_authOptions.TokenEndpoint);
        capturedRequest.Content.Should().BeOfType<FormUrlEncodedContent>();

        var formContent = await capturedRequest.Content!.ReadAsStringAsync();
        formContent.Should().Contain("grant_type=authorization_code");
        formContent.Should().Contain($"code={Uri.EscapeDataString(authCode)}");
        formContent.Should().Contain($"redirect_uri={Uri.EscapeDataString(_authOptions.RedirectUri.ToString())}");
        formContent.Should().Contain($"client_id={Uri.EscapeDataString(_authOptions.ClientId)}");
        formContent.Should().Contain($"client_secret={Uri.EscapeDataString(_authOptions.ClientSecret)}");
        formContent.Should().Contain($"code_verifier={Uri.EscapeDataString(codeVerifier)}");
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_WhenHttpRequestFails_ShouldThrowHttpRequestException()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier));
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_WithInvalidJson_ShouldThrowJsonException()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";

        var invalidJsonResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("invalid json", Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(invalidJsonResponse);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(
            () => _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier));
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_WithoutRefreshToken_ShouldReturnTokenWithNullRefreshToken()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";

        var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "access-token-only",
                token_type = "Bearer",
                expires_in = 3600
                // No refresh_token
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(tokenResponse);

        // Act
        var result = await _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier);

        // Assert
        result.RefreshToken.Should().BeNull();
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_WithoutScope_ShouldReturnTokenWithNullScopes()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";

        var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "access-token",
                token_type = "Bearer",
                expires_in = 3600
                // No scope
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(tokenResponse);

        // Act
        var result = await _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier);

        // Assert
        result.Scopes.Should().BeNull();
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        var authCode = "test-authorization-code";
        var codeVerifier = "test-code-verifier";
        
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => _oAuthFlowHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier, cts.Token));
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GenerateAuthorizationUrl_WithEmptyOrNullState_ShouldNotIncludeState(string? state)
    {
        // Act
        var (authorizeUrl, codeVerifier) = _oAuthFlowHelper.GenerateAuthorizationUrl(state);

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        if (string.IsNullOrEmpty(state))
        {
            query["state"].Should().BeNull("Empty or null state should not be included");
        }
    }

    [Fact]
    public void GenerateAuthorizationUrl_ShouldUrlEncodeParameters()
    {
        // Arrange
        var specialCharsOptions = new ProcoreAuthOptions
        {
            ClientId = "client+with spaces&special=chars",
            RedirectUri = new Uri("https://example.com/callback?param=value&other=test"),
            Scopes = new[] { "read write", "admin:all" }
        };

        var mockOptionsWithSpecialChars = Substitute.For<IOptions<ProcoreAuthOptions>>();
        mockOptionsWithSpecialChars.Value.Returns(specialCharsOptions);
        var helper = new OAuthFlowHelper(mockOptionsWithSpecialChars, _httpClient);

        // Act
        var (authorizeUrl, codeVerifier) = helper.GenerateAuthorizationUrl();

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        query["client_id"].Should().Be(specialCharsOptions.ClientId, "Client ID should be properly decoded");
        query["redirect_uri"].Should().Be(specialCharsOptions.RedirectUri.ToString(), "Redirect URI should be properly decoded");
        query["scope"].Should().Be("read write admin:all", "Scopes should be properly decoded");
    }

    [Fact] 
    public void GenerateAuthorizationUrl_ShouldHandleEmptyScopes()
    {
        // Arrange
        var emptyScopes = new ProcoreAuthOptions
        {
            ClientId = "test-client",
            RedirectUri = new Uri("https://example.com/callback"),
            Scopes = Array.Empty<string>()
        };

        var mockOptionsWithEmptyScopes = Substitute.For<IOptions<ProcoreAuthOptions>>();
        mockOptionsWithEmptyScopes.Value.Returns(emptyScopes);
        var helper = new OAuthFlowHelper(mockOptionsWithEmptyScopes, _httpClient);

        // Act
        var (authorizeUrl, codeVerifier) = helper.GenerateAuthorizationUrl();

        // Assert
        var uri = new Uri(authorizeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        query["scope"].Should().Be("", "Empty scopes should result in empty scope parameter");
    }

    /// <summary>
    /// Helper method to generate expected code challenge for verification
    /// This replicates the PKCE SHA256 challenge generation logic
    /// </summary>
    private static string GenerateExpectedCodeChallenge(string codeVerifier)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}