using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Procore.SDK.Shared.Authentication;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Extensions;

/// <summary>
/// Extensions for configuring Procore SDK services in the dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Procore SDK services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">Configuration to bind options from</param>
    /// <param name="configureAuth">Optional action to configure authentication options</param>
    /// <param name="configureHttp">Optional action to configure HTTP client options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddProcoreSDK(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ProcoreAuthOptions>? configureAuth = null,
        Action<HttpClientOptions>? configureHttp = null)
    {
        // Validate parameters
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // Configure authentication options
        var authSection = configuration.GetSection("ProcoreAuth");
        services.Configure<ProcoreAuthOptions>(authSection);
        
        if (configureAuth != null)
        {
            services.PostConfigure(configureAuth);
        }

        // Configure HTTP client options
        services.Configure<HttpClientOptions>(options =>
        {
            options.BaseAddress = new Uri("https://api.procore.com");
            options.Timeout = TimeSpan.FromMinutes(1);
            options.MaxConnectionsPerServer = 10;
            options.PooledConnectionLifetime = TimeSpan.FromMinutes(15);
            options.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);
        });

        var httpSection = configuration.GetSection("ProcoreApi");
        if (httpSection.Exists())
        {
            services.Configure<HttpClientOptions>(httpSection);
        }

        if (configureHttp != null)
        {
            services.PostConfigure(configureHttp);
        }

        // Register authentication services
        RegisterAuthenticationServices(services);

        // Register HTTP client services
        RegisterHttpClientServices(services);

        // Register Kiota services
        RegisterKiotaServices(services);

        // Register client services
        RegisterClientServices(services);

        // Register health checks
        RegisterHealthChecks(services);

        return services;
    }

    /// <summary>
    /// Adds Procore SDK services with simplified configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="clientId">OAuth client ID</param>
    /// <param name="clientSecret">OAuth client secret</param>
    /// <param name="redirectUri">OAuth redirect URI</param>
    /// <param name="scopes">OAuth scopes</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddProcoreSDK(
        this IServiceCollection services,
        string clientId,
        string clientSecret,
        string redirectUri,
        params string[] scopes)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientSecret);
        ArgumentException.ThrowIfNullOrWhiteSpace(redirectUri);

        // Configure authentication options directly
        services.Configure<ProcoreAuthOptions>(options =>
        {
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.RedirectUri = redirectUri;
            options.Scopes = scopes ?? Array.Empty<string>();
        });

        // Configure default HTTP options
        services.Configure<HttpClientOptions>(options =>
        {
            options.BaseAddress = new Uri("https://api.procore.com");
            options.Timeout = TimeSpan.FromMinutes(1);
            options.MaxConnectionsPerServer = 10;
            options.PooledConnectionLifetime = TimeSpan.FromMinutes(15);
            options.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);
        });

        // Register all services
        RegisterAuthenticationServices(services);
        RegisterHttpClientServices(services);
        RegisterKiotaServices(services);
        RegisterClientServices(services);
        RegisterHealthChecks(services);

        return services;
    }

    private static void RegisterAuthenticationServices(IServiceCollection services)
    {
        // Register token storage (in-memory by default, can be overridden)
        services.TryAddSingleton<ITokenStorage, InMemoryTokenStorage>();
        
        // Register token manager
        services.TryAddSingleton<ITokenManager, TokenManager>();
        
        // Register OAuth flow helper
        services.TryAddSingleton<OAuthFlowHelper>();
        
        // Register authentication handler
        services.TryAddSingleton<ProcoreAuthHandler>();
    }

    private static void RegisterHttpClientServices(IServiceCollection services)
    {
        // Register named HTTP client for Procore API
        services.AddHttpClient("Procore", (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
            client.BaseAddress = options.BaseAddress;
            client.Timeout = options.Timeout;
        })
        .AddHttpMessageHandler<ProcoreAuthHandler>()
        .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
            var handler = new SocketsHttpHandler
            {
                MaxConnectionsPerServer = options.MaxConnectionsPerServer,
                PooledConnectionLifetime = options.PooledConnectionLifetime,
                PooledConnectionIdleTimeout = options.PooledConnectionIdleTimeout,
                UseCookies = false
            };
            return handler;
        });

        // Register default HTTP client factory
        services.TryAddSingleton<IHttpClientFactory>(serviceProvider =>
            serviceProvider.GetRequiredService<IHttpClientFactory>());
    }

    private static void RegisterKiotaServices(IServiceCollection services)
    {
        // Register request adapter
        services.TryAddSingleton<Microsoft.Kiota.Abstractions.IRequestAdapter>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("Procore");
            var logger = serviceProvider.GetService<ILogger<HttpClientRequestAdapter>>();
            
            return new HttpClientRequestAdapter(
                authenticationProvider: new EmptyAuthenticationProvider(), // Auth is handled by our handler
                httpClient: httpClient);
        });
    }

    private static void RegisterClientServices(IServiceCollection services)
    {
        // Register Core client (using temporary mock for compilation)
        // TODO: Replace with actual ProcoreCoreClient once Core project compilation is fixed
        services.TryAddScoped<ICoreClient>(provider => new TemporaryCoreClient());
        
        // Register generated Kiota clients (when available)
        // These would be registered when the generation issues are resolved
        // services.TryAddScoped<CoreClient>();
    }

    private static void RegisterHealthChecks(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<ProcoreApiHealthCheck>("procore-api", 
                HealthStatus.Degraded, 
                new[] { "procore", "api", "connectivity" });
    }
}

/// <summary>
/// Configuration options for HTTP client behavior
/// </summary>
public class HttpClientOptions
{
    /// <summary>
    /// Base address for the Procore API
    /// </summary>
    public Uri BaseAddress { get; set; } = new("https://api.procore.com");

    /// <summary>
    /// Request timeout for HTTP calls
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Maximum number of connections per server
    /// </summary>
    public int MaxConnectionsPerServer { get; set; } = 10;

    /// <summary>
    /// How long pooled connections should live
    /// </summary>
    public TimeSpan PooledConnectionLifetime { get; set; } = TimeSpan.FromMinutes(15);

    /// <summary>
    /// How long pooled connections can be idle
    /// </summary>
    public TimeSpan PooledConnectionIdleTimeout { get; set; } = TimeSpan.FromMinutes(2);
}

/// <summary>
/// Health check for Procore API connectivity
/// </summary>
public class ProcoreApiHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProcoreApiHealthCheck> _logger;

    public ProcoreApiHealthCheck(IHttpClientFactory httpClientFactory, ILogger<ProcoreApiHealthCheck> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient("Procore");
            using var response = await client.GetAsync("/ping", cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Procore API is accessible");
            }
            else
            {
                var message = $"Procore API returned {response.StatusCode}";
                _logger.LogWarning("Procore API health check failed: {Message}", message);
                return HealthCheckResult.Degraded(message);
            }
        }
        catch (Exception ex)
        {
            var message = "Procore API is not accessible";
            _logger.LogError(ex, "Procore API health check failed: {Message}", message);
            return HealthCheckResult.Unhealthy(message, ex);
        }
    }
}