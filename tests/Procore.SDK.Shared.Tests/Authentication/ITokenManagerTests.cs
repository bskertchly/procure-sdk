using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Shared.Tests.Authentication;

/// <summary>
/// TDD Tests for ITokenManager interface specification
/// These tests define the expected behavior before implementation
/// </summary>
public class ITokenManagerTests
{
    [Fact]
    public void ITokenManager_ShouldDefineGetAccessTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenManager).GetMethods();
        var getTokenMethod = methods.FirstOrDefault(m => m.Name == "GetAccessTokenAsync");

        // Assert
        getTokenMethod.Should().NotBeNull("ITokenManager must define GetAccessTokenAsync method");
        getTokenMethod!.ReturnType.Should().Be(typeof(Task<AccessToken?>), 
            "GetAccessTokenAsync should return Task<AccessToken?>");
        
        var parameters = getTokenMethod.GetParameters();
        parameters.Should().HaveCount(1, "GetAccessTokenAsync should have one parameter");
        parameters[0].Name.Should().Be("cancellationToken");
        parameters[0].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[0].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }

    [Fact]
    public void ITokenManager_ShouldDefineRefreshTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenManager).GetMethods();
        var refreshMethod = methods.FirstOrDefault(m => m.Name == "RefreshTokenAsync");

        // Assert
        refreshMethod.Should().NotBeNull("ITokenManager must define RefreshTokenAsync method");
        refreshMethod!.ReturnType.Should().Be(typeof(Task<AccessToken>),
            "RefreshTokenAsync should return Task<AccessToken>");
            
        var parameters = refreshMethod.GetParameters();
        parameters.Should().HaveCount(1, "RefreshTokenAsync should have one parameter");
        parameters[0].Name.Should().Be("cancellationToken");
        parameters[0].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[0].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }

    [Fact]
    public void ITokenManager_ShouldDefineStoreTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenManager).GetMethods();
        var storeMethod = methods.FirstOrDefault(m => m.Name == "StoreTokenAsync");

        // Assert
        storeMethod.Should().NotBeNull("ITokenManager must define StoreTokenAsync method");
        storeMethod!.ReturnType.Should().Be(typeof(Task),
            "StoreTokenAsync should return Task");
            
        var parameters = storeMethod.GetParameters();
        parameters.Should().HaveCount(2, "StoreTokenAsync should have two parameters");
        parameters[0].Name.Should().Be("token");
        parameters[0].ParameterType.Should().Be(typeof(AccessToken));
        parameters[1].Name.Should().Be("cancellationToken");
        parameters[1].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[1].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }

    [Fact]
    public void ITokenManager_ShouldDefineClearTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenManager).GetMethods();
        var clearMethod = methods.FirstOrDefault(m => m.Name == "ClearTokenAsync");

        // Assert
        clearMethod.Should().NotBeNull("ITokenManager must define ClearTokenAsync method");
        clearMethod!.ReturnType.Should().Be(typeof(Task),
            "ClearTokenAsync should return Task");
            
        var parameters = clearMethod.GetParameters();
        parameters.Should().HaveCount(1, "ClearTokenAsync should have one parameter");
        parameters[0].Name.Should().Be("cancellationToken");
        parameters[0].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[0].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }

    [Fact]
    public void ITokenManager_ShouldDefineTokenRefreshedEvent()
    {
        // Arrange & Act
        var events = typeof(ITokenManager).GetEvents();
        var tokenRefreshedEvent = events.FirstOrDefault(e => e.Name == "TokenRefreshed");

        // Assert
        tokenRefreshedEvent.Should().NotBeNull("ITokenManager must define TokenRefreshed event");
        tokenRefreshedEvent!.EventHandlerType.Should().Be(typeof(EventHandler<TokenRefreshedEventArgs>),
            "TokenRefreshed event should be EventHandler<TokenRefreshedEventArgs>");
    }
}

