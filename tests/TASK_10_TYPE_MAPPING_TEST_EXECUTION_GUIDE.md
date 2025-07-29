# Task 10: Type Mapping Test Execution Guide

## Quick Start

This guide provides step-by-step instructions for implementing and executing the comprehensive type mapping test strategy for Task 10.

## 1. Test Infrastructure Setup

### 1.1 Required Test Base Classes

Create these foundational test classes in `/tests/Procore.SDK.Core.Tests/TypeMapping/`:

```bash
# Base test infrastructure
TypeMapperTestBase<TWrapper, TGenerated>.cs     # Enhanced base test class
DataIntegrityValidator<TWrapper, TGenerated>.cs # Deep data validation
PerformanceValidator.cs                         # Performance measurement
ConcurrencyValidator.cs                         # Thread-safety testing
MemoryValidator.cs                              # Resource monitoring
```

### 1.2 Test Data Builders

Create comprehensive test data builders in `/tests/*/TestData/`:

```bash
# Test data builders per client
ComplexUserTestDataBuilder.cs                  # Core client test data
ComplexProjectTestDataBuilder.cs              # ProjectManagement test data
ComplexInvoiceTestDataBuilder.cs               # ConstructionFinancials test data
ComplexObservationTestDataBuilder.cs          # QualitySafety test data
ComplexTimecardTestDataBuilder.cs              # FieldProductivity test data
ComplexResourceTestDataBuilder.cs              # ResourceManagement test data
```

## 2. Test Implementation Priority

### Phase 1: Core Infrastructure (Week 1)
1. **Enhanced TypeMapperTestBase** - Extend existing base class with performance, memory, and concurrency testing
2. **DataIntegrityValidator** - Implement deep comparison with circular reference detection
3. **PerformanceValidator** - Create comprehensive performance measurement framework
4. **Test Data Builders** - Build realistic test data generators for all clients

### Phase 2: Individual Mapper Tests (Week 2)
1. **Core Client Mappers** - UserTypeMapper, CompanyTypeMapper, DocumentTypeMapper
2. **ProjectManagement Mappers** - ProjectTypeMapper, CreateProjectRequestMapper
3. **Other Client Mappers** - Implement tests for remaining 4 clients

### Phase 3: Integration & Performance (Week 3)
1. **Cross-Client Integration** - Test type mapping between different clients
2. **Performance Benchmarking** - Comprehensive performance validation
3. **Concurrency Testing** - Thread-safety and concurrent access validation
4. **Memory Usage Testing** - Resource leak detection and monitoring

### Phase 4: Compliance & CI/CD (Week 4)
1. **Compliance Validation** - Automated compliance checking system
2. **CI/CD Integration** - Pipeline setup with quality gates
3. **Test Execution Guide** - Complete documentation
4. **Final Validation** - End-to-end test suite execution

## 3. Performance Requirements Implementation

### 3.1 Performance Test Template

```csharp
[Fact]
public void Mapper_SingleOperation_MeetsPerformanceTargets()
{
    // Arrange
    var mapper = CreateMapper();
    var source = CreateComplexGenerated();
    
    // Warm up (JIT compilation)
    for (int i = 0; i < 10; i++) mapper.MapToWrapper(source);
    
    // Act & Measure
    var stopwatch = Stopwatch.StartNew();
    var result = mapper.MapToWrapper(source);
    stopwatch.Stop();
    
    // Assert - Must be <1ms target, <5ms maximum
    stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(1);
    result.Should().NotBeNull();
}
```

### 3.2 Memory Usage Test Template

```csharp
[Theory]
[InlineData(100), InlineData(500), InlineData(1000)]
public void Mapper_BatchOperations_UsesMinimalMemory(int operationCount)
{
    var mapper = CreateMapper();
    var sources = CreateComplexGeneratedCollection(operationCount);
    var memoryBefore = GC.GetTotalMemory(true);
    
    // Act
    foreach (var source in sources) mapper.MapToWrapper(source);
    
    var memoryAfter = GC.GetTotalMemory(false);
    var memoryPerOperation = (memoryAfter - memoryBefore) / operationCount;
    
    // Assert - <1KB per operation
    memoryPerOperation.Should().BeLessThan(1024);
}
```

## 4. Data Integrity Testing Implementation

### 4.1 Round-Trip Validation Template

