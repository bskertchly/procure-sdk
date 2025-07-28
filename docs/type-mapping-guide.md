# Type Mapping Guide

## Overview

The Procore SDK includes a robust type mapping infrastructure that provides seamless conversion between wrapper domain models and generated Kiota client types. This system ensures high-performance, reliable data transformation while maintaining clean separation between the public API and generated implementation details.

## Architecture

### Core Components

1. **BaseTypeMapper<TWrapper, TGenerated>**: Abstract base class providing common functionality
2. **ITypeMapper<TWrapper, TGenerated>**: Generic interface for type mapping operations
3. **TypeMapperRegistry**: Centralized registry for mapper discovery and performance monitoring
4. **TypeMappingExtensions**: Extension methods for convenient conversions
5. **TypeMapperMetrics**: Performance tracking and validation

### Design Principles

- **Performance First**: All conversions target <1ms execution time
- **Error Safety**: Comprehensive error handling with detailed exception context
- **Metrics Driven**: Built-in performance monitoring and validation
- **Type Safety**: Strong typing throughout the mapping pipeline
- **Extensibility**: Easy to add new mappers for additional types

## Performance Requirements

All type mappers must meet these performance targets:

- **Conversion Time**: <1ms per operation average
- **Error Rate**: <1% of all conversions
- **Memory Efficiency**: Minimal allocation overhead
- **Thread Safety**: Safe for concurrent access

## Implementation Guide

### Creating a Type Mapper

```csharp
public class ProjectTypeMapper : BaseTypeMapper<Project, GeneratedProject>
{
    protected override Project DoMapToWrapper(GeneratedProject source)
    {
        return new Project
        {
            Id = source.Id ?? 0,
            Name = source.Name ?? string.Empty,
            Status = MapGeneratedStatusToWrapper(source.Flag),
            StartDate = MapDateToDateTime(source.StartDate),
            // ... other mappings
        };
    }

    protected override GeneratedProject DoMapToGenerated(Project source)
    {
        return new GeneratedProject
        {
            Id = source.Id,
            Name = source.Name,
            Flag = MapWrapperStatusToGenerated(source.Status),
            // ... other mappings
        };
    }
}
```

### Registration and Usage

```csharp
// Registration in DI container
services.AddTypeMapping();
services.AddTypeMapper<Project, GeneratedProject, ProjectTypeMapper>();

// Usage in application code
var wrapperProject = generatedProject.ToWrapper<Project, GeneratedProject>(serviceProvider);
var generatedProject = wrapperProject.ToGenerated<Project, GeneratedProject>(serviceProvider);
```

### Helper Methods

The `BaseTypeMapper` provides utility methods for common conversion scenarios:

```csharp
// Enum mapping with fallback
var status = MapEnum<GeneratedStatus, WrapperStatus>(source.Status, WrapperStatus.Default);

// Nullable enum mapping
var nullableStatus = MapNullableEnum<GeneratedStatus, WrapperStatus>(source.Status);

// DateTime conversions
var dateTime = MapDateTime(source.CreatedAt);
var nullableDateTime = MapNullableDateTime(source.UpdatedAt);
var dateOnly = MapDateToDateTime(source.StartDate);
```

## Best Practices

### 1. Performance Optimization

- Cache expensive computations
- Avoid unnecessary allocations
- Use efficient string operations
- Minimize reflection usage

```csharp
// Good: Direct property mapping
result.Name = source.Name ?? string.Empty;

// Avoid: Reflection-based mapping
var property = typeof(TTarget).GetProperty("Name");
property.SetValue(result, source.Name);
```

### 2. Error Handling

- Always validate input parameters
- Provide meaningful error context
- Use try-catch in mapper methods
- Preserve original exceptions when appropriate

```csharp
protected override Project DoMapToWrapper(GeneratedProject source)
{
    try
    {
        // Mapping logic here
        return result;
    }
    catch (Exception ex)
    {
        throw new TypeMappingException(
            $"Failed to map {typeof(GeneratedProject).Name} to {typeof(Project).Name}",
            ex,
            typeof(GeneratedProject),
            typeof(Project));
    }
}
```

### 3. Data Validation

- Check for null values before mapping
- Validate data ranges and formats
- Provide sensible defaults for missing data

