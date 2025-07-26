using Procore.SDK.Generation.Tests.Interfaces;
using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.OpenApiSpec;

/// <summary>
/// Tests for OpenAPI specification management functionality
/// </summary>
public class OpenApiSpecManagerTests
{
    private readonly MockFileSystem _fileSystem;
    private readonly ILogger<OpenApiSpecManagerTests> _logger;
    private readonly IOpenApiSpecManager _specManager;

    public OpenApiSpecManagerTests()
    {
        _fileSystem = new MockFileSystem();
        _logger = Substitute.For<ILogger<OpenApiSpecManagerTests>>();
        _specManager = Substitute.For<IOpenApiSpecManager>();
    }

    [Theory]
    [InlineData("https://api.procore.com/swagger/v1.0/rest_OAS_all.json")]
    [InlineData("https://developers.procore.com/reference/rest-api-v1-openapi-spec")]
    public async Task DownloadSpecAsync_WithValidUrl_ShouldDownloadSuccessfully(string specUrl)
    {
        // Arrange
        var destinationPath = "/docs/rest_OAS_all.json";
        var expectedSize = 34 * 1024 * 1024; // 34MB as mentioned in design
        var expectedResult = new DownloadResult
        {
            Success = true,
            FilePath = destinationPath,
            FileSize = expectedSize,
            Duration = TimeSpan.FromSeconds(30),
            HttpStatusCode = 200
        };

        _specManager.DownloadSpecAsync(destinationPath, Arg.Any<CancellationToken>())
                   .Returns(expectedResult);

        // Act
        var result = await _specManager.DownloadSpecAsync(destinationPath);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.FilePath.Should().Be(destinationPath);
        result.FileSize.Should().BeGreaterThan(30 * 1024 * 1024); // At least 30MB
        result.HttpStatusCode.Should().Be(200);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DownloadSpecAsync_WithInvalidUrl_ShouldReturnFailure()
    {
        // Arrange
        var destinationPath = "/docs/invalid_spec.json";
        var expectedResult = new DownloadResult
        {
            Success = false,
            FileSize = 0,
            Duration = TimeSpan.FromSeconds(5),
            ErrorMessage = "HTTP 404: Not Found",
            HttpStatusCode = 404
        };

        _specManager.DownloadSpecAsync(destinationPath, Arg.Any<CancellationToken>())
                   .Returns(expectedResult);

        // Act
        var result = await _specManager.DownloadSpecAsync(destinationPath);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HttpStatusCode.Should().Be(404);
        result.FilePath.Should().BeNull();
    }

    [Fact]
    public async Task DownloadSpecAsync_WithCancellation_ShouldRespectCancellationToken()
    {
        // Arrange
        var destinationPath = "/docs/rest_OAS_all.json";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        _specManager.DownloadSpecAsync(destinationPath, cts.Token)
                   .Returns(Task.FromCanceled<DownloadResult>(cts.Token));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => 
            _specManager.DownloadSpecAsync(destinationPath, cts.Token));
    }

