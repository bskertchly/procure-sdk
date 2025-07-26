# Procore SDK Authentication Tests

This directory contains comprehensive Test-Driven Development (TDD) tests for the Procore SDK authentication infrastructure. These tests define the expected behavior of authentication components **before** implementation, following TDD principles.

## Test-Driven Development Approach

### Philosophy

These tests are written with TDD principles in mind:

1. **Red**: Tests are written first and will fail until implementation is complete
2. **Green**: Implementation should make tests pass with minimal code
3. **Refactor**: Code can be improved while maintaining test coverage

### Benefits

- **Specification by Example**: Tests serve as executable specifications
- **Design Validation**: Writing tests first validates the API design
- **Implementation Guidance**: Failing tests guide the implementation engineer
- **Regression Prevention**: Comprehensive test coverage prevents regressions
- **Documentation**: Tests document expected behavior and edge cases

## Test Structure

### Project Organization

```
tests/Procore.SDK.Shared.Tests/
├── Authentication/
│   ├── ITokenManagerTests.cs          # Interface specification tests
│   ├── TokenManagerTests.cs           # TokenManager implementation tests
│   ├── ProcoreAuthOptionsTests.cs     # Configuration class tests
│   ├── ProcoreAuthHandlerTests.cs     # HTTP message handler tests
│   ├── OAuthFlowHelperTests.cs        # OAuth flow and PKCE tests
│   ├── ITokenStorageTests.cs          # Storage interface and implementations
│   ├── AutomaticTokenRefreshTests.cs  # Integration tests for token refresh
│   └── ErrorHandlingTests.cs          # Comprehensive error scenarios
├── GlobalUsings.cs                    # Global using statements
├── Procore.SDK.Shared.Tests.csproj   # Test project file
└── README.md                          # This documentation file
```

### Test Categories

#### 1. Interface Specification Tests (`ITokenManagerTests.cs`)
- **Purpose**: Define the contract for token management interfaces
- **Scope**: Interface methods, properties, and events
- **Approach**: Reflection-based validation of interface definitions
- **Example**: Verify `ITokenManager.GetAccessTokenAsync()` signature

#### 2. Implementation Tests (`TokenManagerTests.cs`)
- **Purpose**: Test concrete implementation behavior
- **Scope**: Method implementations, state management, event handling
- **Approach**: Mock dependencies, verify interactions and state changes
- **Example**: Test automatic token refresh when token expires

#### 3. Configuration Tests (`ProcoreAuthOptionsTests.cs`)
- **Purpose**: Validate configuration class behavior
- **Scope**: Property getters/setters, default values, validation
- **Approach**: Direct property manipulation and validation
- **Example**: Verify default OAuth endpoints and margin settings

#### 4. HTTP Handler Tests (`ProcoreAuthHandlerTests.cs`)
- **Purpose**: Test HTTP message interception and modification
- **Scope**: Authorization header injection, 401 retry logic, request cloning
- **Approach**: Mock HTTP message handlers, verify request modifications
- **Example**: Test automatic token refresh on 401 Unauthorized response

#### 5. OAuth Flow Tests (`OAuthFlowHelperTests.cs`)
- **Purpose**: Test OAuth 2.0 with PKCE implementation
- **Scope**: Authorization URL generation, code exchange, PKCE validation
- **Approach**: Mock HTTP responses, validate cryptographic operations
- **Example**: Verify PKCE code challenge is correct SHA256 hash

#### 6. Storage Tests (`ITokenStorageTests.cs`)
- **Purpose**: Test token storage implementations
- **Scope**: In-memory, file-based, and encrypted storage
- **Approach**: Test data persistence, encryption, thread safety
- **Example**: Verify file storage encrypts tokens and persists across instances

#### 7. Integration Tests (`AutomaticTokenRefreshTests.cs`)
- **Purpose**: Test component integration for token refresh scenarios
- **Scope**: End-to-end token refresh workflows
- **Approach**: Mock HTTP responses, verify complete refresh flow
- **Example**: Test TokenManager and AuthHandler working together for 401 handling

