using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Shared.Tests.Authentication;

/// <summary>
/// TDD Tests for ProcoreAuthOptions configuration class
/// These tests define the expected behavior for OAuth configuration options
/// </summary>
public class ProcoreAuthOptionsTests
{
    [Fact]
    public void ProcoreAuthOptions_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var options = new ProcoreAuthOptions();

        // Assert
        options.ClientId.Should().Be(string.Empty, "ClientId should default to empty string");
        options.ClientSecret.Should().Be(string.Empty, "ClientSecret should default to empty string");
        options.RedirectUri.Should().Be(string.Empty, "RedirectUri should default to empty string");
        options.Scopes.Should().BeEquivalentTo(Array.Empty<string>(), "Scopes should default to empty array");
        options.AuthorizationEndpoint.Should().Be(new Uri("https://app.procore.com/oauth/authorize"),
            "AuthorizationEndpoint should default to Procore authorization URL");
        options.TokenEndpoint.Should().Be(new Uri("https://api.procore.com/oauth/token"),
            "TokenEndpoint should default to Procore token URL");
        options.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(5),
            "TokenRefreshMargin should default to 5 minutes");
        options.UsePkce.Should().BeTrue("UsePkce should default to true");
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowClientIdConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var clientId = "test-client-id-12345";

        // Act
        options.ClientId = clientId;

        // Assert
        options.ClientId.Should().Be(clientId);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowClientSecretConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var clientSecret = "super-secret-client-secret";

        // Act
        options.ClientSecret = clientSecret;

        // Assert
        options.ClientSecret.Should().Be(clientSecret);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowRedirectUriConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var redirectUri = new Uri("https://myapp.com/oauth/callback");

        // Act
        options.RedirectUri = redirectUri;

        // Assert
        options.RedirectUri.Should().Be(redirectUri);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowScopesConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var scopes = new[] { "read", "write", "admin" };

        // Act
        options.Scopes = scopes;

        // Assert
        options.Scopes.Should().BeEquivalentTo(scopes);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowAuthorizationEndpointConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var customEndpoint = new Uri("https://custom-auth.example.com/oauth/authorize");

        // Act
        options.AuthorizationEndpoint = customEndpoint;

        // Assert
        options.AuthorizationEndpoint.Should().Be(customEndpoint);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowTokenEndpointConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var customEndpoint = new Uri("https://custom-auth.example.com/oauth/token");

        // Act
        options.TokenEndpoint = customEndpoint;

        // Assert
        options.TokenEndpoint.Should().Be(customEndpoint);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowTokenRefreshMarginConfiguration()
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var customMargin = TimeSpan.FromMinutes(10);

        // Act
        options.TokenRefreshMargin = customMargin;

        // Assert
        options.TokenRefreshMargin.Should().Be(customMargin);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ProcoreAuthOptions_ShouldAllowPkceConfiguration(bool usePkce)
    {
        // Arrange
        var options = new ProcoreAuthOptions();

        // Act
        options.UsePkce = usePkce;

        // Assert
        options.UsePkce.Should().Be(usePkce);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldHavePublicSetters()
    {
        // This test ensures all properties can be set (validation for dependency injection scenarios)
        // Arrange
        var options = new ProcoreAuthOptions();

        // Act & Assert (should not throw)
        options.ClientId = "test";
        options.ClientSecret = "secret";
        options.RedirectUri = new Uri("https://example.com");
        options.Scopes = new[] { "read" };
        options.AuthorizationEndpoint = new Uri("https://auth.example.com");
        options.TokenEndpoint = new Uri("https://token.example.com");
        options.TokenRefreshMargin = TimeSpan.FromMinutes(1);
        options.UsePkce = false;

        // Verify all properties were set
        options.ClientId.Should().Be("test");
        options.ClientSecret.Should().Be("secret");
        options.RedirectUri.Should().Be("https://example.com");
        options.Scopes.Should().BeEquivalentTo(new[] { "read" });
        options.AuthorizationEndpoint.Should().Be(new Uri("https://auth.example.com"));
        options.TokenEndpoint.Should().Be(new Uri("https://token.example.com"));
        options.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(1));
        options.UsePkce.Should().BeFalse();
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowEmptyScopes()
    {
        // Arrange
        var options = new ProcoreAuthOptions();

        // Act
        options.Scopes = Array.Empty<string>();

        // Assert
        options.Scopes.Should().BeEmpty();
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowNullScopes()
    {
        // Arrange
        var options = new ProcoreAuthOptions();

        // Act
        options.Scopes = null!;

        // Assert
        options.Scopes.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void ProcoreAuthOptions_ShouldAllowEmptyOrNullStrings(string? value)
    {
        // This validates that the configuration system can handle empty/null values
        // which is important for validation scenarios
        
        // Arrange
        var options = new ProcoreAuthOptions();

        // Act & Assert (should not throw)
        options.ClientId = value ?? string.Empty;
        options.ClientSecret = value ?? string.Empty;
        options.RedirectUri = string.IsNullOrEmpty(value) ? null : new Uri(value);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowZeroRefreshMargin()
    {
        // Arrange
        var options = new ProcoreAuthOptions();

        // Act
        options.TokenRefreshMargin = TimeSpan.Zero;

        // Assert
        options.TokenRefreshMargin.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldAllowNegativeRefreshMargin()
    {
        // This might be useful for testing scenarios where we want immediate refresh
        // Arrange
        var options = new ProcoreAuthOptions();
        var negativeMargin = TimeSpan.FromMinutes(-1);

        // Act
        options.TokenRefreshMargin = negativeMargin;

        // Assert
        options.TokenRefreshMargin.Should().Be(negativeMargin);
    }

    [Fact]
    public void ProcoreAuthOptions_Properties_ShouldBeReadWritable()
    {
        // This test ensures all properties have both getters and setters
        // Important for serialization and configuration binding
        
        // Arrange
        var type = typeof(ProcoreAuthOptions);

        // Act & Assert
        var properties = type.GetProperties();
        
        foreach (var property in properties)
        {
            property.CanRead.Should().BeTrue($"{property.Name} should be readable");
            property.CanWrite.Should().BeTrue($"{property.Name} should be writable");
        }
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldHaveParameterlessConstructor()
    {
        // This is required for configuration binding and dependency injection
        // Arrange & Act
        var constructors = typeof(ProcoreAuthOptions).GetConstructors();
        var parameterlessConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);

        // Assert
        parameterlessConstructor.Should().NotBeNull(
            "ProcoreAuthOptions should have a parameterless constructor for configuration binding");
        
        // Verify it can be instantiated
        var instance = Activator.CreateInstance<ProcoreAuthOptions>();
        instance.Should().NotBeNull();
    }

    [Fact]
    public void ProcoreAuthOptions_DefaultEndpoints_ShouldUseHttps()
    {
        // Arrange & Act
        var options = new ProcoreAuthOptions();

        // Assert
        options.AuthorizationEndpoint.Scheme.Should().Be("https",
            "Authorization endpoint should use HTTPS for security");
        options.TokenEndpoint.Scheme.Should().Be("https",
            "Token endpoint should use HTTPS for security");
    }

    [Fact]
    public void ProcoreAuthOptions_DefaultEndpoints_ShouldPointToProcoreDomains()
    {
        // Arrange & Act
        var options = new ProcoreAuthOptions();

        // Assert
        options.AuthorizationEndpoint.Host.Should().Be("app.procore.com",
            "Authorization endpoint should point to Procore's app domain");
        options.TokenEndpoint.Host.Should().Be("api.procore.com",
            "Token endpoint should point to Procore's API domain");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(60)]
    public void ProcoreAuthOptions_ShouldAllowVariousRefreshMargins(int minutes)
    {
        // Arrange
        var options = new ProcoreAuthOptions();
        var margin = TimeSpan.FromMinutes(minutes);

        // Act
        options.TokenRefreshMargin = margin;

        // Assert
        options.TokenRefreshMargin.Should().Be(margin);
    }
}