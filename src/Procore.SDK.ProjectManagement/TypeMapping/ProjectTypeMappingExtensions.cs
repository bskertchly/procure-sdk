using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ProjectManagement.Models;
using GeneratedProject = Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.GetResponse;

namespace Procore.SDK.ProjectManagement.TypeMapping;

/// <summary>
/// Extension methods for convenient type mapping operations specific to ProjectManagement types.
/// </summary>
public static class ProjectTypeMappingExtensions
{
    /// <summary>
    /// Maps a generated GetResponse to a wrapper Project domain model.
    /// </summary>
    /// <param name="source">The generated GetResponse to map from</param>
    /// <param name="mapper">The type mapper to use</param>
    /// <returns>The mapped Project domain model</returns>
    public static Project ToProject(this GeneratedProject source, ITypeMapper<Project, GeneratedProject> mapper)
    {
        return mapper.MapToWrapper(source);
    }

    /// <summary>
    /// Maps a wrapper Project domain model to a generated GetResponse.
    /// </summary>
    /// <param name="source">The Project domain model to map from</param>
    /// <param name="mapper">The type mapper to use</param>
    /// <returns>The mapped GetResponse</returns>
    public static GeneratedProject ToGetResponse(this Project source, ITypeMapper<Project, GeneratedProject> mapper)
    {
        return mapper.MapToGenerated(source);
    }

    /// <summary>
    /// Safely attempts to map a generated GetResponse to a wrapper Project domain model.
    /// </summary>
    /// <param name="source">The generated GetResponse to map from</param>
    /// <param name="mapper">The type mapper to use</param>
    /// <returns>The mapped Project domain model, or null if mapping fails</returns>
    public static Project? TryToProject(this GeneratedProject? source, ITypeMapper<Project, GeneratedProject> mapper)
    {
        if (source == null)
            return null;

        return mapper.TryMapToWrapper(source, out var result) ? result : null;
    }

    /// <summary>
    /// Safely attempts to map a wrapper Project domain model to a generated GetResponse.
    /// </summary>
    /// <param name="source">The Project domain model to map from</param>
    /// <param name="mapper">The type mapper to use</param>
    /// <returns>The mapped GetResponse, or null if mapping fails</returns>
    public static GeneratedProject? TryToGetResponse(this Project? source, ITypeMapper<Project, GeneratedProject> mapper)
    {
        if (source == null)
            return null;

        return mapper.TryMapToGenerated(source, out var result) ? result : null;
    }
}