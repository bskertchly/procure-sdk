using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers.CorrelationId;
using Serilog.Events;
// using Serilog.Formatting.Compact;
using Procore.SDK.Core.Resilience;
using Procore.SDK.Core.Logging;
using Procore.SDK.Core.ErrorHandling;

namespace Procore.SDK.Core.Extensions;

/// <summary>
/// Extension methods for registering resilience and error handling services.
/// </summary>
public static class ResilienceServiceCollectionExtensions
{
    /// <summary>
    /// Adds Procore SDK resilience services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddProcoreResilience(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure resilience options
        services.Configure<ResilienceOptions>(options => configuration.GetSection(ResilienceOptions.SectionName).Bind(options));
        
        // Register core resilience services
        services.AddSingleton<PolicyFactory>();
        services.AddTransient<ProcoreResilienceHandler>();
        services.AddTransient<ErrorMapper>();
        services.AddSingleton<StructuredLogger>();

        // Add logging configuration
        services.AddSerilogResilience(configuration);

        return services;
    }

    /// <summary>
    /// Adds Procore SDK resilience services with custom options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure resilience options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddProcoreResilience(
        this IServiceCollection services,
        Action<ResilienceOptions> configureOptions)
    {
        services.Configure(configureOptions);
        
        // Register core resilience services
        services.AddSingleton<PolicyFactory>();
        services.AddTransient<ProcoreResilienceHandler>();
        services.AddTransient<ErrorMapper>();
        services.AddSingleton<StructuredLogger>();

        return services;
    }

    /// <summary>
    /// Configures Serilog for structured logging with correlation ID enrichment.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSerilogResilience(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("Application", "Procore.SDK.Core")
            .WriteTo.Console()
            .WriteTo.File(
                "logs/procore-sdk-.log",
                rollingInterval: Serilog.RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();

        // Add Serilog to DI container
        services.AddLogging(builder => builder.AddSerilog(dispose: true));

        return services;
    }

    /// <summary>
    /// Adds resilience policies to an HTTP client.
    /// </summary>
    /// <param name="builder">The HTTP client builder.</param>
    /// <returns>The HTTP client builder for chaining.</returns>
    public static IHttpClientBuilder AddProcoreResilience(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<ProcoreResilienceHandler>();
    }

    /// <summary>
    /// Validates resilience configuration options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection ValidateResilienceOptions(this IServiceCollection services)
    {
        services.AddOptions<ResilienceOptions>()
            .ValidateDataAnnotations()
            .Validate(options =>
            {
                // Custom validation logic
                if (options.Retry.MaxAttempts > 0 && options.Retry.BaseDelayMs <= 0)
                {
                    return false;
                }

                if (options.CircuitBreaker.Enabled && options.CircuitBreaker.FailureThreshold <= 0)
                {
                    return false;
                }

                if (options.Timeout.Enabled && options.Timeout.DefaultTimeoutInSeconds <= 0)
                {
                    return false;
                }

                return true;
            }, "Invalid resilience configuration");

        return services;
    }
}