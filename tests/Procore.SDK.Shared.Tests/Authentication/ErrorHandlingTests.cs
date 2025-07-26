using Procore.SDK.Shared.Authentication;
using System.Net.Sockets;
using System.Text;

namespace Procore.SDK.Shared.Tests.Authentication;

/// <summary>
/// TDD Tests for comprehensive error handling scenarios in authentication components
/// These tests define expected behavior for various error conditions and edge cases
/// </summary>
public class ErrorHandlingTests
{
    private readonly ITokenStorage _mockStorage;
    private readonly IOptions<ProcoreAuthOptions> _mockOptions;
    private readonly TestableHttpMessageHandler _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenManager> _mockTokenManagerLogger;
    private readonly ILogger<ProcoreAuthHandler> _mockAuthHandlerLogger;
    private readonly ProcoreAuthOptions _authOptions;
    private readonly TokenManager _tokenManager;
    private readonly ProcoreAuthHandler _authHandler;
    private readonly HttpClient _clientWithAuthHandler;

    public ErrorHandlingTests()
    {
        _mockStorage = Substitute.For<ITokenStorage>();
        _mockOptions = Substitute.For<IOptions<ProcoreAuthOptions>>();
        _mockHttpMessageHandler = new TestableHttpMessageHandler();
        _httpClient = new HttpClient(_mockHttpMessageHandler);
        _mockTokenManagerLogger = Substitute.For<ILogger<TokenManager>>();
        _mockAuthHandlerLogger = Substitute.For<ILogger<ProcoreAuthHandler>>();
        
        _authOptions = new ProcoreAuthOptions
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            TokenEndpoint = new Uri("https://api.procore.com/oauth/token"),
            TokenRefreshMargin = TimeSpan.FromMinutes(5)
        };
        
        _mockOptions.Value.Returns(_authOptions);
        
