using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Base class for type mapper testing providing common functionality for validation,
/// performance testing, and data integrity checks.
/// </summary>
/// <typeparam name="TWrapper">The wrapper domain model type</typeparam>
/// <typeparam name="TGenerated">The generated Kiota client type</typeparam>
public abstract class TypeMapperTestBase<TWrapper, TGenerated>
    where TWrapper : class, new()
    where TGenerated : class, new()
{
    protected readonly ITestOutputHelper Output;
    protected readonly DataIntegrityValidator<TWrapper, TGenerated> IntegrityValidator;
    protected readonly PerformanceValidator PerformanceValidator;

    protected TypeMapperTestBase(ITestOutputHelper output)
    {
        Output = output;
        IntegrityValidator = new DataIntegrityValidator<TWrapper, TGenerated>();
        PerformanceValidator = new PerformanceValidator();
    }

    /// <summary>
    /// Creates the type mapper instance to be tested.
    /// </summary>
    protected abstract ITypeMapper<TWrapper, TGenerated> CreateMapper();

    /// <summary>
    /// Creates a complex wrapper object for testing.
    /// </summary>
    protected abstract TWrapper CreateComplexWrapper();

    /// <summary>
    /// Creates a complex generated object for testing.
    /// </summary>
    protected abstract TGenerated CreateComplexGenerated();

    /// <summary>
    /// Creates a minimal wrapper object for edge case testing.
    /// </summary>
    protected virtual TWrapper CreateMinimalWrapper() => new TWrapper();

    /// <summary>
    /// Creates a minimal generated object for edge case testing.
    /// </summary>
    protected virtual TGenerated CreateMinimalGenerated() => new TGenerated();

    #region Basic Mapping Tests

    [Fact]
    public virtual void Mapper_ToWrapper_ShouldMapAllProperties()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.Should().NotBeNull();
        ValidateBasicMappingToWrapper(source, result);
    }

    [Fact]
    public virtual void Mapper_ToGenerated_ShouldMapAllProperties()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexWrapper();

        // Act
        var result = mapper.MapToGenerated(source);

        // Assert
        result.Should().NotBeNull();
        ValidateBasicMappingToGenerated(source, result);
    }

    [Fact]
    public virtual void Mapper_RoundTripConversion_ShouldPreserveData()
    {
        // Arrange
        var mapper = CreateMapper();
        var originalWrapper = CreateComplexWrapper();

        // Act - Round trip conversion
        var generated = mapper.MapToGenerated(originalWrapper);
        var roundTripWrapper = mapper.MapToWrapper(generated);

        // Assert
        var integrityResult = IntegrityValidator.ValidateRoundTripIntegrity(
            originalWrapper, mapper);

        integrityResult.IsValid.Should().BeTrue(
            $"Round trip conversion should preserve all data. Failures: {string.Join(", ", integrityResult.Failures)}");
    }

    #endregion

    #region Performance Tests

    [Fact]
    public virtual void Mapper_SingleToWrapper_ShouldMeetPerformanceTarget()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();

        // Act & Assert
        var result = PerformanceValidator.MeasureOperation(
            () => mapper.MapToWrapper(source),
            "ToWrapper conversion");

        result.ElapsedMilliseconds.Should().BeLessOrEqualTo(1,
            $"ToWrapper conversion should complete within 1ms, actual: {result.ElapsedMilliseconds}ms");
        
        Output.WriteLine($"ToWrapper performance: {result.ElapsedTicks} ticks ({result.ElapsedMilliseconds}ms)");
    }

    [Fact]
    public virtual void Mapper_SingleToGenerated_ShouldMeetPerformanceTarget()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexWrapper();

        // Act & Assert
        var result = PerformanceValidator.MeasureOperation(
            () => mapper.MapToGenerated(source),
            "ToGenerated conversion");

        result.ElapsedMilliseconds.Should().BeLessOrEqualTo(1,
            $"ToGenerated conversion should complete within 1ms, actual: {result.ElapsedMilliseconds}ms");
        
        Output.WriteLine($"ToGenerated performance: {result.ElapsedTicks} ticks ({result.ElapsedMilliseconds}ms)");
    }

    [Theory]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public virtual void Mapper_BatchOperations_ShouldMaintainPerformance(int operationCount)
    {
        // Arrange
        var mapper = CreateMapper();
        var wrapperSource = CreateComplexWrapper();
        var generatedSource = CreateComplexGenerated();

        // Act & Assert - Batch ToWrapper operations
        var wrapperTimes = new List<long>();
        for (int i = 0; i < operationCount; i++)
        {
            var result = PerformanceValidator.MeasureOperation(
                () => mapper.MapToWrapper(generatedSource),
                $"Batch ToWrapper {i}");
            wrapperTimes.Add(result.ElapsedMilliseconds);
        }

        // Act & Assert - Batch ToGenerated operations
        var generatedTimes = new List<long>();
        for (int i = 0; i < operationCount; i++)
        {
            var result = PerformanceValidator.MeasureOperation(
                () => mapper.MapToGenerated(wrapperSource),
                $"Batch ToGenerated {i}");
            generatedTimes.Add(result.ElapsedMilliseconds);
        }

        // Validate performance
        var maxWrapperTime = wrapperTimes.Max();
        var maxGeneratedTime = generatedTimes.Max();
        var avgWrapperTime = wrapperTimes.Average();
        var avgGeneratedTime = generatedTimes.Average();

        Output.WriteLine($"Batch performance ({operationCount} ops):");
        Output.WriteLine($"  ToWrapper - Max: {maxWrapperTime}ms, Avg: {avgWrapperTime:F3}ms");
        Output.WriteLine($"  ToGenerated - Max: {maxGeneratedTime}ms, Avg: {avgGeneratedTime:F3}ms");

        maxWrapperTime.Should().BeLessOrEqualTo(5, "All ToWrapper operations should complete within 5ms");
        maxGeneratedTime.Should().BeLessOrEqualTo(5, "All ToGenerated operations should complete within 5ms");
        avgWrapperTime.Should().BeLessOrEqualTo(1.0, "Average ToWrapper time should be within 1ms");
        avgGeneratedTime.Should().BeLessOrEqualTo(1.0, "Average ToGenerated time should be within 1ms");
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public virtual void Mapper_ToWrapper_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mapper = CreateMapper();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => mapper.MapToWrapper(null!));
        exception.ParamName.Should().Be("source");
    }

    [Fact]
    public virtual void Mapper_ToGenerated_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mapper = CreateMapper();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => mapper.MapToGenerated(null!));
        exception.ParamName.Should().Be("source");
    }

    [Fact]
    public virtual void Mapper_TryMapToWrapper_WithNullSource_ShouldReturnFalse()
    {
        // Arrange
        var mapper = CreateMapper();

        // Act
        var result = mapper.TryMapToWrapper(null!, out var mapped);

        // Assert
        result.Should().BeFalse();
        mapped.Should().BeNull();
    }

    [Fact]
    public virtual void Mapper_TryMapToGenerated_WithNullSource_ShouldReturnFalse()
    {
        // Arrange
        var mapper = CreateMapper();

        // Act
        var result = mapper.TryMapToGenerated(null!, out var mapped);

        // Assert
        result.Should().BeFalse();
        mapped.Should().BeNull();
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public virtual void Mapper_WithMinimalData_ShouldHandleGracefully()
    {
        // Arrange
        var mapper = CreateMapper();
        var minimalWrapper = CreateMinimalWrapper();
        var minimalGenerated = CreateMinimalGenerated();

        // Act & Assert - Should not throw
        var toGenerated = mapper.MapToGenerated(minimalWrapper);
        var toWrapper = mapper.MapToWrapper(minimalGenerated);

        toGenerated.Should().NotBeNull();
        toWrapper.Should().NotBeNull();
    }

    [Fact]
    public virtual void Mapper_MetricsTracking_ShouldRecordOperations()
    {
        // Arrange
        var mapper = CreateMapper();
        var wrapper = CreateComplexWrapper();
        var generated = CreateComplexGenerated();

        // Reset metrics
        ((ITypeMapper)mapper).Metrics.Reset();

        // Act
        mapper.MapToWrapper(generated);
        mapper.MapToGenerated(wrapper);

        // Assert
        var metrics = ((ITypeMapper)mapper).Metrics;
        metrics.ToWrapperCalls.Should().Be(1);
        metrics.ToGeneratedCalls.Should().Be(1);
        metrics.ToWrapperErrors.Should().Be(0);
        metrics.ToGeneratedErrors.Should().Be(0);
    }

    #endregion

    #region Validation Helper Methods

    /// <summary>
    /// Validates basic mapping from generated to wrapper type.
    /// Override in derived classes for type-specific validation.
    /// </summary>
    protected virtual void ValidateBasicMappingToWrapper(TGenerated source, TWrapper result)
    {
        // Base implementation provides basic non-null validation
        // Derived classes should override for specific property validation
        result.Should().NotBeNull();
    }

    /// <summary>
    /// Validates basic mapping from wrapper to generated type.
    /// Override in derived classes for type-specific validation.
    /// </summary>
    protected virtual void ValidateBasicMappingToGenerated(TWrapper source, TGenerated result)
    {
        // Base implementation provides basic non-null validation
        // Derived classes should override for specific property validation
        result.Should().NotBeNull();
    }

    /// <summary>
    /// Validates that all mappers for a client meet performance and compliance requirements.
    /// </summary>
    protected void ValidateMapper(ITypeMapper mapper)
    {
        // Performance validation
        var performanceValidation = mapper.Metrics.ValidatePerformance();
        performanceValidation.OverallValid.Should().BeTrue(
            $"Mapper {mapper.GetType().Name} should meet performance targets");

        // Type information validation
        mapper.WrapperType.Should().NotBeNull();
        mapper.GeneratedType.Should().NotBeNull();
        mapper.Metrics.Should().NotBeNull();
    }

    #endregion
}

