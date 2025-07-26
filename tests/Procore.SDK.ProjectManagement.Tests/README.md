# Procore SDK ProjectManagement Client Tests

This test project provides comprehensive test coverage for the Procore SDK ProjectManagement Client implementation. The tests define the expected behavior and API surface for the wrapper client that provides domain-specific convenience methods over the generated Kiota client.

## 📋 Overview

The ProjectManagement Client tests are organized into several categories that validate different aspects of the wrapper implementation:

- **Interface Definition Tests** - Define the expected API surface for project management operations
- **Wrapper Implementation Tests** - Test the ProjectManagementClient wrapper functionality
- **Error Handling Tests** - Comprehensive error mapping and handling for project management scenarios
- **Authentication Integration Tests** - Integration with the SDK's authentication system
- **Resource Management Tests** - Proper disposal and resource cleanup
- **Integration Tests** - End-to-end API connectivity validation

## 🏗️ Test Structure

### Project Organization

```
tests/Procore.SDK.ProjectManagement.Tests/
├── Procore.SDK.ProjectManagement.Tests.csproj    # Test project configuration
├── GlobalUsings.cs                                # Global using statements
├── README.md                                      # This documentation
├── Models/
│   └── TestModels.cs                             # Domain models and interface definitions
├── Interfaces/
│   └── IProjectManagementClientTests.cs         # Interface contract tests
├── ProjectManagementClientTests.cs              # Main wrapper implementation tests
├── ErrorHandling/
│   └── ErrorMappingTests.cs                     # HTTP error mapping tests
├── Authentication/
│   └── AuthenticationIntegrationTests.cs        # Auth system integration
├── ResourceManagement/
│   └── DisposalTests.cs                         # Resource disposal and cleanup
└── Integration/
    └── EndToEndTests.cs                         # Real API integration tests
```

## 🎯 Domain-Specific Test Coverage

### Project Management Operations

The ProjectManagement client provides comprehensive project lifecycle management capabilities:

#### 1. Project Operations
- **Project CRUD**: Create, read, update, delete projects
- **Project Status Management**: Handle project phases and status transitions
- **Project Filtering**: Active projects, project search by name
- **Project Budget Tracking**: Budget totals and variance analysis

#### 2. Budget Management
- **Budget Line Items**: Detailed budget breakdown and tracking
- **Budget Changes**: Change request workflow and approval
- **Budget Analysis**: Variance reporting and budget totals
- **Financial Tracking**: Actual vs budgeted amounts

#### 3. Contract Management
- **Commitment Contracts**: Contract lifecycle management
- **Change Orders**: Prime and commitment change orders
- **Contract Status Tracking**: Execution and completion workflows
- **Financial Integration**: Contract amounts and payment tracking

#### 4. Workflow Management
- **Workflow Instances**: Active workflow tracking
- **Workflow Control**: Start, restart, and terminate workflows
- **Workflow Templates**: Template-based workflow creation
- **Assignment Management**: Workflow task assignments

#### 5. Meeting Management
- **Meeting Scheduling**: Meeting creation and scheduling
- **Attendee Management**: Meeting participant tracking
- **Meeting Documentation**: Meeting minutes and outcomes
- **Calendar Integration**: Project calendar management

## 🧪 Test Categories

### 1. Interface Definition Tests (`IProjectManagementClientTests.cs`)

These tests define the expected interface contract for the ProjectManagementClient wrapper:

```csharp
[Fact]
public void IProjectManagementClient_Should_Define_Project_Operations()
{
    // Defines expected CRUD operations for projects
    var interfaceType = typeof(IProjectManagementClient);
    interfaceType.Should().HaveMethod("GetProjectsAsync", new[] { typeof(int), typeof(CancellationToken) });
    interfaceType.Should().HaveMethod("GetProjectAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    interfaceType.Should().HaveMethod("CreateProjectAsync", new[] { typeof(int), typeof(CreateProjectRequest), typeof(CancellationToken) });
    // ... more method definitions
}
```

**Key Areas Tested**:
- Project CRUD operations
- Budget management operations
- Contract and change order operations
- Workflow management operations
- Meeting management operations
- Convenience methods and pagination support

### 2. Wrapper Implementation Tests (`ProjectManagementClientTests.cs`)

These tests validate the actual implementation of the ProjectManagementClient wrapper:

