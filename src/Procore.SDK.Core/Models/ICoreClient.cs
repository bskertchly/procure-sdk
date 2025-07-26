using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Core.Models;

/// <summary>
/// Defines the contract for the Core client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
/// </summary>
public interface ICoreClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }

    // Company Operations
    Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken cancellationToken = default);
    Task<Company> GetCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Company> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);
    Task<Company> UpdateCompanyAsync(int companyId, UpdateCompanyRequest request, CancellationToken cancellationToken = default);
    Task DeleteCompanyAsync(int companyId, CancellationToken cancellationToken = default);

    // User Operations
    Task<IEnumerable<User>> GetUsersAsync(int companyId, CancellationToken cancellationToken = default);
    Task<User> GetUserAsync(int companyId, int userId, CancellationToken cancellationToken = default);
    Task<User> CreateUserAsync(int companyId, CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<User> UpdateUserAsync(int companyId, int userId, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task DeactivateUserAsync(int companyId, int userId, CancellationToken cancellationToken = default);

    // Document Operations
    Task<IEnumerable<Document>> GetDocumentsAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Document> GetDocumentAsync(int companyId, int documentId, CancellationToken cancellationToken = default);
    Task<Document> UploadDocumentAsync(int companyId, UploadDocumentRequest request, CancellationToken cancellationToken = default);
    Task<Document> UpdateDocumentAsync(int companyId, int documentId, UpdateDocumentRequest request, CancellationToken cancellationToken = default);
    Task DeleteDocumentAsync(int companyId, int documentId, CancellationToken cancellationToken = default);

    // Custom Field Operations
    Task<IEnumerable<CustomField>> GetCustomFieldsAsync(int companyId, string resourceType, CancellationToken cancellationToken = default);
    Task<CustomField> GetCustomFieldAsync(int companyId, int fieldId, CancellationToken cancellationToken = default);
    Task<CustomField> CreateCustomFieldAsync(int companyId, CreateCustomFieldRequest request, CancellationToken cancellationToken = default);
    Task<CustomField> UpdateCustomFieldAsync(int companyId, int fieldId, UpdateCustomFieldRequest request, CancellationToken cancellationToken = default);
    Task DeleteCustomFieldAsync(int companyId, int fieldId, CancellationToken cancellationToken = default);

    // Convenience Methods
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    Task<Company> GetCompanyByNameAsync(string companyName, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> SearchUsersAsync(int companyId, string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Document>> GetDocumentsByTypeAsync(int companyId, string documentType, CancellationToken cancellationToken = default);

    // Pagination Support
    Task<PagedResult<Company>> GetCompaniesPagedAsync(PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetUsersPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default);
    Task<PagedResult<Document>> GetDocumentsPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default);
}