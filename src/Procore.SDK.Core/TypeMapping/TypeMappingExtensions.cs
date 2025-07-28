using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Extension methods for convenient type mapping operations.
/// </summary>
public static class TypeMappingExtensions
{
    /// <summary>
    /// Maps a generated type to a wrapper domain model using the registered mapper.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="source">The generated type instance to map from</param>
    /// <param name="serviceProvider">Service provider to resolve the mapper registry</param>
    /// <returns>The mapped wrapper domain model</returns>
    public static TWrapper ToWrapper<TWrapper, TGenerated>(this TGenerated source, IServiceProvider serviceProvider)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        var mapper = registry.GetMapper<TWrapper, TGenerated>();
        return mapper.MapToWrapper(source);
    }

    /// <summary>
    /// Maps a wrapper domain model to a generated type using the registered mapper.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <param name="serviceProvider">Service provider to resolve the mapper registry</param>
    /// <returns>The mapped generated type</returns>
    public static TGenerated ToGenerated<TWrapper, TGenerated>(this TWrapper source, IServiceProvider serviceProvider)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        var mapper = registry.GetMapper<TWrapper, TGenerated>();
        return mapper.MapToGenerated(source);
    }

    /// <summary>
    /// Maps a collection of generated types to wrapper domain models.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="source">The collection of generated types to map from</param>
    /// <param name="serviceProvider">Service provider to resolve the mapper registry</param>
    /// <returns>The mapped collection of wrapper domain models</returns>
    public static IEnumerable<TWrapper> ToWrappers<TWrapper, TGenerated>(this IEnumerable<TGenerated> source, IServiceProvider serviceProvider)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        var mapper = registry.GetMapper<TWrapper, TGenerated>();
        return source.Select(mapper.MapToWrapper);
    }

    /// <summary>
    /// Maps a collection of wrapper domain models to generated types.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="source">The collection of wrapper domain models to map from</param>
    /// <param name="serviceProvider">Service provider to resolve the mapper registry</param>
    /// <returns>The mapped collection of generated types</returns>
    public static IEnumerable<TGenerated> ToGenerated<TWrapper, TGenerated>(this IEnumerable<TWrapper> source, IServiceProvider serviceProvider)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        var mapper = registry.GetMapper<TWrapper, TGenerated>();
        return source.Select(mapper.MapToGenerated);
    }

    /// <summary>
    /// Attempts to map a generated type to a wrapper domain model, returning null if mapping fails.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="source">The generated type instance to map from</param>
    /// <param name="serviceProvider">Service provider to resolve the mapper registry</param>
    /// <returns>The mapped wrapper domain model, or null if mapping fails</returns>
    public static TWrapper? TryToWrapper<TWrapper, TGenerated>(this TGenerated source, IServiceProvider serviceProvider)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        if (source == null || serviceProvider == null)
            return null;

        try
        {
            var registry = serviceProvider.GetService<ITypeMapperRegistry>();
            if (registry == null)
                return null;

            if (registry.TryGetMapper<TWrapper, TGenerated>(out var mapper) && mapper != null)
            {
                mapper.TryMapToWrapper(source, out var result);
                return result;
            }
        }
        catch
        {
            // Swallow exceptions in try methods
        }

        return null;
    }

    /// <summary>
    /// Attempts to map a wrapper domain model to a generated type, returning null if mapping fails.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <param name="serviceProvider">Service provider to resolve the mapper registry</param>
    /// <returns>The mapped generated type, or null if mapping fails</returns>
    public static TGenerated? TryToGenerated<TWrapper, TGenerated>(this TWrapper source, IServiceProvider serviceProvider)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        if (source == null || serviceProvider == null)
            return null;

        try
        {
            var registry = serviceProvider.GetService<ITypeMapperRegistry>();
            if (registry == null)
                return null;

            if (registry.TryGetMapper<TWrapper, TGenerated>(out var mapper) && mapper != null)
            {
                mapper.TryMapToGenerated(source, out var result);
                return result;
            }
        }
        catch
        {
            // Swallow exceptions in try methods
        }

        return null;
    }
}

/// <summary>
/// Extension methods for dependency injection setup.
/// </summary>
public static class TypeMappingServiceCollectionExtensions
{
    /// <summary>
    /// Adds type mapping services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddTypeMapping(this IServiceCollection services)
    {
        services.AddSingleton<ITypeMapperRegistry, TypeMapperRegistry>();
        return services;
    }

    /// <summary>
    /// Registers a type mapper in the dependency injection container and mapper registry.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <typeparam name="TMapper">The mapper implementation type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddTypeMapper<TWrapper, TGenerated, TMapper>(this IServiceCollection services)
        where TWrapper : class, new()
        where TGenerated : class, new()
        where TMapper : class, ITypeMapper<TWrapper, TGenerated>
    {
        services.AddSingleton<TMapper>();
        services.AddSingleton<ITypeMapper<TWrapper, TGenerated>>(provider => provider.GetRequiredService<TMapper>());

        // Register with the registry when the service provider is built
        services.AddSingleton(provider =>
        {
            var registry = provider.GetRequiredService<ITypeMapperRegistry>();
            var mapper = provider.GetRequiredService<TMapper>();
            registry.Register<TWrapper, TGenerated>(mapper);
            return registry;
        });

        return services;
    }
}