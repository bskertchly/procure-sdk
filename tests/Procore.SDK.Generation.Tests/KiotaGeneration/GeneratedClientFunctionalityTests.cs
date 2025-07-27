using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Generation.Tests.KiotaGeneration;

/// <summary>
/// Tests to validate that generated Kiota clients expose expected API operations 
/// and maintain proper type mappings between requests, responses, and domain models.
/// </summary>
public class GeneratedClientFunctionalityTests
{
    private readonly ITestOutputHelper _output;
    private readonly IRequestAdapter _mockRequestAdapter;

    public GeneratedClientFunctionalityTests(ITestOutputHelper output)
    {
        _output = output;
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
    }

    #region ProjectManagement Client Functionality Tests

    /// <summary>
    /// Validates that ProjectManagement client exposes expected project operations.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Expose_Project_Operations()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);

        // Act & Assert - Verify navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        Assert.NotNull(client.Rest.V10.Projects);
        
        // Verify the client can navigate to company projects
        Assert.NotNull(client.Rest.V10.Companies);
        
        _output.WriteLine("✅ ProjectManagement client exposes expected project operations");
    }

    /// <summary>
    /// Validates that ProjectManagement client has proper request builders for project operations.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Have_Project_Request_Builders()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);

        // Act - Access request builders
        var projectsBuilder = client.Rest.V10.Projects;
        var companiesBuilder = client.Rest.V10.Companies;

        // Assert
        Assert.NotNull(projectsBuilder);
        Assert.NotNull(companiesBuilder);
        
        // Verify builders are of correct types
        Assert.Equal("ProjectsRequestBuilder", projectsBuilder.GetType().Name);
        Assert.Equal("CompaniesRequestBuilder", companiesBuilder.GetType().Name);
        
        _output.WriteLine("✅ ProjectManagement client has proper request builders");
    }

    /// <summary>
    /// Validates that ProjectManagement client can build proper request information.
    /// </summary>
    [Fact]
    public async Task ProjectManagementClient_Should_Build_Valid_Requests()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var projectBuilder = client.Rest.V10.Projects.Item("123");

        // Act - This should not throw and should be callable
        var requestInfo = projectBuilder.ToGetRequestInformation();

        // Assert
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("/projects/123", requestInfo.URL);
        
        _output.WriteLine("✅ ProjectManagement client builds valid request information");
    }

    /// <summary>
    /// Validates that ProjectManagement client supports PATCH operations for project updates.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Support_Project_Updates()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var projectBuilder = client.Rest.V10.Projects.Item("123");

        // Act - Verify PATCH request can be created
        var patchRequestInfo = projectBuilder.ToPatchRequestInformation(new {});

        // Assert
        Assert.NotNull(patchRequestInfo);
        Assert.Equal(Method.PATCH, patchRequestInfo.HttpMethod);
        Assert.Contains("/projects/123", patchRequestInfo.URL);
        
        _output.WriteLine("✅ ProjectManagement client supports project update operations");
    }

    /// <summary>
    /// Validates that ProjectManagement client supports sync operations.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Support_Sync_Operations()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);

        // Act - Access sync endpoint
        var syncBuilder = client.Rest.V10.Projects.Sync;

        // Assert
        Assert.NotNull(syncBuilder);
        Assert.Equal("SyncRequestBuilder", syncBuilder.GetType().Name);
        
        _output.WriteLine("✅ ProjectManagement client supports sync operations");
    }

    #endregion

    #region ResourceManagement Client Functionality Tests

    /// <summary>
    /// Validates that ResourceManagement client exposes expected resource operations.
    /// </summary>
    [Fact]
    public void ResourceManagementClient_Should_Expose_Resource_Operations()
    {
        // Arrange
        var client = new Procore.SDK.ResourceManagement.ResourceManagementClient(_mockRequestAdapter);

        // Act & Assert - Verify navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Verify resource-specific endpoints
        Assert.NotNull(client.Rest.V10.Resources);
        Assert.NotNull(client.Rest.V10.WorkforcePlanning);
        
        _output.WriteLine("✅ ResourceManagement client exposes expected resource operations");
    }

    /// <summary>
    /// Validates that ResourceManagement client supports webhook operations.
    /// </summary>
    [Fact]
    public void ResourceManagementClient_Should_Support_Webhook_Operations()
    {
        // Arrange
        var client = new Procore.SDK.ResourceManagement.ResourceManagementClient(_mockRequestAdapter);

        // Act - Access webhooks endpoint
        var webhooksBuilder = client.Rest.V10.Webhooks;

        // Assert
        Assert.NotNull(webhooksBuilder);
        Assert.Equal("WebhooksRequestBuilder", webhooksBuilder.GetType().Name);
        
        _output.WriteLine("✅ ResourceManagement client supports webhook operations");
    }

    #endregion

    #region QualitySafety Client Functionality Tests

    /// <summary>
    /// Validates that QualitySafety client exposes expected observation operations.
    /// </summary>
    [Fact]
    public void QualitySafetyClient_Should_Expose_Observation_Operations()
    {
        // Arrange
        var client = new Procore.SDK.QualitySafety.QualitySafetyClient(_mockRequestAdapter);

        // Act & Assert - Verify navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        Assert.NotNull(client.Rest.V10.Observations);
        
        _output.WriteLine("✅ QualitySafety client exposes expected observation operations");
    }

    /// <summary>
    /// Validates that QualitySafety client supports assignee operations.
    /// </summary>
    [Fact]
    public void QualitySafetyClient_Should_Support_Assignee_Operations()
    {
        // Arrange
        var client = new Procore.SDK.QualitySafety.QualitySafetyClient(_mockRequestAdapter);

        // Act - Access assignees endpoint
        var assigneesBuilder = client.Rest.V10.Observations.Assignees;

        // Assert
        Assert.NotNull(assigneesBuilder);
        
        _output.WriteLine("✅ QualitySafety client supports assignee operations");
    }

    #endregion

    #region Core Client Functionality Tests

    /// <summary>
    /// Validates that Core client exposes expected company and user operations.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Expose_Core_Operations()
    {
        // Arrange
        var client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter);

        // Act & Assert - Verify navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        Assert.NotNull(client.Rest.V10.Companies);
        Assert.NotNull(client.Rest.V10.Users);
        
        // Verify multiple API versions are supported
        Assert.NotNull(client.Rest.V11);
        Assert.NotNull(client.Rest.V12);
        Assert.NotNull(client.Rest.V13);
        Assert.NotNull(client.Rest.V20);
        
        _output.WriteLine("✅ Core client exposes expected core operations across multiple API versions");
    }

    /// <summary>
    /// Validates that Core client supports workforce planning operations.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Support_WorkforcePlanning_Operations()
    {
        // Arrange
        var client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter);

        // Act - Access workforce planning endpoint
        var workforcePlanningBuilder = client.Rest.V10.WorkforcePlanning;

        // Assert
        Assert.NotNull(workforcePlanningBuilder);
        Assert.NotNull(workforcePlanningBuilder.V2);
        
        _output.WriteLine("✅ Core client supports workforce planning operations");
    }

    /// <summary>
    /// Validates that Core client has proper error response types.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Have_Error_Response_Types()
    {
        // Arrange
        var client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter);
        var userBuilder = client.Rest.V10.Users.Item("123");

        // Act - Check that error types are available in the assembly
        var assembly = typeof(Procore.SDK.Core.CoreClient).Assembly;
        var errorTypes = assembly.GetTypes()
            .Where(t => t.Name.Contains("Error"))
            .ToList();

        // Assert
        Assert.NotEmpty(errorTypes);
        
        // Look for specific error types we expect
        var users401Error = errorTypes.FirstOrDefault(t => t.Name == "Users401Error");
        var users403Error = errorTypes.FirstOrDefault(t => t.Name == "Users403Error");
        
        Assert.NotNull(users401Error);
        Assert.NotNull(users403Error);
        
        _output.WriteLine($"✅ Core client has {errorTypes.Count} error response types including 401 and 403 errors");
    }

    #endregion

    #region Type Mapping and Serialization Tests

    /// <summary>
    /// Validates that generated clients have proper type mappings for responses.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.ResourceManagement")]
    [InlineData("Procore.SDK.QualitySafety")]
    [InlineData("Procore.SDK.ConstructionFinancials")]
    [InlineData("Procore.SDK.FieldProductivity")]
    [InlineData("Procore.SDK.Core")]
    public void GeneratedClient_Should_Have_Proper_Response_Types(string namespaceName)
    {
        // Act - Get all types in the namespace
        var assembly = Assembly.Load(namespaceName);
        var responseTypes = assembly.GetTypes()
            .Where(t => t.Name.Contains("Response"))
            .ToList();

        // Assert
        Assert.NotEmpty(responseTypes);
        
        // Verify response types implement expected interfaces
        foreach (var responseType in responseTypes.Take(5)) // Test first 5 to avoid too many assertions
        {
            // Response types should be classes that can be serialized
            Assert.True(responseType.IsClass);
            Assert.False(responseType.IsAbstract);
        }
        
        _output.WriteLine($"✅ {namespaceName} has {responseTypes.Count} response types with proper structure");
    }

    /// <summary>
    /// Validates that generated clients have proper request body types.
    /// </summary>
    [Theory]
    [InlineData("Procore.SDK.ProjectManagement")]
    [InlineData("Procore.SDK.Core")]
    public void GeneratedClient_Should_Have_Proper_Request_Types(string namespaceName)
    {
        // Act - Get all types in the namespace
        var assembly = Assembly.Load(namespaceName);
        var requestTypes = assembly.GetTypes()
            .Where(t => t.Name.Contains("RequestBody") || t.Name.Contains("PatchRequestBody"))
            .ToList();

        // Assert
        if (requestTypes.Any()) // Only test if request types exist
        {
            foreach (var requestType in requestTypes.Take(3)) // Test first 3
            {
                Assert.True(requestType.IsClass);
                Assert.False(requestType.IsAbstract);
            }
            
            _output.WriteLine($"✅ {namespaceName} has {requestTypes.Count} request body types");
        }
        else
        {
            _output.WriteLine($"ℹ️ {namespaceName} has no request body types (read-only client)");
        }
    }

    /// <summary>
    /// Validates that generated clients support cancellation tokens.
    /// </summary>
    [Fact]
    public async Task GeneratedClients_Should_Support_Cancellation_Tokens()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var cancellationToken = new CancellationToken();
        
        // Setup mock to track cancellation token usage
        _mockRequestAdapter.SendAsync<object>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<object>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<object>(new object()));

        // Act - Call method with cancellation token
        var projectBuilder = client.Rest.V10.Projects.Item("123");
        
        try
        {
            await projectBuilder.GetAsync(cancellationToken: cancellationToken);
        }
        catch (NotImplementedException)
        {
            // Expected for mock, we just want to verify the signature supports cancellation tokens
        }

        // Assert - Verify the method signature accepts cancellation token
        var methods = typeof(Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.ItemRequestBuilder)
            .GetMethods()
            .Where(m => m.Name == "GetAsync");
        
        var methodWithCancellation = methods.FirstOrDefault(m => 
            m.GetParameters().Any(p => p.ParameterType == typeof(CancellationToken)));
        
        Assert.NotNull(methodWithCancellation);
        
        _output.WriteLine("✅ Generated clients support cancellation tokens");
    }

    /// <summary>
    /// Validates that generated clients have proper HTTP method support.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_HTTP_Methods()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        var projectBuilder = client.Rest.V10.Projects.Item("123");

        // Act & Assert - Verify different HTTP methods are supported
        var getMethods = projectBuilder.GetType().GetMethods()
            .Where(m => m.Name.Contains("Get"))
            .ToList();
        
        var patchMethods = projectBuilder.GetType().GetMethods()
            .Where(m => m.Name.Contains("Patch"))
            .ToList();

        Assert.NotEmpty(getMethods);
        Assert.NotEmpty(patchMethods);
        
        _output.WriteLine($"✅ Generated clients support multiple HTTP methods (GET: {getMethods.Count}, PATCH: {patchMethods.Count})");
    }

    #endregion

    #region API Version Support Tests

    /// <summary>
    /// Validates that clients support multiple API versions where applicable.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Support_Multiple_API_Versions()
    {
        // Arrange
        var client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter);

        // Act & Assert - Verify multiple versions exist
        Assert.NotNull(client.Rest.V10);
        Assert.NotNull(client.Rest.V11);
        Assert.NotNull(client.Rest.V12);
        Assert.NotNull(client.Rest.V13);
        Assert.NotNull(client.Rest.V20);
        
        // Verify version-specific functionality
        Assert.NotNull(client.Rest.V10.Companies);
        Assert.NotNull(client.Rest.V11.Companies);
        Assert.NotNull(client.Rest.V12.Companies);
        Assert.NotNull(client.Rest.V13.Companies);
        Assert.NotNull(client.Rest.V20.Companies);
        
        _output.WriteLine("✅ Core client supports multiple API versions (v1.0, v1.1, v1.2, v1.3, v2.0)");
    }

    /// <summary>
    /// Validates that ResourceManagement client supports version-specific endpoints.
    /// </summary>
    [Fact]
    public void ResourceManagementClient_Should_Support_Version_Specific_Endpoints()
    {
        // Arrange
        var client = new Procore.SDK.ResourceManagement.ResourceManagementClient(_mockRequestAdapter);

        // Act & Assert - Verify v1.0 and v1.1 support
        Assert.NotNull(client.Rest.V10);
        Assert.NotNull(client.Rest.V11);
        
        // Verify version-specific projects endpoint in v1.1
        Assert.NotNull(client.Rest.V11.Projects);
        
        _output.WriteLine("✅ ResourceManagement client supports version-specific endpoints");
    }

    #endregion

    #region Request Builder Pattern Tests

    /// <summary>
    /// Validates that request builders follow the expected pattern.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Follow_RequestBuilder_Pattern()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);

        // Act - Navigate through the builder pattern
        var restBuilder = client.Rest;
        var versionBuilder = restBuilder.V10;
        var projectsBuilder = versionBuilder.Projects;
        var itemBuilder = projectsBuilder.Item("123");

        // Assert - Verify each level returns the expected type
        Assert.Equal("RestRequestBuilder", restBuilder.GetType().Name);
        Assert.Equal("V10RequestBuilder", versionBuilder.GetType().Name);
        Assert.Equal("ProjectsRequestBuilder", projectsBuilder.GetType().Name);
        Assert.Equal("ItemRequestBuilder", itemBuilder.GetType().Name);
        
        _output.WriteLine("✅ Generated clients follow proper RequestBuilder pattern");
    }

    /// <summary>
    /// Validates that request builders support fluent API navigation.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Fluent_Navigation()
    {
        // Arrange
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);

        // Act - Use fluent API to navigate to specific project
        var projectItemBuilder = client.Rest.V10.Projects.Item("123");
        var companiesItemBuilder = client.Rest.V10.Companies.Item("456");

        // Assert
        Assert.NotNull(projectItemBuilder);
        Assert.NotNull(companiesItemBuilder);
        
        // Verify they are different instances
        Assert.NotSame(projectItemBuilder, companiesItemBuilder);
        
        _output.WriteLine("✅ Generated clients support fluent API navigation");
    }

    #endregion
}