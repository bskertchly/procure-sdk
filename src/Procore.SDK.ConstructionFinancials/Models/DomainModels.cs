using System;
using System.Collections.Generic;

namespace Procore.SDK.ConstructionFinancials.Models;

/// <summary>
/// Domain models for ConstructionFinancials client
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum TransactionType
{
    Payment,
    Receipt,
    Adjustment,
    Transfer,
    Accrual
}