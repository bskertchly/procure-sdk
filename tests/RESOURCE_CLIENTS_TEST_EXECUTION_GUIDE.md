# Resource Clients Test Execution Guide

This guide provides comprehensive instructions for executing tests across all 5 resource client test projects that support the Procore SDK additional resource implementations.

## Test Projects Overview

The following test projects provide comprehensive coverage for each resource client:

1. **Procore.SDK.ProjectManagement.Tests** - Project lifecycle, budgets, contracts, workflows
2. **Procore.SDK.QualitySafety.Tests** - Observations, inspections, safety management
3. **Procore.SDK.ConstructionFinancials.Tests** - Financial operations, invoicing, costs
4. **Procore.SDK.FieldProductivity.Tests** - Field operations, productivity tracking
5. **Procore.SDK.ResourceManagement.Tests** - Resource allocation, workforce planning

## Test Architecture Summary

Each resource client test project follows the established pattern:

```
Procore.SDK.{ResourceArea}.Tests/
├── GlobalUsings.cs                 # Standard test imports
├── Procore.SDK.{ResourceArea}.Tests.csproj  # Project configuration
├── Models/                         # Domain models and interfaces
│   └── TestModels.cs              # Resource-specific domain models and interfaces
├── Interfaces/                     # Interface definition tests
│   └── I{ResourceArea}ClientTests.cs  # API surface validation
├── {ResourceArea}ClientTests.cs    # Main wrapper implementation tests
├── ErrorHandling/                  # Error mapping tests (to be implemented)
│   └── ErrorMappingTests.cs       # HTTP error to domain exception mapping
├── Authentication/                 # Authentication integration (to be implemented)
│   └── AuthenticationIntegrationTests.cs  # Token management integration
├── ResourceManagement/             # Resource disposal (to be implemented)
│   └── DisposalTests.cs           # IDisposable implementation tests
└── Integration/                    # End-to-end tests (to be implemented)
    └── EndToEndTests.cs           # Real API integration tests
```

## Quick Start

### Run All Resource Client Tests

```bash
# From the solution root
dotnet test tests/Procore.SDK.ProjectManagement.Tests/
dotnet test tests/Procore.SDK.QualitySafety.Tests/
dotnet test tests/Procore.SDK.ConstructionFinancials.Tests/
dotnet test tests/Procore.SDK.FieldProductivity.Tests/
dotnet test tests/Procore.SDK.ResourceManagement.Tests/
```

### Run All Tests in Parallel

```bash
# Run all resource client tests simultaneously
dotnet test tests/Procore.SDK.*.Tests/ --parallel
```

## Test Categories

### 1. Interface Definition Tests (✅ Implemented)

These tests validate the expected API surface for each resource client:

```bash
# Run interface definition tests for all clients
dotnet test --filter "FullyQualifiedName~IClientTests"

# Run for specific client
dotnet test tests/Procore.SDK.ProjectManagement.Tests/ --filter "FullyQualifiedName~IProjectManagementClientTests"
```

**Current Status**: ✅ Completed for ProjectManagement and QualitySafety

### 2. Implementation Tests (🚧 In Progress)

These tests validate the wrapper implementation functionality:

```bash
# Run implementation tests for all clients  
dotnet test --filter "FullyQualifiedName~ClientTests"

# Run for specific operations
dotnet test --filter "FullyQualifiedName~ProjectOperations"
dotnet test --filter "FullyQualifiedName~ObservationOperations"
```

**Current Status**: 🚧 Template implemented for ProjectManagement

### 3. Authentication Integration Tests (🔲 Pending)

These tests validate authentication integration:

```bash
# Run authentication tests
dotnet test --filter "FullyQualifiedName~AuthenticationIntegration"
```

**Current Status**: 🔲 To be implemented

### 4. Error Handling Tests (🔲 Pending)

These tests validate error mapping and handling:

```bash
# Run error handling tests
dotnet test --filter "FullyQualifiedName~ErrorMapping"
```

**Current Status**: 🔲 To be implemented

### 5. Integration Tests (🔲 Pending)

These tests validate real API connectivity:

```bash
# Set up credentials first
export Procore__ClientId="your_client_id"
export Procore__ClientSecret="your_client_secret"
export Procore__BaseUrl="https://sandbox.procore.com"

# Run integration tests
dotnet test --filter "Category=Integration"
```

**Current Status**: 🔲 To be implemented

## Domain-Specific Test Scenarios

### ProjectManagement Tests

**Test Scenarios**:
- Project CRUD operations
- Budget management (line items, changes, forecasting)
- Contract management (commitments, change orders)
- Workflow management (instances, templates, control)
- Meeting management and scheduling

