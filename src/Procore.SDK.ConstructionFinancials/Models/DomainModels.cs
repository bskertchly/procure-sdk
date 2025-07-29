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

// Invoice Configuration Models
public class InvoiceConfiguration
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int ProjectId { get; set; }
    public int ContractId { get; set; }
    public bool MoveMaterialsToPreviousWorkCompleted { get; set; }
    public bool SeparateBillingForStoredMaterials { get; set; }
    public string StoredMaterialsBillingMethod { get; set; } = string.Empty;
    public bool SlidingScaleRetainageEnabled { get; set; }
    public RetainageRuleSet? RetainageRuleSet { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class RetainageRuleSet
{
    public int Id { get; set; }
    public List<RetainageRule> Rules { get; set; } = new();
    public bool IsActive { get; set; }
}

public class RetainageRule
{
    public int Id { get; set; }
    public string RuleType { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public decimal ThresholdAmount { get; set; }
    public bool IsActive { get; set; }
}

// Async Job Models
public class AsyncJob
{
    public string Uuid { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public AsyncJobStatus Status { get; set; }
    public string JobType { get; set; } = string.Empty;
    public AsyncJobResult? Result { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public int? ProgressPercentage { get; set; }
}

public enum AsyncJobStatus
{
    Pending,
    InProgress,
    Completed,
    Failed,
    Cancelled
}

public class AsyncJobResult
{
    public int TotalRecords { get; set; }
    public int ProcessedRecords { get; set; }
    public int FailedRecords { get; set; }
    public List<string> Errors { get; set; } = new();
    public Dictionary<string, object> Data { get; set; } = new();
}

// Compliance Document Models
public class ComplianceDocument
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public List<ComplianceDocumentFile> Files { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ComplianceDocumentFile
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string DownloadUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}