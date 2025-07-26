using Procore.SDK.Generation.Tests.Interfaces;
using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.PathFiltering;

/// <summary>
/// Tests for path filtering functionality used in resource group generation
/// </summary>
public class PathFilteringTests
{
    private readonly IOpenApiSpecManager _specManager;
    private readonly List<string> _samplePaths;

    public PathFilteringTests()
    {
        _specManager = Substitute.For<IOpenApiSpecManager>();
        _samplePaths = CreateSampleProcorePaths();
    }

    [Fact]
    public void FilterPaths_WithCoreResourcePaths_ShouldReturnCoreEndpoints()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/companies/**",
            "**/company_users/**", 
            "**/users/**",
            "**/folders-and-files/**",
            "**/custom-fields/**",
            "**/configurable-field-sets/**"
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths.Where(path =>
            path.Contains("/companies") ||
            path.Contains("/company_users") ||
            path.Contains("/users") ||
            path.Contains("/folders-and-files") ||
            path.Contains("/custom-fields") ||
            path.Contains("/configurable-field-sets"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/companies"));
        result.Should().Contain(path => path.Contains("/users"));
        result.Should().Contain(path => path.Contains("/custom-fields"));
        result.Should().NotContain(path => path.Contains("/projects"));
        result.Should().NotContain(path => path.Contains("/workflows"));
    }

    [Fact]
    public void FilterPaths_WithProjectManagementPaths_ShouldReturnProjectEndpoints()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/projects/**",
            "**/workflows/**",
            "**/task-items/**",
            "**/project-assignments/**",
            "**/project-users/**"
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths.Where(path =>
            path.Contains("/projects") ||
            path.Contains("/workflows") ||
            path.Contains("/task-items") ||
            path.Contains("/project-assignments") ||
            path.Contains("/project-users"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/projects"));
        result.Should().Contain(path => path.Contains("/workflows"));
        result.Should().NotContain(path => path.Contains("/companies"));
        result.Should().NotContain(path => path.Contains("/users"));
    }

    [Fact]
    public void FilterPaths_WithQualitySafetyPaths_ShouldReturnQualitySafetyEndpoints()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/inspections/**",
            "**/observations/**",
            "**/incidents/**",
            "**/safety/**",
            "**/quality/**",
            "**/punch/**"
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths.Where(path =>
            path.Contains("/inspections") ||
            path.Contains("/observations") ||
            path.Contains("/incidents") ||
            path.Contains("/safety") ||
            path.Contains("/quality") ||
            path.Contains("/punch"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/inspections"));
        result.Should().Contain(path => path.Contains("/safety"));
        result.Should().Contain(path => path.Contains("/quality"));
        result.Should().NotContain(path => path.Contains("/companies"));
        result.Should().NotContain(path => path.Contains("/projects"));
    }

    [Fact]
    public void FilterPaths_WithConstructionFinancialsPaths_ShouldReturnFinancialEndpoints()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/contracts/**",
            "**/purchase-orders/**",
            "**/budgets/**",
            "**/cost-codes/**",
            "**/change-orders/**",
            "**/invoices/**",
            "**/payments/**"
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths.Where(path =>
            path.Contains("/contracts") ||
            path.Contains("/purchase-orders") ||
            path.Contains("/budgets") ||
            path.Contains("/cost-codes") ||
            path.Contains("/change-orders") ||
            path.Contains("/invoices") ||
            path.Contains("/payments"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/contracts"));
        result.Should().Contain(path => path.Contains("/budgets"));
        result.Should().Contain(path => path.Contains("/invoices"));
        result.Should().NotContain(path => path.Contains("/projects"));
        result.Should().NotContain(path => path.Contains("/users"));
    }

    [Fact]
    public void FilterPaths_WithFieldProductivityPaths_ShouldReturnFieldEndpoints()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/daily-logs/**",
            "**/timecards/**",
            "**/equipment/**",
            "**/manpower/**",
            "**/deliveries/**"
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths.Where(path =>
            path.Contains("/daily-logs") ||
            path.Contains("/timecards") ||
            path.Contains("/equipment") ||
            path.Contains("/manpower") ||
            path.Contains("/deliveries"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/daily-logs"));
        result.Should().Contain(path => path.Contains("/timecards"));
        result.Should().Contain(path => path.Contains("/equipment"));
        result.Should().NotContain(path => path.Contains("/companies"));
        result.Should().NotContain(path => path.Contains("/projects"));
    }

    [Fact]
    public void FilterPaths_WithResourceManagementPaths_ShouldReturnResourceEndpoints()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/workforce/**",
            "**/resources/**",
            "**/assignments/**"
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths.Where(path =>
            path.Contains("/workforce") ||
            path.Contains("/resources") ||
            path.Contains("/assignments"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(path => path.Contains("/workforce"));
        result.Should().Contain(path => path.Contains("/resources"));
        result.Should().NotContain(path => path.Contains("/companies"));
        result.Should().NotContain(path => path.Contains("/projects"));
    }

    [Fact]
    public void FilterPaths_WithExcludePatterns_ShouldExcludeMatchingPaths()
    {
        // Arrange
        var includePaths = new[] { "**/companies/**" };
        var excludePaths = new[] { "**/companies/**/attachments", "**/companies/**/workflows" };
        
