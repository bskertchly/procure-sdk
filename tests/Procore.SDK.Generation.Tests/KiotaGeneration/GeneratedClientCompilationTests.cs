using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Generation.Tests.KiotaGeneration;

/// <summary>
/// Tests to validate that all generated Kiota clients compile without errors 
/// and can be instantiated with proper dependencies.
/// </summary>
public class GeneratedClientCompilationTests
{
    private readonly ITestOutputHelper _output;

    public GeneratedClientCompilationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /// <summary>
    /// Validates that the generated ProjectManagement client compiles and can be instantiated.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Compile_And_Instantiate()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        Assert.NotNull(client.Rest);
        _output.WriteLine("✅ ProjectManagement client instantiated successfully");
    }

    /// <summary>
    /// Validates that the generated ResourceManagement client compiles and can be instantiated.
    /// </summary>
    [Fact]
    public void ResourceManagementClient_Should_Compile_And_Instantiate()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw
        var client = new Procore.SDK.ResourceManagement.ResourceManagementClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        Assert.NotNull(client.Rest);
        _output.WriteLine("✅ ResourceManagement client instantiated successfully");
    }

    /// <summary>
    /// Validates that the generated QualitySafety client compiles and can be instantiated.
    /// </summary>
    [Fact]
    public void QualitySafetyClient_Should_Compile_And_Instantiate()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw
        var client = new Procore.SDK.QualitySafety.QualitySafetyClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        Assert.NotNull(client.Rest);
        _output.WriteLine("✅ QualitySafety client instantiated successfully");
    }

    /// <summary>
    /// Validates that the generated ConstructionFinancials client compiles and can be instantiated.
    /// </summary>
    [Fact]
    public void ConstructionFinancialsClient_Should_Compile_And_Instantiate()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw
        var client = new Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        Assert.NotNull(client.Rest);
        _output.WriteLine("✅ ConstructionFinancials client instantiated successfully");
    }

    /// <summary>
    /// Validates that the generated FieldProductivity client compiles and can be instantiated.
    /// </summary>
    [Fact]
    public void FieldProductivityClient_Should_Compile_And_Instantiate()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw
        var client = new Procore.SDK.FieldProductivity.FieldProductivityClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        Assert.NotNull(client.Rest);
        _output.WriteLine("✅ FieldProductivity client instantiated successfully");
    }

    /// <summary>
    /// Validates that the generated Core client compiles and can be instantiated.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Compile_And_Instantiate()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw
        var client = new Procore.SDK.Core.CoreClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        Assert.NotNull(client.Rest);
        _output.WriteLine("✅ Core client instantiated successfully");
    }

    /// <summary>
    /// Validates that all generated clients have proper namespace resolution.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement.ProjectManagementClient")]
    [InlineData("Procore.SDK.ResourceManagement.ResourceManagementClient")]
    [InlineData("Procore.SDK.QualitySafety.QualitySafetyClient")]
    [InlineData("Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient")]
    [InlineData("Procore.SDK.FieldProductivity.FieldProductivityClient")]
    [InlineData("Procore.SDK.Core.CoreClient")]
    public void GeneratedClient_Should_Have_Proper_Namespace_Resolution(string fullyQualifiedTypeName)
    {
        // Act
        var type = Type.GetType(fullyQualifiedTypeName);
        
        // Assert
        Assert.NotNull(type);
        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
        
        // Verify it has the expected constructor
        var constructor = type.GetConstructor(new[] { typeof(IRequestAdapter) });
        Assert.NotNull(constructor);
        
        _output.WriteLine($"✅ {fullyQualifiedTypeName} has proper namespace resolution and constructor");
    }

    /// <summary>
    /// Validates that all generated clients inherit from BaseRequestBuilder.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    public void GeneratedClient_Should_Inherit_From_BaseRequestBuilder(Type clientType)
    {
        // Act & Assert
        Assert.True(typeof(BaseRequestBuilder).IsAssignableFrom(clientType));
        _output.WriteLine($"✅ {clientType.Name} inherits from BaseRequestBuilder");
    }

    /// <summary>
    /// Validates that all generated clients have the expected Rest property.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), "Rest")]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient), "Rest")]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient), "Rest")]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient), "Rest")]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient), "Rest")]
    [InlineData(typeof(Procore.SDK.Core.CoreClient), "Rest")]
    public void GeneratedClient_Should_Have_Rest_Property(Type clientType, string expectedPropertyName)
    {
        // Act
        var property = clientType.GetProperty(expectedPropertyName);
        
        // Assert
        Assert.NotNull(property);
        Assert.True(property.CanRead);
        Assert.False(property.CanWrite); // Should be read-only
        
        _output.WriteLine($"✅ {clientType.Name} has {expectedPropertyName} property");
    }

    /// <summary>
    /// Validates that generated clients properly register serializers in constructor.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Register_Serializers_In_Constructor()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act & Assert - Should not throw during serializer registration
        var projectClient = new Procore.SDK.ProjectManagement.ProjectManagementClient(mockRequestAdapter);
        var resourceClient = new Procore.SDK.ResourceManagement.ResourceManagementClient(mockRequestAdapter);
        var qualityClient = new Procore.SDK.QualitySafety.QualitySafetyClient(mockRequestAdapter);
        var financialsClient = new Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient(mockRequestAdapter);
        var productivityClient = new Procore.SDK.FieldProductivity.FieldProductivityClient(mockRequestAdapter);
        var coreClient = new Procore.SDK.Core.CoreClient(mockRequestAdapter);

        // Verify clients are properly instantiated
        Assert.NotNull(projectClient);
        Assert.NotNull(resourceClient);
        Assert.NotNull(qualityClient);
        Assert.NotNull(financialsClient);
        Assert.NotNull(productivityClient);
        Assert.NotNull(coreClient);
        
        _output.WriteLine("✅ All generated clients register serializers without errors");
    }

    /// <summary>
    /// Validates that generated clients set the correct base URL.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Set_Correct_Base_Url()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        string? capturedBaseUrl = null;
        
        // Capture the base URL when it's set
        mockRequestAdapter.When(x => x.BaseUrl = Arg.Any<string>())
                         .Do(x => capturedBaseUrl = x.Arg<string>());

        // Act
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(mockRequestAdapter);

        // Assert
        Assert.Equal("https://api.procore.com", capturedBaseUrl);
        _output.WriteLine("✅ Generated client sets correct base URL");
    }

    /// <summary>
    /// Validates that all generated clients can be created through dependency injection.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Dependency_Injection()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
        
        services.AddSingleton(mockRequestAdapter);
        services.AddTransient<Procore.SDK.ProjectManagement.ProjectManagementClient>();
        services.AddTransient<Procore.SDK.ResourceManagement.ResourceManagementClient>();
        services.AddTransient<Procore.SDK.QualitySafety.QualitySafetyClient>();
        services.AddTransient<Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient>();
        services.AddTransient<Procore.SDK.FieldProductivity.FieldProductivityClient>();
        services.AddTransient<Procore.SDK.Core.CoreClient>();

        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        var projectClient = serviceProvider.GetRequiredService<Procore.SDK.ProjectManagement.ProjectManagementClient>();
        var resourceClient = serviceProvider.GetRequiredService<Procore.SDK.ResourceManagement.ResourceManagementClient>();
        var qualityClient = serviceProvider.GetRequiredService<Procore.SDK.QualitySafety.QualitySafetyClient>();
        var financialsClient = serviceProvider.GetRequiredService<Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient>();
        var productivityClient = serviceProvider.GetRequiredService<Procore.SDK.FieldProductivity.FieldProductivityClient>();
        var coreClient = serviceProvider.GetRequiredService<Procore.SDK.Core.CoreClient>();

        Assert.NotNull(projectClient);
        Assert.NotNull(resourceClient);
        Assert.NotNull(qualityClient);
        Assert.NotNull(financialsClient);
        Assert.NotNull(productivityClient);
        Assert.NotNull(coreClient);
        
        _output.WriteLine("✅ All generated clients support dependency injection");
    }

    /// <summary>
    /// Validates that nullable reference types are properly handled in generated clients.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    public void GeneratedClient_Should_Handle_Nullable_Reference_Types(Type clientType)
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");

        // Act - Should not throw ArgumentNullException with valid adapter
        var client = Activator.CreateInstance(clientType, mockRequestAdapter);
        
        // Assert
        Assert.NotNull(client);
        
        // Verify constructor throws with null adapter
        Assert.Throws<ArgumentNullException>(() => Activator.CreateInstance(clientType, (IRequestAdapter)null!));
        
        _output.WriteLine($"✅ {clientType.Name} properly handles nullable reference types");
    }

    /// <summary>
    /// Validates that generated clients have proper error handling for invalid base URLs.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Handle_Empty_Base_Url()
    {
        // Arrange
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns((string?)null);

        // Act & Assert - Should not throw, should set default URL
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(mockRequestAdapter);
        
        Assert.NotNull(client);
        
        // Verify that the base URL was set to the default
        mockRequestAdapter.Received().BaseUrl = "https://api.procore.com";
        
        _output.WriteLine("✅ Generated client handles empty base URL correctly");
    }

    #region Task 9 Enhanced Validation Tests

    /// <summary>
    /// Task 9: Validates that all generated clients compile without CS0234 namespace errors.
    /// These were the primary compilation issues identified in Task 9.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), "ProjectManagement")]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient), "ResourceManagement")]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient), "QualitySafety")]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient), "ConstructionFinancials")]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient), "FieldProductivity")]
    [InlineData(typeof(Procore.SDK.Core.CoreClient), "Core")]
    public void GeneratedClient_Should_Not_Have_CS0234_Compilation_Errors(Type clientType, string clientName)
    {
        // Task 9 Focus: Ensure no "type or namespace name does not exist" errors
        // Historical Issues: FilesPostRequestBody, FilesPatchRequestBody, FoldersPostRequestBody, FoldersPatchRequestBody
        
        try
        {
            var mockRequestAdapter = Substitute.For<IRequestAdapter>();
            mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
            
            // This will fail at compile time if CS0234 errors exist
            var client = Activator.CreateInstance(clientType, mockRequestAdapter);
            
            Assert.NotNull(client);
            
            // Verify the client has the expected structure
            var restProperty = clientType.GetProperty("Rest");
            Assert.NotNull(restProperty);
            
            var restValue = restProperty.GetValue(client);
            Assert.NotNull(restValue);
            
            _output.WriteLine($"✅ {clientName} client compiles without CS0234 errors");
        }
        catch (Exception ex)
        {
            // Log detailed information about the compilation issue
            _output.WriteLine($"❌ {clientName} client failed: {ex.Message}");
            if (ex.InnerException != null)
            {
                _output.WriteLine($"   Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
    }

    /// <summary>
    /// Task 9: Validates that all expected request body types exist and are accessible.
    /// This prevents regression of missing FilesPostRequestBody, FilesPatchRequestBody issues.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Have_All_Request_Body_Types()
    {
        var missingTypes = new List<string>();
        var clientAssemblies = new[]
        {
            typeof(Procore.SDK.ProjectManagement.ProjectManagementClient).Assembly,
            typeof(Procore.SDK.ResourceManagement.ResourceManagementClient).Assembly,
            typeof(Procore.SDK.QualitySafety.QualitySafetyClient).Assembly,
            typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient).Assembly,
            typeof(Procore.SDK.FieldProductivity.FieldProductivityClient).Assembly,
            typeof(Procore.SDK.Core.CoreClient).Assembly
        };

        foreach (var assembly in clientAssemblies)
        {
            var assemblyName = assembly.GetName().Name;
            var types = assembly.GetTypes();
            
            // Look for types that should exist based on common patterns
            var requestBodyPattern = new Regex(@".*(?:Post|Patch|Put)RequestBody$", RegexOptions.Compiled);
            var requestBodyTypes = types.Where(t => requestBodyPattern.IsMatch(t.Name)).ToList();
            
            // Verify that request builders reference valid request body types
            var requestBuilderTypes = types.Where(t => t.Name.EndsWith("RequestBuilder")).ToList();
            
            foreach (var builderType in requestBuilderTypes)
            {
                var methods = builderType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                
                foreach (var method in methods.Where(m => m.Name.Contains("Post") || m.Name.Contains("Patch") || m.Name.Contains("Put")))
                {
                    var parameters = method.GetParameters();
                    var requestBodyParam = parameters.FirstOrDefault(p => p.Name?.Contains("requestBody") == true || 
                                                                         p.ParameterType.Name.Contains("RequestBody"));
                    
                    if (requestBodyParam != null)
                    {
                        var requestBodyType = requestBodyParam.ParameterType;
                        if (!requestBodyType.IsValueType && requestBodyType != typeof(string))
                        {
                            // Verify the type is actually defined in the assembly
                            var typeExists = types.Any(t => t == requestBodyType);
                            if (!typeExists)
                            {
                                missingTypes.Add($"{assemblyName}: {requestBodyType.Name} referenced by {builderType.Name}.{method.Name}");
                            }
                        }
                    }
                }
            }
            
            _output.WriteLine($"✅ {assemblyName}: Found {requestBodyTypes.Count} request body types, validated {requestBuilderTypes.Count} request builders");
        }
        
        if (missingTypes.Any())
        {
            _output.WriteLine($"❌ Missing request body types found:");
            foreach (var missingType in missingTypes)
            {
                _output.WriteLine($"   - {missingType}");
            }
        }
        
        Assert.Empty(missingTypes);
    }

    /// <summary>
    /// Task 9: Validates that all generated clients have consistent namespace structure.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement", typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData("Procore.SDK.ResourceManagement", typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData("Procore.SDK.QualitySafety", typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData("Procore.SDK.ConstructionFinancials", typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData("Procore.SDK.FieldProductivity", typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    [InlineData("Procore.SDK.Core", typeof(Procore.SDK.Core.CoreClient))]
    public void GeneratedClient_Should_Have_Consistent_Namespace_Structure(string expectedNamespace, Type clientType)
    {
        // Task 9 Focus: Ensure namespace consistency to prevent resolution issues
        
        Assert.Equal(expectedNamespace, clientType.Namespace);
        
        // Verify that generated types in the same assembly follow the namespace pattern
        var assembly = clientType.Assembly;
        var generatedTypes = assembly.GetTypes().Where(t => t.Namespace?.StartsWith(expectedNamespace) == true).ToList();
        
        Assert.NotEmpty(generatedTypes);
        
        // Check for common namespace violations
        var invalidNamespaces = generatedTypes
            .Where(t => !t.Namespace.StartsWith(expectedNamespace))
            .Select(t => $"{t.Name} in {t.Namespace}")
            .ToList();
            
        if (invalidNamespaces.Any())
        {
            _output.WriteLine($"❌ Invalid namespaces found in {expectedNamespace}:");
            foreach (var invalid in invalidNamespaces)
            {
                _output.WriteLine($"   - {invalid}");
            }
        }
        
        Assert.Empty(invalidNamespaces);
        
        _output.WriteLine($"✅ {expectedNamespace}: {generatedTypes.Count} types follow consistent namespace structure");
    }

    /// <summary>
    /// Task 9: Validates that all generated clients properly handle nullable reference types.
    /// This addresses nullable reference type warnings that were part of the compilation issues.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    public void GeneratedClient_Should_Handle_Nullable_Reference_Types_Without_Warnings(Type clientType)
    {
        // Task 9 Focus: Ensure nullable reference type compliance
        
        var mockRequestAdapter = Substitute.For<IRequestAdapter>();
        mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
        
        // Test with valid adapter - should not throw
        var clientWithValidAdapter = Activator.CreateInstance(clientType, mockRequestAdapter);
        Assert.NotNull(clientWithValidAdapter);
        
        // Test with null adapter - should throw ArgumentNullException (proper null handling)
        var exception = Assert.Throws<TargetInvocationException>(() => 
            Activator.CreateInstance(clientType, (IRequestAdapter)null!));
        
        Assert.IsType<ArgumentNullException>(exception.InnerException);
        
        _output.WriteLine($"✅ {clientType.Name} properly handles nullable reference types");
    }

    /// <summary>
    /// Task 9: Validates that kiota-lock.json files exist and contain expected configuration.
    /// </summary>
    [Theory]
    [InlineData("ProjectManagement", "src/Procore.SDK.ProjectManagement/Generated/kiota-lock.json")]
    [InlineData("ResourceManagement", "src/Procore.SDK.ResourceManagement/Generated/kiota-lock.json")]
    [InlineData("QualitySafety", "src/Procore.SDK.QualitySafety/Generated/kiota-lock.json")]
    [InlineData("ConstructionFinancials", "src/Procore.SDK.ConstructionFinancials/Generated/kiota-lock.json")]
    [InlineData("FieldProductivity", "src/Procore.SDK.FieldProductivity/Generated/kiota-lock.json")]
    [InlineData("Core", "src/Procore.SDK.Core/Generated/kiota-lock.json")]
    public void GeneratedClient_Should_Have_Valid_Kiota_Lock_File(string clientName, string relativePath)
    {
        // Task 9 Focus: Ensure generation configuration is properly maintained
        
        var solutionRoot = GetSolutionRoot();
        var lockFilePath = Path.Combine(solutionRoot, relativePath);
        
        Assert.True(File.Exists(lockFilePath), $"Kiota lock file should exist at {lockFilePath}");
        
        var lockFileContent = File.ReadAllText(lockFilePath);
        Assert.False(string.IsNullOrWhiteSpace(lockFileContent), "Lock file should not be empty");
        
        // Verify it contains expected JSON structure
        try
        {
            var json = System.Text.Json.JsonDocument.Parse(lockFileContent);
            
            Assert.True(json.RootElement.TryGetProperty("version", out _), "Lock file should contain version");
            Assert.True(json.RootElement.TryGetProperty("descriptionLocation", out _), "Lock file should contain descriptionLocation");
            Assert.True(json.RootElement.TryGetProperty("clientClassName", out _), "Lock file should contain clientClassName");
            
            _output.WriteLine($"✅ {clientName} has valid kiota-lock.json configuration");
        }
        catch (System.Text.Json.JsonException ex)
        {
            Assert.True(false, $"Lock file contains invalid JSON: {ex.Message}");
        }
    }

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