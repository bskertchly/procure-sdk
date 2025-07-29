using System;
using System.Collections.Generic;
using FluentAssertions;
using Procore.SDK.Core.Tests.TypeMapping;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using Procore.SDK.ConstructionFinancials.TypeMapping;
using Xunit;
using Xunit.Abstractions;
using GeneratedDocumentsGetResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse;

namespace Procore.SDK.ConstructionFinancials.Tests.TypeMapping;

/// <summary>
/// Unit tests for FinancialTransactionTypeMapper validating performance, data integrity, and conversion accuracy.
/// Implements comprehensive test coverage following Task 10 requirements.
/// </summary>
public class FinancialTransactionTypeMapperTests : TypeMapperTestBase<FinancialTransaction, GeneratedDocumentsGetResponse>
{
    public FinancialTransactionTypeMapperTests(ITestOutputHelper output) : base(output)
    {
    }

    protected override ITypeMapper<FinancialTransaction, GeneratedDocumentsGetResponse> CreateMapper()
        => new FinancialTransactionTypeMapper();

    protected override FinancialTransaction CreateComplexWrapper()
        => new FinancialTransaction
        {
            Id = 12345,
            ProjectId = 67890,
            Type = TransactionType.Payment,
            Amount = 15000.75m,
            TransactionDate = new DateTime(2024, 7, 15, 14, 30, 0),
            Description = "Complex financial transaction for testing purposes",
            InvoiceId = 54321,
            Reference = "REF-2024-07-15-001",
            CreatedAt = new DateTime(2024, 7, 15, 9, 0, 0),
            UpdatedAt = new DateTime(2024, 7, 15, 14, 30, 0)
        };

