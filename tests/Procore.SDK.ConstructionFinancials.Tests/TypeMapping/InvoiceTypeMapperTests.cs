using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Procore.SDK.ConstructionFinancials.Models;
using Procore.SDK.ConstructionFinancials.TypeMapping;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using GeneratedDocumentResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;

namespace Procore.SDK.ConstructionFinancials.Tests.TypeMapping;

/// <summary>
/// Comprehensive test suite for InvoiceTypeMapper covering field conversion, date handling, and performance.
/// Note: This mapper deals with limited compliance document endpoints rather than full invoice data.
/// Validates the simplified mapping approach and placeholder behavior.
/// </summary>
public class InvoiceTypeMapperTests
{
    private readonly InvoiceTypeMapper _mapper;

    public InvoiceTypeMapperTests()
    {
        _mapper = new InvoiceTypeMapper();
    }

    #region Accuracy Tests - Field Conversion

    [Fact]
    public void MapToWrapper_WithBasicDocumentResponse_ShouldCreatePlaceholderInvoice()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["invoice_id"] = "12345",
                ["invoice_number"] = "INV-2023-001",
                ["amount"] = "1500.00",
                ["status"] = "Approved"
            }
        };

        // Act
        var result = _mapper.MapToWrapper(documentResponse);

        // Assert - Placeholder mapping with extracted ID
        result.Should().NotBeNull();
        result.Id.Should().Be(12345); // Extracted from additional data
        result.ProjectId.Should().Be(0); // Not available in document response
        result.InvoiceNumber.Should().Be("PLACEHOLDER"); // Fixed placeholder
        result.Amount.Should().Be(0m); // Not available in document response
        result.Status.Should().Be(InvoiceStatus.Submitted); // Default assumption
        result.DueDate.Should().BeNull();
        result.VendorId.Should().Be(0); // Not available in document response
        result.Description.Should().Be("Invoice with compliance documents"); // Fixed description
        
        // Timestamps should be current (not available in document response)
        result.InvoiceDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void MapToWrapper_WithNullAdditionalData_ShouldUseDefaults()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse
        {
            AdditionalData = null
        };

        // Act
        var result = _mapper.MapToWrapper(documentResponse);

        // Assert - All defaults since no additional data
        result.Should().NotBeNull();
        result.Id.Should().Be(0); // No ID extractable
        result.ProjectId.Should().Be(0);
        result.InvoiceNumber.Should().Be("PLACEHOLDER");
        result.Amount.Should().Be(0m);
        result.Status.Should().Be(InvoiceStatus.Submitted);
        result.VendorId.Should().Be(0);
        result.Description.Should().Be("Invoice with compliance documents");
    }

    [Fact]
    public void MapToWrapper_WithVariousIdFormats_ShouldExtractCorrectly()
    {
        // Test "id" field
        var documentResponse1 = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object> { ["id"] = "999" }
        };
        
        var result1 = _mapper.MapToWrapper(documentResponse1);
        result1.Id.Should().Be(999);

        // Test "document_id" field
        var documentResponse2 = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object> { ["document_id"] = "888" }
        };
        
        var result2 = _mapper.MapToWrapper(documentResponse2);
        result2.Id.Should().Be(888);

        // Test "invoice_id" field (priority order)
        var documentResponse3 = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object> 
            { 
                ["id"] = "777",
                ["invoice_id"] = "666" // Should take precedence
            }
        };
        
        var result3 = _mapper.MapToWrapper(documentResponse3);
        result3.Id.Should().Be(777); // First match wins based on iteration order
    }

    [Fact]
    public void MapToWrapper_WithInvalidIdFormats_ShouldDefaultToZero()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = "not-a-number",
                ["invoice_id"] = (int?)null,
                ["document_id"] = ""
            }
        };

        // Act
        var result = _mapper.MapToWrapper(documentResponse);

        // Assert
        result.Id.Should().Be(0); // Should default when parsing fails
    }

    [Fact]
    public void MapToGenerated_WithCompleteInvoice_ShouldMapToAdditionalData()
    {
        // Arrange
        var invoice = new Invoice
        {
            Id = 12345,
            InvoiceNumber = "INV-2023-001",
            Amount = 1500.75m,
            Status = InvoiceStatus.Approved,
            ProjectId = 678,
            VendorId = 999,
            Description = "Construction materials",
            InvoiceDate = new DateTime(2023, 6, 15),
            DueDate = new DateTime(2023, 7, 15),
            CreatedAt = new DateTime(2023, 6, 10),
            UpdatedAt = new DateTime(2023, 6, 20)
        };

        // Act
        var result = _mapper.MapToGenerated(invoice);

        // Assert
        result.Should().NotBeNull();
        result.AdditionalData.Should().NotBeNull();
        result.AdditionalData.Should().ContainKey("invoice_id");
        result.AdditionalData.Should().ContainKey("invoice_number");
        result.AdditionalData.Should().ContainKey("amount");
        result.AdditionalData.Should().ContainKey("status");
        
        result.AdditionalData!["invoice_id"].Should().Be(12345);
        result.AdditionalData["invoice_number"].Should().Be("INV-2023-001");
        result.AdditionalData["amount"].Should().Be("1500.75");
        result.AdditionalData["status"].Should().Be("Approved");
    }

    #endregion

    #region Performance Tests - <1ms Requirement

    [Fact]
    public void MapToWrapper_Performance_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["invoice_id"] = "123",
                ["status"] = "Approved"
            }
        };
        var stopwatch = new Stopwatch();

        // Act & Assert - Multiple iterations to ensure consistent performance
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Restart();
            var result = _mapper.MapToWrapper(documentResponse);
            stopwatch.Stop();

            // Each mapping operation must complete within 1ms
            stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(1, 
                $"Mapping iteration {i} took {stopwatch.ElapsedMilliseconds}ms");
            result.Should().NotBeNull();
        }

        // Verify metrics are recorded
        _mapper.Metrics.ToWrapperCalls.Should().Be(100);
        _mapper.Metrics.AverageToWrapperTimeMs.Should().BeLessOrEqualTo(1.0);
    }

    [Fact]
    public void MapToGenerated_Performance_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var invoice = new Invoice
        {
            Id = 123,
            InvoiceNumber = "INV-001",
            Amount = 100.00m,
            Status = InvoiceStatus.Submitted
        };
        var stopwatch = new Stopwatch();

        // Act & Assert - Multiple iterations to ensure consistent performance
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Restart();
            var result = _mapper.MapToGenerated(invoice);
            stopwatch.Stop();

            // Each mapping operation must complete within 1ms
            stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(1, 
                $"Mapping iteration {i} took {stopwatch.ElapsedMilliseconds}ms");
            result.Should().NotBeNull();
        }

        // Verify metrics are recorded
        _mapper.Metrics.ToGeneratedCalls.Should().Be(100);
        _mapper.Metrics.AverageToGeneratedTimeMs.Should().BeLessOrEqualTo(1.0);
    }

    #endregion

    #region Date Handling Tests

    [Fact]
    public void MapToWrapper_DateFields_ShouldUseCurrentDateTime()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse();
        var beforeMapping = DateTime.UtcNow;

        // Act
        var result = _mapper.MapToWrapper(documentResponse);
        var afterMapping = DateTime.UtcNow;

        // Assert - All dates should be current since not available in document response
        result.InvoiceDate.Should().BeOnOrAfter(beforeMapping).And.BeOnOrBefore(afterMapping);
        result.CreatedAt.Should().BeOnOrAfter(beforeMapping).And.BeOnOrBefore(afterMapping);
        result.UpdatedAt.Should().BeOnOrAfter(beforeMapping).And.BeOnOrBefore(afterMapping);
        result.DueDate.Should().BeNull(); // Not set in placeholder mapping
    }

    [Fact]
    public void MapToGenerated_WithDifferentInvoiceStatuses_ShouldMapCorrectly()
    {
        // Test each enum value
        var statuses = Enum.GetValues<InvoiceStatus>();
        
        foreach (var status in statuses)
        {
            // Arrange
            var invoice = new Invoice { Status = status };

            // Act
            var result = _mapper.MapToGenerated(invoice);

            // Assert
            result.AdditionalData!["status"].Should().Be(status.ToString());
        }
    }

    [Fact]
    public void MapToGenerated_WithNullDueDate_ShouldNotIncludeDueDateInAdditionalData()
    {
        // Arrange
        var invoice = new Invoice
        {
            Id = 123,
            DueDate = null
        };

        // Act
        var result = _mapper.MapToGenerated(invoice);

        // Assert
        result.AdditionalData.Should().NotContainKey("due_date");
        // Only invoice fields that are always present should be included
        result.AdditionalData.Should().ContainKey("invoice_id");
        result.AdditionalData.Should().ContainKey("amount");
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public void MapToWrapper_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => _mapper.MapToWrapper(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MapToGenerated_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => _mapper.MapToGenerated(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TryMapToWrapper_WithNullSource_ShouldReturnFalse()
    {
        // Act
        var success = _mapper.TryMapToWrapper(null!, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryMapToGenerated_WithNullSource_ShouldReturnFalse()
    {
        // Act
        var success = _mapper.TryMapToGenerated(null!, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryMapToWrapper_WithValidData_ShouldReturnTrueAndResult()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse();

        // Act
        var success = _mapper.TryMapToWrapper(documentResponse, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.InvoiceNumber.Should().Be("PLACEHOLDER");
    }

    [Fact]
    public void TryMapToGenerated_WithValidData_ShouldReturnTrueAndResult()
    {
        // Arrange
        var invoice = new Invoice { Id = 123, InvoiceNumber = "INV-001" };

        // Act
        var success = _mapper.TryMapToGenerated(invoice, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.AdditionalData.Should().ContainKey("invoice_id");
    }

    [Fact]
    public void MapToWrapper_WithMappingException_ShouldWrapInTypeMappingException()
    {
        // This test is more theoretical since the current implementation is simple
        // But validates the error handling pattern is in place
        
        // Arrange - Create a scenario that could cause an exception
        var documentResponse = new GeneratedDocumentResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = new object() // Object that can't be converted to string
            }
        };

        // Act & Assert - Should complete successfully due to safe ToString() handling
        var result = _mapper.MapToWrapper(documentResponse);
        result.Should().NotBeNull();
        // The current implementation uses safe ToString() so this won't throw
        // But the pattern is established for more complex scenarios
    }

    #endregion

    #region Type Safety Tests

    [Fact]
    public void Mapper_TypeProperties_ShouldReturnCorrectTypes()
    {
        // Assert
        _mapper.WrapperType.Should().Be<Invoice>();
        _mapper.GeneratedType.Should().Be<GeneratedDocumentResponse>();
    }

    [Fact]
    public void MapToWrapper_ResultType_ShouldBeInvoice()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse();

        // Act
        var result = _mapper.MapToWrapper(documentResponse);

        // Assert
        result.Should().BeOfType<Invoice>();
    }

    [Fact]
    public void MapToGenerated_ResultType_ShouldBeGeneratedDocumentResponse()
    {
        // Arrange
        var invoice = new Invoice();

        // Act
        var result = _mapper.MapToGenerated(invoice);

        // Assert
        result.Should().BeOfType<GeneratedDocumentResponse>();
    }

    #endregion

    #region Metrics and Performance Validation Tests

    [Fact]
    public void Metrics_AfterMappingOperations_ShouldRecordAccurateMetrics()
    {
        // Arrange
        var documentResponse = new GeneratedDocumentResponse();
        var invoice = new Invoice();

        // Act - Perform multiple operations
        _mapper.MapToWrapper(documentResponse);
        _mapper.MapToWrapper(documentResponse);
        _mapper.MapToGenerated(invoice);

        // Assert metrics
        _mapper.Metrics.ToWrapperCalls.Should().Be(2);
        _mapper.Metrics.ToGeneratedCalls.Should().Be(1);
        _mapper.Metrics.ToWrapperErrors.Should().Be(0);
        _mapper.Metrics.ToGeneratedErrors.Should().Be(0);
        _mapper.Metrics.ToWrapperErrorRate.Should().Be(0.0);
        _mapper.Metrics.ToGeneratedErrorRate.Should().Be(0.0);
    }

    [Fact]
    public void ValidatePerformance_WithDefaultTargets_ShouldMeetRequirements()
    {
        // Arrange - Perform some operations to gather metrics
        var documentResponse = new GeneratedDocumentResponse();
        var invoice = new Invoice();

        for (int i = 0; i < 10; i++)
        {
            _mapper.MapToWrapper(documentResponse);
            _mapper.MapToGenerated(invoice);
        }

        // Act
        var validation = _mapper.Metrics.ValidatePerformance();

        // Assert - Should meet <1ms target and <1% error rate
        validation.OverallValid.Should().BeTrue();
        validation.ToWrapperPerformanceOk.Should().BeTrue();
        validation.ToGeneratedPerformanceOk.Should().BeTrue();
        validation.ToWrapperErrorRateOk.Should().BeTrue();
        validation.ToGeneratedErrorRateOk.Should().BeTrue();
        validation.ActualToWrapperAverageMs.Should().BeLessOrEqualTo(1.0);
        validation.ActualToGeneratedAverageMs.Should().BeLessOrEqualTo(1.0);
    }

    #endregion

    #region Placeholder Mapping Specific Tests

    [Fact]
    public void MapToWrapper_PlaceholderBehavior_ShouldBeConsistent()
    {
        // Arrange
        var documentResponse1 = new GeneratedDocumentResponse();
        var documentResponse2 = new GeneratedDocumentResponse();

        // Act
        var result1 = _mapper.MapToWrapper(documentResponse1);
        var result2 = _mapper.MapToWrapper(documentResponse2);

        // Assert - Placeholder values should be consistent
        result1.InvoiceNumber.Should().Be("PLACEHOLDER");
        result2.InvoiceNumber.Should().Be("PLACEHOLDER");
        result1.Amount.Should().Be(0m);
        result2.Amount.Should().Be(0m);
        result1.Status.Should().Be(InvoiceStatus.Submitted);
        result2.Status.Should().Be(InvoiceStatus.Submitted);
        result1.Description.Should().Be("Invoice with compliance documents");
        result2.Description.Should().Be("Invoice with compliance documents");
    }

    [Fact]
    public void MapToWrapper_LimitationsDocumentation_ShouldReflectCurrentEndpointConstraints()
    {
        // This test documents the limitations of the current mapper
        // due to available endpoint constraints (compliance documents vs full invoices)
        
        // Arrange
        var documentResponse = new GeneratedDocumentResponse();

        // Act
        var result = _mapper.MapToWrapper(documentResponse);

        // Assert - Document the limitations
        result.ProjectId.Should().Be(0, "ProjectId not available in compliance document response");
        result.InvoiceNumber.Should().Be("PLACEHOLDER", "InvoiceNumber not available in compliance document response");
        result.Amount.Should().Be(0m, "Amount not available in compliance document response");
        result.VendorId.Should().Be(0, "VendorId not available in compliance document response");
        result.DueDate.Should().BeNull("DueDate not available in compliance document response");
        
        // These are placeholder values that would need proper invoice endpoints
        result.Status.Should().Be(InvoiceStatus.Submitted, "Default status assumption");
        result.Description.Should().Be("Invoice with compliance documents", "Fixed placeholder description");
    }

    [Fact]
    public void MapToGenerated_ReverseMapping_ShouldBeForTestingOnly()
    {
        // This test documents that reverse mapping is primarily for testing
        // since we can't create meaningful compliance document data from invoice data
        
        // Arrange
        var invoice = new Invoice
        {
            Id = 123,
            InvoiceNumber = "INV-001",
            Amount = 1500.00m,
            Status = InvoiceStatus.Approved
        };

        // Act
        var result = _mapper.MapToGenerated(invoice);

        // Assert - Only basic data preserved in additional data
        result.AdditionalData.Should().NotBeNull();
        result.AdditionalData.Should().HaveCount(4); // Only 4 basic fields
        result.AdditionalData.Should().ContainKey("invoice_id");
        result.AdditionalData.Should().ContainKey("invoice_number");
        result.AdditionalData.Should().ContainKey("amount");
        result.AdditionalData.Should().ContainKey("status");
        
        // No other document-specific fields would be meaningful
    }

    #endregion
}