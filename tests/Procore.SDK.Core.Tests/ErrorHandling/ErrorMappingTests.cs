using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.Tests.ErrorHandling;

/// <summary>
/// Tests for HTTP status code mapping to domain-specific exceptions.
/// These tests define how the CoreClient should transform HTTP errors
/// into meaningful domain exceptions.
/// </summary>
public class ErrorMappingTests
{
    private readonly ErrorMapper _sut;

    public ErrorMappingTests()
    {
        _sut = new ErrorMapper();
    }

    #region HTTP Status Code Mapping Tests

    [Fact]
    public void MapHttpException_Should_Map_400_To_InvalidRequestException()
    {
        // Arrange
        var httpException = new HttpRequestException("Bad Request");
        httpException.Data["StatusCode"] = HttpStatusCode.BadRequest;
        httpException.Data["ResponseBody"] = """{"error": "Invalid input"}""";

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<InvalidRequestException>();
        result.Message.Should().Contain("Bad Request");
        result.As<InvalidRequestException>().ErrorCode.Should().Be("INVALID_REQUEST");
    }

    [Fact]
    public void MapHttpException_Should_Map_401_To_UnauthorizedException()
    {
        // Arrange
        var httpException = new HttpRequestException("Unauthorized");
        httpException.Data["StatusCode"] = HttpStatusCode.Unauthorized;

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<UnauthorizedException>();
        result.Message.Should().Contain("Unauthorized");
        result.As<UnauthorizedException>().ErrorCode.Should().Be("UNAUTHORIZED");
    }

    [Fact]
    public void MapHttpException_Should_Map_403_To_ForbiddenException()
    {
        // Arrange
        var httpException = new HttpRequestException("Forbidden");
        httpException.Data["StatusCode"] = HttpStatusCode.Forbidden;

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<ForbiddenException>();
        result.Message.Should().Contain("Forbidden");
        result.As<ForbiddenException>().ErrorCode.Should().Be("FORBIDDEN");
    }

    [Fact]
    public void MapHttpException_Should_Map_404_To_ResourceNotFoundException()
    {
        // Arrange
        var httpException = new HttpRequestException("Not Found");
        httpException.Data["StatusCode"] = HttpStatusCode.NotFound;
        httpException.Data["ResourceType"] = "Company";
        httpException.Data["ResourceId"] = "123";

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<ResourceNotFoundException>();
        result.Message.Should().Contain("Company");
        result.Message.Should().Contain("123");
    }

    [Fact]
    public void MapHttpException_Should_Map_422_To_InvalidRequestException_With_ValidationErrors()
    {
        // Arrange
        var validationErrors = new
        {
            errors = new Dictionary<string, string[]>
            {
                { "name", new[] { "Name is required", "Name must be unique" } },
                { "email", new[] { "Email format is invalid" } }
            }
        };

        var httpException = new HttpRequestException("Unprocessable Entity");
        httpException.Data["StatusCode"] = HttpStatusCode.UnprocessableEntity;
        httpException.Data["ResponseBody"] = JsonSerializer.Serialize(validationErrors);

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<InvalidRequestException>();
        var invalidRequestException = result.As<InvalidRequestException>();
        invalidRequestException.ErrorCode.Should().Be("INVALID_REQUEST");
        invalidRequestException.Details.Should().NotBeNull();
        invalidRequestException.Details!.Should().ContainKey("errors");
    }