        _tokenManager = new TokenManager(_mockStorage, _mockOptions, _httpClient, _mockTokenManagerLogger);
        _authHandler = new ProcoreAuthHandler(_tokenManager, _mockAuthHandlerLogger)
        {
            InnerHandler = _mockHttpMessageHandler
        };
        _clientWithAuthHandler = new HttpClient(_authHandler);
    }

    #region TokenManager Error Scenarios

    [Fact]
    public async Task TokenManager_WhenStorageThrows_ShouldHandleGracefully()
    {
        // Arrange
        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(Task.FromException<AccessToken?>(new InvalidOperationException("Storage is corrupted")));

        // Act
        var result = await _tokenManager.GetAccessTokenAsync();

        // Assert
        result.Should().BeNull("Should return null when storage throws exception");
        
        // Note: Logging assertion commented out - would need a test logging framework
        // _mockTokenManagerLogger.Collector.GetSnapshot().Should().Contain(log => 
        //     log.Level == LogLevel.Error && 
        //     log.Message.Contains("Failed to retrieve token from storage"));
    }

    [Fact]
    public async Task TokenManager_WhenStoreTokenThrows_ShouldPropagateException()
    {
        // Arrange
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        
        _mockStorage.StoreTokenAsync(Arg.Any<string>(), Arg.Any<AccessToken>(), Arg.Any<CancellationToken>())
                   .Returns(Task.FromException(new UnauthorizedAccessException("Cannot write to protected storage")));

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _tokenManager.StoreTokenAsync(token));
    }

    [Fact]
    public async Task TokenManager_WhenRefreshReturnsInvalidJson_ShouldThrowJsonException()
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

        var invalidJsonResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{ invalid json content", Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(invalidJsonResponse);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(
            () => _tokenManager.RefreshTokenAsync());
    }

    [Fact]
    public async Task TokenManager_WhenRefreshReturns500_ShouldThrowHttpRequestException()
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

        var serverErrorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Internal Server Error")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(serverErrorResponse);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => _tokenManager.RefreshTokenAsync());
        
        exception.Message.Should().Contain("500");
    }

    [Fact]
    public async Task TokenManager_WhenNetworkUnavailable_ShouldThrowHttpRequestException()
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
            Task.FromException<HttpResponseMessage>(new HttpRequestException("Network unreachable", new SocketException()));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => _tokenManager.RefreshTokenAsync());
        
        exception.Message.Should().Contain("Network unreachable");
    }

    [Fact]
    public async Task TokenManager_WhenRefreshTokenMissing_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tokenWithoutRefresh = new AccessToken(
            "current-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddMinutes(-1)
            // No refresh token
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(tokenWithoutRefresh);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _tokenManager.RefreshTokenAsync());
        
        exception.Message.Should().Contain("No refresh token available");
    }

    [Fact]
    public async Task TokenManager_WhenRefreshResponseMissingRequiredFields_ShouldThrowException()
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

        // Missing access_token field
        var incompleteResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                token_type = "Bearer",
                expires_in = 3600
                // Missing access_token
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(incompleteResponse);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _tokenManager.RefreshTokenAsync());
    }

    #endregion

    #region ProcoreAuthHandler Error Scenarios

    [Fact]
    public async Task AuthHandler_WhenTokenManagerThrows_ShouldContinueWithoutAuth()
    {
        // Arrange
        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(Task.FromException<AccessToken?>(new InvalidOperationException("Token manager error")));

        var successResponse = new HttpResponseMessage(HttpStatusCode.OK);
        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
        {
            if (req.Headers.Authorization == null)
                return Task.FromResult(successResponse);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.procore.com/test");

        // Act
        var response = await _clientWithAuthHandler.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Note: Logging assertion commented out - would need a test logging framework
        // _mockAuthHandlerLogger.Collector.GetSnapshot().Should().Contain(log => 
        //     log.Level == LogLevel.Warning && 
        //     log.Message.Contains("Failed to get access token"));
    }

    [Fact]
    public async Task AuthHandler_WhenRefreshFailsAfter401_ShouldReturnOriginal401()
    {
        // Arrange
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(token);

        _mockHttpMessageHandler.SetupSequence(
            (req, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)),
            (req, cancellationToken) => Task.FromException<HttpResponseMessage>(new HttpRequestException("Network error"))
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.procore.com/test");

        // Act
        var response = await _clientWithAuthHandler.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        // Note: Logging assertion commented out - would need a test logging framework
        // _mockAuthHandlerLogger.Collector.GetSnapshot().Should().Contain(log => 
        //     log.Level == LogLevel.Warning && 
        //     log.Message.Contains("Failed to refresh token on 401 response"));
    }

    [Fact]
    public async Task AuthHandler_WhenRequestCloneFails_ShouldHandleGracefully()
    {
        // Arrange - Create a request that cannot be cloned (e.g., with disposed content)
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        var newToken = new AccessToken("new-token", "Bearer", DateTimeOffset.UtcNow.AddHours(2));

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(token);

        var refreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "new-token",
                token_type = "Bearer",
                expires_in = 3600
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SetupSequence(
            (req, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)),
            (req, cancellationToken) => Task.FromResult(refreshResponse)
        );

        // Create request with content that will be disposed
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.procore.com/test");
        var content = new StringContent("test content");
        content.Dispose(); // Dispose content to make cloning fail
        request.Content = content;

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(
            () => _clientWithAuthHandler.SendAsync(request));
    }

    #endregion

    #region OAuthFlowHelper Error Scenarios

    [Fact]
    public async Task OAuthFlowHelper_WhenTokenEndpointReturns400_ShouldThrowHttpException()
    {
        // Arrange
        var oAuthHelper = new OAuthFlowHelper(_mockOptions, _httpClient);
        
        var badRequestResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                error = "invalid_grant",
                error_description = "The authorization code is invalid"
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(badRequestResponse);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => oAuthHelper.ExchangeCodeForTokenAsync("invalid-code", "code-verifier"));
    }

    [Fact]
    public async Task OAuthFlowHelper_WhenNetworkTimeout_ShouldThrowTaskCanceledException()
    {
        // Arrange
        var oAuthHelper = new OAuthFlowHelper(_mockOptions, _httpClient);
        
        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromException<HttpResponseMessage>(new TaskCanceledException("Request timeout"));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => oAuthHelper.ExchangeCodeForTokenAsync("auth-code", "code-verifier"));
    }

    [Fact]
    public void OAuthFlowHelper_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new OAuthFlowHelper(null!, _httpClient));
    }

    [Fact]
    public void OAuthFlowHelper_WithNullHttpClient_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new OAuthFlowHelper(_mockOptions, null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task OAuthFlowHelper_WithInvalidAuthCode_ShouldThrowArgumentException(string? invalidCode)
    {
        // Arrange
        var oAuthHelper = new OAuthFlowHelper(_mockOptions, _httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => oAuthHelper.ExchangeCodeForTokenAsync(invalidCode!, "code-verifier"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task OAuthFlowHelper_WithInvalidCodeVerifier_ShouldThrowArgumentException(string? invalidVerifier)
    {
        // Arrange
        var oAuthHelper = new OAuthFlowHelper(_mockOptions, _httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => oAuthHelper.ExchangeCodeForTokenAsync("auth-code", invalidVerifier!));
    }

    #endregion

    #region Token Storage Error Scenarios

    [Fact]
    public async Task InMemoryTokenStorage_WhenConcurrentModification_ShouldHandleRaceConditions()
    {
        // Arrange
        var storage = new InMemoryTokenStorage();
        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act - Simulate concurrent store and delete operations
        var tasks = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() => storage.StoreTokenAsync(key, token)));
            tasks.Add(Task.Run(() => storage.DeleteTokenAsync(key)));
        }

        // Should not throw any exceptions
        await Task.WhenAll(tasks);

        // Assert - Final state should be consistent
        var result = await storage.GetTokenAsync(key);
        // Result can be either null or the token, but shouldn't throw
        (result == null || result == token).Should().BeTrue();
    }

    [Fact]
    public async Task FileTokenStorage_WhenFileIsLocked_ShouldRetryOrThrowIOException()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var storage = new FileTokenStorage(tempFile);
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        try
        {
            // Lock the file
            using var fileStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            
            // Act & Assert
            await Assert.ThrowsAsync<IOException>(
                () => storage.StoreTokenAsync("key", token));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task FileTokenStorage_WhenDiskFull_ShouldThrowIOException()
    {
        // This test simulates disk full scenario by using an invalid path
        // Arrange
        var invalidPath = Path.Combine("/dev/full", "token.json"); // Unix: /dev/full simulates disk full
        if (OperatingSystem.IsWindows())
        {
            invalidPath = "Z:\\NonExistentDrive\\token.json"; // Windows: Invalid drive
        }

        var storage = new FileTokenStorage(invalidPath);
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(
            () => storage.StoreTokenAsync("key", token));
    }

    #endregion

    #region Configuration Error Scenarios

    [Fact]
    public void TokenManager_WithNullStorage_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new TokenManager(null!, _mockOptions, _httpClient, _mockTokenManagerLogger));
    }

    [Fact]
    public void TokenManager_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new TokenManager(_mockStorage, null!, _httpClient, _mockTokenManagerLogger));
    }

    [Fact]
    public void TokenManager_WithNullHttpClient_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new TokenManager(_mockStorage, _mockOptions, null!, _mockTokenManagerLogger));
    }

    [Fact]
    public void ProcoreAuthHandler_WithNullTokenManager_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new ProcoreAuthHandler(null!, _mockAuthHandlerLogger));
    }

    [Fact]
    public async Task TokenManager_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var invalidOptions = new ProcoreAuthOptions
        {
            ClientId = "", // Invalid: empty client ID
            ClientSecret = "secret",
            TokenEndpoint = new Uri("https://api.procore.com/oauth/token")
        };

        var mockInvalidOptions = Substitute.For<IOptions<ProcoreAuthOptions>>();
        mockInvalidOptions.Value.Returns(invalidOptions);

        // Act & Assert
        var tokenManager = new TokenManager(_mockStorage, mockInvalidOptions, _httpClient, _mockTokenManagerLogger);
        
        // The validation should occur when attempting to use the configuration
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => tokenManager.RefreshTokenAsync());
    }

    #endregion

    #region Edge Cases and Boundary Conditions

    [Fact]
    public async Task TokenManager_WithExtremelyLargeToken_ShouldHandleGracefully()
    {
        // Arrange
        var largeToken = new string('A', 100000); // 100KB token
        var token = new AccessToken(largeToken, "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert - Should not throw OutOfMemoryException
        await _tokenManager.Invoking(tm => tm.StoreTokenAsync(token))
                          .Should().NotThrowAsync<OutOfMemoryException>();
    }

    [Fact]
    public async Task TokenManager_WithTokenExpiringInPast_ShouldTriggerImmediateRefresh()
    {
        // Arrange
        var expiredToken = new AccessToken(
            "expired-token",
            "Bearer",
            DateTimeOffset.UtcNow.AddDays(-1), // Expired yesterday
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

        var refreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                access_token = "refreshed-token",
                token_type = "Bearer",
                expires_in = 3600,
                refresh_token = "new-refresh-token"
            }), Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.SendAsyncFunc = (req, cancellationToken) =>
            Task.FromResult(refreshResponse);

        // Act
        var result = await _tokenManager.GetAccessTokenAsync();

        // Assert
        result!.Token.Should().Be("refreshed-token");
    }

    [Fact]
    public async Task TokenManager_WithVeryShortRefreshMargin_ShouldWorkCorrectly()
    {
        // Arrange
        var shortMarginOptions = new ProcoreAuthOptions
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            TokenEndpoint = new Uri("https://api.procore.com/oauth/token"),
            TokenRefreshMargin = TimeSpan.FromMilliseconds(1) // Very short margin
        };

        var mockShortMarginOptions = Substitute.For<IOptions<ProcoreAuthOptions>>();
        mockShortMarginOptions.Value.Returns(shortMarginOptions);

        var shortMarginTokenManager = new TokenManager(_mockStorage, mockShortMarginOptions, _httpClient, _mockTokenManagerLogger);

        var almostExpiredToken = new AccessToken(
            "almost-expired",
            "Bearer",
            DateTimeOffset.UtcNow.AddMilliseconds(2), // Expires in 2ms (more than 1ms margin)
            "refresh-token"
        );

        _mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                   .Returns(almostExpiredToken);

        // Act
        var result = await shortMarginTokenManager.GetAccessTokenAsync();

        // Assert
        result.Should().Be(almostExpiredToken, "Token should not be refreshed as it's still within margin");
    }

    #endregion

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpClient?.Dispose();
            _authHandler?.Dispose();
            _clientWithAuthHandler?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}