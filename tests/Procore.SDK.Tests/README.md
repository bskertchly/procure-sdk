# Procore SDK Dependency Injection Tests

This test project provides comprehensive test coverage for the Procore SDK's dependency injection setup and configuration.

## Overview

The Procore SDK uses Microsoft's dependency injection container to manage services and their lifetimes. The `ServiceCollectionExtensions.AddProcoreSDK()` method provides a convenient way to register all required services with proper configuration.

## Test Structure

### Test Categories

1. **ServiceCollectionExtensionsTests** - Core registration logic and parameter validation
2. **HttpClientConfigurationTests** - HTTP client setup and connection pooling
3. **AuthenticationServiceRegistrationTests** - OAuth and authentication service registration
4. **KiotaRequestAdapterTests** - Kiota integration and request adapter setup
5. **CoreClientRegistrationTests** - Core client registration and dependency resolution
6. **OptionsValidationTests** - Configuration binding and validation
7. **HealthCheckTests** - Health check registration and functionality
8. **DependencyInjectionIntegrationTests** - End-to-end integration tests

## Expected DI Container Configuration

### Service Registrations

The `AddProcoreSDK()` method registers the following services:

#### Authentication Services (Singleton Lifetime)
- `ITokenStorage` → `InMemoryTokenStorage` (default, can be overridden)
- `ITokenManager` → `TokenManager`
- `OAuthFlowHelper` → `OAuthFlowHelper`
- `ProcoreAuthHandler` → `ProcoreAuthHandler`

#### HTTP Services (Singleton Lifetime)
- `IHttpClientFactory` → Configured with "Procore" named client
- `IRequestAdapter` → `HttpClientRequestAdapter` (Kiota)

#### Client Services (Scoped Lifetime)
- `ICoreClient` → `ProcoreCoreClient`

#### Configuration Services
- `IOptions<ProcoreAuthOptions>` → Bound from configuration
- `IOptions<HttpClientOptions>` → Bound from configuration
- `IOptionsMonitor<T>` and `IOptionsSnapshot<T>` → Available for both option types

#### Health Check Services
- `HealthCheckService` → Standard ASP.NET Core health checks
- `ProcoreApiHealthCheck` → Custom health check for Procore API connectivity

### Service Lifetimes

| Service Type | Lifetime | Rationale |
|--------------|----------|-----------|
| `ITokenStorage` | Singleton | Token storage should persist across requests |
| `ITokenManager` | Singleton | Token management state should be shared |
| `OAuthFlowHelper` | Singleton | Stateless utility service |
| `ProcoreAuthHandler` | Singleton | HTTP message handler should be reused |
| `IRequestAdapter` | Singleton | Kiota adapter can be safely reused |
| `ICoreClient` | Scoped | Client may maintain request-specific state |

## Usage Patterns

### Basic Usage

```csharp
// Simple configuration
services.AddProcoreSDK(
    clientId: "your-client-id",
    clientSecret: "your-client-secret", 
    redirectUri: "https://your-app.com/callback",
    scopes: new[] { "read", "write" }
);

// Configuration-based setup
services.AddProcoreSDK(configuration);

// Custom configuration
services.AddProcoreSDK(configuration, 
    auth => {
        auth.TokenRefreshMargin = TimeSpan.FromMinutes(10);
        auth.UsePkce = false;
    },
    http => {
        http.Timeout = TimeSpan.FromMinutes(5);
        http.MaxConnectionsPerServer = 25;
    }
);
```

### Configuration Structure

```json
{
  "ProcoreAuth": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RedirectUri": "https://your-app.com/callback",
    "Scopes": ["read", "write"],
    "TokenRefreshMargin": "00:05:00",
    "UsePkce": true,
    "AuthorizationEndpoint": "https://app.procore.com/oauth/authorize",
    "TokenEndpoint": "https://api.procore.com/oauth/token"
  },
  "ProcoreApi": {
    "BaseAddress": "https://api.procore.com",
    "Timeout": "00:01:00",
    "MaxConnectionsPerServer": 10,
    "PooledConnectionLifetime": "00:15:00",
    "PooledConnectionIdleTimeout": "00:02:00"
  }
}
```

### Service Resolution

```csharp
// Resolve services in controllers or other services
public class ProjectController : ControllerBase
{
    private readonly ICoreClient _coreClient;
    private readonly ITokenManager _tokenManager;

    public ProjectController(ICoreClient coreClient, ITokenManager tokenManager)
    {
        _coreClient = coreClient;
        _tokenManager = tokenManager;
    }

    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _coreClient.GetCompaniesAsync();
        return Ok(companies);
    }
}
```

### Health Checks

```csharp
// Add health check endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/procore", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("procore")
});
```

## Test Configuration

### Test Dependencies

The test project uses the following packages:
- **xUnit** - Testing framework
- **FluentAssertions** - Assertion library
- **NSubstitute** - Mocking framework
- **Microsoft.Extensions.*** - Dependency injection and configuration

### Mock Services

Tests use NSubstitute to create mock implementations of external dependencies:
- HTTP clients for testing network scenarios
- Custom implementations for testing service override behavior
- Logger instances for testing logging behavior

### Test Data

Test configuration uses in-memory configuration providers with predefined test data to ensure consistent test behavior across environments.

## Best Practices

### Service Registration
- Always use `TryAdd*` methods to allow service overrides
- Register services with appropriate lifetimes
- Validate configuration parameters early
- Provide meaningful error messages for invalid configuration

### Testing
- Test both positive and negative scenarios
- Verify service lifetimes are correct
- Test configuration binding and validation
- Include integration tests for complete workflows
- Mock external dependencies to ensure deterministic tests

### Configuration
- Use the Options pattern for configuration
- Support both programmatic and configuration-based setup
- Provide sensible defaults for optional settings
- Validate configuration at startup when possible

## Error Handling

The DI setup includes comprehensive error handling:

1. **Parameter Validation** - Null and empty parameter checks
2. **Configuration Validation** - Invalid configuration value handling
3. **Service Resolution** - Dependency resolution error handling
4. **HTTP Configuration** - Connection pool and timeout validation

## Security Considerations

- Token storage uses secure storage mechanisms when available
- HTTP clients are configured with appropriate security settings
- Authentication handlers properly manage token lifecycle
- Health checks don't expose sensitive information

## Performance Considerations

- HTTP client connection pooling is properly configured
- Services are registered with appropriate lifetimes to avoid memory leaks
- Request adapters are reused to minimize overhead
- Health checks are lightweight and don't impact performance

## Extensibility

The DI setup is designed to be extensible:

- Custom token storage implementations can be provided
- HTTP client configuration can be customized
- Additional health checks can be added
- Service implementations can be overridden

## Testing the DI Setup

To run the DI tests:

```bash
# Run all tests
dotnet test

# Run specific test category
dotnet test --filter "ClassName=ServiceCollectionExtensionsTests"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

The tests verify:
- ✅ All required services are registered
- ✅ Service lifetimes are correct
- ✅ Configuration binding works properly
- ✅ HTTP client setup is configured correctly
- ✅ Authentication services are properly wired
- ✅ Health checks function as expected
- ✅ Integration scenarios work end-to-end
- ✅ Error scenarios are handled gracefully
- ✅ Service resolution works in all contexts