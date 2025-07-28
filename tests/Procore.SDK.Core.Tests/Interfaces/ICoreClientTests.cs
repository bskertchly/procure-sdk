using Procore.SDK.Core.Tests.Models;

namespace Procore.SDK.Core.Tests.Interfaces;

/// <summary>
/// Tests that define the expected ICoreClient interface contract.
/// These tests serve as specifications for the wrapper API that should hide
/// the complexity of the generated Kiota client.
/// </summary>
public class ICoreClientTests
{
    [Fact]
    public void ICoreClient_Should_Define_Company_Operations()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Company CRUD operations
        interfaceType.Should().HaveMethod("GetCompaniesAsync", new[] { typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("GetCompanyAsync", new[] { typeof(int), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("CreateCompanyAsync", new[] { typeof(CreateCompanyRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("UpdateCompanyAsync", new[] { typeof(int), typeof(UpdateCompanyRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("DeleteCompanyAsync", new[] { typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void ICoreClient_Should_Define_User_Operations()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - User CRUD operations
        interfaceType.Should().HaveMethod("GetUsersAsync", new[] { typeof(int), typeof(CancellationToken) }); // company_id required
        interfaceType.Should().HaveMethod("GetUserAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) }); // company_id, user_id
        interfaceType.Should().HaveMethod("CreateUserAsync", new[] { typeof(int), typeof(CreateUserRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("UpdateUserAsync", new[] { typeof(int), typeof(int), typeof(UpdateUserRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("DeactivateUserAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void ICoreClient_Should_Define_Document_Operations()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Document operations
        interfaceType.Should().HaveMethod("GetDocumentsAsync", new[] { typeof(int), typeof(CancellationToken) }); // company_id required
        interfaceType.Should().HaveMethod("GetDocumentAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) }); // company_id, document_id
        interfaceType.Should().HaveMethod("UploadDocumentAsync", new[] { typeof(int), typeof(UploadDocumentRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("UpdateDocumentAsync", new[] { typeof(int), typeof(int), typeof(UpdateDocumentRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("DeleteDocumentAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void ICoreClient_Should_Define_CustomField_Operations()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Custom Field operations
        interfaceType.Should().HaveMethod("GetCustomFieldsAsync", new[] { typeof(int), typeof(string), typeof(CancellationToken) }); // company_id, resource_type
        interfaceType.Should().HaveMethod("GetCustomFieldAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) }); // company_id, field_id
        interfaceType.Should().HaveMethod("CreateCustomFieldAsync", new[] { typeof(int), typeof(CreateCustomFieldRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("UpdateCustomFieldAsync", new[] { typeof(int), typeof(int), typeof(UpdateCustomFieldRequest), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("DeleteCustomFieldAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void ICoreClient_Should_Define_Convenience_Methods()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Convenience methods for common operations
        interfaceType.Should().HaveMethod("GetCurrentUserAsync", new[] { typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("GetCompanyByNameAsync", new[] { typeof(string), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("SearchUsersAsync", new[] { typeof(int), typeof(string), typeof(CancellationToken) }); // company_id, search_term
        interfaceType.Should().HaveMethod("GetDocumentsByTypeAsync", new[] { typeof(int), typeof(string), typeof(CancellationToken) }); // company_id, document_type
    }

    [Fact]
    public void ICoreClient_Should_Define_Pagination_Support()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Paginated methods
        interfaceType.Should().HaveMethod("GetCompaniesPagedAsync", new[] { typeof(PaginationOptions), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("GetUsersPagedAsync", new[] { typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
        interfaceType.Should().HaveMethod("GetDocumentsPagedAsync", new[] { typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
    }

    [Fact]
    public void ICoreClient_Should_Inherit_From_IDisposable()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Should implement IDisposable for proper resource cleanup
        interfaceType.Should().Implement<IDisposable>();
    }

    [Fact]
    public void ICoreClient_Should_Provide_Raw_Client_Access()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert - Should provide access to the underlying Kiota client for advanced scenarios
        interfaceType.Should().HaveProperty<object>("RawClient");
    }
}