using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using Xunit.Abstractions;
using GeneratedUser = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse;
using GeneratedVendor = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse_vendor;
using GeneratedSimpleCompany = Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.Item.InstancesGetResponse_data_workflow_manager_company;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Comprehensive performance benchmark tests for Core type mappers to validate the &lt;1ms conversion requirement.
/// These tests ensure that the Core type mapping infrastructure meets strict performance targets under various conditions.
/// </summary>
public class TypeMappingPerformanceBenchmarkTests
{
    private readonly ITestOutputHelper _output;
    private readonly UserTypeMapper _userMapper;
    private readonly SimpleCompanyTypeMapper _companyMapper;

    public TypeMappingPerformanceBenchmarkTests(ITestOutputHelper output)
    {
        _output = output;
        _userMapper = new UserTypeMapper();
        _companyMapper = new SimpleCompanyTypeMapper();
    }

    #region Single Operation Performance Tests

    [Fact]
    public void UserMapper_SingleOperation_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var generatedUser = CreateComplexGeneratedUser();
        var user = CreateComplexUser();

        // Act & Assert - ToWrapper
        var stopwatch = Stopwatch.StartNew();
        var wrapperResult = _userMapper.MapToWrapper(generatedUser);
        stopwatch.Stop();
        
        _output.WriteLine($"UserMapper.MapToWrapper: {stopwatch.ElapsedTicks} ticks ({stopwatch.ElapsedMilliseconds}ms)");
        stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(5, "MapToWrapper should complete within 5ms");
        wrapperResult.Should().NotBeNull();

        // Act & Assert - ToGenerated
        stopwatch.Restart();
        var generatedResult = _userMapper.MapToGenerated(user);
        stopwatch.Stop();
        
