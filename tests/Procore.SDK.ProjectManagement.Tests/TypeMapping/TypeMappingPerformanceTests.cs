using System;
using System.Diagnostics;
using Xunit;
using FluentAssertions;
using Procore.SDK.ProjectManagement.Models;
using Procore.SDK.ProjectManagement.TypeMapping;
using GeneratedProject = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse;
using GeneratedFlag = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_flag;
using GeneratedCompany = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_company;

namespace Procore.SDK.ProjectManagement.Tests.TypeMapping;

/// <summary>
/// Performance tests to validate that type mapping operations meet the <1ms per conversion requirement.
/// </summary>
public class TypeMappingPerformanceTests
{
    private readonly ProjectTypeMapper _mapper;

    public TypeMappingPerformanceTests()
    {
        _mapper = new ProjectTypeMapper();
    }

    [Fact]
    public void MapToWrapper_Performance_ShouldMeetTargets()
    {
        // Arrange
        var generatedProject = CreateComplexGeneratedProject();
        const int warmupIterations = 100;
        const int testIterations = 10000;

        // Warmup to ensure JIT compilation
        for (int i = 0; i < warmupIterations; i++)
        {
            _mapper.MapToWrapper(generatedProject);
        }

        // Reset metrics after warmup
        _mapper.Metrics.Reset();

        // Act - Measure performance over many iterations
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < testIterations; i++)
        {
            _mapper.MapToWrapper(generatedProject);
        }
        
        stopwatch.Stop();

        // Assert
        var averageTimeMs = (double)stopwatch.ElapsedMilliseconds / testIterations;
        var metricsAverage = _mapper.Metrics.AverageToWrapperTimeMs;

        // Validate against <1ms target
        averageTimeMs.Should().BeLessThan(1.0, 
            $"Average mapping time was {averageTimeMs:F4}ms, should be < 1ms");
        
        // Validate metrics tracking
        _mapper.Metrics.ToWrapperCalls.Should().Be(testIterations);
        _mapper.Metrics.ToWrapperErrors.Should().Be(0);
        
