namespace Procore.SDK.Generation.Tests.Models;

/// <summary>
/// Configuration for Kiota client generation
/// </summary>
public record GenerationConfiguration
{
    /// <summary>
    /// Name of the resource group (e.g., "core", "project-management")
    /// </summary>
    public required string ResourceGroup { get; init; }

    /// <summary>
    /// Generated client class name
    /// </summary>
    public required string ClassName { get; init; }

    /// <summary>
    /// Generated client namespace
    /// </summary>
    public required string Namespace { get; init; }

    /// <summary>
    /// Output directory for generated code
    /// </summary>
    public required string OutputDirectory { get; init; }

    /// <summary>
    /// OpenAPI spec file path
    /// </summary>
    public required string OpenApiSpecPath { get; init; }

    /// <summary>
    /// Paths to include in generation
    /// </summary>
    public IReadOnlyList<string> IncludePaths { get; init; } = [];

    /// <summary>
    /// Paths to exclude from generation
    /// </summary>
    public IReadOnlyList<string> ExcludePaths { get; init; } = [];

    /// <summary>
    /// Whether to exclude backward compatible features
    /// </summary>
    public bool ExcludeBackwardCompatible { get; init; } = true;

    /// <summary>
    /// Whether to clean output directory before generation
    /// </summary>
    public bool CleanOutput { get; init; } = true;

    /// <summary>
    /// Target programming language
    /// </summary>
    public string Language { get; init; } = "CSharp";
}

/// <summary>
/// Result of client generation operation
/// </summary>
public record GenerationResult
{
    /// <summary>
    /// Whether generation was successful
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Generated files
    /// </summary>
    public IReadOnlyList<string> GeneratedFiles { get; init; } = [];

    /// <summary>
    /// Error messages if generation failed
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = [];

    /// <summary>
    /// Warning messages
    /// </summary>
    public IReadOnlyList<string> Warnings { get; init; } = [];

    /// <summary>
    /// Generation duration
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Size of generated code in bytes
    /// </summary>
    public long GeneratedCodeSize { get; init; }
}

/// <summary>
/// Validation result for OpenAPI specification
/// </summary>
public record OpenApiValidationResult
{
    /// <summary>
    /// Whether the OpenAPI spec is valid
    /// </summary>
    public required bool IsValid { get; init; }

    /// <summary>
    /// OpenAPI version
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Number of endpoints in the spec
    /// </summary>
    public int EndpointCount { get; init; }

    /// <summary>
    /// Size of the spec file in bytes
    /// </summary>
    public long FileSize { get; init; }

    /// <summary>
    /// Validation errors
    /// </summary>
    public IReadOnlyList<string> ValidationErrors { get; init; } = [];

    /// <summary>
    /// Validation warnings
    /// </summary>
    public IReadOnlyList<string> ValidationWarnings { get; init; } = [];

    /// <summary>
    /// Available resource groups/paths
    /// </summary>
    public IReadOnlyList<string> AvailablePaths { get; init; } = [];
}

/// <summary>
/// Script generation configuration
/// </summary>
public record ScriptGenerationConfiguration
{
    /// <summary>
    /// Script type (PowerShell or Bash)
    /// </summary>
    public required ScriptType Type { get; init; }

    /// <summary>
    /// Resource configurations to include in script
    /// </summary>
    public IReadOnlyList<GenerationConfiguration> ResourceConfigurations { get; init; } = [];

    /// <summary>
    /// Output file path for the script
    /// </summary>
    public required string OutputPath { get; init; }

    /// <summary>
    /// Whether to include error handling
    /// </summary>
    public bool IncludeErrorHandling { get; init; } = true;

    /// <summary>
    /// Whether to include progress reporting
    /// </summary>
    public bool IncludeProgressReporting { get; init; } = true;

    /// <summary>
    /// Whether to include validation steps
    /// </summary>
    public bool IncludeValidation { get; init; } = true;
}

/// <summary>
/// Script execution result
/// </summary>
public record ScriptExecutionResult
{
    /// <summary>
    /// Whether script executed successfully
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Exit code from script execution
    /// </summary>
    public int ExitCode { get; init; }

    /// <summary>
    /// Standard output from script
    /// </summary>
    public string StandardOutput { get; init; } = string.Empty;

    /// <summary>
    /// Standard error from script
    /// </summary>
    public string StandardError { get; init; } = string.Empty;

    /// <summary>
    /// Execution duration
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Generated clients from script execution
    /// </summary>
    public IReadOnlyList<GenerationResult> GeneratedClients { get; init; } = [];
}

/// <summary>
/// Compilation validation result
/// </summary>
public record CompilationResult
{
    /// <summary>
    /// Whether compilation was successful
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Compilation errors
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = [];

    /// <summary>
    /// Compilation warnings
    /// </summary>
    public IReadOnlyList<string> Warnings { get; init; } = [];

    /// <summary>
    /// Generated assembly path
    /// </summary>
    public string? AssemblyPath { get; init; }

    /// <summary>
    /// Compilation duration
    /// </summary>
    public TimeSpan Duration { get; init; }
}

/// <summary>
/// Script types supported for generation
/// </summary>
public enum ScriptType
{
    PowerShell,
    Bash
}