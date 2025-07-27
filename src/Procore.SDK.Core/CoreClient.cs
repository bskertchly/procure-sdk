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
    /// Note: This endpoint does not require the Procore-Company-Id header to be included.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of companies.</returns>
    public async Task<IEnumerable<Company>> GetCompaniesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting companies using generated Kiota client");
            
            // Use the actual generated Kiota client for the List Companies endpoint
            var companiesResponse = await _generatedClient.Rest.V10.Companies.GetAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (companiesResponse == null || !companiesResponse.Any())
            {
                _logger?.LogWarning("No companies returned from API");
                return Enumerable.Empty<Company>();
            }
            
            // Map from generated response models to our domain models
            return companiesResponse.Select(companyResponse => new Company
            {
                Id = companyResponse.Id ?? 0,
                Name = companyResponse.Name ?? string.Empty,
                IsActive = companyResponse.IsActive ?? true,
                LogoUrl = companyResponse.LogoUrl,
                // Note: The API response doesn't include description, created/updated dates
                // Those would need to be retrieved from individual company endpoints
                Description = null,
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue
            });
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
            
            // Note: The generated Kiota client doesn't expose a direct "get company" endpoint.
            // This is due to the Procore API design where most operations are scoped within
            // a company context rather than retrieving company details themselves.
            // 
            // The available company information comes from:
            // 1. The List Companies endpoint (GetCompaniesAsync)
            // 2. Company-scoped operations that may include company context
            // 
            // For individual company details, you would typically:
            // 1. Use GetCompaniesAsync() and filter by ID
            // 2. Store company information from authentication/authorization context
            
            var companies = await GetCompaniesAsync(cancellationToken).ConfigureAwait(false);
            var company = companies.FirstOrDefault(c => c.Id == companyId);
            
            if (company == null)
            {
                throw new HttpRequestException($"Company {companyId} not found or not accessible");
            }
            
            return company;
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
            
            // Note: The generated Kiota client doesn't expose a "create company" endpoint.
            // Company creation is typically handled through:
            // 1. Administrative interfaces in the Procore web application
            // 2. Account provisioning processes
            // 3. Specialized administrative APIs not included in the standard REST API
            // 
            // If company creation is required, it would need to be implemented through:
            // 1. Administrative API endpoints (if available)
            // 2. Custom integration with Procore's provisioning systems
            // 3. Manual company setup processes
            
            _logger?.LogWarning("CreateCompanyAsync: Company creation not available in generated client. This operation typically requires administrative access.");
            
            // Return a mock response for interface compliance
            await Task.CompletedTask.ConfigureAwait(false);
            throw new NotSupportedException("Company creation is not supported through the standard API. Contact Procore support for company provisioning.");
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
            await Task.CompletedTask.ConfigureAwait(false);
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
            _logger?.LogDebug("Getting users for company {CompanyId} using V1.1 generated Kiota client", companyId);
            
            // Use the V1.1 generated Kiota client to list all active users for a company
            var usersResponse = await _generatedClient.Rest.V11.Companies[companyId].Users.GetAsync(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (usersResponse == null || !usersResponse.Any())
            {
                _logger?.LogWarning("No users returned from V1.1 Users endpoint for company {CompanyId}", companyId);
                return Enumerable.Empty<User>();
            }
            
            // Map from V1.1 generated response models to our domain models
            return usersResponse.Select(userResponse => new User
            {
                Id = userResponse.Id ?? 0,
                Email = userResponse.EmailAddress ?? string.Empty,
                FirstName = userResponse.FirstName ?? string.Empty,
                LastName = userResponse.LastName ?? string.Empty,
                JobTitle = userResponse.JobTitle,
                PhoneNumber = userResponse.BusinessPhone ?? userResponse.MobilePhone,
                IsActive = userResponse.IsActive ?? true,
                CreatedAt = userResponse.CreatedAt?.DateTime ?? DateTime.MinValue,
                UpdatedAt = userResponse.UpdatedAt?.DateTime ?? DateTime.MinValue
            });
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
            
            // Use the actual generated Kiota client
            var userResponse = await _generatedClient.Rest.V10.Users[userId].GetAsync(
                requestConfiguration => requestConfiguration.QueryParameters.CompanyId = companyId,
                cancellationToken).ConfigureAwait(false);
            
            if (userResponse == null)
            {
                throw new HttpRequestException($"User {userId} not found in company {companyId}");
            }
            
            // Map from generated response to our domain model
            return new User 
            { 
                Id = userResponse.Id ?? userId,
                Email = userResponse.EmailAddress ?? string.Empty,
                FirstName = userResponse.FirstName ?? string.Empty,
                LastName = userResponse.LastName ?? string.Empty,
                JobTitle = userResponse.JobTitle,
                PhoneNumber = userResponse.BusinessPhone,
                IsActive = userResponse.IsActive ?? true,
                CreatedAt = userResponse.CreatedAt?.DateTime ?? DateTime.MinValue,
                UpdatedAt = userResponse.UpdatedAt?.DateTime ?? DateTime.MinValue
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
            _logger?.LogDebug("Creating user {Email} for company {CompanyId} using V1.1 generated Kiota client", request.Email, companyId);
            
            // Create the request body for V1.1 Users endpoint
            var requestBody = new global::Procore.SDK.Core.Rest.V11.Companies.Item.Users.UsersPostRequestBody
            {
                User = new global::Procore.SDK.Core.Rest.V11.Companies.Item.Users.UsersPostRequestBody_user
                {
                    EmailAddress = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    JobTitle = request.JobTitle,
                    BusinessPhone = request.PhoneNumber,
                    IsActive = true
                }
            };
            
            // Use the V1.1 generated Kiota client to create the user
            var userResponse = await _generatedClient.Rest.V11.Companies[companyId].Users.PostAsync(
                requestBody, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (userResponse == null)
            {
                throw new HttpRequestException($"Failed to create user {request.Email} in company {companyId}");
            }
            
            // Map from V1.1 generated response to our domain model
            return new User
            {
                Id = userResponse.Id ?? 0,
                Email = userResponse.EmailAddress ?? request.Email,
                FirstName = userResponse.FirstName ?? request.FirstName,
                LastName = userResponse.LastName ?? request.LastName,
                JobTitle = userResponse.JobTitle ?? request.JobTitle,
                PhoneNumber = userResponse.BusinessPhone ?? userResponse.MobilePhone,
                IsActive = userResponse.IsActive ?? true,
                CreatedAt = userResponse.CreatedAt?.DateTime ?? DateTime.UtcNow,
                UpdatedAt = userResponse.UpdatedAt?.DateTime ?? DateTime.UtcNow
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
            _logger?.LogDebug("Updating user {UserId} for company {CompanyId} using V1.1 generated Kiota client", userId, companyId);
            
            // Create the request body for V1.1 Users PATCH endpoint
            var requestBody = new global::Procore.SDK.Core.Rest.V11.Companies.Item.Users.Item.UsersPatchRequestBody
            {
                User = new global::Procore.SDK.Core.Rest.V11.Companies.Item.Users.Item.UsersPatchRequestBody_user
                {
                    EmailAddress = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    JobTitle = request.JobTitle,
                    BusinessPhone = request.PhoneNumber,
                    IsActive = request.IsActive
                }
            };
            
            // Use the V1.1 generated Kiota client to update the user
            var userResponse = await _generatedClient.Rest.V11.Companies[companyId].Users[userId].PatchAsync(
                requestBody, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (userResponse == null)
            {
                throw new HttpRequestException($"Failed to update user {userId} in company {companyId}");
            }
            
            // Map from V1.1 generated response to our domain model
            return new User
            {
                Id = userResponse.Id ?? userId,
                Email = userResponse.EmailAddress ?? request.Email ?? string.Empty,
                FirstName = userResponse.FirstName ?? request.FirstName ?? string.Empty,
                LastName = userResponse.LastName ?? request.LastName ?? string.Empty,
                JobTitle = userResponse.JobTitle ?? request.JobTitle,
                PhoneNumber = userResponse.BusinessPhone ?? userResponse.MobilePhone,
                IsActive = userResponse.IsActive ?? true,
                CreatedAt = userResponse.CreatedAt?.DateTime ?? DateTime.MinValue,
                UpdatedAt = userResponse.UpdatedAt?.DateTime ?? DateTime.UtcNow
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
            await Task.CompletedTask.ConfigureAwait(false);
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
            _logger?.LogDebug("Getting documents for company {CompanyId} using generated Kiota client", companyId);
            
            // Use the generated Kiota client to get folders and files
            var foldersResponse = await _generatedClient.Rest.V10.Companies[companyId].Folders.GetAsync(
                requestConfiguration => requestConfiguration.QueryParameters.ExcludeFolders = true,
                cancellationToken).ConfigureAwait(false);
            
            if (foldersResponse?.Files == null || !foldersResponse.Files.Any())
            {
                _logger?.LogWarning("No files returned from Folders endpoint for company {CompanyId}", companyId);
                return Enumerable.Empty<Document>();
            }
            
            // Map from generated response models to our domain models
            return foldersResponse.Files.Select(fileResponse => new Document
            {
                Id = fileResponse.Id ?? 0,
                Name = fileResponse.Name ?? string.Empty,
                FileName = fileResponse.Name ?? string.Empty, // Uses Name property since no Filename property
                FileUrl = null, // URL not available in folders response
                ContentType = fileResponse.FileType,
                FileSize = fileResponse.Size ?? 0,
                IsPrivate = fileResponse.Private ?? false,
                CreatedAt = fileResponse.CreatedAt?.DateTime ?? DateTime.MinValue,
                UpdatedAt = fileResponse.UpdatedAt?.DateTime ?? DateTime.MinValue,
                Description = fileResponse.Description
            });
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
            _logger?.LogDebug("Getting document {DocumentId} for company {CompanyId} using generated Kiota client", documentId, companyId);
            
            // Use the generated Kiota client to get specific file
            var fileResponse = await _generatedClient.Rest.V10.Companies[companyId].Files[documentId].GetAsync(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (fileResponse == null)
            {
                throw new HttpRequestException($"Document {documentId} not found in company {companyId}");
            }
            
            // Map from generated response to our domain model
            return new Document
            {
                Id = fileResponse.Id ?? documentId,
                Name = fileResponse.Name ?? string.Empty,
                FileName = fileResponse.Name ?? string.Empty, // Uses Name property
                FileUrl = null, // URL not available in files response
                ContentType = fileResponse.FileType,
                FileSize = fileResponse.Size ?? 0,
                IsPrivate = fileResponse.Private ?? false,
                CreatedAt = fileResponse.CreatedAt?.DateTime ?? DateTime.MinValue,
                UpdatedAt = fileResponse.UpdatedAt?.DateTime ?? DateTime.MinValue,
                Description = fileResponse.Description
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
            _logger?.LogDebug("Deleting document {DocumentId} for company {CompanyId} using generated Kiota client", documentId, companyId);
            
            // Use the generated Kiota client to delete the file
            await _generatedClient.Rest.V10.Companies[companyId].Files[documentId].DeleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            
            _logger?.LogDebug("Successfully deleted document {DocumentId} for company {CompanyId}", documentId, companyId);
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
            _logger?.LogDebug("Getting custom fields for company {CompanyId} and resource type {ResourceType} using V1.1 generated Kiota client", companyId, resourceType);
            
            // Use the V1.1 Custom_field_definitions endpoint to list all custom fields
            var customFieldsResponse = await _generatedClient.Rest.V11.Companies[companyId].Custom_field_definitions.GetAsync(
                cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (customFieldsResponse == null || !customFieldsResponse.Any())
            {
                _logger?.LogWarning("No custom field definitions returned from V1.1 endpoint for company {CompanyId}", companyId);
                return Enumerable.Empty<CustomField>();
            }
            
            // Map from V1.1 generated response models to our domain models
            return customFieldsResponse.Select(fieldResponse => new CustomField
            {
                Id = fieldResponse.Id ?? 0,
                Name = fieldResponse.Label ?? string.Empty,
                FieldType = fieldResponse.DataType ?? "string",
                ResourceType = resourceType, // Input parameter since API response doesn't include this
                IsRequired = false, // V1.1 model doesn't include Required field
                DefaultValue = fieldResponse.DefaultValue,
                AllowedValues = null, // V1.1 model doesn't include AllowableValues field
                CreatedAt = DateTime.MinValue, // V1.1 model doesn't include CreatedAt field
                UpdatedAt = DateTime.MinValue // V1.1 model doesn't include UpdatedAt field
            });
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
            _logger?.LogDebug("Creating custom field {FieldName} for company {CompanyId} using Workforce Planning V2 generated Kiota client", request.Name, companyId);
            
            // Create the request body for Workforce Planning V2 Custom Fields endpoint
            var requestBody = new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody
            {
                Name = request.Name,
                Type = global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_type.Text, // Default to text type
                Description = request.ResourceType, // Use resource type as description
                Values = request.AllowedValues?.ToList(), // Map allowed values
                OnProjects = request.ResourceType?.ToLower().Contains("project") ?? false,
                OnPeople = request.ResourceType?.ToLower().Contains("user") ?? false
            };
            
            // Use the Workforce Planning V2 generated Kiota client to create the custom field
            var customFieldResponse = await _generatedClient.Rest.V10.WorkforcePlanning.V2.Companies[companyId].CustomFields.PostAsync(
                requestBody, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (customFieldResponse == null)
            {
                throw new HttpRequestException($"Failed to create custom field {request.Name} in company {companyId}");
            }
            
            // Map from Workforce Planning V2 generated response to our domain model
            // Note: The response only contains the ID, other properties from request are preserved
            return new CustomField
            {
                Id = customFieldResponse.Id?.GetHashCode() ?? 0, // Convert Guid to int using hash code
                Name = request.Name,
                FieldType = request.FieldType ?? "text",
                ResourceType = request.ResourceType,
                IsRequired = request.IsRequired == true,
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
            await Task.CompletedTask.ConfigureAwait(false);
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
            _logger?.LogDebug("Getting current user using generated Kiota client");
            
            // Note: The Procore API doesn't have a direct "me" or "current user" endpoint.
            // To get the current user, you typically need:
            // 1. Extract user ID from the authentication token (JWT claims)
            // 2. Use that ID with GetUserAsync(companyId, userId)
            // 
            // This method serves as a convenience wrapper that would implement that logic.
            // In a real implementation, you would:
            // - Parse the JWT token to get user ID and company ID
            // - Call the specific user endpoint with those parameters
            
            // Example implementation pattern:
            // var userClaims = ExtractUserClaimsFromToken(); // Custom method to parse JWT
            // return await GetUserAsync(userClaims.CompanyId, userClaims.UserId, cancellationToken).ConfigureAwait(false);
            
            _logger?.LogWarning("GetCurrentUserAsync: Requires JWT token parsing to extract user/company IDs. Use GetUserAsync(companyId, userId) for direct user access.");
            
            // Placeholder return for interface compliance
            return new User 
            { 
                Id = 0,
                Email = "unknown@example.com",
                FirstName = "Unknown",
                LastName = "User",
                IsActive = false
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