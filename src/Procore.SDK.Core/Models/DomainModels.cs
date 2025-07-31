using System;
using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.Core.Models;

/// <summary>
/// Represents a Procore company.
/// </summary>
public class Company
{
    /// <summary>
    /// Gets or sets the unique identifier for the company.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the company.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the company is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the company was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the company was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the URL of the company's logo.
    /// </summary>
    public Uri? LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets the company's address information.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the company.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Represents a Procore user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

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
    /// Gets or sets a value indicating whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user last signed in.
    /// </summary>
    public DateTime? LastSignInAt { get; set; }

    /// <summary>
    /// Gets or sets the URL of the user's avatar image.
    /// </summary>
    public Uri? AvatarUrl { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the company associated with the user.
    /// </summary>
    public Company? Company { get; set; }

    /// <summary>
    /// Gets or sets the custom fields associated with the user.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// Represents a Procore document.
/// </summary>
public class Document
{
    /// <summary>
    /// Gets or sets the unique identifier for the document.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the document.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the document.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the file name of the document.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL where the document file can be accessed.
    /// </summary>
    public Uri? FileUrl { get; set; }

    /// <summary>
    /// Gets or sets the size of the document file in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the MIME content type of the document.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the document was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the document was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who created the document.
    /// </summary>
    public User? CreatedBy { get; set; }

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
/// Represents a Procore custom field definition.
/// </summary>
public class CustomField
{
    /// <summary>
    /// Gets or sets the unique identifier for the custom field.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the custom field.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the custom field (e.g., text, number, date).
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
    /// Gets or sets the allowed values for the custom field (for dropdown/select fields).
    /// </summary>
    public string[]? AllowedValues { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the custom field was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the custom field was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents an address.
/// </summary>
public class Address
{
    /// <summary>
    /// Gets or sets the first line of the street address.
    /// </summary>
    public string? Street1 { get; set; }

    /// <summary>
    /// Gets or sets the second line of the street address.
    /// </summary>
    public string? Street2 { get; set; }

    /// <summary>
    /// Gets or sets the city name.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the state or province name.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Gets or sets the postal or zip code.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the country name.
    /// </summary>
    public string? Country { get; set; }
}