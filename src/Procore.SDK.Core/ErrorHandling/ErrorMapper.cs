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
    /// <returns>A domain-specific exception.</returns>
    public ProcoreCoreException MapHttpException(HttpRequestException httpException)
    {
        var statusCode = GetStatusCode(httpException);
        var responseBody = GetResponseBody(httpException);

        return statusCode switch
        {
            HttpStatusCode.BadRequest => CreateInvalidRequestException(httpException, responseBody),
            HttpStatusCode.Unauthorized => new UnauthorizedException(httpException.Message),
            HttpStatusCode.Forbidden => new ForbiddenException(httpException.Message),
            HttpStatusCode.NotFound => CreateResourceNotFoundException(httpException),
            HttpStatusCode.UnprocessableEntity => CreateInvalidRequestException(httpException, responseBody),
            HttpStatusCode.TooManyRequests => CreateRateLimitExceededException(httpException),
            >= HttpStatusCode.InternalServerError => new ProcoreCoreException(httpException.Message, "SERVER_ERROR"),
            _ => new ProcoreCoreException(httpException.Message, "UNKNOWN_ERROR")
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

    private static InvalidRequestException CreateInvalidRequestException(HttpRequestException httpException, string? responseBody)
    {
        var details = ParseResponseBody(responseBody);
        return new InvalidRequestException(httpException.Message, details);
    }

    private static ResourceNotFoundException CreateResourceNotFoundException(HttpRequestException httpException)
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
        if (string.IsNullOrEmpty(path))
            return null;

        // Extract ID from path like "/rest/v1.0/companies/123"
        var segments = path.Split('/');
        return segments.LastOrDefault(s => int.TryParse(s, out _));
    }

    private static RateLimitExceededException CreateRateLimitExceededException(HttpRequestException httpException)
    {
        var retryAfter = TimeSpan.Zero;

        if (httpException.Data.Contains("RetryAfter"))
        {
            var retryAfterValue = httpException.Data["RetryAfter"]?.ToString();
            retryAfter = ParseRetryAfter(retryAfterValue);
        }

        return new RateLimitExceededException(retryAfter);
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
}