using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Procore.SDK.Core.Models;
using Procore.SDK.Extensions;
using System.Net.Http;

namespace Procore.SDK.Tests.Extensions;

/// <summary>
/// Comprehensive tests for ServiceCollectionExtensions AddProcoreSDK method
/// </summary>
public class ServiceCollectionExtensionsTests : IDisposable
{
    private readonly ServiceCollection _services;
    private readonly ConfigurationBuilder _configBuilder;
    private ServiceProvider? _serviceProvider;

    public ServiceCollectionExtensionsTests()
    {
        _services = new ServiceCollection();
        _configBuilder = new ConfigurationBuilder();
        
        // Add logging for tests
        _services.AddLogging();
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void AddProcoreSDK_WithConfiguration_ShouldRegisterAllRequiredServices()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert - Verify all core services are registered
        AssertServiceIsRegistered<ITokenStorage>();
        AssertServiceIsRegistered<ITokenManager>();
        AssertServiceIsRegistered<OAuthFlowHelper>();
        AssertServiceIsRegistered<ProcoreAuthHandler>();
        AssertServiceIsRegistered<IHttpClientFactory>();
        AssertServiceIsRegistered<IRequestAdapter>();
        AssertServiceIsRegistered<ICoreClient>();
        AssertServiceIsRegistered<IOptions<ProcoreAuthOptions>>();
        AssertServiceIsRegistered<IOptions<HttpClientOptions>>();
    }

    [Fact]
    public void AddProcoreSDK_WithSimpleConfiguration_ShouldRegisterAllRequiredServices()
    {
        // Arrange
        const string clientId = "test-client-id";
        const string clientSecret = "test-client-secret";
        const string redirectUri = "https://localhost:5001/callback";
        var scopes = new[] { "read", "write" };

        // Act
        _services.AddProcoreSDK(clientId, clientSecret, redirectUri, scopes);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert - Verify all core services are registered
        AssertServiceIsRegistered<ITokenStorage>();
        AssertServiceIsRegistered<ITokenManager>();
        AssertServiceIsRegistered<OAuthFlowHelper>();
        AssertServiceIsRegistered<ProcoreAuthHandler>();
        AssertServiceIsRegistered<IHttpClientFactory>();
        AssertServiceIsRegistered<IRequestAdapter>();
        AssertServiceIsRegistered<ICoreClient>();
        
        // Verify auth options are configured correctly
        var authOptions = _serviceProvider!.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.ClientId.Should().Be(clientId);
        authOptions.ClientSecret.Should().Be(clientSecret);
        authOptions.RedirectUri.Should().Be(redirectUri);
        authOptions.Scopes.Should().BeEquivalentTo(scopes);
    }

    [Fact]
    public void AddProcoreSDK_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        ServiceCollection? services = null;
        var configuration = CreateTestConfiguration();

