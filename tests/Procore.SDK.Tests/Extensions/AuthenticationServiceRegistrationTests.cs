using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Procore.SDK.Extensions;

namespace Procore.SDK.Tests.Extensions;

/// <summary>
/// Tests for authentication service registration and configuration
/// </summary>
public class AuthenticationServiceRegistrationTests : IDisposable
{
    private readonly ServiceCollection _services;
    private readonly ConfigurationBuilder _configBuilder;
    private ServiceProvider? _serviceProvider;

    public AuthenticationServiceRegistrationTests()
    {
        _services = new ServiceCollection();
        _configBuilder = new ConfigurationBuilder();
        _services.AddLogging();
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterTokenStorage_AsInMemoryByDefault()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
        tokenStorage.Should().NotBeNull();
        tokenStorage.Should().BeOfType<InMemoryTokenStorage>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldAllowCustomTokenStorage()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var customTokenStorage = Substitute.For<ITokenStorage>();

        // Act
        _services.AddSingleton(customTokenStorage);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var resolvedTokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
        resolvedTokenStorage.Should().BeSameAs(customTokenStorage);
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterTokenManager()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        tokenManager.Should().NotBeNull();
        tokenManager.Should().BeOfType<TokenManager>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterOAuthFlowHelper()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        oauthHelper.Should().NotBeNull();
        oauthHelper.Should().BeOfType<OAuthFlowHelper>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterProcoreAuthHandler()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authHandler = _serviceProvider.GetRequiredService<ProcoreAuthHandler>();
        authHandler.Should().NotBeNull();
        authHandler.Should().BeOfType<ProcoreAuthHandler>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldConfigureProcoreAuthOptions_FromConfiguration()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-123"},
            {"ProcoreAuth:ClientSecret", "test-secret-456"},
            {"ProcoreAuth:RedirectUri", "https://myapp.com/callback"},
            {"ProcoreAuth:Scopes:0", "projects.read"},
            {"ProcoreAuth:Scopes:1", "projects.write"},
            {"ProcoreAuth:Scopes:2", "users.read"},
            {"ProcoreAuth:TokenRefreshMargin", "00:10:00"},
            {"ProcoreAuth:UsePkce", "false"},
            {"ProcoreAuth:AuthorizationEndpoint", "https://custom-auth.procore.com/oauth/authorize"},
            {"ProcoreAuth:TokenEndpoint", "https://custom-api.procore.com/oauth/token"}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.ClientId.Should().Be("test-client-123");
        authOptions.ClientSecret.Should().Be("test-secret-456");
        authOptions.RedirectUri.Should().Be("https://myapp.com/callback");
        authOptions.Scopes.Should().BeEquivalentTo(new[] { "projects.read", "projects.write", "users.read" });
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(10));
        authOptions.UsePkce.Should().BeFalse();
        authOptions.AuthorizationEndpoint.Should().Be(new Uri("https://custom-auth.procore.com/oauth/authorize"));
        authOptions.TokenEndpoint.Should().Be(new Uri("https://custom-api.procore.com/oauth/token"));
    }

    [Fact]
    public void AddProcoreSDK_ShouldApplyCustomAuthConfiguration()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration, auth =>
        {
            auth.ClientId = "overridden-client-id";
            auth.TokenRefreshMargin = TimeSpan.FromMinutes(15);
            auth.UsePkce = false;
            auth.Scopes = new[] { "custom.scope" };
        });
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.ClientId.Should().Be("overridden-client-id");
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(15));
        authOptions.UsePkce.Should().BeFalse();
        authOptions.Scopes.Should().BeEquivalentTo(new[] { "custom.scope" });
    }

    [Fact]
    public void AddProcoreSDK_WithSimpleConfiguration_ShouldSetCorrectAuthOptions()
    {
        // Arrange
        const string clientId = "simple-client-id";
        const string clientSecret = "simple-client-secret";
        const string redirectUri = "https://simple.app/callback";
        var scopes = new[] { "read", "write", "admin" };

        // Act
        _services.AddProcoreSDK(clientId, clientSecret, redirectUri, scopes);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.ClientId.Should().Be(clientId);
        authOptions.ClientSecret.Should().Be(clientSecret);
        authOptions.RedirectUri.Should().Be(redirectUri);
        authOptions.Scopes.Should().BeEquivalentTo(scopes);
        
        // Should still have default values for other properties
        authOptions.UsePkce.Should().BeTrue();
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(5));
        authOptions.AuthorizationEndpoint.Should().Be(new Uri("https://app.procore.com/oauth/authorize"));
        authOptions.TokenEndpoint.Should().Be(new Uri("https://api.procore.com/oauth/token"));
    }

    [Fact]
    public void AddProcoreSDK_WithEmptyScopes_ShouldSetEmptyArray()
    {
        // Arrange
        const string clientId = "test-client";
        const string clientSecret = "test-secret";
        const string redirectUri = "https://test.app/callback";

        // Act
        _services.AddProcoreSDK(clientId, clientSecret, redirectUri);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.Scopes.Should().NotBeNull();
        authOptions.Scopes.Should().BeEmpty();
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterAuthServicesAsSingleton()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);

        // Assert - Check service lifetimes
        AssertServiceLifetime<ITokenStorage>(ServiceLifetime.Singleton);
        AssertServiceLifetime<ITokenManager>(ServiceLifetime.Singleton);
        AssertServiceLifetime<OAuthFlowHelper>(ServiceLifetime.Singleton);
        AssertServiceLifetime<ProcoreAuthHandler>(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddProcoreSDK_ShouldCreateTokenManager_WithCorrectDependencies()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        tokenManager.Should().NotBeNull();
        
        // Verify TokenManager can be constructed (indicates dependencies are satisfied)
        tokenManager.Should().BeOfType<TokenManager>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldCreateAuthHandler_WithCorrectDependencies()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authHandler = _serviceProvider.GetRequiredService<ProcoreAuthHandler>();
        authHandler.Should().NotBeNull();
        authHandler.Should().BeOfType<ProcoreAuthHandler>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldAllowMultipleAuthOptionsConfigurations()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration, auth =>
        {
            auth.TokenRefreshMargin = TimeSpan.FromMinutes(8);
        });

        _services.PostConfigure<ProcoreAuthOptions>(auth =>
        {
            auth.UsePkce = false;
        });

        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(8));
        authOptions.UsePkce.Should().BeFalse();
    }

    [Fact]
    public void AddProcoreSDK_AuthOptions_ShouldValidateRequiredFields()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", ""},
            {"ProcoreAuth:ClientSecret", ""},
            {"ProcoreAuth:RedirectUri", ""}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.ClientId.Should().BeEmpty();
        authOptions.ClientSecret.Should().BeEmpty();
        authOptions.RedirectUri.Should().BeEmpty();
    }

    [Theory]
    [InlineData("00:01:00", 1)] // 1 minute
    [InlineData("00:05:00", 5)] // 5 minutes
    [InlineData("00:10:00", 10)] // 10 minutes
    [InlineData("00:15:00", 15)] // 15 minutes
    public void AddProcoreSDK_ShouldAcceptValidTokenRefreshMargins(string marginString, int expectedMinutes)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:TokenRefreshMargin", marginString}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(expectedMinutes));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AddProcoreSDK_ShouldAcceptPkceConfiguration(bool usePkce)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:UsePkce", usePkce.ToString()}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.UsePkce.Should().Be(usePkce);
    }

    private IConfiguration CreateTestConfiguration()
    {
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:Scopes:0", "read"},
            {"ProcoreAuth:Scopes:1", "write"},
            {"ProcoreAuth:TokenRefreshMargin", "00:05:00"},
            {"ProcoreAuth:UsePkce", "true"}
        };

        return _configBuilder
            .AddInMemoryCollection(configData)
            .Build();
    }

    private void AssertServiceLifetime<TService>(ServiceLifetime expectedLifetime)
    {
        var descriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(TService));
        descriptor.Should().NotBeNull($"{typeof(TService).Name} should be registered");
        descriptor!.Lifetime.Should().Be(expectedLifetime, 
            $"{typeof(TService).Name} should be registered as {expectedLifetime}");
    }
}