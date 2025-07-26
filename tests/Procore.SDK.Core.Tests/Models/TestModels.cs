namespace Procore.SDK.Core.Models;

/// <summary>
/// Test model definitions for TDD development of Core Client.
/// These models define the expected API surface for the wrapper client.
/// </summary>

#region Core Client Interface

/// <summary>
/// Defines the contract for the Core client wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
/// </summary>
public interface ICoreClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; } // TODO: Replace with CoreClient when available

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

#endregion

#region Domain Models

/// <summary>
/// Represents a Procore company.
/// </summary>
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? LogoUrl { get; set; }
    public Address? Address { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Represents a Procore user.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? JobTitle { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastSignInAt { get; set; }
    public string? AvatarUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public Company? Company { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Represents a Procore document.
/// </summary>
public class Document
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User? CreatedBy { get; set; }
    public bool IsPrivate { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Represents a Procore custom field definition.
/// </summary>
public class CustomField
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? DefaultValue { get; set; }
    public string[]? AllowedValues { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents an address.
/// </summary>
public class Address
{
    public string? Street1 { get; set; }
    public string? Street2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}

#endregion

#region Request Models

/// <summary>
/// Request model for creating a company.
/// </summary>
public class CreateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Address? Address { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for updating a company.
/// </summary>
public class UpdateCompanyRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public Address? Address { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for creating a user.
/// </summary>
public class CreateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for updating a user.
/// </summary>
public class UpdateUserRequest
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for uploading a document.
/// </summary>
public class UploadDocumentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Stream FileStream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for updating a document.
/// </summary>
public class UpdateDocumentRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsPrivate { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for creating a custom field.
/// </summary>
public class CreateCustomFieldRequest
{
    public string Name { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? DefaultValue { get; set; }
    public string[]? AllowedValues { get; set; }
}

/// <summary>
/// Request model for updating a custom field.
/// </summary>
public class UpdateCustomFieldRequest
{
    public string? Name { get; set; }
    public bool? IsRequired { get; set; }
    public string? DefaultValue { get; set; }
    public string[]? AllowedValues { get; set; }
}

#endregion

#region Pagination Models

/// <summary>
/// Options for paginated requests.
/// </summary>
public class PaginationOptions
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 100;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } = "asc";
    public Dictionary<string, object>? Filters { get; set; }
}

/// <summary>
/// Represents a paginated result.
/// </summary>
/// <typeparam name="T">The type of items in the result.</typeparam>
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

#endregion

#region Exception Models

/// <summary>
/// Base exception for Procore SDK Core client operations.
/// </summary>
public class ProcoreCoreException : Exception
{
    public string? ErrorCode { get; }
    public Dictionary<string, object>? Details { get; }

    public ProcoreCoreException(string message) : base(message) { }
    
    public ProcoreCoreException(string message, Exception innerException) : base(message, innerException) { }
    
    public ProcoreCoreException(string message, string? errorCode, Dictionary<string, object>? details = null) 
        : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }
}

/// <summary>
/// Exception thrown when a resource is not found.
/// </summary>
public class ResourceNotFoundException : ProcoreCoreException
{
    public ResourceNotFoundException(string resourceType, int id) 
        : base($"{resourceType} with ID {id} was not found.", "RESOURCE_NOT_FOUND") { }
}

/// <summary>
/// Exception thrown when a request is invalid.
/// </summary>
public class InvalidRequestException : ProcoreCoreException
{
    public InvalidRequestException(string message, Dictionary<string, object>? validationErrors = null) 
        : base(message, "INVALID_REQUEST", validationErrors) { }
}

/// <summary>
/// Exception thrown when access is forbidden.
/// </summary>
public class ForbiddenException : ProcoreCoreException
{
    public ForbiddenException(string message) 
        : base(message, "FORBIDDEN") { }
}

/// <summary>
/// Exception thrown when authentication fails.
/// </summary>
public class UnauthorizedException : ProcoreCoreException
{
    public UnauthorizedException(string message) 
        : base(message, "UNAUTHORIZED") { }
}

/// <summary>
/// Exception thrown when rate limits are exceeded.
/// </summary>
public class RateLimitExceededException : ProcoreCoreException
{
    public TimeSpan RetryAfter { get; }

    public RateLimitExceededException(TimeSpan retryAfter) 
        : base($"Rate limit exceeded. Retry after {retryAfter.TotalSeconds} seconds.", "RATE_LIMIT_EXCEEDED")
    {
        RetryAfter = retryAfter;
    }
}

#endregion