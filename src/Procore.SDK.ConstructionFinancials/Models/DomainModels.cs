using System;
using System.Collections.Generic;

namespace Procore.SDK.ConstructionFinancials.Models;

/// <summary>
/// Domain models for ConstructionFinancials client.
/// </summary>

// Core Financial Models

/// <summary>
/// Represents an invoice in the construction financials system.
/// </summary>
public class Invoice
{
    /// <summary>
    /// Gets or sets the unique identifier for the invoice.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this invoice.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the invoice number.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount of the invoice.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the current status of the invoice.
    /// </summary>
    public InvoiceStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date the invoice was issued.
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Gets or sets the due date for invoice payment.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the vendor identifier for this invoice.
    /// </summary>
    public int VendorId { get; set; }

    /// <summary>
    /// Gets or sets the description or notes for the invoice.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the invoice was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the invoice was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Defines the status of an invoice in the system.
/// </summary>
public enum InvoiceStatus
{
    /// <summary>
    /// Invoice is in draft state.
    /// </summary>
    Draft,
    
    /// <summary>
    /// Invoice has been submitted for approval.
    /// </summary>
    Submitted,
    
    /// <summary>
    /// Invoice has been approved for payment.
    /// </summary>
    Approved,
    
    /// <summary>
    /// Invoice has been paid.
    /// </summary>
    Paid,
    
    /// <summary>
    /// Invoice has been rejected.
    /// </summary>
    Rejected,
    
    /// <summary>
    /// Invoice has been cancelled.
    /// </summary>
    Cancelled
}

/// <summary>
/// Represents a cost code for project expense tracking.
/// </summary>
public class CostCode
{
    /// <summary>
    /// Gets or sets the unique identifier for the cost code.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the cost code value.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the cost code.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the budgeted amount for this cost code.
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// Gets or sets the actual amount spent for this cost code.
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// Gets or sets the committed amount for this cost code.
    /// </summary>
    public decimal CommittedAmount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the cost code was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the cost code was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a financial transaction in the system.
/// </summary>
public class FinancialTransaction
{
    /// <summary>
    /// Gets or sets the unique identifier for the transaction.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this transaction.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the type of financial transaction.
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Gets or sets the transaction amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date when the transaction occurred.
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Gets or sets the description of the transaction.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the associated invoice identifier, if applicable.
    /// </summary>
    public int? InvoiceId { get; set; }

    /// <summary>
    /// Gets or sets the reference number or code for the transaction.
    /// </summary>
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the transaction was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the transaction was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Defines the type of financial transaction.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Payment transaction.
    /// </summary>
    Payment,
    
    /// <summary>
    /// Receipt transaction.
    /// </summary>
    Receipt,
    
    /// <summary>
    /// Adjustment transaction.
    /// </summary>
    Adjustment,
    
    /// <summary>
    /// Transfer transaction.
    /// </summary>
    Transfer,
    
    /// <summary>
    /// Accrual transaction.
    /// </summary>
    Accrual
}

// Invoice Configuration Models

/// <summary>
/// Represents the configuration settings for invoice processing.
/// </summary>
public class InvoiceConfiguration
{
    /// <summary>
    /// Gets or sets the unique identifier for the invoice configuration.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the company identifier for this configuration.
    /// </summary>
    public int CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the project identifier for this configuration.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the contract identifier for this configuration.
    /// </summary>
    public int ContractId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether materials should be moved to previous work completed.
    /// </summary>
    public bool MoveMaterialsToPreviousWorkCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether separate billing is used for stored materials.
    /// </summary>
    public bool SeparateBillingForStoredMaterials { get; set; }

    /// <summary>
    /// Gets or sets the billing method for stored materials.
    /// </summary>
    public string StoredMaterialsBillingMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether sliding scale retainage is enabled.
    /// </summary>
    public bool SlidingScaleRetainageEnabled { get; set; }

    /// <summary>
    /// Gets or sets the retainage rule set for this configuration.
    /// </summary>
    public RetainageRuleSet? RetainageRuleSet { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the configuration was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the configuration was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a set of retainage rules for invoice configuration.
/// </summary>
public class RetainageRuleSet
{
    /// <summary>
    /// Gets or sets the unique identifier for the retainage rule set.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of retainage rules in this set.
    /// </summary>
    public List<RetainageRule> Rules { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether this rule set is active.
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Represents a single retainage rule within a rule set.
/// </summary>
public class RetainageRule
{
    /// <summary>
    /// Gets or sets the unique identifier for the retainage rule.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the type of retainage rule.
    /// </summary>
    public string RuleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the percentage to be retained.
    /// </summary>
    public decimal Percentage { get; set; }

    /// <summary>
    /// Gets or sets the threshold amount for applying this rule.
    /// </summary>
    public decimal ThresholdAmount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this rule is active.
    /// </summary>
    public bool IsActive { get; set; }
}

// Async Job Models

/// <summary>
/// Represents an asynchronous job in the system.
/// </summary>
public class AsyncJob
{
    /// <summary>
    /// Gets or sets the unique identifier for the asynchronous job.
    /// </summary>
    public string Uuid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier associated with this job.
    /// </summary>
    public int CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the current status of the job.
    /// </summary>
    public AsyncJobStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the type of job being executed.
    /// </summary>
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the result of the job execution.
    /// </summary>
    public AsyncJobResult? Result { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the job was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the job was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the error message if the job failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the progress percentage of the job execution.
    /// </summary>
    public int? ProgressPercentage { get; set; }
}

/// <summary>
/// Defines the status of an asynchronous job.
/// </summary>
public enum AsyncJobStatus
{
    /// <summary>
    /// Job is pending execution.
    /// </summary>
    Pending,
    
    /// <summary>
    /// Job is currently in progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Job has completed successfully.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Job has failed.
    /// </summary>
    Failed,
    
    /// <summary>
    /// Job has been cancelled.
    /// </summary>
    Cancelled
}

/// <summary>
/// Represents the result of an asynchronous job execution.
/// </summary>
public class AsyncJobResult
{
    /// <summary>
    /// Gets or sets the total number of records to be processed.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the number of records successfully processed.
    /// </summary>
    public int ProcessedRecords { get; set; }

    /// <summary>
    /// Gets or sets the number of records that failed to process.
    /// </summary>
    public int FailedRecords { get; set; }

    /// <summary>
    /// Gets or sets the list of error messages encountered during processing.
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Gets or sets additional data returned from the job execution.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
}

// Compliance Document Models

/// <summary>
/// Represents a compliance document associated with an invoice.
/// </summary>
public class ComplianceDocument
{
    /// <summary>
    /// Gets or sets the unique identifier for the compliance document.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the invoice identifier this document is associated with.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Gets or sets the project identifier this document is associated with.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the title of the compliance document.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the compliance document.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of compliance document.
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of files associated with this compliance document.
    /// </summary>
    public List<ComplianceDocumentFile> Files { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the document was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the document was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents a file within a compliance document.
/// </summary>
public class ComplianceDocumentFile
{
    /// <summary>
    /// Gets or sets the unique identifier for the compliance document file.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the MIME content type of the file.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the file in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the URL for downloading the file.
    /// </summary>
    public string DownloadUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the file was uploaded.
    /// </summary>
    public DateTime UploadedAt { get; set; }
}