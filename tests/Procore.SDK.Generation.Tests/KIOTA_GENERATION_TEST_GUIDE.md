# Kiota Client Generation & Compilation Test Execution Guide

This guide provides comprehensive instructions for executing tests related to **Task 9: Kiota Client Generation & Compilation Fix**.

## Overview

The Kiota Generation tests validate that all generated clients compile correctly, function as expected, and integrate properly with wrapper clients and authentication systems.

## Test Categories

### 1. Compilation Tests ✅
**File**: `KiotaGeneration/GeneratedClientCompilationTests.cs`
**Purpose**: Validate that all generated Kiota clients compile without errors

```bash
# Run only compilation tests
dotnet test --filter "FullyQualifiedName~GeneratedClientCompilationTests"

# Run with detailed output
dotnet test --filter "FullyQualifiedName~GeneratedClientCompilationTests" --logger "console;verbosity=detailed"
```

### 2. Functionality Tests ✅
**File**: `KiotaGeneration/GeneratedClientFunctionalityTests.cs`
**Purpose**: Verify that generated clients expose expected API operations

```bash
# Run only functionality tests
dotnet test --filter "FullyQualifiedName~GeneratedClientFunctionalityTests"

# Run specific functionality test
dotnet test --filter "FullyQualifiedName~GeneratedClientFunctionalityTests.ProjectManagementClient_Should_Expose_Project_Operations"
```

### 3. Integration Tests ✅
**File**: `KiotaGeneration/WrapperClientIntegrationTests.cs`
**Purpose**: Validate integration between wrapper and generated clients

```bash
# Run only integration tests
dotnet test --filter "FullyQualifiedName~WrapperClientIntegrationTests"

# Run authentication-specific tests
dotnet test --filter "FullyQualifiedName~WrapperClientIntegrationTests" --filter "FullyQualifiedName~Authentication"
```

### 4. Configuration Tests ✅
**File**: `KiotaGeneration/KiotaConfigurationTests.cs"
**Purpose**: Validate Kiota generation settings and OpenAPI parsing

```bash
# Run only configuration tests
dotnet test --filter "FullyQualifiedName~KiotaConfigurationTests"

# Run OpenAPI-specific tests
dotnet test --filter "FullyQualifiedName~KiotaConfigurationTests" --filter "FullyQualifiedName~OpenAPI"
```

### 5. Performance Tests ✅
**File**: `KiotaGeneration/GeneratedClientPerformanceTests.cs`
**Purpose**: Validate performance and memory characteristics

```bash
# Run only performance tests
dotnet test --filter "FullyQualifiedName~GeneratedClientPerformanceTests"

# Run memory-specific tests
dotnet test --filter "FullyQualifiedName~GeneratedClientPerformanceTests" --filter "FullyQualifiedName~Memory"
```

## Complete Test Suite Execution

### Run All Kiota Generation Tests
```bash
# Run all tests in the Kiota generation category
dotnet test --filter "FullyQualifiedName~KiotaGeneration"

# Run with code coverage
dotnet test --filter "FullyQualifiedName~KiotaGeneration" --collect:"XPlat Code Coverage"

# Run with detailed logging and results export
dotnet test --filter "FullyQualifiedName~KiotaGeneration" \
  --logger "console;verbosity=detailed" \
  --logger "trx;LogFileName=kiota-tests.trx" \
  --results-directory ./TestResults
```

### Run All Generation Tests (including other generation tests)
```bash
# Run all tests in the Generation test project
dotnet test tests/Procore.SDK.Generation.Tests/

# Run with parallel execution disabled (for performance tests)
dotnet test tests/Procore.SDK.Generation.Tests/ --parallel none
```

## Test Environment Setup

### Prerequisites
1. **.NET 8.0 SDK** installed
2. **All project dependencies** restored
3. **Generated clients** must be present (run Kiota generation first)

### Environment Validation
```bash
# Verify .NET version
dotnet --version

# Restore all dependencies
dotnet restore