```csharp
[Fact]
public void Mapper_RoundTripConversion_PreservesAllData()
{
    var mapper = CreateMapper();
    var originalWrapper = CreateComplexWrapper();
    
    // Act - Round trip conversion
    var generated = mapper.MapToGenerated(originalWrapper);
    var roundTripWrapper = mapper.MapToWrapper(generated);
    
    // Assert - Zero data loss
    var integrityResult = IntegrityValidator.ValidateRoundTripIntegrity(
        originalWrapper, mapper);
    
    integrityResult.IsValid.Should().BeTrue(
        $"Data should be preserved. Failures: {string.Join(", ", integrityResult.Failures)}");
}
```

### 4.2 Complex Object Validation Template

```csharp
[Theory]
[MemberData(nameof(ComplexObjectTestCases))]
public void Mapper_ComplexObjects_PreserveNestedData(ComplexObjectTestCase testCase)
{
    var mapper = CreateMapper();
    var originalObject = testCase.CreateObject();
    
    // Act
    var generated = mapper.MapToGenerated(originalObject);
    var roundTrip = mapper.MapToWrapper(generated);
    
    // Assert
    var integrityResult = IntegrityValidator.ValidateBidirectionalConsistency(
        originalObject, generated, mapper);
    
    integrityResult.IsValid.Should().BeTrue(
        $"Complex object '{testCase.Description}' should preserve all nested data");
}
```

## 5. Concurrency Testing Implementation

### 5.1 Thread-Safety Test Template

```csharp
[Fact]
public void Mapper_ConcurrentAccess_IsThreadSafe()
{
    var mapper = CreateMapper();
    var testData = CreateComplexWrapperCollection(100);
    var results = new ConcurrentBag<TWrapper>();
    var exceptions = new ConcurrentBag<Exception>();
    
    var tasks = Enumerable.Range(0, 10).Select(i => Task.Run(() =>
    {
        try
        {
            foreach (var item in testData)
            {
                var generated = mapper.MapToGenerated(item);
                var result = mapper.MapToWrapper(generated);
                results.Add(result);
            }
        }
        catch (Exception ex) { exceptions.Add(ex); }
    })).ToArray();
    
    Task.WaitAll(tasks, TimeSpan.FromSeconds(30));
    
    exceptions.Should().BeEmpty("No exceptions should occur during concurrent access");
    results.Should().HaveCount(1000, "All operations should complete successfully");
}
```

## 6. Client-Specific Test Implementation

### 6.1 Core Client Test Example

```csharp
public class UserTypeMapperTests : TypeMapperTestBase<User, GeneratedUser>
{
    private readonly ComplexUserTestDataBuilder _testDataBuilder;
    
    public UserTypeMapperTests(ITestOutputHelper output) : base(output)
    {
        _testDataBuilder = new ComplexUserTestDataBuilder();
    }
    
    protected override ITypeMapper<User, GeneratedUser> CreateMapper()
        => new UserTypeMapper();
        
    protected override User CreateComplexWrapper()
        => _testDataBuilder.BuildComplexUser();
        
    protected override GeneratedUser CreateComplexGenerated()
        => new ComplexGeneratedUserBuilder().BuildComplexGenerated();
    
    // All base test methods are inherited and will execute automatically
    
    [Fact]
    public void UserMapper_WithNestedCompanyData_PreservesAllFields()
    {
        var mapper = CreateMapper();
        var userWithCompany = _testDataBuilder.BuildUserWithFullNestedData();
        
        var generated = mapper.MapToGenerated(userWithCompany);
        var roundTrip = mapper.MapToWrapper(generated);
        
        roundTrip.Company.Should().NotBeNull();
        roundTrip.Company.Address.Should().NotBeNull();
        roundTrip.Company.Address.Street1.Should().Be(userWithCompany.Company.Address.Street1);
    }
}
```

## 7. Test Execution Commands

### 7.1 Unit Tests
```bash
# Run all type mapping unit tests
dotnet test tests/**/*TypeMapping*.csproj --filter Category=Unit --logger trx

# Run specific client tests
dotnet test tests/Procore.SDK.Core.Tests/TypeMapping/ --logger trx
dotnet test tests/Procore.SDK.ProjectManagement.Tests/TypeMapping/ --logger trx
```

### 7.2 Performance Tests
```bash
# Run performance benchmarks
dotnet test tests/**/*Performance*.csproj --filter Category=Performance --logger trx

# Run with detailed performance reporting
dotnet test tests/**/*TypeMapping*.csproj --filter Category=Performance --logger "console;verbosity=detailed"
```