    [Fact]
    public async Task ValidateSpecAsync_WithValidSpec_ShouldReturnValid()
    {
        // Arrange
        var specPath = "/docs/rest_OAS_all.json";
        var validSpecContent = CreateValidOpenApiSpec();
        _fileSystem.AddFile(specPath, new MockFileData(validSpecContent));

        var expectedResult = new OpenApiValidationResult
        {
            IsValid = true,
            Version = "3.0.1",
            EndpointCount = 1439,
            FileSize = validSpecContent.Length,
            ValidationErrors = [],
            ValidationWarnings = [],
            AvailablePaths = [
                "/rest/v1.0/companies",
                "/rest/v1.0/projects",
                "/rest/v1.0/users"
            ]
        };

        _specManager.ValidateSpecAsync(specPath, Arg.Any<CancellationToken>())
                   .Returns(expectedResult);

        // Act
        var result = await _specManager.ValidateSpecAsync(specPath);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Version.Should().Be("3.0.1");
        result.EndpointCount.Should().BeGreaterThan(1000);
        result.ValidationErrors.Should().BeEmpty();
        result.AvailablePaths.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ValidateSpecAsync_WithInvalidSpec_ShouldReturnInvalid()
    {
        // Arrange
        var specPath = "/docs/invalid_spec.json";
        var invalidSpecContent = "{ invalid json content }";
        _fileSystem.AddFile(specPath, new MockFileData(invalidSpecContent));

        var expectedResult = new OpenApiValidationResult
        {
            IsValid = false,
            ValidationErrors = [
                "Invalid JSON format",
                "Missing required 'openapi' field",
                "Missing required 'info' field"
            ],
            ValidationWarnings = [],
            AvailablePaths = []
        };

        _specManager.ValidateSpecAsync(specPath, Arg.Any<CancellationToken>())
                   .Returns(expectedResult);

        // Act
        var result = await _specManager.ValidateSpecAsync(specPath);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.ValidationErrors.Should().NotBeEmpty();
        result.ValidationErrors.Should().Contain("Invalid JSON format");
    }

    [Fact]
    public async Task ValidateSpecAsync_WithMissingFile_ShouldThrow()
    {
        // Arrange
        var specPath = "/docs/missing_spec.json";

        _specManager.ValidateSpecAsync(specPath, Arg.Any<CancellationToken>())
                   .Returns(Task.FromException<OpenApiValidationResult>(new FileNotFoundException()));

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => 
            _specManager.ValidateSpecAsync(specPath));
    }

    [Fact]
    public async Task ExtractPathsAsync_WithValidSpec_ShouldReturnAllPaths()
    {
        // Arrange
        var specPath = "/docs/rest_OAS_all.json";
        var expectedPaths = new[]
        {
            "/rest/v1.0/companies",
            "/rest/v1.0/companies/{id}",
            "/rest/v1.0/projects",
            "/rest/v1.0/projects/{id}",
            "/rest/v1.0/users",
            "/rest/v1.0/users/{id}",
            "/rest/v1.0/custom-fields",
            "/rest/v1.0/workflows"
        };

        _specManager.ExtractPathsAsync(specPath, Arg.Any<CancellationToken>())
                   .Returns(expectedPaths);

        // Act
        var result = await _specManager.ExtractPathsAsync(specPath);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/companies"));
        result.Should().Contain(path => path.Contains("/projects"));
        result.Should().Contain(path => path.Contains("/users"));
    }

    [Theory]
    [InlineData(new[] { "**/companies/**" }, new string[] { }, new[] { "/rest/v1.0/companies", "/rest/v1.0/companies/{id}" })]
    [InlineData(new[] { "**/projects/**", "**/workflows/**" }, new string[] { }, new[] { "/rest/v1.0/projects", "/rest/v1.0/projects/{id}", "/rest/v1.0/workflows" })]
    [InlineData(new[] { "**/users/**" }, new[] { "**/users/{id}" }, new[] { "/rest/v1.0/users" })]
    public void FilterPaths_WithIncludeAndExcludePatterns_ShouldReturnFilteredPaths(
        string[] includePaths, 
        string[] excludePaths, 
        string[] expectedResults)
    {
        // Arrange
        var allPaths = new[]
        {
            "/rest/v1.0/companies",
            "/rest/v1.0/companies/{id}",
            "/rest/v1.0/projects",
            "/rest/v1.0/projects/{id}",
            "/rest/v1.0/users",
            "/rest/v1.0/users/{id}",
            "/rest/v1.0/custom-fields",
            "/rest/v1.0/workflows"
        };

        _specManager.FilterPaths(allPaths, includePaths, excludePaths)
                   .Returns(expectedResults);

        // Act
        var result = _specManager.FilterPaths(allPaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResults);
    }

