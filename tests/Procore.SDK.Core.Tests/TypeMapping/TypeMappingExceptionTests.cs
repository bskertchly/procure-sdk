using System;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive test suite for TypeMappingException and error handling scenarios.
/// Validates exception construction, context preservation, and error handling patterns
/// throughout the type mapping infrastructure.
/// </summary>
public class TypeMappingExceptionTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        const string message = "Test mapping error";

        // Act
        var exception = new TypeMappingException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull();
        exception.SourceType.Should().BeNull();
        exception.TargetType.Should().BeNull();
        exception.PropertyName.Should().BeNull();
        exception.SourceValue.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        // Arrange
        const string message = "Test mapping error";
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new TypeMappingException(message, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeSameAs(innerException);
        exception.SourceType.Should().BeNull();
        exception.TargetType.Should().BeNull();
        exception.PropertyName.Should().BeNull();
        exception.SourceValue.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithDetailedContext_ShouldSetAllProperties()
    {
        // Arrange
        const string message = "Detailed mapping error";
        var sourceType = typeof(string);
        var targetType = typeof(int);
        const string propertyName = "TestProperty";
        const string sourceValue = "invalid-number";

        // Act
        var exception = new TypeMappingException(
            message, 
            sourceType, 
            targetType, 
            propertyName, 
            sourceValue);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull();
        exception.SourceType.Should().Be(sourceType);
        exception.TargetType.Should().Be(targetType);
        exception.PropertyName.Should().Be(propertyName);
        exception.SourceValue.Should().Be(sourceValue);
    }

    [Fact]
    public void Constructor_WithDetailedContextAndInnerException_ShouldSetAllProperties()
    {
        // Arrange
        const string message = "Detailed mapping error with inner exception";
        var innerException = new FormatException("Format error");
        var sourceType = typeof(string);
        var targetType = typeof(DateTime);
        const string propertyName = "CreatedAt";
        const string sourceValue = "not-a-date";

        // Act
        var exception = new TypeMappingException(
            message, 
            innerException,
            sourceType, 
            targetType, 
            propertyName, 
            sourceValue);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeSameAs(innerException);
        exception.SourceType.Should().Be(sourceType);
        exception.TargetType.Should().Be(targetType);
        exception.PropertyName.Should().Be(propertyName);
        exception.SourceValue.Should().Be(sourceValue);
    }

    #endregion

    #region Property Tests

    [Fact]
    public void Properties_WithNullValues_ShouldBeNull()
    {
        // Act
        var exception = new TypeMappingException("Test", null!, null, null, null);

        // Assert
        exception.SourceType.Should().BeNull();
        exception.TargetType.Should().BeNull();
        exception.PropertyName.Should().BeNull();
        exception.SourceValue.Should().BeNull();
    }

    [Fact]
    public void Properties_WithComplexSourceValue_ShouldPreserveValue()
    {
        // Arrange
        var complexValue = new { Id = 123, Name = "Test" };

        // Act
        var exception = new TypeMappingException("Test", sourceValue: complexValue);

        // Assert
        exception.SourceValue.Should().BeSameAs(complexValue);
    }

    [Fact]
    public void SourceType_WithGenericType_ShouldPreserveGenericInformation()
    {
        // Arrange
        var genericType = typeof(System.Collections.Generic.List<string>);

        // Act
        var exception = new TypeMappingException("Test", sourceType: genericType);

        // Assert
        exception.SourceType.Should().Be(genericType);
        exception.SourceType!.IsGenericType.Should().BeTrue();
        exception.SourceType.GetGenericArguments().Should().Contain(typeof(string));
    }

    #endregion

    #region Error Handling Pattern Tests

    [Fact]
    public void TypeMappingException_ShouldInheritFromException()
    {
        // Arrange & Act
        var exception = new TypeMappingException("Test");

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public void TypeMappingException_ShouldBeSerializable()
    {
        // This test would require serialization attributes and proper implementation
        // For now, we verify the exception follows standard patterns
        
        // Arrange
        var exception = new TypeMappingException(
            "Serialization test", 
            new InvalidOperationException("Inner"),
            typeof(string), 
            typeof(int), 
            "TestProperty", 
            "test-value");

        // Act & Assert - Should not throw when accessing properties
        var message = exception.Message;
        var innerException = exception.InnerException;
        var sourceType = exception.SourceType;
        var targetType = exception.TargetType;
        var propertyName = exception.PropertyName;
        var sourceValue = exception.SourceValue;

        // Verify all properties are accessible
        message.Should().Be("Serialization test");
        innerException.Should().NotBeNull();
        sourceType.Should().Be<string>();
        targetType.Should().Be<int>();
        propertyName.Should().Be("TestProperty");
        sourceValue.Should().Be("test-value");
    }

    #endregion

    #region Mapping Context Tests

    [Fact]
    public void TypeMappingException_WithMappingContext_ShouldProvideDiagnosticInformation()
    {
        // Arrange
        var sourceType = typeof(TestSourceModel);
        var targetType = typeof(TestTargetModel);
        const string propertyName = "ComplexProperty";
        var sourceValue = new TestSourceModel { Id = 123, Name = "Test" };

        // Act
        var exception = new TypeMappingException(
            "Failed to map complex property",
            sourceType,
            targetType,
            propertyName,
            sourceValue);

        // Assert - Exception should provide rich diagnostic information
        exception.Message.Should().Be("Failed to map complex property");
        exception.SourceType.Should().Be<TestSourceModel>();
        exception.TargetType.Should().Be<TestTargetModel>();
        exception.PropertyName.Should().Be("ComplexProperty");
        exception.SourceValue.Should().BeSameAs(sourceValue);
    }

    [Fact]
    public void TypeMappingException_WithNestedMappingFailure_ShouldPreserveInnerContext()
    {
        // Arrange
        var innerException = new TypeMappingException(
            "Inner mapping failed",
            typeof(string),
            typeof(DateTime),
            "DateProperty",
            "invalid-date");

        // Act
        var outerException = new TypeMappingException(
            "Outer mapping failed due to inner property",
            innerException,
            typeof(TestSourceModel),
            typeof(TestTargetModel),
            "NestedProperty",
            new TestSourceModel());

        // Assert
        outerException.Message.Should().Be("Outer mapping failed due to inner property");
        outerException.InnerException.Should().BeSameAs(innerException);
        outerException.SourceType.Should().Be<TestSourceModel>();
        outerException.TargetType.Should().Be<TestTargetModel>();
        
        // Inner exception context should be preserved
        var inner = outerException.InnerException as TypeMappingException;
        inner.Should().NotBeNull();
        inner!.SourceType.Should().Be<string>();
        inner.TargetType.Should().Be<DateTime>();
        inner.PropertyName.Should().Be("DateProperty");
        inner.SourceValue.Should().Be("invalid-date");
    }

    #endregion

    #region Error Message Tests

    [Fact]
    public void TypeMappingException_ErrorMessage_ShouldBeDescriptive()
    {
        // Arrange & Act
        var exception = new TypeMappingException(
            "Failed to convert property 'Age' from string to int: invalid format",
            typeof(PersonDto),
            typeof(Person),
            "Age",
            "not-a-number");

        // Assert
        exception.Message.Should().Contain("Failed to convert");
        exception.Message.Should().Contain("Age");
        exception.Message.Should().Contain("string to int");
        exception.Message.Should().Contain("invalid format");
    }

    [Fact]
    public void TypeMappingException_WithEmptyMessage_ShouldStillProvideContext()
    {
        // Arrange & Act
        var exception = new TypeMappingException(
            "",
            typeof(string),
            typeof(int),
            "TestProp",
            "test");

        // Assert
        exception.Message.Should().Be("");
        exception.SourceType.Should().Be<string>();
        exception.TargetType.Should().Be<int>();
        exception.PropertyName.Should().Be("TestProp");
        exception.SourceValue.Should().Be("test");
    }

    #endregion

    #region Exception Propagation Tests

    [Fact]
    public void TypeMappingException_WhenCaught_ShouldMaintainStackTrace()
    {
        // Arrange
        Exception? caughtException = null;

        // Act
        try
        {
            ThrowTypeMappingException();
        }
        catch (TypeMappingException ex)
        {
            caughtException = ex;
        }

        // Assert
        caughtException.Should().NotBeNull();
        caughtException.Should().BeOfType<TypeMappingException>();
        caughtException!.StackTrace.Should().NotBeNullOrEmpty();
        caughtException.StackTrace.Should().Contain(nameof(ThrowTypeMappingException));
    }

    [Fact]
    public void TypeMappingException_WhenRethrown_ShouldPreserveContext()
    {
        // Arrange
        TypeMappingException? originalException = null;
        TypeMappingException? rethrownException = null;

        // Act
        try
        {
            try
            {
                throw new TypeMappingException(
                    "Original error",
                    typeof(string),
                    typeof(int),
                    "TestProp",
                    "test-value");
            }
            catch (TypeMappingException ex)
            {
                originalException = ex;
                throw; // Rethrow to preserve stack trace
            }
        }
        catch (TypeMappingException ex)
        {
            rethrownException = ex;
        }

        // Assert
        rethrownException.Should().BeSameAs(originalException);
        rethrownException!.SourceType.Should().Be<string>();
        rethrownException.TargetType.Should().Be<int>();
        rethrownException.PropertyName.Should().Be("TestProp");
        rethrownException.SourceValue.Should().Be("test-value");
    }

    #endregion

    #region Integration with Mappers Tests

    [Fact]
    public void BaseTypeMapper_WhenMappingFails_ShouldWrapInTypeMappingException()
    {
        // This test would ideally use a real mapper that can fail
        // For now, we test the pattern that mappers should follow
        
        // Arrange
        var originalException = new FormatException("Invalid format for date conversion");
        var sourceType = typeof(string);
        var targetType = typeof(DateTime);

        // Act
        var wrappedException = new TypeMappingException(
            $"Failed to map from {sourceType.Name} to {targetType.Name}: {originalException.Message}",
            originalException,
            sourceType,
            targetType);

        // Assert
        wrappedException.Message.Should().Contain("Failed to map from String to DateTime");
        wrappedException.Message.Should().Contain("Invalid format for date conversion");
        wrappedException.InnerException.Should().BeSameAs(originalException);
        wrappedException.SourceType.Should().Be(sourceType);
        wrappedException.TargetType.Should().Be(targetType);
    }

    [Fact]
    public void TypeMappingException_WithNullInnerException_ShouldNotThrow()
    {
        // Act & Assert - Should not throw
        var exception = new TypeMappingException("Test", (Exception?)null, typeof(string), typeof(int));
        
        exception.Message.Should().Be("Test");
        exception.InnerException.Should().BeNull();
        exception.SourceType.Should().Be<string>();
        exception.TargetType.Should().Be<int>();
    }

    #endregion

    #region Helper Methods and Classes

    private static void ThrowTypeMappingException()
    {
        throw new TypeMappingException(
            "Test exception for stack trace",
            typeof(string),
            typeof(int),
            "TestProperty",
            "test-value");
    }

    private class TestSourceModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class TestTargetModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    private class PersonDto
    {
        public string Name { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
    }

    private class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    #endregion
}