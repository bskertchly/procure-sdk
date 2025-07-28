using System;
using System.Linq;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ProjectManagement.Models;
using GeneratedProject = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse;
using GeneratedFlag = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_flag;
using GeneratedCompany = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_company;
using GeneratedProjectType = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_project_type;
using GeneratedProjectStage = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse_project_stage;

namespace Procore.SDK.ProjectManagement.TypeMapping;

/// <summary>
/// Type mapper for converting between wrapper Project domain model and generated Kiota GetResponse type.
/// This serves as the reference implementation demonstrating performance-optimized mapping patterns.
/// </summary>
public class ProjectTypeMapper : BaseTypeMapper<Project, GeneratedProject>
{
    /// <summary>
    /// Maps from generated Kiota GetResponse to wrapper Project domain model.
    /// </summary>
    /// <param name="source">The generated GetResponse to map from</param>
    /// <returns>The mapped Project domain model</returns>
    protected override Project DoMapToWrapper(GeneratedProject source)
    {
        try
        {
            return new Project
            {
                Id = source.Id ?? 0,
                Name = source.Name ?? string.Empty,
                Description = source.Description ?? string.Empty,
                Status = MapGeneratedStatusToWrapper(source.Flag),
                StartDate = MapDateToDateTime(source.StartDate),
                EndDate = MapDateToDateTime(source.CompletionDate),
                CompanyId = source.Company?.Id ?? 0,
                Budget = ParseBudgetAmount(source.TotalValue),
                ProjectType = source.ProjectType?.Name ?? string.Empty,
                Phase = MapToProjectPhase(source.ProjectStage?.Name),
                IsActive = source.Active ?? false,
                CreatedAt = MapDateTime(source.CreatedAt),
                UpdatedAt = MapDateTime(source.UpdatedAt)
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map GetResponse to Project: {ex.Message}",
                ex,
                typeof(GeneratedProject),
                typeof(Project));
        }
    }

    /// <summary>
    /// Maps from wrapper Project domain model to generated Kiota GetResponse.
    /// Note: This is primarily for testing and may not include all generated properties.
    /// </summary>
    /// <param name="source">The Project domain model to map from</param>
    /// <returns>The mapped GetResponse</returns>
    protected override GeneratedProject DoMapToGenerated(Project source)
    {
        try
        {
            return new GeneratedProject
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Flag = MapWrapperStatusToGenerated(source.Status),
                StartDate = source.StartDate.HasValue ? new Microsoft.Kiota.Abstractions.Date(source.StartDate.Value) : null,
                CompletionDate = source.EndDate.HasValue ? new Microsoft.Kiota.Abstractions.Date(source.EndDate.Value) : null,
                TotalValue = source.Budget?.ToString("F2"),
                Active = source.IsActive,
                CreatedAt = source.CreatedAt != DateTime.MinValue ? new DateTimeOffset(source.CreatedAt) : null,
                UpdatedAt = source.UpdatedAt != DateTime.MinValue ? new DateTimeOffset(source.UpdatedAt) : null,
                Company = source.CompanyId > 0 ? new GeneratedCompany { Id = source.CompanyId } : null,
                ProjectType = !string.IsNullOrEmpty(source.ProjectType) ? 
                    new GeneratedProjectType { Name = source.ProjectType } : null,
                ProjectStage = new GeneratedProjectStage { Name = MapProjectPhaseToString(source.Phase) }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map Project to GetResponse: {ex.Message}",
                ex,
                typeof(Project),
                typeof(GeneratedProject));
        }
    }

    /// <summary>
    /// Maps generated flag enum to wrapper project status enum.
    /// </summary>
    private static ProjectStatus MapGeneratedStatusToWrapper(GeneratedFlag? flag)
    {
        return flag switch
        {
            GeneratedFlag.Green => ProjectStatus.Active,
            GeneratedFlag.Yellow => ProjectStatus.OnHold,
            GeneratedFlag.Red => ProjectStatus.Cancelled,
            _ => ProjectStatus.Planning
        };
    }

    /// <summary>
    /// Maps wrapper project status enum to generated flag enum.
    /// </summary>
    private static GeneratedFlag? MapWrapperStatusToGenerated(ProjectStatus status)
    {
        return status switch
        {
            ProjectStatus.Active => GeneratedFlag.Green,
            ProjectStatus.OnHold => GeneratedFlag.Yellow,
            ProjectStatus.Cancelled => GeneratedFlag.Red,
            ProjectStatus.Completed => GeneratedFlag.Green,
            _ => null
        };
    }

    /// <summary>
    /// Maps project stage name to wrapper project phase enum.
    /// </summary>
    private static ProjectPhase MapToProjectPhase(string? stageName)
    {
        if (string.IsNullOrEmpty(stageName))
            return ProjectPhase.PreConstruction;

        return stageName.ToLowerInvariant() switch
        {
            var name when name.Contains("pre") || name.Contains("planning") => ProjectPhase.PreConstruction,
            var name when name.Contains("construction") || name.Contains("build") => ProjectPhase.Construction,
            var name when name.Contains("post") || name.Contains("completion") => ProjectPhase.PostConstruction,
            var name when name.Contains("close") || name.Contains("final") => ProjectPhase.Closeout,
            _ => ProjectPhase.Construction
        };
    }

    /// <summary>
    /// Maps wrapper project phase enum to string for generated type.
    /// </summary>
    private static string MapProjectPhaseToString(ProjectPhase phase)
    {
        return phase switch
        {
            ProjectPhase.PreConstruction => "Pre-Construction",
            ProjectPhase.Construction => "Construction",
            ProjectPhase.PostConstruction => "Post-Construction",
            ProjectPhase.Closeout => "Closeout",
            _ => "Construction"
        };
    }

    /// <summary>
    /// Safely parses budget amount from string representation.
    /// </summary>
    private static decimal? ParseBudgetAmount(string? budgetString)
    {
        if (string.IsNullOrEmpty(budgetString))
            return null;

        // Remove currency symbols and formatting
        var cleanString = budgetString
            .Replace("$", "")
            .Replace(",", "")
            .Trim();

        if (decimal.TryParse(cleanString, out var result))
            return result;

        return null;
    }
}