# Test Execution Guide for Procore SDK Sample Applications

## Overview

This guide provides comprehensive instructions for executing the test suite for Procore SDK sample applications, covering OAuth PKCE flow implementation, API integration, and performance validation.

## Test Categories and Execution

### 1. Unit Tests

#### Console Application Authentication Tests
```bash
# Run console OAuth flow tests
dotnet test --filter "FullyQualifiedName~ConsoleOAuthFlowTests"

# Run specific test methods
dotnet test --filter "Method=ConsoleApp_InitialAuthentication_ShouldGenerateValidAuthUrl"
dotnet test --filter "Method=ConsoleApp_UserProvidesAuthCode_ShouldExchangeForToken"
```

#### Web Application Authentication Tests
```bash
# Run web application callback tests
dotnet test --filter "FullyQualifiedName~CallbackHandlingTests"

# Run specific web app scenarios
dotnet test --filter "Method=WebApp_ValidCallback_ShouldProcessSuccessfullyAndRedirect"
dotnet test --filter "Method=WebApp_CallbackWithInvalidState_ShouldRejectAndShowError"
```

### 2. Integration Tests

#### CRUD Operations Tests
```bash
# Run all API integration tests
dotnet test --filter "FullyQualifiedName~CrudOperationsTests"

# Run specific API operation tests
dotnet test --filter "Method=CoreClient_AuthenticatedRequest_ShouldIncludeProperHeaders"
dotnet test --filter "Method=CoreClient_TokenExpiredDuringRequest_ShouldRefreshAutomatically"
```

#### End-to-End Workflow Tests
```bash
# Run complete workflow tests
dotnet test --filter "FullyQualifiedName~EndToEndWorkflowTests"

# Run specific workflow scenarios
dotnet test --filter "Method=EndToEnd_ConsoleApplicationWorkflow_ShouldCompleteSuccessfully"
dotnet test --filter "Method=EndToEnd_FullProjectLifecycleWorkflow_ShouldCompleteAllOperations"
```

### 3. Performance Tests

#### Authentication Performance Tests
```bash
# Run performance benchmarks
dotnet test --filter "Category=Performance"

# Run specific performance tests
dotnet test --filter "Method=AuthURL_Generation_ShouldCompleteUnderOneMillisecond"
dotnet test --filter "Method=TokenExchange_ShouldCompleteUnder100Milliseconds"
```

#### Load Testing
```bash
# Run load tests specifically
dotnet test --filter "FullyQualifiedName~AuthenticationPerformanceTests" --filter "Method~LoadTest"

# Run stress tests
dotnet test --filter "Method~StressTest"
```

## Test Execution Strategies

### Quick Validation (< 5 minutes)
Essential tests for basic functionality validation:

```bash
# Core authentication flow validation
dotnet test --filter "TestCategory=Essential|Priority=High" 

# Specific essential tests
dotnet test --filter "Method=ConsoleApp_InitialAuthentication_ShouldGenerateValidAuthUrl"
dotnet test --filter "Method=WebApp_ValidCallback_ShouldProcessSuccessfullyAndRedirect"
dotnet test --filter "Method=EndToEnd_ConsoleApplicationWorkflow_ShouldCompleteSuccessfully"
```

### Comprehensive Testing (15-30 minutes)
Full test suite execution:

```bash
# Run all tests with detailed output
dotnet test --verbosity detailed

# Run with coverage collection
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Run with logger for CI/CD
dotnet test --logger "trx;LogFileName=TestResults.trx" --logger "console;verbosity=detailed"
```

### Continuous Integration Testing
Optimized for CI/CD pipelines:

```bash
# Parallel execution with retry on failure
dotnet test --parallel --retry-failed-tests 3 --logger "trx" --collect:"XPlat Code Coverage"

# With timeout and specific categories
dotnet test --filter "TestCategory!=Manual" --blame-hang-timeout 5m --blame-crash
```

## Test Configuration

### Environment Setup

#### Required Configuration Files
1. **appsettings.test.json**
   ```json
   {
     "ProcoreAuth": {
       "ClientId": "test-client-id",
       "ClientSecret": "test-client-secret",
       "RedirectUri": "https://localhost:5001/auth/callback",
       "Scopes": ["read", "write", "admin"]
     }
   }
   ```

2. **Test Environment Variables**
   ```bash
   export ASPNETCORE_ENVIRONMENT=Testing
   export PROCORE_CLIENT_ID=test-client-id
   export PROCORE_CLIENT_SECRET=test-client-secret
   ```

#### User Secrets Setup (Development)
```bash
# Initialize user secrets for test project
dotnet user-secrets init --project tests/Procore.SDK.Samples.Tests

# Set test credentials
dotnet user-secrets set "ProcoreAuth:ClientId" "your-test-client-id"
dotnet user-secrets set "ProcoreAuth:ClientSecret" "your-test-client-secret"
```

### Test Categories and Filtering

#### Available Test Categories
- **Unit**: Individual component testing
- **Integration**: Component interaction testing  
- **Performance**: Benchmarking and load testing
- **EndToEnd**: Complete workflow validation
- **Manual**: Tests requiring manual intervention

#### Priority Levels
- **High**: Critical functionality that must work
- **Medium**: Important features with good coverage
- **Low**: Edge cases and optimization scenarios

#### Filtering Examples
```bash
# Run only high-priority tests
dotnet test --filter "Priority=High"

# Run integration and end-to-end tests
dotnet test --filter "TestCategory=Integration|TestCategory=EndToEnd"

# Exclude performance tests (faster execution)
dotnet test --filter "TestCategory!=Performance"

# Run tests for specific sample application
dotnet test --filter "FullyQualifiedName~ConsoleApp"
dotnet test --filter "FullyQualifiedName~WebApp"
```