        // Act & Assert
        var act = () => services!.AddProcoreSDK(configuration);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("services");
    }

    [Fact]
    public void AddProcoreSDK_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        // Arrange
        IConfiguration? configuration = null;

        // Act & Assert
        var act = () => _services.AddProcoreSDK(configuration!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("configuration");
    }

    [Fact]
    public void AddProcoreSDK_WithNullClientId_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => _services.AddProcoreSDK(null!, "secret", "uri");
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("clientId");
    }

    [Fact]
    public void AddProcoreSDK_WithEmptyClientId_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => _services.AddProcoreSDK("", "secret", "uri");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("clientId");
    }

    [Fact]
    public void AddProcoreSDK_WithNullClientSecret_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => _services.AddProcoreSDK("client", null!, "uri");
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("clientSecret");
    }

    [Fact]
    public void AddProcoreSDK_WithEmptyClientSecret_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => _services.AddProcoreSDK("client", "", "uri");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("clientSecret");
    }

    [Fact]
    public void AddProcoreSDK_WithNullRedirectUri_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => _services.AddProcoreSDK("client", "secret", null!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("redirectUri");
    }

    [Fact]
    public void AddProcoreSDK_WithEmptyRedirectUri_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = () => _services.AddProcoreSDK("client", "secret", "");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("redirectUri");
    }

    [Fact]
    public void AddProcoreSDK_WithCustomAuthConfiguration_ShouldApplyConfiguration()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var customMargin = TimeSpan.FromMinutes(10);

        // Act
        _services.AddProcoreSDK(configuration, auth =>
        {
            auth.TokenRefreshMargin = customMargin;
            auth.UsePkce = false;
        });
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.TokenRefreshMargin.Should().Be(customMargin);
        authOptions.UsePkce.Should().BeFalse();
    }

    [Fact]
    public void AddProcoreSDK_WithCustomHttpConfiguration_ShouldApplyConfiguration()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var customTimeout = TimeSpan.FromMinutes(5);
        var customMaxConnections = 20;

        // Act
        _services.AddProcoreSDK(configuration, null, http =>
        {
            http.Timeout = customTimeout;
            http.MaxConnectionsPerServer = customMaxConnections;
        });
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var httpOptions = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        httpOptions.Timeout.Should().Be(customTimeout);
        httpOptions.MaxConnectionsPerServer.Should().Be(customMaxConnections);
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterServicesAsSingleton_WhereAppropriate()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);

        // Assert - Check service lifetimes
        var tokenStorageDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ITokenStorage));
        tokenStorageDescriptor?.Lifetime.Should().Be(ServiceLifetime.Singleton);

        var tokenManagerDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ITokenManager));
        tokenManagerDescriptor?.Lifetime.Should().Be(ServiceLifetime.Singleton);

        var authHandlerDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ProcoreAuthHandler));
        authHandlerDescriptor?.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterServicesAsScoped_WhereAppropriate()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);

        // Assert - Check service lifetimes
        var coreClientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICoreClient));
        coreClientDescriptor?.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddProcoreSDK_CalledMultipleTimes_ShouldNotRegisterDuplicateServices()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _services.AddProcoreSDK(configuration);

        // Assert - Should only have one registration of each service type
        var tokenStorageDescriptors = _services.Where(s => s.ServiceType == typeof(ITokenStorage));
        tokenStorageDescriptors.Should().HaveCount(1);

        var tokenManagerDescriptors = _services.Where(s => s.ServiceType == typeof(ITokenManager));
        tokenManagerDescriptors.Should().HaveCount(1);

        var coreClientDescriptors = _services.Where(s => s.ServiceType == typeof(ICoreClient));
        coreClientDescriptors.Should().HaveCount(1);
    }

    [Fact]
    public void AddProcoreSDK_ShouldConfigureDefaultHttpClientOptions()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var httpOptions = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        httpOptions.BaseAddress.Should().Be(new Uri("https://api.procore.com"));
        httpOptions.Timeout.Should().Be(TimeSpan.FromMinutes(1));
        httpOptions.MaxConnectionsPerServer.Should().Be(10);
        httpOptions.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(15));
        httpOptions.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(2));
    }

    [Fact]
    public void AddProcoreSDK_ShouldConfigureDefaultAuthOptions()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.AuthorizationEndpoint.Should().Be(new Uri("https://app.procore.com/oauth/authorize"));
        authOptions.TokenEndpoint.Should().Be(new Uri("https://api.procore.com/oauth/token"));
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(5));
        authOptions.UsePkce.Should().BeTrue();
    }

    [Fact]
    public void AddProcoreSDK_ShouldAllowServiceOverride()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var customTokenStorage = Substitute.For<ITokenStorage>();

        // Act
        _services.AddSingleton(customTokenStorage);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert - Should use the pre-registered service
        var resolvedTokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
        resolvedTokenStorage.Should().BeSameAs(customTokenStorage);
    }

    [Fact]
    public void AddProcoreSDK_ShouldReturnServiceCollection_ForChaining()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        var result = _services.AddProcoreSDK(configuration);

        // Assert
        result.Should().BeSameAs(_services);
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
            {"ProcoreAuth:UsePkce", "true"},
            {"ProcoreApi:BaseUrl", "https://api.procore.com"},
            {"ProcoreApi:Timeout", "00:01:00"},
            {"ProcoreApi:MaxConnectionsPerServer", "10"}
        };

        return _configBuilder
            .AddInMemoryCollection(configData)
            .Build();
    }

    private void AssertServiceIsRegistered<TService>()
    {
        var service = _serviceProvider!.GetService<TService>();
        service.Should().NotBeNull($"{typeof(TService).Name} should be registered");
    }
}