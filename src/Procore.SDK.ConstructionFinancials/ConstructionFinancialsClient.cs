using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.ErrorHandling;
using Procore.SDK.Core.Logging;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using Procore.SDK.ConstructionFinancials.TypeMapping;
using CoreModels = Procore.SDK.Core.Models;
using GeneratedDocumentResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;

namespace Procore.SDK.ConstructionFinancials;

/// <summary>
/// Implementation of the ConstructionFinancials client wrapper that provides domain-specific 
/// convenience methods over the generated Kiota client.
/// </summary>
public class ProcoreConstructionFinancialsClient : IConstructionFinancialsClient
{
    private readonly Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient _generatedClient;
    private readonly ILogger<ProcoreConstructionFinancialsClient>? _logger;
    private readonly ErrorMapper? _errorMapper;
    private readonly StructuredLogger? _structuredLogger;
    private readonly ITypeMapper<Invoice, GeneratedDocumentResponse>? _invoiceMapper;
    private bool _disposed;

    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    public object RawClient => _generatedClient;

    /// <summary>
    /// Initializes a new instance of the ProcoreConstructionFinancialsClient.
    /// </summary>
    /// <param name="requestAdapter">The request adapter to use for HTTP communication.</param>
    /// <param name="logger">Optional logger for diagnostic information.</param>
    /// <param name="errorMapper">Optional error mapper for exception handling.</param>
    /// <param name="structuredLogger">Optional structured logger for correlation tracking.</param>
    /// <param name="invoiceMapper">Optional type mapper for invoice conversion.</param>
    public ProcoreConstructionFinancialsClient(
        IRequestAdapter requestAdapter, 
        ILogger<ProcoreConstructionFinancialsClient>? logger = null,
        ErrorMapper? errorMapper = null,
        StructuredLogger? structuredLogger = null,
        ITypeMapper<Invoice, GeneratedDocumentResponse>? invoiceMapper = null)
    {
        _generatedClient = new Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient(requestAdapter);
        _logger = logger;
        _errorMapper = errorMapper;
        _structuredLogger = structuredLogger;
        _invoiceMapper = invoiceMapper;
    }

    #region Private Helper Methods

    /// <summary>
    /// Executes an operation with proper error handling and logging.
    /// </summary>
    private async Task<T> ExecuteWithResilienceAsync<T>(
        Func<Task<T>> operation,
        string operationName,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        correlationId ??= Guid.NewGuid().ToString();
        
        using var operationScope = _structuredLogger?.BeginOperation(operationName, correlationId);
        
        try
        {
            _logger?.LogDebug("Executing operation {Operation} with correlation ID {CorrelationId}", operationName, correlationId);
            
            return await operation().ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            var mappedException = _errorMapper?.MapHttpException(ex, correlationId) ?? 
                new CoreModels.ProcoreCoreException(ex.Message, "HTTP_ERROR", null, correlationId);
            
            _structuredLogger?.LogError(mappedException, operationName, correlationId, 
                "HTTP error in operation {Operation}", operationName);
            
            throw mappedException;
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            _structuredLogger?.LogWarning(operationName, correlationId,
                "Operation {Operation} was cancelled", operationName);
            throw;
        }
        catch (Exception ex)
        {
            var wrappedException = new CoreModels.ProcoreCoreException(
                $"Unexpected error in {operationName}: {ex.Message}", 
                "UNEXPECTED_ERROR", 
                null, 
                correlationId);
            
            _structuredLogger?.LogError(wrappedException, operationName, correlationId,
                "Unexpected error in operation {Operation}", operationName);
            
            throw wrappedException;
        }
    }

    /// <summary>
    /// Executes an operation with proper error handling and logging (void return).
    /// </summary>
    private async Task ExecuteWithResilienceAsync(
        Func<Task> operation,
        string operationName,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        await ExecuteWithResilienceAsync(async () =>
        {
            await operation();
            return true; // Return a dummy value
        }, operationName, correlationId, cancellationToken);
    }

    #endregion

    #region Invoice Operations

    /// <summary>
    /// Gets all invoices for a project.
    /// Note: Currently returns compliance invoice documents from V2.0 API.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of invoices.</returns>
    public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting invoices for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Note: This implementation uses compliance documents as a proxy for invoices
            // The actual invoice endpoints may require different API access or versions
            var invoices = new List<Invoice>();
            
