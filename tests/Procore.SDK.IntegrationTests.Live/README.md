# Procore SDK Live Integration Tests

This test suite provides comprehensive integration testing against real Procore sandbox environments, validating production readiness of the SDK with actual API interactions.

## Overview

The Live Integration Tests validate:

- **Authentication Flow**: OAuth 2.0 PKCE with real Procore OAuth server
- **Client Integration**: All 5 SDK clients against live APIs
- **Resilience Patterns**: Retry policies, circuit breakers, timeouts with real failures
- **Performance Testing**: Response times, concurrency, and rate limit compliance
- **End-to-End Workflows**: Complete construction management scenarios
- **Data Consistency**: Cross-client operations and data integrity

## Test Architecture

```
├── Infrastructure/
│   ├── LiveSandboxFixture.cs        # Real Procore sandbox management
│   ├── IntegrationTestBase.cs       # Base test class with common functionality
│   └── TestDataBuilder.cs           # Realistic test data generation
├── Authentication/
│   └── OAuthFlowIntegrationTests.cs # OAuth 2.0 flow validation
├── Clients/
│   ├── CoreClientIntegrationTests.cs
│   ├── QualitySafetyClientIntegrationTests.cs
│   └── [Other client tests...]
├── Resilience/
│   └── ResiliencePatternsIntegrationTests.cs
├── Workflows/
│   └── EndToEndWorkflowTests.cs     # Cross-client workflows
└── Performance/
    └── LoadTestingIntegrationTests.cs
```

## Prerequisites

### 1. Procore Sandbox Account

You need access to a Procore sandbox environment with:

- Valid sandbox credentials (Client ID, Client Secret)
- OAuth 2.0 application registration
- Test company and project data access
- API permissions for all client domains

### 2. Configuration Setup

Configure credentials using any of these methods:

#### Option A: User Secrets (Recommended for Development)

```bash
cd tests/Procore.SDK.IntegrationTests.Live
dotnet user-secrets set "Procore:ClientId" "your_sandbox_client_id"
dotnet user-secrets set "Procore:ClientSecret" "your_sandbox_client_secret"
dotnet user-secrets set "Procore:TestCompanyId" "12345"
dotnet user-secrets set "Procore:TestUserEmail" "test@yourcompany.com"
```

#### Option B: Environment Variables

```bash
export PROCORE_INTEGRATION_Procore__ClientId="your_sandbox_client_id"
export PROCORE_INTEGRATION_Procore__ClientSecret="your_sandbox_client_secret"
export PROCORE_INTEGRATION_Procore__TestCompanyId="12345"
export PROCORE_INTEGRATION_Procore__TestUserEmail="test@yourcompany.com"
```

#### Option C: Configuration File

Update `appsettings.integrationtest.json`:

```json
{
  "Procore": {
    "ClientId": "your_sandbox_client_id",
    "ClientSecret": "your_sandbox_client_secret",
    "BaseUrl": "https://sandbox.procore.com",
    "TestCompanyId": "12345",
    "TestUserEmail": "test@yourcompany.com"
  }
}
```

⚠️ **Never commit real credentials to source control**

### 3. OAuth Setup

Complete initial OAuth authentication:

1. Run authentication setup (first time only):
   ```bash
   dotnet test --filter "Category=Integration&Focus=Authentication" --logger "console;verbosity=detailed"
   ```

2. Follow OAuth flow prompts to authorize the application
3. Tokens will be stored in `integration-test-tokens.json` for subsequent runs

## Running Tests

### Test Categories

Tests are organized by category and can be run selectively:

```bash
# All integration tests
dotnet test --filter "Category=Integration"

# Authentication tests only
dotnet test --filter "Category=Integration&Focus=Authentication"

# Performance tests only
dotnet test --filter "Category=Integration&Focus=Performance"

# Specific client tests
dotnet test --filter "Category=Integration&Client=QualitySafety"

# End-to-end workflow tests
dotnet test --filter "Category=Integration&Focus=EndToEnd"
```

### Test Execution Tiers

**Fast Tests** (< 30 seconds):
```bash
dotnet test --filter "Category=Integration&Priority=High"
```

**Medium Tests** (< 5 minutes):
```bash
dotnet test --filter "Category=Integration&TestType=ClientIntegration"
```

**Slow Tests** (< 30 minutes):
```bash
dotnet test --filter "Category=Integration&Focus=Performance"
```

### Performance Testing

Run comprehensive performance validation:

```bash
# Response time validation
dotnet test --filter "TestType=ResponseTime"

# Concurrency testing
dotnet test --filter "TestType=Concurrency"

# Rate limiting compliance
dotnet test --filter "TestType=RateLimiting"

# NBomber load testing
dotnet test --filter "External=NBomber"
```

## Test Features

### Realistic Test Data