```bash
# Run ProjectManagement specific tests
dotnet test tests/Procore.SDK.ProjectManagement.Tests/

# Run specific operation tests
dotnet test tests/Procore.SDK.ProjectManagement.Tests/ --filter "FullyQualifiedName~Project"
dotnet test tests/Procore.SDK.ProjectManagement.Tests/ --filter "FullyQualifiedName~Budget"
dotnet test tests/Procore.SDK.ProjectManagement.Tests/ --filter "FullyQualifiedName~Contract"
```

### QualitySafety Tests

**Test Scenarios**:
- Safety observation management
- Inspection workflows and templates
- Incident reporting and tracking
- Compliance checking and validation

```bash
# Run QualitySafety specific tests
dotnet test tests/Procore.SDK.QualitySafety.Tests/

# Run specific operation tests
dotnet test tests/Procore.SDK.QualitySafety.Tests/ --filter "FullyQualifiedName~Observation"
dotnet test tests/Procore.SDK.QualitySafety.Tests/ --filter "FullyQualifiedName~Inspection"
dotnet test tests/Procore.SDK.QualitySafety.Tests/ --filter "FullyQualifiedName~Safety"
```

### ConstructionFinancials Tests

**Test Scenarios**:
- Invoice processing and approval workflows
- Financial transaction management
- Cost code tracking and analysis
- Payment processing and reconciliation

```bash
# Run ConstructionFinancials specific tests
dotnet test tests/Procore.SDK.ConstructionFinancials.Tests/

# Run specific operation tests
dotnet test tests/Procore.SDK.ConstructionFinancials.Tests/ --filter "FullyQualifiedName~Invoice"
dotnet test tests/Procore.SDK.ConstructionFinancials.Tests/ --filter "FullyQualifiedName~Transaction"
dotnet test tests/Procore.SDK.ConstructionFinancials.Tests/ --filter "FullyQualifiedName~Cost"
```

### FieldProductivity Tests

**Test Scenarios**:
- Productivity reporting and tracking
- Field activity management
- Resource utilization analysis
- Performance metrics and benchmarking

```bash
# Run FieldProductivity specific tests
dotnet test tests/Procore.SDK.FieldProductivity.Tests/

# Run specific operation tests
dotnet test tests/Procore.SDK.FieldProductivity.Tests/ --filter "FullyQualifiedName~Productivity"
dotnet test tests/Procore.SDK.FieldProductivity.Tests/ --filter "FullyQualifiedName~Activity"
dotnet test tests/Procore.SDK.FieldProductivity.Tests/ --filter "FullyQualifiedName~Resource"
```

### ResourceManagement Tests

**Test Scenarios**:
- Resource allocation and scheduling
- Workforce planning and assignments
- Capacity planning and optimization
- Resource utilization analytics

```bash
# Run ResourceManagement specific tests
dotnet test tests/Procore.SDK.ResourceManagement.Tests/

# Run specific operation tests
dotnet test tests/Procore.SDK.ResourceManagement.Tests/ --filter "FullyQualifiedName~Resource"
dotnet test tests/Procore.SDK.ResourceManagement.Tests/ --filter "FullyQualifiedName~Allocation"
dotnet test tests/Procore.SDK.ResourceManagement.Tests/ --filter "FullyQualifiedName~Workforce"
```

## Performance Testing

### Response Time Benchmarks

Each resource client has specific performance targets:

```bash
# Run performance benchmarks (when implemented)
dotnet test --filter "Category=Performance"

# Monitor execution times
dotnet test --logger:console;verbosity=normal
```

**Performance Targets**:
- ProjectManagement: <3s for project operations, <5s for budget analysis
- QualitySafety: <2s for observations, <4s for inspection workflows  
- ConstructionFinancials: <3s for invoices, <5s for financial reporting
- FieldProductivity: <2s for productivity reports, <3s for analytics
- ResourceManagement: <3s for allocations, <4s for optimization

### Load Testing

```bash
# Run concurrent user simulation (when implemented)
dotnet test --filter "Category=Load"
```

**Load Targets**:
- 50+ concurrent users per resource client
- 100+ concurrent operations across all clients
- Memory efficiency with large datasets

## CI/CD Integration

### GitHub Actions Integration

```yaml
# .github/workflows/resource-clients-tests.yml
name: Resource Clients Tests

on: [push, pull_request]

jobs:
  test-resource-clients:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --no-restore
      
    - name: Test ProjectManagement
      run: dotnet test tests/Procore.SDK.ProjectManagement.Tests/ --no-build --verbosity normal
      
    - name: Test QualitySafety
      run: dotnet test tests/Procore.SDK.QualitySafety.Tests/ --no-build --verbosity normal
      
    - name: Test ConstructionFinancials
      run: dotnet test tests/Procore.SDK.ConstructionFinancials.Tests/ --no-build --verbosity normal
      
    - name: Test FieldProductivity
      run: dotnet test tests/Procore.SDK.FieldProductivity.Tests/ --no-build --verbosity normal
      
    - name: Test ResourceManagement
      run: dotnet test tests/Procore.SDK.ResourceManagement.Tests/ --no-build --verbosity normal
```

