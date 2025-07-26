using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.Interfaces;

/// <summary>
/// Interface for managing OpenAPI specification files
/// </summary>
public interface IOpenApiSpecManager
{
    /// <summary>
    /// Downloads the OpenAPI specification from the Procore API
    /// </summary>
    /// <param name="destinationPath">Path where the spec should be saved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download result with file information</returns>
    Task<DownloadResult> DownloadSpecAsync(string destinationPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates an OpenAPI specification file
    /// </summary>
    /// <param name="specPath">Path to the OpenAPI specification file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<OpenApiValidationResult> ValidateSpecAsync(string specPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts available paths from the OpenAPI specification
    /// </summary>
    /// <param name="specPath">Path to the OpenAPI specification file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of available API paths</returns>
    Task<IReadOnlyList<string>> ExtractPathsAsync(string specPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Filters paths based on inclusion and exclusion patterns
    /// </summary>
    /// <param name="allPaths">All available paths</param>
    /// <param name="includePaths">Paths to include (glob patterns)</param>
    /// <param name="excludePaths">Paths to exclude (glob patterns)</param>
    /// <returns>Filtered paths</returns>
    IReadOnlyList<string> FilterPaths(IReadOnlyList<string> allPaths, IReadOnlyList<string> includePaths, IReadOnlyList<string> excludePaths);

    /// <summary>
    /// Gets statistics about the OpenAPI specification
    /// </summary>
    /// <param name="specPath">Path to the OpenAPI specification file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Specification statistics</returns>
    Task<SpecificationStatistics> GetSpecStatisticsAsync(string specPath, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of OpenAPI spec download operation
/// </summary>
public record DownloadResult
{
    /// <summary>
    /// Whether download was successful
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Downloaded file path
    /// </summary>
    public string? FilePath { get; init; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; init; }

    /// <summary>
    /// Download duration
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Error message if download failed
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// HTTP status code from download request
    /// </summary>
    public int? HttpStatusCode { get; init; }
}

/// <summary>
/// Statistics about an OpenAPI specification
/// </summary>
public record SpecificationStatistics
{
    /// <summary>
    /// OpenAPI specification version
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    /// API title
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// API version
    /// </summary>
    public required string ApiVersion { get; init; }

    /// <summary>
    /// Total number of paths
    /// </summary>
    public int PathCount { get; init; }

    /// <summary>
    /// Total number of operations (endpoints)
    /// </summary>
    public int OperationCount { get; init; }

    /// <summary>
    /// Number of schemas defined
    /// </summary>
    public int SchemaCount { get; init; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; init; }

    /// <summary>
    /// Resource groups available in the API
    /// </summary>
    public IReadOnlyList<string> ResourceGroups { get; init; } = [];

    /// <summary>
    /// Common path prefixes
    /// </summary>
    public IReadOnlyList<string> PathPrefixes { get; init; } = [];
}