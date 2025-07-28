using System;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Interface for bidirectional type mapping between wrapper domain models and generated Kiota client types.
/// Provides high-performance conversion with validation and error handling.
/// </summary>
/// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
/// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
public interface ITypeMapper<TWrapper, TGenerated> 
    where TWrapper : class, new()
    where TGenerated : class, new()
{
    /// <summary>
    /// Maps from generated Kiota client type to wrapper domain model.
    /// </summary>
    /// <param name="source">The generated type instance to map from</param>
    /// <returns>The mapped wrapper domain model</returns>
    /// <exception cref="ArgumentNullException">When source is null</exception>
    /// <exception cref="TypeMappingException">When mapping fails due to data issues</exception>
    TWrapper MapToWrapper(TGenerated source);

    /// <summary>
    /// Maps from wrapper domain model to generated Kiota client type.
    /// </summary>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <returns>The mapped generated type</returns>
    /// <exception cref="ArgumentNullException">When source is null</exception>
    /// <exception cref="TypeMappingException">When mapping fails due to data issues</exception>
    TGenerated MapToGenerated(TWrapper source);

    /// <summary>
    /// Attempts to map from generated type to wrapper, returning false if mapping fails.
    /// </summary>
    /// <param name="source">The generated type instance to map from</param>
    /// <param name="result">The mapped wrapper domain model if successful</param>
    /// <returns>True if mapping succeeded, false otherwise</returns>
    bool TryMapToWrapper(TGenerated source, out TWrapper? result);

    /// <summary>
    /// Attempts to map from wrapper to generated type, returning false if mapping fails.
    /// </summary>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <param name="result">The mapped generated type if successful</param>
    /// <returns>True if mapping succeeded, false otherwise</returns>
    bool TryMapToGenerated(TWrapper source, out TGenerated? result);
}

/// <summary>
/// Non-generic interface for type mapper registration and discovery.
/// </summary>
public interface ITypeMapper
{
    /// <summary>
    /// Gets the wrapper type this mapper handles.
    /// </summary>
    Type WrapperType { get; }

    /// <summary>
    /// Gets the generated type this mapper handles.
    /// </summary>
    Type GeneratedType { get; }

    /// <summary>
    /// Gets performance metrics for this mapper.
    /// </summary>
    TypeMapperMetrics Metrics { get; }
}