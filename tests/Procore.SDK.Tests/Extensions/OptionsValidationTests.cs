using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Procore.SDK.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Procore.SDK.Tests.Extensions;

/// <summary>
/// Tests for options pattern configuration validation and error scenarios
/// </summary>
public class OptionsValidationTests : IDisposable
{
    private readonly ServiceCollection _services;
    private readonly ConfigurationBuilder _configBuilder;
    private ServiceProvider? _serviceProvider;

    public OptionsValidationTests()
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
    public void ProcoreAuthOptions_ShouldBindCorrectly_FromConfiguration()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:Scopes:0", "read"},
            {"ProcoreAuth:Scopes:1", "write"},
            {"ProcoreAuth:TokenRefreshMargin", "00:05:00"},
            {"ProcoreAuth:UsePkce", "true"},
            {"ProcoreAuth:AuthorizationEndpoint", "https://app.procore.com/oauth/authorize"},
            {"ProcoreAuth:TokenEndpoint", "https://api.procore.com/oauth/token"}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        options.ClientId.Should().Be("test-client-id");
        options.ClientSecret.Should().Be("test-client-secret");
        options.RedirectUri.Should().Be("https://localhost:5001/callback");
        options.Scopes.Should().BeEquivalentTo(new[] { "read", "write" });
        options.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(5));
        options.UsePkce.Should().BeTrue();
        options.AuthorizationEndpoint.Should().Be(new Uri("https://app.procore.com/oauth/authorize"));
        options.TokenEndpoint.Should().Be(new Uri("https://api.procore.com/oauth/token"));
    }

    [Fact]
    public void HttpClientOptions_ShouldBindCorrectly_FromConfiguration()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:BaseAddress", "https://custom-api.procore.com"},
            {"ProcoreApi:Timeout", "00:02:30"},
            {"ProcoreApi:MaxConnectionsPerServer", "25"},
            {"ProcoreApi:PooledConnectionLifetime", "00:20:00"},
            {"ProcoreApi:PooledConnectionIdleTimeout", "00:03:00"}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        options.BaseAddress.Should().Be(new Uri("https://custom-api.procore.com"));
        options.Timeout.Should().Be(TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(30)));
        options.MaxConnectionsPerServer.Should().Be(25);
        options.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(20));
        options.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(3));
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldHandleMissingConfiguration_WithDefaults()
    {
        // Arrange
        var configData = new Dictionary<string, string?>(); // Empty configuration

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        options.ClientId.Should().BeEmpty();
        options.ClientSecret.Should().BeEmpty();
        options.RedirectUri.Should().BeEmpty();
        options.Scopes.Should().BeEmpty();
        options.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(5));
        options.UsePkce.Should().BeTrue();
        options.AuthorizationEndpoint.Should().Be(new Uri("https://app.procore.com/oauth/authorize"));
        options.TokenEndpoint.Should().Be(new Uri("https://api.procore.com/oauth/token"));
    }

    [Fact]
    public void HttpClientOptions_ShouldHandleMissingConfiguration_WithDefaults()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"}
            // No ProcoreApi section
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        options.BaseAddress.Should().Be(new Uri("https://api.procore.com"));
        options.Timeout.Should().Be(TimeSpan.FromMinutes(1));
        options.MaxConnectionsPerServer.Should().Be(10);
        options.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(15));
        options.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(2));
    }

    [Theory]
    [InlineData("invalid-uri")]
    [InlineData("not-a-url")]
    [InlineData("")]
    public void ProcoreAuthOptions_ShouldHandleInvalidRedirectUri_Gracefully(string invalidUri)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", invalidUri}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        options.RedirectUri.Should().Be(invalidUri); // Configuration binding doesn't validate URIs
    }

    [Theory]
    [InlineData("invalid-uri")]
    [InlineData("not-a-url")]
    [InlineData("")]
    public void HttpClientOptions_ShouldHandleInvalidBaseAddress_Gracefully(string invalidBaseAddress)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:BaseAddress", invalidBaseAddress}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act & Assert
        _services.AddProcoreSDK(configuration);
        
        if (string.IsNullOrEmpty(invalidBaseAddress))
        {
            // Should use default
            _serviceProvider = _services.BuildServiceProvider();
            var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
            options.BaseAddress.Should().Be(new Uri("https://api.procore.com"));
        }
        else
        {
            // Should throw when building service provider and resolving options
            _serviceProvider = _services.BuildServiceProvider();
            var act = () => _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
            act.Should().Throw<UriFormatException>();
        }
    }

    [Theory]
    [InlineData("invalid-timespan")]
    [InlineData("not-a-timespan")]
    [InlineData("25:00:00")] // Invalid hour
    public void ProcoreAuthOptions_ShouldHandleInvalidTimeSpan_ForTokenRefreshMargin(string invalidTimeSpan)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:TokenRefreshMargin", invalidTimeSpan}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        // Invalid timespan should fallback to default
        options.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(5));
    }

    [Theory]
    [InlineData("invalid-timespan")]
    [InlineData("not-a-timespan")]
    [InlineData("25:00:00")] // Invalid hour
    public void HttpClientOptions_ShouldHandleInvalidTimeSpan_ForTimeout(string invalidTimeSpan)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:Timeout", invalidTimeSpan}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        // Invalid timespan should fallback to default
        options.Timeout.Should().Be(TimeSpan.FromMinutes(1));
    }

    [Theory]
    [InlineData("not-a-number")]
    [InlineData("invalid")]
    [InlineData("-5")]
    public void HttpClientOptions_ShouldHandleInvalidNumbers_ForConnectionCount(string invalidNumber)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreApi:MaxConnectionsPerServer", invalidNumber}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        // Invalid number should fallback to default
        options.MaxConnectionsPerServer.Should().Be(10);
    }

    [Theory]
    [InlineData("not-a-boolean")]
    [InlineData("invalid")]
    [InlineData("yes")]
    [InlineData("no")]
    public void ProcoreAuthOptions_ShouldHandleInvalidBoolean_ForUsePkce(string invalidBoolean)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:UsePkce", invalidBoolean}
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        // Invalid boolean should fallback to default
        options.UsePkce.Should().BeTrue();
    }

    [Fact]
    public void ProcoreAuthOptions_ShouldHandleEmptyScopes_Array()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            {"ProcoreAuth:ClientId", "test-client-id"},
            {"ProcoreAuth:ClientSecret", "test-client-secret"},
            {"ProcoreAuth:RedirectUri", "https://localhost:5001/callback"},
            {"ProcoreAuth:Scopes", ""} // Empty string instead of array
        };

        var configuration = _configBuilder
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var options = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        options.Scopes.Should().BeEmpty();
    }

    [Fact]
    public void Options_ShouldSupportPostConfiguration()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        
        _services.PostConfigure<ProcoreAuthOptions>(auth =>
        {
            auth.TokenRefreshMargin = TimeSpan.FromMinutes(10);
        });

        _services.PostConfigure<HttpClientOptions>(http =>
        {
            http.MaxConnectionsPerServer = 50;
        });

        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authOptions = _serviceProvider.GetRequiredService<IOptions<ProcoreAuthOptions>>().Value;
        authOptions.TokenRefreshMargin.Should().Be(TimeSpan.FromMinutes(10));

        var httpOptions = _serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        httpOptions.MaxConnectionsPerServer.Should().Be(50);
    }

    [Fact]
    public void Options_ShouldSupportOptionsMonitor()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authMonitor = _serviceProvider.GetRequiredService<IOptionsMonitor<ProcoreAuthOptions>>();
        var httpMonitor = _serviceProvider.GetRequiredService<IOptionsMonitor<HttpClientOptions>>();

        authMonitor.Should().NotBeNull();
        httpMonitor.Should().NotBeNull();

        var authOptions = authMonitor.CurrentValue;
        var httpOptions = httpMonitor.CurrentValue;

        authOptions.Should().NotBeNull();
        httpOptions.Should().NotBeNull();
    }

    [Fact]
    public void Options_ShouldSupportOptionsSnapshot()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = _serviceProvider.CreateScope();
        var authSnapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ProcoreAuthOptions>>();
        var httpSnapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<HttpClientOptions>>();

        authSnapshot.Should().NotBeNull();
        httpSnapshot.Should().NotBeNull();

        var authOptions = authSnapshot.Value;
        var httpOptions = httpSnapshot.Value;

        authOptions.Should().NotBeNull();
        httpOptions.Should().NotBeNull();
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
            {"ProcoreApi:BaseAddress", "https://api.procore.com"},
            {"ProcoreApi:Timeout", "00:01:00"},
            {"ProcoreApi:MaxConnectionsPerServer", "10"}
        };

        return _configBuilder
            .AddInMemoryCollection(configData)
            .Build();
    }
}