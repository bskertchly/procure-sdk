using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.ErrorHandling;

/// <summary>
/// Maps HTTP exceptions to domain-specific exceptions.
/// This class transforms HTTP errors into meaningful domain exceptions.
/// </summary>
public class ErrorMapper
{
    /// <summary>
    /// Maps an HttpRequestException to a domain-specific ProcoreCoreException.
    /// </summary>
    /// <param name="httpException">The HTTP exception to map.</param>
    /// <param name="correlationId">Optional correlation ID for tracking.</param>
    /// <returns>A domain-specific exception.</returns>
    public ProcoreCoreException MapHttpException(HttpRequestException httpException, string? correlationId = null)
    {
        var statusCode = GetStatusCode(httpException);
        var responseBody = GetResponseBody(httpException);

        return statusCode switch
        {
            HttpStatusCode.BadRequest => CreateValidationException(httpException, responseBody, correlationId),
            HttpStatusCode.Unauthorized => new AuthenticationException(httpException.Message, correlationId),
            HttpStatusCode.Forbidden => new ForbiddenException(httpException.Message),
            HttpStatusCode.NotFound => CreateResourceNotFoundException(httpException, correlationId),
            HttpStatusCode.UnprocessableEntity => CreateValidationException(httpException, responseBody, correlationId),
            HttpStatusCode.TooManyRequests => CreateRateLimitExceededException(httpException, correlationId),
            HttpStatusCode.ServiceUnavailable => CreateServiceUnavailableException(httpException, correlationId),
            >= HttpStatusCode.InternalServerError => new ProcoreCoreException(httpException.Message, "SERVER_ERROR", null, correlationId),
            _ => new ProcoreCoreException(httpException.Message, "UNKNOWN_ERROR", null, correlationId)
        };
    }

    private static HttpStatusCode GetStatusCode(HttpRequestException httpException)
    {
        if (httpException.Data.Contains("StatusCode") && 
            httpException.Data["StatusCode"] is HttpStatusCode statusCode)
        {
            return statusCode;
        }
        return HttpStatusCode.InternalServerError;
    }

    private static string? GetResponseBody(HttpRequestException httpException)
    {
        return httpException.Data.Contains("ResponseBody") ? 
            httpException.Data["ResponseBody"]?.ToString() : 
            null;
    }

    private static ValidationException CreateValidationException(HttpRequestException httpException, string? responseBody, string? correlationId)
    {
        var validationErrors = ParseValidationErrors(responseBody);
        return new ValidationException(httpException.Message, validationErrors, correlationId);
    }

    private static ResourceNotFoundException CreateResourceNotFoundException(HttpRequestException httpException, string? correlationId)
    {
        // Try to extract resource type and ID from context
        var resourceType = httpException.Data.Contains("ResourceType") ? 
            httpException.Data["ResourceType"]?.ToString() ?? "Resource" : 
            "Resource";
        
        var resourceIdString = httpException.Data.Contains("ResourceId") ? 
            httpException.Data["ResourceId"]?.ToString() : 
            ExtractIdFromPath(httpException);

        if (int.TryParse(resourceIdString, out var resourceId))
        {
            return new ResourceNotFoundException(resourceType, resourceId);
        }

        return new ResourceNotFoundException("Resource", 0);
    }

    private static string? ExtractIdFromPath(HttpRequestException httpException)
    {
        if (!httpException.Data.Contains("RequestPath"))
            return null;

        var path = httpException.Data["RequestPath"]?.ToString();
        if (string.IsNullOrEmpty(path) || !IsValidPath(path))
            return null;

        // Extract ID from path like "/rest/v1.0/companies/123"
        var segments = path.Split('/');
        return segments.LastOrDefault(s => int.TryParse(s, out _));
    }

    /// <summary>
    /// Validates that a path is safe to process.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns>True if the path is safe to process.</returns>
    private static bool IsValidPath(string path)
    {
        // Basic security checks for path traversal and malicious content
        if (path.Contains("..") || 
            path.Contains("\\") ||
            path.Length > 2000 ||
            !path.StartsWith("/"))
        {
            return false;
        }

        return true;
    }