    [Fact]
    public void MapHttpException_Should_Map_429_To_RateLimitExceededException()
    {
        // Arrange
        var httpException = new HttpRequestException("Too Many Requests");
        httpException.Data["StatusCode"] = HttpStatusCode.TooManyRequests;
        httpException.Data["RetryAfter"] = "120";

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<RateLimitExceededException>();
        var rateLimitException = result.As<RateLimitExceededException>();
        rateLimitException.ErrorCode.Should().Be("RATE_LIMIT_EXCEEDED");
        rateLimitException.RetryAfter.Should().Be(TimeSpan.FromSeconds(120));
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, "Internal Server Error")]
    [InlineData(HttpStatusCode.BadGateway, "Bad Gateway")]
    [InlineData(HttpStatusCode.ServiceUnavailable, "Service Unavailable")]
    [InlineData(HttpStatusCode.GatewayTimeout, "Gateway Timeout")]
    public void MapHttpException_Should_Map_5xx_To_ProcoreCoreException(HttpStatusCode statusCode, string message)
    {
        // Arrange
        var httpException = new HttpRequestException(message);
        httpException.Data["StatusCode"] = statusCode;

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<ProcoreCoreException>();
        result.Message.Should().Contain(message);
        result.As<ProcoreCoreException>().ErrorCode.Should().Be("SERVER_ERROR");
    }

    [Fact]
    public void MapHttpException_Should_Handle_Unknown_Status_Codes()
    {
        // Arrange
        var httpException = new HttpRequestException("Unknown Error");
        httpException.Data["StatusCode"] = (HttpStatusCode)418; // I'm a teapot

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<ProcoreCoreException>();
        result.Message.Should().Contain("Unknown Error");
        result.As<ProcoreCoreException>().ErrorCode.Should().Be("UNKNOWN_ERROR");
    }

    #endregion

    #region Response Body Parsing Tests

    [Fact]
    public void MapHttpException_Should_Parse_Procore_Error_Response_Format()
    {
        // Arrange
        var procoreErrorResponse = new
        {
            error = "Validation failed",
            error_description = "The request could not be processed due to validation errors",
            errors = new Dictionary<string, object>
            {
                { "name", new[] { "can't be blank" } },
                { "email", new[] { "is invalid", "has already been taken" } }
            }
        };

        var httpException = new HttpRequestException("Unprocessable Entity");
        httpException.Data["StatusCode"] = HttpStatusCode.UnprocessableEntity;
        httpException.Data["ResponseBody"] = JsonSerializer.Serialize(procoreErrorResponse);

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<InvalidRequestException>();
        var invalidRequestException = result.As<InvalidRequestException>();
        invalidRequestException.Message.Should().Contain("Validation failed");
        invalidRequestException.Details.Should().ContainKey("errors");
    }

    [Fact]
    public void MapHttpException_Should_Handle_Malformed_Response_Body()
    {
        // Arrange
        var httpException = new HttpRequestException("Bad Request");
        httpException.Data["StatusCode"] = HttpStatusCode.BadRequest;
        httpException.Data["ResponseBody"] = "This is not valid JSON {";

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<InvalidRequestException>();
        result.Message.Should().Contain("Bad Request");
        result.As<InvalidRequestException>().Details.Should().BeNull();
    }

    [Fact]
    public void MapHttpException_Should_Handle_Empty_Response_Body()
    {
        // Arrange
        var httpException = new HttpRequestException("Internal Server Error");
        httpException.Data["StatusCode"] = HttpStatusCode.InternalServerError;
        httpException.Data["ResponseBody"] = "";

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<ProcoreCoreException>();
        result.Message.Should().Contain("Internal Server Error");
    }

    #endregion

    #region Rate Limiting Tests

    [Theory]
    [InlineData("60", 60)]
    [InlineData("3600", 3600)]
    [InlineData("", 0)]
    [InlineData("invalid", 0)]
    public void MapHttpException_Should_Parse_RetryAfter_Header(string retryAfterValue, int expectedSeconds)
    {
        // Arrange
        var httpException = new HttpRequestException("Too Many Requests");
        httpException.Data["StatusCode"] = HttpStatusCode.TooManyRequests;
        if (!string.IsNullOrEmpty(retryAfterValue))
        {
            httpException.Data["RetryAfter"] = retryAfterValue;
        }

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<RateLimitExceededException>();
        var rateLimitException = result.As<RateLimitExceededException>();
        rateLimitException.RetryAfter.Should().Be(TimeSpan.FromSeconds(expectedSeconds));
    }

    [Fact]
    public void MapHttpException_Should_Parse_RetryAfter_DateTime_Format()
    {
        // Arrange
        var retryAfterDate = DateTimeOffset.UtcNow.AddMinutes(5);
        var httpException = new HttpRequestException("Too Many Requests");
        httpException.Data["StatusCode"] = HttpStatusCode.TooManyRequests;
        httpException.Data["RetryAfter"] = retryAfterDate.ToString("R"); // RFC1123 format

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<RateLimitExceededException>();
        var rateLimitException = result.As<RateLimitExceededException>();
        rateLimitException.RetryAfter.Should().BeCloseTo(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));
    }

    #endregion

    #region Context Preservation Tests

    [Fact]
    public void MapHttpException_Should_Preserve_Original_Exception_As_InnerException()
    {
        // Arrange
        var httpException = new HttpRequestException("Original message");
        httpException.Data["StatusCode"] = HttpStatusCode.BadRequest;

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.InnerException.Should().BeSameAs(httpException);
    }

    [Fact]
    public void MapHttpException_Should_Include_Request_Context_When_Available()
    {
        // Arrange
        var httpException = new HttpRequestException("Not Found");
        httpException.Data["StatusCode"] = HttpStatusCode.NotFound;
        httpException.Data["RequestPath"] = "/rest/v1.0/companies/123";
        httpException.Data["RequestMethod"] = "GET";

        // Act
        var result = _sut.MapHttpException(httpException);

        // Assert
        result.Should().BeOfType<ResourceNotFoundException>();
        result.Message.Should().Contain("123"); // Should extract ID from path
    }

    #endregion
}

/// <summary>
/// Test implementation of error mapper for TDD development.
/// This class defines the expected behavior for mapping HTTP exceptions
/// to domain-specific exceptions.
/// </summary>
public class ErrorMapper
{
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