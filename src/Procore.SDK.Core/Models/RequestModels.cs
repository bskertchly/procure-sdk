using System;
using System.Collections.Generic;
using System.IO;

namespace Procore.SDK.Core.Models;

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