    protected override GeneratedDocumentsGetResponse CreateComplexGenerated()
        => new GeneratedDocumentsGetResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 12345,
                ["project_id"] = 67890,
                ["type"] = "payment",
                ["amount"] = 15000.75m,
                ["transaction_date"] = new DateTimeOffset(2024, 7, 15, 14, 30, 0, TimeSpan.Zero),
                ["description"] = "Complex financial transaction for testing purposes",
                ["invoice_id"] = 54321,
                ["reference"] = "REF-2024-07-15-001",
                ["created_at"] = new DateTimeOffset(2024, 7, 15, 9, 0, 0, TimeSpan.Zero),
                ["updated_at"] = new DateTimeOffset(2024, 7, 15, 14, 30, 0, TimeSpan.Zero)
            }
        };

    protected override FinancialTransaction CreateMinimalWrapper()
        => new FinancialTransaction
        {
            Id = 1,
            ProjectId = 1,
            Type = TransactionType.Payment,
            Amount = 0m,
            TransactionDate = DateTime.MinValue,
            Description = string.Empty,
            Reference = string.Empty,
            CreatedAt = DateTime.MinValue,
            UpdatedAt = DateTime.MinValue
        };

    protected override GeneratedDocumentsGetResponse CreateMinimalGenerated()
        => new GeneratedDocumentsGetResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["project_id"] = 1,
                ["type"] = "payment",
                ["amount"] = 0m,
                ["transaction_date"] = DateTimeOffset.MinValue,
                ["description"] = string.Empty,
                ["reference"] = string.Empty,
                ["created_at"] = DateTimeOffset.MinValue,
                ["updated_at"] = DateTimeOffset.MinValue
            }
        };

    [Fact]
    public void Mapper_ToWrapper_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(12345);
        result.ProjectId.Should().Be(67890);
        result.Type.Should().Be(TransactionType.Payment);
        result.Amount.Should().Be(15000.75m);
        result.TransactionDate.Should().Be(new DateTime(2024, 7, 15, 14, 30, 0));
        result.Description.Should().Be("Complex financial transaction for testing purposes");
        result.InvoiceId.Should().Be(54321);
        result.Reference.Should().Be("REF-2024-07-15-001");
        result.CreatedAt.Should().Be(new DateTime(2024, 7, 15, 9, 0, 0));
        result.UpdatedAt.Should().Be(new DateTime(2024, 7, 15, 14, 30, 0));
    }

    [Fact]
    public void Mapper_ToGenerated_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexWrapper();

        // Act
        var result = mapper.MapToGenerated(source);

        // Assert
        result.Should().NotBeNull();
        result.AdditionalData.Should().NotBeNull();
        result.AdditionalData["id"].Should().Be(12345);
        result.AdditionalData["project_id"].Should().Be(67890);
        result.AdditionalData["type"].Should().Be("payment");
        result.AdditionalData["amount"].Should().Be(15000.75m);
        result.AdditionalData["description"].Should().Be("Complex financial transaction for testing purposes");
        result.AdditionalData["invoice_id"].Should().Be(54321);
        result.AdditionalData["reference"].Should().Be("REF-2024-07-15-001");
    }

    [Theory]
    [InlineData("payment", TransactionType.Payment)]
    [InlineData("receipt", TransactionType.Receipt)]
    [InlineData("adjustment", TransactionType.Adjustment)]
    [InlineData("transfer", TransactionType.Transfer)]
    [InlineData("accrual", TransactionType.Accrual)]
    [InlineData("invalid", TransactionType.Payment)] // Fallback case
    [InlineData("", TransactionType.Payment)] // Empty string fallback
    public void Mapper_TransactionTypeMapping_ShouldHandleAllEnumValues(string sourceType, TransactionType expectedType)
    {
        // Arrange
        var mapper = CreateMapper();
        var source = new GeneratedDocumentsGetResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["project_id"] = 1,
                ["type"] = sourceType,
                ["amount"] = 100m,
                ["transaction_date"] = DateTimeOffset.UtcNow,
                ["description"] = "Test transaction",
                ["reference"] = "TEST-001",
                ["created_at"] = DateTimeOffset.UtcNow,
                ["updated_at"] = DateTimeOffset.UtcNow
            }
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.Type.Should().Be(expectedType);
    }

    [Fact]
    public void Mapper_WithNullInvoiceId_ShouldHandleGracefully()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();
        source.AdditionalData.Remove("invoice_id"); // Remove invoice_id to test null handling

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.InvoiceId.Should().BeNull();
    }

    [Fact]
    public void Mapper_WithNullAdditionalData_ShouldCreateMinimalTransaction()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = new GeneratedDocumentsGetResponse
        {
            AdditionalData = null
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.ProjectId.Should().Be(0);
        result.Type.Should().Be(TransactionType.Payment); // Default enum value
        result.Amount.Should().Be(0m);
        result.Description.Should().Be(string.Empty);
        result.Reference.Should().Be(string.Empty);
    }

    [Fact]
    public void Mapper_ComplexObjectConversion_ShouldMeetPerformanceTargets()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();

        // Act & Assert - Test performance requirements
        var result = PerformanceValidator.MeasureOperation(
            () => mapper.MapToWrapper(source),
            "Complex FinancialTransaction ToWrapper conversion");

        result.ElapsedMilliseconds.Should().BeLessOrEqualTo(1,
            $"Complex FinancialTransaction conversion should complete within 1ms, actual: {result.ElapsedMilliseconds}ms");
        
        Output.WriteLine($"Complex FinancialTransaction mapping performance: {result.ElapsedTicks} ticks ({result.ElapsedMilliseconds}ms)");
    }

    [Fact]
    public void Mapper_DataIntegrityValidation_ShouldPreserveAllData()
    {
        // Arrange
        var mapper = CreateMapper();
        var originalTransaction = CreateComplexWrapper();

        // Act
        var integrityResult = IntegrityValidator.ValidateRoundTripIntegrity(originalTransaction, mapper);

        // Assert
        integrityResult.IsValid.Should().BeTrue(
            $"FinancialTransaction round trip conversion should preserve all data. Failures: {string.Join(", ", integrityResult.Failures)}");
    }

    protected override void ValidateBasicMappingToWrapper(GeneratedDocumentsGetResponse source, FinancialTransaction result)
    {
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.ProjectId.Should().BeGreaterThan(0);
        result.Type.Should().BeOneOf(Enum.GetValues<TransactionType>());
        result.Description.Should().NotBeNull();
        result.Reference.Should().NotBeNull();
    }

    protected override void ValidateBasicMappingToGenerated(FinancialTransaction source, GeneratedDocumentsGetResponse result)
    {
        result.Should().NotBeNull();
        result.AdditionalData.Should().NotBeNull();
        result.AdditionalData.Should().ContainKey("id");
        result.AdditionalData.Should().ContainKey("project_id");
        result.AdditionalData.Should().ContainKey("type");
        result.AdditionalData.Should().ContainKey("amount");
    }
}