The test suite uses the Bogus library to generate realistic test data:

- **Projects**: Complete project information with addresses, budgets, timelines
- **Observations**: Safety and quality observations with proper categorization
- **Invoices**: Financial data with realistic amounts and approval workflows
- **Resources**: Workforce planning with proper trade classifications
- **Reports**: Comprehensive reporting with date ranges and metrics

### Performance Monitoring

All tests include automatic performance tracking:

- **Response Times**: P50, P95, P99 percentiles
- **Memory Usage**: Memory leak detection and resource efficiency
- **Concurrency**: Thread safety and resource contention
- **Rate Limiting**: API limit compliance and backoff strategies

### Error Handling Validation

Tests validate error scenarios:

- **Network Failures**: Connection timeouts and interruptions
- **API Errors**: 4xx/5xx status codes with proper exception mapping
- **Authentication**: Token expiration and refresh scenarios
- **Rate Limiting**: 429 responses with exponential backoff

### Data Consistency

Cross-client data integrity validation:

- **Project Lifecycle**: Data consistency across all clients
- **Referential Integrity**: Foreign key relationships maintained
- **Timestamp Consistency**: Creation and modification times
- **Status Synchronization**: Status updates reflected across systems

## Configuration Options

### Performance Thresholds

Customize performance expectations:

```json
{
  "TestSettings": {
    "PerformanceThresholds": {
      "AuthenticationMs": 2000,
      "ApiOperationMs": 5000,
      "BulkOperationMs": 30000
    }
  }
}
```

### Test Behavior

Control test execution:

```json
{
  "TestSettings": {
    "TimeoutSeconds": 30,
    "MaxRetries": 3,
    "ConcurrentRequests": 10
  }
}
```

## Continuous Integration

### CI/CD Pipeline Integration

```yaml
# GitHub Actions example
- name: Run Integration Tests
  run: |
    dotnet test tests/Procore.SDK.IntegrationTests.Live \
      --filter "Category=Integration&Priority=High" \
      --logger "trx;LogFileName=integration-results.trx" \
      --logger "console;verbosity=normal" \
      --collect:"XPlat Code Coverage"
  env:
    PROCORE_INTEGRATION_Procore__ClientId: ${{ secrets.PROCORE_CLIENT_ID }}
    PROCORE_INTEGRATION_Procore__ClientSecret: ${{ secrets.PROCORE_CLIENT_SECRET }}
```

### Test Scheduling

**Pull Request Tests**:
- Authentication validation
- Core client operations
- High-priority workflow tests

**Nightly Tests**:
- Full test suite execution
- Performance regression detection
- Load testing scenarios

**Release Tests**:
- Complete integration validation
- Performance benchmarking
- Stress testing scenarios

## Troubleshooting

### Common Issues

**Authentication Failures**:
```
Error: No valid authentication token available
Solution: Run OAuth setup or verify credentials
```

**Rate Limiting**:
```
Error: HTTP 429 - Too Many Requests
Solution: Tests handle rate limiting automatically with backoff
```

**Sandbox Unavailability**:
```
Error: Connection timeout to sandbox
Solution: Verify sandbox status and network connectivity
```

### Debug Mode

Enable detailed logging:

```bash
export ASPNETCORE_ENVIRONMENT=Development
dotnet test --logger "console;verbosity=detailed"
```

### Test Data Cleanup

Tests automatically clean up created data, but manual cleanup may be needed:

```bash
# List test projects
GET /rest/v1.0/companies/{company_id}/projects?filters[name]=*Integration%20Test*

# Clean up test data
DELETE /rest/v1.0/companies/{company_id}/projects/{project_id}
```

## Performance Reports

Tests generate comprehensive performance reports:

- **HTML Reports**: Visual performance dashboards
- **CSV Data**: Raw metrics for analysis
- **JSON Exports**: Machine-readable performance data
- **NBomber Reports**: Detailed load testing analysis

Report location: `./performance-reports/`

## Contributing

### Adding New Tests

1. Inherit from `IntegrationTestBase`
2. Use realistic test data builders
3. Include performance tracking
4. Add appropriate test traits
5. Document test scenarios

### Test Best Practices

- **Isolation**: Tests should not depend on each other
- **Cleanup**: Use fixture disposal for resource cleanup
- **Performance**: Track and validate response times
- **Error Handling**: Test both success and failure scenarios
- **Documentation**: Include clear test descriptions and expected outcomes

## Support

For issues with the integration test suite:

1. Check sandbox connectivity and credentials
2. Verify OAuth authentication setup
3. Review test logs for specific error messages
4. Consult Procore API documentation for endpoint requirements
5. Open GitHub issues for SDK-specific problems

---

**Note**: These tests run against live Procore sandbox environments and require valid credentials and network connectivity. Test execution times may vary based on API response times and rate limiting.