### 7.3 Integration Tests
```bash
# Run cross-client integration tests
dotnet test tests/**/*Integration*.csproj --filter Category=TypeMapping --logger trx

# Run full integration test suite
dotnet test tests/ --filter "Category=Integration&Category=TypeMapping" --logger trx
```

### 7.4 Compliance Validation
```bash
# Run automated compliance validation
dotnet run --project tests/TypeMappingComplianceValidator

# Generate compliance report
dotnet run --project tests/TypeMappingComplianceValidator -- --output compliance-report.html
```

## 8. Success Criteria Checklist

### 8.1 Performance Criteria ✅
- [ ] 100% of single conversions complete within 1ms target (5ms maximum)
- [ ] Batch operations maintain <1ms average across all mappers
- [ ] Complex object conversions complete within 2ms
- [ ] Memory usage <1MB per 1000 operations across all clients
- [ ] Concurrent operations maintain <2ms average under contention

### 8.2 Data Integrity Criteria ✅
- [ ] 100% data preservation in round-trip conversions
- [ ] All properties correctly mapped with proper type conversion
- [ ] Custom fields fully preserved with type information
- [ ] Enum values consistently converted with fallback handling
- [ ] DateTime conversions preserve precision and timezone data
- [ ] Collection mappings maintain order and handle null elements

### 8.3 Coverage Criteria ✅
- [ ] 95%+ unit test coverage for all type mappers
- [ ] 90%+ integration test coverage across clients
- [ ] 100% performance test coverage for all mappers
- [ ] 95%+ edge case coverage including null/empty scenarios
- [ ] 100% compliance validation coverage

### 8.4 Quality Criteria ✅
- [ ] Thread-safety validated under concurrent access
- [ ] Memory leaks prevented in sustained operations
- [ ] Error handling provides clear context and recovery
- [ ] All type mappers registered in dependency injection
- [ ] Comprehensive logging for troubleshooting

## 9. Troubleshooting Common Issues

### 9.1 Performance Issues
**Problem**: Conversions taking >1ms
**Solution**: 
1. Profile with `dotnet-trace` to identify bottlenecks
2. Check for unnecessary allocations in mapper code
3. Verify efficient enum and DateTime conversions
4. Consider caching for expensive operations

### 9.2 Data Integrity Failures
**Problem**: Round-trip validation failing
**Solution**:
1. Enable detailed logging in `DataIntegrityValidator`
2. Check for null handling in mapper implementations
3. Verify custom field preservation logic
4. Test with minimal data to isolate the issue

### 9.3 Memory Leaks
**Problem**: Memory usage growing with batch operations
**Solution**:
1. Use memory profilers (JetBrains dotMemory, PerfView)
2. Check for event handler subscriptions without unsubscription
3. Verify proper disposal of resources
4. Test with GC.Collect() to identify uncollectable objects

### 9.4 Thread Safety Issues
**Problem**: Concurrent access causing exceptions
**Solution**:
1. Verify mapper implementations are stateless
2. Check for shared mutable state in mappers
3. Use thread-safe collections where needed
4. Test with ThreadSanitizer tools

## 10. Reporting and Documentation

### 10.1 Test Reports
- **Performance Report**: `performance-reports/task10-performance.html`
- **Coverage Report**: `coverage-reports/task10-coverage.html`
- **Compliance Report**: `compliance-reports/task10-compliance.html`
- **Integration Report**: `integration-reports/task10-integration.html`

### 10.2 Metrics Dashboard
Key metrics to monitor:
- Average conversion time per mapper
- Memory usage per operation
- Test coverage percentage
- Compliance score
- Thread contention metrics

## 11. Next Steps

1. **Start with Core Infrastructure** - Implement enhanced base classes and validators
2. **Create Test Data Builders** - Build comprehensive test data generation
3. **Implement Client-Specific Tests** - Focus on most critical mappers first
4. **Add Performance Monitoring** - Implement benchmarking framework
5. **Set Up CI/CD Integration** - Automate test execution and reporting
6. **Validate Against Requirements** - Ensure all success criteria are met

## Conclusion

This execution guide provides a clear roadmap for implementing the comprehensive type mapping test strategy. Focus on building the foundational infrastructure first, then systematically implement tests for each client, ensuring all performance, integrity, and quality requirements are met.

For detailed implementation examples and advanced scenarios, refer to the main [Task 10 Comprehensive Test Strategy](./TASK_10_TYPE_MAPPING_COMPREHENSIVE_TEST_STRATEGY.md) document.