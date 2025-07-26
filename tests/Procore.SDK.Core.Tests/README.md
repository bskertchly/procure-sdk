# Procore SDK Core Client Tests

This test project provides comprehensive TDD (Test-Driven Development) tests for the Procore SDK Core Client implementation. The tests define the expected behavior and API surface for the wrapper client that provides domain-specific convenience methods over the generated Kiota client.

## 📋 Overview

The Core Client tests are organized into several categories that validate different aspects of the wrapper implementation:

- **Interface Definition Tests** - Define the expected API surface
- **Wrapper Implementation Tests** - Test the CoreClient wrapper functionality
- **Error Handling Tests** - Comprehensive error mapping and handling
- **Authentication Integration Tests** - Integration with the SDK's authentication system
- **Resource Management Tests** - Proper disposal and resource cleanup
- **Integration Tests** - End-to-end API connectivity validation

## 🏗️ Test Structure

### Project Organization

```
tests/Procore.SDK.Core.Tests/
├── Procore.SDK.Core.Tests.csproj    # Test project configuration
├── GlobalUsings.cs                   # Global using statements
├── README.md                         # This documentation
├── Models/
│   └── TestModels.cs                # Domain models and interface definitions
├── Interfaces/
│   └── ICoreClientTests.cs          # Interface contract tests
├── CoreClientTests.cs               # Main wrapper implementation tests
├── ErrorHandling/
│   └── ErrorMappingTests.cs         # HTTP error mapping tests
├── Authentication/
│   └── AuthenticationIntegrationTests.cs  # Auth system integration
├── ResourceManagement/
│   └── DisposalTests.cs             # Resource disposal and cleanup
└── Integration/
    └── EndToEndTests.cs             # Real API integration tests
```

### Test Categories

#### 1. Interface Definition Tests (`ICoreClientTests.cs`)

These tests define the expected interface contract for the CoreClient wrapper:

```csharp
[Fact]
public void ICoreClient_Should_Define_Company_Operations()
{
    // Defines expected CRUD operations for companies
    var interfaceType = typeof(ICoreClient);
    interfaceType.Should().HaveMethod("GetCompaniesAsync", new[] { typeof(CancellationToken) });
    interfaceType.Should().HaveMethod("GetCompanyAsync", new[] { typeof(int), typeof(CancellationToken) });
    // ... more method definitions
}
```

**Purpose**: Serve as specifications for the wrapper API that should hide the complexity of the generated Kiota client.

#### 2. Wrapper Implementation Tests (`CoreClientTests.cs`)

These tests validate the actual implementation of the CoreClient wrapper:

```csharp
[Fact]
public async Task GetCompaniesAsync_Should_Return_Companies()
{
    // Arrange - Setup mocks and expected data
    var expectedCompanies = new List<Company> { /* ... */ };
    _mockRequestAdapter.SendAsync(/* ... */).Returns(/* ... */);

    // Act - Call the wrapper method
    var result = await _sut.GetCompaniesAsync();

    // Assert - Verify correct behavior
    result.Should().NotBeNull();
    result.Should().HaveCount(2);
}
```

**Key Areas Tested**:
- CRUD operations for all resource types (Companies, Users, Documents, Custom Fields)
- Convenience methods (GetCurrentUser, SearchUsers, etc.)
- Pagination support
- Parameter validation
- Response mapping

#### 3. Error Handling Tests (`ErrorMappingTests.cs`)

Tests for mapping HTTP status codes to domain-specific exceptions:

```csharp
[Fact]
public void MapHttpException_Should_Map_401_To_UnauthorizedException()
{
    // Tests transformation of HTTP 401 to UnauthorizedException
    var httpException = new HttpRequestException("Unauthorized");
    httpException.Data["StatusCode"] = HttpStatusCode.Unauthorized;

    var result = _sut.MapHttpException(httpException);

    result.Should().BeOfType<UnauthorizedException>();
    result.As<UnauthorizedException>().ErrorCode.Should().Be("UNAUTHORIZED");
}
```

