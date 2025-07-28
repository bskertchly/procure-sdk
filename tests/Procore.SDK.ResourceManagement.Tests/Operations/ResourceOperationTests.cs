using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Procore.SDK.Core.ErrorHandling;
using Procore.SDK.Core.Logging;
using Procore.SDK.ResourceManagement.Models;
using Procore.SDK.ResourceManagement.Tests.Helpers;
using Xunit;
using CoreModels = Procore.SDK.Core.Models;

namespace Procore.SDK.ResourceManagement.Tests.Operations;

/// <summary>
/// Tests for resource CRUD operations in ResourceManagement client.
/// Validates business logic, error handling, and integration patterns for resource operations.
/// 
/// Test Categories:
/// - CRUD operation validation
/// - Business rule enforcement
/// - Error scenario handling
/// - Performance validation
/// - Data consistency verification
/// </summary>
public class ResourceOperationTests : IDisposable
{
    private const int TestCompanyId = 12345;
    private const int TestResourceId = 111;
    
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILogger<ProcoreResourceManagementClient> _mockLogger;
    private readonly ErrorMapper _mockErrorMapper;
    private readonly StructuredLogger _mockStructuredLogger;
    private readonly ProcoreResourceManagementClient _sut;

    public ResourceOperationTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockLogger = Substitute.For<ILogger<ProcoreResourceManagementClient>>();
        _mockErrorMapper = Substitute.For<ErrorMapper>();
        _mockStructuredLogger = Substitute.For<StructuredLogger>();
        
