using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.Models;
using GeneratedUser = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse;
using GeneratedSimpleCompany = Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.Item.InstancesGetResponse_data_workflow_manager_company;

namespace Procore.SDK.Core.TypeMapping;

/// <summary>
/// Extension methods for registering Core type mappers with dependency injection.
/// Provides type mapping capabilities for common Core domain objects like User and Company.
/// </summary>
public static class CoreTypeMappingExtensions
{
    /// <summary>
    /// Registers Core type mappers with the service collection.
    /// Includes mappers for User, Company, and other common Core domain objects.
    /// </summary>
    /// <param name="services">The service collection to register mappers with</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddCoreTypeMapping(this IServiceCollection services)
    {
        // Register the type mapper registry if not already registered
        services.AddTypeMapping();

        // Register Core type mappers
        services.AddTypeMapper<User, GeneratedUser, UserTypeMapper>();
        services.AddTypeMapper<Company, GeneratedSimpleCompany, SimpleCompanyTypeMapper>();

        return services;
    }

    /// <summary>
    /// Registers an extended set of Core type mappers including specialized mappers.
    /// Use this when you need additional Core type mapping capabilities.
    /// </summary>
    /// <param name="services">The service collection to register mappers with</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddExtendedCoreTypeMapping(this IServiceCollection services)
    {
        // Register base Core type mapping
        services.AddCoreTypeMapping();

        // Future: Add additional specialized Core mappers here
        // services.AddTypeMapper<Document, GeneratedDocument, DocumentTypeMapper>();
        // services.AddTypeMapper<CustomField, GeneratedCustomField, CustomFieldTypeMapper>();

        return services;
    }
}