# Build all projects
dotnet build

# Verify generated clients exist
ls src/*/Generated/
```

## Test Execution Strategies

### Development Workflow

#### Quick Validation (< 2 minutes)
```bash
# Run compilation tests only
dotnet test --filter "FullyQualifiedName~GeneratedClientCompilationTests"
```

#### Standard Validation (< 5 minutes)
```bash
# Run compilation and functionality tests
dotnet test --filter "FullyQualifiedName~GeneratedClientCompilationTests|FullyQualifiedName~GeneratedClientFunctionalityTests"
```

#### Comprehensive Validation (< 15 minutes)
```bash
# Run all Kiota generation tests
dotnet test --filter "FullyQualifiedName~KiotaGeneration"
```

#### Full Suite (< 30 minutes)
```bash
# Run all generation tests including performance
dotnet test tests/Procore.SDK.Generation.Tests/ --parallel none
```

### Continuous Integration

#### Pre-Commit Validation
```bash
#!/bin/bash
# Pre-commit hook for Kiota generation changes

echo "Running Kiota generation validation..."

# Quick compilation check
dotnet test --filter "FullyQualifiedName~GeneratedClientCompilationTests" --logger "console;verbosity=minimal"

if [ $? -eq 0 ]; then
  echo "✅ Kiota generation validation passed"
  exit 0
else
  echo "❌ Kiota generation validation failed"
  exit 1
fi
```

#### Build Pipeline Integration
```yaml
# Azure DevOps Pipeline Example
- task: DotNetCoreCLI@2
  displayName: 'Run Kiota Generation Tests'
  inputs:
    command: 'test'
    projects: 'tests/Procore.SDK.Generation.Tests/'
    arguments: '--filter "FullyQualifiedName~KiotaGeneration" --logger trx --collect "XPlat Code Coverage"'
    publishTestResults: true
```

#### GitHub Actions Integration
```yaml
# GitHub Actions Workflow Example
- name: Run Kiota Generation Tests
  run: |
    dotnet test tests/Procore.SDK.Generation.Tests/ \
      --filter "FullyQualifiedName~KiotaGeneration" \
      --logger "console;verbosity=minimal" \
      --logger "trx;LogFileName=kiota-tests.trx" \
      --collect "XPlat Code Coverage"
```

## Troubleshooting Common Issues

### Compilation Test Failures

#### Issue: Generated clients not found
```
System.TypeLoadException: Could not load type 'Procore.SDK.ProjectManagement.ProjectManagementClient'
```

**Solution**:
```bash
# Regenerate clients
dotnet run --project tools/ClientGenerator

# Rebuild projects
dotnet build

# Re-run tests
dotnet test --filter "FullyQualifiedName~GeneratedClientCompilationTests"
```

#### Issue: Namespace resolution problems
```
System.ArgumentNullException: Value cannot be null. (Parameter 'type')
```

**Solution**:
1. Verify all project references are correct
2. Check that generated clients are in the expected namespaces
3. Ensure proper using statements in test files

### Functionality Test Failures

#### Issue: Missing API endpoints
```
AssertionFailedException: Expected client.Rest.V10.Projects to not be null
```

**Solution**:
1. Verify Kiota generation included expected paths
2. Check kiota-lock.json for proper path filtering
3. Regenerate clients with updated path filters

#### Issue: Request builder navigation fails
```
NullReferenceException: Object reference not set to an instance of an object
```

**Solution**:
1. Verify the generated client structure matches expectations
2. Check for breaking changes in Kiota version
3. Validate OpenAPI specification structure

### Integration Test Failures

#### Issue: Authentication integration problems
```
InvalidOperationException: Authentication handler not properly configured
```

**Solution**:
1. Verify mock request adapter setup
2. Check authentication flow integration
3. Ensure proper token manager configuration

### Configuration Test Failures

#### Issue: Missing kiota-lock.json files
```
FileNotFoundException: Could not find file 'kiota-lock.json'
```

**Solution**:
```bash
# Regenerate all clients
./tools/generate-clients.sh

# Verify lock files exist
find . -name "kiota-lock.json" -type f
```

#### Issue: OpenAPI specification validation fails
```
JsonException: The JSON value could not be converted to System.Text.Json.JsonElement
```

**Solution**:
1. Verify OpenAPI specification is valid JSON
2. Check file permissions and accessibility
3. Validate OpenAPI structure meets expectations

### Performance Test Failures

#### Issue: Performance thresholds exceeded
```
AssertionFailedException: Average instantiation time should be < 10ms, was 15.2ms
```

**Solution**:
1. Run tests on a faster machine or less loaded environment
2. Adjust performance thresholds if consistently failing
3. Profile code to identify performance bottlenecks

#### Issue: Memory leak detection
```
AssertionFailedException: Memory increase should be < 50MB, was 75.3MB
```

**Solution**:
1. Force garbage collection before measuring
2. Check for static references holding objects
3. Verify proper disposal patterns

## Test Result Analysis

### Understanding Test Output

#### Successful Test Run
```
✅ ProjectManagement client instantiated successfully
✅ Core client exposes expected core operations across multiple API versions
✅ Wrapper client successfully integrates with generated client
✅ All lock files reference the same OpenAPI specification
✅ Generated clients handle 50 concurrent operations without issues

Test Run Successful.
Total tests: 87
     Passed: 87
     Failed: 0
     Skipped: 0
```

#### Failed Test Analysis
```
❌ Test Failed: GeneratedClient_Should_Compile_And_Instantiate[ProjectManagement]
   Expected: True
   Actual: False
   Message: Could not instantiate ProjectManagement client

Stack Trace:
   at GeneratedClientCompilationTests.ProjectManagementClient_Should_Compile_And_Instantiate()
```

### Performance Metrics

Monitor these key performance indicators:

- **Client Instantiation**: < 10ms average
- **Memory Usage**: < 100KB per client instance
- **Request Builder Creation**: < 0.1ms per builder
- **Concurrent Operations**: Support 50+ concurrent users

### Coverage Metrics

Target coverage goals:

- **Compilation Tests**: 100% (all clients must compile)
- **Functionality Tests**: 95% (core API surface coverage)
- **Integration Tests**: 90% (critical integration paths)
- **Configuration Tests**: 85% (generation quality validation)
- **Performance Tests**: 80% (performance characteristic validation)

## Advanced Test Scenarios

### Load Testing
```bash
# Run performance tests with increased load
dotnet test --filter "FullyQualifiedName~GeneratedClientPerformanceTests.Multiple_Client_Instances_Should_Work_Concurrently" \
  --logger "console;verbosity=detailed"
```

### Memory Profiling
```bash
# Run with memory profiling (requires additional tools)
dotnet test --filter "FullyQualifiedName~GeneratedClientPerformanceTests" \
  --collect "DotMemory" \
  --settings profiling.runsettings
```

### Stress Testing
```bash
# Run tests multiple times to detect race conditions
for i in {1..10}; do
  echo "Run $i"
  dotnet test --filter "FullyQualifiedName~WrapperClientIntegrationTests" || break
done
```

## Maintenance and Updates

### Regular Maintenance Tasks

1. **Update Performance Baselines**: Review and adjust performance thresholds quarterly
2. **Validate Test Coverage**: Ensure new generated clients are covered by tests
3. **Review Configuration**: Update tests when OpenAPI specification changes
4. **Performance Monitoring**: Track performance trends over time

### Test Evolution

As the codebase evolves:

1. **Add New Client Tests**: When new generated clients are added
2. **Update API Surface Tests**: When API endpoints change
3. **Modify Performance Targets**: As infrastructure improves
4. **Enhance Integration Tests**: As authentication patterns evolve

This comprehensive test execution guide ensures thorough validation of the Kiota client generation and compilation functionality while providing clear guidance for both development and CI/CD scenarios.