using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.ConstructionFinancials.Tests.Models;

/// <summary>
/// Test domain models for ConstructionFinancials client testing
/// </summary>

// Core Financial Models
public class Invoice
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int VendorId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public enum InvoiceStatus
{
    Draft,
    Submitted,
    Approved,
    Paid,
    Rejected,
    Cancelled
}

public class CostCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal CommittedAmount { get; set; }
}

public class FinancialTransaction
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? InvoiceId { get; set; }
    public string Reference { get; set; } = string.Empty;
}

public enum TransactionType
{
    Payment,
    Receipt,
    Adjustment,
    Transfer,
    Accrual
}

// Request Models
public class CreateInvoiceRequest
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int VendorId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class ProcessPaymentRequest
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}

// Pagination Models
public class PaginationOptions
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 100;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// Defines the contract for the ConstructionFinancials client wrapper
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
    Task<PagedResult<Invoice>> GetInvoicesPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<FinancialTransaction>> GetTransactionsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default);
}