#### 8. Error Handling Tests (`ErrorHandlingTests.cs`)
- **Purpose**: Test error scenarios and edge cases
- **Scope**: Network failures, invalid responses, storage errors
- **Approach**: Mock failures, verify graceful degradation
- **Example**: Test behavior when token storage is corrupted

## Test Patterns and Conventions

### Naming Conventions

```csharp
// Pattern: ComponentName_WhenCondition_ShouldExpectedBehavior
[Fact]
public async Task TokenManager_WhenTokenExpired_ShouldRefreshAutomatically()
{
    // Test implementation
}

// Pattern: InterfaceName_ShouldDefineRequiredMethod
[Fact]  
public void ITokenManager_ShouldDefineGetAccessTokenMethod()
{
    // Interface validation
}
```

### Arrangement Patterns

```csharp
// AAA Pattern: Arrange, Act, Assert
[Fact]
public async Task Example_Test()
{
    // Arrange - Set up test data and mocks
    var mockStorage = Substitute.For<ITokenStorage>();
    var token = new AccessToken("test", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
    mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
               .Returns(token);
    
    // Act - Execute the operation being tested
    var result = await tokenManager.GetAccessTokenAsync();
    
    // Assert - Verify the expected outcome
    result.Should().Be(token);
}
```

### Mock Usage Patterns

```csharp
// Setup mock behavior with NSubstitute
_mockStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
           .Returns(expectedToken);

// Verify mock interactions
_mockStorage.Received(1).StoreTokenAsync(
    "expected-key", 
    Arg.Is<AccessToken>(t => t.Token == "expected-token"), 
    Arg.Any<CancellationToken>());
```

## Test Dependencies

### Testing Frameworks
- **xUnit**: Primary testing framework for .NET
- **FluentAssertions**: Expressive assertion library
- **NSubstitute**: Mocking framework for creating test doubles with cleaner syntax

### Supporting Libraries
- **Microsoft.Extensions.Logging.Testing**: Fake logger for testing log output
- **Microsoft.Extensions.Options**: Options pattern support for configuration testing
- **Microsoft.Extensions.Http**: HTTP client factory testing support

### Key Testing Concepts

#### Mocking Strategy
- **ITokenStorage**: Mocked to test persistence operations without actual storage
- **HttpMessageHandler**: Mocked to simulate HTTP responses without network calls
- **IOptions<T>**: Mocked to provide configuration without dependency injection setup
- **ILogger**: Fake logger to verify logging behavior without output dependency

#### Test Isolation
- Each test method is independent and can run in any order
- Mock objects are reset between tests through test constructor pattern
- No shared state between test methods
- Proper disposal of resources using IDisposable pattern

#### Edge Case Coverage
- **Null/Empty Values**: Test handling of null and empty strings
- **Network Failures**: Simulate various network error conditions
- **Concurrent Operations**: Test thread safety and race conditions
- **Resource Exhaustion**: Test behavior under resource constraints
- **Invalid Data**: Test parsing and validation of malformed data

## Running the Tests

### Prerequisites
- .NET 8.0 SDK or later
- Test project dependencies restored (`dotnet restore`)

### Command Line Execution

```bash
# Run all authentication tests
dotnet test tests/Procore.SDK.Shared.Tests/

# Run tests with verbose output
dotnet test tests/Procore.SDK.Shared.Tests/ --verbosity normal

# Run specific test class
dotnet test tests/Procore.SDK.Shared.Tests/ --filter TokenManagerTests

# Run tests with code coverage
dotnet test tests/Procore.SDK.Shared.Tests/ --collect:"XPlat Code Coverage"
```

### IDE Integration
Tests are compatible with:
- Visual Studio Test Explorer
- Visual Studio Code with C# extension
- JetBrains Rider
- Any IDE supporting xUnit test discovery

## Implementation Guidance

### For Implementation Engineers

When implementing the authentication components, use these tests as your specification:

1. **Start with Interface Tests**: Ensure your interfaces match the expected signatures
2. **Implement Incrementally**: Make one failing test pass at a time
3. **Verify Test Coverage**: All tests should pass when implementation is complete
4. **Don't Modify Tests**: Tests define the requirements - modify implementation instead
5. **Add Integration Points**: Ensure components work together as tested

