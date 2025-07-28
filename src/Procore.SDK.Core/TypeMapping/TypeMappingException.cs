using System;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Exception thrown when type mapping operations fail due to data incompatibility or validation errors.
/// </summary>
public class TypeMappingException : Exception
{
    /// <summary>
    /// Gets the source type that was being mapped from.
    /// </summary>
    public Type? SourceType { get; }

    /// <summary>
    /// Gets the target type that was being mapped to.
    /// </summary>
    public Type? TargetType { get; }

    /// <summary>
    /// Gets the property name where the mapping failed, if applicable.
    /// </summary>
    public string? PropertyName { get; }

    /// <summary>
    /// Gets the source value that caused the mapping failure, if applicable.
    /// </summary>
    public object? SourceValue { get; }

    /// <summary>
    /// Initializes a new instance of the TypeMappingException class.
    /// </summary>
    /// <param name="message">The error message</param>
    public TypeMappingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the TypeMappingException class.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public TypeMappingException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the TypeMappingException class with detailed mapping context.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="sourceType">The source type being mapped from</param>
    /// <param name="targetType">The target type being mapped to</param>
    /// <param name="propertyName">The property name where mapping failed</param>
    /// <param name="sourceValue">The source value that caused the failure</param>
    public TypeMappingException(
        string message, 
        Type? sourceType = null, 
        Type? targetType = null, 
        string? propertyName = null, 
        object? sourceValue = null) : base(message)
    {
        SourceType = sourceType;
        TargetType = targetType;
        PropertyName = propertyName;
        SourceValue = sourceValue;
    }

    /// <summary>
    /// Initializes a new instance of the TypeMappingException class with detailed mapping context and inner exception.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    /// <param name="sourceType">The source type being mapped from</param>
    /// <param name="targetType">The target type being mapped to</param>
    /// <param name="propertyName">The property name where mapping failed</param>
    /// <param name="sourceValue">The source value that caused the failure</param>
    public TypeMappingException(
        string message, 
        Exception innerException,
        Type? sourceType = null, 
        Type? targetType = null, 
        string? propertyName = null, 
        object? sourceValue = null) : base(message, innerException)
    {
        SourceType = sourceType;
        TargetType = targetType;
        PropertyName = propertyName;
        SourceValue = sourceValue;
    }
}