```csharp
[Fact]
public async Task GetProjectsAsync_Should_Return_Projects_For_Company()
{
    // Arrange - Setup mocks and expected data
    var companyId = 123;
    var expectedProjects = new List<Project>
    {
        new() { Id = 1, Name = "Office Building", Status = ProjectStatus.Active },
        new() { Id = 2, Name = "Warehouse", Status = ProjectStatus.Planning }
    };
    _mockRequestAdapter.SendAsync(/* ... */).Returns(expectedProjects.ToArray());

    // Act - Call the wrapper method
    var result = await _sut.GetProjectsAsync(companyId);

    // Assert - Verify correct behavior
    result.Should().NotBeNull();
    result.Should().HaveCount(2);
    result.First().Name.Should().Be("Office Building");
}
```

**Test Scenarios**:
- Project lifecycle management (create, update, delete)
- Budget operations (line items, changes, analysis)
- Contract management (commitments, change orders)
- Workflow operations (instances, control, templates)
- Meeting management (scheduling, attendees, documentation)
- Convenience methods (active projects, budget totals, variances)
- Pagination support for large datasets

### 3. Error Handling Tests (`ErrorMappingTests.cs`)

Tests for mapping HTTP status codes to domain-specific exceptions:

```csharp
[Fact]
public void MapHttpException_Should_Map_404_To_ProjectNotFoundException()
{
    // Tests transformation of HTTP 404 to ProjectNotFoundException
    var httpException = new HttpRequestException("Project not found");
    httpException.Data["StatusCode"] = HttpStatusCode.NotFound;

    var result = _sut.MapHttpException(httpException);

    result.Should().BeOfType<ProjectNotFoundException>();
    result.As<ProjectNotFoundException>().ErrorCode.Should().Be("PROJECT_NOT_FOUND");
}
```

**Project Management Specific Error Mappings**:
- `400 Bad Request` → `InvalidProjectDataException`
- `401 Unauthorized` → `UnauthorizedException`
- `403 Forbidden` → `InsufficientProjectPermissionsException`
- `404 Not Found` → `ProjectNotFoundException` / `BudgetNotFoundException`
- `409 Conflict` → `ProjectConflictException` (e.g., duplicate project names)
- `422 Unprocessable Entity` → `InvalidBudgetChangeException`
- `429 Too Many Requests` → `RateLimitExceededException`
- `5xx Server Errors` → `ProjectManagementException`

### 4. Authentication Integration Tests (`AuthenticationIntegrationTests.cs`)

Tests for integration with the SDK's authentication infrastructure:

```csharp
[Fact]
public async Task ProjectManagementClient_Should_Request_Token_Before_API_Calls()
{
    // Verifies that the client properly integrates with token management
    const string expectedToken = "valid_access_token";
    _mockTokenManager.GetAccessTokenAsync(Arg.Any<CancellationToken>())
        .Returns(expectedToken);

    await _sut.GetProjectAsync(123, 456);

    // Verify token was requested and included in HTTP headers
    await _mockTokenManager.Received(1).GetAccessTokenAsync(Arg.Any<CancellationToken>());
}
```

**Authentication Scenarios Tested**:
- Token retrieval before project API calls
- Automatic token refresh on 401 responses
- Token validation for project operations
- Concurrent request handling with shared tokens
- Cancellation token propagation through auth flows

### 5. Resource Management Tests (`DisposalTests.cs`)

Tests for proper implementation of the Dispose pattern:

```csharp
[Fact]
public void ProjectManagementClient_Should_Dispose_Underlying_Resources()
{
    // Verifies proper resource cleanup
    var disposableRequestAdapter = Substitute.For<IRequestAdapter, IDisposable>();
    var client = new ProjectManagementClient(generatedClient, _mockTokenManager, _mockLogger);

    client.Dispose();

    ((IDisposable)disposableRequestAdapter).Received(1).Dispose();
}
```

**Resource Management Scenarios**:
- IDisposable implementation
- Multiple dispose calls safety
- ObjectDisposedException after disposal
- Internal cache cleanup for project data
- Thread-safe disposal

### 6. Integration Tests (`EndToEndTests.cs`)

End-to-end tests that validate real API connectivity:

```csharp
[Fact]
[Trait("Category", "Integration")]
public async Task ProjectManagementClient_Should_Retrieve_Real_Projects()
{
    // Skip if no credentials
    if (_skipIntegrationTests) return;

    var projects = await _projectManagementClient.GetProjectsAsync(companyId);

    projects.Should().NotBeNull();
    // Additional project-specific validations
}
```