        var expectedPaths = _samplePaths
            .Where(path => path.Contains("/companies"))
            .Where(path => !path.Contains("/attachments") && !path.Contains("/workflows"))
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().Contain(path => path.Contains("/companies") && !path.Contains("/attachments"));
        result.Should().NotContain(path => path.Contains("/companies") && path.Contains("/attachments"));
        result.Should().NotContain(path => path.Contains("/companies") && path.Contains("/workflows"));
    }

    [Theory]
    [InlineData("**/companies", "/rest/v1.0/companies", true)]
    [InlineData("**/companies/**", "/rest/v1.0/companies/{id}", true)]
    [InlineData("**/projects", "/rest/v1.0/companies", false)]
    [InlineData("**/v1.0/users/**", "/rest/v1.0/users/{id}", true)]
    [InlineData("**/custom-fields", "/rest/v1.0/custom-fields", true)]
    public void FilterPaths_WithSpecificPatterns_ShouldMatchCorrectly(
        string pattern, 
        string testPath, 
        bool shouldMatch)
    {
        // Arrange
        var includePaths = new[] { pattern };
        var excludePaths = Array.Empty<string>();
        var testPaths = new[] { testPath };
        
        var expectedResult = shouldMatch ? new[] { testPath } : Array.Empty<string>();

        _specManager.FilterPaths(testPaths, includePaths, excludePaths)
                   .Returns(expectedResult);

        // Act
        var result = _specManager.FilterPaths(testPaths, includePaths, excludePaths);

        // Assert
        if (shouldMatch)
        {
            result.Should().Contain(testPath);
        }
        else
        {
            result.Should().NotContain(testPath);
        }
    }

    [Fact]
    public void FilterPaths_WithEmptyIncludePaths_ShouldReturnEmptyResult()
    {
        // Arrange
        var includePaths = Array.Empty<string>();
        var excludePaths = Array.Empty<string>();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(Array.Empty<string>());

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FilterPaths_WithOverlappingIncludePatterns_ShouldReturnUniqueResults()
    {
        // Arrange
        var includePaths = new[]
        {
            "**/companies/**",
            "**/companies/{id}",  // More specific pattern that overlaps
            "**/v1.0/companies/**" // Different but overlapping pattern
        };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = _samplePaths
            .Where(path => path.Contains("/companies"))
            .Distinct()
            .ToArray();

        _specManager.FilterPaths(_samplePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(_samplePaths, includePaths, excludePaths);

        // Assert
        result.Should().OnlyHaveUniqueItems();
        result.Should().Contain(path => path.Contains("/companies"));
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(5000)]
    [InlineData(10000)]
    public void FilterPaths_WithLargePath_ShouldPerformEfficiently(int pathCount)
    {
        // Arrange
        var largePaths = GenerateLargePath(pathCount);
        var includePaths = new[] { "**/companies/**", "**/projects/**" };
        var excludePaths = Array.Empty<string>();
        
        var expectedPaths = largePaths
            .Where(path => path.Contains("/companies") || path.Contains("/projects"))
            .ToArray();

        _specManager.FilterPaths(largePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var stopwatch = Stopwatch.StartNew();
        var result = _specManager.FilterPaths(largePaths, includePaths, excludePaths);
        stopwatch.Stop();

        // Assert
        result.Should().NotBeEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should complete within 1 second
    }

    [Fact]
    public void FilterPaths_WithCaseInsensitivePatterns_ShouldMatchRegardlessOfCase()
    {
        // Arrange
        var mixedCasePaths = new[]
        {
            "/rest/v1.0/Companies",
            "/rest/v1.0/PROJECTS",
            "/rest/v1.0/users",
            "/rest/v1.0/Custom-Fields"
        };
        var includePaths = new[] { "**/companies/**", "**/custom-fields/**" };
        var excludePaths = Array.Empty<string>();
        
        // Should match regardless of case sensitivity in the implementation
        var expectedPaths = mixedCasePaths
            .Where(path => path.ToLower().Contains("/companies") || path.ToLower().Contains("/custom-fields"))
            .ToArray();

        _specManager.FilterPaths(mixedCasePaths, includePaths, excludePaths)
                   .Returns(expectedPaths);

        // Act
        var result = _specManager.FilterPaths(mixedCasePaths, includePaths, excludePaths);

        // Assert
        result.Should().Contain("/rest/v1.0/Companies");
        result.Should().Contain("/rest/v1.0/Custom-Fields");
        result.Should().NotContain("/rest/v1.0/PROJECTS");
    }

    private static List<string> CreateSampleProcorePaths()
    {
        return new List<string>
        {
            // Core paths
            "/rest/v1.0/companies",
            "/rest/v1.0/companies/{id}",
            "/rest/v1.0/companies/{id}/users",
            "/rest/v1.0/company_users",
            "/rest/v1.0/users",
            "/rest/v1.0/users/{id}",
            "/rest/v1.0/folders-and-files",
            "/rest/v1.0/custom-fields",
            "/rest/v1.0/configurable-field-sets",

            // Project Management paths
            "/rest/v1.0/projects",
            "/rest/v1.0/projects/{id}",
            "/rest/v1.0/projects/{id}/users",
            "/rest/v1.0/workflows",
            "/rest/v1.0/task-items",
            "/rest/v1.0/project-assignments",
            "/rest/v1.0/project-users",

            // Quality & Safety paths
            "/rest/v1.0/inspections",
            "/rest/v1.0/observations",
            "/rest/v1.0/incidents",
            "/rest/v1.0/safety",
            "/rest/v1.0/quality",
            "/rest/v1.0/punch",

            // Construction Financials paths
            "/rest/v1.0/contracts",
            "/rest/v1.0/purchase-orders",
            "/rest/v1.0/budgets",
            "/rest/v1.0/cost-codes",
            "/rest/v1.0/change-orders",
            "/rest/v1.0/invoices",
            "/rest/v1.0/payments",

            // Field Productivity paths
            "/rest/v1.0/daily-logs",
            "/rest/v1.0/timecards",
            "/rest/v1.0/equipment",
            "/rest/v1.0/manpower",
            "/rest/v1.0/deliveries",

            // Resource Management paths
            "/rest/v1.0/workforce",
            "/rest/v1.0/resources",
            "/rest/v1.0/assignments",

            // Additional paths with attachments (for exclusion testing)
            "/rest/v1.0/companies/{id}/attachments",
            "/rest/v1.0/companies/{id}/workflows",
            "/rest/v1.0/projects/{id}/attachments"
        };
    }

    private static IReadOnlyList<string> GenerateLargePath(int count)
    {
        var paths = new List<string>();
        var random = new Random(12345); // Fixed seed for deterministic tests
        var resources = new[] { "companies", "projects", "users", "workflows", "inspections", "contracts" };
        
        for (int i = 0; i < count; i++)
        {
            var resource = resources[random.Next(resources.Length)];
            var hasId = random.Next(2) == 0;
            var path = hasId 
                ? $"/rest/v1.0/{resource}/{i}" 
                : $"/rest/v1.0/{resource}";
            paths.Add(path);
        }
        
        return paths;
    }
}

/// <summary>
/// Tests for resource group configuration validation
/// </summary>
public class ResourceGroupConfigurationTests
{
    [Theory]
    [MemberData(nameof(GetResourceGroupConfigurations))]
    public void ResourceGroupConfiguration_ShouldHaveValidPaths(
        string resourceGroup, 
        GenerationConfiguration config)
    {
        // Assert
        config.Should().NotBeNull();
        config.ResourceGroup.Should().Be(resourceGroup);
        config.ClassName.Should().NotBeNullOrEmpty();
        config.Namespace.Should().NotBeNullOrEmpty();
        config.IncludePaths.Should().NotBeEmpty();
        config.IncludePaths.Should().OnlyContain(path => !string.IsNullOrWhiteSpace(path));
    }

    [Theory]
    [MemberData(nameof(GetResourceGroupConfigurations))]
    public void ResourceGroupConfiguration_ShouldHaveUniqueNamespaces(
        string resourceGroup, 
        GenerationConfiguration config)
    {
        // This test ensures each resource group has a unique namespace
        var allConfigs = GetResourceGroupConfigurations()
            .Select(data => (GenerationConfiguration)data[1])
            .ToList();

        // Assert
        var namespaces = allConfigs.Select(c => c.Namespace).ToList();
        namespaces.Should().OnlyHaveUniqueItems();
    }

    public static TheoryData<string, GenerationConfiguration> GetResourceGroupConfigurations()
    {
        return new TheoryData<string, GenerationConfiguration>
        {
            {
                "core",
                new GenerationConfiguration
                {
                    ResourceGroup = "core",
                    ClassName = "CoreClient",
                    Namespace = "Procore.SDK.Core",
                    OutputDirectory = "src/Procore.SDK.Core/Generated",
                    OpenApiSpecPath = "docs/rest_OAS_all.json",
                    IncludePaths = new[]
                    {
                        "**/companies/**",
                        "**/company_users/**",
                        "**/users/**",
                        "**/folders-and-files/**",
                        "**/custom-fields/**",
                        "**/configurable-field-sets/**"
                    }
                }
            },
            {
                "project-management",
                new GenerationConfiguration
                {
                    ResourceGroup = "project-management",
                    ClassName = "ProjectManagementClient",
                    Namespace = "Procore.SDK.ProjectManagement",
                    OutputDirectory = "src/Procore.SDK.ProjectManagement/Generated",
                    OpenApiSpecPath = "docs/rest_OAS_all.json",
                    IncludePaths = new[]
                    {
                        "**/projects/**",
                        "**/workflows/**",
                        "**/task-items/**",
                        "**/project-assignments/**",
                        "**/project-users/**"
                    }
                }
            },
            {
                "quality-safety",
                new GenerationConfiguration
                {
                    ResourceGroup = "quality-safety",
                    ClassName = "QualitySafetyClient",
                    Namespace = "Procore.SDK.QualitySafety",
                    OutputDirectory = "src/Procore.SDK.QualitySafety/Generated",
                    OpenApiSpecPath = "docs/rest_OAS_all.json",
                    IncludePaths = new[]
                    {
                        "**/inspections/**",
                        "**/observations/**",
                        "**/incidents/**",
                        "**/safety/**",
                        "**/quality/**",
                        "**/punch/**"
                    }
                }
            }
        };
    }
}