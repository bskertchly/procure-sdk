using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive data integrity validation patterns for type mapping operations.
/// Ensures zero data loss during conversions and validates complex object hierarchies.
/// </summary>
public class DataIntegrityValidationTestPatterns
{
    private readonly ITestOutputHelper _output;

    public DataIntegrityValidationTestPatterns(ITestOutputHelper output)
    {
        _output = output;
    }

    #region Core Validation Patterns

    /// <summary>
    /// Validates complete round-trip data integrity for any type mapper.
    /// </summary>
    public DataIntegrityTestResult ValidateRoundTripIntegrity<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper originalWrapper,
        string testName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new DataIntegrityTestResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            WrapperType = typeof(TWrapper).Name,
            GeneratedType = typeof(TGenerated).Name,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            // Step 1: Convert wrapper → generated
            var generated = mapper.MapToGenerated(originalWrapper);
            result.ToGeneratedSuccess = generated != null;

            if (!result.ToGeneratedSuccess)
            {
                result.Issues.Add("ToGenerated conversion returned null");
                result.IsValid = false;
                return result;
            }

            // Step 2: Convert generated → wrapper (round trip)
            var roundTripWrapper = mapper.MapToWrapper(generated!);
            result.ToWrapperSuccess = roundTripWrapper != null;

            if (!result.ToWrapperSuccess)
            {
                result.Issues.Add("ToWrapper conversion returned null");
                result.IsValid = false;
                return result;
            }

            // Step 3: Deep comparison of original vs round-trip
            var comparisonResult = CompareObjects(originalWrapper, roundTripWrapper!, typeof(TWrapper));
            result.PropertyComparisons = comparisonResult.PropertyComparisons;
            result.Issues.AddRange(comparisonResult.Issues);
            result.MissingProperties = comparisonResult.MissingProperties;
            result.ModifiedProperties = comparisonResult.ModifiedProperties;