    private static RateLimitExceededException CreateRateLimitExceededException(HttpRequestException httpException, string? correlationId)
    {
        var retryAfter = TimeSpan.Zero;

        if (httpException.Data.Contains("RetryAfter"))
        {
            var retryAfterValue = httpException.Data["RetryAfter"]?.ToString();
            retryAfter = ParseRetryAfter(retryAfterValue);
        }

        return new RateLimitExceededException(retryAfter, correlationId);
    }

    private static ServiceUnavailableException CreateServiceUnavailableException(HttpRequestException httpException, string? correlationId)
    {
        var retryAfter = TimeSpan.Zero;

        if (httpException.Data.Contains("RetryAfter"))
        {
            var retryAfterValue = httpException.Data["RetryAfter"]?.ToString();
            retryAfter = ParseRetryAfter(retryAfterValue);
        }

        return new ServiceUnavailableException(httpException.Message, retryAfter, correlationId);
    }

    private static TimeSpan ParseRetryAfter(string? retryAfterValue)
    {
        if (string.IsNullOrEmpty(retryAfterValue))
            return TimeSpan.Zero;

        // Try parsing as seconds first
        if (int.TryParse(retryAfterValue, out var seconds))
        {
            return TimeSpan.FromSeconds(seconds);
        }

        // Try parsing as HTTP date
        if (DateTimeOffset.TryParse(retryAfterValue, out var dateTime))
        {
            var retryAfter = dateTime - DateTimeOffset.UtcNow;
            return retryAfter > TimeSpan.Zero ? retryAfter : TimeSpan.Zero;
        }

        return TimeSpan.Zero;
    }

    private static Dictionary<string, object>? ParseResponseBody(string? responseBody)
    {
        if (string.IsNullOrEmpty(responseBody))
            return null;

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static Dictionary<string, string[]>? ParseValidationErrors(string? responseBody)
    {
        if (string.IsNullOrEmpty(responseBody))
            return null;

        try
        {
            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            
            if (response?.TryGetValue("errors", out var errorsObj) == true)
            {
                if (errorsObj is JsonElement errorsElement && errorsElement.ValueKind == JsonValueKind.Object)
                {
                    var validationErrors = new Dictionary<string, string[]>();
                    
                    foreach (var property in errorsElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            var messages = new List<string>();
                            foreach (var item in property.Value.EnumerateArray())
                            {
                                if (item.ValueKind == JsonValueKind.String)
                                {
                                    var message = item.GetString();
                                    if (!string.IsNullOrEmpty(message) && IsValidErrorMessage(message))
                                    {
                                        messages.Add(message);
                                    }
                                }
                            }
                            validationErrors[property.Name] = messages.ToArray();
                        }
                        else if (property.Value.ValueKind == JsonValueKind.String)
                        {
                            var message = property.Value.GetString();
                            if (!string.IsNullOrEmpty(message) && IsValidErrorMessage(message))
                            {
                                validationErrors[property.Name] = new[] { message };
                            }
                        }
                    }
                    
                    return validationErrors.Count > 0 ? validationErrors : null;
                }
            }
            
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// Validates error messages to prevent information disclosure.
    /// </summary>
    /// <param name="message">The error message to validate.</param>
    /// <returns>True if the message is safe to include in exceptions.</returns>
    private static bool IsValidErrorMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return false;

        // Block messages that might contain sensitive information
        var lowerMessage = message.ToLowerInvariant();
        
        // Block potential file paths, SQL errors, internal server details
        if (lowerMessage.Contains("stack trace") ||
            lowerMessage.Contains("database") ||
            lowerMessage.Contains("sql") ||
            lowerMessage.Contains("connection string") ||
            lowerMessage.Contains("password") ||
            lowerMessage.Contains("token") ||
            lowerMessage.Contains("secret") ||
            lowerMessage.Contains("internal server error") ||
            lowerMessage.Contains("file not found") ||
            lowerMessage.Contains("path") ||
            message.Length > 500) // Prevent overly long messages
        {
            return false;
        }

        return true;
    }
}