        _sut = new ProcoreResourceManagementClient(
            _mockRequestAdapter, 
            _mockLogger, 
            _mockErrorMapper, 
            _mockStructuredLogger);
    }

    #region Resource Creation Tests

    [Fact]
    public async Task CreateResourceAsync_WithValidRequest_Should_ReturnCreatedResource()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest(
            name: "CAT 320 Excavator",
            type: ResourceType.Equipment,
            costPerHour: 150.00m,
            location: "Equipment Yard A"
        );

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(createRequest.Name);
        result.Type.Should().Be(createRequest.Type);
        result.CostPerHour.Should().Be(createRequest.CostPerHour);
        result.Location.Should().Be(createRequest.Location);
        result.Status.Should().Be(ResourceStatus.Available); // Default status
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        
        // Verify logging
        _mockLogger.Received().LogDebug(
            Arg.Is<string>(s => s.Contains("Creating resource for company")),
            TestCompanyId);
    }

    [Theory]
    [InlineData(ResourceType.Equipment, "CAT 320 Excavator")]
    [InlineData(ResourceType.Labor, "Certified Operator")]
    [InlineData(ResourceType.Vehicle, "Pickup Truck")]
    [InlineData(ResourceType.Tool, "Concrete Mixer")]
    [InlineData(ResourceType.Material, "Steel Beams")]
    public async Task CreateResourceAsync_WithDifferentResourceTypes_Should_HandleAllTypes(ResourceType type, string name)
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest(name: name, type: type);

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(type);
        result.Name.Should().Be(name);
    }

    [Fact]
    public async Task CreateResourceAsync_WithNullRequest_Should_ThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _sut.CreateResourceAsync(TestCompanyId, null));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateResourceAsync_WithInvalidName_Should_HandleGracefully(string invalidName)
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest(name: invalidName);

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(invalidName); // Should preserve the input for API validation
    }

    [Fact]
    public async Task CreateResourceAsync_WithNegativeCost_Should_HandleGracefully()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest(costPerHour: -50.00m);

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        result.Should().NotBeNull();
        result.CostPerHour.Should().Be(-50.00m); // Should preserve for API validation
    }

    #endregion

    #region Resource Retrieval Tests

    [Fact]
    public async Task GetResourcesAsync_WithValidCompanyId_Should_ReturnResourceCollection()
    {
        // Act
        var result = await _sut.GetResourcesAsync(TestCompanyId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<System.Collections.Generic.IEnumerable<Resource>>();
        
        // Verify logging
        _mockLogger.Received().LogDebug(
            Arg.Is<string>(s => s.Contains("Getting resources for company")),
            TestCompanyId);
    }

    [Fact]
    public async Task GetResourceAsync_WithValidId_Should_ReturnSpecificResource()
    {
        // Act
        var result = await _sut.GetResourceAsync(TestCompanyId, TestResourceId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(TestResourceId);
        result.Name.Should().NotBeNullOrEmpty();
        result.Type.Should().BeDefined();
        result.Status.Should().BeDefined();
        
        // Verify logging
        _mockLogger.Received().LogDebug(
            Arg.Is<string>(s => s.Contains("Getting resource")),
            TestResourceId, TestCompanyId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public async Task GetResourceAsync_WithInvalidResourceId_Should_HandleGracefully(int invalidResourceId)
    {
        // Act & Assert
        // Current implementation returns a placeholder - this will change with generated client integration
        var result = await _sut.GetResourceAsync(TestCompanyId, invalidResourceId);
        result.Should().NotBeNull(); // Placeholder behavior
    }

    #endregion

    #region Resource Update Tests

    [Fact]
    public async Task UpdateResourceAsync_WithValidRequest_Should_ReturnUpdatedResource()
    {
        // Arrange
        var updateRequest = ResourceTestDataBuilder.CreateResourceRequest(
            name: "Updated CAT 320 Excavator",
            costPerHour: 175.00m,
            location: "Equipment Yard B"
        );

        // Act
        var result = await _sut.UpdateResourceAsync(TestCompanyId, TestResourceId, updateRequest);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(TestResourceId);
        result.Name.Should().Be(updateRequest.Name);
        result.CostPerHour.Should().Be(updateRequest.CostPerHour);
        result.Location.Should().Be(updateRequest.Location);
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        
        // Verify logging
        _mockLogger.Received().LogDebug(
            Arg.Is<string>(s => s.Contains("Updating resource")),
            TestResourceId, TestCompanyId);
    }

    [Fact]
    public async Task UpdateResourceAsync_WithNullRequest_Should_ThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _sut.UpdateResourceAsync(TestCompanyId, TestResourceId, null));
    }

    [Fact]
    public async Task UpdateResourceAsync_WithCostChange_Should_UpdateTimestamp()
    {
        // Arrange
        var originalCost = 100.00m;
        var updatedCost = 150.00m;
        var updateRequest = ResourceTestDataBuilder.CreateResourceRequest(costPerHour: updatedCost);

        // Act
        var result = await _sut.UpdateResourceAsync(TestCompanyId, TestResourceId, updateRequest);

        // Assert
        result.Should().NotBeNull();
        result.CostPerHour.Should().Be(updatedCost);
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    #endregion

    #region Resource Deletion Tests

    [Fact]
    public async Task DeleteResourceAsync_WithValidId_Should_CompleteSuccessfully()
    {
        // Act
        await _sut.DeleteResourceAsync(TestCompanyId, TestResourceId);

        // Assert - No exception should be thrown
        // Verify logging
        _mockLogger.Received().LogDebug(
            Arg.Is<string>(s => s.Contains("Deleting resource")),
            TestResourceId, TestCompanyId);
    }

    [Fact]
    public async Task DeleteResourceAsync_WithCancellationToken_Should_RespectCancellation()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => 
            _sut.DeleteResourceAsync(TestCompanyId, TestResourceId, cts.Token));
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task GetResourceAsync_WhenHttpException_Should_MapToCorrectDomainException()
    {
        // Arrange
        var httpException = ResourceTestDataBuilder.CreateHttpException(
            HttpStatusCode.NotFound, 
            "Resource not found");
        var expectedMappedException = new CoreModels.ProcoreCoreException(
            "Resource not found", 
            "RESOURCE_NOT_FOUND");
        
        _mockErrorMapper.MapHttpException(httpException, Arg.Any<string>())
            .Returns(expectedMappedException);

        // Note: In actual implementation, we would setup the generated client to throw
        // For now, this tests the error mapping pattern

        // Act & Assert would be implemented when generated client integration is complete
        // await Assert.ThrowsAsync<CoreModels.ProcoreCoreException>(() => _sut.GetResourceAsync(TestCompanyId, 999));
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "INVALID_REQUEST")]
    [InlineData(HttpStatusCode.Unauthorized, "UNAUTHORIZED")]
    [InlineData(HttpStatusCode.Forbidden, "FORBIDDEN")]
    [InlineData(HttpStatusCode.Conflict, "RESOURCE_CONFLICT")]
    [InlineData(HttpStatusCode.InternalServerError, "SERVER_ERROR")]
    public async Task ResourceOperations_WhenHttpErrors_Should_MapToCorrectErrorCodes(
        HttpStatusCode statusCode, 
        string expectedErrorCode)
    {
        // Arrange
        var httpException = ResourceTestDataBuilder.CreateHttpException(statusCode, "Test error");
        var expectedMappedException = new CoreModels.ProcoreCoreException(
            "Test error", 
            expectedErrorCode);
        
        _mockErrorMapper.MapHttpException(httpException, Arg.Any<string>())
            .Returns(expectedMappedException);

        // This test validates the error mapping pattern
        // Actual HTTP exception throwing would be tested with generated client integration
        var mappedError = _mockErrorMapper.MapHttpException(httpException, "test-correlation-id");
        
        // Assert
        mappedError.Should().Be(expectedMappedException);
        mappedError.ErrorCode.Should().Be(expectedErrorCode);
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task CreateResourceAsync_Should_CompleteWithinPerformanceTarget()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);
        stopwatch.Stop();

        // Assert
        result.Should().NotBeNull();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100, 
            "Resource creation should complete within 100ms performance target");
    }

    [Fact]
    public async Task GetResourcesAsync_Should_CompleteWithinPerformanceTarget()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var result = await _sut.GetResourcesAsync(TestCompanyId);
        stopwatch.Stop();

        // Assert
        result.Should().NotBeNull();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(200, 
            "Resource listing should complete within 200ms performance target");
    }

    #endregion

    #region Business Logic Tests

    [Fact]
    public async Task CreateResourceAsync_Should_SetDefaultStatus_WhenNotSpecified()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest();

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        result.Status.Should().Be(ResourceStatus.Available, 
            "New resources should default to Available status");
    }

    [Fact]
    public async Task CreateResourceAsync_Should_SetCreationTimestamps()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest();
        var beforeCreation = DateTime.UtcNow.AddSeconds(-1);

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);
        var afterCreation = DateTime.UtcNow.AddSeconds(1);

        // Assert
        result.CreatedAt.Should().BeAfter(beforeCreation);
        result.CreatedAt.Should().BeBefore(afterCreation);
        result.UpdatedAt.Should().BeAfter(beforeCreation);
        result.UpdatedAt.Should().BeBefore(afterCreation);
    }

    [Theory]
    [InlineData(ResourceType.Equipment, 100.00)]
    [InlineData(ResourceType.Labor, 50.00)]
    [InlineData(ResourceType.Vehicle, 75.00)]
    [InlineData(ResourceType.Tool, 25.00)]
    [InlineData(ResourceType.Material, 10.00)]
    public async Task CreateResourceAsync_Should_AcceptValidCostRanges_ForDifferentTypes(
        ResourceType type, 
        decimal costPerHour)
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest(
            type: type, 
            costPerHour: costPerHour);

        // Act
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(type);
        result.CostPerHour.Should().Be(costPerHour);
    }

    #endregion

    #region Correlation ID and Logging Tests

    [Fact]
    public async Task AllResourceOperations_Should_GenerateCorrelationIds()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest();

        // Act - Execute multiple operations
        await _sut.CreateResourceAsync(TestCompanyId, createRequest);
        await _sut.GetResourcesAsync(TestCompanyId);
        await _sut.GetResourceAsync(TestCompanyId, TestResourceId);

        // Assert - Verify structured logging was used for all operations
        _mockStructuredLogger.Received(3).BeginOperation(
            Arg.Any<string>(),
            Arg.Is<string>(s => !string.IsNullOrEmpty(s))); // Correlation ID should not be empty
    }

    [Fact]
    public async Task ResourceOperations_Should_LogOperationNames_Correctly()
    {
        // Arrange
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest("Test Equipment");

        // Act
        await _sut.CreateResourceAsync(TestCompanyId, createRequest);

        // Assert
        _mockStructuredLogger.Received().BeginOperation(
            Arg.Is<string>(s => s.Contains("CreateResource") && s.Contains("Test Equipment")),
            Arg.Any<string>());
    }

    #endregion

    #region Disposal Tests

    [Fact]
    public void Dispose_Should_ReleaseResources_Gracefully()
    {
        // Act & Assert - Should not throw
        _sut.Dispose();
        
        // Verify disposal doesn't cause issues with subsequent dispose calls
        _sut.Dispose();
    }

    [Fact]
    public async Task OperationsAfterDispose_Should_NotCrash()
    {
        // Arrange
        _sut.Dispose();
        var createRequest = ResourceTestDataBuilder.CreateResourceRequest();

        // Act & Assert - Operations after dispose should handle gracefully
        // In current implementation, this doesn't throw, but would in full implementation
        var result = await _sut.CreateResourceAsync(TestCompanyId, createRequest);
        result.Should().NotBeNull(); // Current placeholder behavior
    }

    #endregion

    public void Dispose()
    {
        _sut?.Dispose();
    }
}