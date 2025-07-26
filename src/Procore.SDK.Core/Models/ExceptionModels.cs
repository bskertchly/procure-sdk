using System;
using System.Collections.Generic;

namespace Procore.SDK.Core.Models;

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