### Expected Test Results

#### Before Implementation
- All tests should **FAIL** with compilation errors or assertion failures
- This is expected and validates the TDD approach

#### During Implementation
- Tests should gradually **PASS** as components are implemented
- Use failing tests to guide implementation decisions

#### After Implementation
- All tests should **PASS** without modification
- High code coverage (>90%) should be achieved
- No test should be skipped or ignored

## Test-First Development Workflow

### Recommended Implementation Order

1. **AccessToken Record** (`ITokenManagerTests.cs`)
   - Implement the AccessToken record type
   - Verify constructor parameters and properties

2. **ITokenStorage Interface** (`ITokenStorageTests.cs`)
   - Implement ITokenStorage interface
   - Create InMemoryTokenStorage implementation
   - Create FileTokenStorage implementation
   - Create ProtectedDataTokenStorage implementation

3. **ProcoreAuthOptions** (`ProcoreAuthOptionsTests.cs`)
   - Implement configuration class with proper defaults
   - Ensure all properties are settable for DI binding

4. **ITokenManager Interface** (`ITokenManagerTests.cs`)
   - Implement ITokenManager interface
   - Create TokenRefreshedEventArgs class

5. **TokenManager Implementation** (`TokenManagerTests.cs`)
   - Implement TokenManager class
   - Add automatic refresh logic
   - Implement event firing

6. **OAuthFlowHelper** (`OAuthFlowHelperTests.cs`)
   - Implement PKCE code generation
   - Add authorization URL building
   - Add token exchange functionality

7. **ProcoreAuthHandler** (`ProcoreAuthHandlerTests.cs`)
   - Implement HTTP message handler
   - Add automatic token injection
   - Add 401 retry logic with token refresh

8. **Integration Verification** (`AutomaticTokenRefreshTests.cs`)
   - Verify all components work together
   - Test end-to-end token refresh scenarios

9. **Error Handling** (`ErrorHandlingTests.cs`)
   - Add comprehensive error handling
   - Ensure graceful degradation
   - Validate edge case behavior

### Validation Checkpoints

After implementing each component:

1. **Run Related Tests**: Verify component-specific tests pass  
2. **Run Integration Tests**: Ensure no regressions in dependent components
3. **Check Code Coverage**: Maintain high coverage percentage
4. **Review Error Scenarios**: Ensure error handling tests pass
5. **Validate Logging**: Check that appropriate log messages are generated

## Quality Gates

### Definition of Done

A component is considered complete when:

- ✅ All related unit tests pass
- ✅ All integration tests pass  
- ✅ All error handling tests pass
- ✅ Code coverage exceeds 90%
- ✅ No test methods are skipped or ignored
- ✅ Proper logging is implemented and tested
- ✅ Thread safety is verified where applicable
- ✅ Resource disposal is implemented correctly

### Performance Considerations

While not extensively covered in these tests, implementation should consider:

- **Token Refresh Timing**: Minimize unnecessary refresh operations
- **Concurrent Request Handling**: Use semaphores to prevent multiple simultaneous refreshes
- **Storage Efficiency**: Implement efficient serialization for file storage  
- **Memory Management**: Proper disposal of HTTP resources
- **Network Resilience**: Implement retry logic with exponential backoff

## Maintenance and Evolution

### Adding New Tests

When adding new functionality:

1. **Write Failing Tests First**: Follow TDD principles
2. **Update Related Test Classes**: Ensure comprehensive coverage
3. **Add Error Scenarios**: Include error handling tests
4. **Update Documentation**: Modify this README as needed

### Modifying Existing Tests

Tests should generally **not** be modified unless:

- Requirements have legitimately changed
- Tests contain actual errors (not implementation gaps)
- New edge cases are discovered that must be covered

### Backward Compatibility

When evolving the authentication system:

- Existing tests must continue to pass
- New tests can be added for additional functionality
- Breaking changes require careful consideration and team approval

---

This test suite provides comprehensive coverage of the Procore SDK authentication infrastructure and serves as both specification and validation for the implementation. The TDD approach ensures high-quality, well-designed components that meet all requirements.