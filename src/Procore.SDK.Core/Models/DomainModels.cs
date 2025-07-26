using System;
using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.Core.Models;

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