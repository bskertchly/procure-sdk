using System;

namespace Procore.SDK.ConstructionFinancials.Models;

/// <summary>
/// Request models for ConstructionFinancials client operations.
/// </summary>

// Invoice Request Models

/// <summary>
/// Request model for creating a new invoice.
/// </summary>
public class CreateInvoiceRequest
{
    /// <summary>
    /// Gets or sets the invoice number to assign to the new invoice.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount for the invoice.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date the invoice was issued.
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Gets or sets the due date for the invoice payment.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the vendor identifier for the invoice.
    /// </summary>
    public int VendorId { get; set; }

    /// <summary>
    /// Gets or sets the description or notes for the invoice.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

// Payment Request Models

/// <summary>
/// Request model for processing a payment.
/// </summary>
public class ProcessPaymentRequest
{
    /// <summary>
    /// Gets or sets the identifier of the invoice to process payment for.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Gets or sets the payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date the payment was made.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the method used for payment.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment reference number or code.
    /// </summary>
    public string Reference { get; set; } = string.Empty;
}

// Invoice Configuration Request Models

/// <summary>
/// Request model for updating invoice configuration settings.
/// </summary>
public class UpdateInvoiceConfigurationRequest
{
    /// <summary>
    /// Gets or sets a value indicating whether materials should be moved to previous work completed.
    /// </summary>
    public bool? MoveMaterialsToPreviousWorkCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether separate billing should be used for stored materials.
    /// </summary>
    public bool? SeparateBillingForStoredMaterials { get; set; }

    /// <summary>
    /// Gets or sets the billing method for stored materials.
    /// </summary>
    public string? StoredMaterialsBillingMethod { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether sliding scale retainage should be enabled.
    /// </summary>
    public bool? SlidingScaleRetainageEnabled { get; set; }
}

// Compliance Document Request Models

/// <summary>
/// Request model for creating a compliance document.
/// </summary>
public class CreateComplianceDocumentRequest
{
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
    public string DocumentType { get; set; } = "invoice_compliance";
}

// Financial Analytics Request Models

/// <summary>
/// Request model for financial analytics operations.
/// </summary>
public class FinancialAnalyticsRequest
{
    /// <summary>
    /// Gets or sets the start date for the analytics period.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date for the analytics period.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the list of categories to include in the analysis.
    /// </summary>
    public List<string> Categories { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether subcontractors should be included in the analysis.
    /// </summary>
    public bool IncludeSubcontractors { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether materials should be included in the analysis.
    /// </summary>
    public bool IncludeMaterials { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether labor should be included in the analysis.
    /// </summary>
    public bool IncludeLabor { get; set; } = true;
}

// Advanced Query Request Models

/// <summary>
/// Request model for advanced invoice querying with filters.
/// </summary>
public class InvoiceQueryRequest
{
    /// <summary>
    /// Gets or sets the start date for filtering invoices.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date for filtering invoices.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the list of invoice statuses to filter by.
    /// </summary>
    public List<InvoiceStatus> Statuses { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of vendor identifiers to filter by.
    /// </summary>
    public List<int> VendorIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the minimum amount for filtering invoices.
    /// </summary>
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// Gets or sets the maximum amount for filtering invoices.
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// Gets or sets the search term for filtering invoices by text content.
    /// </summary>
    public string? SearchTerm { get; set; }
}

/// <summary>
/// Request model for bulk operations on multiple invoices.
/// </summary>
public class BulkInvoiceOperationRequest
{
    /// <summary>
    /// Gets or sets the list of invoice identifiers to operate on.
    /// </summary>
    public List<int> InvoiceIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the type of bulk operation to perform.
    /// </summary>
    public BulkOperationType OperationType { get; set; }

    /// <summary>
    /// Gets or sets additional parameters for the bulk operation.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
}

/// <summary>
/// Defines the type of bulk operation to perform on invoices.
/// </summary>
public enum BulkOperationType
{
    /// <summary>
    /// Approve multiple invoices.
    /// </summary>
    Approve,
    
    /// <summary>
    /// Reject multiple invoices.
    /// </summary>
    Reject,
    
    /// <summary>
    /// Update status of multiple invoices.
    /// </summary>
    UpdateStatus,
    
    /// <summary>
    /// Process payments for multiple invoices.
    /// </summary>
    ProcessPayment,
    
    /// <summary>
    /// Generate reports for multiple invoices.
    /// </summary>
    GenerateReports
}