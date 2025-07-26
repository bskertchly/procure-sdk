using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.Interfaces;

/// <summary>
/// Interface for generating Kiota clients
/// </summary>
public interface IKiotaClientGenerator
{
    /// <summary>
    /// Generates a Kiota client based on configuration
    /// </summary>
    /// <param name="configuration">Generation configuration</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generation result</returns>
    Task<GenerationResult> GenerateClientAsync(GenerationConfiguration configuration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates that Kiota CLI is available and properly configured
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<KiotaValidationResult> ValidateKiotaCliAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the version of the installed Kiota CLI
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kiota version information</returns>
    Task<string> GetKiotaVersionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds the Kiota command line arguments for a given configuration
    /// </summary>
    /// <param name="configuration">Generation configuration</param>
    /// <returns>Command line arguments</returns>
    string BuildKiotaArguments(GenerationConfiguration configuration);

    /// <summary>
    /// Validates that generated code compiles successfully
    /// </summary>
    /// <param name="generatedCodePath">Path to generated code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Compilation validation result</returns>
    Task<CompilationResult> ValidateGeneratedCodeAsync(string generatedCodePath, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of Kiota CLI validation
/// </summary>
public record KiotaValidationResult
{
    /// <summary>
    /// Whether Kiota CLI is available and working
    /// </summary>
    public required bool IsAvailable { get; init; }

    /// <summary>
    /// Kiota version
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Supported languages
    /// </summary>
    public IReadOnlyList<string> SupportedLanguages { get; init; } = [];

    /// <summary>
    /// Installation path
    /// </summary>
    public string? InstallationPath { get; init; }

    /// <summary>
    /// Error message if validation failed
    /// </summary>
    public string? ErrorMessage { get; init; }
}