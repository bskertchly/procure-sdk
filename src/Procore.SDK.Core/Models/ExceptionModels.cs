using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Procore.SDK.Core.Models;

/// <summary>
/// Base exception for Procore SDK Core client operations.
/// Provides structured error information with correlation tracking and context preservation.
/// </summary>
public class ProcoreCoreException : Exception
{
    /// <summary>
    /// The error code that identifies the type of error.
    /// </summary>
    [JsonPropertyName("error_code")]
    public string? ErrorCode { get; }
    
    /// <summary>
    /// Additional details about the error, excluding sensitive information.
    /// </summary>
    [JsonPropertyName("details")]
    public Dictionary<string, object>? Details { get; }
    
    /// <summary>
    /// Correlation ID for tracking the request across systems.
    /// </summary>
    [JsonPropertyName("correlation_id")]
    public string? CorrelationId { get; }
    
    /// <summary>
    /// Timestamp when the error occurred.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// Initializes a new instance of the ProcoreCoreException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProcoreCoreException(string message) : base(message) 
    {
        Timestamp = DateTimeOffset.UtcNow;
    }
    
    /// <summary>
    /// Initializes a new instance of the ProcoreCoreException class with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ProcoreCoreException(string message, Exception innerException) : base(message, innerException) 
    {
        Timestamp = DateTimeOffset.UtcNow;
    }
    
    /// <summary>
    /// Initializes a new instance of the ProcoreCoreException class with detailed error information.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="errorCode">The specific error code for categorization.</param>
    /// <param name="details">Additional details about the error.</param>
    /// <param name="correlationId">The correlation ID for tracking.</param>
    public ProcoreCoreException(string message, string? errorCode, Dictionary<string, object>? details = null, string? correlationId = null) 
        : base(message)
    {
        ErrorCode = errorCode;
        Details = SanitizeDetails(details);
        CorrelationId = correlationId;
        Timestamp = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Removes sensitive information from error details.
    /// </summary>
    private static Dictionary<string, object>? SanitizeDetails(Dictionary<string, object>? details)
    {
        if (details == null) return null;

        var sensitiveKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "password", "token", "secret", "key", "authorization", "auth", "access_token", "refresh_token", "api_key"
        };

        var sanitized = new Dictionary<string, object>();
        foreach (var kvp in details)
        {
            if (!sensitiveKeys.Contains(kvp.Key))
            {
                sanitized[kvp.Key] = kvp.Value;
            }
        }

        return sanitized.Count > 0 ? sanitized : null;
    }
}

/// <summary>
/// Exception thrown when a resource is not found.
/// </summary>
public class ResourceNotFoundException : ProcoreCoreException
{
    /// <summary>
    /// Initializes a new instance of the ResourceNotFoundException class.
    /// </summary>
    /// <param name="resourceType">The type of resource that was not found.</param>
    /// <param name="id">The ID of the resource that was not found.</param>
    public ResourceNotFoundException(string resourceType, int id) 
        : base($"{resourceType} with ID {id} was not found.", "RESOURCE_NOT_FOUND") { }
}

/// <summary>
/// Exception thrown when a request is invalid.
/// </summary>
public class InvalidRequestException : ProcoreCoreException
{
    /// <summary>
    /// Initializes a new instance of the InvalidRequestException class.
    /// </summary>
    /// <param name="message">The error message describing the invalid request.</param>
    /// <param name="validationErrors">The validation errors associated with the request.</param>
    public InvalidRequestException(string message, Dictionary<string, object>? validationErrors = null) 
        : base(message, "INVALID_REQUEST", validationErrors) { }
}

/// <summary>
/// Exception thrown when access is forbidden.
/// </summary>
public class ForbiddenException : ProcoreCoreException
{
    /// <summary>
    /// Initializes a new instance of the ForbiddenException class.
    /// </summary>
    /// <param name="message">The error message describing why access is forbidden.</param>
    public ForbiddenException(string message) 
        : base(message, "FORBIDDEN") { }
}

/// <summary>
/// Exception thrown when authentication fails.
/// </summary>
public class UnauthorizedException : ProcoreCoreException
{
    /// <summary>
    /// Initializes a new instance of the UnauthorizedException class.
    /// </summary>
    /// <param name="message">The error message describing the authentication failure.</param>
    public UnauthorizedException(string message) 
        : base(message, "UNAUTHORIZED") { }
}

