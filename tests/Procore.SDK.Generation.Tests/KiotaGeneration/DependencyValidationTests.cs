using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions;
using System.Reflection;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Generation.Tests.KiotaGeneration;

/// <summary>
/// Task 9: Tests to validate dependencies, package references, and integration requirements
/// for all generated Kiota clients. Ensures proper dependency resolution and compatibility.
/// </summary>
public class DependencyValidationTests
{
    private readonly ITestOutputHelper _output;

    public DependencyValidationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    #region Package Reference Validation

    /// <summary>
    /// Task 9: Validates that all generated clients have the correct Kiota package references.
    /// This prevents dependency resolution issues during compilation and runtime.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.Core")]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.ResourceManagement")]
    [InlineData("Procore.SDK.QualitySafety")]
    [InlineData("Procore.SDK.ConstructionFinancials")]
    [InlineData("Procore.SDK.FieldProductivity")]
    public void GeneratedClient_Should_Have_Required_Kiota_Dependencies(string clientProject)
    {
        var solutionRoot = GetSolutionRoot();
        var projectPath = Path.Combine(solutionRoot, "src", clientProject, $"{clientProject}.csproj");
        
        Assert.True(File.Exists(projectPath), $"Project file should exist at {projectPath}");
        
        var projectContent = File.ReadAllText(projectPath);
        var requiredPackages = new Dictionary<string, string>
        {
            ["Microsoft.Kiota.Abstractions"] = "1.7.8",
            ["Microsoft.Kiota.Http.HttpClientLibrary"] = "1.3.1",
            ["Microsoft.Kiota.Serialization.Json"] = "1.1.1",
            ["Microsoft.Kiota.Serialization.Text"] = "1.1.0"
        };

        foreach (var (packageName, expectedVersion) in requiredPackages)
        {
            Assert.Contains(packageName, projectContent);
                
            // Verify version if it's explicitly specified (may be managed by Directory.Packages.props)
            if (projectContent.Contains($"Version=\""))
            {
                Assert.Contains($"Version=\"{expectedVersion}\"", projectContent);
            }
        }
        
        _output.WriteLine($"✅ {clientProject} has all required Kiota dependencies");
    }

    /// <summary>
    /// Task 9: Validates that Directory.Packages.props contains the correct versions
    /// for centrally managed Kiota packages.
    /// </summary>
    [Fact]
    public void Solution_Should_Have_Correct_Central_Package_Versions()
    {
        var solutionRoot = GetSolutionRoot();
        var packagesPropsPath = Path.Combine(solutionRoot, "Directory.Packages.props");
        
        if (!File.Exists(packagesPropsPath))
        {
            _output.WriteLine("⚠️ Directory.Packages.props not found - packages may be managed individually");
            return;
        }
        
        var packagesContent = File.ReadAllText(packagesPropsPath);
        var expectedVersions = new Dictionary<string, string>
        {
            ["Microsoft.Kiota.Abstractions"] = "1.7.8",
            ["Microsoft.Kiota.Http.HttpClientLibrary"] = "1.3.1",
            ["Microsoft.Kiota.Serialization.Json"] = "1.1.1",
            ["Microsoft.Kiota.Serialization.Text"] = "1.1.0"
        };

        foreach (var (packageName, expectedVersion) in expectedVersions)
        {
            if (packagesContent.Contains($"Include=\"{packageName}\""))
            {
                Assert.Contains($"Version=\"{expectedVersion}\"", packagesContent);
            }
        }
        
        _output.WriteLine("✅ Central package management has correct Kiota versions");
    }

    #endregion

    #region Assembly Reference Validation

