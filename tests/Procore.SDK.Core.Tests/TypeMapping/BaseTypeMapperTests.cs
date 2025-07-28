using System;
using System.Diagnostics;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive test suite for BaseTypeMapper abstract functionality and utility methods.
/// Tests the base infrastructure that all type mappers inherit including performance tracking,
/// error handling, enum mapping, and DateTime conversion utilities.
/// </summary>
public class BaseTypeMapperTests
{
    private readonly TestTypeMapper _mapper;

    public BaseTypeMapperTests()
    {
        _mapper = new TestTypeMapper();
    }

    #region Abstract Base Functionality Tests

    [Fact]
    public void TypeProperties_ShouldReturnCorrectTypes()
    {
        // Assert
        _mapper.WrapperType.Should().Be<TestWrapper>();
        _mapper.GeneratedType.Should().Be<TestGenerated>();
    }

    [Fact]
    public void MapToWrapper_WithValidSource_ShouldCallDoMapToWrapper()
    {
        // Arrange
        var source = new TestGenerated { Value = "test" };

        // Act
        var result = _mapper.MapToWrapper(source);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("mapped from test");
        _mapper.DoMapToWrapperCalled.Should().BeTrue();
    }

    [Fact]
    public void MapToGenerated_WithValidSource_ShouldCallDoMapToGenerated()
    {
        // Arrange
        var source = new TestWrapper { Value = "test" };

        // Act
        var result = _mapper.MapToGenerated(source);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("mapped from test");
        _mapper.DoMapToGeneratedCalled.Should().BeTrue();
    }