/// <summary>
/// TDD Tests for AccessToken record specification
/// These tests define the expected behavior before implementation
/// </summary>
public class AccessTokenTests
{
    [Fact]
    public void AccessToken_ShouldBeRecord()
    {
        // Arrange & Act
        var type = typeof(AccessToken);

        // Assert
        type.Should().NotBeNull("AccessToken type should exist");
        // Records in .NET have compiler-generated methods that we can check for
        type.GetMethod("get_EqualityContract", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Should().NotBeNull("AccessToken should be a record type");
    }

    [Fact]
    public void AccessToken_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var properties = typeof(AccessToken).GetProperties();

        // Assert
        properties.Should().Contain(p => p.Name == "Token" && p.PropertyType == typeof(string),
            "AccessToken should have Token property of type string");
        properties.Should().Contain(p => p.Name == "TokenType" && p.PropertyType == typeof(string),
            "AccessToken should have TokenType property of type string");
        properties.Should().Contain(p => p.Name == "ExpiresAt" && p.PropertyType == typeof(DateTimeOffset),
            "AccessToken should have ExpiresAt property of type DateTimeOffset");
        properties.Should().Contain(p => p.Name == "RefreshToken" && p.PropertyType == typeof(string),
            "AccessToken should have RefreshToken property of type string?");
        properties.Should().Contain(p => p.Name == "Scopes" && p.PropertyType == typeof(string[]),
            "AccessToken should have Scopes property of type string[]?");
    }

    [Fact]
    public void AccessToken_ShouldHaveCorrectConstructor()
    {
        // This test will validate that AccessToken can be constructed with the expected parameters
        // Will fail until the record is implemented with proper constructor

        // Arrange
        var token = "test-token";
        var tokenType = "Bearer";
        var expiresAt = DateTimeOffset.UtcNow.AddHours(1);
        var refreshToken = "refresh-token";
        var scopes = new[] { "read", "write" };

        // Act & Assert
        var accessToken = new AccessToken(token, tokenType, expiresAt, refreshToken, scopes);
        
        accessToken.Token.Should().Be(token);
        accessToken.TokenType.Should().Be(tokenType);
        accessToken.ExpiresAt.Should().Be(expiresAt);
        accessToken.RefreshToken.Should().Be(refreshToken);
        accessToken.Scopes.Should().BeEquivalentTo(scopes);
    }

    [Fact]
    public void AccessToken_ShouldAllowOptionalParameters()
    {
        // Arrange
        var token = "test-token";
        var tokenType = "Bearer";
        var expiresAt = DateTimeOffset.UtcNow.AddHours(1);

        // Act & Assert
        var accessToken = new AccessToken(token, tokenType, expiresAt);
        
        accessToken.Token.Should().Be(token);
        accessToken.TokenType.Should().Be(tokenType);
        accessToken.ExpiresAt.Should().Be(expiresAt);
        accessToken.RefreshToken.Should().BeNull();
        accessToken.Scopes.Should().BeNull();
    }
}

/// <summary>
/// TDD Tests for TokenRefreshedEventArgs specification
/// These tests define the expected behavior before implementation
/// </summary>
public class TokenRefreshedEventArgsTests
{
    [Fact]
    public void TokenRefreshedEventArgs_ShouldInheritFromEventArgs()
    {
        // Arrange & Act
        var type = typeof(TokenRefreshedEventArgs);

        // Assert
        type.Should().NotBeNull("TokenRefreshedEventArgs type should exist");
        type.BaseType.Should().Be(typeof(EventArgs),
            "TokenRefreshedEventArgs should inherit from EventArgs");
    }

    [Fact]
    public void TokenRefreshedEventArgs_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var properties = typeof(TokenRefreshedEventArgs).GetProperties();

        // Assert
        properties.Should().Contain(p => p.Name == "NewToken" && p.PropertyType == typeof(AccessToken),
            "TokenRefreshedEventArgs should have NewToken property of type AccessToken");
        properties.Should().Contain(p => p.Name == "OldToken" && p.PropertyType == typeof(AccessToken),
            "TokenRefreshedEventArgs should have OldToken property of type AccessToken?");
    }

    [Fact]
    public void TokenRefreshedEventArgs_ShouldHaveCorrectConstructor()
    {
        // Arrange
        var newToken = new AccessToken("new-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        var oldToken = new AccessToken("old-token", "Bearer", DateTimeOffset.UtcNow.AddMinutes(-1));

        // Act & Assert
        var eventArgs = new TokenRefreshedEventArgs(newToken, oldToken);
        
        eventArgs.NewToken.Should().Be(newToken);
        eventArgs.OldToken.Should().Be(oldToken);
    }

    [Fact]
    public void TokenRefreshedEventArgs_ShouldAllowNullOldToken()
    {
        // Arrange
        var newToken = new AccessToken("new-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert
        var eventArgs = new TokenRefreshedEventArgs(newToken);
        
        eventArgs.NewToken.Should().Be(newToken);
        eventArgs.OldToken.Should().BeNull();
    }
}