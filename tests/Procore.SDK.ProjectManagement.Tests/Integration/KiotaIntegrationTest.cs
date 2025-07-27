using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
using NSubstitute;
using Xunit;

namespace Procore.SDK.ProjectManagement.Tests.Integration;

/// <summary>
/// Integration test to validate that the wrapper client can successfully
/// instantiate and work with the generated Kiota client.
/// </summary>
public class KiotaIntegrationTest
{
    [Fact]
    public void WrapperClient_CanInstantiateGeneratedClient()
    {
        // Arrange
        var httpClient = new HttpClient();
        var requestAdapter = new HttpClientRequestAdapter(new MockAuthenticationProvider());

        // Act & Assert - This should not throw
        var generatedClient = new Procore.SDK.ProjectManagement.ProjectManagementClient(requestAdapter);
        var wrapperClient = new ProcoreProjectManagementClient(requestAdapter);

        // Verify the wrapper can access the generated client
        Assert.NotNull(wrapperClient.RawClient);
        Assert.IsType<Procore.SDK.ProjectManagement.ProjectManagementClient>(wrapperClient.RawClient);
    }

    [Fact]
    public void GeneratedClient_HasExpectedProperties()
    {
        // Arrange
        var requestAdapter = new HttpClientRequestAdapter(new MockAuthenticationProvider());
        var generatedClient = new Procore.SDK.ProjectManagement.ProjectManagementClient(requestAdapter);

        // Act & Assert
        Assert.NotNull(generatedClient.Rest);
        // Note: RequestAdapter is protected, so we can't directly test BaseUrl here
        // The fact that the client instantiates successfully validates the integration
    }

    [Fact]
    public void GeneratedClient_CanAccessProjectsEndpoint()
    {
        // Arrange
        var requestAdapter = new HttpClientRequestAdapter(new MockAuthenticationProvider());
        var generatedClient = new Procore.SDK.ProjectManagement.ProjectManagementClient(requestAdapter);

        // Act & Assert - Basic endpoint access should not throw
        Assert.NotNull(generatedClient.Rest.V10.Projects);
    }

    /// <summary>
    /// Mock authentication provider for testing purposes
    /// </summary>
    private class MockAuthenticationProvider : IAuthenticationProvider
    {
        public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            request.Headers.TryAdd("Authorization", "Bearer test_token");
            return Task.CompletedTask;
        }
    }
}