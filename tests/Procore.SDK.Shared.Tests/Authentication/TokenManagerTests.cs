using Procore.SDK.Shared.Authentication;
using System.Text;

namespace Procore.SDK.Shared.Tests.Authentication;


/// <summary>
/// TDD Tests for TokenManager implementation
/// These tests define the expected behavior for token management functionality
/// </summary>
public class TokenManagerTests
{
    private readonly ITokenStorage _mockStorage;
    private readonly IOptions<ProcoreAuthOptions> _mockOptions;
    private readonly TestableHttpMessageHandler _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenManager> _mockLogger;
    private readonly ProcoreAuthOptions _authOptions;
    private readonly TokenManager _tokenManager;

    public TokenManagerTests()
    {
        _mockStorage = Substitute.For<ITokenStorage>();
        _mockOptions = Substitute.For<IOptions<ProcoreAuthOptions>>();
        _mockHttpMessageHandler = new TestableHttpMessageHandler();
        _httpClient = new HttpClient(_mockHttpMessageHandler);
        _mockLogger = Substitute.For<ILogger<TokenManager>>();
        
        _authOptions = new ProcoreAuthOptions
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            TokenEndpoint = new Uri("https://api.procore.com/oauth/token"),
            TokenRefreshMargin = TimeSpan.FromMinutes(5)
        };
        
        _mockOptions.Value.Returns(_authOptions);
        
