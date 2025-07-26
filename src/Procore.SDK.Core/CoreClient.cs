using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.ErrorHandling;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core;

/// <summary>
/// Implementation of the Core client wrapper that provides domain-specific 
/// convenience methods over the generated Kiota client.
/// </summary>
public class ProcoreCoreClient : ICoreClient
{
    private readonly Procore.SDK.Core.CoreClient _generatedClient;
    private readonly ErrorMapper _errorMapper;
    private readonly ILogger<ProcoreCoreClient>? _logger;
    private bool _disposed;

    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    public object RawClient => _generatedClient;

    /// <summary>
    /// Initializes a new instance of the ProcoreCoreClient.
    /// </summary>
    /// <param name="requestAdapter">The request adapter to use for HTTP communication.</param>
    /// <param name="logger">Optional logger for diagnostic information.</param>
    public ProcoreCoreClient(IRequestAdapter requestAdapter, ILogger<ProcoreCoreClient>? logger = null)
    {
        _generatedClient = new Procore.SDK.Core.CoreClient(requestAdapter);
        _errorMapper = new ErrorMapper();
        _logger = logger;
    }

    #region Company Operations

    /// <summary>
    /// Gets all companies accessible to the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of companies.</returns>
    public async Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting companies");
            
            // Note: This is a placeholder implementation
            // The actual implementation would call the generated Kiota client
            // and map the response to domain models
            
            // For now, return empty collection to make tests compile
            return Enumerable.Empty<Company>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get companies");
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets a specific company by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The company.</returns>
    public async Task<Company> GetCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting company {CompanyId}", companyId);
            
            // Placeholder implementation
            return new Company { Id = companyId, Name = "Placeholder Company" };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get company {CompanyId}", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Creates a new company.
    /// </summary>
    /// <param name="request">The company creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created company.</returns>
    public async Task<Company> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Creating company {CompanyName}", request.Name);
            
