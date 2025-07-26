using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.Interfaces;

/// <summary>
/// Interface for generating automation scripts
/// </summary>
public interface IScriptGenerator
{
    /// <summary>
    /// Generates a script for automating client generation
    /// </summary>
    /// <param name="configuration">Script generation configuration</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated script content</returns>
    Task<string> GenerateScriptAsync(ScriptGenerationConfiguration configuration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a generated script
    /// </summary>
    /// <param name="scriptPath">Path to the script file</param>
    /// <param name="workingDirectory">Working directory for script execution</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Script execution result</returns>
    Task<ScriptExecutionResult> ExecuteScriptAsync(string scriptPath, string workingDirectory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates that the required script runtime is available
    /// </summary>
    /// <param name="scriptType">Type of script to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Runtime validation result</returns>
    Task<RuntimeValidationResult> ValidateRuntimeAsync(ScriptType scriptType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the default file extension for a script type
    /// </summary>
    /// <param name="scriptType">Script type</param>
    /// <returns>File extension (including dot)</returns>
    string GetFileExtension(ScriptType scriptType);

    /// <summary>
    /// Builds script header with error handling and logging
    /// </summary>
    /// <param name="scriptType">Script type</param>
    /// <param name="includeErrorHandling">Whether to include error handling</param>
    /// <returns>Script header content</returns>
    string BuildScriptHeader(ScriptType scriptType, bool includeErrorHandling = true);
}

/// <summary>
/// Result of script runtime validation
/// </summary>
public record RuntimeValidationResult
{
    /// <summary>
    /// Whether the runtime is available
    /// </summary>
    public required bool IsAvailable { get; init; }

    /// <summary>
    /// Runtime version
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Runtime executable path
    /// </summary>
    public string? ExecutablePath { get; init; }

    /// <summary>
    /// Error message if validation failed
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Supported features/capabilities
    /// </summary>
    public IReadOnlyList<string> SupportedFeatures { get; init; } = [];
}