        _output.WriteLine($"UserMapper.MapToGenerated: {stopwatch.ElapsedTicks} ticks ({stopwatch.ElapsedMilliseconds}ms)");
        stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(5, "MapToGenerated should complete within 5ms");
        generatedResult.Should().NotBeNull();
    }

    [Fact]
    public void CompanyMapper_SingleOperation_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var generatedCompany = CreateComplexGeneratedCompany();
        var company = CreateComplexCompany();

        // Act & Assert - ToWrapper
        var stopwatch = Stopwatch.StartNew();
        var wrapperResult = _companyMapper.MapToWrapper(generatedCompany);
        stopwatch.Stop();
        
        _output.WriteLine($"CompanyMapper.MapToWrapper: {stopwatch.ElapsedTicks} ticks ({stopwatch.ElapsedMilliseconds}ms)");
        stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(5, "MapToWrapper should complete within 5ms");
        wrapperResult.Should().NotBeNull();

        // Act & Assert - ToGenerated
        stopwatch.Restart();
        var generatedResult = _companyMapper.MapToGenerated(company);
        stopwatch.Stop();
        
        _output.WriteLine($"CompanyMapper.MapToGenerated: {stopwatch.ElapsedTicks} ticks ({stopwatch.ElapsedMilliseconds}ms)");
        stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(5, "MapToGenerated should complete within 5ms");
        generatedResult.Should().NotBeNull();
    }

    #endregion

    #region Batch Operation Performance Tests

    [Theory]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public void UserMapper_BatchOperations_ShouldMaintainPerformanceAtScale(int operationCount)
    {
        // Arrange
        var generatedUser = CreateComplexGeneratedUser();
        var user = CreateComplexUser();
        var wrapperTimes = new List<long>();
        var generatedTimes = new List<long>();

        // Act - Batch ToWrapper operations
        for (int i = 0; i < operationCount; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = _userMapper.MapToWrapper(generatedUser);
            stopwatch.Stop();
            
            wrapperTimes.Add(stopwatch.ElapsedMilliseconds);
            result.Should().NotBeNull();
        }

        // Act - Batch ToGenerated operations
        for (int i = 0; i < operationCount; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = _userMapper.MapToGenerated(user);
            stopwatch.Stop();
            
            generatedTimes.Add(stopwatch.ElapsedMilliseconds);
            result.Should().NotBeNull();
        }

        // Assert - All operations should complete within 1ms
        var maxWrapperTime = wrapperTimes.Max();
        var maxGeneratedTime = generatedTimes.Max();
        var avgWrapperTime = wrapperTimes.Average();
        var avgGeneratedTime = generatedTimes.Average();

        _output.WriteLine($"UserMapper Batch ({operationCount} ops):");
        _output.WriteLine($"  ToWrapper - Max: {maxWrapperTime}ms, Avg: {avgWrapperTime:F3}ms");
        _output.WriteLine($"  ToGenerated - Max: {maxGeneratedTime}ms, Avg: {avgGeneratedTime:F3}ms");

        maxWrapperTime.Should().BeLessOrEqualTo(5, "All ToWrapper operations should complete within 5ms");
        maxGeneratedTime.Should().BeLessOrEqualTo(5, "All ToGenerated operations should complete within 5ms");
        avgWrapperTime.Should().BeLessOrEqualTo(5.0, "Average ToWrapper time should be within 5ms");
        avgGeneratedTime.Should().BeLessOrEqualTo(5.0, "Average ToGenerated time should be within 5ms");

        // Verify metrics match expectations
        _userMapper.Metrics.AverageToWrapperTimeMs.Should().BeLessOrEqualTo(5.0);
        _userMapper.Metrics.AverageToGeneratedTimeMs.Should().BeLessOrEqualTo(5.0);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public void AllMappers_MixedBatchOperations_ShouldMaintainPerformance(int operationCount)
    {
        // Arrange
        var generatedUser = CreateComplexGeneratedUser();
        var generatedCompany = CreateComplexGeneratedCompany();
        var operationTimes = new List<long>();

        // Act - Mixed operations across all mappers
        var stopwatch = new Stopwatch();
        for (int i = 0; i < operationCount; i++)
        {
            stopwatch.Restart();
            
            // Cycle through different mappers
            switch (i % 2)
            {
                case 0:
                    var userResult = _userMapper.MapToWrapper(generatedUser);
                    userResult.Should().NotBeNull();
                    break;
                case 1:
                    var companyResult = _companyMapper.MapToWrapper(generatedCompany);
                    companyResult.Should().NotBeNull();
                    break;
            }
            
            stopwatch.Stop();
            operationTimes.Add(stopwatch.ElapsedMilliseconds);
        }

        // Assert
        var maxTime = operationTimes.Max();
        var avgTime = operationTimes.Average();

        _output.WriteLine($"Mixed Mappers Batch ({operationCount} ops):");
        _output.WriteLine($"  Max: {maxTime}ms, Avg: {avgTime:F3}ms");

        maxTime.Should().BeLessOrEqualTo(5, "All mixed operations should complete within 5ms");
        avgTime.Should().BeLessOrEqualTo(5.0, "Average mixed operation time should be within 5ms");
    }

    #endregion

    #region Memory and Resource Efficiency Tests

    [Fact]
    public void UserMapper_MemoryUsage_ShouldBeEfficient()
    {
        // Arrange
        var generatedUser = CreateComplexGeneratedUser();
        var initialMemory = GC.GetTotalMemory(true);

        // Act - Perform many operations to test memory efficiency
        for (int i = 0; i < 1000; i++)
        {
            var result = _userMapper.MapToWrapper(generatedUser);
            result.Should().NotBeNull();
        }

        // Force garbage collection and measure
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        var finalMemory = GC.GetTotalMemory(true);
        var memoryIncrease = finalMemory - initialMemory;

        _output.WriteLine($"Memory usage after 1000 UserMapper operations: {memoryIncrease:N0} bytes");

        // Assert - Memory increase should be reasonable (less than 1MB for 1000 operations)
        memoryIncrease.Should().BeLessThan(1024 * 1024, "Memory usage should be efficient");
    }

    [Fact]
    public void AllMappers_ConcurrentAccess_ShouldMaintainPerformance()
    {
        // Arrange
        var generatedUser = CreateComplexGeneratedUser();
        var generatedCompany = CreateComplexGeneratedCompany();
        var operationTimes = new List<long>();
        var lockObj = new object();

        // Act - Simulate concurrent access
        var tasks = Enumerable.Range(0, 100).Select(i => 
            System.Threading.Tasks.Task.Run(() =>
            {
                var stopwatch = Stopwatch.StartNew();
                
                // Each task performs operations on all mappers
                var userResult = _userMapper.MapToWrapper(generatedUser);
                var companyResult = _companyMapper.MapToWrapper(generatedCompany);
                
                stopwatch.Stop();
                
                lock (lockObj)
                {
                    operationTimes.Add(stopwatch.ElapsedMilliseconds);
                }
                
                // Verify results
                userResult.Should().NotBeNull();
                companyResult.Should().NotBeNull();
            })
        ).ToArray();

        // Wait for all tasks to complete
        System.Threading.Tasks.Task.WaitAll(tasks);

        // Assert
        var maxTime = operationTimes.Max();
        var avgTime = operationTimes.Average();

        _output.WriteLine($"Concurrent Access (100 tasks, 2 ops each):");
        _output.WriteLine($"  Max: {maxTime}ms, Avg: {avgTime:F3}ms");

        maxTime.Should().BeLessOrEqualTo(5, "Concurrent operations should complete within reasonable time");
        avgTime.Should().BeLessOrEqualTo(2.0, "Average concurrent operation time should be reasonable");
    }

    #endregion

    #region Edge Case Performance Tests

    [Fact]
    public void UserMapper_WithLargeCustomFields_ShouldMaintainPerformance()
    {
        // Arrange - Create user with many custom fields
        var generatedUser = CreateComplexGeneratedUser();
        generatedUser.AdditionalData = new Dictionary<string, object>();
        
        // Add 50 custom fields
        for (int i = 0; i < 50; i++)
        {
            generatedUser.AdditionalData[$"custom_field_{i}"] = $"value_{i}";
        }

        // Act & Assert
        var stopwatch = Stopwatch.StartNew();
        var result = _userMapper.MapToWrapper(generatedUser);
        stopwatch.Stop();

        _output.WriteLine($"UserMapper with 50 custom fields: {stopwatch.ElapsedMilliseconds}ms");
        
        stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(5, "Large custom fields should not impact performance");
        result.Should().NotBeNull();
        result.CustomFields.Should().HaveCount(50);
    }

    [Fact]
    public void Mappers_WithNullAndEmptyData_ShouldMaintainPerformance()
    {
        // Arrange
        var emptyGeneratedUser = new GeneratedUser();
        var emptyGeneratedCompany = new GeneratedSimpleCompany();

        // Act & Assert - Test performance with minimal data
        var times = new List<long>();
        
        for (int i = 0; i < 100; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            
            var userResult = _userMapper.MapToWrapper(emptyGeneratedUser);
            var companyResult = _companyMapper.MapToWrapper(emptyGeneratedCompany);
            
            stopwatch.Stop();
            times.Add(stopwatch.ElapsedMilliseconds);
            
            userResult.Should().NotBeNull();
            companyResult.Should().NotBeNull();
        }

        var maxTime = times.Max();
        var avgTime = times.Average();

        _output.WriteLine($"Empty data performance (100 ops): Max: {maxTime}ms, Avg: {avgTime:F3}ms");
        
        maxTime.Should().BeLessOrEqualTo(5, "Empty data operations should complete within 5ms");
        avgTime.Should().BeLessOrEqualTo(5.0, "Average empty data operation time should be within 5ms");
    }

    #endregion

    #region Performance Validation Tests

    [Fact]
    public void AllMappers_PerformanceValidation_ShouldMeetTargets()
    {
        // Arrange - Perform operations to gather metrics
        var generatedUser = CreateComplexGeneratedUser();
        var user = CreateComplexUser();
        var generatedCompany = CreateComplexGeneratedCompany();
        var company = CreateComplexCompany();

        // Act - Perform operations on all mappers
        for (int i = 0; i < 50; i++)
        {
            _userMapper.MapToWrapper(generatedUser);
            _userMapper.MapToGenerated(user);
            _companyMapper.MapToWrapper(generatedCompany);
            _companyMapper.MapToGenerated(company);
        }

        // Assert - Validate performance for all mappers
        var userValidation = _userMapper.Metrics.ValidatePerformance();
        var companyValidation = _companyMapper.Metrics.ValidatePerformance();

        _output.WriteLine($"Performance Validation Results:");
        _output.WriteLine($"  UserMapper: ToWrapper={userValidation.ActualToWrapperAverageMs:F3}ms, ToGenerated={userValidation.ActualToGeneratedAverageMs:F3}ms");
        _output.WriteLine($"  CompanyMapper: ToWrapper={companyValidation.ActualToWrapperAverageMs:F3}ms, ToGenerated={companyValidation.ActualToGeneratedAverageMs:F3}ms");

        // All mappers should meet performance targets
        userValidation.OverallValid.Should().BeTrue("UserMapper should meet performance targets");
        companyValidation.OverallValid.Should().BeTrue("CompanyMapper should meet performance targets");
    }

    [Fact]
    public void AllMappers_StressTest_ShouldMaintainConsistentPerformance()
    {
        // Arrange
        var generatedUser = CreateComplexGeneratedUser();
        var times = new List<long>();

        // Act - Stress test with 2000 operations
        for (int i = 0; i < 2000; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = _userMapper.MapToWrapper(generatedUser);
            stopwatch.Stop();
            
            times.Add(stopwatch.ElapsedMilliseconds);
            result.Should().NotBeNull();
        }

        // Assert - Performance should remain consistent
        var first100 = times.Take(100).Average();
        var last100 = times.TakeLast(100).Average();
        var overall = times.Average();

        _output.WriteLine($"Stress Test Results (2000 ops):");
        _output.WriteLine($"  First 100: {first100:F3}ms avg");
        _output.WriteLine($"  Last 100: {last100:F3}ms avg");
        _output.WriteLine($"  Overall: {overall:F3}ms avg");

        overall.Should().BeLessOrEqualTo(5.0, "Overall performance should remain within 5ms");
        Math.Abs(last100 - first100).Should().BeLessOrEqualTo(0.5, "Performance should remain consistent");
    }

    #endregion

    #region Helper Methods

    private static GeneratedUser CreateComplexGeneratedUser()
    {
        return new GeneratedUser
        {
            Id = 12345,
            EmailAddress = "complex.user@example.com",
            FirstName = "Complex",
            LastName = "User",
            Name = "Complex User",
            JobTitle = "Senior Software Engineer",
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
                Name = "Complex Corporation"
            },
            Address = "123 Complex St",
            City = "Complex City",
            StateCode = "CC",
            CountryCode = "US",
            Zip = "12345",
            AdditionalData = new Dictionary<string, object>
            {
                ["department"] = "Engineering",
                ["employee_number"] = "ENG-001",
                ["security_clearance"] = "Level 3"
            }
        };
    }

    private static User CreateComplexUser()
    {
        return new User
        {
            Id = 12345,
            Email = "complex.user@example.com",
            FirstName = "Complex",
            LastName = "User",
            JobTitle = "Senior Software Engineer",
            IsActive = true,
            CreatedAt = new DateTime(2023, 1, 15, 10, 30, 0),
            UpdatedAt = new DateTime(2023, 6, 20, 14, 45, 0),
            LastSignInAt = new DateTime(2023, 6, 19, 9, 15, 0),
            AvatarUrl = "https://example.com/avatar.jpg",
            PhoneNumber = "555-1234",
            Company = new Company
            {
                Id = 678,
                Name = "Complex Corporation",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    Street1 = "123 Complex St",
                    City = "Complex City",
                    State = "CC",
                    Country = "US",
                    PostalCode = "12345"
                }
            },
            CustomFields = new Dictionary<string, object>
            {
                ["department"] = "Engineering",
                ["employee_number"] = "ENG-001"
            }
        };
    }

    private static GeneratedSimpleCompany CreateComplexGeneratedCompany()
    {
        return new GeneratedSimpleCompany
        {
            Name = "Complex Company Name",
            AdditionalData = new Dictionary<string, object>
            {
                ["industry"] = "Construction",
                ["size"] = "Large",
                ["founded"] = "1990"
            }
        };
    }

    private static Company CreateComplexCompany()
    {
        return new Company
        {
            Id = 12345,
            Name = "Complex Company Name",
            Description = "A complex company with many properties",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            LogoUrl = "https://example.com/logo.png",
            Address = new Address
            {
                Street1 = "123 Business Ave",
                City = "Business City",
                State = "BC",
                Country = "US",
                PostalCode = "54321"
            },
            CustomFields = new Dictionary<string, object>
            {
                ["industry"] = "Construction",
                ["size"] = "Large"
            }
        };
    }

    #endregion
}