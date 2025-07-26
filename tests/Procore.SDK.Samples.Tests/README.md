# Procore SDK Sample Applications Test Suite

This test suite provides comprehensive testing for the Procore SDK sample applications, focusing on OAuth PKCE flow implementation, error handling, and real-world usage scenarios.

## Overview

The test suite validates:
- **OAuth 2.0 PKCE Flow**: Complete authentication workflow from authorization URL generation to token exchange
- **Token Management**: Storage, retrieval, refresh, and lifecycle management
- **Error Handling**: Comprehensive error scenarios and recovery mechanisms
- **Security**: CSRF protection, token security, and vulnerability prevention
- **Performance**: Load testing, memory usage, and scalability validation
- **Integration**: End-to-end workflows and component interaction

## Test Categories

### 1. Unit Tests
- **ConsoleApp/Authentication/**: Console application OAuth flow tests
- **WebApp/Authentication/**: Web application callback handling and state validation
- **Shared/Configuration/**: Dependency injection and options validation

### 2. Integration Tests
- **Integration/EndToEndTests.cs**: Complete workflow validation
- **Integration/CrudOperationsTests.cs**: API interaction testing

### 3. Error Handling Tests
- **Shared/TestHelpers/ErrorScenarioTests.cs**: Comprehensive error scenarios
- OAuth error responses (invalid_grant, invalid_client, etc.)
- Network failures and timeouts
- Malformed responses and edge cases
- Security vulnerabilities and CSRF protection

### 4. Performance Tests
- **Performance/AuthenticationPerformanceTests.cs**: Benchmarks and load testing
- Concurrent operation handling
- Memory usage validation
- Long-running performance characteristics

## Key Test Scenarios

### OAuth PKCE Flow Testing

```csharp
[Fact]
public async Task ConsoleApp_InitialAuthentication_ShouldGenerateValidAuthUrl()
{
    // Tests authorization URL generation with proper PKCE parameters
    var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(expectedState);
    
    authUrl.Should().Contain("code_challenge_method=S256");
    authUrl.Should().Contain($"state={Uri.EscapeDataString(expectedState)}");
    codeVerifier.Length.Should().BeInRange(43, 128); // RFC 7636 compliance
}
```

### Token Management Testing

```csharp
[Fact]
public async Task TokenManager_ExpiredToken_ShouldAutomaticallyRefresh()
{
    // Tests automatic token refresh for expired tokens
    var expiredToken = new AccessToken(/*expired token*/);
    await tokenManager.StoreTokenAsync(expiredToken);
    
    // Setup refresh response mock
    fixture.MockRefreshTokenResponse(/*new token response*/);
    
    var currentToken = await tokenManager.GetAccessTokenAsync();
    currentToken.Should().NotBeNull();
    currentToken.Token.Should().Be("refreshed-access-token");
}
```

### Error Handling Testing

```csharp
[Theory]
[InlineData("invalid_grant", "Authorization code is invalid")]
[InlineData("invalid_client", "Client authentication failed")]
public async Task OAuth_StandardErrorResponses_ShouldHandleGracefully(
    string errorCode, string description)
{
    fixture.MockTokenErrorResponse(HttpStatusCode.BadRequest, new
    {
        error = errorCode,
        error_description = description
    });
    
    await Assert.ThrowsAsync<HttpRequestException>(
        () => oauthHelper.ExchangeCodeForTokenAsync("invalid-code", codeVerifier));
}
```

### Web Application Callback Testing

```csharp
[Fact]
public async Task WebApp_CallbackWithInvalidState_ShouldRejectAndRedirectToError()
{
    var client = fixture.CreateClient();
    var invalidState = "tampered-state-value";
    
    var response = await client.GetAsync($"/auth/callback?code=valid&state={invalidState}");
    
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    // Validates CSRF protection through state parameter verification
}
```

## Test Infrastructure

### TestAuthFixture
Provides complete test environment with:
- Dependency injection container setup
- Mock HTTP message handlers
- Token storage simulation
- API response mocking
- Authentication state management

### Mock Capabilities
- **Token Responses**: Success and error scenarios
- **API Responses**: CRUD operation simulation
- **Network Failures**: Timeout and connectivity issues
- **Session Management**: State storage and validation

## Running Tests

### All Tests
```bash
dotnet test
```

### Specific Categories
```bash
# Unit tests only
dotnet test --filter "Category=Unit"