/// <summary>
/// Exception thrown when rate limits are exceeded.
/// </summary>
public class RateLimitExceededException : ProcoreCoreException
{
    /// <summary>
    /// Time to wait before retrying the request.
    /// </summary>
    [JsonPropertyName("retry_after")]
    public TimeSpan RetryAfter { get; }

    /// <summary>
    /// Initializes a new instance of the RateLimitExceededException class.
    /// </summary>
    /// <param name="retryAfter">The time to wait before retrying the request.</param>
    /// <param name="correlationId">The correlation ID for tracking.</param>
    public RateLimitExceededException(TimeSpan retryAfter, string? correlationId = null) 
        : base($"Rate limit exceeded. Retry after {retryAfter.TotalSeconds} seconds.", "RATE_LIMIT_EXCEEDED", null, correlationId)
    {
        RetryAfter = retryAfter;
    }
}

/// <summary>
/// Exception thrown when the Procore API is temporarily unavailable.
/// </summary>
public class ServiceUnavailableException : ProcoreCoreException
{
    /// <summary>
    /// Time to wait before retrying the request.
    /// </summary>
    [JsonPropertyName("retry_after")]
    public TimeSpan? RetryAfter { get; }

    /// <summary>
    /// Initializes a new instance of the ServiceUnavailableException class.
    /// </summary>
    /// <param name="message">The error message describing the service unavailability.</param>
    /// <param name="retryAfter">The time to wait before retrying the request.</param>
    /// <param name="correlationId">The correlation ID for tracking.</param>
    public ServiceUnavailableException(string message, TimeSpan? retryAfter = null, string? correlationId = null) 
        : base(message, "SERVICE_UNAVAILABLE", null, correlationId)
    {
        RetryAfter = retryAfter;
    }
}

/// <summary>
/// Exception thrown when authentication credentials are invalid or expired.
/// </summary>
public class AuthenticationException : ProcoreCoreException
{
    /// <summary>
    /// Initializes a new instance of the AuthenticationException class.
    /// </summary>
    /// <param name="message">The error message describing the authentication failure.</param>
    /// <param name="correlationId">The correlation ID for tracking.</param>
    public AuthenticationException(string message, string? correlationId = null) 
        : base(message, "AUTHENTICATION_FAILED", null, correlationId) { }
}

/// <summary>
/// Exception thrown when request validation fails.
/// </summary>
public class ValidationException : ProcoreCoreException
{
    /// <summary>
    /// Field-specific validation errors.
    /// </summary>
    [JsonPropertyName("validation_errors")]
    public Dictionary<string, string[]>? ValidationErrors { get; }

    /// <summary>
    /// Initializes a new instance of the ValidationException class.
    /// </summary>
    /// <param name="message">The error message describing the validation failure.</param>
    /// <param name="validationErrors">Field-specific validation errors.</param>
    /// <param name="correlationId">The correlation ID for tracking.</param>
    public ValidationException(string message, Dictionary<string, string[]>? validationErrors = null, string? correlationId = null) 
        : base(message, "VALIDATION_ERROR", ConvertValidationErrors(validationErrors), correlationId)
    {
        ValidationErrors = validationErrors;
    }

    private static Dictionary<string, object>? ConvertValidationErrors(Dictionary<string, string[]>? validationErrors)
    {
        if (validationErrors == null) return null;
        
        var result = new Dictionary<string, object>();
        foreach (var kvp in validationErrors)
        {
            result[kvp.Key] = kvp.Value;
        }
        return result;
    }
}

/// <summary>
/// Exception thrown when a network-level error occurs.
/// </summary>
public class NetworkException : ProcoreCoreException
{
    /// <summary>
    /// Initializes a new instance of the NetworkException class.
    /// </summary>
    /// <param name="message">The error message describing the network failure.</param>
    /// <param name="innerException">The underlying exception that caused this network error.</param>
    /// <param name="correlationId">The correlation ID for tracking.</param>
    public NetworkException(string message, Exception innerException, string? correlationId = null) 
        : base(message, "NETWORK_ERROR", new Dictionary<string, object>
        {
            ["inner_exception_type"] = innerException.GetType().Name
        }, correlationId)
    {
    }
}