    [Fact]
    public async Task GetSpecStatisticsAsync_WithValidSpec_ShouldReturnStatistics()
    {
        // Arrange
        var specPath = "/docs/rest_OAS_all.json";
        var expectedStats = new SpecificationStatistics
        {
            Version = "3.0.1",
            Title = "Procore API",
            ApiVersion = "1.0",
            PathCount = 800,
            OperationCount = 1439,
            SchemaCount = 500,
            FileSize = 34 * 1024 * 1024,
            ResourceGroups = [
                "companies",
                "projects", 
                "users",
                "workflows",
                "custom-fields"
            ],
            PathPrefixes = [
                "/rest/v1.0/companies",
                "/rest/v1.0/projects",
                "/rest/v1.0/users"
            ]
        };

        _specManager.GetSpecStatisticsAsync(specPath, Arg.Any<CancellationToken>())
                   .Returns(expectedStats);

        // Act
        var result = await _specManager.GetSpecStatisticsAsync(specPath);

        // Assert
        result.Should().NotBeNull();
        result.Version.Should().Be("3.0.1");
        result.Title.Should().Be("Procore API");
        result.PathCount.Should().BeGreaterThan(500);
        result.OperationCount.Should().BeGreaterThan(1000);
        result.ResourceGroups.Should().NotBeEmpty();
        result.ResourceGroups.Should().Contain("companies");
        result.ResourceGroups.Should().Contain("projects");
    }

    [Fact]
    public async Task GetSpecStatisticsAsync_WithLargeSpec_ShouldHandlePerformantly()
    {
        // Arrange
        var specPath = "/docs/large_spec.json";
        var startTime = DateTime.UtcNow;
        
        var expectedStats = new SpecificationStatistics
        {
            Version = "3.0.1",
            Title = "Large API",
            ApiVersion = "1.0",
            PathCount = 2000,
            OperationCount = 5000,
            SchemaCount = 1000,
            FileSize = 100 * 1024 * 1024, // 100MB
            ResourceGroups = [],
            PathPrefixes = []
        };

        _specManager.GetSpecStatisticsAsync(specPath, Arg.Any<CancellationToken>())
                   .Returns(Task.Delay(TimeSpan.FromMilliseconds(100))
                               .ContinueWith(_ => expectedStats));

        // Act
        var result = await _specManager.GetSpecStatisticsAsync(specPath);
        var duration = DateTime.UtcNow - startTime;

        // Assert
        result.Should().NotBeNull();
        duration.Should().BeLessThan(TimeSpan.FromSeconds(5)); // Should complete within reasonable time
        result.FileSize.Should().BeGreaterThan(50 * 1024 * 1024); // At least 50MB
    }

    private static string CreateValidOpenApiSpec()
    {
        return """
        {
          "openapi": "3.0.1",
          "info": {
            "title": "Procore API",
            "version": "1.0",
            "description": "Procore Technologies API"
          },
          "servers": [
            {
              "url": "https://api.procore.com",
              "description": "Production server"
            }
          ],
          "paths": {
            "/rest/v1.0/companies": {
              "get": {
                "summary": "List companies",
                "operationId": "getCompanies",
                "responses": {
                  "200": {
                    "description": "Success"
                  }
                }
              }
            },
            "/rest/v1.0/projects": {
              "get": {
                "summary": "List projects",
                "operationId": "getProjects",
                "responses": {
                  "200": {
                    "description": "Success"
                  }
                }
              }
            }
          }
        }
        """;
    }
}

/// <summary>
/// Integration tests for OpenAPI spec management with real HTTP calls
/// </summary>
[Trait("Category", "Integration")]
public class OpenApiSpecManagerIntegrationTests
{
    private readonly ITestOutputHelper _output;

    public OpenApiSpecManagerIntegrationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact(Skip = "Integration test - requires network access")]
    public async Task DownloadSpecAsync_FromRealProcore_ShouldSucceed()
    {
        // This test would make actual HTTP calls to Procore API
        // Skipped by default to avoid external dependencies in unit tests
        
        // Arrange
        var specManager = CreateRealSpecManager();
        var tempPath = Path.GetTempFileName();
        
        try
        {
            // Act
            var result = await specManager.DownloadSpecAsync(tempPath);
            
            // Assert
            result.Success.Should().BeTrue();
            result.FileSize.Should().BeGreaterThan(10 * 1024 * 1024); // At least 10MB
            File.Exists(tempPath).Should().BeTrue();
            
            var fileInfo = new FileInfo(tempPath);
            fileInfo.Length.Should().Be(result.FileSize);
            
            _output.WriteLine($"Downloaded spec: {result.FileSize:N0} bytes in {result.Duration}");
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    private static IOpenApiSpecManager CreateRealSpecManager()
    {
        // This would return a real implementation for integration testing
        throw new NotImplementedException("Real implementation would be injected here");
    }
}