**Integration Scenarios**:
- Real project data retrieval and manipulation
- Budget operations with live data
- Contract and change order workflows
- Workflow management operations
- Meeting scheduling and management
- Performance benchmarking for project operations

## 🚀 Running the Tests

### Unit Tests Only

```bash
dotnet test --filter "Category!=Integration"
```

### All Tests (Including Integration)

```bash
# Set up environment variables for integration tests
export Procore__ClientId="your_client_id"
export Procore__ClientSecret="your_client_secret"
export Procore__BaseUrl="https://sandbox.procore.com"

dotnet test
```

### Specific Test Categories

```bash
# Run only integration tests
dotnet test --filter "Category=Integration"

# Run only project operation tests
dotnet test --filter "FullyQualifiedName~ProjectOperations"

# Run only budget management tests
dotnet test --filter "FullyQualifiedName~Budget"
```

## 📊 Performance Expectations

### Response Time Targets
- **Project Operations**: <3 seconds for CRUD operations
- **Budget Operations**: <5 seconds for complex budget analysis
- **Contract Operations**: <4 seconds for contract retrieval
- **Workflow Operations**: <2 seconds for workflow management
- **Authentication**: <2 seconds for token operations
- **Error Handling**: <100ms for exception mapping

### Resource Usage
- **Memory**: Efficient handling of large project datasets
- **Concurrent Requests**: Support for 50+ simultaneous project operations
- **Thread Safety**: Safe concurrent access to project data

## 🔧 Test Configuration

### Project Dependencies

The test project references:

- **xUnit** - Testing framework
- **NSubstitute** - Mocking framework
- **FluentAssertions** - Assertion library
- **Microsoft.Kiota.*** - Kiota client libraries
- **Microsoft.Extensions.*** - Dependency injection and logging

### Test Data Configuration

Integration tests require configuration via `appsettings.test.json`:

```json
{
  "Procore": {
    "ClientId": "your_test_client_id",
    "ClientSecret": "your_test_client_secret",
    "BaseUrl": "https://sandbox.procore.com",
    "RedirectUri": "http://localhost:8080/callback"
  },
  "TestData": {
    "CompanyId": 123,
    "ProjectId": 456,
    "UserId": 789
  }
}
```

## 🎯 Implementation Checklist

Based on these tests, the actual ProjectManagementClient implementation should:

- ✅ Implement the `IProjectManagementClient` interface defined in `TestModels.cs`
- ✅ Wrap the generated Kiota client with domain-specific methods
- ✅ Map HTTP errors to appropriate project management exceptions
- ✅ Integrate with the authentication infrastructure
- ✅ Implement proper resource disposal
- ✅ Support pagination for large project datasets
- ✅ Handle concurrent requests safely
- ✅ Provide meaningful error messages and logging
- ✅ Support convenience methods for common project operations
- ✅ Implement budget management workflows
- ✅ Support contract and change order operations
- ✅ Provide workflow management capabilities
- ✅ Enable meeting scheduling and management

## 🔍 Test Development Guidelines

### Adding New Tests

When adding new project management functionality:

1. **Define Interface First**: Add method signatures to `IProjectManagementClient`
2. **Create Interface Tests**: Add validation to `IProjectManagementClientTests.cs`
3. **Implement Wrapper Tests**: Add behavior tests to `ProjectManagementClientTests.cs`
4. **Add Error Handling**: Include error scenarios in `ErrorMappingTests.cs`
5. **Include Integration Tests**: Add real API tests to `EndToEndTests.cs`

### Test Naming Conventions

- Interface tests: `IProjectManagementClient_Should_Define_{Operation}_{Context}`
- Implementation tests: `{Method}_Should_{ExpectedBehavior}_When_{Condition}`
- Error tests: `{Method}_Should_Throw_{ExceptionType}_When_{ErrorCondition}`

## 📚 Related Documentation

- [Comprehensive Test Strategy](../COMPREHENSIVE_TEST_STRATEGY.md) - Overall testing approach
- [Core Client Tests](../Procore.SDK.Core.Tests/README.md) - Foundation test patterns
- [Authentication Tests](../Procore.SDK.Shared.Tests/README.md) - Authentication testing
- [ProjectManagement API Documentation](../../docs/project-management-api.md) - API reference

---

**Note**: These tests define the expected behavior for the ProjectManagement client implementation that will be created as part of Task 7. They serve as both specification and validation for the wrapper client that provides a clean, intuitive API surface over the generated Kiota client for project management operations.