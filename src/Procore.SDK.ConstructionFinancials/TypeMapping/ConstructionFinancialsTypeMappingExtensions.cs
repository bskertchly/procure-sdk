using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using GeneratedDocumentResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;

namespace Procore.SDK.ConstructionFinancials.TypeMapping;

/// <summary>
/// Extension methods for registering ConstructionFinancials type mappers with dependency injection.
/// Provides type mapping capabilities for ConstructionFinancials domain objects.
/// </summary>
public static class ConstructionFinancialsTypeMappingExtensions
{
    /// <summary>
    /// Registers ConstructionFinancials type mappers with the service collection.
    /// Includes mappers for Invoice, CostCode, and other ConstructionFinancials domain objects.
    /// </summary>
    /// <param name="services">The service collection to register mappers with</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddConstructionFinancialsTypeMapping(this IServiceCollection services)
    {
        // Register the Core type mapping infrastructure
        services.AddCoreTypeMapping();

        // Register ConstructionFinancials-specific type mappers
        services.AddTypeMapper<Invoice, GeneratedDocumentResponse, InvoiceTypeMapper>();

        // Future mappers can be added here:
        // services.AddTypeMapper<CostCode, GeneratedCostCode, CostCodeTypeMapper>();
        // services.AddTypeMapper<FinancialTransaction, GeneratedTransaction, FinancialTransactionTypeMapper>();

        return services;
    }
}