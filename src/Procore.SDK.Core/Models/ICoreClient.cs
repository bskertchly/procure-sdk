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
    
    /// <summary>
    /// Gets all companies.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of companies.</returns>
    Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific company by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The company.</returns>
    Task<Company> GetCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new company.
    /// </summary>
    /// <param name="request">The company creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created company.</returns>
    Task<Company> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing company.
    /// </summary>
    /// <param name="companyId">The company ID to update.</param>
    /// <param name="request">The company update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated company.</returns>
    Task<Company> UpdateCompanyAsync(int companyId, UpdateCompanyRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a company.
    /// </summary>
    /// <param name="companyId">The company ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteCompanyAsync(int companyId, CancellationToken cancellationToken = default);

    // User Operations
    
    /// <summary>
    /// Gets all users for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of users.</returns>
    Task<IEnumerable<User>> GetUsersAsync(int companyId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific user by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The user.</returns>
    Task<User> GetUserAsync(int companyId, int userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created user.</returns>
    Task<User> CreateUserAsync(int companyId, CreateUserRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="userId">The user ID to update.</param>
    /// <param name="request">The user update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated user.</returns>
    Task<User> UpdateUserAsync(int companyId, int userId, UpdateUserRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deactivates a user.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="userId">The user ID to deactivate.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeactivateUserAsync(int companyId, int userId, CancellationToken cancellationToken = default);

    // Document Operations
    
    /// <summary>
    /// Gets all documents for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of documents.</returns>
    Task<IEnumerable<Document>> GetDocumentsAsync(int companyId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific document by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentId">The document ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The document.</returns>
    Task<Document> GetDocumentAsync(int companyId, int documentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Uploads a new document.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The document upload request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The uploaded document.</returns>
    Task<Document> UploadDocumentAsync(int companyId, UploadDocumentRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing document.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentId">The document ID to update.</param>
    /// <param name="request">The document update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated document.</returns>
    Task<Document> UpdateDocumentAsync(int companyId, int documentId, UpdateDocumentRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a document.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentId">The document ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteDocumentAsync(int companyId, int documentId, CancellationToken cancellationToken = default);

    // Custom Field Operations
    
    /// <summary>
    /// Gets all custom fields for a specific resource type.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceType">The resource type to get custom fields for.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of custom fields.</returns>
    Task<IEnumerable<CustomField>> GetCustomFieldsAsync(int companyId, string resourceType, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific custom field by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="fieldId">The custom field ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The custom field.</returns>
    Task<CustomField> GetCustomFieldAsync(int companyId, int fieldId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new custom field.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The custom field creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created custom field.</returns>
    Task<CustomField> CreateCustomFieldAsync(int companyId, CreateCustomFieldRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing custom field.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="fieldId">The custom field ID to update.</param>
    /// <param name="request">The custom field update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated custom field.</returns>
    Task<CustomField> UpdateCustomFieldAsync(int companyId, int fieldId, UpdateCustomFieldRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a custom field.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="fieldId">The custom field ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteCustomFieldAsync(int companyId, int fieldId, CancellationToken cancellationToken = default);

    // Convenience Methods
    
    /// <summary>
    /// Gets the current authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The current user.</returns>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a company by name.
    /// </summary>
    /// <param name="companyName">The name of the company to find.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The company with the specified name.</returns>
    Task<Company> GetCompanyByNameAsync(string companyName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Searches for users by a search term.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="searchTerm">The search term to match against user data.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of users matching the search term.</returns>
    Task<IEnumerable<User>> SearchUsersAsync(int companyId, string searchTerm, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets documents filtered by document type.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentType">The document type to filter by.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of documents of the specified type.</returns>
    Task<IEnumerable<Document>> GetDocumentsByTypeAsync(int companyId, string documentType, CancellationToken cancellationToken = default);

    // Pagination Support
    
    /// <summary>
    /// Gets companies with pagination support.
    /// </summary>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of companies.</returns>
    Task<PagedResult<Company>> GetCompaniesPagedAsync(PaginationOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets users with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of users.</returns>
    Task<PagedResult<User>> GetUsersPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets documents with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of documents.</returns>
    Task<PagedResult<Document>> GetDocumentsPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default);
}