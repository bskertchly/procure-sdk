using System;
using System.Collections.Generic;
using System.IO;

namespace Procore.SDK.Core.Models;

/// <summary>
/// Request model for creating a company.
/// </summary>
public class CreateCompanyRequest
{
    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the company.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the address information for the company.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the company.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for updating a company.
/// </summary>
public class UpdateCompanyRequest
{
    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the company.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the company is active.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the address information for the company.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the company.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for creating a user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the job title of the user.
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the user.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for updating a user.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the job title of the user.
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is active.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the user.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for uploading a document.
/// </summary>
public class UploadDocumentRequest
{
    /// <summary>
    /// Gets or sets the name of the document.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the document.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the file stream containing the document data.
    /// </summary>
    public Stream FileStream { get; set; } = Stream.Null;

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the MIME content type of the document.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the document is private.
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the document.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for updating a document.
/// </summary>
public class UpdateDocumentRequest
{
    /// <summary>
    /// Gets or sets the name of the document.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the document.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the document is private.
    /// </summary>
    public bool? IsPrivate { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the document.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Request model for creating a custom field.
/// </summary>
public class CreateCustomFieldRequest
{
    /// <summary>
    /// Gets or sets the name of the custom field.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the custom field.
    /// </summary>
    public string FieldType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type this custom field applies to.
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this custom field is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the default value for the custom field.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the allowed values for the custom field.
    /// </summary>
    public string[]? AllowedValues { get; set; }
}

/// <summary>
/// Request model for updating a custom field.
/// </summary>
public class UpdateCustomFieldRequest
{
    /// <summary>
    /// Gets or sets the name of the custom field.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this custom field is required.
    /// </summary>
    public bool? IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the default value for the custom field.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the allowed values for the custom field.
    /// </summary>
    public string[]? AllowedValues { get; set; }
}