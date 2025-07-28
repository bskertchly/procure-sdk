using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using GeneratedUser = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse;
using GeneratedVendor = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse_vendor;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive test suite for UserTypeMapper covering accuracy, performance, edge cases, and error handling.
/// Validates the type mapping infrastructure meets the &lt;1ms performance requirement with no data loss.
/// </summary>
public class UserTypeMapperTests
{
    private readonly UserTypeMapper _mapper;

    public UserTypeMapperTests()
    {
        _mapper = new UserTypeMapper();
    }

    #region Accuracy Tests - Complete Data Mapping

    [Fact]
    public void MapToWrapper_WithCompleteUserData_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var generatedUser = CreateCompleteGeneratedUser();

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert - Core fields
        result.Id.Should().Be(12345);
        result.Email.Should().Be("test.user@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.JobTitle.Should().Be("Senior Engineer");
        result.IsActive.Should().BeTrue();
        result.AvatarUrl.Should().Be("https://example.com/avatar.jpg");
        result.PhoneNumber.Should().Be("555-1234"); // Business phone priority

        // DateTime mappings
        result.CreatedAt.Should().Be(new DateTime(2023, 1, 15, 10, 30, 0));
        result.UpdatedAt.Should().Be(new DateTime(2023, 6, 20, 14, 45, 0));
        result.LastSignInAt.Should().Be(new DateTime(2023, 6, 19, 9, 15, 0));

        // Company mapping
        result.Company.Should().NotBeNull();
        result.Company!.Id.Should().Be(678);
        result.Company.Name.Should().Be("ACME Corporation");
        result.Company.IsActive.Should().BeTrue();

        // Custom fields
        result.CustomFields.Should().NotBeNull();
        result.CustomFields.Should().ContainKey("department");
        result.CustomFields!["department"].Should().Be("Engineering");
        result.CustomFields.Should().ContainKey("employee_number");
        result.CustomFields["employee_number"].Should().Be("ENG-001");
    }

    [Fact]
    public void MapToGenerated_WithCompleteUserData_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var user = CreateCompleteUser();

        // Act
        var result = _mapper.MapToGenerated(user);

        // Assert - Core fields
        result.Id.Should().Be(12345);
        result.EmailAddress.Should().Be("test.user@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Name.Should().Be("John Doe");
        result.JobTitle.Should().Be("Senior Engineer");
        result.IsActive.Should().BeTrue();
        result.Avatar.Should().Be("https://example.com/avatar.jpg");
        result.BusinessPhone.Should().Be("555-1234");

        // DateTime mappings
        result.CreatedAt!.Value.DateTime.Should().Be(new DateTime(2023, 1, 15, 10, 30, 0));
        result.UpdatedAt!.Value.DateTime.Should().Be(new DateTime(2023, 6, 20, 14, 45, 0));
        result.LastLoginAt!.Value.DateTime.Should().Be(new DateTime(2023, 6, 19, 9, 15, 0));

        // Vendor mapping
        result.Vendor.Should().NotBeNull();
        result.Vendor!.Id.Should().Be(678);
        result.Vendor.Name.Should().Be("ACME Corporation");

        // Address fields from company
        result.Address.Should().Be("123 Main St");
        result.City.Should().Be("New York");
        result.StateCode.Should().Be("NY");
        result.CountryCode.Should().Be("US");
        result.Zip.Should().Be("10001");
    }

    #endregion

    #region Performance Tests - <1ms Requirement

    [Fact]
    public void MapToWrapper_Performance_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var generatedUser = CreateCompleteGeneratedUser();
        var stopwatch = new Stopwatch();

        // Act & Assert - Multiple iterations to ensure consistent performance
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Restart();
            var result = _mapper.MapToWrapper(generatedUser);
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
        var user = CreateCompleteUser();
        var stopwatch = new Stopwatch();

        // Act & Assert - Multiple iterations to ensure consistent performance
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Restart();
            var result = _mapper.MapToGenerated(user);
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
    public void MapToWrapper_WithNullValues_ShouldHandleGracefully()
    {
        // Arrange
        var generatedUser = new GeneratedUser
        {
            Id = null,
            EmailAddress = null,
            FirstName = null,
            LastName = null,
            JobTitle = null,
            IsActive = null,
            CreatedAt = null,
            UpdatedAt = null,
            LastLoginAt = null,
            Avatar = null,
            BusinessPhone = null,
            MobilePhone = null,
            Vendor = null,
            AdditionalData = null
        };

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert - Should provide safe defaults
        result.Id.Should().Be(0);
        result.Email.Should().Be(string.Empty);
        result.FirstName.Should().Be(string.Empty);
        result.LastName.Should().Be(string.Empty);
        result.JobTitle.Should().BeNull();
        result.IsActive.Should().BeFalse();
        result.CreatedAt.Should().Be(DateTime.MinValue);
        result.UpdatedAt.Should().Be(DateTime.MinValue);
        result.LastSignInAt.Should().BeNull();
        result.AvatarUrl.Should().BeNull();
        result.PhoneNumber.Should().BeNull();
        result.Company.Should().BeNull();
        result.CustomFields.Should().BeNull();
    }

