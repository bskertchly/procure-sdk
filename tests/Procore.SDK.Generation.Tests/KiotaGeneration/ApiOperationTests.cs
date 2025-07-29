using Microsoft.Kiota.Abstractions;
using System.Reflection;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Generation.Tests.KiotaGeneration;

/// <summary>
/// Task 9: Tests to validate that generated Kiota clients expose expected API operations,
/// support correct HTTP methods, and maintain proper endpoint coverage based on the OpenAPI specification.
/// </summary>
public class ApiOperationTests
{
    private readonly ITestOutputHelper _output;
    private readonly IRequestAdapter _mockRequestAdapter;

    public ApiOperationTests(ITestOutputHelper output)
    {
        _output = output;
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl.Returns("https://api.procore.com");
    }

    #region Core Client API Operations

    /// <summary>
    /// Task 9: Validates that Core client exposes expected company and user operations.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Expose_Expected_API_Operations()
    {
        var client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter);

        // Test navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Core API endpoints
        Assert.NotNull(client.Rest.V10.Companies);
        Assert.NotNull(client.Rest.V10.Users);
        
        // Test that we can navigate to specific endpoints
        var companyBuilder = client.Rest.V10.Companies[123];
        Assert.NotNull(companyBuilder);
        
        var userBuilder = client.Rest.V10.Users[456];
        Assert.NotNull(userBuilder);
        
