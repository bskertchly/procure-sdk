using System;
using Procore.SDK.Core.Models;
using Procore.SDK.Core.ErrorHandling;

namespace Procore.SDK.Core.Tests;

/// <summary>
/// Simple test to verify our domain models and interfaces can be instantiated 
/// without dependency on the problematic generated Kiota client.
/// </summary>
public class SimpleCompilationTest
{
    [Fact]
    public void Can_Create_Domain_Models()
    {
        // Arrange & Act
        var company = new Company { Id = 1, Name = "Test Company" };
        var user = new User { Id = 1, Email = "test@example.com" };
        var document = new Document { Id = 1, Name = "Test Document" };
        var customField = new CustomField { Id = 1, Name = "Test Field" };
        var address = new Address { City = "Test City" };
        
        // Assert
        company.Should().NotBeNull();
        user.Should().NotBeNull();
        document.Should().NotBeNull();
        customField.Should().NotBeNull();
        address.Should().NotBeNull();
        
        company.Id.Should().Be(1);
        user.Email.Should().Be("test@example.com");
        document.Name.Should().Be("Test Document");
        customField.Name.Should().Be("Test Field");
        address.City.Should().Be("Test City");
    }
    
    [Fact]
    public void Can_Create_Request_Models()
    {
        // Arrange & Act
        var createCompanyRequest = new CreateCompanyRequest { Name = "New Company" };
        var updateCompanyRequest = new UpdateCompanyRequest { Name = "Updated Company" };
        var createUserRequest = new CreateUserRequest { Email = "new@example.com", FirstName = "John", LastName = "Doe" };
        var updateUserRequest = new UpdateUserRequest { FirstName = "Jane" };
        
        // Assert
        createCompanyRequest.Should().NotBeNull();
        updateCompanyRequest.Should().NotBeNull();
        createUserRequest.Should().NotBeNull();
        updateUserRequest.Should().NotBeNull();
        
        createCompanyRequest.Name.Should().Be("New Company");
        createUserRequest.Email.Should().Be("new@example.com");
    }
    
    [Fact]
    public void Can_Create_Pagination_Models()
    {
        // Arrange & Act
        var paginationOptions = new PaginationOptions { Page = 2, PerPage = 50 };
        var pagedResult = new PagedResult<Company>
        {
            Items = new[] { new Company { Id = 1, Name = "Test" } },
            TotalCount = 100,
            Page = 2,
            PerPage = 50,
            TotalPages = 2,
            HasNextPage = false,
            HasPreviousPage = true
        };
        
        // Assert
        paginationOptions.Should().NotBeNull();
        pagedResult.Should().NotBeNull();
        
        paginationOptions.Page.Should().Be(2);
        pagedResult.TotalCount.Should().Be(100);
        pagedResult.Items.Should().HaveCount(1);
    }
    
    [Fact]
    public void Can_Create_Exception_Models()
    {
        // Arrange & Act
        var coreException = new ProcoreCoreException("Test message", "TEST_CODE");
        var notFoundException = new ResourceNotFoundException("Company", 123);
        var invalidRequestException = new InvalidRequestException("Invalid request");
        var forbiddenException = new ForbiddenException("Access denied");
        var unauthorizedException = new UnauthorizedException("Not authenticated");
        var rateLimitException = new RateLimitExceededException(TimeSpan.FromSeconds(60));
        
        // Assert
        coreException.Should().NotBeNull();
        notFoundException.Should().NotBeNull();
        invalidRequestException.Should().NotBeNull();
        forbiddenException.Should().NotBeNull();
        unauthorizedException.Should().NotBeNull();
        rateLimitException.Should().NotBeNull();
        
        coreException.ErrorCode.Should().Be("TEST_CODE");
        notFoundException.ErrorCode.Should().Be("RESOURCE_NOT_FOUND");
        rateLimitException.RetryAfter.Should().Be(TimeSpan.FromSeconds(60));
    }
    
    [Fact]
    public void Can_Create_ErrorMapper()
    {
        // Arrange & Act
        var errorMapper = new ErrorMapper();
        
        // Assert
        errorMapper.Should().NotBeNull();
    }
    
    [Fact]
    public void ICoreClient_Interface_Exists()
    {
        // Arrange & Act
        var interfaceType = typeof(ICoreClient);
        
        // Assert
        interfaceType.Should().NotBeNull();
        interfaceType.IsInterface.Should().BeTrue();
        interfaceType.Should().Implement<IDisposable>();
    }
}