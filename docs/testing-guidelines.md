# Testing Guidelines - Procore SDK

This document outlines the testing standards, patterns, and organization for the Procore SDK .NET test suite.

## Test Organization Structure

### Test Project Naming
- **Pattern**: `Procore.SDK.{Component}.Tests`
- **Examples**: 
  - `Procore.SDK.Core.Tests`
  - `Procore.SDK.Shared.Tests`
  - `Procore.SDK.IntegrationTests.Live`

### Directory Structure
```
tests/
├── Procore.SDK.Shared.Tests/          # Shared authentication & utilities
├── Procore.SDK.Core.Tests/            # Core API client tests
├── Procore.SDK.ProjectManagement.Tests/
├── Procore.SDK.QualitySafety.Tests/
├── Procore.SDK.ConstructionFinancials.Tests/
├── Procore.SDK.FieldProductivity.Tests/
├── Procore.SDK.ResourceManagement.Tests/
├── Procore.SDK.Resilience.Tests/      # Circuit breaker & resilience
├── Procore.SDK.Generation.Tests/      # Code generation tests
├── Procore.SDK.Samples.Tests/         # Sample code tests
├── Procore.SDK.Benchmarks/            # Performance benchmarks
├── Procore.SDK.IntegrationTests.Live/ # Live API integration tests
└── Procore.SDK.Tests/                 # End-to-end tests
```

## Test Categories and Patterns

### 1. Unit Tests
**Location**: Component-specific test projects  
**Pattern**: Test individual classes and methods in isolation  
**Naming**: `{ClassName}Tests.cs`

```csharp
public class TokenManagerTests : AuthenticationTestBase
{
    [Fact]
    public async Task GetAccessTokenAsync_Should_Return_Valid_Token()
    {
        // Arrange
        var validToken = CreateValidToken();
        MockTokenStorage.GetTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                       .Returns(validToken);

        // Act
        var result = await Sut.GetAccessTokenAsync();

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("valid-access-token");
    }
}
```

### 2. Integration Tests
**Location**: `Procore.SDK.IntegrationTests.Live`  
**Pattern**: Test component interactions and external dependencies  
**Naming**: `{Component}IntegrationTests.cs`

```csharp
[Collection("Integration")]
public class AuthenticationIntegrationTests : TestBase
{
    [Fact(Skip = "Requires live API credentials")]
    public async Task OAuth_Flow_Should_Complete_Successfully()
    {
        // Integration test implementation
    }
}
```

### 3. End-to-End Tests
**Location**: `Procore.SDK.Tests`  
**Pattern**: Test complete user workflows  
**Naming**: `{Workflow}E2ETests.cs`

## Base Classes and Utilities

### TestBase Classes
All test classes should inherit from appropriate base classes:

1. **`TestBase`** - Basic test infrastructure
2. **`AuthenticationTestBase`** - Authentication-specific tests
3. **Custom base classes** - Domain-specific test infrastructure

### Test Utilities Location
**Shared utilities**: `Procore.SDK.Shared.Tests/TestUtilities/`

- `TestBase.cs` - Base test class
- `TestableHttpMessageHandler.cs` - HTTP mocking
- `TestDataBuilder.cs` - Test data creation

### Using Test Utilities

```csharp
public class MyComponentTests : TestBase
{
    [Fact]
    public async Task Should_Handle_Http_Request()
    {
        // Arrange
        HttpHandler.SetupResponse(HttpStatusCode.OK, """{"id": 123}""");
        
        // Act
        var response = await HttpClient.GetAsync("/api/test");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        HttpHandler.VerifyRequestMade(HttpMethod.Get, "/api/test").Should().BeTrue();
    }
}
```

## Test Data Management

### Test Data Builders
Use fluent builders for consistent test data creation:

```csharp
// Good: Using TestDataBuilder
var token = TestDataBuilder.AccessToken()
    .WithToken("test-token")
    .ExpiresIn(TimeSpan.FromHours(1))
    .WithScopes("read", "write")
    .Build();

// Avoid: Manual object construction
var token = new AccessToken("test-token", "Bearer", DateTimeOffset.Now.AddHours(1));
```

### Test Data Patterns
1. **Arrange Phase**: Use builders and factories
2. **Realistic Data**: Use meaningful test data that reflects real usage
3. **Edge Cases**: Include boundary conditions and error scenarios
4. **Isolation**: Each test should create its own data

## Naming Conventions

### Test Methods
**Pattern**: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

```csharp
[Fact]
public async Task GetAccessTokenAsync_WhenTokenExpired_ShouldRefreshAutomatically()

[Fact] 
public void Constructor_WithNullHttpClient_ShouldThrowArgumentNullException()

[Theory]
[InlineData("", "Code cannot be empty")]
[InlineData(null, "Code cannot be null")]
public void ValidateCode_WithInvalidInput_ShouldThrowArgumentException(string code, string reason)
```

### Test Classes
**Pattern**: `{ComponentName}Tests`

```csharp
public class TokenManagerTests
public class ProcoreAuthHandlerTests  
public class CoreClientIntegrationTests
```

## Test Organization Within Files

