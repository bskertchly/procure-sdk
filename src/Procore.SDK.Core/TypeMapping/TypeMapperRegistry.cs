using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Default implementation of type mapper registry providing thread-safe registration and discovery.
/// </summary>
public class TypeMapperRegistry : ITypeMapperRegistry
{
    private readonly ConcurrentDictionary<(Type Wrapper, Type Generated), ITypeMapper> _mappers = new();

    /// <summary>
    /// Registers a type mapper in the registry.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="mapper">The mapper instance to register</param>
    public void Register<TWrapper, TGenerated>(ITypeMapper<TWrapper, TGenerated> mapper)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        ArgumentNullException.ThrowIfNull(mapper);

        var key = (typeof(TWrapper), typeof(TGenerated));
        _mappers.AddOrUpdate(key, (ITypeMapper)mapper, (_, _) => (ITypeMapper)mapper);
    }

    /// <summary>
    /// Gets a type mapper for the specified wrapper and generated types.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <returns>The registered mapper</returns>
    /// <exception cref="InvalidOperationException">When no mapper is registered for the types</exception>
    public ITypeMapper<TWrapper, TGenerated> GetMapper<TWrapper, TGenerated>()
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var key = (typeof(TWrapper), typeof(TGenerated));
        
        if (_mappers.TryGetValue(key, out var mapper) && mapper is ITypeMapper<TWrapper, TGenerated> typedMapper)
        {
            return typedMapper;
        }

        throw new InvalidOperationException(
            $"No type mapper registered for wrapper type {typeof(TWrapper).Name} and generated type {typeof(TGenerated).Name}");
    }

    /// <summary>
    /// Attempts to get a type mapper for the specified wrapper and generated types.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="mapper">The registered mapper if found</param>
    /// <returns>True if a mapper was found, false otherwise</returns>
    public bool TryGetMapper<TWrapper, TGenerated>(out ITypeMapper<TWrapper, TGenerated>? mapper)
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        mapper = null;
        var key = (typeof(TWrapper), typeof(TGenerated));
        
        if (_mappers.TryGetValue(key, out var untypedMapper) && untypedMapper is ITypeMapper<TWrapper, TGenerated> typedMapper)
        {
            mapper = typedMapper;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets a type mapper by wrapper and generated type objects.
    /// </summary>
    /// <param name="wrapperType">The wrapper domain model type</param>
    /// <param name="generatedType">The generated Kiota client type</param>
    /// <returns>The registered mapper</returns>
    /// <exception cref="InvalidOperationException">When no mapper is registered for the types</exception>
    public ITypeMapper GetMapper(Type wrapperType, Type generatedType)
    {
        ArgumentNullException.ThrowIfNull(wrapperType);
        ArgumentNullException.ThrowIfNull(generatedType);

        var key = (wrapperType, generatedType);
        
        if (_mappers.TryGetValue(key, out var mapper))
        {
            return mapper;
        }

        throw new InvalidOperationException(
            $"No type mapper registered for wrapper type {wrapperType.Name} and generated type {generatedType.Name}");
    }

    /// <summary>
    /// Attempts to get a type mapper by wrapper and generated type objects.
    /// </summary>
    /// <param name="wrapperType">The wrapper domain model type</param>
    /// <param name="generatedType">The generated Kiota client type</param>
    /// <param name="mapper">The registered mapper if found</param>
    /// <returns>True if a mapper was found, false otherwise</returns>
    public bool TryGetMapper(Type wrapperType, Type generatedType, out ITypeMapper? mapper)
    {
        mapper = null;

        if (wrapperType == null || generatedType == null)
        {
            return false;
        }

        var key = (wrapperType, generatedType);
        return _mappers.TryGetValue(key, out mapper);
    }

    /// <summary>
    /// Gets all registered mappers.
    /// </summary>
    /// <returns>Collection of all registered mappers</returns>
    public IEnumerable<ITypeMapper> GetAllMappers()
    {
        return _mappers.Values.ToList(); // Create a snapshot to avoid modification during enumeration
    }

    /// <summary>
    /// Gets mappers for a specific wrapper type.
    /// </summary>
    /// <param name="wrapperType">The wrapper domain model type</param>
    /// <returns>Collection of mappers for the wrapper type</returns>
    public IEnumerable<ITypeMapper> GetMappersForWrapperType(Type wrapperType)
    {
        ArgumentNullException.ThrowIfNull(wrapperType);

        return _mappers
            .Where(kvp => kvp.Key.Wrapper == wrapperType)
            .Select(kvp => kvp.Value)
            .ToList();
    }

    /// <summary>
    /// Gets mappers for a specific generated type.
    /// </summary>
    /// <param name="generatedType">The generated Kiota client type</param>
    /// <returns>Collection of mappers for the generated type</returns>
    public IEnumerable<ITypeMapper> GetMappersForGeneratedType(Type generatedType)
    {
        ArgumentNullException.ThrowIfNull(generatedType);

        return _mappers
            .Where(kvp => kvp.Key.Generated == generatedType)
            .Select(kvp => kvp.Value)
            .ToList();
    }

    /// <summary>
    /// Validates performance of all registered mappers.
    /// </summary>
    /// <param name="targetAverageMs">The target average time per operation in milliseconds</param>
    /// <param name="maxErrorRate">The maximum acceptable error rate</param>
    /// <returns>Performance validation results for all mappers</returns>
    public IEnumerable<TypeMapperPerformanceResult> ValidatePerformance(double targetAverageMs = 1.0, double maxErrorRate = 0.01)
    {
        var results = new List<TypeMapperPerformanceResult>();

        foreach (var (key, mapper) in _mappers)
        {
            var validationResult = mapper.Metrics.ValidatePerformance(targetAverageMs, maxErrorRate);
            
            results.Add(new TypeMapperPerformanceResult
            {
                WrapperType = key.Wrapper,
                GeneratedType = key.Generated,
                ValidationResult = validationResult,
                Metrics = mapper.Metrics
            });
        }

        return results;
    }

    /// <summary>
    /// Clears all registered mappers.
    /// </summary>
    public void Clear()
    {
        _mappers.Clear();
    }
}