            // Since we need an invoice ID to get documents, this method currently returns empty
            // In a real implementation, you would first need to get the list of invoices
            // from another endpoint or data source, then iterate through them
            _logger?.LogWarning("GetInvoicesAsync currently returns empty - requires invoice IDs to query compliance documents");
            
            return invoices;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get invoices for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific invoice by ID.
    /// Note: Currently gets compliance document data for the invoice from V2.0 API.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The invoice.</returns>
    public async Task<Invoice> GetInvoiceAsync(int companyId, int projectId, int invoiceId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(async () =>
        {
            _logger?.LogDebug("Getting invoice {InvoiceId} for project {ProjectId} in company {CompanyId}", invoiceId, projectId, companyId);
            
            // Get compliance documents for this invoice using V2.0 API
            var documentsResponse = await _generatedClient.Rest.V20.Companies[companyId.ToString()]
                .Projects[projectId.ToString()].Compliance.Invoices[invoiceId.ToString()].Documents
                .GetAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            // Use type mapper if available, otherwise fall back to manual mapping
            if (_invoiceMapper != null)
            {
                var mappedInvoice = _invoiceMapper.MapToWrapper(documentsResponse);
                // Override with known context data
                mappedInvoice.Id = invoiceId;
                mappedInvoice.ProjectId = projectId;
                return mappedInvoice;
            }
            else
            {
                // Fallback manual mapping for backward compatibility
                return new Invoice 
                { 
                    Id = invoiceId,
                    ProjectId = projectId,
                    InvoiceNumber = $"INV-{invoiceId}",
                    Amount = 0m, // Amount not available in compliance documents endpoint
                    Status = InvoiceStatus.Submitted, // Status mapping would need actual field from API
                    InvoiceDate = DateTime.UtcNow, // Would need actual date from API
                    DueDate = null,
                    VendorId = 0, // Vendor ID not available in this endpoint
                    Description = "Invoice with compliance documents",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }
        }, nameof(GetInvoiceAsync), cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Creates a new invoice.
    /// Note: Currently creates a compliance document for an existing invoice using V2.0 API.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The invoice creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created invoice.</returns>
    public async Task<Invoice> CreateInvoiceAsync(int companyId, int projectId, CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Creating invoice for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Note: The available endpoint creates compliance documents for existing invoices
            // This would typically require the invoice to already exist and you're adding documentation
            // For now, we'll return a placeholder indicating this limitation
            _logger?.LogWarning("CreateInvoiceAsync currently limited - available endpoint creates compliance documents, not invoices");
            
            return new Invoice 
            { 
                Id = 1, // Would be generated by actual invoice creation endpoint
                ProjectId = projectId,
                InvoiceNumber = request.InvoiceNumber,
                Amount = request.Amount,
                Status = InvoiceStatus.Draft,
                InvoiceDate = request.InvoiceDate,
                DueDate = request.DueDate,
                VendorId = request.VendorId,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create invoice for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Approves an invoice.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The approved invoice.</returns>
    public async Task<Invoice> ApproveInvoiceAsync(int companyId, int projectId, int invoiceId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Approving invoice {InvoiceId} for project {ProjectId} in company {CompanyId}", invoiceId, projectId, companyId);
            
            // Placeholder implementation
            return new Invoice 
            { 
                Id = invoiceId,
                ProjectId = projectId,
                Status = InvoiceStatus.Approved,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to approve invoice {InvoiceId} for project {ProjectId} in company {CompanyId}", invoiceId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Rejects an invoice with a reason.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="invoiceId">The invoice ID.</param>
    /// <param name="reason">The rejection reason.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The rejected invoice.</returns>
    public async Task<Invoice> RejectInvoiceAsync(int companyId, int projectId, int invoiceId, string reason, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(reason))
        {
            throw new ArgumentException("Reason cannot be null or empty", nameof(reason));
        }
        
        try
        {
            _logger?.LogDebug("Rejecting invoice {InvoiceId} for project {ProjectId} in company {CompanyId} with reason: {Reason}", invoiceId, projectId, companyId, reason);
            
            // Placeholder implementation
            return new Invoice 
            { 
                Id = invoiceId,
                ProjectId = projectId,
                Status = InvoiceStatus.Rejected,
                Description = $"Rejected: {reason}",
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to reject invoice {InvoiceId} for project {ProjectId} in company {CompanyId}", invoiceId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Payment Operations

    /// <summary>
    /// Processes a payment for an invoice.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="request">The payment processing request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The financial transaction record.</returns>
    public async Task<FinancialTransaction> ProcessPaymentAsync(int companyId, int projectId, ProcessPaymentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        try
        {
            _logger?.LogDebug("Processing payment for invoice {InvoiceId} in project {ProjectId} in company {CompanyId}", request.InvoiceId, projectId, companyId);
            
            // Placeholder implementation
            return new FinancialTransaction 
            { 
                Id = 1,
                ProjectId = projectId,
                Type = TransactionType.Payment,
                Amount = request.Amount,
                TransactionDate = request.PaymentDate,
                Description = $"Payment for invoice {request.InvoiceId}",
                InvoiceId = request.InvoiceId,
                Reference = request.Reference,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to process payment for invoice {InvoiceId} in project {ProjectId} in company {CompanyId}", request.InvoiceId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets all financial transactions for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of financial transactions.</returns>
    public async Task<IEnumerable<FinancialTransaction>> GetTransactionsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting transactions for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            return Enumerable.Empty<FinancialTransaction>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get transactions for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Cost Code Operations

    /// <summary>
    /// Gets all cost codes for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of cost codes.</returns>
    public async Task<IEnumerable<CostCode>> GetCostCodesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting cost codes for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            return Enumerable.Empty<CostCode>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get cost codes for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a specific cost code by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="costCodeId">The cost code ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The cost code.</returns>
    public async Task<CostCode> GetCostCodeAsync(int companyId, int projectId, int costCodeId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting cost code {CostCodeId} for project {ProjectId} in company {CompanyId}", costCodeId, projectId, companyId);
            
            // Placeholder implementation
            return new CostCode 
            { 
                Id = costCodeId,
                Code = "01000",
                Description = "General Construction",
                BudgetAmount = 100000m,
                ActualAmount = 85000m,
                CommittedAmount = 90000m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get cost code {CostCodeId} for project {ProjectId} in company {CompanyId}", costCodeId, projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Financial Reporting

    /// <summary>
    /// Gets the total cost for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The total project cost.</returns>
    public async Task<decimal> GetProjectTotalCostAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting total cost for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            return 500000.00m;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get total cost for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets a cost summary breakdown for a project.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A dictionary with cost summary by category.</returns>
    public async Task<Dictionary<string, decimal>> GetCostSummaryAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting cost summary for project {ProjectId} in company {CompanyId}", projectId, companyId);
            
            // Placeholder implementation
            return new Dictionary<string, decimal>
            {
                ["Labor"] = 150000.00m,
                ["Materials"] = 200000.00m,
                ["Equipment"] = 100000.00m,
                ["Subcontractors"] = 75000.00m,
                ["Other"] = 25000.00m
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get cost summary for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region Pagination Support

    /// <summary>
    /// Gets invoices with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of invoices.</returns>
    public async Task<CoreModels.PagedResult<Invoice>> GetInvoicesPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting invoices with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<Invoice>
            {
                Items = Enumerable.Empty<Invoice>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get invoices with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    /// <summary>
    /// Gets financial transactions with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of financial transactions.</returns>
    public async Task<CoreModels.PagedResult<FinancialTransaction>> GetTransactionsPagedAsync(int companyId, int projectId, CoreModels.PaginationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        try
        {
            _logger?.LogDebug("Getting transactions with pagination for project {ProjectId} in company {CompanyId} (page {Page}, per page {PerPage})", projectId, companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new CoreModels.PagedResult<FinancialTransaction>
            {
                Items = Enumerable.Empty<FinancialTransaction>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get transactions with pagination for project {ProjectId} in company {CompanyId}", projectId, companyId);
            throw new InvalidOperationException($"Operation failed for company {companyId}", ex);
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Disposes of the ProcoreConstructionFinancialsClient and its resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the ProcoreConstructionFinancialsClient and its resources.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // The generated client doesn't implement IDisposable, so we don't dispose it
            _disposed = true;
        }
    }

    #endregion
}