```csharp
// Safe null handling
public decimal? Budget { get; set; } = source.TotalValue != null ? 
    (decimal)source.TotalValue.Value : null;

// Range validation
public int Id { get; set; } = Math.Max(0, source.Id ?? 0);
```

## Testing Strategies

### Unit Tests

```csharp
[Fact]
public void MapToWrapper_WithValidInput_ShouldMapCorrectly()
{
    // Arrange
    var mapper = new ProjectTypeMapper();
    var generatedProject = CreateTestGeneratedProject();

    // Act
    var result = mapper.MapToWrapper(generatedProject);

    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(generatedProject.Id);
    result.Name.Should().Be(generatedProject.Name);
}
```

### Performance Tests

```csharp
[Fact]
public void MapToWrapper_Performance_ShouldMeetTargets()
{
    var mapper = new ProjectTypeMapper();
    var source = CreateComplexGeneratedProject();
    const int iterations = 10000;

    var stopwatch = Stopwatch.StartNew();
    
    for (int i = 0; i < iterations; i++)
    {
        mapper.MapToWrapper(source);
    }
    
    stopwatch.Stop();
    var averageMs = (double)stopwatch.ElapsedMilliseconds / iterations;
    
    averageMs.Should().BeLessThan(1.0, "Mapping should be <1ms per operation");
}
```

### Round-Trip Tests

```csharp
[Fact]
public void RoundTripMapping_ShouldPreserveKeyData()
{
    var originalProject = CreateValidProject();
    
    var generatedProject = mapper.MapToGenerated(originalProject);
    var roundTripProject = mapper.MapToWrapper(generatedProject);
    
    roundTripProject.Id.Should().Be(originalProject.Id);
    roundTripProject.Name.Should().Be(originalProject.Name);
}
```

## Monitoring and Validation

### Performance Metrics

The system automatically tracks performance metrics for all mappers:

```csharp
// Access mapper metrics
var metrics = mapper.Metrics;
Console.WriteLine($"Average conversion time: {metrics.AverageToWrapperTimeMs}ms");
Console.WriteLine($"Error rate: {metrics.ToWrapperErrorRate:P}");

// Validate performance targets
var validation = metrics.ValidatePerformance(targetAverageMs: 1.0);
if (!validation.OverallValid)
{
    // Handle performance issues
}
```

### Registry Monitoring

```csharp
// Get all registered mappers
var allMappers = registry.GetAllMappers();

// Validate performance across all mappers
var results = registry.ValidatePerformance();
var failingMappers = results.Where(r => !r.ValidationResult.OverallValid);
```

## Common Patterns

### Enum Mapping

```csharp
private static ProjectStatus MapGeneratedStatusToWrapper(GeneratedFlag? flag)
{
    return flag switch
    {
        GeneratedFlag.Green => ProjectStatus.Active,
        GeneratedFlag.Yellow => ProjectStatus.OnHold,
        GeneratedFlag.Red => ProjectStatus.Cancelled,
        _ => ProjectStatus.Planning
    };
}
```

### Complex Object Mapping

```csharp
Company = source.CompanyId > 0 ? new GeneratedCompany 
{ 
    Id = source.CompanyId,
    Name = source.CompanyName 
} : null
```

### Collection Mapping

```csharp
public IEnumerable<WrapperItem> Items { get; set; } = 
    source.GeneratedItems?.Select(item => mapper.MapToWrapper(item)) ?? 
    Enumerable.Empty<WrapperItem>();
```

## Troubleshooting

### Common Issues

1. **Performance Problems**
   - Check for expensive operations in mapping logic
   - Verify metrics to identify bottlenecks
   - Consider caching for complex computations

2. **Type Conversion Errors**
   - Ensure nullable types are handled properly
   - Add explicit type casts where needed
   - Validate input data ranges

3. **Memory Leaks**
   - Avoid circular references in mapped objects
   - Dispose of resources properly
   - Monitor memory usage in performance tests

### Debugging Tips

- Use the `TryMap` methods for error-safe conversions
- Check mapper metrics for performance insights
- Enable detailed logging for complex mapping scenarios
- Use debugger breakpoints in mapper implementations

## Future Enhancements

- **AutoMapper Integration**: Optional integration with AutoMapper for complex scenarios
- **Code Generation**: Automatic mapper generation from type annotations
- **Schema Validation**: Runtime validation of mapping schemas
- **Performance Profiling**: Advanced profiling and optimization tools