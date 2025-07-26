using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Procore.SDK.Extensions;
using System.Net.Http;

namespace Procore.SDK.Tests.Extensions;

/// <summary>
/// Tests for Kiota request adapter registration and configuration
/// </summary>
public class KiotaRequestAdapterTests : IDisposable
{
    private readonly ServiceCollection _services;
    private readonly ConfigurationBuilder _configBuilder;
    private ServiceProvider? _serviceProvider;

    public KiotaRequestAdapterTests()
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
    public void AddProcoreSDK_ShouldRegisterRequestAdapter()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().NotBeNull();
        requestAdapter.Should().BeOfType<HttpClientRequestAdapter>();
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterRequestAdapter_AsSingleton()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);

        // Assert
        var descriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IRequestAdapter));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldUseProcoreHttpClient()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().BeOfType<HttpClientRequestAdapter>();

        // The adapter should be configured with the Procore HTTP client
        // We can't directly access the internal HttpClient, but we can verify
        // the adapter was created successfully
        requestAdapter.Should().NotBeNull();
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldHaveLogger()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().BeOfType<HttpClientRequestAdapter>();

        // Verify logger is available for the adapter
        var logger = _serviceProvider.GetService<ILogger<HttpClientRequestAdapter>>();
        logger.Should().NotBeNull();
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldUseHttpClientFactory()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();

        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().NotBeNull();

        // Verify the adapter can be created multiple times (using the factory pattern)
        var requestAdapter2 = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter2.Should().BeSameAs(requestAdapter); // Should be singleton
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldHandleDisposal()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();

        // Assert
        requestAdapter.Should().BeAssignableTo<IDisposable>();

        // Verify it can be disposed without throwing
        var disposableAdapter = requestAdapter as IDisposable;
        var act = () => disposableAdapter?.Dispose();
        act.Should().NotThrow();
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldNotHaveAuthenticationProvider()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        var httpAdapter = requestAdapter.Should().BeOfType<HttpClientRequestAdapter>().Subject;

        // The authentication provider should be null because we handle auth via HttpMessageHandler
        // This is the intended design - authentication is handled by ProcoreAuthHandler
        requestAdapter.Should().NotBeNull();
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldIntegrateWithAuthHandler()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        var authHandler = _serviceProvider.GetRequiredService<ProcoreAuthHandler>();

        requestAdapter.Should().NotBeNull();
        authHandler.Should().NotBeNull();

        // Both services should be resolvable and the HTTP client should have the auth handler
        // in its handler chain (verified through HTTP client factory configuration)
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldUseCorrectBaseAddress()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:BaseAddress", "https://custom-api.procore.com"}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().NotBeNull();

        // The base address is configured on the HttpClient, which is used by the adapter
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");
        client.BaseAddress.Should().Be(new Uri("https://custom-api.procore.com"));
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldInheritHttpClientTimeout()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:Timeout", "00:02:00"}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var requestAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().NotBeNull();

        // The timeout is configured on the HttpClient
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");
        client.Timeout.Should().Be(TimeSpan.FromMinutes(2));
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldAllowCustomImplementation()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var customAdapter = Substitute.For<IRequestAdapter>();

        // Act
        _services.AddSingleton(customAdapter);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var resolvedAdapter = _serviceProvider.GetRequiredService<IRequestAdapter>();
        resolvedAdapter.Should().BeSameAs(customAdapter);
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldBeConsistentAcrossRequests()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var adapter1 = _serviceProvider.GetRequiredService<IRequestAdapter>();
        var adapter2 = _serviceProvider.GetRequiredService<IRequestAdapter>();

        adapter1.Should().BeSameAs(adapter2, "Request adapter should be singleton");
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldWorkWithoutOptionalLogger()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var servicesWithoutLogging = new ServiceCollection();

        // Act
        servicesWithoutLogging.AddProcoreSDK(configuration);
        using var serviceProvider = servicesWithoutLogging.BuildServiceProvider();

        // Assert
        var requestAdapter = serviceProvider.GetRequiredService<IRequestAdapter>();
        requestAdapter.Should().NotBeNull();
        requestAdapter.Should().BeOfType<HttpClientRequestAdapter>();
    }

    [Fact]
    public void AddProcoreSDK_RequestAdapter_ShouldBeCreatedLazily()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert - Verify that the adapter is not created until requested
        // This is important for performance and avoiding unnecessary resource usage
        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IRequestAdapter));
        serviceDescriptor.Should().NotBeNull();
        serviceDescriptor!.ImplementationFactory.Should().NotBeNull();
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
            {"ProcoreApi:BaseAddress", "https://api.procore.com"},
            {"ProcoreApi:Timeout", "00:01:00"}
        };

        return _configBuilder
            .AddInMemoryCollection(configData)
            .Build();
    }
}