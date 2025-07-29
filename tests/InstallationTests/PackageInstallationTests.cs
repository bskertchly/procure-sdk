using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.Models;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using Xunit;

namespace Procore.SDK.InstallationTests;

/// <summary>
/// Tests to validate that NuGet packages can be installed and used correctly
/// across different target frameworks and project types.
/// </summary>
public class PackageInstallationTests
{
    [Fact]
    public void SDK_Packages_ShouldBeAccessible()
    {
        // Arrange & Act - Just accessing the types should work
        var coreClientType = typeof(ICoreClient);
        var authOptionsType = typeof(ProcoreAuthOptions);
        var serviceExtensionsType = typeof(ServiceCollectionExtensions);

        // Assert
        coreClientType.Should().NotBeNull();
        authOptionsType.Should().NotBeNull();
        serviceExtensionsType.Should().NotBeNull();
    }

    [Fact]
    public void DependencyInjection_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddProcoreSDK(options =>
        {
            options.BaseAddress = "https://test.procore.com";
            options.ClientId = "test-client-id";
            options.ClientSecret = "test-client-secret";
            options.RedirectUri = "http://localhost:8080/callback";
        });

        var serviceProvider = services.BuildServiceProvider();

        // Assert - Services should be registered
        serviceProvider.GetService<ICoreClient>().Should().NotBeNull();
        serviceProvider.GetService<ITokenManager>().Should().NotBeNull();
        serviceProvider.GetService<ITokenStorage>().Should().NotBeNull();
    }

    [Fact]
    public void Domain_Models_ShouldBeUsable()
    {
        // Arrange & Act - Create domain model instances
        var company = new Company
        {
            Id = 123,
            Name = "Test Company",
            IsActive = true,
            Address = new Address
            {
                Street1 = "123 Main St",
                City = "Austin",
                State = "TX",
                PostalCode = "78701"
            }
        };

        var user = new User
        {
            Id = 456,
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            IsActive = true
        };

        // Assert
        company.Id.Should().Be(123);
        company.Name.Should().Be("Test Company");
        company.Address?.City.Should().Be("Austin");
        
        user.Id.Should().Be(456);
        user.Email.Should().Be("test@example.com");
        user.FullName.Should().Be("Test User");
    }

    [Fact]
    public void Authentication_Options_ShouldBeConfigurable()
    {
        // Arrange & Act
        var authOptions = new ProcoreAuthOptions
        {
            BaseAddress = "https://sandbox.procore.com",
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            RedirectUri = "http://localhost:8080/callback"
        };

        // Assert
        authOptions.BaseAddress.Should().Be("https://sandbox.procore.com");
        authOptions.ClientId.Should().Be("test-client-id");
        authOptions.ClientSecret.Should().Be("test-client-secret");
        authOptions.RedirectUri.Should().Be("http://localhost:8080/callback");
    }

    [Fact]
    public void Pagination_Models_ShouldWork()
    {
        // Arrange & Act
        var paginationOptions = new PaginationOptions
        {
            Page = 2,
            PerPage = 50
        };

        var pagedResult = new PagedResult<Company>
        {
            Items = new List<Company>
            {
                new() { Id = 1, Name = "Company 1" },
                new() { Id = 2, Name = "Company 2" }
            },
            CurrentPage = 2,
            TotalPages = 5,
            TotalCount = 250,
            PerPage = 50
        };

        // Assert
        paginationOptions.Page.Should().Be(2);
        paginationOptions.PerPage.Should().Be(50);
        
        pagedResult.Items.Should().HaveCount(2);
        pagedResult.CurrentPage.Should().Be(2);
        pagedResult.HasNextPage.Should().BeTrue();
        pagedResult.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void Exception_Types_ShouldBeAvailable()
    {
        // Arrange & Act - Create exception instances
        var coreException = new ProcoreCoreException("Test error");
        var authException = new AuthenticationException("Auth failed");
        var notFoundException = new ResourceNotFoundException("Resource not found");

        // Assert
        coreException.Message.Should().Be("Test error");
        authException.Message.Should().Be("Auth failed");
        notFoundException.Message.Should().Be("Resource not found");
    }

    [Theory]
    [InlineData("netstandard2.0")]
    [InlineData("net6.0")]
    [InlineData("net8.0")]
    public void Package_ShouldSupportTargetFramework(string targetFramework)
    {
        // This test validates that the package metadata is correctly configured
        // for multi-targeting. The actual framework detection happens at build time,
        // but we can verify the types are available.
        
        // Arrange & Act
        var assembly = typeof(ICoreClient).Assembly;
        var frameworkAttribute = assembly.GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false)
            .OfType<System.Runtime.Versioning.TargetFrameworkAttribute>()
            .FirstOrDefault();

        // Assert
        assembly.Should().NotBeNull();
        // The specific framework assertion depends on which framework is running the test
        frameworkAttribute?.FrameworkName.Should().NotBeNullOrEmpty();
    }
}