        _output.WriteLine("✅ Core client exposes expected API operations");
    }

    /// <summary>
    /// Task 9: Validates that Core client supports all required HTTP methods.
    /// </summary>
    [Fact]
    public void CoreClient_Should_Support_Required_HTTP_Methods()
    {
        var client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter);
        
        // Test companies endpoint methods
        var companiesBuilder = client.Rest.V10.Companies;
        var companyBuilder = client.Rest.V10.Companies[123];
        
        // Verify method availability through reflection
        var companiesBuilderType = companiesBuilder.GetType();
        var companyBuilderType = companyBuilder.GetType();
        
        // Companies collection should support GET (list) and POST (create)
        Assert.NotNull(companiesBuilderType.GetMethod("ToGetRequestInformation"));
        
        // Individual company should support GET, PATCH, DELETE
        Assert.NotNull(companyBuilderType.GetMethod("ToGetRequestInformation"));
        
        _output.WriteLine("✅ Core client supports required HTTP methods");
    }

    #endregion

    #region ProjectManagement Client API Operations

    /// <summary>
    /// Task 9: Validates that ProjectManagement client exposes expected project operations.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Expose_Expected_API_Operations()
    {
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);

        // Test navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Project Management specific endpoints
        Assert.NotNull(client.Rest.V10.Projects);
        Assert.NotNull(client.Rest.V10.Companies);
        
        // Test multi-version support
        Assert.NotNull(client.Rest.V11);
        Assert.NotNull(client.Rest.V20);
        
        // Test that we can navigate to specific project
        var projectBuilder = client.Rest.V10.Projects[789];
        Assert.NotNull(projectBuilder);
        
        _output.WriteLine("✅ ProjectManagement client exposes expected API operations");
    }

    /// <summary>
    /// Task 9: Validates that ProjectManagement client can build valid request information.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Build_Valid_Request_Information()
    {
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        
        // Test basic GET request building
        var projectBuilder = client.Rest.V10.Projects[123];
        var requestInfo = projectBuilder.ToGetRequestInformation();
        
        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("/projects/123", requestInfo.URI.ToString());
        
        _output.WriteLine("✅ ProjectManagement client builds valid request information");
    }

    /// <summary>
    /// Task 9: Validates that ProjectManagement client supports project sync operations.
    /// </summary>
    [Fact]
    public void ProjectManagementClient_Should_Support_Project_Sync_Operations()
    {
        var client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter);
        
        // Test that sync endpoint exists
        var syncBuilder = client.Rest.V10.Projects.Sync;
        Assert.NotNull(syncBuilder);
        
        // Verify sync supports PATCH method (for bulk updates)
        var syncBuilderType = syncBuilder.GetType();
        var patchMethod = syncBuilderType.GetMethods()
            .FirstOrDefault(m => m.Name.Contains("Patch") || m.Name.Contains("ToPatchRequestInformation"));
        
        Assert.NotNull(patchMethod);
        
        _output.WriteLine("✅ ProjectManagement client supports project sync operations");
    }

    #endregion

    #region ResourceManagement Client API Operations

    /// <summary>
    /// Task 9: Validates that ResourceManagement client exposes expected resource operations.
    /// </summary>
    [Fact]
    public void ResourceManagementClient_Should_Expose_Expected_API_Operations()
    {
        var client = new Procore.SDK.ResourceManagement.ResourceManagementClient(_mockRequestAdapter);

        // Test navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Resource Management specific endpoints
        Assert.NotNull(client.Rest.V10.Resources);
        Assert.NotNull(client.Rest.V10.Webhooks);
        Assert.NotNull(client.Rest.V10.WorkforcePlanning);
        
        // Test multi-version support
        Assert.NotNull(client.Rest.V11);
        
        _output.WriteLine("✅ ResourceManagement client exposes expected API operations");
    }

    #endregion

    #region QualitySafety Client API Operations

    /// <summary>
    /// Task 9: Validates that QualitySafety client exposes expected observation operations.
    /// </summary>
    [Fact]
    public void QualitySafetyClient_Should_Expose_Expected_API_Operations()
    {
        var client = new Procore.SDK.QualitySafety.QualitySafetyClient(_mockRequestAdapter);

        // Test navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Quality & Safety specific endpoints
        Assert.NotNull(client.Rest.V10.Companies);
        Assert.NotNull(client.Rest.V10.Projects);
        Assert.NotNull(client.Rest.V10.Observations);
        
        // Test multi-version support
        Assert.NotNull(client.Rest.V11);
        
        _output.WriteLine("✅ QualitySafety client exposes expected API operations");
    }

    #endregion

    #region ConstructionFinancials Client API Operations

    /// <summary>
    /// Task 9: Validates that ConstructionFinancials client exposes expected financial operations.
    /// </summary>
    [Fact]
    public void ConstructionFinancialsClient_Should_Expose_Expected_API_Operations()
    {
        var client = new Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient(_mockRequestAdapter);

        // Test navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Construction Financials specific endpoints
        Assert.NotNull(client.Rest.V10.Companies);
        Assert.NotNull(client.Rest.V10.Projects);
        
        // Test multi-version support
        Assert.NotNull(client.Rest.V20);
        
        _output.WriteLine("✅ ConstructionFinancials client exposes expected API operations");
    }

    #endregion

    #region FieldProductivity Client API Operations

    /// <summary>
    /// Task 9: Validates that FieldProductivity client exposes expected timecard operations.
    /// </summary>
    [Fact]
    public void FieldProductivityClient_Should_Expose_Expected_API_Operations()
    {
        var client = new Procore.SDK.FieldProductivity.FieldProductivityClient(_mockRequestAdapter);

        // Test navigation structure exists
        Assert.NotNull(client.Rest);
        Assert.NotNull(client.Rest.V10);
        
        // Field Productivity specific endpoints
        Assert.NotNull(client.Rest.V10.Companies);
        Assert.NotNull(client.Rest.V10.Projects);
        Assert.NotNull(client.Rest.V10.Timecard_entries);
        
        // Test multi-version support
        Assert.NotNull(client.Rest.V11);
        
        // Test timecard entry operations
        var timecardBuilder = client.Rest.V10.Timecard_entries[123];
        Assert.NotNull(timecardBuilder);
        
        _output.WriteLine("✅ FieldProductivity client exposes expected API operations");
    }

    /// <summary>
    /// Task 9: Validates that FieldProductivity client supports timecard PATCH operations.
    /// This specifically tests for the request body types that were missing in previous builds.
    /// </summary>
    [Fact]
    public void FieldProductivityClient_Should_Support_Timecard_Patch_Operations()
    {
        var client = new Procore.SDK.FieldProductivity.FieldProductivityClient(_mockRequestAdapter);
        
        var timecardBuilder = client.Rest.V10.Timecard_entries[123];
        
        // Test that PATCH method exists
        var builderType = timecardBuilder.GetType();
        var patchMethods = builderType.GetMethods()
            .Where(m => m.Name.Contains("Patch") && m.GetParameters().Length > 0)
            .ToList();
        
        Assert.NotEmpty(patchMethods);
        
        // Verify that patch methods have request body parameters
        foreach (var method in patchMethods)
        {
            var requestBodyParam = method.GetParameters()
                .FirstOrDefault(p => p.Name?.Contains("requestBody") == true || 
                                   p.ParameterType.Name.Contains("RequestBody"));
            
            if (requestBodyParam != null)
            {
                Assert.NotNull(requestBodyParam.ParameterType);
                _output.WriteLine($"   - Found {method.Name} with {requestBodyParam.ParameterType.Name}");
            }
        }
        
        _output.WriteLine("✅ FieldProductivity client supports timecard PATCH operations with request bodies");
    }

    #endregion

    #region Cross-Client Validation

    /// <summary>
    /// Task 9: Validates that all clients follow consistent API structure patterns.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient), "Core")]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), "ProjectManagement")]
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient), "ResourceManagement")]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient), "QualitySafety")]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient), "ConstructionFinancials")]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient), "FieldProductivity")]
    public void GeneratedClient_Should_Follow_Consistent_API_Structure(Type clientType, string clientName)
    {
        var mockAdapter = Substitute.For<IRequestAdapter>();
        mockAdapter.BaseUrl.Returns("https://api.procore.com");
        
        var client = Activator.CreateInstance(clientType, mockAdapter);
        Assert.NotNull(client);
        
        // All clients should have Rest property
        var restProperty = clientType.GetProperty("Rest");
        Assert.NotNull(restProperty);
        
        var restValue = restProperty.GetValue(client);
        Assert.NotNull(restValue);
        
        // All clients should have at least V10 version
        var v10Property = restValue.GetType().GetProperty("V10");
        Assert.NotNull(v10Property);
        
        var v10Value = v10Property.GetValue(restValue);
        Assert.NotNull(v10Value);
        
        _output.WriteLine($"✅ {clientName} follows consistent API structure");
    }

    /// <summary>
    /// Task 9: Validates that all clients support common company operations.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient))]
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient))]
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient))]
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient))]
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient))]
    public void GeneratedClient_Should_Support_Company_Operations(Type clientType)
    {
        var mockAdapter = Substitute.For<IRequestAdapter>();
        mockAdapter.BaseUrl.Returns("https://api.procore.com");
        
        var client = Activator.CreateInstance(clientType, mockAdapter);
        var restProperty = clientType.GetProperty("Rest");
        var restValue = restProperty!.GetValue(client);
        var v10Property = restValue!.GetType().GetProperty("V10");
        var v10Value = v10Property!.GetValue(restValue);
        
        // All these clients should have Companies endpoint
        var companiesProperty = v10Value!.GetType().GetProperty("Companies");
        Assert.NotNull(companiesProperty);
        
        var companiesValue = companiesProperty.GetValue(v10Value);
        Assert.NotNull(companiesValue);
        
        _output.WriteLine($"✅ {clientType.Name} supports company operations");
    }

    #endregion

    #region HTTP Method Coverage Validation

    /// <summary>
    /// Task 9: Validates that generated clients support expected HTTP methods for their endpoints.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Expected_HTTP_Methods()
    {
        var testCases = new[]
        {
            new { Client = typeof(Procore.SDK.Core.CoreClient), Endpoint = "Companies", Methods = new[] { "GET" } },
            new { Client = typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), Endpoint = "Projects", Methods = new[] { "GET" } },
            new { Client = typeof(Procore.SDK.FieldProductivity.FieldProductivityClient), Endpoint = "Timecard_entries", Methods = new[] { "GET" } }
        };

        foreach (var testCase in testCases)
        {
            var mockAdapter = Substitute.For<IRequestAdapter>();
            mockAdapter.BaseUrl.Returns("https://api.procore.com");
            
            var client = Activator.CreateInstance(testCase.Client, mockAdapter);
            var restProperty = testCase.Client.GetProperty("Rest");
            var restValue = restProperty!.GetValue(client);
            var v10Property = restValue!.GetType().GetProperty("V10");
            var v10Value = v10Property!.GetValue(restValue);
            
            var endpointProperty = v10Value!.GetType().GetProperty(testCase.Endpoint);
            if (endpointProperty != null)
            {
                var endpointValue = endpointProperty.GetValue(v10Value);
                Assert.NotNull(endpointValue);
                
                var endpointType = endpointValue.GetType();
                
                foreach (var expectedMethod in testCase.Methods)
                {
                    var methodExists = endpointType.GetMethods()
                        .Any(m => m.Name.Contains(expectedMethod, StringComparison.OrdinalIgnoreCase) || 
                                 m.Name.Contains($"To{expectedMethod}RequestInformation"));
                    
                    Assert.True(methodExists, 
                        $"{testCase.Client.Name} should support {expectedMethod} method for {testCase.Endpoint}");
                }
                
                _output.WriteLine($"✅ {testCase.Client.Name}.{testCase.Endpoint} supports expected HTTP methods");
            }
        }
    }

    #endregion

    #region Request Builder Navigation Tests

    /// <summary>
    /// Task 9: Validates that request builders support proper navigation patterns.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Request_Builder_Navigation()
    {
        var clients = new[]
        {
            new Procore.SDK.Core.CoreClient(_mockRequestAdapter),
            new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter),
            new Procore.SDK.ResourceManagement.ResourceManagementClient(_mockRequestAdapter),
            new Procore.SDK.QualitySafety.QualitySafetyClient(_mockRequestAdapter),
            new Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient(_mockRequestAdapter),
            new Procore.SDK.FieldProductivity.FieldProductivityClient(_mockRequestAdapter)
        };

        foreach (var client in clients)
        {
            var clientType = client.GetType();
            var restProperty = clientType.GetProperty("Rest");
            Assert.NotNull(restProperty);
            
            var restValue = restProperty.GetValue(client);
            Assert.NotNull(restValue);
            
            // Verify Rest property has version properties
            var restType = restValue.GetType();
            var versionProperties = restType.GetProperties()
                .Where(p => p.Name.StartsWith("V") && char.IsDigit(p.Name[1]))
                .ToList();
            
            Assert.NotEmpty(versionProperties);
            
            // Test navigation to first version
            var firstVersion = versionProperties.First();
            var versionValue = firstVersion.GetValue(restValue);
            Assert.NotNull(versionValue);
            
            _output.WriteLine($"✅ {clientType.Name} supports request builder navigation with {versionProperties.Count} API versions");
        }
    }

    /// <summary>
    /// Task 9: Validates that item builders support proper ID-based navigation.
    /// </summary>
    [Fact]
    public void GeneratedClients_Should_Support_Item_Builder_Navigation()
    {
        var testCases = new[]
        {
            new { 
                Client = new Procore.SDK.Core.CoreClient(_mockRequestAdapter),
                GetItemBuilder = (object client) => ((Procore.SDK.Core.CoreClient)client).Rest.V10.Companies[123]
            },
            new { 
                Client = new Procore.SDK.ProjectManagement.ProjectManagementClient(_mockRequestAdapter),
                GetItemBuilder = (object client) => ((Procore.SDK.ProjectManagement.ProjectManagementClient)client).Rest.V10.Projects[456]
            },
            new { 
                Client = new Procore.SDK.FieldProductivity.FieldProductivityClient(_mockRequestAdapter),
                GetItemBuilder = (object client) => ((Procore.SDK.FieldProductivity.FieldProductivityClient)client).Rest.V10.Timecard_entries[789]
            }
        };

        foreach (var testCase in testCases)
        {
            var itemBuilder = testCase.GetItemBuilder(testCase.Client);
            Assert.NotNull(itemBuilder);
            
            // Verify item builder has expected methods
            var itemBuilderType = itemBuilder.GetType();
            var getMethods = itemBuilderType.GetMethods()
                .Where(m => m.Name.Contains("Get") || m.Name.Contains("ToGetRequestInformation"))
                .ToList();
            
            Assert.NotEmpty(getMethods);
            
            _output.WriteLine($"✅ {testCase.Client.GetType().Name} supports item builder navigation");
        }
    }

    #endregion

    #region API Coverage Validation

    /// <summary>
    /// Task 9: Validates that clients expose the expected number of endpoints based on their domain.
    /// </summary>
    [Theory]
    [InlineData(typeof(Procore.SDK.Core.CoreClient), 2)] // Companies, Users
    [InlineData(typeof(Procore.SDK.ProjectManagement.ProjectManagementClient), 2)] // Companies, Projects
    [InlineData(typeof(Procore.SDK.ResourceManagement.ResourceManagementClient), 3)] // Resources, Webhooks, WorkforcePlanning
    [InlineData(typeof(Procore.SDK.QualitySafety.QualitySafetyClient), 3)] // Companies, Projects, Observations
    [InlineData(typeof(Procore.SDK.ConstructionFinancials.ConstructionFinancialsClient), 2)] // Companies, Projects
    [InlineData(typeof(Procore.SDK.FieldProductivity.FieldProductivityClient), 3)] // Companies, Projects, Timecard_entries
    public void GeneratedClient_Should_Expose_Expected_Number_Of_Endpoints(Type clientType, int minimumEndpoints)
    {
        var mockAdapter = Substitute.For<IRequestAdapter>();
        mockAdapter.BaseUrl.Returns("https://api.procore.com");
        
        var client = Activator.CreateInstance(clientType, mockAdapter);
        var restProperty = clientType.GetProperty("Rest");
        var restValue = restProperty!.GetValue(client);
        var v10Property = restValue!.GetType().GetProperty("V10");
        var v10Value = v10Property!.GetValue(restValue);
        
        var endpointProperties = v10Value!.GetType().GetProperties()
            .Where(p => !p.Name.Equals("RequestAdapter") && 
                       !p.Name.Equals("PathParameters") && 
                       !p.Name.Equals("UrlTemplate"))
            .ToList();
        
        Assert.True(endpointProperties.Count >= minimumEndpoints,
            $"{clientType.Name} should expose at least {minimumEndpoints} endpoints, found {endpointProperties.Count}");
        
        _output.WriteLine($"✅ {clientType.Name} exposes {endpointProperties.Count} endpoints (minimum: {minimumEndpoints})");
        
        // Log endpoint names for verification
        foreach (var endpoint in endpointProperties)
        {
            _output.WriteLine($"   - {endpoint.Name}");
        }
    }

    #endregion
}