        // Performance validation
        var validation = _mapper.Metrics.ValidatePerformance(targetAverageMs: 1.0);
        validation.ToWrapperPerformanceOk.Should().BeTrue();
        validation.OverallValid.Should().BeTrue();
    }

    [Fact]
    public void MapToGenerated_Performance_ShouldMeetTargets()
    {
        // Arrange
        var project = CreateComplexProject();
        const int warmupIterations = 100;
        const int testIterations = 10000;

        // Warmup to ensure JIT compilation
        for (int i = 0; i < warmupIterations; i++)
        {
            _mapper.MapToGenerated(project);
        }

        // Reset metrics after warmup
        _mapper.Metrics.Reset();

        // Act - Measure performance over many iterations
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < testIterations; i++)
        {
            _mapper.MapToGenerated(project);
        }
        
        stopwatch.Stop();

        // Assert
        var averageTimeMs = (double)stopwatch.ElapsedMilliseconds / testIterations;

        // Validate against <1ms target
        averageTimeMs.Should().BeLessThan(1.0, 
            $"Average mapping time was {averageTimeMs:F4}ms, should be < 1ms");
        
        // Validate metrics tracking
        _mapper.Metrics.ToGeneratedCalls.Should().Be(testIterations);
        _mapper.Metrics.ToGeneratedErrors.Should().Be(0);
        
        // Performance validation
        var validation = _mapper.Metrics.ValidatePerformance(targetAverageMs: 1.0);
        validation.ToGeneratedPerformanceOk.Should().BeTrue();
        validation.OverallValid.Should().BeTrue();
    }

    [Fact]
    public void BidirectionalMapping_Performance_ShouldMeetTargets()
    {
        // Arrange
        var originalProject = CreateComplexProject();
        const int testIterations = 5000; // Fewer iterations due to double conversion

        // Warmup
        for (int i = 0; i < 50; i++)
        {
            var temp = _mapper.MapToGenerated(originalProject);
            _mapper.MapToWrapper(temp);
        }

        // Reset metrics
        _mapper.Metrics.Reset();

        // Act - Measure bidirectional mapping performance
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < testIterations; i++)
        {
            var generated = _mapper.MapToGenerated(originalProject);
            _mapper.MapToWrapper(generated);
        }
        
        stopwatch.Stop();

        // Assert
        var averageTimeMs = (double)stopwatch.ElapsedMilliseconds / (testIterations * 2); // Two operations per iteration

        // Validate against <1ms target per operation
        averageTimeMs.Should().BeLessThan(1.0, 
            $"Average bidirectional mapping time was {averageTimeMs:F4}ms per operation, should be < 1ms");
        
        // Both directions should have been called
        _mapper.Metrics.ToGeneratedCalls.Should().Be(testIterations);
        _mapper.Metrics.ToWrapperCalls.Should().Be(testIterations);
    }

    [Fact]
    public void TryMapping_Performance_ShouldNotBeSignificantlySlower()
    {
        // Arrange
        var generatedProject = CreateComplexGeneratedProject();
        const int testIterations = 10000;

        // Test regular mapping performance
        var regularStopwatch = Stopwatch.StartNew();
        for (int i = 0; i < testIterations; i++)
        {
            _mapper.MapToWrapper(generatedProject);
        }
        regularStopwatch.Stop();

        // Reset and test TryMapping performance
        _mapper.Metrics.Reset();
        
        var tryStopwatch = Stopwatch.StartNew();
        for (int i = 0; i < testIterations; i++)
        {
            _mapper.TryMapToWrapper(generatedProject, out _);
        }
        tryStopwatch.Stop();

        // Assert
        var regularAverageMs = (double)regularStopwatch.ElapsedMilliseconds / testIterations;
        var tryAverageMs = (double)tryStopwatch.ElapsedMilliseconds / testIterations;

        // TryMapping should not be more than 50% slower than regular mapping
        var slowdownRatio = tryAverageMs / regularAverageMs;
        slowdownRatio.Should().BeLessThan(1.5, 
            $"TryMapping is {slowdownRatio:F2}x slower than regular mapping, should be < 1.5x");
        
        // Both should still meet the <1ms target
        tryAverageMs.Should().BeLessThan(1.0, 
            $"TryMapping average time was {tryAverageMs:F4}ms, should be < 1ms");
    }

    [Fact]
    public void ConcurrentMapping_Performance_ShouldRemainStable()
    {
        // Arrange
        var generatedProject = CreateComplexGeneratedProject();
        const int threadsCount = 4;
        const int iterationsPerThread = 2500;
        var tasks = new System.Threading.Tasks.Task[threadsCount];

        // Act - Test concurrent mapping performance
        var stopwatch = Stopwatch.StartNew();
        
        for (int t = 0; t < threadsCount; t++)
        {
            tasks[t] = System.Threading.Tasks.Task.Run(() =>
            {
                for (int i = 0; i < iterationsPerThread; i++)
                {
                    _mapper.MapToWrapper(generatedProject);
                }
            });
        }
        
        System.Threading.Tasks.Task.WaitAll(tasks);
        stopwatch.Stop();

        // Assert
        var totalOperations = threadsCount * iterationsPerThread;
        var averageTimeMs = (double)stopwatch.ElapsedMilliseconds / totalOperations;

        // Should still meet performance targets under concurrent load
        averageTimeMs.Should().BeLessThan(2.0, // Slightly relaxed for concurrent scenarios
            $"Concurrent mapping average time was {averageTimeMs:F4}ms, should be < 2ms");
        
        // Validate metrics accuracy under concurrent access
        _mapper.Metrics.ToWrapperCalls.Should().Be(totalOperations);
        _mapper.Metrics.ToWrapperErrors.Should().Be(0);
    }

    private static GeneratedProject CreateComplexGeneratedProject()
    {
        return new GeneratedProject
        {
            Id = 123456,
            Name = "Complex Test Project with Long Name",
            Description = "Complex test description with multiple lines\nand special characters: !@#$%^&*()",
            Flag = GeneratedFlag.Green,
            StartDate = Microsoft.Kiota.Abstractions.Date.FromDateTime(new DateTime(2024, 1, 15)),
            CompletionDate = Microsoft.Kiota.Abstractions.Date.FromDateTime(new DateTime(2024, 12, 15)),
            Company = new GeneratedCompany 
            { 
                Id = 789, 
                Name = "Complex Test Company Name with Special Characters & Numbers 123" 
            },
            TotalValue = "$12,345,678.90",
            Active = true,
            CreatedAt = new DateTimeOffset(2024, 1, 1, 10, 30, 45, TimeSpan.FromHours(-8)),
            UpdatedAt = new DateTimeOffset(2024, 6, 15, 14, 22, 33, TimeSpan.FromHours(-8)),
            AccountingProjectNumber = "ACC-2024-123456",
            Address = "123 Complex Project Street, Suite 456",
            City = "San Francisco",
            StateCode = "CA",
            CountryCode = "US",
            Zip = "94105",
            Phone = "+1-555-123-4567",
            Latitude = 37.7749f,
            Longitude = -122.4194f
        };
    }

    private static Project CreateComplexProject()
    {
        return new Project
        {
            Id = 123456,
            Name = "Complex Test Project with Long Name",
            Description = "Complex test description with multiple lines\nand special characters: !@#$%^&*()",
            Status = ProjectStatus.Active,
            StartDate = new DateTime(2024, 1, 15),
            EndDate = new DateTime(2024, 12, 15),
            CompanyId = 789,
            Budget = 12345678.90m,
            ProjectType = "Complex Commercial Development",
            Phase = ProjectPhase.Construction,
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1, 10, 30, 45),
            UpdatedAt = new DateTime(2024, 6, 15, 14, 22, 33)
        };
    }
}