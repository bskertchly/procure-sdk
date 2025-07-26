using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.Interfaces;

/// <summary>
/// Interface for integrating generation into build process
/// </summary>
public interface IBuildIntegrationManager
{
    /// <summary>
    /// Integrates client generation into MSBuild targets
    /// </summary>
    /// <param name="projectPath">Path to the project file</param>
    /// <param name="targetName">Name of the MSBuild target to create</param>
    /// <param name="generationConfigurations">Configurations for generation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Integration result</returns>
    Task<BuildIntegrationResult> IntegrateWithMSBuildAsync(
        string projectPath, 
        string targetName, 
        IReadOnlyList<GenerationConfiguration> generationConfigurations,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates build props file for generation configuration
    /// </summary>
    /// <param name="propsFilePath">Path for the props file</param>
    /// <param name="configurations">Generation configurations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Props file content</returns>
    Task<string> CreateBuildPropsAsync(
        string propsFilePath, 
        IReadOnlyList<GenerationConfiguration> configurations,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates build integration setup
    /// </summary>
    /// <param name="projectPath">Path to the project file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<BuildValidationResult> ValidateBuildIntegrationAsync(string projectPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the generation build target
    /// </summary>
    /// <param name="projectPath">Path to the project file</param>
    /// <param name="targetName">Name of the target to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Build execution result</returns>
    Task<BuildExecutionResult> ExecuteBuildTargetAsync(string projectPath, string targetName, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of build integration operation
/// </summary>
public record BuildIntegrationResult
{
    /// <summary>
    /// Whether integration was successful
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Created or modified files
    /// </summary>
    public IReadOnlyList<string> ModifiedFiles { get; init; } = [];

    /// <summary>
    /// Error messages if integration failed
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = [];

    /// <summary>
    /// Warning messages
    /// </summary>
    public IReadOnlyList<string> Warnings { get; init; } = [];

    /// <summary>
    /// Generated MSBuild target content
    /// </summary>
    public string? TargetContent { get; init; }
}

/// <summary>
/// Result of build validation
/// </summary>
public record BuildValidationResult
{
    /// <summary>
    /// Whether build integration is valid
    /// </summary>
    public required bool IsValid { get; init; }

    /// <summary>
    /// Available build targets
    /// </summary>
    public IReadOnlyList<string> AvailableTargets { get; init; } = [];

    /// <summary>
    /// Validation errors
    /// </summary>
    public IReadOnlyList<string> ValidationErrors { get; init; } = [];

    /// <summary>
    /// MSBuild version
    /// </summary>
    public string? MSBuildVersion { get; init; }

    /// <summary>
    /// Project framework version
    /// </summary>
    public string? TargetFramework { get; init; }
}

/// <summary>
/// Result of build target execution
/// </summary>
public record BuildExecutionResult
{
    /// <summary>
    /// Whether build execution was successful
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Exit code from build
    /// </summary>
    public int ExitCode { get; init; }

    /// <summary>
    /// Build output
    /// </summary>
    public string Output { get; init; } = string.Empty;

    /// <summary>
    /// Build errors
    /// </summary>
    public string ErrorOutput { get; init; } = string.Empty;

    /// <summary>
    /// Build duration
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Generated artifacts
    /// </summary>
    public IReadOnlyList<string> GeneratedArtifacts { get; init; } = [];
}