            // Placeholder implementation
            return new Company 
            { 
                Id = 1, 
                Name = request.Name, 
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create company {CompanyName}", request.Name);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Updates an existing company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The company update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated company.</returns>
    public async Task<Company> UpdateCompanyAsync(int companyId, UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Updating company {CompanyId}", companyId);
            
            // Placeholder implementation
            return new Company 
            { 
                Id = companyId, 
                Name = request.Name ?? "Updated Company",
                Description = request.Description,
                IsActive = request.IsActive ?? true,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update company {CompanyId}", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Deletes a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Deleting company {CompanyId}", companyId);
            
            // Placeholder implementation
            await Task.CompletedTask;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to delete company {CompanyId}", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    #endregion

    #region User Operations

    /// <summary>
    /// Gets all users for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of users.</returns>
    public async Task<IEnumerable<User>> GetUsersAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting users for company {CompanyId}", companyId);
            
            // Placeholder implementation
            return Enumerable.Empty<User>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get users for company {CompanyId}", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets a specific user by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The user.</returns>
    public async Task<User> GetUserAsync(int companyId, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting user {UserId} for company {CompanyId}", userId, companyId);
            
            // Placeholder implementation
            return new User 
            { 
                Id = userId, 
                Email = "placeholder@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsActive = true
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get user {UserId} for company {CompanyId}", userId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created user.</returns>
    public async Task<User> CreateUserAsync(int companyId, CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Creating user {Email} for company {CompanyId}", request.Email, companyId);
            
            // Placeholder implementation
            return new User 
            { 
                Id = 1,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                JobTitle = request.JobTitle,
                PhoneNumber = request.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create user {Email} for company {CompanyId}", request.Email, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="request">The user update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated user.</returns>
    public async Task<User> UpdateUserAsync(int companyId, int userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Updating user {UserId} for company {CompanyId}", userId, companyId);
            
            // Placeholder implementation
            return new User 
            { 
                Id = userId,
                Email = request.Email ?? "updated@example.com",
                FirstName = request.FirstName ?? "Updated",
                LastName = request.LastName ?? "User",
                JobTitle = request.JobTitle,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.IsActive ?? true,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update user {UserId} for company {CompanyId}", userId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Deactivates a user.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeactivateUserAsync(int companyId, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Deactivating user {UserId} for company {CompanyId}", userId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to deactivate user {UserId} for company {CompanyId}", userId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    #endregion

    #region Document Operations

    /// <summary>
    /// Gets all documents for a company.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of documents.</returns>
    public async Task<IEnumerable<Document>> GetDocumentsAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting documents for company {CompanyId}", companyId);
            
            // Placeholder implementation
            return Enumerable.Empty<Document>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get documents for company {CompanyId}", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets a specific document by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentId">The document ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The document.</returns>
    public async Task<Document> GetDocumentAsync(int companyId, int documentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting document {DocumentId} for company {CompanyId}", documentId, companyId);
            
            // Placeholder implementation
            return new Document 
            { 
                Id = documentId,
                Name = "Placeholder Document",
                FileName = "placeholder.pdf",
                FileUrl = "https://example.com/placeholder.pdf",
                ContentType = "application/pdf",
                FileSize = 1024
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get document {DocumentId} for company {CompanyId}", documentId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Uploads a new document.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The document upload request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The uploaded document.</returns>
    public async Task<Document> UploadDocumentAsync(int companyId, UploadDocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Uploading document {DocumentName} for company {CompanyId}", request.Name, companyId);
            
            // Placeholder implementation
            return new Document 
            { 
                Id = 1,
                Name = request.Name,
                Description = request.Description,
                FileName = request.FileName,
                ContentType = request.ContentType,
                FileSize = request.FileStream.Length,
                IsPrivate = request.IsPrivate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to upload document {DocumentName} for company {CompanyId}", request.Name, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Updates an existing document.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentId">The document ID.</param>
    /// <param name="request">The document update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated document.</returns>
    public async Task<Document> UpdateDocumentAsync(int companyId, int documentId, UpdateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Updating document {DocumentId} for company {CompanyId}", documentId, companyId);
            
            // Placeholder implementation
            return new Document 
            { 
                Id = documentId,
                Name = request.Name ?? "Updated Document",
                Description = request.Description,
                IsPrivate = request.IsPrivate ?? false,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update document {DocumentId} for company {CompanyId}", documentId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Deletes a document.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentId">The document ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteDocumentAsync(int companyId, int documentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Deleting document {DocumentId} for company {CompanyId}", documentId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to delete document {DocumentId} for company {CompanyId}", documentId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    #endregion

    #region Custom Field Operations

    /// <summary>
    /// Gets all custom fields for a company and resource type.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="resourceType">The resource type.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of custom fields.</returns>
    public async Task<IEnumerable<CustomField>> GetCustomFieldsAsync(int companyId, string resourceType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(resourceType)) throw new ArgumentException("Resource type cannot be null or empty", nameof(resourceType));
        
        try
        {
            _logger?.LogDebug("Getting custom fields for company {CompanyId} and resource type {ResourceType}", companyId, resourceType);
            
            // Placeholder implementation
            return Enumerable.Empty<CustomField>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get custom fields for company {CompanyId} and resource type {ResourceType}", companyId, resourceType);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets a specific custom field by ID.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="fieldId">The custom field ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The custom field.</returns>
    public async Task<CustomField> GetCustomFieldAsync(int companyId, int fieldId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting custom field {FieldId} for company {CompanyId}", fieldId, companyId);
            
            // Placeholder implementation
            return new CustomField 
            { 
                Id = fieldId,
                Name = "Placeholder Field",
                FieldType = "string",
                ResourceType = "project",
                IsRequired = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get custom field {FieldId} for company {CompanyId}", fieldId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Creates a new custom field.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="request">The custom field creation request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The created custom field.</returns>
    public async Task<CustomField> CreateCustomFieldAsync(int companyId, CreateCustomFieldRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Creating custom field {FieldName} for company {CompanyId}", request.Name, companyId);
            
            // Placeholder implementation
            return new CustomField 
            { 
                Id = 1,
                Name = request.Name,
                FieldType = request.FieldType,
                ResourceType = request.ResourceType,
                IsRequired = request.IsRequired,
                DefaultValue = request.DefaultValue,
                AllowedValues = request.AllowedValues,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to create custom field {FieldName} for company {CompanyId}", request.Name, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Updates an existing custom field.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="fieldId">The custom field ID.</param>
    /// <param name="request">The custom field update request.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The updated custom field.</returns>
    public async Task<CustomField> UpdateCustomFieldAsync(int companyId, int fieldId, UpdateCustomFieldRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        try
        {
            _logger?.LogDebug("Updating custom field {FieldId} for company {CompanyId}", fieldId, companyId);
            
            // Placeholder implementation
            return new CustomField 
            { 
                Id = fieldId,
                Name = request.Name ?? "Updated Field",
                IsRequired = request.IsRequired ?? false,
                DefaultValue = request.DefaultValue,
                AllowedValues = request.AllowedValues,
                UpdatedAt = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to update custom field {FieldId} for company {CompanyId}", fieldId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Deletes a custom field.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="fieldId">The custom field ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    public async Task DeleteCustomFieldAsync(int companyId, int fieldId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Deleting custom field {FieldId} for company {CompanyId}", fieldId, companyId);
            
            // Placeholder implementation
            await Task.CompletedTask;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to delete custom field {FieldId} for company {CompanyId}", fieldId, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    #endregion

    #region Convenience Methods

    /// <summary>
    /// Gets the current authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The current user.</returns>
    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting current user");
            
            // Placeholder implementation
            return new User 
            { 
                Id = 1,
                Email = "current@example.com",
                FirstName = "Current",
                LastName = "User",
                IsActive = true
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get current user");
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets a company by name.
    /// </summary>
    /// <param name="companyName">The company name.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The company.</returns>
    public async Task<Company> GetCompanyByNameAsync(string companyName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(companyName)) throw new ArgumentException("Company name cannot be null or empty", nameof(companyName));
        
        try
        {
            _logger?.LogDebug("Getting company by name {CompanyName}", companyName);
            
            // Placeholder implementation
            return new Company 
            { 
                Id = 1,
                Name = companyName,
                IsActive = true
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get company by name {CompanyName}", companyName);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Searches for users by a search term.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of matching users.</returns>
    public async Task<IEnumerable<User>> SearchUsersAsync(int companyId, string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(searchTerm)) throw new ArgumentException("Search term cannot be null or empty", nameof(searchTerm));
        
        try
        {
            _logger?.LogDebug("Searching users for company {CompanyId} with term {SearchTerm}", companyId, searchTerm);
            
            // Placeholder implementation
            return Enumerable.Empty<User>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to search users for company {CompanyId} with term {SearchTerm}", companyId, searchTerm);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets documents filtered by type.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="documentType">The document type.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of documents of the specified type.</returns>
    public async Task<IEnumerable<Document>> GetDocumentsByTypeAsync(int companyId, string documentType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(documentType)) throw new ArgumentException("Document type cannot be null or empty", nameof(documentType));
        
        try
        {
            _logger?.LogDebug("Getting documents of type {DocumentType} for company {CompanyId}", documentType, companyId);
            
            // Placeholder implementation
            return Enumerable.Empty<Document>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get documents of type {DocumentType} for company {CompanyId}", documentType, companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    #endregion

    #region Pagination Support

    /// <summary>
    /// Gets companies with pagination support.
    /// </summary>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of companies.</returns>
    public async Task<PagedResult<Company>> GetCompaniesPagedAsync(PaginationOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        
        try
        {
            _logger?.LogDebug("Getting companies with pagination (page {Page}, per page {PerPage})", options.Page, options.PerPage);
            
            // Placeholder implementation
            return new PagedResult<Company>
            {
                Items = Enumerable.Empty<Company>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get companies with pagination");
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets users with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of users.</returns>
    public async Task<PagedResult<User>> GetUsersPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        
        try
        {
            _logger?.LogDebug("Getting users for company {CompanyId} with pagination (page {Page}, per page {PerPage})", companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new PagedResult<User>
            {
                Items = Enumerable.Empty<User>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get users for company {CompanyId} with pagination", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    /// <summary>
    /// Gets documents with pagination support.
    /// </summary>
    /// <param name="companyId">The company ID.</param>
    /// <param name="options">Pagination options.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A paged result of documents.</returns>
    public async Task<PagedResult<Document>> GetDocumentsPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        
        try
        {
            _logger?.LogDebug("Getting documents for company {CompanyId} with pagination (page {Page}, per page {PerPage})", companyId, options.Page, options.PerPage);
            
            // Placeholder implementation
            return new PagedResult<Document>
            {
                Items = Enumerable.Empty<Document>(),
                TotalCount = 0,
                Page = options.Page,
                PerPage = options.PerPage,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false
            };
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Failed to get documents for company {CompanyId} with pagination", companyId);
            throw _errorMapper.MapHttpException(ex);
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Disposes of the ProcoreCoreClient and its resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the ProcoreCoreClient and its resources.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // The generated client doesn't implement IDisposable, so we don't dispose it
            _disposed = true;
        }
    }

    #endregion
}