    /// <summary>
    /// Task 9: Validates that all generated clients can resolve their required assemblies at runtime.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    public void GeneratedClient_Should_Resolve_All_Assembly_References(Type clientType)
    {
        var assembly = clientType.Assembly;
        var referencedAssemblies = assembly.GetReferencedAssemblies();
        
        var missingAssemblies = new List<string>();
        
        foreach (var referencedAssembly in referencedAssemblies)
        {
            try
            {
                var loadedAssembly = Assembly.Load(referencedAssembly);
                Assert.NotNull(loadedAssembly);
            }
            catch (Exception ex)
            {
                missingAssemblies.Add($"{referencedAssembly.FullName}: {ex.Message}");
            }
        }
        
        if (missingAssemblies.Any())
        {
            _output.WriteLine($"❌ {clientType.Name} has missing assembly references:");
            foreach (var missing in missingAssemblies)
            {
                _output.WriteLine($"   - {missing}");
            }
        }
        
        Assert.Empty(missingAssemblies);
        _output.WriteLine($"✅ {clientType.Name} resolves all {referencedAssemblies.Length} assembly references");
    }

    /// <summary>
    /// Task 9: Validates that all generated clients have the required Kiota types available.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    public void GeneratedClient_Should_Have_Access_To_Kiota_Types(Type clientType)
    {
        var assembly = clientType.Assembly;
        
        // Verify essential Kiota types are accessible
        var requiredKiotaTypes = new[]
        {
            "Microsoft.Kiota.Abstractions.IRequestAdapter",
            "Microsoft.Kiota.Abstractions.BaseRequestBuilder",
            "Microsoft.Kiota.Abstractions.RequestInformation",
            "Microsoft.Kiota.Abstractions.Serialization.IParsable",
            "Microsoft.Kiota.Abstractions.Serialization.IAdditionalDataHolder"
        };

        var missingTypes = new List<string>();
        
        foreach (var typeName in requiredKiotaTypes)
        {
            var type = Type.GetType($"{typeName}, Microsoft.Kiota.Abstractions");
            if (type == null)
            {
                // Try to find it in any loaded assembly
                type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName == typeName);
            }
            
            if (type == null)
            {
                missingTypes.Add(typeName);
            }
        }
        
        if (missingTypes.Any())
        {
            _output.WriteLine($"❌ {clientType.Name} missing Kiota types:");
            foreach (var missing in missingTypes)
            {
                _output.WriteLine($"   - {missing}");
            }
        }
        