    [Fact]
    public void MapToWrapper_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => _mapper.MapToWrapper(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MapToGenerated_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => _mapper.MapToGenerated(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Performance Tracking Tests

    [Fact]
    public void MapToWrapper_ShouldRecordPerformanceMetrics()
    {
        // Arrange
        var source = new TestGenerated { Value = "test" };

        // Act
        var result = _mapper.MapToWrapper(source);

        // Assert
        result.Should().NotBeNull();
        _mapper.Metrics.ToWrapperCalls.Should().Be(1);
        _mapper.Metrics.ToWrapperTimeMs.Should().BeGreaterOrEqualTo(0);
        _mapper.Metrics.ToWrapperErrors.Should().Be(0);
    }

    [Fact]
    public void MapToGenerated_ShouldRecordPerformanceMetrics()
    {
        // Arrange
        var source = new TestWrapper { Value = "test" };

        // Act
        var result = _mapper.MapToGenerated(source);

        // Assert
        result.Should().NotBeNull();
        _mapper.Metrics.ToGeneratedCalls.Should().Be(1);
        _mapper.Metrics.ToGeneratedTimeMs.Should().BeGreaterOrEqualTo(0);
        _mapper.Metrics.ToGeneratedErrors.Should().Be(0);
    }

    [Fact]
    public void MapToWrapper_WhenExceptionThrown_ShouldRecordError()
    {
        // Arrange
        var source = new TestGenerated { Value = "throw" }; // Special value to trigger exception

        // Act & Assert
        var action = () => _mapper.MapToWrapper(source);
        action.Should().Throw<TypeMappingException>();

        // Verify error was recorded
        _mapper.Metrics.ToWrapperCalls.Should().Be(1);
        _mapper.Metrics.ToWrapperErrors.Should().Be(1);
        _mapper.Metrics.ToWrapperErrorRate.Should().Be(1.0);
    }

    [Fact]
    public void MapToGenerated_WhenExceptionThrown_ShouldRecordError()
    {
        // Arrange
        var source = new TestWrapper { Value = "throw" }; // Special value to trigger exception

        // Act & Assert
        var action = () => _mapper.MapToGenerated(source);
        action.Should().Throw<TypeMappingException>();

        // Verify error was recorded
        _mapper.Metrics.ToGeneratedCalls.Should().Be(1);
        _mapper.Metrics.ToGeneratedErrors.Should().Be(1);
        _mapper.Metrics.ToGeneratedErrorRate.Should().Be(1.0);
    }

    #endregion

    #region TryMap Methods Tests

    [Fact]
    public void TryMapToWrapper_WithValidSource_ShouldReturnTrueAndResult()
    {
        // Arrange
        var source = new TestGenerated { Value = "test" };

        // Act
        var success = _mapper.TryMapToWrapper(source, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Value.Should().Be("mapped from test");
    }

    [Fact]
    public void TryMapToGenerated_WithValidSource_ShouldReturnTrueAndResult()
    {
        // Arrange
        var source = new TestWrapper { Value = "test" };

        // Act
        var success = _mapper.TryMapToGenerated(source, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Value.Should().Be("mapped from test");
    }

    [Fact]
    public void TryMapToWrapper_WithNullSource_ShouldReturnFalse()
    {
        // Act
        var success = _mapper.TryMapToWrapper(null!, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryMapToGenerated_WithNullSource_ShouldReturnFalse()
    {
        // Act
        var success = _mapper.TryMapToGenerated(null!, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryMapToWrapper_WhenExceptionThrown_ShouldReturnFalse()
    {
        // Arrange
        var source = new TestGenerated { Value = "throw" };

        // Act
        var success = _mapper.TryMapToWrapper(source, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
        
        // Should still record metrics for failed attempt
        _mapper.Metrics.ToWrapperCalls.Should().Be(1);
        _mapper.Metrics.ToWrapperErrors.Should().Be(1);
    }

    [Fact]
    public void TryMapToGenerated_WhenExceptionThrown_ShouldReturnFalse()
    {
        // Arrange
        var source = new TestWrapper { Value = "throw" };

        // Act
        var success = _mapper.TryMapToGenerated(source, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
        
        // Should still record metrics for failed attempt
        _mapper.Metrics.ToGeneratedCalls.Should().Be(1);
        _mapper.Metrics.ToGeneratedErrors.Should().Be(1);
    }

    #endregion

    #region Enum Mapping Utility Tests

    [Fact]
    public void MapEnum_WithValidEnum_ShouldMapCorrectly()
    {
        // Act
        var result = TestTypeMapper.TestMapEnum(TestSourceEnum.Value1, TestTargetEnum.Default);

        // Assert
        result.Should().Be(TestTargetEnum.Value1);
    }

    [Fact]
    public void MapEnum_WithNullEnum_ShouldReturnDefault()
    {
        // Act
        var result = TestTypeMapper.TestMapEnum(null, TestTargetEnum.Default);

        // Assert
        result.Should().Be(TestTargetEnum.Default);
    }

    [Fact]
    public void MapEnum_WithUnmappableEnum_ShouldReturnDefault()
    {
        // Act
        var result = TestTypeMapper.TestMapEnum(TestSourceEnum.Unmappable, TestTargetEnum.Default);

        // Assert
        result.Should().Be(TestTargetEnum.Default);
    }

    [Fact]
    public void MapNullableEnum_WithValidEnum_ShouldMapCorrectly()
    {
        // Act
        var result = TestTypeMapper.TestMapNullableEnum(TestSourceEnum.Value2);

        // Assert
        result.Should().Be(TestTargetEnum.Value2);
    }

    [Fact]
    public void MapNullableEnum_WithNullEnum_ShouldReturnNull()
    {
        // Act
        var result = TestTypeMapper.TestMapNullableEnum(null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void MapNullableEnum_WithUnmappableEnum_ShouldReturnNull()
    {
        // Act
        var result = TestTypeMapper.TestMapNullableEnum(TestSourceEnum.Unmappable);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region DateTime Mapping Utility Tests

    [Fact]
    public void MapDateTime_WithValidDateTimeOffset_ShouldMapCorrectly()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(2023, 6, 15, 14, 30, 0, TimeSpan.Zero);

        // Act
        var result = TestTypeMapper.TestMapDateTime(dateTimeOffset);

        // Assert
        result.Should().Be(new DateTime(2023, 6, 15, 14, 30, 0));
    }

    [Fact]
    public void MapDateTime_WithNullDateTimeOffset_ShouldReturnMinValue()
    {
        // Act
        var result = TestTypeMapper.TestMapDateTime(null);

        // Assert
        result.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void MapNullableDateTime_WithValidDateTimeOffset_ShouldMapCorrectly()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(2023, 6, 15, 14, 30, 0, TimeSpan.Zero);

        // Act
        var result = TestTypeMapper.TestMapNullableDateTime(dateTimeOffset);

        // Assert
        result.Should().Be(new DateTime(2023, 6, 15, 14, 30, 0));
    }

    [Fact]
    public void MapNullableDateTime_WithNullDateTimeOffset_ShouldReturnNull()
    {
        // Act
        var result = TestTypeMapper.TestMapNullableDateTime(null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void MapDateToDateTime_WithValidDate_ShouldMapCorrectly()
    {
        // Arrange
        var date = new Microsoft.Kiota.Abstractions.Date(2023, 6, 15);

        // Act
        var result = TestTypeMapper.TestMapDateToDateTime(date);

        // Assert
        result.Should().Be(new DateTime(2023, 6, 15));
    }

    [Fact]
    public void MapDateToDateTime_WithNullDate_ShouldReturnNull()
    {
        // Act
        var result = TestTypeMapper.TestMapDateToDateTime(null);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Error Handling and Exception Wrapping Tests

    [Fact]
    public void MapToWrapper_WhenInnerExceptionThrown_ShouldWrapInTypeMappingException()
    {
        // Arrange
        var source = new TestGenerated { Value = "inner-exception" };

        // Act & Assert
        var action = () => _mapper.MapToWrapper(source);
        var exception = action.Should().Throw<TypeMappingException>().Which;
        
        exception.Message.Should().Contain("Failed to map from TestGenerated to TestWrapper");
        exception.InnerException.Should().NotBeNull();
        exception.InnerException!.Message.Should().Be("Inner exception");
        exception.SourceType.Should().Be<TestGenerated>();
        exception.TargetType.Should().Be<TestWrapper>();
    }

    [Fact]
    public void MapToGenerated_WhenInnerExceptionThrown_ShouldWrapInTypeMappingException()
    {
        // Arrange
        var source = new TestWrapper { Value = "inner-exception" };

        // Act & Assert
        var action = () => _mapper.MapToGenerated(source);
        var exception = action.Should().Throw<TypeMappingException>().Which;
        
        exception.Message.Should().Contain("Failed to map from TestWrapper to TestGenerated");
        exception.InnerException.Should().NotBeNull();
        exception.InnerException!.Message.Should().Be("Inner exception");
        exception.SourceType.Should().Be<TestWrapper>();
        exception.TargetType.Should().Be<TestGenerated>();
    }

    [Fact]
    public void MapToWrapper_WhenTypeMappingExceptionThrown_ShouldNotWrap()
    {
        // Arrange
        var source = new TestGenerated { Value = "type-mapping-exception" };

        // Act & Assert
        var action = () => _mapper.MapToWrapper(source);
        var exception = action.Should().Throw<TypeMappingException>().Which;
        
        exception.Message.Should().Be("Direct TypeMappingException");
        exception.InnerException.Should().BeNull(); // Should not be wrapped
    }

    #endregion

    #region Performance Requirements Tests

    [Fact]
    public void BaseTypeMapper_PerformanceOverhead_ShouldBeMinimal()
    {
        // Arrange
        var source = new TestGenerated { Value = "test" };
        var stopwatch = new Stopwatch();

        // Act - Measure the overhead of base mapper functionality
        stopwatch.Start();
        for (int i = 0; i < 1000; i++)
        {
            _mapper.MapToWrapper(source);
        }
        stopwatch.Stop();

        // Assert - Base mapper overhead should not prevent <1ms per operation
        var averageMs = (double)stopwatch.ElapsedMilliseconds / 1000;
        averageMs.Should().BeLessOrEqualTo(1.0, "Base mapper overhead should allow <1ms per operation");
        
        // Verify all operations were recorded
        _mapper.Metrics.ToWrapperCalls.Should().Be(1000);
        _mapper.Metrics.AverageToWrapperTimeMs.Should().BeLessOrEqualTo(1.0);
    }

    #endregion

    #region Test Helper Classes and Enums

    public class TestWrapper
    {
        public string Value { get; set; } = string.Empty;
    }

    public class TestGenerated
    {
        public string Value { get; set; } = string.Empty;
    }

    public enum TestSourceEnum
    {
        Value1,
        Value2,
        Unmappable
    }

    public enum TestTargetEnum
    {
        Default,
        Value1,
        Value2
    }

    public class TestTypeMapper : BaseTypeMapper<TestWrapper, TestGenerated>
    {
        public bool DoMapToWrapperCalled { get; private set; }
        public bool DoMapToGeneratedCalled { get; private set; }

        protected override TestWrapper DoMapToWrapper(TestGenerated source)
        {
            DoMapToWrapperCalled = true;

            return source.Value switch
            {
                "throw" => throw new TypeMappingException("Test exception"),
                "inner-exception" => throw new InvalidOperationException("Inner exception"),
                "type-mapping-exception" => throw new TypeMappingException("Direct TypeMappingException"),
                _ => new TestWrapper { Value = $"mapped from {source.Value}" }
            };
        }

        protected override TestGenerated DoMapToGenerated(TestWrapper source)
        {
            DoMapToGeneratedCalled = true;

            return source.Value switch
            {
                "throw" => throw new TypeMappingException("Test exception"),
                "inner-exception" => throw new InvalidOperationException("Inner exception"),
                "type-mapping-exception" => throw new TypeMappingException("Direct TypeMappingException"),
                _ => new TestGenerated { Value = $"mapped from {source.Value}" }
            };
        }

        // Public wrappers for testing protected utility methods
        public static TestTargetEnum TestMapEnum(TestSourceEnum? source, TestTargetEnum defaultValue)
        {
            return MapEnum(source, defaultValue);
        }

        public static TestTargetEnum? TestMapNullableEnum(TestSourceEnum? source)
        {
            return MapNullableEnum<TestSourceEnum, TestTargetEnum>(source);
        }

        public static DateTime TestMapDateTime(DateTimeOffset? source)
        {
            return MapDateTime(source);
        }

        public static DateTime? TestMapNullableDateTime(DateTimeOffset? source)
        {
            return MapNullableDateTime(source);
        }

        public static DateTime? TestMapDateToDateTime(Microsoft.Kiota.Abstractions.Date? source)
        {
            return MapDateToDateTime(source);
        }
    }

    #endregion
}