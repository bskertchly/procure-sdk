using System;
using System.Collections.Generic;
using FluentAssertions;
using Procore.SDK.Core.Tests.TypeMapping;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using Procore.SDK.ConstructionFinancials.TypeMapping;
using Xunit;
using Xunit.Abstractions;
using GeneratedDocumentsPostResponse = Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse;

namespace Procore.SDK.ConstructionFinancials.Tests.TypeMapping;

/// <summary>
/// Unit tests for CostCodeTypeMapper validating financial precision, performance, and data integrity.
/// Focuses on decimal precision handling and complex financial calculations.
/// </summary>
public class CostCodeTypeMapperTests : TypeMapperTestBase<CostCode, GeneratedDocumentsPostResponse>
{
    public CostCodeTypeMapperTests(ITestOutputHelper output) : base(output)
    {
    }

    protected override ITypeMapper<CostCode, GeneratedDocumentsPostResponse> CreateMapper()
        => new CostCodeTypeMapper();

    protected override CostCode CreateComplexWrapper()
        => new CostCode
        {
            Id = 98765,
            Code = "01.0100.001",
            Description = "General Conditions - Site Preparation and Utilities",
            BudgetAmount = 125000.50m,
            ActualAmount = 87456.75m,
            CommittedAmount = 34567.25m,
            CreatedAt = new DateTime(2024, 1, 15, 8, 0, 0),
            UpdatedAt = new DateTime(2024, 7, 29, 16, 45, 30)
        };

