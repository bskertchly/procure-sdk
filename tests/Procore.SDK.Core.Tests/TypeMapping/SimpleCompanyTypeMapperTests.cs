using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using GeneratedSimpleCompany = Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.Item.InstancesGetResponse_data_workflow_manager_company;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive test suite for SimpleCompanyTypeMapper covering basic mapping, null safety, and performance.
/// Validates simple company type conversions meet the &lt;1ms performance requirement.
/// </summary>
public class SimpleCompanyTypeMapperTests
{
    private readonly SimpleCompanyTypeMapper _mapper;

    public SimpleCompanyTypeMapperTests()
    {
        _mapper = new SimpleCompanyTypeMapper();
    }

    #region Accuracy Tests - Basic Data Mapping

    [Fact]
    public void MapToWrapper_WithValidCompanyData_ShouldMapNameCorrectly()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "ACME Corporation"
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0); // Simple companies don't include ID
        result.Name.Should().Be("ACME Corporation");
        result.Description.Should().BeNull();
        result.IsActive.Should().BeTrue(); // Default assumption
        result.LogoUrl.Should().BeNull();
        result.Address.Should().BeNull();
        result.CustomFields.Should().BeNull();
        
        // Timestamps should be set to current time (approximately)
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MapToWrapper_WithCustomFields_ShouldExtractNonSystemFields()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "ACME Corporation",
            AdditionalData = new Dictionary<string, object>
            {
                ["name"] = "ACME Corporation", // System property - should be filtered
                ["custom_field"] = "custom_value", // Custom field - should be kept
                ["industry"] = "Construction" // Custom field - should be kept
            }
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.CustomFields.Should().NotBeNull();
        result.CustomFields.Should().NotContainKey("name"); // System property filtered out
        result.CustomFields.Should().ContainKey("custom_field");
        result.CustomFields.Should().ContainKey("industry");
        result.CustomFields!["custom_field"].Should().Be("custom_value");
        result.CustomFields["industry"].Should().Be("Construction");
    }

    [Fact]
    public void MapToGenerated_WithValidCompanyData_ShouldMapNameOnly()
    {
        // Arrange
        var company = new Company
        {
            Id = 12345, // Should be ignored in simple mapping
            Name = "ACME Corporation",
            Description = "A construction company", // Should be ignored
            IsActive = true, // Should be ignored
            LogoUrl = "https://example.com/logo.png", // Should be ignored
            Address = new Address // Should be ignored
            {
                Street1 = "123 Main St",
                City = "New York"
            }
        };

        // Act
        var result = _mapper.MapToGenerated(company);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("ACME Corporation");
        // Only name should be mapped for simple company types
    }

    #endregion

    #region Performance Tests - <1ms Requirement

    [Fact]
    public void MapToWrapper_Performance_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany { Name = "Test Company" };
        var stopwatch = new Stopwatch();

        // Act & Assert - Multiple iterations to ensure consistent performance
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Restart();
            var result = _mapper.MapToWrapper(generatedCompany);
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
        var company = new Company { Name = "Test Company" };
        var stopwatch = new Stopwatch();

        // Act & Assert - Multiple iterations to ensure consistent performance
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Restart();
            var result = _mapper.MapToGenerated(company);
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

    #region Edge Case Tests - Null Safety and Missing Data

    [Fact]
    public void MapToWrapper_WithNullName_ShouldProvideEmptyString()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = null
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.Name.Should().Be(string.Empty);
        result.IsActive.Should().BeTrue(); // Default assumption
    }

    [Fact]
    public void MapToWrapper_WithEmptyName_ShouldPreserveEmptyString()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = ""
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.Name.Should().Be(string.Empty);
    }

    [Fact]
    public void MapToWrapper_WithNullAdditionalData_ShouldReturnNullCustomFields()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "Test Company",
            AdditionalData = null
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.CustomFields.Should().BeNull();
    }

    [Fact]
    public void MapToWrapper_WithEmptyAdditionalData_ShouldReturnNullCustomFields()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "Test Company",
            AdditionalData = new Dictionary<string, object>()
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.CustomFields.Should().BeNull();
    }

    [Fact]
    public void MapToWrapper_WithOnlySystemPropertiesInAdditionalData_ShouldReturnNullCustomFields()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "Test Company",
            AdditionalData = new Dictionary<string, object>
            {
                ["name"] = "Test Company" // Only system property
            }
        };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.CustomFields.Should().BeNull(); // No custom fields after filtering
    }

    [Fact]
    public void MapToGenerated_WithNullName_ShouldMapNull()
    {
        // Arrange
        var company = new Company
        {
            Name = null!
        };

        // Act
        var result = _mapper.MapToGenerated(company);

        // Assert
        result.Name.Should().BeNull();
    }

    [Fact]
    public void MapToGenerated_WithEmptyName_ShouldMapEmptyString()
    {
        // Arrange
        var company = new Company
        {
            Name = ""
        };

        // Act
        var result = _mapper.MapToGenerated(company);

        // Assert
        result.Name.Should().Be("");
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
        var generatedCompany = new GeneratedSimpleCompany { Name = "Test Company" };

        // Act
        var success = _mapper.TryMapToWrapper(generatedCompany, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Company");
    }

    [Fact]
    public void TryMapToGenerated_WithValidData_ShouldReturnTrueAndResult()
    {
        // Arrange
        var company = new Company { Name = "Test Company" };

        // Act
        var success = _mapper.TryMapToGenerated(company, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Company");
    }

    #endregion

    #region Type Safety Tests

    [Fact]
    public void Mapper_TypeProperties_ShouldReturnCorrectTypes()
    {
        // Assert
        _mapper.WrapperType.Should().Be<Company>();
        _mapper.GeneratedType.Should().Be<GeneratedSimpleCompany>();
    }

    [Fact]
    public void MapToWrapper_ResultType_ShouldBeCompany()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany { Name = "Test Company" };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert
        result.Should().BeOfType<Company>();
    }

    [Fact]
    public void MapToGenerated_ResultType_ShouldBeGeneratedSimpleCompany()
    {
        // Arrange
        var company = new Company { Name = "Test Company" };

        // Act
        var result = _mapper.MapToGenerated(company);

        // Assert
        result.Should().BeOfType<GeneratedSimpleCompany>();
    }

    #endregion

    #region Metrics and Performance Validation Tests

    [Fact]
    public void Metrics_AfterMappingOperations_ShouldRecordAccurateMetrics()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany { Name = "Test Company" };
        var company = new Company { Name = "Test Company" };

        // Act - Perform multiple operations
        _mapper.MapToWrapper(generatedCompany);
        _mapper.MapToWrapper(generatedCompany);
        _mapper.MapToGenerated(company);

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
        var generatedCompany = new GeneratedSimpleCompany { Name = "Test Company" };
        var company = new Company { Name = "Test Company" };

        for (int i = 0; i < 10; i++)
        {
            _mapper.MapToWrapper(generatedCompany);
            _mapper.MapToGenerated(company);
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

    #region Simple Company Specific Tests

    [Fact]
    public void MapToWrapper_SimpleCompanyCharacteristics_ShouldSetAppropriateDefaults()
    {
        // Arrange
        var generatedCompany = new GeneratedSimpleCompany { Name = "Simple Company" };

        // Act
        var result = _mapper.MapToWrapper(generatedCompany);

        // Assert - Simple companies have limited data
        result.Id.Should().Be(0); // No ID in simple types
        result.Name.Should().Be("Simple Company");
        result.Description.Should().BeNull(); // Not available in simple types
        result.IsActive.Should().BeTrue(); // Default assumption for referenced companies
        result.LogoUrl.Should().BeNull(); // Not available in simple types
        result.Address.Should().BeNull(); // Not available in simple types
        
        // Timestamps should be current (since not available in simple types)
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MapToGenerated_FullCompanyToSimple_ShouldPreserveOnlyName()
    {
        // Arrange - Rich Company object
        var company = new Company
        {
            Id = 12345,
            Name = "Full Company Data",
            Description = "This should be ignored",
            IsActive = false,
            LogoUrl = "https://example.com/logo.png",
            Address = new Address
            {
                Street1 = "123 Main St",
                City = "New York",
                State = "NY"
            },
            CustomFields = new Dictionary<string, object>
            {
                ["industry"] = "Construction"
            }
        };

        // Act
        var result = _mapper.MapToGenerated(company);

        // Assert - Only name should be preserved in simple mapping
        result.Name.Should().Be("Full Company Data");
        // All other data is lost in simple company mapping - this is expected behavior
    }

    #endregion
}