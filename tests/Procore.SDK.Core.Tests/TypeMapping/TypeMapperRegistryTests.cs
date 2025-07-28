using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Tests for the TypeMapperRegistry to ensure proper registration, discovery, and performance monitoring.
/// </summary>
public class TypeMapperRegistryTests
{
    private readonly TypeMapperRegistry _registry;

    public TypeMapperRegistryTests()
    {
        _registry = new TypeMapperRegistry();
    }

    [Fact]
    public void Register_WithValidMapper_ShouldRegisterSuccessfully()
    {
        // Arrange
        var mapper = new TestTypeMapper();

        // Act
        _registry.Register<TestWrapper, TestGenerated>(mapper);

        // Assert
        var retrievedMapper = _registry.GetMapper<TestWrapper, TestGenerated>();
        retrievedMapper.Should().BeSameAs(mapper);
    }

    [Fact]
    public void Register_WithNullMapper_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            _registry.Register<TestWrapper, TestGenerated>(null!));
    }

    [Fact]
    public void GetMapper_WithRegisteredTypes_ShouldReturnMapper()
    {
        // Arrange
        var mapper = new TestTypeMapper();
        _registry.Register<TestWrapper, TestGenerated>(mapper);

        // Act
        var retrievedMapper = _registry.GetMapper<TestWrapper, TestGenerated>();

        // Assert
        retrievedMapper.Should().BeSameAs(mapper);
    }

    [Fact]
    public void GetMapper_WithUnregisteredTypes_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            _registry.GetMapper<TestWrapper, TestGenerated>());
    }

    [Fact]
    public void TryGetMapper_WithRegisteredTypes_ShouldReturnTrueAndMapper()
    {
        // Arrange
        var mapper = new TestTypeMapper();
        _registry.Register<TestWrapper, TestGenerated>(mapper);

        // Act
        var success = _registry.TryGetMapper<TestWrapper, TestGenerated>(out var retrievedMapper);

        // Assert
        success.Should().BeTrue();
        retrievedMapper.Should().BeSameAs(mapper);
    }

    [Fact]
    public void TryGetMapper_WithUnregisteredTypes_ShouldReturnFalse()
    {
        // Act
        var success = _registry.TryGetMapper<TestWrapper, TestGenerated>(out var mapper);

        // Assert
        success.Should().BeFalse();
        mapper.Should().BeNull();
    }

    [Fact]
    public void GetMapper_ByTypeObjects_WithRegisteredTypes_ShouldReturnMapper()
    {
        // Arrange
        var mapper = new TestTypeMapper();
        _registry.Register<TestWrapper, TestGenerated>(mapper);

        // Act
        var retrievedMapper = _registry.GetMapper(typeof(TestWrapper), typeof(TestGenerated));

        // Assert
        retrievedMapper.Should().BeSameAs(mapper);
        retrievedMapper.WrapperType.Should().Be(typeof(TestWrapper));
        retrievedMapper.GeneratedType.Should().Be(typeof(TestGenerated));
    }

    [Fact]
    public void GetMapper_ByTypeObjects_WithUnregisteredTypes_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            _registry.GetMapper(typeof(TestWrapper), typeof(TestGenerated)));
    }

    [Fact]
    public void TryGetMapper_ByTypeObjects_WithRegisteredTypes_ShouldReturnTrueAndMapper()
    {
        // Arrange
        var mapper = new TestTypeMapper();
        _registry.Register<TestWrapper, TestGenerated>(mapper);

        // Act
        var success = _registry.TryGetMapper(typeof(TestWrapper), typeof(TestGenerated), out var retrievedMapper);

        // Assert
        success.Should().BeTrue();
        retrievedMapper.Should().BeSameAs(mapper);
    }

    [Fact]
    public void TryGetMapper_ByTypeObjects_WithNullTypes_ShouldReturnFalse()
    {
        // Act
        var success = _registry.TryGetMapper(null!, typeof(TestGenerated), out var mapper);

        // Assert
        success.Should().BeFalse();
        mapper.Should().BeNull();
    }

    [Fact]
    public void GetAllMappers_WithMultipleRegistrations_ShouldReturnAllMappers()
    {
        // Arrange
        var mapper1 = new TestTypeMapper();
        var mapper2 = new TestTypeMapper2();
        _registry.Register<TestWrapper, TestGenerated>(mapper1);
        _registry.Register<TestWrapper2, TestGenerated2>(mapper2);

        // Act
        var allMappers = _registry.GetAllMappers().ToList();

        // Assert
        allMappers.Should().HaveCount(2);
        allMappers.Should().Contain(mapper1);
        allMappers.Should().Contain(mapper2);
    }

    [Fact]
    public void GetMappersForWrapperType_WithMatchingMappers_ShouldReturnCorrectMappers()
    {
        // Arrange
        var mapper1 = new TestTypeMapper();
        var mapper2 = new TestTypeMapper2();
        var mapper3 = new TestTypeMapper3();
        _registry.Register<TestWrapper, TestGenerated>(mapper1);
        _registry.Register<TestWrapper2, TestGenerated2>(mapper2);
        _registry.Register<TestWrapper, TestGenerated2>(mapper3); // Same wrapper, different generated

        // Act
        var wrappersForTestWrapper = _registry.GetMappersForWrapperType(typeof(TestWrapper)).ToList();

        // Assert
        wrappersForTestWrapper.Should().HaveCount(2);
        wrappersForTestWrapper.Should().Contain(mapper1);
        wrappersForTestWrapper.Should().Contain(mapper3);
        wrappersForTestWrapper.Should().NotContain(mapper2);
    }

    [Fact]
    public void GetMappersForGeneratedType_WithMatchingMappers_ShouldReturnCorrectMappers()
    {
        // Arrange
        var mapper1 = new TestTypeMapper();
        var mapper2 = new TestTypeMapper2();
        var mapper3 = new TestTypeMapper3();
        _registry.Register<TestWrapper, TestGenerated>(mapper1);
        _registry.Register<TestWrapper2, TestGenerated2>(mapper2);
        _registry.Register<TestWrapper, TestGenerated2>(mapper3);

        // Act
        var mappersForTestGenerated2 = _registry.GetMappersForGeneratedType(typeof(TestGenerated2)).ToList();

        // Assert
        mappersForTestGenerated2.Should().HaveCount(2);
        mappersForTestGenerated2.Should().Contain(mapper2);
        mappersForTestGenerated2.Should().Contain(mapper3);
        mappersForTestGenerated2.Should().NotContain(mapper1);
    }

    [Fact]
    public void ValidatePerformance_WithMappers_ShouldReturnValidationResults()
    {
        // Arrange
        var mapper1 = new TestTypeMapper();
        var mapper2 = new TestTypeMapper2();
        _registry.Register<TestWrapper, TestGenerated>(mapper1);
        _registry.Register<TestWrapper2, TestGenerated2>(mapper2);

        // Simulate some usage to generate metrics
        mapper1.MapToWrapper(new TestGenerated());
        mapper2.MapToWrapper(new TestGenerated2());

        // Act
        var results = _registry.ValidatePerformance(targetAverageMs: 1.0).ToList();

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(r => r.ValidationResult != null);
        results.Should().OnlyContain(r => r.Metrics != null);
        
        var result1 = results.First(r => r.WrapperType == typeof(TestWrapper));
        result1.GeneratedType.Should().Be(typeof(TestGenerated));
        
        var result2 = results.First(r => r.WrapperType == typeof(TestWrapper2));
        result2.GeneratedType.Should().Be(typeof(TestGenerated2));
    }

    [Fact]
    public void Clear_WithMultipleMappers_ShouldRemoveAllMappers()
    {
        // Arrange
        var mapper1 = new TestTypeMapper();
        var mapper2 = new TestTypeMapper2();
        _registry.Register<TestWrapper, TestGenerated>(mapper1);
        _registry.Register<TestWrapper2, TestGenerated2>(mapper2);

        // Act
        _registry.Clear();

        // Assert
        _registry.GetAllMappers().Should().BeEmpty();
        
        // Should throw when trying to get cleared mappers
        Assert.Throws<InvalidOperationException>(() => 
            _registry.GetMapper<TestWrapper, TestGenerated>());
    }

    [Fact]
    public void Register_SameTypesMultipleTimes_ShouldReplaceMapper()
    {
        // Arrange
        var mapper1 = new TestTypeMapper();
        var mapper2 = new TestTypeMapper();

        // Act
        _registry.Register<TestWrapper, TestGenerated>(mapper1);
        _registry.Register<TestWrapper, TestGenerated>(mapper2);

        // Assert
        var retrievedMapper = _registry.GetMapper<TestWrapper, TestGenerated>();
        retrievedMapper.Should().BeSameAs(mapper2);
        retrievedMapper.Should().NotBeSameAs(mapper1);
    }

    // Test classes for mapper registration tests
    public class TestWrapper { public int Id { get; set; } }
    public class TestGenerated { public int Id { get; set; } }
    public class TestWrapper2 { public int Id { get; set; } }
    public class TestGenerated2 { public int Id { get; set; } }

    public class TestTypeMapper : BaseTypeMapper<TestWrapper, TestGenerated>
    {
        protected override TestWrapper DoMapToWrapper(TestGenerated source) => 
            new TestWrapper { Id = source.Id };

        protected override TestGenerated DoMapToGenerated(TestWrapper source) => 
            new TestGenerated { Id = source.Id };
    }

    public class TestTypeMapper2 : BaseTypeMapper<TestWrapper2, TestGenerated2>
    {
        protected override TestWrapper2 DoMapToWrapper(TestGenerated2 source) => 
            new TestWrapper2 { Id = source.Id };

        protected override TestGenerated2 DoMapToGenerated(TestWrapper2 source) => 
            new TestGenerated2 { Id = source.Id };
    }

    public class TestTypeMapper3 : BaseTypeMapper<TestWrapper, TestGenerated2>
    {
        protected override TestWrapper DoMapToWrapper(TestGenerated2 source) => 
            new TestWrapper { Id = source.Id };

        protected override TestGenerated2 DoMapToGenerated(TestWrapper source) => 
            new TestGenerated2 { Id = source.Id };
    }
}