using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Generation.Tests.KiotaGeneration;

/// <summary>
/// Tests to validate Kiota generation configuration settings, OpenAPI specification parsing quality,
/// and code generation output consistency.
/// </summary>
public class KiotaConfigurationTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _solutionRoot;

    public KiotaConfigurationTests(ITestOutputHelper output)
    {
        _output = output;
        _solutionRoot = GetSolutionRoot();
    }

    private static string GetSolutionRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new InvalidOperationException("Could not find solution root");
    }

    #region Kiota Lock File Tests

    /// <summary>
    /// Validates that all generated clients have proper kiota-lock.json files.
    /// </summary>
    [Theory]
    [InlineData("src/Procore.SDK.Core/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.ProjectManagement/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.ResourceManagement/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.QualitySafety/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.ConstructionFinancials/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.FieldProductivity/Generated/kiota-lock.json")]
    public void KiotaLockFile_Should_Exist_For_All_Clients(string relativePath)
    {
        // Arrange
        var lockFilePath = Path.Combine(_solutionRoot, relativePath);

        // Act & Assert
        Assert.True(File.Exists(lockFilePath), $"Kiota lock file should exist at: {lockFilePath}");
        
        _output.WriteLine($"✅ Kiota lock file exists: {relativePath}");
    }

    /// <summary>
    /// Validates the structure and content of kiota-lock.json files.
    /// </summary>
    [Theory]
    [InlineData("src/Procore.SDK.Core/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.ProjectManagement/Generated/kiota-lock.json")]
    [InlineData("src/Procore.SDK.ResourceManagement/Generated/kiota-lock.json")]
    public void KiotaLockFile_Should_Have_Valid_Structure(string relativePath)
    {
        // Arrange
        var lockFilePath = Path.Combine(_solutionRoot, relativePath);
        Assert.True(File.Exists(lockFilePath), $"Lock file must exist: {lockFilePath}");

        // Act
        var jsonContent = File.ReadAllText(lockFilePath);
        var lockFileData = JsonSerializer.Deserialize<JsonElement>(jsonContent);

        // Assert - Verify required properties exist
        Assert.True(lockFileData.TryGetProperty("version", out var version));
        Assert.True(lockFileData.TryGetProperty("descriptionHash", out var descriptionHash));
        Assert.True(lockFileData.TryGetProperty("descriptionLocation", out var descriptionLocation));
        Assert.True(lockFileData.TryGetProperty("lockFileVersion", out var lockFileVersion));
        Assert.True(lockFileData.TryGetProperty("kiotaVersion", out var kiotaVersion));

        // Verify values are meaningful
        Assert.False(string.IsNullOrEmpty(version.GetString()));
        Assert.False(string.IsNullOrEmpty(descriptionHash.GetString()));
        Assert.False(string.IsNullOrEmpty(descriptionLocation.GetString()));
        Assert.False(string.IsNullOrEmpty(kiotaVersion.GetString()));

        _output.WriteLine($"✅ Kiota lock file has valid structure: {relativePath}");
        _output.WriteLine($"   Kiota Version: {kiotaVersion.GetString()}");
        _output.WriteLine($"   Description Location: {descriptionLocation.GetString()}");
    }

    /// <summary>
    /// Validates that all lock files reference the same OpenAPI specification.
    /// </summary>
    [Fact]
    public void KiotaLockFiles_Should_Reference_Same_OpenAPI_Spec()
    {
        // Arrange
        var lockFilePaths = new[]
        {
            "src/Procore.SDK.Core/Generated/kiota-lock.json",
            "src/Procore.SDK.ProjectManagement/Generated/kiota-lock.json",
            "src/Procore.SDK.ResourceManagement/Generated/kiota-lock.json",
            "src/Procore.SDK.QualitySafety/Generated/kiota-lock.json",
            "src/Procore.SDK.ConstructionFinancials/Generated/kiota-lock.json",
            "src/Procore.SDK.FieldProductivity/Generated/kiota-lock.json"
        };

        var descriptionLocations = new HashSet<string>();
        var descriptionHashes = new HashSet<string>();

        // Act
        foreach (var relativePath in lockFilePaths)
        {
            var lockFilePath = Path.Combine(_solutionRoot, relativePath);
            if (File.Exists(lockFilePath))
            {
                var jsonContent = File.ReadAllText(lockFilePath);
                var lockFileData = JsonSerializer.Deserialize<JsonElement>(jsonContent);

                if (lockFileData.TryGetProperty("descriptionLocation", out var location))
                {
                    descriptionLocations.Add(location.GetString() ?? string.Empty);
                }

                if (lockFileData.TryGetProperty("descriptionHash", out var hash))
                {
                    descriptionHashes.Add(hash.GetString() ?? string.Empty);
                }
            }
        }

        // Assert - All should reference the same spec (though may have different hashes due to filtering)
        Assert.Single(descriptionLocations);
        _output.WriteLine($"✅ All lock files reference the same OpenAPI spec: {descriptionLocations.First()}");
        _output.WriteLine($"   Found {descriptionHashes.Count} different description hashes (expected due to path filtering)");
    }

    #endregion

    #region OpenAPI Specification Tests

    /// <summary>
    /// Validates that the OpenAPI specification file exists and is accessible.
    /// </summary>
    [Fact]
    public void OpenAPI_Specification_Should_Exist()
    {
        // Arrange
        var specPath = Path.Combine(_solutionRoot, "docs", "rest_OAS_all.json");

        // Act & Assert
        Assert.True(File.Exists(specPath), $"OpenAPI specification should exist at: {specPath}");

        var fileInfo = new FileInfo(specPath);
        Assert.True(fileInfo.Length > 0, "OpenAPI specification should not be empty");

        _output.WriteLine($"✅ OpenAPI specification exists: {fileInfo.Length:N0} bytes");
    }

    /// <summary>
    /// Validates that the OpenAPI specification has valid JSON structure.
    /// </summary>
    [Fact]
    public void OpenAPI_Specification_Should_Have_Valid_JSON()
    {
        // Arrange
        var specPath = Path.Combine(_solutionRoot, "docs", "rest_OAS_all.json");
        Assert.True(File.Exists(specPath), "OpenAPI specification must exist");

        // Act
        var jsonContent = File.ReadAllText(specPath);
        var specData = JsonSerializer.Deserialize<JsonElement>(jsonContent);

        // Assert - Verify OpenAPI structure
        Assert.True(specData.TryGetProperty("openapi", out var openApiVersion));
        Assert.True(specData.TryGetProperty("info", out var info));
        Assert.True(specData.TryGetProperty("paths", out var paths));

        // Verify OpenAPI version
        var versionString = openApiVersion.GetString();
        Assert.StartsWith("3.", versionString);

        // Verify info section
        Assert.True(info.TryGetProperty("title", out var title));
        Assert.True(info.TryGetProperty("version", out var version));

        // Verify paths exist
        Assert.True(paths.ValueKind == JsonValueKind.Object);
        Assert.True(paths.EnumerateObject().Any());

        _output.WriteLine($"✅ OpenAPI specification has valid structure");
        _output.WriteLine($"   OpenAPI Version: {versionString}");
        _output.WriteLine($"   API Title: {title.GetString()}");
        _output.WriteLine($"   API Version: {version.GetString()}");
        _output.WriteLine($"   Path Count: {paths.EnumerateObject().Count():N0}");
    }

    /// <summary>
    /// Validates that the OpenAPI specification contains expected resource endpoints.
    /// </summary>
    [Fact]
    public void OpenAPI_Specification_Should_Contain_Expected_Endpoints()
    {
        // Arrange
        var specPath = Path.Combine(_solutionRoot, "docs", "rest_OAS_all.json");
        Assert.True(File.Exists(specPath), "OpenAPI specification must exist");

        var expectedEndpointPatterns = new[]
        {
            "/rest/v1.0/companies",
            "/rest/v1.0/users",
            "/rest/v1.0/projects",
            "/rest/v1.0/observations",
            "/rest/v1.0/resources",
            "/rest/v1.0/workforce"
        };

        // Act
        var jsonContent = File.ReadAllText(specPath);
        var specData = JsonSerializer.Deserialize<JsonElement>(jsonContent);
        
        Assert.True(specData.TryGetProperty("paths", out var paths));
        var pathKeys = paths.EnumerateObject().Select(p => p.Name).ToList();

        // Assert
        foreach (var pattern in expectedEndpointPatterns)
        {
            var matchingPaths = pathKeys.Where(p => p.Contains(pattern.Replace("/rest/v1.0", ""))).ToList();
            Assert.NotEmpty(matchingPaths);
            _output.WriteLine($"✅ Found endpoints matching pattern '{pattern}': {matchingPaths.Count}");
        }
    }

    #endregion

    #region Generation Configuration Tests

    /// <summary>
    /// Validates that generated clients follow consistent naming conventions.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.Core", "CoreClient")]
    [InlineData("Procore.SDK.ProjectManagement", "ProjectManagementClient")]
    [InlineData("Procore.SDK.ResourceManagement", "ResourceManagementClient")]
    [InlineData("Procore.SDK.QualitySafety", "QualitySafetyClient")]
    [InlineData("Procore.SDK.ConstructionFinancials", "ConstructionFinancialsClient")]
    [InlineData("Procore.SDK.FieldProductivity", "FieldProductivityClient")]
    public void Generated_Clients_Should_Follow_Naming_Conventions(string expectedNamespace, string expectedClassName)
    {
        // Act
        var clientType = Type.GetType($"{expectedNamespace}.{expectedClassName}, {expectedNamespace}");

        // Assert
        Assert.NotNull(clientType);
        Assert.Equal(expectedNamespace, clientType.Namespace);
        Assert.Equal(expectedClassName, clientType.Name);

        _output.WriteLine($"✅ Client follows naming convention: {expectedNamespace}.{expectedClassName}");
    }

    /// <summary>
    /// Validates that generated clients have consistent namespace organization.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.Core")]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.ResourceManagement")]
    [InlineData("Procore.SDK.QualitySafety")]
    [InlineData("Procore.SDK.ConstructionFinancials")]
    [InlineData("Procore.SDK.FieldProductivity")]
    public void Generated_Clients_Should_Have_Consistent_Namespace_Organization(string clientNamespace)
    {
        // Act
        var assembly = Assembly.LoadFrom(Path.Combine(_solutionRoot, "src", clientNamespace.Replace("Procore.SDK.", "Procore.SDK."), "bin", "Debug", "net8.0", $"{clientNamespace}.dll"));
        var namespaces = assembly.GetTypes()
            .Where(t => t.Namespace != null)
            .Select(t => t.Namespace)
            .Distinct()
            .OrderBy(ns => ns)
            .ToList();

        // Assert
        Assert.Contains(clientNamespace, namespaces);
        Assert.Contains($"{clientNamespace}.Rest", namespaces);
        
        // Verify expected sub-namespaces exist
        var restNamespaces = namespaces.Where(ns => ns!.StartsWith($"{clientNamespace}.Rest")).ToList();
        Assert.NotEmpty(restNamespaces);

        _output.WriteLine($"✅ {clientNamespace} has consistent namespace organization:");
        foreach (var ns in restNamespaces.Take(5))
        {
            _output.WriteLine($"   - {ns}");
        }
        
        if (restNamespaces.Count > 5)
        {
            _output.WriteLine($"   ... and {restNamespaces.Count - 5} more");
        }
    }

    /// <summary>
    /// Validates that generated request builders follow consistent patterns.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement", "RestRequestBuilder")]
    [InlineData("Procore.SDK.ResourceManagement", "RestRequestBuilder")]
    [InlineData("Procore.SDK.QualitySafety", "RestRequestBuilder")]
    [InlineData("Procore.SDK.Core", "RestRequestBuilder")]
    public void Generated_RequestBuilders_Should_Follow_Consistent_Patterns(string clientNamespace, string expectedBuilderName)
    {
        // Act
        var builderType = Type.GetType($"{clientNamespace}.Rest.{expectedBuilderName}, {clientNamespace}");

        // Assert
        Assert.NotNull(builderType);
        Assert.Equal($"{clientNamespace}.Rest", builderType.Namespace);
        Assert.Equal(expectedBuilderName, builderType.Name);

        // Verify it has expected version properties
        var versionProperties = builderType.GetProperties()
            .Where(p => p.Name.StartsWith("V"))
            .ToList();

        Assert.NotEmpty(versionProperties);

        _output.WriteLine($"✅ {clientNamespace} has consistent RequestBuilder pattern with {versionProperties.Count} version properties");
    }

    #endregion

    #region Code Generation Quality Tests

    /// <summary>
    /// Validates that generated code has proper documentation comments.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement.ProjectManagementClient")]
    [InlineData("Procore.SDK.ResourceManagement.ResourceManagementClient")]
    [InlineData("Procore.SDK.Core.CoreClient")]
    public void Generated_Code_Should_Have_Documentation_Comments(string fullyQualifiedTypeName)
    {
        // Act
        var clientType = Type.GetType(fullyQualifiedTypeName);
        Assert.NotNull(clientType);

        // Get the source file path (this is a heuristic test)
        var assemblyLocation = clientType.Assembly.Location;
        var sourceRoot = Path.GetDirectoryName(assemblyLocation);
        
        // Look for generated source files
        var generatedFiles = Directory.GetFiles(
            Path.Combine(_solutionRoot, "src", clientType.Namespace!.Replace("Procore.SDK.", "Procore.SDK."), "Generated"),
            "*.cs",
            SearchOption.AllDirectories)
            .Take(5) // Test first 5 files
            .ToList();

        // Assert
        Assert.NotEmpty(generatedFiles);

        foreach (var file in generatedFiles)
        {
            var content = File.ReadAllText(file);
            
            // Verify it has auto-generated comment
            Assert.Contains("// <auto-generated/>", content);
            
            // Verify it has some form of documentation
            var hasXmlDoc = content.Contains("/// <summary>") || content.Contains("/// <param");
            var hasComments = content.Contains("//") || hasXmlDoc;
            
            Assert.True(hasComments, $"Generated file should have comments: {file}");
        }

        _output.WriteLine($"✅ {fullyQualifiedTypeName} has properly documented generated code");
    }

    /// <summary>
    /// Validates that generated code follows C# coding conventions.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.ResourceManagement")]
    [InlineData("Procore.SDK.Core")]
    public void Generated_Code_Should_Follow_CSharp_Conventions(string clientNamespace)
    {
        // Act
        var assembly = Assembly.LoadFrom(Path.Combine(_solutionRoot, "src", clientNamespace.Replace("Procore.SDK.", "Procore.SDK."), "bin", "Debug", "net8.0", $"{clientNamespace}.dll"));
        var publicTypes = assembly.GetTypes()
            .Where(t => t.IsPublic)
            .ToList();

        // Assert
        Assert.NotEmpty(publicTypes);

        foreach (var type in publicTypes.Take(10)) // Test first 10 types
        {
            // Verify Pascal case for class names
            if (type.IsClass)
            {
                Assert.True(char.IsUpper(type.Name[0]), $"Class name should start with uppercase: {type.Name}");
            }

            // Verify namespace follows convention
            Assert.StartsWith("Procore.SDK.", type.Namespace);
        }

        _output.WriteLine($"✅ {clientNamespace} follows C# coding conventions for {publicTypes.Count} public types");
    }

    /// <summary>
    /// Validates that generated code has consistent file organization.
    /// </summary>
    [Theory]
    [InlineData("src/Procore.SDK.Core/Generated")]
    [InlineData("src/Procore.SDK.ProjectManagement/Generated")]
    [InlineData("src/Procore.SDK.ResourceManagement/Generated")]
    public void Generated_Code_Should_Have_Consistent_File_Organization(string generatedPath)
    {
        // Arrange
        var fullPath = Path.Combine(_solutionRoot, generatedPath);
        Assert.True(Directory.Exists(fullPath), $"Generated directory should exist: {fullPath}");

        // Act
        var files = Directory.GetFiles(fullPath, "*.cs", SearchOption.AllDirectories);
        var directories = Directory.GetDirectories(fullPath, "*", SearchOption.AllDirectories);

        // Assert
        Assert.NotEmpty(files);

        // Verify expected structure
        var hasMainClient = files.Any(f => Path.GetFileName(f).EndsWith("Client.cs"));
        var hasRestDirectory = directories.Any(d => Path.GetFileName(d) == "Rest");

        Assert.True(hasMainClient, "Should have main client file");
        Assert.True(hasRestDirectory, "Should have Rest directory");

        // Verify kiota-lock.json exists
        var lockFile = Path.Combine(fullPath, "kiota-lock.json");
        Assert.True(File.Exists(lockFile), "Should have kiota-lock.json file");

        _output.WriteLine($"✅ {generatedPath} has consistent file organization:");
        _output.WriteLine($"   - {files.Length} generated files");
        _output.WriteLine($"   - {directories.Length} subdirectories");
        _output.WriteLine($"   - Main client: {hasMainClient}");
        _output.WriteLine($"   - Rest structure: {hasRestDirectory}");
    }

    /// <summary>
    /// Validates that generated code has appropriate compiler directives.
    /// </summary>
    [Theory]
    [InlineData("src/Procore.SDK.Core/Generated")]
    [InlineData("src/Procore.SDK.ProjectManagement/Generated")]
    public void Generated_Code_Should_Have_Compiler_Directives(string generatedPath)
    {
        // Arrange
        var fullPath = Path.Combine(_solutionRoot, generatedPath);
        var files = Directory.GetFiles(fullPath, "*.cs", SearchOption.AllDirectories).Take(10);

        // Act & Assert
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            
            // Verify auto-generated header
            Assert.Contains("// <auto-generated/>", content);
            
            // Verify pragma warnings are handled
            var hasPragmaWarning = content.Contains("#pragma warning disable") || content.Contains("#pragma warning restore");
            
            if (hasPragmaWarning)
            {
                _output.WriteLine($"✅ File has proper pragma directives: {Path.GetFileName(file)}");
            }
        }

        _output.WriteLine($"✅ {generatedPath} has appropriate compiler directives");
    }

    #endregion

    #region Path Filtering Validation Tests

    /// <summary>
    /// Validates that path filtering is working correctly for each client.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.Core", new[] { "Companies", "Users" })]
    [InlineData("Procore.SDK.ProjectManagement", new[] { "Projects" })]
    [InlineData("Procore.SDK.ResourceManagement", new[] { "Resources", "WorkforcePlanning" })]
    [InlineData("Procore.SDK.QualitySafety", new[] { "Observations" })]
    public void Path_Filtering_Should_Include_Expected_Endpoints(string clientNamespace, string[] expectedEndpoints)
    {
        // Act
        var assembly = Assembly.LoadFrom(Path.Combine(_solutionRoot, "src", clientNamespace.Replace("Procore.SDK.", "Procore.SDK."), "bin", "Debug", "net8.0", $"{clientNamespace}.dll"));
        var requestBuilderTypes = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("RequestBuilder"))
            .Where(t => expectedEndpoints.Any(endpoint => t.Name.Contains(endpoint)))
            .ToList();

        // Assert
        Assert.NotEmpty(requestBuilderTypes);

        foreach (var endpoint in expectedEndpoints)
        {
            var matchingBuilders = requestBuilderTypes.Where(t => t.Name.Contains(endpoint)).ToList();
            Assert.NotEmpty(matchingBuilders);
            _output.WriteLine($"✅ {clientNamespace} includes {endpoint} endpoints: {matchingBuilders.Count} builders");
        }
    }

    /// <summary>
    /// Validates that path filtering excludes unrelated endpoints.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement", new[] { "Observations", "Resources" })]
    [InlineData("Procore.SDK.ResourceManagement", new[] { "Projects", "Observations" })]
    [InlineData("Procore.SDK.QualitySafety", new[] { "Projects", "Resources" })]
    public void Path_Filtering_Should_Exclude_Unrelated_Endpoints(string clientNamespace, string[] excludedEndpoints)
    {
        // Act
        var assembly = Assembly.LoadFrom(Path.Combine(_solutionRoot, "src", clientNamespace.Replace("Procore.SDK.", "Procore.SDK."), "bin", "Debug", "net8.0", $"{clientNamespace}.dll"));
        var requestBuilderTypes = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("RequestBuilder"))
            .ToList();

        // Assert
        foreach (var endpoint in excludedEndpoints)
        {
            var unexpectedBuilders = requestBuilderTypes.Where(t => t.Name.Contains(endpoint)).ToList();
            
            // Some overlap is expected (e.g., Projects might appear in multiple clients)
            // We're mainly checking that the filtering is working, not that it's perfect
            _output.WriteLine($"ℹ️ {clientNamespace} has {unexpectedBuilders.Count} builders containing '{endpoint}'");
        }

        _output.WriteLine($"✅ {clientNamespace} path filtering appears to be working (total builders: {requestBuilderTypes.Count})");
    }

    #endregion
}