### Recommended Structure
```csharp
public class ComponentTests : TestBase
{
    // Test-specific setup
    private readonly IComponentUnderTest _sut;
    
    public ComponentTests()
    {
        _sut = new ComponentUnderTest();
    }

    #region Constructor Tests
    [Fact]
    public void Constructor_Tests() { }
    #endregion

    #region Method Group 1 Tests  
    [Fact]
    public void Method1_Tests() { }
    #endregion

    #region Error Handling Tests
    [Fact] 
    public void ErrorHandling_Tests() { }
    #endregion

    #region Integration Tests
    [Fact]
    public void Integration_Tests() { }
    #endregion
}
```

## Assertions and Fluent API

### Use FluentAssertions
```csharp
// Good: FluentAssertions
result.Should().NotBeNull();
result.StatusCode.Should().Be(HttpStatusCode.OK);
response.Items.Should().HaveCount(2);
exception.Should().BeOfType<ArgumentNullException>();

// Avoid: xUnit asserts
Assert.NotNull(result);
Assert.Equal(HttpStatusCode.OK, result.StatusCode);
```

### Common Assertion Patterns
```csharp
// Collections
items.Should().HaveCount(3);
items.Should().Contain(x => x.Id == 123);
items.Should().BeInAscendingOrder(x => x.Name);

// Exceptions
action.Should().ThrowAsync<ArgumentException>()
      .WithMessage("Parameter cannot be null");

// Objects
result.Should().BeEquivalentTo(expected, options => 
    options.Excluding(x => x.CreatedAt));
```

## Mocking Guidelines

### Use NSubstitute
```csharp
// Setup
var mockService = Substitute.For<IService>();
mockService.GetDataAsync(Arg.Any<int>())
          .Returns(Task.FromResult(expectedData));

// Verification
await mockService.Received(1).GetDataAsync(123);
mockService.DidNotReceive().DeleteData(Arg.Any<int>());
```

### Mocking Patterns
1. **Mock dependencies, not the system under test**
2. **Use specific argument matchers when behavior matters**
3. **Verify interactions for side effects**
4. **Reset mocks between tests when using shared instances**

## Coverage Requirements

### Minimum Thresholds
- **Line Coverage**: 80% minimum, 85% recommended
- **Branch Coverage**: 75% minimum
- **Method Coverage**: 90% minimum

### Coverage Exclusions
- Generated code (marked with `[GeneratedCode]`)
- Test assemblies
- Obsolete code
- External library integrations (adapter pattern recommended)

### Coverage Reporting
- **Local**: Run `./scripts/coverage-report.sh`
- **CI/CD**: Automated coverage reports in GitHub Actions
- **IDE**: Use coverage tools in Visual Studio/Rider

## Performance Testing

### Benchmark Tests
**Location**: `Procore.SDK.Benchmarks`  
**Framework**: BenchmarkDotNet

```csharp
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class TokenManagerBenchmarks
{
    [Benchmark]
    public async Task GetAccessToken_Performance()
    {
        // Benchmark implementation
    }
}
```

### Performance Guidelines
1. **Baseline Measurements**: Establish performance baselines
2. **Regression Detection**: Fail CI on significant performance regression
3. **Memory Usage**: Monitor memory allocations and GC pressure
4. **Async Patterns**: Test async/await performance implications

## CI/CD Integration

### GitHub Actions Workflow
- **Multiple OS Testing**: Ubuntu, Windows, macOS
- **Code Coverage**: Automated coverage collection and reporting
- **Quality Gates**: Coverage thresholds, security scanning
- **Performance Tests**: Automated benchmark execution
- **Integration Tests**: Optional integration test execution

### Local Development
```bash
# Run all tests
dotnet test

# Run with coverage
./scripts/coverage-report.sh

# Run specific test project
dotnet test tests/Procore.SDK.Core.Tests

# Run integration tests (requires credentials)
PROCORE_CLIENT_ID=xxx PROCORE_CLIENT_SECRET=yyy \
dotnet test tests/Procore.SDK.IntegrationTests.Live
```

## Best Practices Summary

### ✅ Do
- Inherit from appropriate base classes
- Use TestDataBuilder for consistent test data
- Follow AAA pattern (Arrange, Act, Assert)
- Write descriptive test names
- Use FluentAssertions for better readability
- Mock external dependencies
- Test both happy path and error scenarios
- Keep tests fast and isolated
- Use appropriate test categories and collections

### ❌ Don't
- Test implementation details
- Create brittle tests that break with refactoring
- Use hard-coded dates/times without context
- Share mutable state between tests
- Test framework code (focus on business logic)
- Write tests that depend on external services in unit tests
- Ignore failing tests
- Skip error handling scenarios
- Mix unit and integration test concerns

## Migration Guide

### Updating Existing Tests
1. **Add base class inheritance**: Extend `TestBase` or `AuthenticationTestBase`
2. **Replace HTTP mocking**: Use `TestableHttpMessageHandler`
3. **Update test data creation**: Use `TestDataBuilder`
4. **Standardize assertions**: Convert to FluentAssertions
5. **Add coverage attributes**: Ensure proper coverage collection

### Example Migration
```csharp
// Before
public class OldTests
{
    [Fact]
    public void Test_Method()
    {
        var token = new AccessToken("test", "Bearer", DateTimeOffset.Now);
        Assert.NotNull(token);
    }
}

// After  
public class NewTests : AuthenticationTestBase
{
    [Fact]
    public void Test_Method_Should_Work_Correctly()
    {
        // Arrange
        var token = TestDataBuilder.AccessToken()
                                  .WithToken("test")
                                  .Build();
        
        // Act & Assert
        token.Should().NotBeNull();
        token.Token.Should().Be("test");
    }
}
```