    protected override GeneratedDocumentsPostResponse CreateComplexGenerated()
        => new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 98765,
                ["code"] = "01.0100.001",
                ["description"] = "General Conditions - Site Preparation and Utilities",
                ["budget_amount"] = 125000.50m,
                ["actual_amount"] = 87456.75m,
                ["committed_amount"] = 34567.25m,
                ["created_at"] = new DateTimeOffset(2024, 1, 15, 8, 0, 0, TimeSpan.Zero),
                ["updated_at"] = new DateTimeOffset(2024, 7, 29, 16, 45, 30, TimeSpan.Zero)
            }
        };

    protected override CostCode CreateMinimalWrapper()
        => new CostCode
        {
            Id = 1,
            Code = "00.0000.000",
            Description = string.Empty,
            BudgetAmount = 0m,
            ActualAmount = 0m,
            CommittedAmount = 0m,
            CreatedAt = DateTime.MinValue,
            UpdatedAt = DateTime.MinValue
        };

    protected override GeneratedDocumentsPostResponse CreateMinimalGenerated()
        => new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["code"] = "00.0000.000",
                ["description"] = string.Empty,
                ["budget_amount"] = 0m,
                ["actual_amount"] = 0m,
                ["committed_amount"] = 0m,
                ["created_at"] = DateTimeOffset.MinValue,
                ["updated_at"] = DateTimeOffset.MinValue
            }
        };

    [Fact]
    public void Mapper_ToWrapper_ShouldMapAllFinancialPropertiesCorrectly()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(98765);
        result.Code.Should().Be("01.0100.001");
        result.Description.Should().Be("General Conditions - Site Preparation and Utilities");
        result.BudgetAmount.Should().Be(125000.50m);
        result.ActualAmount.Should().Be(87456.75m);
        result.CommittedAmount.Should().Be(34567.25m);
        result.CreatedAt.Should().Be(new DateTime(2024, 1, 15, 8, 0, 0));
        result.UpdatedAt.Should().Be(new DateTime(2024, 7, 29, 16, 45, 30));
    }

    [Theory]
    [InlineData(123.456789, 123.46)]  // Rounds up
    [InlineData(123.454999, 123.45)]  // Rounds down
    [InlineData(123.455, 123.46)]     // Rounds away from zero
    [InlineData(0.0, 0.0)]            // Zero handling
    [InlineData(999999.999, 1000000.00)] // Large number rounding
    public void Mapper_DecimalPrecision_ShouldRoundToTwoDecimalPlaces(decimal input, decimal expected)
    {
        // Arrange
        var mapper = CreateMapper();
        var source = new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["code"] = "TEST.001",
                ["description"] = "Test Cost Code",
                ["budget_amount"] = input,
                ["actual_amount"] = input,
                ["committed_amount"] = input,
                ["created_at"] = DateTimeOffset.UtcNow,
                ["updated_at"] = DateTimeOffset.UtcNow
            }
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.BudgetAmount.Should().Be(expected);
        result.ActualAmount.Should().Be(expected);
        result.CommittedAmount.Should().Be(expected);
    }

    [Theory]
    [InlineData(123.45, 123.45)]     // Decimal input
    [InlineData(123.45d, 123.45)]    // Double input
    [InlineData(123.45f, 123.45)]    // Float input
    [InlineData(123, 123.00)]        // Integer input
    [InlineData(123L, 123.00)]       // Long input
    public void Mapper_NumericTypeConversion_ShouldHandleAllNumericTypes(object input, decimal expected)
    {
        // Arrange
        var mapper = CreateMapper();
        var source = new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["code"] = "TEST.001",
                ["description"] = "Test Cost Code",
                ["budget_amount"] = input,
                ["actual_amount"] = 0m,
                ["committed_amount"] = 0m,
                ["created_at"] = DateTimeOffset.UtcNow,
                ["updated_at"] = DateTimeOffset.UtcNow
            }
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.BudgetAmount.Should().Be(expected);
    }

    [Fact]
    public void Mapper_StringToDecimalConversion_ShouldParseValidNumbers()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["code"] = "TEST.001",
                ["description"] = "Test Cost Code",
                ["budget_amount"] = "12345.67",  // Valid string number
                ["actual_amount"] = "invalid",   // Invalid string (should default to 0)
                ["committed_amount"] = "",       // Empty string (should default to 0)
                ["created_at"] = DateTimeOffset.UtcNow,
                ["updated_at"] = DateTimeOffset.UtcNow
            }
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.BudgetAmount.Should().Be(12345.67m);
        result.ActualAmount.Should().Be(0m);
        result.CommittedAmount.Should().Be(0m);
    }

    [Fact]
    public void Mapper_FinancialCalculations_ShouldMaintainPrecision()
    {
        // Arrange - Test real-world financial scenario
        var mapper = CreateMapper();
        var budgetAmount = 100000.00m;
        var actualAmount = 85432.10m;
        var committedAmount = 12567.90m;
        
        var source = new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["code"] = "FINANCE.TEST",
                ["description"] = "Financial Precision Test",
                ["budget_amount"] = budgetAmount,
                ["actual_amount"] = actualAmount,
                ["committed_amount"] = committedAmount,
                ["created_at"] = DateTimeOffset.UtcNow,
                ["updated_at"] = DateTimeOffset.UtcNow
            }
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert - Verify no precision loss in financial calculations
        var totalSpent = result.ActualAmount + result.CommittedAmount;
        var remainingBudget = result.BudgetAmount - totalSpent;
        
        totalSpent.Should().Be(98000.00m);
        remainingBudget.Should().Be(2000.00m);
        
        // Verify individual amounts maintain precision
        result.BudgetAmount.Should().Be(budgetAmount);
        result.ActualAmount.Should().Be(actualAmount);
        result.CommittedAmount.Should().Be(committedAmount);
    }

    [Fact]
    public void Mapper_LargeFinancialAmounts_ShouldHandleCorrectly()
    {
        // Arrange - Test with large construction project amounts
        var mapper = CreateMapper();
        var largeAmount = 50_000_000.99m; // $50M project
        
        var source = new GeneratedDocumentsPostResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["code"] = "LARGE.PROJECT",
                ["description"] = "Large Scale Construction Project",
                ["budget_amount"] = largeAmount,
                ["actual_amount"] = largeAmount * 0.65m, // 65% complete
                ["committed_amount"] = largeAmount * 0.25m, // 25% committed
                ["created_at"] = DateTimeOffset.UtcNow,
                ["updated_at"] = DateTimeOffset.UtcNow
            }
        };

        // Act
        var result = mapper.MapToWrapper(source);

        // Assert
        result.BudgetAmount.Should().Be(50_000_000.99m);
        result.ActualAmount.Should().Be(32_500_000.64m); // Rounded appropriately
        result.CommittedAmount.Should().Be(12_500_000.25m); // Rounded appropriately
    }

    [Fact]
    public void Mapper_PerformanceWithFinancialPrecision_ShouldMeetTargets()
    {
        // Arrange
        var mapper = CreateMapper();
        var source = CreateComplexGenerated();

        // Act & Assert - Test performance with financial precision calculations
        var result = PerformanceValidator.MeasureOperation(
            () => mapper.MapToWrapper(source),
            "Complex CostCode with financial precision ToWrapper conversion");

        result.ElapsedMilliseconds.Should().BeLessOrEqualTo(1,
            $"CostCode financial precision conversion should complete within 1ms, actual: {result.ElapsedMilliseconds}ms");
        
        Output.WriteLine($"CostCode financial mapping performance: {result.ElapsedTicks} ticks ({result.ElapsedMilliseconds}ms)");
    }

    [Fact]
    public void Mapper_FinancialDataIntegrity_ShouldPreserveAllPrecision()
    {
        // Arrange
        var mapper = CreateMapper();
        var originalCostCode = CreateComplexWrapper();

        // Act
        var integrityResult = IntegrityValidator.ValidateRoundTripIntegrity(originalCostCode, mapper);

        // Assert
        integrityResult.IsValid.Should().BeTrue(
            $"CostCode round trip conversion should preserve all financial precision. Failures: {string.Join(", ", integrityResult.Failures)}");
    }

    protected override void ValidateBasicMappingToWrapper(GeneratedDocumentsPostResponse source, CostCode result)
    {
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThanOrEqualTo(0);
        result.Code.Should().NotBeNull();
        result.Description.Should().NotBeNull();
        result.BudgetAmount.Should().BeGreaterThanOrEqualTo(0);
        result.ActualAmount.Should().BeGreaterThanOrEqualTo(0);
        result.CommittedAmount.Should().BeGreaterThanOrEqualTo(0);
    }

    protected override void ValidateBasicMappingToGenerated(CostCode source, GeneratedDocumentsPostResponse result)
    {
        result.Should().NotBeNull();
        result.AdditionalData.Should().NotBeNull();
        result.AdditionalData.Should().ContainKey("id");
        result.AdditionalData.Should().ContainKey("code");
        result.AdditionalData.Should().ContainKey("description");
        result.AdditionalData.Should().ContainKey("budget_amount");
        result.AdditionalData.Should().ContainKey("actual_amount");
        result.AdditionalData.Should().ContainKey("committed_amount");
    }
}