# Integration tests
dotnet test --filter "Category=Integration"

# Performance tests (manual)
dotnet test --filter "Category=Performance"
```

### With Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Performance Benchmarks

The performance test suite includes:

- **Authorization URL Generation**: Sub-millisecond performance
- **Token Exchange**: Under 100ms average response time
- **Concurrent Operations**: 100+ concurrent requests handled
- **Memory Usage**: Validated for memory leak prevention
- **Load Testing**: Sustained performance under load

### Example Benchmark Results
```
| Method                    | Mean     | Error   | StdDev  | Allocated |
|-------------------------- |---------:|--------:|--------:|----------:|
| GenerateAuthorizationUrl  | 0.234 ms | 0.012ms | 0.008ms |     1.2KB |
| TokenExchange            | 45.67 ms | 2.34ms  | 1.89ms  |     3.4KB |
| TokenStorageRetrieval    | 0.067 ms | 0.003ms | 0.002ms |     0.8KB |
```

## Security Testing

### CSRF Protection
- State parameter validation
- Session management security
- Malicious redirect URI detection

### Token Security
- Sensitive data logging prevention
- Secure token storage validation
- Concurrent access safety

### Input Validation
- Malformed JSON handling
- Empty/null parameter rejection
- URL encoding validation

## Configuration

### Test Settings (appsettings.test.json)
```json
{
  "ProcoreAuth": {
    "ClientId": "test-client-id",
    "ClientSecret": "test-client-secret",
    "RedirectUri": "https://localhost:5001/auth/callback",
    "Scopes": ["read", "write", "admin"]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## Sample Usage Patterns

The tests demonstrate proper SDK usage patterns for:

### Console Applications
1. Generate authorization URL
2. Display to user for browser authentication
3. Capture authorization code from user
4. Exchange code for access token
5. Store token for future API calls
6. Handle token refresh automatically

### Web Applications
1. Redirect user to authorization URL
2. Handle OAuth callback with state validation
3. Exchange authorization code for token
4. Store token in user session/claims
5. Make authenticated API calls
6. Handle errors and token refresh

## Extending Tests

### Adding New Test Scenarios
1. Create test class in appropriate category folder
2. Inherit from `TestAuthFixture` for authentication tests
3. Use established patterns for mocking and assertions
4. Add performance benchmarks for critical paths

### Custom Mock Scenarios
```csharp
// Custom API response
fixture.MockApiResponse("/api/v1/custom", new { data = "test" });

// Custom error scenario
fixture.MockTokenErrorResponse(HttpStatusCode.ServiceUnavailable, new
{
    error = "temporarily_unavailable",
    error_description = "Service is temporarily unavailable"
});
```

## Continuous Integration

Tests are designed for CI/CD integration with:
- Fast execution (< 5 minutes for full suite)
- Deterministic results
- Clear failure reporting
- Coverage reporting
- Performance regression detection

## Troubleshooting

### Common Issues
1. **Mock Setup**: Ensure proper mock configuration before test execution
2. **Async Operations**: Use proper async/await patterns in tests
3. **Resource Cleanup**: Dispose resources properly to prevent test interference
4. **Timing Issues**: Use appropriate timeouts and retries for flaky tests

### Debug Tips
- Enable verbose logging for detailed execution traces
- Use test-specific service scopes to isolate test state
- Validate mock setup with captured request verification
- Check test execution order for state dependencies