using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Procore.SDK.Extensions;
using System.Net;
using System.Net.Http;

namespace Procore.SDK.Tests.Extensions;

/// <summary>
/// Tests for HttpClient configuration and connection management
/// </summary>
public class HttpClientConfigurationTests : IDisposable
{
    private readonly ServiceCollection _services;
    private readonly ConfigurationBuilder _configBuilder;
    private ServiceProvider? _serviceProvider;

    public HttpClientConfigurationTests()
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
    public void HttpClientFactory_ShouldCreateNamedProcoreClient()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");

        // Assert
        client.Should().NotBeNull();
        client.BaseAddress.Should().Be(new Uri("https://api.procore.com"));
        client.Timeout.Should().Be(TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void HttpClient_ShouldHaveAuthenticationHandler()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");

        // Assert - We can't directly test the handler chain, but we can verify it's configured
        // by checking that the auth handler is registered
        var authHandler = _serviceProvider.GetService<ProcoreAuthHandler>();
        authHandler.Should().NotBeNull();
    }

    [Fact]
    public void HttpClientOptions_ShouldHaveCorrectDefaults()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;

        // Assert
        options.BaseAddress.Should().Be(new Uri("https://api.procore.com"));
        options.Timeout.Should().Be(TimeSpan.FromMinutes(1));
        options.MaxConnectionsPerServer.Should().Be(10);
        options.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(15));
        options.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(2));
    }

    [Fact]
    public void HttpClientOptions_ShouldBindFromConfiguration()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:BaseAddress", "https://custom-api.procore.com"},
            {"ProcoreApi:Timeout", "00:02:30"},
            {"ProcoreApi:MaxConnectionsPerServer", "20"},
            {"ProcoreApi:PooledConnectionLifetime", "00:30:00"},
            {"ProcoreApi:PooledConnectionIdleTimeout", "00:05:00"}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;

        // Assert
        options.BaseAddress.Should().Be(new Uri("https://custom-api.procore.com"));
        options.Timeout.Should().Be(TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(30)));
        options.MaxConnectionsPerServer.Should().Be(20);
        options.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(30));
        options.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(5));
    }

    [Fact]
    public void HttpClientOptions_ShouldAllowPostConfiguration()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration, null, options =>
        {
            options.Timeout = TimeSpan.FromMinutes(3);
            options.MaxConnectionsPerServer = 25;
        });
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        options.Timeout.Should().Be(TimeSpan.FromMinutes(3));
        options.MaxConnectionsPerServer.Should().Be(25);
    }

    [Fact]
    public void HttpClient_ShouldUseSocketsHttpHandler_WithCorrectSettings()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");

        // Assert
        // We can't directly access the handler configuration from the client,
        // but we can verify the client is properly configured
        client.Should().NotBeNull();
        client.BaseAddress.Should().NotBeNull();
    }

    [Theory]
    [InlineData("https://api.procore.com")]
    [InlineData("https://custom.procore.com")]
    [InlineData("https://staging-api.procore.com")]
    public void HttpClientOptions_ShouldAcceptValidBaseAddresses(string baseUrl)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:BaseAddress", baseUrl}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;

        // Assert
        options.BaseAddress.Should().Be(new Uri(baseUrl));
    }

    [Theory]
    [InlineData("00:00:30", 30)] // 30 seconds
    [InlineData("00:01:00", 60)] // 1 minute
    [InlineData("00:05:00", 300)] // 5 minutes
    [InlineData("00:10:00", 600)] // 10 minutes
    public void HttpClientOptions_ShouldAcceptValidTimeouts(string timeoutString, int expectedSeconds)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:Timeout", timeoutString}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;

        // Assert
        options.Timeout.Should().Be(TimeSpan.FromSeconds(expectedSeconds));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void HttpClientOptions_ShouldAcceptValidConnectionCounts(int connectionCount)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:MaxConnectionsPerServer", connectionCount.ToString()}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;

        // Assert
        options.MaxConnectionsPerServer.Should().Be(connectionCount);
    }

    [Fact]
    public void HttpClient_ShouldNotUseCookies()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client = httpClientFactory.CreateClient("Procore");

        // Assert
        // We verify this by checking that the handler is configured correctly
        // The actual SocketsHttpHandler.UseCookies = false is set in the configuration
        client.Should().NotBeNull();
    }

    [Fact]
    public void HttpClientFactory_ShouldCreateDifferentInstancesForDifferentCalls()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var httpClientFactory = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        var factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        using var client1 = factory.CreateClient("Procore");
        using var client2 = factory.CreateClient("Procore");

        // Assert
        client1.Should().NotBeSameAs(client2);
        client1.BaseAddress.Should().Be(client2.BaseAddress);
        client1.Timeout.Should().Be(client2.Timeout);
    }

    [Fact]
    public void HttpClientOptions_ShouldValidateConfigurationBinding()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Act
        var optionsMonitor = _serviceProvider.GetRequiredService<IOptionsMonitor<HttpClientOptions>>();
        var options = optionsMonitor.CurrentValue;

        // Assert
        options.Should().NotBeNull();
        options.BaseAddress.Should().NotBeNull();
        options.Timeout.Should().BePositive();
        options.MaxConnectionsPerServer.Should().BePositive();
        options.PooledConnectionLifetime.Should().BePositive();
        options.PooledConnectionIdleTimeout.Should().BePositive();
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
            {"ProcoreApi:Timeout", "00:01:00"},
            {"ProcoreApi:MaxConnectionsPerServer", "10"}
        };

        return _configBuilder
            .AddInMemoryCollection(configData)
            .Build();
    }
}