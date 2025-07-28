using System;
using System.Diagnostics;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Base implementation for type mappers providing common functionality including performance tracking,
/// error handling, and validation. Derived classes implement the actual mapping logic.
/// </summary>
/// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
/// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
public abstract class BaseTypeMapper<TWrapper, TGenerated> : ITypeMapper<TWrapper, TGenerated>, ITypeMapper
    where TWrapper : class, new()
    where TGenerated : class, new()
{
    private readonly TypeMapperMetrics _metrics = new();

    /// <summary>
    /// Gets the wrapper type this mapper handles.
    /// </summary>
    public Type WrapperType => typeof(TWrapper);

    /// <summary>
    /// Gets the generated type this mapper handles.
    /// </summary>
    public Type GeneratedType => typeof(TGenerated);

    /// <summary>
    /// Gets performance metrics for this mapper.
    /// </summary>
    public TypeMapperMetrics Metrics => _metrics;

    /// <summary>
    /// Maps from generated Kiota client type to wrapper domain model with performance tracking.
    /// </summary>
    /// <param name="source">The generated type instance to map from</param>
    /// <returns>The mapped wrapper domain model</returns>
    /// <exception cref="ArgumentNullException">When source is null</exception>
    /// <exception cref="TypeMappingException">When mapping fails due to data issues</exception>
    public TWrapper MapToWrapper(TGenerated source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var stopwatch = Stopwatch.StartNew();
        var success = false;

        try
        {
            var result = DoMapToWrapper(source);
            success = true;
            return result;
        }
        catch (Exception ex) when (!(ex is TypeMappingException))
        {
            throw new TypeMappingException(
                $"Failed to map from {typeof(TGenerated).Name} to {typeof(TWrapper).Name}: {ex.Message}",
                ex,
                typeof(TGenerated),
                typeof(TWrapper));
        }
        finally
        {
            stopwatch.Stop();
            _metrics.RecordToWrapper(stopwatch.ElapsedMilliseconds, success);
        }
    }

    /// <summary>
    /// Maps from wrapper domain model to generated Kiota client type with performance tracking.
    /// </summary>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <returns>The mapped generated type</returns>
    /// <exception cref="ArgumentNullException">When source is null</exception>
    /// <exception cref="TypeMappingException">When mapping fails due to data issues</exception>
    public TGenerated MapToGenerated(TWrapper source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var stopwatch = Stopwatch.StartNew();
        var success = false;

        try
        {
            var result = DoMapToGenerated(source);
            success = true;
            return result;
        }
        catch (Exception ex) when (!(ex is TypeMappingException))
        {
            throw new TypeMappingException(
                $"Failed to map from {typeof(TWrapper).Name} to {typeof(TGenerated).Name}: {ex.Message}",
                ex,
                typeof(TWrapper),
                typeof(TGenerated));
        }
        finally
        {
            stopwatch.Stop();
            _metrics.RecordToGenerated(stopwatch.ElapsedMilliseconds, success);
        }
    }

    /// <summary>
    /// Attempts to map from generated type to wrapper, returning false if mapping fails.
    /// </summary>
    /// <param name="source">The generated type instance to map from</param>
    /// <param name="result">The mapped wrapper domain model if successful</param>
    /// <returns>True if mapping succeeded, false otherwise</returns>
    public bool TryMapToWrapper(TGenerated source, out TWrapper? result)
    {
        result = null;

        if (source == null)
        {
            return false;
        }

        var stopwatch = Stopwatch.StartNew();
        var success = false;

        try
        {
            result = DoMapToWrapper(source);
            success = true;
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            stopwatch.Stop();
            _metrics.RecordToWrapper(stopwatch.ElapsedMilliseconds, success);
        }
    }

    /// <summary>
    /// Attempts to map from wrapper to generated type, returning false if mapping fails.
    /// </summary>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <param name="result">The mapped generated type if successful</param>
    /// <returns>True if mapping succeeded, false otherwise</returns>
    public bool TryMapToGenerated(TWrapper source, out TGenerated? result)
    {
        result = null;

        if (source == null)
        {
            return false;
        }

        var stopwatch = Stopwatch.StartNew();
        var success = false;

        try
        {
            result = DoMapToGenerated(source);
            success = true;
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            stopwatch.Stop();
            _metrics.RecordToGenerated(stopwatch.ElapsedMilliseconds, success);
        }
    }

    /// <summary>
    /// Derived classes implement this method to perform the actual mapping from generated to wrapper type.
    /// </summary>
    /// <param name="source">The generated type instance to map from</param>
    /// <returns>The mapped wrapper domain model</returns>
    protected abstract TWrapper DoMapToWrapper(TGenerated source);

    /// <summary>
    /// Derived classes implement this method to perform the actual mapping from wrapper to generated type.
    /// </summary>
    /// <param name="source">The wrapper domain model to map from</param>
    /// <returns>The mapped generated type</returns>
    protected abstract TGenerated DoMapToGenerated(TWrapper source);

    /// <summary>
    /// Helper method to safely map enum values with fallback to default.
    /// </summary>
    /// <typeparam name="TSource">Source enum type</typeparam>
    /// <typeparam name="TTarget">Target enum type</typeparam>
    /// <param name="source">Source enum value</param>
    /// <param name="defaultValue">Default value if mapping fails</param>
    /// <returns>Mapped enum value or default</returns>
    protected static TTarget MapEnum<TSource, TTarget>(TSource? source, TTarget defaultValue)
        where TSource : struct, Enum
        where TTarget : struct, Enum
    {
        if (source == null)
            return defaultValue;

        if (Enum.TryParse<TTarget>(source.ToString(), ignoreCase: true, out var result))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Helper method to safely map nullable enum values.
    /// </summary>
    /// <typeparam name="TSource">Source enum type</typeparam>
    /// <typeparam name="TTarget">Target enum type</typeparam>
    /// <param name="source">Source enum value</param>
    /// <returns>Mapped enum value or null</returns>
    protected static TTarget? MapNullableEnum<TSource, TTarget>(TSource? source)
        where TSource : struct, Enum
        where TTarget : struct, Enum
    {
        if (source == null)
            return null;

        if (Enum.TryParse<TTarget>(source.ToString(), ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Helper method to safely map DateTime values, handling null and conversion errors.
    /// </summary>
    /// <param name="source">Source DateTime value</param>
    /// <returns>Mapped DateTime value</returns>
    protected static DateTime MapDateTime(DateTimeOffset? source)
    {
        return source?.DateTime ?? DateTime.MinValue;
    }

    /// <summary>
    /// Helper method to safely map nullable DateTime values.
    /// </summary>
    /// <param name="source">Source DateTime value</param>
    /// <returns>Mapped nullable DateTime value</returns>
    protected static DateTime? MapNullableDateTime(DateTimeOffset? source)
    {
        return source?.DateTime;
    }

    /// <summary>
    /// Helper method to safely map Date values to DateTime.
    /// </summary>
    /// <param name="source">Source Date value</param>
    /// <returns>Mapped DateTime value</returns>
    protected static DateTime? MapDateToDateTime(Microsoft.Kiota.Abstractions.Date? source)
    {
        if (source == null)
            return null;
            
        return new DateTime(source.Value.Year, source.Value.Month, source.Value.Day);
    }
}