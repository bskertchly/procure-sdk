using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ResourceManagement.Models;
using Generated = Procore.SDK.ResourceManagement.Rest.V11.Projects.Item.Schedule.Resources.Item;

namespace Procore.SDK.ResourceManagement.TypeMapping;

/// <summary>
/// Extension methods for registering ResourceManagement type mappers with dependency injection.
/// </summary>
public static class ResourceManagementTypeMappingExtensions
{
    /// <summary>
    /// Registers all ResourceManagement type mappers with the service collection.
    /// </summary>
    /// <param name="services">The service collection to register mappers with</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddResourceManagementTypeMappers(this IServiceCollection services)
    {
        services.AddSingleton<ITypeMapper<Resource, Generated.ResourcesGetResponse>, ResourceTypeMapper>();
        services.AddSingleton<ResourceTypeMapper>();
        
        return services;
    }
}