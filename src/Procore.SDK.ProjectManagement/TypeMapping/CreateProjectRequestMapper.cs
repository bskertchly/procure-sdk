using System;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ProjectManagement.Models;
using GeneratedPatchRequest = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.PatchRequestBody;
using GeneratedPatchProject = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.PatchRequestBody_project;

namespace Procore.SDK.ProjectManagement.TypeMapping;

/// <summary>
/// Type mapper for converting wrapper CreateProjectRequest to generated Kiota PatchRequestBody.
/// Handles the mapping from domain request models to generated API request structures.
/// </summary>
public class CreateProjectRequestMapper : BaseTypeMapper<CreateProjectRequest, GeneratedPatchRequest>
{
    /// <summary>
    /// Maps from generated PatchRequestBody to wrapper CreateProjectRequest.
    /// Note: This reverse mapping is primarily for testing scenarios.
    /// </summary>
    /// <param name="source">The generated PatchRequestBody to map from</param>
    /// <returns>The mapped CreateProjectRequest</returns>
    protected override CreateProjectRequest DoMapToWrapper(GeneratedPatchRequest source)
    {
        try
        {
            var project = source.Project;
            
            return new CreateProjectRequest
            {
                Name = project?.Name ?? string.Empty,
                Description = project?.Description ?? string.Empty,
                StartDate = MapDateToDateTime(project?.StartDate),
                EndDate = MapDateToDateTime(project?.CompletionDate),
                Budget = project?.TotalValue != null ? (decimal)project.TotalValue.Value : null,
                ProjectType = string.Empty // Project type mapping not available in PatchRequestBody_project
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map PatchRequestBody to CreateProjectRequest: {ex.Message}",
                ex,
                typeof(GeneratedPatchRequest),
                typeof(CreateProjectRequest));
        }
    }

    /// <summary>
    /// Maps from wrapper CreateProjectRequest to generated PatchRequestBody.
    /// </summary>
    /// <param name="source">The CreateProjectRequest to map from</param>
    /// <returns>The mapped PatchRequestBody</returns>
    protected override GeneratedPatchRequest DoMapToGenerated(CreateProjectRequest source)
    {
        try
        {
            return new GeneratedPatchRequest
            {
                Project = new GeneratedPatchProject
                {
                    Name = source.Name,
                    Description = source.Description,
                    StartDate = source.StartDate.HasValue ? 
                        new Microsoft.Kiota.Abstractions.Date(source.StartDate.Value) : null,
                    CompletionDate = source.EndDate.HasValue ? 
                        new Microsoft.Kiota.Abstractions.Date(source.EndDate.Value) : null,
                    TotalValue = source.Budget.HasValue ? (float)source.Budget.Value : null
                }
            };
        }
        catch (Exception ex)
        {
            throw new TypeMappingException(
                $"Failed to map CreateProjectRequest to PatchRequestBody: {ex.Message}",
                ex,
                typeof(CreateProjectRequest),
                typeof(GeneratedPatchRequest));
        }
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