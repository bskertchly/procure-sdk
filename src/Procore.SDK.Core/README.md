# Procore SDK Core Client

[![NuGet Version](https://img.shields.io/nuget/v/Procore.SDK.Core.svg)](https://www.nuget.org/packages/Procore.SDK.Core/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Procore.SDK.Core.svg)](https://www.nuget.org/packages/Procore.SDK.Core/)

Core client for the Procore SDK, providing access to companies, users, documents, and configuration management through Procore's foundational API resources. This module also provides comprehensive error handling and resilience patterns including structured logging, retry policies, circuit breakers, and correlation tracking.

## Features

### 🛡️ Resilience Patterns
- **Retry Policies**: Exponential backoff with jitter to prevent thundering herd problems
- **Circuit Breakers**: Automatic failure detection and recovery with configurable thresholds
- **Timeout Policies**: Request timeout management with proper cancellation token support
- **Combined Policies**: Intelligent policy composition for optimal resilience

### 📊 Structured Logging
- **Correlation Tracking**: Unique correlation IDs for request tracing across systems
- **Performance Metrics**: Automatic logging of request duration and success rates
- **Structured Output**: JSON-formatted logs with searchable properties
- **Security-Safe**: Automatic sanitization of sensitive information

### 🚨 Enhanced Exception Handling
- **Custom Exception Hierarchy**: Domain-specific exceptions with rich context
- **Error Mapping**: Automatic HTTP status code to domain exception mapping
- **Context Preservation**: Correlation IDs and request context maintained across retries
- **Serializable Exceptions**: JSON-serializable exceptions for distributed systems

### ⚙️ Configuration-Driven
- **Environment-Specific**: Different settings for development, staging, and production
- **Runtime Adjustable**: Hot-reload configuration support
- **Validation**: Built-in configuration validation with clear error messages

### 🏢 API Coverage
- **Companies API (V1.0)**: Full CRUD operations with search and filtering
- **Users API (V1.1)**: Complete user management with role assignments
- **Documents API (V1.0)**: File operations with metadata and permissions
- **Custom Fields API (V2.0)**: Configuration management with validation

## Quick Start

### 1. Add Dependencies

```xml
<PackageReference Include="Procore.SDK.Core" Version="1.0.0" />
```

### 2. Configure Services

```csharp
using Procore.SDK.Core.Extensions;

// In Program.cs or Startup.cs
builder.Services.AddProcoreResilience(builder.Configuration);
builder.Services.ValidateResilienceOptions();

// Add resilience to HTTP clients
builder.Services.AddHttpClient<IProjectManagementClient, ProcoreProjectManagementClient>()
    .AddProcoreResilience();
```

### 3. Configuration (appsettings.json)

```json
{
  "Procore": {
    "Resilience": {
      "Retry": {
        "MaxAttempts": 3,
        "BaseDelayMs": 1000,
        "MaxDelayMs": 30000,
        "BackoffMultiplier": 2.0,
        "MaxJitterMs": 1000,
        "UseExponentialBackoff": true,
        "UseJitter": true
      },
      "CircuitBreaker": {
        "FailureThreshold": 5,
        "DurationOfBreakInSeconds": 30,
        "MinimumThroughput": 10,
        "Enabled": true
      },
      "Timeout": {
        "DefaultTimeoutInSeconds": 30,
        "LongRunningTimeoutInSeconds": 300,
        "Enabled": true
      },
      "Logging": {
        "LogRetryAttempts": true,
        "LogCircuitBreakerEvents": true,
        "LogTimeouts": true,
        "LogPerformanceMetrics": true,
        "IncludeRequestDetails": false
      }
    }
  }
}
```

### 4. Using in Your Code

```csharp
public class MyService
{
    private readonly IProjectManagementClient _projectClient;
    private readonly ILogger<MyService> _logger;

    public MyService(IProjectManagementClient projectClient, ILogger<MyService> logger)
    {
        _projectClient = projectClient;
        _logger = logger;
    }

    public async Task<Project> GetProjectWithResilienceAsync(int companyId, int projectId)
    {
        try
        {
            // Resilience policies are automatically applied
            var project = await _projectClient.GetProjectAsync(companyId, projectId);
            return project;
        }
        catch (RateLimitExceededException ex)
        {
            _logger.LogWarning("Rate limit exceeded. Retry after {RetryAfter} seconds", 
                ex.RetryAfter.TotalSeconds);
            
            // Wait and retry or implement exponential backoff
            await Task.Delay(ex.RetryAfter);
            throw;
        }
        catch (ServiceUnavailableException ex)
        {
            _logger.LogError("Service unavailable: {Message}", ex.Message);
            
            // Implement fallback logic or queue for later processing
            throw;
        }
        catch (AuthenticationException ex)
        {
            _logger.LogError("Authentication failed: {Message}. CorrelationId: {CorrelationId}", 
                ex.Message, ex.CorrelationId);
            
            // Refresh tokens or redirect to login
            throw;
        }
    }
}
```

## Exception Types

### Base Exception
```csharp
public class ProcoreCoreException : Exception
{
    public string? ErrorCode { get; }
    public Dictionary<string, object>? Details { get; }
    public string? CorrelationId { get; }
    public DateTimeOffset Timestamp { get; }
}
```

### Specific Exceptions
- **`AuthenticationException`**: 401/403 responses, token expiry
- **`ValidationException`**: 400 responses, field validation errors
- **`ResourceNotFoundException`**: 404 responses, missing resources
- **`RateLimitExceededException`**: 429 responses, includes retry-after header
- **`ServiceUnavailableException`**: 503 responses, temporary outages
- **`NetworkException`**: Network-level failures, timeouts

## Resilience Context

Each operation includes a resilience context for tracking:

```csharp
public class ResilienceContext
{
    public string CorrelationId { get; }
    public int AttemptNumber { get; set; }
    public string Operation { get; }
    public DateTimeOffset StartTime { get; }
    public Dictionary<string, object> Properties { get; }
    public Exception? LastException { get; set; }
    public TimeSpan ElapsedTime { get; }
}
```

## Logging Examples

### Structured Logs with Correlation
```json
{
  "@t": "2024-01-15T10:30:00.000Z",
  "@l": "Information",
  "@m": "Operation GET /rest/v1.0/companies/123/projects completed in 250ms",
  "CorrelationId": "550e8400-e29b-41d4-a716-446655440000",
  "Operation": "GetProjects-Company-123",
  "DurationMs": 250,
  "StatusCode": 200,
  "AttemptNumber": 1,
  "Application": "Procore.SDK.Core"
}
```

### Retry Attempt Logging
```json
{
  "@t": "2024-01-15T10:30:05.000Z",
  "@l": "Warning",
  "@m": "Retry attempt 2 for operation GetProjects-Company-123 after 2000ms delay",
  "CorrelationId": "550e8400-e29b-41d4-a716-446655440000",
  "Operation": "GetProjects-Company-123",
  "AttemptNumber": 2,
  "DelayMs": 2000,
  "Exception": "HttpRequestException: Service temporarily unavailable"
}
```

### Circuit Breaker Events
```json
{
  "@t": "2024-01-15T10:35:00.000Z",
  "@l": "Error",
  "@m": "Circuit breaker opened for operation GetProjects-Company-123 due to HttpRequestException: Service unavailable. Duration: 30s",
  "CorrelationId": "550e8400-e29b-41d4-a716-446655440000",
  "Operation": "GetProjects-Company-123",
  "CircuitState": "Open",
  "DurationSeconds": 30
}
```

## Performance Monitoring

The resilience framework automatically tracks:
- **Request Duration**: Time from start to completion
- **Success Rate**: Percentage of successful requests
- **Retry Frequency**: Number of retries per operation
- **Circuit Breaker State**: Open/closed/half-open states
- **Error Distribution**: Types and frequency of errors

## Environment-Specific Configuration

### Development
```json
{
  "Procore": {
    "Resilience": {
      "Retry": { "MaxAttempts": 1 },
      "CircuitBreaker": { "Enabled": false },
      "Logging": { "IncludeRequestDetails": true }
    }
  }
}
```

### Production
```json
{
  "Procore": {
    "Resilience": {
      "Retry": { "MaxAttempts": 5, "MaxDelayMs": 60000 },
      "CircuitBreaker": { "FailureThreshold": 10 },
      "Logging": { "IncludeRequestDetails": false }
    }
  }
}
```

## Best Practices

### 1. Correlation ID Management
Always include correlation IDs in logs and pass them through API calls:

```csharp
// Get correlation ID from request headers
var correlationId = HttpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault() 
    ?? Guid.NewGuid().ToString();

// Use in structured logging
using (LogContext.PushProperty("CorrelationId", correlationId))
{
    // Your operations
}
```

### 2. Error Handling Strategy
Implement appropriate error handling for each exception type:

```csharp
try
{
    return await operation();
}
catch (RateLimitExceededException ex)
{
    // Implement exponential backoff or queue for later
}
catch (AuthenticationException ex)
{
    // Refresh tokens or redirect to authentication
}
catch (ValidationException ex)
{
    // Return validation errors to user
}
catch (ServiceUnavailableException ex)
{
    // Implement fallback or queue for retry
}
```

### 3. Circuit Breaker Configuration
Configure circuit breakers based on your service's characteristics:

- **High-traffic services**: Lower failure threshold (3-5 failures)
- **Background services**: Higher failure threshold (10-20 failures)
- **Critical paths**: Shorter break duration (10-30 seconds)
- **Non-critical paths**: Longer break duration (60-300 seconds)

### 4. Monitoring and Alerting
Set up monitoring for:
- Circuit breaker state changes
- High retry rates (>20% of requests)
- Long request durations (>95th percentile)
- Error rate increases (>5% of requests)

## Testing

The resilience framework includes comprehensive test helpers:

```csharp
[Test]
public async Task Should_Retry_On_Transient_Failures()
{
    var mockHandler = new TestHttpMessageHandler(request =>
    {
        if (callCount++ < 2)
            throw new HttpRequestException("Temporary failure");
        return new HttpResponseMessage(HttpStatusCode.OK);
    });

    var policy = _policyFactory.CreateHttpPolicy(new ResilienceContext("test"));
    var response = await policy.ExecuteAsync(() => client.GetAsync("/test"));
    
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

## Migration Guide

### From Basic HTTP Clients

1. **Add resilience packages**
2. **Configure services** with `AddProcoreResilience()`
3. **Update HTTP client registration** with `.AddProcoreResilience()`
4. **Update exception handling** to use specific exception types
5. **Add structured logging** with correlation IDs

### Performance Impact

The resilience framework adds minimal overhead:
- **Retry policies**: <10% overhead for successful requests
- **Circuit breakers**: <5% overhead when closed
- **Combined policies**: <20% overhead for all policies
- **Logging**: <5% overhead with structured JSON output

## Troubleshooting

### Common Issues

1. **High retry rates**: Check service health and thresholds
2. **Circuit breaker always open**: Verify failure threshold configuration
3. **Missing correlation IDs**: Ensure proper middleware setup
4. **Performance degradation**: Review policy configuration

### Debug Logging

Enable debug logging to see detailed resilience operation:

```json
{
  "Logging": {
    "LogLevel": {
      "Procore.SDK.Core": "Debug"
    }
  }
}
```

This will show detailed information about policy execution, retry decisions, and circuit breaker state changes.