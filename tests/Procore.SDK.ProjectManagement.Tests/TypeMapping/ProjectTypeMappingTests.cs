using System;
using Xunit;
using FluentAssertions;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ProjectManagement.Models;
using Procore.SDK.ProjectManagement.TypeMapping;
using GeneratedProject = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse;
using GeneratedFlag = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_flag;
using GeneratedCompany = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_company;
using GeneratedProjectType = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_project_type;
using GeneratedProjectStage = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_project_stage;

namespace Procore.SDK.ProjectManagement.Tests.TypeMapping;

/// <summary>
/// Comprehensive tests for Project type mapping ensuring accurate conversion, performance,
/// and data integrity between wrapper and generated types.
/// </summary>
public class ProjectTypeMappingTests
{
    private readonly ProjectTypeMapper _mapper;

    public ProjectTypeMappingTests()
    {
        _mapper = new ProjectTypeMapper();
    }

    [Fact]
    public void MapToWrapper_WithValidGeneratedProject_ShouldMapCorrectly()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(123);
        result.Name.Should().Be("Test Project");
        result.Description.Should().Be("Test Description");
        result.Status.Should().Be(ProjectStatus.Active);
        result.CompanyId.Should().Be(456);
        result.IsActive.Should().BeTrue();
        result.ProjectType.Should().Be("Commercial");
        result.Phase.Should().Be(ProjectPhase.Construction);
    }

    [Fact]
    public void MapToWrapper_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _mapper.MapToWrapper(null!));
    }

    [Fact]
    public void MapToGenerated_WithValidProject_ShouldMapCorrectly()
    {
        // Arrange
        var project = CreateValidProject();

        // Act
        var result = _mapper.MapToGenerated(project);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(123);
        result.Name.Should().Be("Test Project");
        result.Description.Should().Be("Test Description");
        result.Flag.Should().Be(GeneratedFlag.Green);
        result.Company?.Id.Should().Be(456);
        result.Active.Should().BeTrue();
        result.ProjectType?.Name.Should().Be("Commercial");
        result.ProjectStage?.Name.Should().Be("Construction");
    }

    [Fact]
    public void MapToGenerated_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _mapper.MapToGenerated(null!));
    }

    [Fact]
    public void TryMapToWrapper_WithValidSource_ShouldReturnTrueAndMap()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();

        // Act
        var success = _mapper.TryMapToWrapper(generatedProject, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(123);
        result.Name.Should().Be("Test Project");
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
    public void TryMapToGenerated_WithValidSource_ShouldReturnTrueAndMap()
    {
        // Arrange
        var project = CreateValidProject();

        // Act
        var success = _mapper.TryMapToGenerated(project, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(123);
        result.Name.Should().Be("Test Project");
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

    [Theory]
    [InlineData(GeneratedFlag.Green, ProjectStatus.Active)]
    [InlineData(GeneratedFlag.Yellow, ProjectStatus.OnHold)]
    [InlineData(GeneratedFlag.Red, ProjectStatus.Cancelled)]
    [InlineData(null, ProjectStatus.Planning)]
    public void MapToWrapper_WithDifferentFlags_ShouldMapStatusCorrectly(GeneratedFlag? flag, ProjectStatus expectedStatus)
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        generatedProject.Flag = flag;

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.Status.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(ProjectStatus.Active, GeneratedFlag.Green)]
    [InlineData(ProjectStatus.OnHold, GeneratedFlag.Yellow)]
    [InlineData(ProjectStatus.Cancelled, GeneratedFlag.Red)]
    [InlineData(ProjectStatus.Completed, GeneratedFlag.Green)]
    [InlineData(ProjectStatus.Planning, null)]
    public void MapToGenerated_WithDifferentStatuses_ShouldMapFlagCorrectly(ProjectStatus status, GeneratedFlag? expectedFlag)
    {
        // Arrange
        var project = CreateValidProject();
        project.Status = status;

        // Act
        var result = _mapper.MapToGenerated(project);

        // Assert
        result.Flag.Should().Be(expectedFlag);
    }

    [Fact]
    public void MapToWrapper_WithBudgetString_ShouldParseBudgetCorrectly()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        generatedProject.TotalValue = "$1,234,567.89";

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.Budget.Should().Be(1234567.89m);
    }

    [Fact]
    public void MapToWrapper_WithInvalidBudgetString_ShouldReturnNull()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        generatedProject.TotalValue = "invalid-budget";

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.Budget.Should().BeNull();
    }

    [Fact]
    public void MapToWrapper_WithEmptyBudgetString_ShouldReturnNull()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        generatedProject.TotalValue = "";

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.Budget.Should().BeNull();
    }

    [Theory]
    [InlineData("Pre-Construction Planning", ProjectPhase.PreConstruction)]
    [InlineData("Construction Phase", ProjectPhase.Construction)]
    [InlineData("Post-Construction", ProjectPhase.PostConstruction)]
    [InlineData("Project Closeout", ProjectPhase.Closeout)]
    [InlineData("Unknown Stage", ProjectPhase.Construction)]
    [InlineData(null, ProjectPhase.PreConstruction)]
    public void MapToWrapper_WithDifferentStages_ShouldMapPhaseCorrectly(string? stageName, ProjectPhase expectedPhase)
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        generatedProject.ProjectStage = stageName != null ? new GeneratedProjectStage { Name = stageName } : null;

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.Phase.Should().Be(expectedPhase);
    }

    [Fact]
    public void MapToWrapper_WithNullDates_ShouldHandleGracefully()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        generatedProject.StartDate = null;
        generatedProject.CompletionDate = null;
        generatedProject.CreatedAt = null;
        generatedProject.UpdatedAt = null;

        // Act
        var result = _mapper.MapToWrapper(generatedProject);

        // Assert
        result.StartDate.Should().BeNull();
        result.EndDate.Should().BeNull();
        result.CreatedAt.Should().Be(DateTime.MinValue);
        result.UpdatedAt.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void RoundTripMapping_ShouldPreserveKeyData()
    {
        // Arrange
        var originalProject = CreateValidProject();

        // Act - Map to generated and back
        var generatedProject = _mapper.MapToGenerated(originalProject);
        var roundTripProject = _mapper.MapToWrapper(generatedProject);

        // Assert - Key properties should be preserved
        roundTripProject.Id.Should().Be(originalProject.Id);
        roundTripProject.Name.Should().Be(originalProject.Name);
        roundTripProject.Description.Should().Be(originalProject.Description);
        roundTripProject.CompanyId.Should().Be(originalProject.CompanyId);
        roundTripProject.IsActive.Should().Be(originalProject.IsActive);
        roundTripProject.ProjectType.Should().Be(originalProject.ProjectType);
    }

    [Fact]
    public void Metrics_ShouldTrackPerformance()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        var initialToWrapperCalls = _mapper.Metrics.ToWrapperCalls;

        // Act
        _mapper.MapToWrapper(generatedProject);

        // Assert
        _mapper.Metrics.ToWrapperCalls.Should().Be(initialToWrapperCalls + 1);
        _mapper.Metrics.AverageToWrapperTimeMs.Should().BeGreaterOrEqualTo(0);
        _mapper.Metrics.ToWrapperErrorRate.Should().Be(0); // No errors in successful mapping
    }

    [Fact]
    public void PerformanceTest_ShouldMeetTargets()
    {
        // Arrange
        var generatedProject = CreateValidGeneratedProject();
        const int iterations = 1000;

        // Act - Perform many mappings to get reliable metrics
        for (int i = 0; i < iterations; i++)
        {
            _mapper.MapToWrapper(generatedProject);
        }

        // Assert - Should meet <1ms average target
        var validationResult = _mapper.Metrics.ValidatePerformance(targetAverageMs: 1.0);
        validationResult.ToWrapperPerformanceOk.Should().BeTrue(
            $"Average mapping time was {_mapper.Metrics.AverageToWrapperTimeMs}ms, should be < 1ms");
    }

    private static GeneratedProject CreateValidGeneratedProject()
    {
        return new GeneratedProject
        {
            Id = 123,
            Name = "Test Project",
            Description = "Test Description",
            Flag = GeneratedFlag.Green,
            StartDate = Microsoft.Kiota.Abstractions.Date.FromDateTime(new DateTime(2024, 1, 1)),
            CompletionDate = Microsoft.Kiota.Abstractions.Date.FromDateTime(new DateTime(2024, 12, 31)),
            Company = new GeneratedCompany { Id = 456, Name = "Test Company" },
            TotalValue = "1000000.00",
            Active = true,
            CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero),
            ProjectType = new GeneratedProjectType { Id = 1, Name = "Commercial" },
            ProjectStage = new GeneratedProjectStage { Id = 2, Name = "Construction" }
        };
    }

    private static Project CreateValidProject()
    {
        return new Project
        {
            Id = 123,
            Name = "Test Project",
            Description = "Test Description",
            Status = ProjectStatus.Active,
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 12, 31),
            CompanyId = 456,
            Budget = 1000000.00m,
            ProjectType = "Commercial",
            Phase = ProjectPhase.Construction,
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1),
            UpdatedAt = new DateTime(2024, 1, 2)
        };
    }
}