/// <summary>
/// Data integrity validator for type mapping operations.
/// </summary>
public class DataIntegrityValidator<TWrapper, TGenerated>
    where TWrapper : class, new()
    where TGenerated : class, new()
{
    public DataIntegrityResult ValidateRoundTripIntegrity(
        TWrapper originalWrapper,
        ITypeMapper<TWrapper, TGenerated> mapper)
    {
        try
        {
            // Convert wrapper → generated → wrapper
            var generated = mapper.MapToGenerated(originalWrapper);
            var roundTripWrapper = mapper.MapToWrapper(generated);

            return CompareObjects(originalWrapper, roundTripWrapper);
        }
        catch (Exception ex)
        {
            return new DataIntegrityResult
            {
                IsValid = false,
                Failures = new List<string> { $"Round trip conversion failed: {ex.Message}" }
            };
        }
    }

    private DataIntegrityResult CompareObjects(TWrapper original, TWrapper result)
    {
        var failures = new List<string>();
        var properties = typeof(TWrapper).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead) continue;

            var originalValue = property.GetValue(original);
            var resultValue = property.GetValue(result);

            if (!AreEqual(originalValue, resultValue))
            {
                failures.Add($"Property {property.Name}: expected '{originalValue}', got '{resultValue}'");
            }
        }

        return new DataIntegrityResult
        {
            IsValid = failures.Count == 0,
            Failures = failures
        };
    }

    private static bool AreEqual(object? original, object? result)
    {
        if (original == null && result == null) return true;
        if (original == null || result == null) return false;

        // Handle collections separately
        if (original is System.Collections.IDictionary originalDict && 
            result is System.Collections.IDictionary resultDict)
        {
            return CompareDictionaries(originalDict, resultDict);
        }

        if (original is System.Collections.IEnumerable originalEnum && 
            result is System.Collections.IEnumerable resultEnum &&
            !(original is string) && !(result is string))
        {
            return CompareEnumerables(originalEnum, resultEnum);
        }

        return original.Equals(result);
    }

    private static bool CompareDictionaries(System.Collections.IDictionary original, System.Collections.IDictionary result)
    {
        if (original.Count != result.Count) return false;

        foreach (var key in original.Keys)
        {
            if (!result.Contains(key)) return false;
            if (!AreEqual(original[key], result[key])) return false;
        }

        return true;
    }

    private static bool CompareEnumerables(System.Collections.IEnumerable original, System.Collections.IEnumerable result)
    {
        var originalList = original.Cast<object>().ToList();
        var resultList = result.Cast<object>().ToList();

        if (originalList.Count != resultList.Count) return false;

        return originalList.SequenceEqual(resultList);
    }
}

/// <summary>
/// Performance validator for type mapping operations.
/// </summary>
public class PerformanceValidator
{
    public PerformanceResult MeasureOperation<T>(Func<T> operation, string operationName)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = operation();
        stopwatch.Stop();

        return new PerformanceResult
        {
            OperationName = operationName,
            ElapsedTicks = stopwatch.ElapsedTicks,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Result = result
        };
    }
}

/// <summary>
/// Result of data integrity validation.
/// </summary>
public class DataIntegrityResult
{
    public bool IsValid { get; set; }
    public List<string> Failures { get; set; } = new();
}

/// <summary>
/// Result of performance measurement.
/// </summary>
public class PerformanceResult
{
    public string OperationName { get; set; } = string.Empty;
    public long ElapsedTicks { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public object? Result { get; set; }
}