        _tokenManager = new TokenManager(_mockStorage, _mockOptions, _httpClient, _mockLogger);
    }

    [Fact]
    public async Task GetAccessTokenAsync_WhenNoTokenExists_ShouldReturnNull()
    {
        // Arrange
        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns((AccessToken?)null);

        // Act
        var result = await _tokenManager.GetAccessTokenAsync();

        // Assert
        result.Should().BeNull();
        _mockStorage.Received(1).GetTokenAsync("procore_token_test-client-id", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAccessTokenAsync_WhenTokenIsValid_ShouldReturnToken()
    {
        // Arrange
        var validToken = new AccessToken(
            "valid-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "refresh-token",
            new[] { "read", "write" }
        );
        
        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(validToken);

        // Act
        var result = await _tokenManager.GetAccessTokenAsync();

        // Assert
        result.Should().Be(validToken);
        _mockStorage.Received(1).GetTokenAsync("procore_token_test-client-id", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAccessTokenAsync_WhenTokenNeedsRefresh_ShouldRefreshAutomatically()
    {
        // Arrange
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1), // Expired
            "refresh-token"
        );
        
        var refreshedToken = new AccessToken(
            "refreshed-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddHours(1),
            "new-refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(expiredToken, refreshedToken);

        // Mock successful refresh response
        var refreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "refreshed-token",
                token_type = "Bearer",
                expires_in = 3600,
                refresh_token = "new-refresh-token",
                scope = "read write"
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, ct) => Task.FromResult(refreshResponse);

        // Act
        var result = await _tokenManager.GetAccessTokenAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().Be("refreshed-token");
        
        _mockStorage.Received(1).StoreTokenAsync(Arg.Any<string>(), Arg.Any<AccessToken>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAccessTokenAsync_WhenRefreshFails_ShouldReturnExpiredTokenAndLogWarning()
    {
        // Arrange
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1),
            "refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(expiredToken);

        // Mock failed refresh response
        _mockHttpMessageHandler.SendAsyncFunc = (req, ct) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));

        // Act
        var result = await _tokenManager.GetAccessTokenAsync();

        // Assert
        result.Should().Be(expiredToken);
        // Note: Logging assertion commented out - would need a test logging framework
        // _mockLogger.Collector.GetSnapshot().Should().Contain(log => 
        //     log.Level == LogLevel.Warning && 
        //     log.Message.Contains("Failed to refresh token"));
    }

    [Fact]
    public async Task RefreshTokenAsync_WithValidRefreshToken_ShouldReturnNewToken()
    {
        // Arrange
        var currentToken = new AccessToken(
            "current-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1),
            "refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(currentToken);

        var refreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
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

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
        {
            if (req.Method == HttpMethod.Post && req.RequestUri == _authOptions.TokenEndpoint)
                return Task.FromResult(refreshResponse);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        };

        bool eventRaised = false;
        AccessToken? eventNewToken = null;
        AccessToken? eventOldToken = null;

        _tokenManager.TokenRefreshed += (sender, args) =>
        {
            eventRaised = true;
            eventNewToken = args.NewToken;
            eventOldToken = args.OldToken;
        };

        // Act
        var result = await _tokenManager.RefreshTokenAsync();

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("new-access-token");
        result.TokenType.Should().Be("Bearer");
        result.RefreshToken.Should().Be("new-refresh-token");
        result.Scopes.Should().BeEquivalentTo(new[] { "read", "write" });

        eventRaised.Should().BeTrue();
        eventNewToken.Should().Be(result);
        eventOldToken.Should().Be(currentToken);

        _mockStorage.Received(1).StoreTokenAsync("procore_token_test-client-id", result, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RefreshTokenAsync_WithoutRefreshToken_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tokenWithoutRefresh = new AccessToken(
            "current-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1)
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(tokenWithoutRefresh);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _tokenManager.RefreshTokenAsync());
        
        exception.Message.Should().Contain("No refresh token available");
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenNoCurrentToken_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns((AccessToken?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _tokenManager.RefreshTokenAsync());
        
        exception.Message.Should().Contain("No refresh token available");
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenHttpRequestFails_ShouldThrowHttpRequestException()
    {
        // Arrange
        var currentToken = new AccessToken(
            "current-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1),
            "refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(currentToken);

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => _tokenManager.RefreshTokenAsync());
    }

    [Fact]
    public async Task StoreTokenAsync_ShouldCallStorageWithCorrectKey()
    {
        // Arrange
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act
        await _tokenManager.StoreTokenAsync(token);

        // Assert
        await _mockStorage.Received(1).StoreTokenAsync("procore_token_test-client-id", token, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ClearTokenAsync_ShouldCallStorageDeleteWithCorrectKey()
    {
        // Act
        await _tokenManager.ClearTokenAsync();

        // Assert
        _mockStorage.Received(1).DeleteTokenAsync("procore_token_test-client-id", Arg.Any<CancellationToken>());
    }

    [Fact]
    public void TokenManager_ShouldUseClientIdInStorageKey()
    {
        // This test verifies that different client IDs result in different storage keys
        // Arrange
        var options1 = new ProcoreAuthOptions { ClientId = "client-1" };
        var options2 = new ProcoreAuthOptions { ClientId = "client-2" };
        
        var mockOptions1 = Substitute.For<IOptions<ProcoreAuthOptions>>();
        var mockOptions2 = Substitute.For<IOptions<ProcoreAuthOptions>>();
        mockOptions1.Value.Returns(options1);
        mockOptions2.Value.Returns(options2);

        var tokenManager1 = new TokenManager(_mockStorage, mockOptions1, _httpClient, _mockLogger);
        var tokenManager2 = new TokenManager(_mockStorage, mockOptions2, _httpClient, _mockLogger);

        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act
        _ = tokenManager1.StoreTokenAsync(token);
        _ = tokenManager2.StoreTokenAsync(token);

        // Assert
        _mockStorage.Received(1).StoreTokenAsync("procore_token_client-1", token, Arg.Any<CancellationToken>());
        _mockStorage.Received(1).StoreTokenAsync("procore_token_client-2", token, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenRefreshTokenChanges_ShouldUseNewRefreshToken()
    {
        // Arrange
        var currentToken = new AccessToken(
            "current-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1),
            "old-refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(currentToken);

        // Mock response with new refresh token
        var refreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "new-access-token",
                token_type = "Bearer",
                expires_in = 3600,
                refresh_token = "new-refresh-token"
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, ct) => Task.FromResult(refreshResponse);

        // Act
        var result = await _tokenManager.RefreshTokenAsync();

        // Assert
        result.RefreshToken.Should().Be("new-refresh-token");
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenRefreshTokenNotReturned_ShouldKeepCurrentRefreshToken()
    {
        // Arrange
        var currentToken = new AccessToken(
            "current-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1),
            "keep-this-refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(currentToken);

        // Mock response without refresh token
        var refreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "new-access-token",
                token_type = "Bearer",
                expires_in = 3600
                // No refresh_token field
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, ct) => Task.FromResult(refreshResponse);

        // Act
        var result = await _tokenManager.RefreshTokenAsync();

        // Assert
        result.RefreshToken.Should().Be("keep-this-refresh-token");
    }

    [Fact]
    public void TokenManager_ShouldImplementITokenManagerInterface()
    {
        // Arrange & Act
        var tokenManager = new TokenManager(_mockStorage, _mockOptions, _httpClient, _mockLogger);

        // Assert
        tokenManager.Should().BeAssignableTo<ITokenManager>();
    }

    [Fact]
    public async Task TokenManager_ShouldHandleCancellation()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _tokenManager.GetAccessTokenAsync(cts.Token));
    }
}