    [Fact]
    public void MapToWrapper_WithEmptyAdditionalData_ShouldReturnNullCustomFields()
    {
        // Arrange
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.AdditionalData = new Dictionary<string, object>();

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.CustomFields.Should().BeNull();
    }

    [Fact]
    public void MapToWrapper_WithSystemPropertiesInAdditionalData_ShouldFilterOutSystemProperties()
    {
        // Arrange
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.AdditionalData = new Dictionary<string, object>
        {
            ["id"] = "12345", // System property - should be filtered
            ["email_address"] = "test@example.com", // System property - should be filtered
            ["custom_field"] = "custom_value", // Custom field - should be kept
            ["department"] = "Engineering" // Custom field - should be kept
        };

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.CustomFields.Should().NotBeNull();
        result.CustomFields.Should().NotContainKey("id");
        result.CustomFields.Should().NotContainKey("email_address");
        result.CustomFields.Should().ContainKey("custom_field");
        result.CustomFields.Should().ContainKey("department");
        result.CustomFields!["custom_field"].Should().Be("custom_value");
        result.CustomFields["department"].Should().Be("Engineering");
    }

    [Fact]
    public void MapToWrapper_PhoneNumberPriority_ShouldPrioritizeBusinessPhoneOverMobile()
    {
        // Arrange - Both phones present
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.BusinessPhone = "555-1234";
        generatedUser.MobilePhone = "555-5678";

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.PhoneNumber.Should().Be("555-1234"); // Business phone should be prioritized
    }

    [Fact]
    public void MapToWrapper_PhoneNumberFallback_ShouldUseMobileWhenBusinessPhoneEmpty()
    {
        // Arrange - Only mobile phone present
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.BusinessPhone = "";
        generatedUser.MobilePhone = "555-5678";

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.PhoneNumber.Should().Be("555-5678"); // Should fall back to mobile
    }

    [Fact]
    public void MapToWrapper_PhoneNumberFallback_ShouldUseMobileWhenBusinessPhoneNull()
    {
        // Arrange - Business phone null, mobile present
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.BusinessPhone = null;
        generatedUser.MobilePhone = "555-5678";

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.PhoneNumber.Should().Be("555-5678"); // Should fall back to mobile
    }

    [Fact]
    public void MapToWrapper_WithNullVendor_ShouldMapCompanyAsNull()
    {
        // Arrange
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.Vendor = null;

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.Company.Should().BeNull();
    }

    [Fact]
    public void MapToWrapper_WithVendorMissingData_ShouldProvideDefaults()
    {
        // Arrange
        var generatedUser = CreateMinimalGeneratedUser();
        generatedUser.Vendor = new GeneratedVendor
        {
            Id = null,
            Name = null
        };

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.Company.Should().NotBeNull();
        result.Company!.Id.Should().Be(0);
        result.Company.Name.Should().Be(string.Empty);
        result.Company.IsActive.Should().BeTrue(); // Default assumption
    }

    [Fact]
    public void MapToGenerated_WithNullCompany_ShouldMapVendorAsNull()
    {
        // Arrange
        var user = CreateMinimalUser();
        user.Company = null;

        // Act
        var result = _mapper.MapToGenerated(user);

        // Assert
        result.Vendor.Should().BeNull();
        result.Address.Should().BeNull();
        result.City.Should().BeNull();
        result.StateCode.Should().BeNull();
        result.CountryCode.Should().BeNull();
        result.Zip.Should().BeNull();
    }