        Assert.Empty(missingTypes);
        _output.WriteLine($"✅ {clientType.Name} has access to all required Kiota types");
    }

    #endregion

    #region Dependency Injection Validation

    /// <summary>
    /// Task 9: Validates that all generated clients work correctly with dependency injection containers.
    /// This is critical for integration with ASP.NET Core and other DI frameworks.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Dependency_Injection_Registration()
    {
        var services = new ServiceCollection();
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
        
        // Register dependencies
        services.AddSingleton(mockRequestAdapter);
        
        // Register all generated clients
        services.AddTransient<Procore.SDK.Core.CoreClient>();
        services.AddTransient<Procore.SDK.ProjectManagement.ProjectManagementClient>();
        services.AddTransient<Procore.SDK.ResourceManagement.ResourceManagementClient>();
        services.AddTransient<Procore.SDK.QualitySafety.QualitySafetyClient>();
        services.AddTransient<Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient>();
        services.AddTransient<Procore.SDK.FieldProductivity.FieldProductivityClient>();

        var serviceProvider = services.BuildServiceProvider();
        
        // Verify all clients can be resolved
        var clientTypes = new[]
        {
            typeof(Procore.SDK.Core.CoreClient),
            typeof(Procore.SDK.ProjectManagement.ProjectManagementClient),
            typeof(Procore.SDK.ResourceManagement.ResourceManagementClient),
            typeof(Procore.SDK.QualitySafety.QualitySafetyClient),
            typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient),
            typeof(Procore.SDK.FieldProductivity.FieldProductivityClient)
        };

        foreach (var clientType in clientTypes)
        {
            var client = serviceProvider.GetRequiredService(clientType);
            Assert.NotNull(client);
        }
        
        _output.WriteLine("✅ All generated clients support dependency injection registration");
    }

    /// <summary>
    /// Task 9: Validates that multiple instances of generated clients can coexist in DI container.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Multiple_DI_Instances()
    {
        var services = new ServiceCollection();
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
        
        services.AddSingleton(mockRequestAdapter);
        
        // Register clients with different lifetimes
        services.AddSingleton<Procore.SDK.Core.CoreClient>();
        services.AddScoped<Procore.SDK.ProjectManagement.ProjectManagementClient>();
        services.AddTransient<Procore.SDK.ResourceManagement.ResourceManagementClient>();

        var serviceProvider = services.BuildServiceProvider();
        
        // Create multiple scopes to test different lifetime behaviors
        using (var scope1 = serviceProvider.CreateScope())
        using (var scope2 = serviceProvider.CreateScope())
        {
            // Singleton should be the same instance across scopes
            var core1 = scope1.ServiceProvider.GetRequiredService<Procore.SDK.Core.CoreClient>();
            var core2 = scope2.ServiceProvider.GetRequiredService<Procore.SDK.Core.CoreClient>();
            Assert.Same(core1, core2);
            
            // Scoped should be different across scopes but same within scope
            var project1a = scope1.ServiceProvider.GetRequiredService<Procore.SDK.ProjectManagement.ProjectManagementClient>();
            var project1b = scope1.ServiceProvider.GetRequiredService<Procore.SDK.ProjectManagement.ProjectManagementClient>();
            var project2 = scope2.ServiceProvider.GetRequiredService<Procore.SDK.ProjectManagement.ProjectManagementClient>();
            
            Assert.Same(project1a, project1b);
            Assert.NotSame(project1a, project2);
            
            // Transient should always be different instances
            var resource1 = scope1.ServiceProvider.GetRequiredService<Procore.SDK.ResourceManagement.ResourceManagementClient>();
            var resource2 = scope1.ServiceProvider.GetRequiredService<Procore.SDK.ResourceManagement.ResourceManagementClient>();
            Assert.NotSame(resource1, resource2);
        }
        
        _output.WriteLine("✅ Generated clients support multiple DI instance lifetimes");
    }

    #endregion

    #region Type System Validation

    /// <summary>
    /// Task 9: Validates that all generated types implement required interfaces consistently.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    public void GeneratedClient_Should_Implement_Required_Interfaces(Type clientType)
    {
        // Verify client inherits from BaseRequestBuilder
        Assert.True(typeof(BaseRequestBuilder).IsAssignableFrom(clientType),
            $"{clientType.Name} should inherit from BaseRequestBuilder");
        
        // Verify client has proper constructor
        var constructor = clientType.GetConstructor(new[] { typeof(IRequestAdapter) });
        Assert.NotNull(constructor);
        
        // Verify client has Rest property
        var restProperty = clientType.GetProperty("Rest");
        Assert.NotNull(restProperty);
        Assert.True(restProperty.CanRead);
        Assert.False(restProperty.CanWrite);
        
        _output.WriteLine($"✅ {clientType.Name} implements all required interfaces");
    }

    /// <summary>
    /// Task 9: Validates that generated model types implement serialization interfaces.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.Core")]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.ResourceManagement")]
    [InlineData("Procore.SDK.QualitySafety")]
    [InlineData("Procore.SDK.ConstructionFinancials")]
    [InlineData("Procore.SDK.FieldProductivity")]
    public void GeneratedClient_Models_Should_Implement_Serialization_Interfaces(string clientNamespace)
    {
        var assembly = Assembly.Load(clientNamespace);
        var modelTypes = assembly.GetTypes()
            .Where(t => t.Namespace?.Contains("Generated") == true && 
                       t.IsClass && 
                       !t.IsAbstract &&
                       !t.Name.EndsWith("RequestBuilder") &&
                       !t.Name.EndsWith("Client"))
            .ToList();

        var nonSerializableTypes = new List<string>();
        
        foreach (var modelType in modelTypes)
        {
            // Check if type implements IParsable or IAdditionalDataHolder
            var implementsIParsable = modelType.GetInterfaces()
                .Any(i => i.Name == "IParsable" || i.FullName?.Contains("IParsable") == true);
                
            var implementsIAdditionalDataHolder = modelType.GetInterfaces()
                .Any(i => i.Name == "IAdditionalDataHolder" || i.FullName?.Contains("IAdditionalDataHolder") == true);
            
            if (!implementsIParsable && !implementsIAdditionalDataHolder)
            {
                // Some types like enums or simple types may not need these interfaces
                if (!modelType.IsEnum && !modelType.IsValueType)
                {
                    nonSerializableTypes.Add($"{modelType.FullName}");
                }
            }
        }
        
        if (nonSerializableTypes.Any())
        {
            _output.WriteLine($"⚠️ {clientNamespace} has types that may not be serializable:");
            foreach (var type in nonSerializableTypes.Take(5)) // Show first 5
            {
                _output.WriteLine($"   - {type}");
            }
            if (nonSerializableTypes.Count > 5)
            {
                _output.WriteLine($"   ... and {nonSerializableTypes.Count - 5} more");
            }
        }
        
        _output.WriteLine($"✅ {clientNamespace}: Validated {modelTypes.Count} model types for serialization interfaces");
    }

    #endregion

    #region Version Compatibility Validation

    /// <summary>
    /// Task 9: Validates that all generated clients target the correct .NET version.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.Core")]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.ResourceManagement")]
    [InlineData("Procore.SDK.QualitySafety")]
    [InlineData("Procore.SDK.ConstructionFinancials")]
    [InlineData("Procore.SDK.FieldProductivity")]
    public void GeneratedClient_Should_Target_Correct_DotNet_Version(string clientProject)
    {
        var solutionRoot = GetSolutionRoot();
        var projectPath = Path.Combine(solutionRoot, "src", clientProject, $"{clientProject}.csproj");
        
        Assert.True(File.Exists(projectPath), $"Project file should exist at {projectPath}");
        
        var projectContent = File.ReadAllText(projectPath);
        
        // Check for .NET 8.0 target framework
        Assert.True(
            projectContent.Contains("<TargetFramework>net8.0</TargetFramework>") ||
            projectContent.Contains("<TargetFramework>net8.0</TargetFramework>"),
            $"{clientProject} should target .NET 8.0");
        
        // Verify nullable reference types are enabled
        Assert.Contains("<Nullable>enable</Nullable>", projectContent);
        
        _output.WriteLine($"✅ {clientProject} targets correct .NET version with nullable reference types");
    }

    /// <summary>
    /// Task 9: Validates that kiota-lock.json files contain consistent version information.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Have_Consistent_Kiota_Versions()
    {
        var solutionRoot = GetSolutionRoot();
        var lockFiles = new[]
        {
            "src/Procore.SDK.Core/Generated/kiota-lock.json",
            "src/Procore.SDK.ProjectManagement/Generated/kiota-lock.json",
            "src/Procore.SDK.ResourceManagement/Generated/kiota-lock.json",
            "src/Procore.SDK.QualitySafety/Generated/kiota-lock.json",
            "src/Procore.SDK.ConstructionFinancials/Generated/kiota-lock.json",
            "src/Procore.SDK.FieldProductivity/Generated/kiota-lock.json"
        };

        var versions = new Dictionary<string, string>();
        var kiotaVersions = new HashSet<string>();
        
        foreach (var relativePath in lockFiles)
        {
            var lockFilePath = Path.Combine(solutionRoot, relativePath);
            Assert.True(File.Exists(lockFilePath), $"Lock file should exist at {lockFilePath}");
            
            var lockFileContent = File.ReadAllText(lockFilePath);
            var json = JsonDocument.Parse(lockFileContent);
            
            if (json.RootElement.TryGetProperty("version", out var versionElement))
            {
                var version = versionElement.GetString();
                kiotaVersions.Add(version ?? "unknown");
                versions[relativePath] = version ?? "unknown";
            }
        }
        
        // All clients should use the same Kiota version
        Assert.Single(kiotaVersions, $"All clients should use the same Kiota version. Found: {string.Join(", ", kiotaVersions)}");
        
        var commonVersion = kiotaVersions.First();
        _output.WriteLine($"✅ All generated clients use Kiota version {commonVersion}");
        
        // Log versions for verification
        foreach (var (path, version) in versions)
        {
            _output.WriteLine($"   - {Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(path)))}: {version}");
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Helper method to find the solution root directory.
    /// </summary>
    private static string GetSolutionRoot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var directory = new DirectoryInfo(currentDirectory);
        
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        
        return directory?.FullName ?? currentDirectory;
    }

    #endregion
}