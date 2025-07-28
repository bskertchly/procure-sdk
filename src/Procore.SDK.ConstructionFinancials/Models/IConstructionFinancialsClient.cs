using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.ConstructionFinancials.Models;

/// <summary>
/// Defines the contract for the ConstructionFinancials client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
/// </summary>
public interface IConstructionFinancialsClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }

    // Invoice Operations
    Task<IEnumerable<Invoice>> GetInvoicesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<Invoice> GetInvoiceAsync(int companyId, int projectId, int invoiceId, CancellationToken cancellationToken = default);
    Task<Invoice> CreateInvoiceAsync(int companyId, int projectId, CreateInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<Invoice> ApproveInvoiceAsync(int companyId, int projectId, int invoiceId, CancellationToken cancellationToken = default);
    Task<Invoice> RejectInvoiceAsync(int companyId, int projectId, int invoiceId, string reason, CancellationToken cancellationToken = default);

    // Payment Operations  
    Task<FinancialTransaction> ProcessPaymentAsync(int companyId, int projectId, ProcessPaymentRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialTransaction>> GetTransactionsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Cost Code Operations
    Task<IEnumerable<CostCode>> GetCostCodesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<CostCode> GetCostCodeAsync(int companyId, int projectId, int costCodeId, CancellationToken cancellationToken = default);

    // Financial Reporting
    Task<decimal> GetProjectTotalCostAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, decimal>> GetCostSummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<CoreModels.PagedResult<Invoice>> GetInvoicesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    Task<CoreModels.PagedResult<FinancialTransaction>> GetTransactionsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
}