### Test Coverage Reporting

```bash
# Generate coverage reports
dotnet test --collect:"XPlat Code Coverage"

# Generate combined coverage report
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:./coverage-report -reporttypes:Html
```

**Coverage Targets**:
- Unit Tests: 95%+ line coverage
- Integration Tests: Critical path coverage
- Error Handling: 100% scenario coverage

## Development Workflow

### TDD Development Process

1. **Red Phase**: Tests fail (implementation doesn't exist)
2. **Green Phase**: Implement minimal code to pass tests
3. **Refactor Phase**: Improve implementation while keeping tests green

### Implementation Checklist

For each resource client, implement in order:

1. ✅ **Interface Definition Tests** - Define expected API surface
2. 🚧 **Domain Models** - Create test models and interfaces
3. 🔲 **Client Implementation** - Implement wrapper client
4. 🔲 **Error Handling** - Add domain-specific error mapping
5. 🔲 **Authentication Integration** - Integrate with auth system
6. 🔲 **Resource Management** - Implement IDisposable pattern
7. 🔲 **Integration Tests** - Add real API connectivity tests

### Test Development Guidelines

#### Adding New Test Categories

1. **Create Test File**: Add new test class following naming conventions
2. **Implement Test Methods**: Use AAA pattern (Arrange-Act-Assert)
3. **Add to CI/CD**: Include in automated test execution
4. **Document Scenarios**: Update test documentation

#### Test Naming Conventions

- **Interface Tests**: `I{ResourceArea}Client_Should_Define_{Operation}_{Context}`
- **Implementation Tests**: `{Method}_Should_{ExpectedBehavior}_When_{Condition}`
- **Error Tests**: `{Method}_Should_Throw_{ExceptionType}_When_{ErrorCondition}`
- **Integration Tests**: `{ResourceArea}Client_Should_{Behavior}_With_Real_API`

## Troubleshooting

### Common Issues

1. **Build Failures**
   ```bash
   # Restore packages
   dotnet restore
   
   # Clean and rebuild
   dotnet clean
   dotnet build
   ```

2. **Test Discovery Issues**
   ```bash
   # List discovered tests
   dotnet test --list-tests
   
   # Run with verbose output
   dotnet test --verbosity diagnostic
   ```

3. **Authentication Failures** (for integration tests)
   ```bash
   # Verify environment variables
   echo $Procore__ClientId
   echo $Procore__BaseUrl
   
   # Check test configuration
   cat tests/Procore.SDK.{ResourceArea}.Tests/appsettings.test.json
   ```

4. **Performance Issues**
   ```bash
   # Run tests with timing
   dotnet test --logger:console;verbosity=normal
   
   # Profile memory usage
   dotnet test --collect:"DotNet Memory"
   ```

### Debug Test Execution

```bash
# Debug specific test
dotnet test --filter "FullyQualifiedName=Procore.SDK.ProjectManagement.Tests.ProjectManagementClientTests.GetProjectsAsync_Should_Return_Projects_For_Company" --logger:console;verbosity=diagnostic

# Debug with breakpoints (Visual Studio/Rider)
# Set breakpoints in test methods and run in debug mode
```

## Implementation Status

### Current Progress

- ✅ **Test Infrastructure**: Project setup and configuration
- ✅ **Interface Definitions**: API surface definitions for all clients
- ✅ **Test Models**: Domain models and request/response types
- 🚧 **Implementation Tests**: Template created for ProjectManagement
- 🔲 **Authentication Tests**: To be implemented
- 🔲 **Error Handling Tests**: To be implemented  
- 🔲 **Integration Tests**: To be implemented
- 🔲 **Performance Tests**: To be implemented

### Next Steps

1. **Complete Implementation Tests**: Finish all 5 resource client implementation tests
2. **Add Authentication Integration**: Implement auth tests for all clients
3. **Create Error Handling**: Add comprehensive error mapping tests
4. **Build Integration Tests**: Add real API connectivity validation
5. **Performance Validation**: Implement benchmark and load tests

### Success Criteria

**Functional Success**:
- ✅ Complete API surface coverage for all 5 resource clients
- ✅ Domain-specific test models and interfaces
- 🔲 Comprehensive implementation test coverage
- 🔲 Authentication integration validation
- 🔲 Error handling and mapping verification

**Quality Success**:
- 🔲 95%+ unit test coverage for all clients
- 🔲 Real API integration test coverage
- 🔲 Performance benchmark compliance
- 🔲 Security validation completion

This comprehensive test strategy ensures robust, secure, and performant resource clients that follow established patterns while addressing domain-specific requirements for each area of the Procore SDK.