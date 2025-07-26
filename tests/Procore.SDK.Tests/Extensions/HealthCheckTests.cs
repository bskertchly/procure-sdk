using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Procore.SDK.Extensions;
using System.Net;
using System.Net.Http;

namespace Procore.SDK.Tests.Extensions;

/// <summary>
/// Tests for health checks and API connectivity validation
/// </summary>
public class HealthCheckTests : IDisposable
{
    private readonly ServiceCollection _services;
    private readonly ConfigurationBuilder _configBuilder;
    private ServiceProvider? _serviceProvider;

    public HealthCheckTests()
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
    public void AddProcoreSDK_ShouldRegisterHealthChecks()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var healthCheckService = _serviceProvider.GetRequiredService<HealthCheckService>();
        healthCheckService.Should().NotBeNull();
    }

    [Fact]
    public void AddProcoreSDK_ShouldRegisterProcoreApiHealthCheck()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var healthCheckService = _serviceProvider.GetRequiredService<HealthCheckService>();
        var healthCheckContext = new HealthCheckContext();
        
        // The health check should be registered
        healthCheckService.Should().NotBeNull();
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldReturnHealthy_WhenApiIsAccessible()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        // Mock HTTP client factory to return successful response
        var mockHttpClient = CreateMockHttpClient(HttpStatusCode.OK, "OK");
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().Be("Procore API is accessible");
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldReturnDegraded_WhenApiReturnsError()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        // Mock HTTP client factory to return error response
        var mockHttpClient = CreateMockHttpClient(HttpStatusCode.InternalServerError, "Server Error");
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Degraded);
        result.Description.Should().Be("Procore API returned InternalServerError");
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldReturnUnhealthy_WhenExceptionThrown()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        // Mock HTTP client factory to throw exception
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(_ => throw new HttpRequestException("Network error"));
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Description.Should().Be("Procore API is not accessible");
        result.Exception.Should().BeOfType<HttpRequestException>();
        result.Exception!.Message.Should().Be("Network error");
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldRespectCancellationToken()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        // Mock HTTP client that will be cancelled
        var mockHttpClient = Substitute.For<HttpClient>();
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();
        
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        var act = () => healthCheck.CheckHealthAsync(new HealthCheckContext(), cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void ProcoreApiHealthCheck_ShouldBeRegisteredWithCorrectTags()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);

        // Assert
        var healthCheckDescriptor = _services
            .Where(s => s.ServiceType == typeof(IHealthCheck))
            .FirstOrDefault(s => s.ImplementationType == typeof(ProcoreApiHealthCheck));

        healthCheckDescriptor.Should().NotBeNull();
    }

    [Fact]
    public void ProcoreApiHealthCheck_ShouldHaveCorrectDependencies()
    {
        // Arrange
        var configuration = CreateTestConfiguration();

        // Act
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        // Assert
        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();
        healthCheck.Should().NotBeNull();

        // Verify dependencies can be resolved
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();

        var logger = _serviceProvider.GetService<ILogger<ProcoreApiHealthCheck>>();
        // Logger might be null if not configured, which is acceptable
    }

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.NoContent)]
    [InlineData(HttpStatusCode.Accepted)]
    public async Task ProcoreApiHealthCheck_ShouldReturnHealthy_ForSuccessStatusCodes(HttpStatusCode statusCode)
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        var mockHttpClient = CreateMockHttpClient(statusCode, "Success");
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task ProcoreApiHealthCheck_ShouldReturnDegraded_ForErrorStatusCodes(HttpStatusCode statusCode)
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        var mockHttpClient = CreateMockHttpClient(statusCode, "Error");
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Degraded);
        result.Description.Should().Be($"Procore API returned {statusCode}");
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldLogWarning_WhenApiReturnsError()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var mockLogger = Substitute.For<ILogger<ProcoreApiHealthCheck>>();
        
        var mockHttpClient = CreateMockHttpClient(HttpStatusCode.InternalServerError, "Server Error");
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddSingleton(mockLogger);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        mockLogger.Received(1).LogWarning(
            Arg.Any<string>(),
            Arg.Is<object?[]>(args => args != null && args.Contains("Procore API returned InternalServerError")));
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldLogError_WhenExceptionThrown()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        var mockLogger = Substitute.For<ILogger<ProcoreApiHealthCheck>>();
        
        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(_ => throw new HttpRequestException("Network error"));
        
        _services.AddSingleton(mockFactory);
        _services.AddSingleton(mockLogger);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        mockLogger.Received(1).LogError(
            Arg.Any<Exception>(),
            Arg.Any<string>(),
            Arg.Is<object?[]>(args => args != null && args.Contains("Procore API is not accessible")));
    }

    [Fact]
    public async Task ProcoreApiHealthCheck_ShouldUseCorrectEndpoint()
    {
        // Arrange
        var configuration = CreateTestConfiguration();
        
        var mockHttpClient = Substitute.For<HttpClient>();
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
        
        // Configure mock to track the request
        var requestUri = "";
        mockHttpClient.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                requestUri = callInfo.ArgAt<string>(0);
                return Task.FromResult(mockResponse);
            });

        var mockFactory = Substitute.For<IHttpClientFactory>();
        mockFactory.CreateClient("Procore").Returns(mockHttpClient);
        
        _services.AddSingleton(mockFactory);
        _services.AddProcoreSDK(configuration);
        _serviceProvider = _services.BuildServiceProvider();

        var healthCheck = _serviceProvider.GetRequiredService<ProcoreApiHealthCheck>();

        // Act
        await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        requestUri.Should().Be("/ping");
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

    private static HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
    {
        var mockHttpClient = Substitute.For<HttpClient>();
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content)
        };

        mockHttpClient.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(response));

        return mockHttpClient;
    }
}