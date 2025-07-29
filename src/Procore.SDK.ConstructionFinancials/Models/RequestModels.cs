using System;

namespace Procore.SDK.ConstructionFinancials.Models;

/// <summary>
/// Request models for ConstructionFinancials client operations
/// </summary>

// Invoice Request Models
public class CreateInvoiceRequest
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int VendorId { get; set; }
    public string Description { get; set; } = string.Empty;
}

// Payment Request Models
public class ProcessPaymentRequest
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}

// Invoice Configuration Request Models
public class UpdateInvoiceConfigurationRequest
{
    public bool? MoveMaterialsToPreviousWorkCompleted { get; set; }
    public bool? SeparateBillingForStoredMaterials { get; set; }
    public string? StoredMaterialsBillingMethod { get; set; }
    public bool? SlidingScaleRetainageEnabled { get; set; }
}

// Compliance Document Request Models
public class CreateComplianceDocumentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DocumentType { get; set; } = "invoice_compliance";
}

// Financial Analytics Request Models
public class FinancialAnalyticsRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<string> Categories { get; set; } = new();
    public bool IncludeSubcontractors { get; set; } = true;
    public bool IncludeMaterials { get; set; } = true;
    public bool IncludeLabor { get; set; } = true;
}

// Advanced Query Request Models
public class InvoiceQueryRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<InvoiceStatus> Statuses { get; set; } = new();
    public List<int> VendorIds { get; set; } = new();
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public string? SearchTerm { get; set; }
}

public class BulkInvoiceOperationRequest
{
    public List<int> InvoiceIds { get; set; } = new();
    public BulkOperationType OperationType { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public enum BulkOperationType
{
    Approve,
    Reject,
    UpdateStatus,
    ProcessPayment,
    GenerateReports
}