using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.QualitySafety.Models;

namespace Procore.SDK.QualitySafety.TypeMapping;

/// <summary>
/// Extension methods for registering QualitySafety type mappers with dependency injection.
/// </summary>
public static class QualitySafetyTypeMappingExtensions
{
    /// <summary>
    /// Registers all QualitySafety type mappers with the service collection.
    /// </summary>
    /// <param name="services">The service collection to register mappers with</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddQualitySafetyTypeMappers(this IServiceCollection services)
    {
        // Register concrete type mappers directly - they use different generated types
        services.AddSingleton<ObservationGetResponseMapper>();
        services.AddSingleton<ObservationPostResponseMapper>();
        services.AddSingleton<ObservationPatchResponseMapper>();
        services.AddSingleton<SafetyIncidentPostResponseMapper>();
        services.AddSingleton<NearMissPostResponseMapper>();
        services.AddSingleton<ObservationTypeMapper>();
        
        return services;
    }
}