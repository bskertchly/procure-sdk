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