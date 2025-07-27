# Procore SDK Resilience Tests

This test project contains comprehensive tests for enhanced error handling and resilience patterns in the Procore SDK. The tests validate Polly integration, custom exception handling, resilience patterns, structured logging, and performance impact.

## Test Categories

### 1. Polly Integration Tests (`/Polly/`)

Tests for Polly resilience library integration:

- **RetryPolicyTests**: Exponential backoff, jittered retry, transient failure handling
- **CircuitBreakerTests**: State transitions (Closed → Open → Half-Open), failure thresholds
- **TimeoutPolicyTests**: Request cancellation, timeout precision, integration scenarios
- **PolicyWrapTests**: Combined policy behaviors, complex failure scenarios

### 2. Structured Logging Tests (`/Logging/`)

Tests for proper error logging and monitoring:

- **StructuredLoggingTests**: Correlation IDs, performance metrics, sensitive data handling

### 3. Performance Tests (`/Performance/`)

Tests to ensure resilience patterns don't degrade performance:

- **ResilienceOverheadTests**: Overhead measurement, memory usage, concurrent performance

### 4. Helper Classes (`/Helpers/`)

Reusable test infrastructure:

- **TestHttpMessageHandler**: Configurable HTTP response simulation
- **TestLoggerProvider**: Log capture and assertion utilities
- **PolicyFactory**: Predefined policy configurations for testing

## Test Execution

### Running All Tests

```bash
# Run all resilience tests
dotnet test Procore.SDK.Resilience.Tests

# Run with coverage
dotnet test Procore.SDK.Resilience.Tests --collect:"XPlat Code Coverage"
```

### Running by Category

```bash
# Unit tests (fast-running)
dotnet test --filter Category=Unit

# Integration tests (realistic scenarios)
dotnet test --filter Category=Integration

# Performance tests (overhead measurement)
dotnet test --filter Category=Performance
```

### Running Specific Test Classes

```bash
# Retry policy tests
dotnet test --filter ClassName~RetryPolicyTests

# Circuit breaker tests
dotnet test --filter ClassName~CircuitBreakerTests

# Timeout policy tests
dotnet test --filter ClassName~TimeoutPolicyTests

# Combined policy tests
dotnet test --filter ClassName~PolicyWrapTests

# Logging tests
dotnet test --filter ClassName~StructuredLoggingTests

# Performance tests
dotnet test --filter ClassName~ResilienceOverheadTests
```

## Test Configuration

### Environment Variables

The tests can be configured using environment variables:

```bash
# Set log level for test output
export ASPNETCORE_ENVIRONMENT=Test

# Enable detailed logging
export Logging__LogLevel__Default=Debug
```

### Test Settings

Tests use realistic configurations that mirror production scenarios:

- **Retry Policies**: 3 retries with exponential backoff (100ms base delay)
- **Circuit Breakers**: 3-5 failure threshold, 30s break duration
- **Timeouts**: 5-30 second timeouts for various scenarios
- **Performance Targets**: <15% overhead for individual policies, <25% for combined

## Key Test Scenarios

### Realistic Failure Patterns

Tests simulate real-world Procore API failure scenarios:

1. **Network Issues**: Connection timeouts, DNS failures, packet loss
2. **Service Degradation**: Increasing response times, intermittent failures
3. **Rate Limiting**: HTTP 429 responses with Retry-After headers
4. **Authentication**: Token expiry and refresh scenarios
5. **Circuit Breaker**: Service outages and recovery patterns

### Performance Validation

Performance tests ensure resilience patterns maintain acceptable overhead:

- **Successful Requests**: <15% overhead for retry policies, <10% for circuit breakers
- **Memory Usage**: <2KB increase per request, no memory leaks
- **Concurrent Load**: Proper scaling under concurrent requests
- **High Frequency**: >1000 operations/second capability

### Logging Verification

Structured logging tests validate:

- **Correlation IDs**: Request tracking across retry attempts
- **Performance Metrics**: Request duration, retry counts, circuit breaker states
- **Security**: Sensitive data filtering in logs
- **Monitoring**: Structured data for observability systems

## Test Data

### Mock Responses

Tests use realistic Procore API response patterns:

```json
// Success Response
{
  "companies": [
    {"id": 123, "name": "Test Company"}
  ]
}

// Error Response
{
  "error": "Rate limit exceeded",
  "error_description": "Too many requests",
  "retry_after": 120
}

// Validation Error
{
  "errors": {
    "name": ["can't be blank"],
    "email": ["is invalid"]
  }
}
```

### Failure Injection

Tests use controlled failure injection rather than random failures:

```csharp
// Deterministic failure pattern
var handler = TestHttpMessageHandler.CreateFailThenSucceedHandler(
    failureCount: 2, 
    failureStatusCode: HttpStatusCode.ServiceUnavailable);

// Status code sequence
var handler = TestHttpMessageHandler.CreateStatusCodeSequenceHandler(
    HttpStatusCode.TooManyRequests,
    HttpStatusCode.TooManyRequests, 
    HttpStatusCode.OK);
```

## Integration with CI/CD

### Test Categorization

Tests are categorized for different CI/CD pipeline stages:

- **Unit**: Fast tests for every commit (< 1 second each)
- **Integration**: Comprehensive scenarios for PR validation (< 30 seconds each)
- **Performance**: Overhead validation for release candidates (< 5 minutes each)

### Coverage Requirements

- **Unit Tests**: 95%+ line coverage
- **Integration Tests**: Critical path coverage
- **Performance Tests**: Baseline establishment and regression detection

### Example CI Configuration

```yaml
# Unit tests on every commit
- name: Unit Tests
  run: dotnet test --filter Category=Unit --logger trx
  
# Integration tests on PR
- name: Integration Tests  
  run: dotnet test --filter Category=Integration --logger trx
  if: github.event_name == 'pull_request'
  
# Performance tests on release
- name: Performance Tests
  run: dotnet test --filter Category=Performance --logger trx
  if: github.ref == 'refs/heads/main'
```

## Troubleshooting

### Common Issues

1. **Test Timeouts**: Increase timeout values in test configuration
2. **Memory Pressure**: Run tests with `--collect:"XPlat Code Coverage"` separately
3. **Flaky Performance Tests**: Performance tests include warmup periods and statistical validation

### Debug Output

Enable detailed logging for troubleshooting:

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Test Isolation

Each test class uses isolated instances:

- Fresh `TestLoggerProvider` per test class
- Separate `HttpClient` instances per test
- Controlled mock handlers with deterministic behavior

## Dependencies

- **Polly**: 8.4.1 - Resilience patterns
- **Polly.Extensions.Http**: 3.0.0 - HTTP integration
- **xUnit**: 2.9.0 - Test framework
- **FluentAssertions**: 6.12.0 - Assertion library
- **NSubstitute**: 5.1.0 - Mocking framework

## Related Documentation

- [Enhanced Error Handling Test Plans](../ENHANCED_ERROR_HANDLING_TEST_PLANS.md)
- [Comprehensive Test Strategy](../COMPREHENSIVE_TEST_STRATEGY.md)
- [Procore SDK Core Tests](../Procore.SDK.Core.Tests/README.md)