## Performance Benchmarking

### BenchmarkDotNet Integration
For detailed performance analysis:

```bash
# Run performance benchmarks in Release mode
dotnet run --project tests/Procore.SDK.Samples.Tests --configuration Release -- --benchmark

# Generate performance reports
dotnet run --configuration Release -- --benchmark --exporters json,html
```

### Performance Baselines
Expected performance characteristics:

| Operation | Baseline | Acceptable | Needs Investigation |
|-----------|----------|------------|-------------------|
| Auth URL Generation | <1ms | <5ms | >10ms |
| Token Exchange | <100ms | <500ms | >1000ms |
| Token Storage | <10ms | <50ms | >100ms |
| API Request | <200ms | <1000ms | >2000ms |
| Token Refresh | <500ms | <2000ms | >5000ms |

### Load Testing Scenarios
```bash
# Concurrent user simulation
dotnet test --filter "Method=EndToEnd_ConcurrentUsersWorkflow_ShouldHandleMultipleUsers"

# High-throughput testing
dotnet test --filter "Method=ConcurrentTokenOperations_ShouldHandleHighThroughput"

# Memory leak detection
dotnet test --filter "Method=MemoryUsage_ShouldRemainBoundedDuringLongRunning"
```

## Test Results Analysis

### Coverage Reports
```bash
# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"

# Convert to HTML (requires reportgenerator)
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# View coverage report
open coverage-report/index.html
```

### Test Result Analysis
```bash
# Parse test results for CI/CD
dotnet test --logger "trx" --results-directory TestResults
```

### Performance Analysis
```bash
# Memory profiling
dotnet test --collect:"Code Coverage" --diag --verbosity diagnostic

# Performance trending
dotnet test --filter "Category=Performance" --logger "trx" | grep "elapsed"
```

## Troubleshooting Common Issues

### Authentication Test Failures
1. **Invalid Mock Setup**
   ```bash
   # Check mock configuration
   dotnet test --filter "Method=TestAuthFixture" --verbosity diagnostic
   ```

2. **Timing Issues**
   ```bash
   # Run with increased timeout
   dotnet test --blame-hang-timeout 10m
   ```

### Performance Test Failures
1. **Resource Constraints**
   ```bash
   # Run with limited parallelism
   dotnet test --parallel --max-cpu-count 2
   ```

2. **Memory Issues**
   ```bash
   # Monitor memory usage
   dotnet test --collect:"Code Coverage" --diag
   ```

### Integration Test Failures
1. **Network Simulation Issues**
   ```bash
   # Verify mock HTTP handler
   dotnet test --filter "TestableHttpMessageHandler" --verbosity detailed
   ```

2. **Dependency Injection Issues**
   ```bash
   # Check service registration
   dotnet test --filter "DependencyInjection" --verbosity diagnostic
   ```

## Automated Testing Pipeline

### GitHub Actions Example
```yaml
name: Test Suite
on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0'
    
    - name: Run Tests
      run: |
        dotnet test --configuration Release \
          --logger trx \
          --collect:"XPlat Code Coverage" \
          --results-directory TestResults
    
    - name: Upload Results
      uses: actions/upload-artifact@v3
      with:
        name: test-results
        path: TestResults/
```

### Azure DevOps Pipeline
```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: DotNetCoreCLI@2
  displayName: 'Run Tests'
  inputs:
    command: 'test'
    projects: 'tests/**/*.csproj'
    arguments: '--configuration Release --logger trx --collect:"Code Coverage"'
    publishTestResults: true
```

## Custom Test Scenarios

### Manual Testing Scenarios
For scenarios requiring manual intervention:

1. **Interactive OAuth Flow**
   - Use console application with real browser interaction
   - Verify callback URL handling in web application

2. **Real API Integration**
   - Configure with actual Procore sandbox credentials
   - Test against live API endpoints (with appropriate data)

3. **Cross-Browser Testing**
   - Use Playwright for automated browser testing
   - Verify OAuth flow across different browsers

### Extended Testing Scenarios
```bash
# Long-running stability tests
dotnet test --filter "Method~LongRunning" --blame-hang-timeout 30m

# Cross-platform compatibility
dotnet test --framework net8.0 --runtime win-x64
dotnet test --framework net8.0 --runtime linux-x64
dotnet test --framework net8.0 --runtime osx-x64
```

## Test Maintenance

### Regular Maintenance Tasks
1. **Update Test Data**
   - Refresh mock responses to match API changes
   - Update test credentials periodically

2. **Performance Baseline Updates**
   - Review and adjust performance thresholds
   - Update benchmarks with infrastructure changes

3. **Coverage Analysis**
   - Identify gaps in test coverage
   - Add tests for new functionality

### Test Health Monitoring
```bash
# Check for flaky tests
dotnet test --repeat 10 --logger "trx"

# Analyze test timing trends
dotnet test --filter "Category=Performance" --verbosity diagnostic
```

## Success Criteria

### Functional Testing
- ✅ All authentication flows complete successfully
- ✅ API operations work with proper error handling
- ✅ Token management handles all lifecycle scenarios
- ✅ Security validations prevent common vulnerabilities

### Performance Testing
- ✅ Authentication operations meet performance baselines
- ✅ Concurrent operations scale appropriately
- ✅ Memory usage remains bounded during extended use
- ✅ Load testing validates production readiness

### Quality Assurance
- ✅ Code coverage exceeds 90% for critical paths
- ✅ All edge cases and error scenarios covered
- ✅ Integration tests validate real-world usage patterns
- ✅ Performance tests prevent regression

This comprehensive test execution guide ensures thorough validation of the Procore SDK sample applications across all critical scenarios and performance characteristics.