            result.IsValid = result.Issues.Count == 0;
        }
        catch (Exception ex)
        {
            result.Issues.Add($"Exception during round-trip validation: {ex.Message}");
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    /// <summary>
    /// Validates bidirectional conversion with different source objects.
    /// </summary>
    public BidirectionalValidationResult ValidateBidirectionalConversion<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperSource,
        TGenerated generatedSource,
        string testName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new BidirectionalValidationResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            // Test wrapper → generated → wrapper
            var wrapperToGenerated = mapper.MapToGenerated(wrapperSource);
            var wrapperRoundTrip = mapper.MapToWrapper(wrapperToGenerated);
            
            result.WrapperRoundTripResult = CompareObjects(wrapperSource, wrapperRoundTrip, typeof(TWrapper));

            // Test generated → wrapper → generated
            var generatedToWrapper = mapper.MapToWrapper(generatedSource);
            var generatedRoundTrip = mapper.MapToGenerated(generatedToWrapper);
            
            result.GeneratedRoundTripResult = CompareObjects(generatedSource, generatedRoundTrip, typeof(TGenerated));

            result.IsValid = result.WrapperRoundTripResult.Issues.Count == 0 && 
                           result.GeneratedRoundTripResult.Issues.Count == 0;
        }
        catch (Exception ex)
        {
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    #endregion

    #region Complex Object Validation

    /// <summary>
    /// Validates nested object hierarchies maintain their structure and data.
    /// </summary>
    public NestedObjectValidationResult ValidateNestedObjectIntegrity<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper complexWrapper,
        int maxDepth = 5,
        string testName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new NestedObjectValidationResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            MaxDepth = maxDepth,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            var generated = mapper.MapToGenerated(complexWrapper);
            var roundTrip = mapper.MapToWrapper(generated);

            result.ObjectGraph = AnalyzeObjectGraph(complexWrapper, 0, maxDepth);
            result.IntegrityValidation = ValidateNestedStructure(complexWrapper, roundTrip, 0, maxDepth);
            
            result.IsValid = result.IntegrityValidation.All(v => v.IsValid);
        }
        catch (Exception ex)
        {
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    /// <summary>
    /// Validates collection mappings preserve order, count, and element integrity.
    /// </summary>
    public CollectionValidationResult ValidateCollectionIntegrity<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperWithCollections,
        string testName = "")
        where TWrapper : class, new()  
        where TGenerated : class, new()
    {
        var result = new CollectionValidationResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            var generated = mapper.MapToGenerated(wrapperWithCollections);
            var roundTrip = mapper.MapToWrapper(generated);

            var collectionProperties = GetCollectionProperties(typeof(TWrapper));
            
            foreach (var property in collectionProperties)
            {
                var originalCollection = property.GetValue(wrapperWithCollections) as IEnumerable;
                var roundTripCollection = property.GetValue(roundTrip) as IEnumerable;

                var validation = ValidateCollection(
                    property.Name,
                    originalCollection,
                    roundTripCollection,
                    property.PropertyType);

                result.CollectionValidations.Add(validation);
            }

            result.IsValid = result.CollectionValidations.All(v => v.IsValid);
        }
        catch (Exception ex)
        {
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    #endregion

    #region Custom Field Validation

    /// <summary>
    /// Validates custom fields (Dictionary&lt;string, object&gt;) maintain their values and types.
    /// </summary>
    public CustomFieldValidationResult ValidateCustomFieldIntegrity<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperWithCustomFields,
        string customFieldPropertyName = "CustomFields",
        string testName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new CustomFieldValidationResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            CustomFieldPropertyName = customFieldPropertyName,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            var generated = mapper.MapToGenerated(wrapperWithCustomFields);
            var roundTrip = mapper.MapToWrapper(generated);

            var customFieldProperty = typeof(TWrapper).GetProperty(customFieldPropertyName);
            if (customFieldProperty == null)
            {
                result.Issues.Add($"Custom field property '{customFieldPropertyName}' not found on {typeof(TWrapper).Name}");
                result.IsValid = false;
                return result;
            }

            var originalCustomFields = customFieldProperty.GetValue(wrapperWithCustomFields) as IDictionary<string, object>;
            var roundTripCustomFields = customFieldProperty.GetValue(roundTrip) as IDictionary<string, object>;

            result.CustomFieldValidations = ValidateCustomFields(originalCustomFields, roundTripCustomFields);
            result.IsValid = result.CustomFieldValidations.All(v => v.IsValid);
        }
        catch (Exception ex)
        {
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    #endregion

    #region DateTime and Special Type Validation

    /// <summary>
    /// Validates DateTime, DateTimeOffset, and Date conversions maintain precision.
    /// </summary>
    public DateTimeValidationResult ValidateDateTimeIntegrity<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperWithDates,
        string testName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new DateTimeValidationResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            var generated = mapper.MapToGenerated(wrapperWithDates);
            var roundTrip = mapper.MapToWrapper(generated);

            var dateTimeProperties = GetDateTimeProperties(typeof(TWrapper));

            foreach (var property in dateTimeProperties)
            {
                var originalValue = property.GetValue(wrapperWithDates);
                var roundTripValue = property.GetValue(roundTrip);

                var validation = ValidateDateTime(property.Name, originalValue, roundTripValue, property.PropertyType);
                result.DateTimeValidations.Add(validation);
            }

            result.IsValid = result.DateTimeValidations.All(v => v.IsValid);
        }
        catch (Exception ex)
        {
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    /// <summary>
    /// Validates enum conversions maintain their values correctly.
    /// </summary>
    public EnumValidationResult ValidateEnumIntegrity<TWrapper, TGenerated>(
        ITypeMapper<TWrapper, TGenerated> mapper,
        TWrapper wrapperWithEnums,
        string testName = "")
        where TWrapper : class, new()
        where TGenerated : class, new()
    {
        var result = new EnumValidationResult
        {
            TestName = testName,
            MapperType = mapper.GetType().Name,
            TestTimestamp = DateTime.UtcNow
        };

        try
        {
            var generated = mapper.MapToGenerated(wrapperWithEnums);
            var roundTrip = mapper.MapToWrapper(generated);

            var enumProperties = GetEnumProperties(typeof(TWrapper));

            foreach (var property in enumProperties)
            {
                var originalValue = property.GetValue(wrapperWithEnums);
                var roundTripValue = property.GetValue(roundTrip);

                var validation = ValidateEnum(property.Name, originalValue, roundTripValue, property.PropertyType);
                result.EnumValidations.Add(validation);
            }

            result.IsValid = result.EnumValidations.All(v => v.IsValid);
        }
        catch (Exception ex)
        {
            result.Exception = ex;
            result.IsValid = false;
        }

        return result;
    }

    #endregion

    #region Validation Helper Methods

    private ObjectComparisonResult CompareObjects(object original, object result, Type objectType)
    {
        var comparisonResult = new ObjectComparisonResult();
        var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead) continue;

            var propertyComparison = new PropertyComparison
            {
                PropertyName = property.Name,
                PropertyType = property.PropertyType.Name
            };

            try
            {
                var originalValue = property.GetValue(original);
                var resultValue = property.GetValue(result);

                propertyComparison.OriginalValue = originalValue?.ToString() ?? "null";
                propertyComparison.ResultValue = resultValue?.ToString() ?? "null";

                if (AreEqual(originalValue, resultValue))
                {
                    propertyComparison.IsEqual = true;
                }
                else
                {
                    propertyComparison.IsEqual = false;
                    comparisonResult.Issues.Add($"Property {property.Name}: expected '{originalValue}', got '{resultValue}'");
                    comparisonResult.ModifiedProperties.Add(property.Name);
                }
            }
            catch (Exception ex)
            {
                propertyComparison.IsEqual = false;
                propertyComparison.Exception = ex;
                comparisonResult.Issues.Add($"Property {property.Name}: comparison failed - {ex.Message}");
            }

            comparisonResult.PropertyComparisons.Add(propertyComparison);
        }

        return comparisonResult;
    }

    private static bool AreEqual(object? original, object? result)
    {
        if (original == null && result == null) return true;
        if (original == null || result == null) return false;

        // Handle special cases
        if (original is DateTime originalDateTime && result is DateTime resultDateTime)
        {
            // Allow for small differences in DateTime precision
            return Math.Abs((originalDateTime - resultDateTime).TotalMilliseconds) < 1;
        }

        if (original is IDictionary originalDict && result is IDictionary resultDict)
        {
            return CompareDictionaries(originalDict, resultDict);
        }

        if (original is IEnumerable originalEnum && result is IEnumerable resultEnum &&
            !(original is string) && !(result is string))
        {
            return CompareEnumerables(originalEnum, resultEnum);
        }

        return original.Equals(result);
    }

    private static bool CompareDictionaries(IDictionary original, IDictionary result)
    {
        if (original.Count != result.Count) return false;

        foreach (var key in original.Keys)
        {
            if (!result.Contains(key)) return false;
            if (!AreEqual(original[key], result[key])) return false;
        }

        return true;
    }

    private static bool CompareEnumerables(IEnumerable original, IEnumerable result)
    {
        var originalList = original.Cast<object>().ToList();
        var resultList = result.Cast<object>().ToList();

        if (originalList.Count != resultList.Count) return false;

        return originalList.Zip(resultList, AreEqual).All(isEqual => isEqual);
    }

    private List<ObjectGraphNode> AnalyzeObjectGraph(object obj, int currentDepth, int maxDepth)
    {
        var nodes = new List<ObjectGraphNode>();
        
        if (obj == null || currentDepth >= maxDepth) return nodes;

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead) continue;

            var node = new ObjectGraphNode
            {
                PropertyName = property.Name,
                PropertyType = property.PropertyType.Name,
                Depth = currentDepth,
                IsComplex = IsComplexType(property.PropertyType)
            };

            try
            {
                var value = property.GetValue(obj);
                node.HasValue = value != null;

                if (node.IsComplex && value != null && currentDepth < maxDepth - 1)
                {
                    node.Children = AnalyzeObjectGraph(value, currentDepth + 1, maxDepth);
                }
            }
            catch (Exception ex)
            {
                node.Exception = ex;
            }

            nodes.Add(node);
        }

        return nodes;
    }

    private List<PropertyValidation> ValidateNestedStructure(object original, object result, int currentDepth, int maxDepth)
    {
        var validations = new List<PropertyValidation>();

        if (original == null || result == null || currentDepth >= maxDepth) 
            return validations;

        var properties = original.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead) continue;

            var validation = new PropertyValidation
            {
                PropertyName = property.Name,
                Depth = currentDepth
            };

            try
            {
                var originalValue = property.GetValue(original);
                var resultValue = property.GetValue(result);

                validation.IsValid = AreEqual(originalValue, resultValue);

                if (!validation.IsValid)
                {
                    validation.Issue = $"Nested property {property.Name} at depth {currentDepth} does not match";
                }

                // Recurse into complex types
                if (IsComplexType(property.PropertyType) && originalValue != null && resultValue != null)
                {
                    var nestedValidations = ValidateNestedStructure(originalValue, resultValue, currentDepth + 1, maxDepth);
                    validation.NestedValidations = nestedValidations;
                    
                    if (nestedValidations.Any(v => !v.IsValid))
                    {
                        validation.IsValid = false;
                        validation.Issue += " (nested validation failures)";
                    }
                }
            }
            catch (Exception ex)
            {
                validation.IsValid = false;
                validation.Issue = ex.Message;
            }

            validations.Add(validation);
        }

        return validations;
    }

    private CollectionValidation ValidateCollection(string propertyName, IEnumerable? original, IEnumerable? result, Type collectionType)
    {
        var validation = new CollectionValidation
        {
            PropertyName = propertyName,
            CollectionType = collectionType.Name
        };

        if (original == null && result == null)
        {
            validation.IsValid = true;
            return validation;
        }

        if (original == null || result == null)
        {
            validation.IsValid = false;
            validation.Issue = "One collection is null while the other is not";
            return validation;
        }

        var originalList = original.Cast<object>().ToList();
        var resultList = result.Cast<object>().ToList();

        validation.OriginalCount = originalList.Count;
        validation.ResultCount = resultList.Count;

        if (originalList.Count != resultList.Count)
        {
            validation.IsValid = false;
            validation.Issue = $"Collection count mismatch: expected {originalList.Count}, got {resultList.Count}";
            return validation;
        }

        // Validate elements
        for (int i = 0; i < originalList.Count; i++)
        {
            if (!AreEqual(originalList[i], resultList[i]))
            {
                validation.IsValid = false;
                validation.Issue = $"Element at index {i} does not match";
                validation.MismatchedElements.Add(i);
            }
        }

        validation.IsValid = validation.MismatchedElements.Count == 0;
        return validation;
    }

    private List<CustomFieldValidation> ValidateCustomFields(IDictionary<string, object>? original, IDictionary<string, object>? result)
    {
        var validations = new List<CustomFieldValidation>();

        if (original == null && result == null) return validations;

        if (original == null || result == null)
        {
            validations.Add(new CustomFieldValidation
            {
                FieldName = "CustomFields",
                IsValid = false,
                Issue = "One custom field dictionary is null while the other is not"
            });
            return validations;
        }

        // Check all original fields
        foreach (var kvp in original)
        {
            var validation = new CustomFieldValidation { FieldName = kvp.Key };

            if (!result.ContainsKey(kvp.Key))
            {
                validation.IsValid = false;
                validation.Issue = "Field missing in result";
            }
            else if (!AreEqual(kvp.Value, result[kvp.Key]))
            {
                validation.IsValid = false;
                validation.Issue = $"Value mismatch: expected '{kvp.Value}', got '{result[kvp.Key]}'";
                validation.OriginalValue = kvp.Value?.ToString() ?? "null";
                validation.ResultValue = result[kvp.Key]?.ToString() ?? "null";
            }
            else
            {
                validation.IsValid = true;
                validation.OriginalValue = kvp.Value?.ToString() ?? "null";
                validation.ResultValue = result[kvp.Key]?.ToString() ?? "null";
            }

            validations.Add(validation);
        }

        // Check for extra fields in result
        foreach (var kvp in result)
        {
            if (!original.ContainsKey(kvp.Key))
            {
                validations.Add(new CustomFieldValidation
                {
                    FieldName = kvp.Key,
                    IsValid = false,
                    Issue = "Extra field in result not present in original",
                    ResultValue = kvp.Value?.ToString() ?? "null"
                });
            }
        }

        return validations;
    }

    private DateTimeValidation ValidateDateTime(string propertyName, object? originalValue, object? resultValue, Type propertyType)
    {
        var validation = new DateTimeValidation
        {
            PropertyName = propertyName,
            PropertyType = propertyType.Name
        };

        try
        {
            if (originalValue == null && resultValue == null)
            {
                validation.IsValid = true;
                return validation;
            }

            if (originalValue == null || resultValue == null)
            {
                validation.IsValid = false;
                validation.Issue = "One DateTime value is null while the other is not";
                return validation;
            }

            // Handle different DateTime types
            DateTime? originalDateTime = ConvertToDateTime(originalValue);
            DateTime? resultDateTime = ConvertToDateTime(resultValue);

            if (originalDateTime == null || resultDateTime == null)
            {
                validation.IsValid = false;
                validation.Issue = "Could not convert values to DateTime for comparison";
                return validation;
            }

            // Allow for small precision differences (up to 1 second)
            var difference = Math.Abs((originalDateTime.Value - resultDateTime.Value).TotalSeconds);
            validation.IsValid = difference <= 1.0;
            validation.DifferenceSeconds = difference;

            if (!validation.IsValid)
            {
                validation.Issue = $"DateTime difference ({difference:F3} seconds) exceeds tolerance";
            }

            validation.OriginalValue = originalDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            validation.ResultValue = resultDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        catch (Exception ex)
        {
            validation.IsValid = false;
            validation.Issue = ex.Message;
        }

        return validation;
    }

    private EnumValidation ValidateEnum(string propertyName, object? originalValue, object? resultValue, Type propertyType)
    {
        var validation = new EnumValidation
        {
            PropertyName = propertyName,
            PropertyType = propertyType.Name
        };

        try
        {
            if (originalValue == null && resultValue == null)
            {
                validation.IsValid = true;
                return validation;
            }

            if (originalValue == null || resultValue == null)
            {
                validation.IsValid = false;
                validation.Issue = "One enum value is null while the other is not";
                return validation;
            }

            validation.OriginalValue = originalValue.ToString() ?? "null";
            validation.ResultValue = resultValue.ToString() ?? "null";
            validation.IsValid = originalValue.Equals(resultValue);

            if (!validation.IsValid)
            {
                validation.Issue = $"Enum values do not match: expected '{validation.OriginalValue}', got '{validation.ResultValue}'";
            }
        }
        catch (Exception ex)
        {
            validation.IsValid = false;
            validation.Issue = ex.Message;
        }

        return validation;
    }

    private static DateTime? ConvertToDateTime(object value)
    {
        return value switch
        {
            DateTime dt => dt,
            DateTimeOffset dto => dto.DateTime,
            string str when DateTime.TryParse(str, out var parsed) => parsed,
            _ => null
        };
    }

    private static bool IsComplexType(Type type)
    {
        return !type.IsPrimitive && 
               type != typeof(string) && 
               type != typeof(DateTime) && 
               type != typeof(DateTimeOffset) &&
               type != typeof(decimal) &&
               !type.IsEnum &&
               (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>));
    }

    private static List<PropertyInfo> GetCollectionProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType != typeof(string) && 
                       typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
            .ToList();
    }

    private static List<PropertyInfo> GetDateTimeProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(DateTime) || 
                       p.PropertyType == typeof(DateTime?) ||
                       p.PropertyType == typeof(DateTimeOffset) ||
                       p.PropertyType == typeof(DateTimeOffset?))
            .ToList();
    }

    private static List<PropertyInfo> GetEnumProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType.IsEnum || 
                       (p.PropertyType.IsGenericType && 
                        p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        p.PropertyType.GetGenericArguments()[0].IsEnum))
            .ToList();
    }

    #endregion
}

