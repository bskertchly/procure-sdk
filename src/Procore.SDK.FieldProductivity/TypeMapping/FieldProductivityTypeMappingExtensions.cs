using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.FieldProductivity.Models;
using GeneratedTimecardEntryResponse = Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timecard_entries.Item.Timecard_entriesGetResponse;

namespace Procore.SDK.FieldProductivity.TypeMapping;

/// <summary>
/// Extension methods for registering FieldProductivity type mapping services.
/// </summary>
public static class FieldProductivityTypeMappingExtensions
{
    /// <summary>
    /// Adds FieldProductivity type mapping services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddFieldProductivityTypeMapping(this IServiceCollection services)
    {
        // Register type mappers
        services.TryAddSingleton<ITypeMapper<ProductivityReport, GeneratedTimecardEntryResponse>, TimecardEntryTypeMapper>();
        
        return services;
    }

    /// <summary>
    /// Adds FieldProductivity type mapping services with custom type mapper configurations.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configureTimecardMapper">Configuration action for the timecard entry type mapper.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddFieldProductivityTypeMapping(
        this IServiceCollection services,
        Action<TimecardEntryTypeMapper>? configureTimecardMapper = null)
    {
        // Register custom type mappers
        services.TryAddSingleton<ITypeMapper<ProductivityReport, GeneratedTimecardEntryResponse>>(provider =>
        {
            var mapper = new TimecardEntryTypeMapper();
            configureTimecardMapper?.Invoke(mapper);
            return mapper;
        });
        
        return services;
    }

    /// <summary>
    /// Creates a default TimecardEntryTypeMapper instance for field productivity operations.
    /// </summary>
    /// <returns>A configured TimecardEntryTypeMapper instance.</returns>
    public static TimecardEntryTypeMapper CreateDefaultTimecardMapper()
    {
        return new TimecardEntryTypeMapper();
    }

    /// <summary>
    /// Validates that all required type mappers are properly registered.
    /// </summary>
    /// <param name="services">The service collection to validate.</param>
    /// <returns>True if all required type mappers are registered.</returns>
    public static bool ValidateFieldProductivityTypeMappingRegistration(this IServiceCollection services)
    {
        var requiredTypes = new[]
        {
            typeof(ITypeMapper<ProductivityReport, GeneratedTimecardEntryResponse>)
        };

        return requiredTypes.All(type => services.Any(service => service.ServiceType == type));
    }
}