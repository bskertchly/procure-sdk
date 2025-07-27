using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions;
using System.Reflection;
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
}