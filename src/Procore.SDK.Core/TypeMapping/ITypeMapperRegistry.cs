using System;
using System.Collections.Generic;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Registry for type mappers providing centralized registration, discovery, and performance monitoring.
/// </summary>
public interface ITypeMapperRegistry
{
    /// <summary>
    /// Registers a type mapper in the registry.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="mapper">The mapper instance to register</param>
    void Register<TWrapper, TGenerated>(ITypeMapper<TWrapper, TGenerated> mapper)
        where TWrapper : class, new()
        where TGenerated : class, new();

    /// <summary>
    /// Gets a type mapper for the specified wrapper and generated types.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <returns>The registered mapper</returns>
    /// <exception cref="InvalidOperationException">When no mapper is registered for the types</exception>
    ITypeMapper<TWrapper, TGenerated> GetMapper<TWrapper, TGenerated>()
        where TWrapper : class, new()
        where TGenerated : class, new();

    /// <summary>
    /// Attempts to get a type mapper for the specified wrapper and generated types.
    /// </summary>
    /// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
    /// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
    /// <param name="mapper">The registered mapper if found</param>
    /// <returns>True if a mapper was found, false otherwise</returns>
    bool TryGetMapper<TWrapper, TGenerated>(out ITypeMapper<TWrapper, TGenerated>? mapper)
        where TWrapper : class, new()
        where TGenerated : class, new();

    /// <summary>
    /// Gets a type mapper by wrapper and generated type objects.
    /// </summary>
    /// <param name="wrapperType">The wrapper domain model type</param>
    /// <param name="generatedType">The generated Kiota client type</param>
    /// <returns>The registered mapper</returns>
    /// <exception cref="InvalidOperationException">When no mapper is registered for the types</exception>
    ITypeMapper GetMapper(Type wrapperType, Type generatedType);

    /// <summary>
    /// Attempts to get a type mapper by wrapper and generated type objects.
    /// </summary>
    /// <param name="wrapperType">The wrapper domain model type</param>
    /// <param name="generatedType">The generated Kiota client type</param>
    /// <param name="mapper">The registered mapper if found</param>
    /// <returns>True if a mapper was found, false otherwise</returns>
    bool TryGetMapper(Type wrapperType, Type generatedType, out ITypeMapper? mapper);

    /// <summary>
    /// Gets all registered mappers.
    /// </summary>
    /// <returns>Collection of all registered mappers</returns>
    IEnumerable<ITypeMapper> GetAllMappers();

    /// <summary>
    /// Gets mappers for a specific wrapper type.
    /// </summary>
    /// <param name="wrapperType">The wrapper domain model type</param>
    /// <returns>Collection of mappers for the wrapper type</returns>
    IEnumerable<ITypeMapper> GetMappersForWrapperType(Type wrapperType);

    /// <summary>
    /// Gets mappers for a specific generated type.
    /// </summary>
    /// <param name="generatedType">The generated Kiota client type</param>
    /// <returns>Collection of mappers for the generated type</returns>
    IEnumerable<ITypeMapper> GetMappersForGeneratedType(Type generatedType);

    /// <summary>
    /// Validates performance of all registered mappers.
    /// </summary>
    /// <param name="targetAverageMs">The target average time per operation in milliseconds</param>
    /// <param name="maxErrorRate">The maximum acceptable error rate</param>
    /// <returns>Performance validation results for all mappers</returns>
    IEnumerable<TypeMapperPerformanceResult> ValidatePerformance(double targetAverageMs = 1.0, double maxErrorRate = 0.01);

    /// <summary>
    /// Clears all registered mappers.
    /// </summary>
    void Clear();
}

/// <summary>
/// Performance validation result for a specific type mapper.
/// </summary>
public class TypeMapperPerformanceResult
{
    public Type WrapperType { get; set; } = typeof(object);
    public Type GeneratedType { get; set; } = typeof(object);
    public MetricsValidationResult ValidationResult { get; set; } = new();
    public TypeMapperMetrics Metrics { get; set; } = new();
}