#region Result Classes

public class DataIntegrityTestResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public string WrapperType { get; set; } = string.Empty;
    public string GeneratedType { get; set; } = string.Empty;
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; } = true;
    public bool ToGeneratedSuccess { get; set; }
    public bool ToWrapperSuccess { get; set; }
    public List<PropertyComparison> PropertyComparisons { get; set; } = new();
    public List<string> Issues { get; set; } = new();
    public List<string> MissingProperties { get; set; } = new();
    public List<string> ModifiedProperties { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class BidirectionalValidationResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; }
    public ObjectComparisonResult WrapperRoundTripResult { get; set; } = new();
    public ObjectComparisonResult GeneratedRoundTripResult { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class NestedObjectValidationResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public int MaxDepth { get; set; }
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; }
    public List<ObjectGraphNode> ObjectGraph { get; set; } = new();
    public List<PropertyValidation> IntegrityValidation { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class CollectionValidationResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; }
    public List<CollectionValidation> CollectionValidations { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class CustomFieldValidationResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public string CustomFieldPropertyName { get; set; } = string.Empty;
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; }
    public List<CustomFieldValidation> CustomFieldValidations { get; set; } = new();
    public List<string> Issues { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class DateTimeValidationResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; }
    public List<DateTimeValidation> DateTimeValidations { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class EnumValidationResult
{
    public string TestName { get; set; } = string.Empty;
    public string MapperType { get; set; } = string.Empty;
    public DateTime TestTimestamp { get; set; }
    public bool IsValid { get; set; }
    public List<EnumValidation> EnumValidations { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class ObjectComparisonResult
{
    public List<PropertyComparison> PropertyComparisons { get; set; } = new();
    public List<string> Issues { get; set; } = new();
    public List<string> MissingProperties { get; set; } = new();
    public List<string> ModifiedProperties { get; set; } = new();
}

public class PropertyComparison
{
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string OriginalValue { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
    public bool IsEqual { get; set; }
    public Exception? Exception { get; set; }
}

public class ObjectGraphNode
{
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public int Depth { get; set; }
    public bool IsComplex { get; set; }
    public bool HasValue { get; set; }
    public List<ObjectGraphNode> Children { get; set; } = new();
    public Exception? Exception { get; set; }
}

public class PropertyValidation
{
    public string PropertyName { get; set; } = string.Empty;
    public int Depth { get; set; }
    public bool IsValid { get; set; }
    public string Issue { get; set; } = string.Empty;
    public List<PropertyValidation> NestedValidations { get; set; } = new();
}

public class CollectionValidation
{
    public string PropertyName { get; set; } = string.Empty;
    public string CollectionType { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string Issue { get; set; } = string.Empty;
    public int OriginalCount { get; set; }
    public int ResultCount { get; set; }
    public List<int> MismatchedElements { get; set; } = new();
}

public class CustomFieldValidation
{
    public string FieldName { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string Issue { get; set; } = string.Empty;
    public string OriginalValue { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
}

public class DateTimeValidation
{
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string Issue { get; set; } = string.Empty;
    public string OriginalValue { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
    public double DifferenceSeconds { get; set; }
}

public class EnumValidation
{
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string Issue { get; set; } = string.Empty;
    public string OriginalValue { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
}

#endregion