**Error Mappings Tested**:
- `400 Bad Request` → `InvalidRequestException`
- `401 Unauthorized` → `UnauthorizedException`  
- `403 Forbidden` → `ForbiddenException`
- `404 Not Found` → `ResourceNotFoundException`
- `422 Unprocessable Entity` → `InvalidRequestException` (with validation details)
- `429 Too Many Requests` → `RateLimitExceededException`
- `5xx Server Errors` → `ProcoreCoreException`

#### 4. Authentication Integration Tests (`AuthenticationIntegrationTests.cs`)

Tests for integration with the SDK's authentication infrastructure:

```csharp
[Fact]
public async Task CoreClient_Should_Request_Token_Before_API_Calls()
{
    // Verifies that the client properly integrates with token management
    const string expectedToken = "valid_access_token";
    _mockTokenManager.GetAccessTokenAsync(Arg.Any<CancellationToken>())
        .Returns(expectedToken);

    await _sut.GetCompanyAsync(123);

    // Verify token was requested and included in HTTP headers
    await _mockTokenManager.Received(1).GetAccessTokenAsync(Arg.Any<CancellationToken>());
    var sentRequest = _testHandler.GetSentRequests().First();
    sentRequest.Headers.Authorization?.Parameter.Should().Be(expectedToken);
}
```

**Authentication Scenarios Tested**:
- Token retrieval before API calls
- Automatic token refresh on 401 responses
- Token validation and error handling
- Concurrent request handling with shared tokens
- Cancellation token propagation

#### 5. Resource Management Tests (`DisposalTests.cs`)

Tests for proper implementation of the Dispose pattern:

```csharp
[Fact]
public void CoreClient_Should_Dispose_Underlying_Resources()
{
    // Verifies proper resource cleanup
    var disposableRequestAdapter = Substitute.For<IRequestAdapter, IDisposable>();
    var client = new ProcoreSDK.Core.CoreClient(generatedClient, _mockTokenManager, _mockLogger);

    client.Dispose();

    ((IDisposable)disposableRequestAdapter).Received(1).Dispose();
}
```

**Resource Management Scenarios**:
- IDisposable implementation
- Multiple dispose calls safety
- ObjectDisposedException after disposal
- Internal cache cleanup
- Thread-safe disposal
- Finalizer behavior

#### 6. Integration Tests (`EndToEndTests.cs`)

End-to-end tests that validate real API connectivity:

```csharp
[Fact]
[Trait("Category", "Integration")]
public async Task CoreClient_Should_Authenticate_Successfully()
{
    // Skip if no credentials
    if (_skipIntegrationTests) return;

    var currentUser = await _coreClient.GetCurrentUserAsync();

    currentUser.Should().NotBeNull();
    currentUser.Id.Should().BeGreaterThan(0);
    currentUser.Email.Should().NotBeNullOrEmpty();
}
```

**Integration Scenarios**:
- Real authentication flows
- API operation validation
- Error handling with real API responses
- Performance benchmarking
- Concurrent request handling
- Rate limiting behavior

## 🧪 Test Patterns and Conventions

### Test Naming Convention

Tests follow the pattern: `MethodName_Should_ExpectedBehavior_When_Condition`

Examples:
- `GetCompanyAsync_Should_Return_Single_Company`
- `GetCompanyAsync_Should_Throw_ResourceNotFound_When_Company_Not_Exists`
- `CoreClient_Should_Dispose_Underlying_Resources`

### Arrange-Act-Assert Pattern

All tests follow the AAA pattern:

```csharp
[Fact]
public async Task Method_Should_Behavior()
{
    // Arrange - Set up test data and mocks
    var expectedData = new TestData();
    _mockService.Setup(/* ... */);

    // Act - Execute the method under test
    var result = await _sut.MethodUnderTest();

    // Assert - Verify the expected behavior
    result.Should().NotBeNull();
    result.Should().Be(expectedData);
}
```

### Mock Framework

The tests use **NSubstitute** for mocking:

```csharp
// Creating mocks
var mock = Substitute.For<IInterface>();

// Setting up return values
mock.Method(Arg.Any<string>()).Returns("value");

// Verifying calls
mock.Received(1).Method(Arg.Any<string>());
```

### Assertion Framework

The tests use **FluentAssertions** for more readable assertions:

```csharp
// Instead of: Assert.Equal(expected, actual)
result.Should().Be(expected);

// Instead of: Assert.True(collection.Any())
collection.Should().NotBeEmpty();

// Type checking
exception.Should().BeOfType<SpecificException>();
```

## 🔧 Test Configuration

### Project Dependencies

The test project references:

- **xUnit** - Testing framework
- **NSubstitute** - Mocking framework  
- **FluentAssertions** - Assertion library
- **Microsoft.Kiota.*** - Kiota client libraries
- **Microsoft.Extensions.*** - Dependency injection and logging

### Global Usings

Common namespaces are included globally in `GlobalUsings.cs`:

```csharp
global using Xunit;
global using NSubstitute;
global using FluentAssertions;
global using Microsoft.Extensions.Logging;
global using Microsoft.Kiota.Abstractions;
global using Procore.SDK.Shared.Authentication;
// ... other common usings
```

### Test Categories

Tests are organized using xUnit traits:

- `[Trait("Category", "Integration")]` - Integration tests requiring API credentials
- `[Trait("Category", "Performance")]` - Performance benchmarking tests

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

# Run only performance tests
dotnet test --filter "Category=Performance"
```

### Test Configuration for Integration Tests

Integration tests require configuration via `appsettings.test.json` or environment variables:

```json
{
  "Procore": {
    "ClientId": "your_test_client_id",
    "ClientSecret": "your_test_client_secret",
    "BaseUrl": "https://sandbox.procore.com",
    "RedirectUri": "http://localhost:8080/callback"
  }
}
```

If credentials are not provided, integration tests will be skipped automatically.

## 📊 Expected Test Results

### Test Coverage Goals

- **Unit Tests**: 95%+ coverage of wrapper implementation
- **Integration Tests**: Cover all major API operations
- **Error Handling**: 100% coverage of error mapping scenarios

### Performance Expectations

- **API Calls**: Complete within 5 seconds
- **Authentication**: Token retrieval within 2 seconds  
- **Error Handling**: Exception mapping within 100ms

## 🎯 TDD Development Workflow

These tests serve as specifications for implementing the CoreClient wrapper:

1. **Red**: Tests fail initially because the implementation doesn't exist
2. **Green**: Implement minimal code to make tests pass
3. **Refactor**: Improve implementation while keeping tests green

### Implementation Checklist

Based on these tests, the actual CoreClient implementation should:

- ✅ Implement the `ICoreClient` interface defined in `TestModels.cs`
- ✅ Wrap the generated Kiota client with domain-specific methods
- ✅ Map HTTP errors to appropriate domain exceptions
- ✅ Integrate with the authentication infrastructure
- ✅ Implement proper resource disposal
- ✅ Support pagination and convenience methods
- ✅ Handle concurrent requests safely
- ✅ Provide meaningful error messages and logging

## 🔍 Test Debugging

### Common Issues

1. **Authentication Failures**: Ensure valid credentials are configured
2. **Network Timeouts**: Integration tests may fail with slow connections
3. **Rate Limiting**: Tests may be throttled if run too frequently

### Debugging Tips

- Use detailed logging in integration tests
- Check test output for API response details
- Verify mock setups in unit tests
- Use breakpoints in test methods to inspect state

## 📚 Related Documentation

- [Core Client Implementation Guide](../../docs/core-client-implementation.md)
- [Authentication Infrastructure](../../docs/authentication.md)
- [Error Handling Patterns](../../docs/error-handling.md)
- [SDK Architecture Overview](../../docs/architecture.md)

---

**Note**: These tests define the expected behavior for Task 4: Core Client Implementation from the main implementation plan. They serve as both specification and validation for the wrapper client that should provide a clean, intuitive API surface over the generated Kiota client.