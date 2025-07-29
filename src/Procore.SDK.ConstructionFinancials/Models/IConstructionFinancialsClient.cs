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
    
    /// <summary>
    /// Gets all invoices for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of invoices.</returns>
    Task<IEnumerable<Invoice>> GetInvoicesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific invoice by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The invoice.</returns>
    Task<Invoice> GetInvoiceAsync(int companyId, int projectId, int invoiceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The invoice creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created invoice.</returns>
    Task<Invoice> CreateInvoiceAsync(int companyId, int projectId, CreateInvoiceRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Approves an invoice.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The approved invoice.</returns>
    Task<Invoice> ApproveInvoiceAsync(int companyId, int projectId, int invoiceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Rejects an invoice with a reason.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="reason">The rejection reason.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The rejected invoice.</returns>
    Task<Invoice> RejectInvoiceAsync(int companyId, int projectId, int invoiceId, string reason, CancellationToken cancellationToken = default);

    // Payment Operations
    
    /// <summary>
    /// Processes a payment for an invoice.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The payment processing request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The financial transaction record.</returns>
    Task<FinancialTransaction> ProcessPaymentAsync(int companyId, int projectId, ProcessPaymentRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all financial transactions for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of financial transactions.</returns>
    Task<IEnumerable<FinancialTransaction>> GetTransactionsAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Cost Code Operations
    
    /// <summary>
    /// Gets all cost codes for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of cost codes.</returns>
    Task<IEnumerable<CostCode>> GetCostCodesAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific cost code by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="costCodeId">The cost code ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The cost code.</returns>
    Task<CostCode> GetCostCodeAsync(int companyId, int projectId, int costCodeId, CancellationToken cancellationToken = default);

    // Financial Reporting
    
    /// <summary>
    /// Gets the total cost for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The total project cost.</returns>
    Task<decimal> GetProjectTotalCostAsync(int companyId, int projectId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a cost summary breakdown for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with cost summary by category.</returns>
    Task<Dictionary<string, decimal>> GetCostSummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default);

    // Pagination Support
    
    /// <summary>
    /// Gets invoices with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of invoices.</returns>
    Task<CoreModels.PagedResult<Invoice>> GetInvoicesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets financial transactions with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of financial transactions.</returns>
    Task<CoreModels.PagedResult<FinancialTransaction>> GetTransactionsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);

    // Invoice Configuration Operations (V1.0)
    
    /// <summary>
    /// Gets the invoice configuration for a specific contract using V1.0 API.
    /// </summary>
    /// <param name="projectId">The project ID.</param>
    /// <param name="contractId">The contract ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The invoice configuration.</returns>
    Task<InvoiceConfiguration> GetInvoiceConfigurationAsync(int projectId, int contractId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates the invoice configuration for a specific contract using V1.0 API.
    /// </summary>
    /// <param name="projectId">The project ID.</param>
    /// <param name="contractId">The contract ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated invoice configuration.</returns>
    Task<InvoiceConfiguration> UpdateInvoiceConfigurationAsync(int projectId, int contractId, UpdateInvoiceConfigurationRequest request, CancellationToken cancellationToken = default);

    // Async Job Operations (V1.0)
    
    /// <summary>
    /// Gets the status and details of an async job using V1.0 API.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="jobUuid">The job UUID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The async job details.</returns>
    Task<AsyncJob> GetAsyncJobAsync(int companyId, string jobUuid, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Waits for an async job to complete, polling for status updates.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="jobUuid">The job UUID.</param>
    /// <param name="timeout">Maximum time to wait for completion.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The completed async job details.</returns>
    Task<AsyncJob> WaitForAsyncJobCompletionAsync(int companyId, string jobUuid, TimeSpan? timeout = null, CancellationToken cancellationToken = default);

    // Compliance Document Operations (V2.0)
    
    /// <summary>
    /// Gets compliance documents for a specific invoice with enhanced mapping using V2.0 API.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Collection of compliance documents.</returns>
    Task<IEnumerable<ComplianceDocument>> GetComplianceDocumentsAsync(int companyId, int projectId, string invoiceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a compliance document for an existing invoice using enhanced V2.0 API.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="request">The document creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created compliance document.</returns>
    Task<ComplianceDocument> CreateComplianceDocumentAsync(int companyId, int projectId, string invoiceId, CreateComplianceDocumentRequest request, CancellationToken cancellationToken = default);

    // Advanced Query Operations
    
    /// <summary>
    /// Performs advanced querying of invoices with multiple filter criteria.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The query request with filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of invoices matching the criteria.</returns>
    Task<IEnumerable<Invoice>> QueryInvoicesAsync(int companyId, int projectId, InvoiceQueryRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Performs advanced querying of invoices with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The query request with filter criteria.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of invoices matching the criteria.</returns>
    Task<CoreModels.PagedResult<Invoice>> QueryInvoicesPagedAsync(int companyId, int projectId, InvoiceQueryRequest request, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default);

    // Bulk Operations
    
    /// <summary>
    /// Processes bulk operations on multiple invoices efficiently.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The bulk operation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary indicating success/failure for each invoice ID.</returns>
    Task<Dictionary<int, bool>> ProcessBulkInvoiceOperationAsync(int companyId, int projectId, BulkInvoiceOperationRequest request, CancellationToken cancellationToken = default);

    // Financial Analytics Operations
    
    /// <summary>
    /// Gets detailed cost analysis with category breakdowns and trend analysis.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The analytics request with date range and criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with detailed cost analysis by category.</returns>
    Task<Dictionary<string, decimal>> GetDetailedCostAnalysisAsync(int companyId, int projectId, FinancialAnalyticsRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets comprehensive financial metrics including KPIs and performance indicators.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The analytics request with date range and criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with comprehensive financial metrics and KPIs.</returns>
    Task<Dictionary<string, object>> GetFinancialMetricsAsync(int companyId, int projectId, FinancialAnalyticsRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets transaction history with enhanced filtering and analysis.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The analytics request with date range and criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of financial transactions matching the criteria.</returns>
    Task<IEnumerable<FinancialTransaction>> GetTransactionHistoryAsync(int companyId, int projectId, FinancialAnalyticsRequest request, CancellationToken cancellationToken = default);
}