    [Fact]
    public void MapToGenerated_WithMinValueDateTimes_ShouldMapAsNull()
    {
        // Arrange
        var user = CreateMinimalUser();
        user.CreatedAt = DateTime.MinValue;
        user.UpdatedAt = DateTime.MinValue;
        user.LastSignInAt = null;

        // Act
        var result = _mapper.MapToGenerated(user);

        // Assert
        result.CreatedAt.Should().BeNull();
        result.UpdatedAt.Should().BeNull();
        result.LastLoginAt.Should().BeNull();
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
        var generatedUser = CreateMinimalGeneratedUser();

        // Act
        var success = _mapper.TryMapToWrapper(generatedUser, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(123);
        result.Email.Should().Be("test@example.com");
    }

    [Fact]
    public void TryMapToGenerated_WithValidData_ShouldReturnTrueAndResult()
    {
        // Arrange
        var user = CreateMinimalUser();

        // Act
        var success = _mapper.TryMapToGenerated(user, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(123);
        result.EmailAddress.Should().Be("test@example.com");
    }

    #endregion

    #region Metrics and Performance Validation Tests

    [Fact]
    public void Metrics_AfterMappingOperations_ShouldRecordAccurateMetrics()
    {
        // Arrange
        var generatedUser = CreateMinimalGeneratedUser();
        var user = CreateMinimalUser();

        // Act - Perform multiple operations
        _mapper.MapToWrapper(generatedUser);
        _mapper.MapToWrapper(generatedUser);
        _mapper.MapToGenerated(user);

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
        var generatedUser = CreateMinimalGeneratedUser();
        var user = CreateMinimalUser();

        for (int i = 0; i < 10; i++)
        {
            _mapper.MapToWrapper(generatedUser);
            _mapper.MapToGenerated(user);
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

    #region Type Safety Tests

    [Fact]
    public void Mapper_TypeProperties_ShouldReturnCorrectTypes()
    {
        // Assert
        _mapper.WrapperType.Should().Be<User>();
        _mapper.GeneratedType.Should().Be<GeneratedUser>();
    }

    [Fact]
    public void MapToWrapper_ResultType_ShouldBeUser()
    {
        // Arrange
        var generatedUser = CreateMinimalGeneratedUser();

        // Act
        var result = _mapper.MapToWrapper(generatedUser);

        // Assert
        result.Should().BeOfType<User>();
    }

    [Fact]
    public void MapToGenerated_ResultType_ShouldBeGeneratedUser()
    {
        // Arrange
        var user = CreateMinimalUser();

        // Act
        var result = _mapper.MapToGenerated(user);

        // Assert
        result.Should().BeOfType<GeneratedUser>();
    }

    #endregion

    #region Helper Methods

    private static GeneratedUser CreateCompleteGeneratedUser()
    {
        return new GeneratedUser
        {
            Id = 12345,
            EmailAddress = "test.user@example.com",
            FirstName = "John",
            LastName = "Doe",
            Name = "John Doe",
            JobTitle = "Senior Engineer",
            IsActive = true,
            CreatedAt = new DateTimeOffset(2023, 1, 15, 10, 30, 0, TimeSpan.Zero),
            UpdatedAt = new DateTimeOffset(2023, 6, 20, 14, 45, 0, TimeSpan.Zero),
            LastLoginAt = new DateTimeOffset(2023, 6, 19, 9, 15, 0, TimeSpan.Zero),
            Avatar = "https://example.com/avatar.jpg",
            BusinessPhone = "555-1234",
            MobilePhone = "555-5678",
            Vendor = new GeneratedVendor
            {
                Id = 678,
                Name = "ACME Corporation"
            },
            Address = "123 Main St",
            City = "New York",
            StateCode = "NY",
            CountryCode = "US",
            Zip = "10001",
            AdditionalData = new Dictionary<string, object>
            {
                ["department"] = "Engineering",
                ["employee_number"] = "ENG-001",
                ["id"] = "12345", // System property
                ["email_address"] = "test.user@example.com" // System property
            }
        };
    }

    private static GeneratedUser CreateMinimalGeneratedUser()
    {
        return new GeneratedUser
        {
            Id = 123,
            EmailAddress = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            IsActive = true
        };
    }

    private static User CreateCompleteUser()
    {
        return new User
        {
            Id = 12345,
            Email = "test.user@example.com",
            FirstName = "John",
            LastName = "Doe",
            JobTitle = "Senior Engineer",
            IsActive = true,
            CreatedAt = new DateTime(2023, 1, 15, 10, 30, 0),
            UpdatedAt = new DateTime(2023, 6, 20, 14, 45, 0),
            LastSignInAt = new DateTime(2023, 6, 19, 9, 15, 0),
            AvatarUrl = "https://example.com/avatar.jpg",
            PhoneNumber = "555-1234",
            Company = new Company
            {
                Id = 678,
                Name = "ACME Corporation",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    Street1 = "123 Main St",
                    City = "New York",
                    State = "NY",
                    Country = "US",
                    PostalCode = "10001"
                }
            }
        };
    }

    private static User CreateMinimalUser()
    {
        return new User
        {
            Id = 123,
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    #endregion
}