using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.TypeMapping;
using Xunit;
using GeneratedUser = Procore.SDK.Core.Rest.V13.Users.Item.UsersGetResponse;
using GeneratedSimpleCompany = Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.Item.InstancesGetResponse_data_workflow_manager_company;
using Procore.SDK.Core.Models;

namespace Procore.SDK.Core.Tests.TypeMapping;

/// <summary>
/// Integration tests for dependency injection scenarios with Core type mappers.
/// Validates that the Core type mapping infrastructure integrates correctly with DI containers,
/// service registration, and mapper resolution through the registry.
/// </summary>
public class TypeMappingIntegrationTests
{
    #region Service Collection Registration Tests

    [Fact]
    public void AddTypeMapping_ShouldRegisterRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTypeMapping();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        
        // Core services should be registered
        serviceProvider.GetService<ITypeMapperRegistry>().Should().NotBeNull();
        serviceProvider.GetService<TypeMapperRegistry>().Should().NotBeNull();
        
        // Registry should be singleton
        var registry1 = serviceProvider.GetService<ITypeMapperRegistry>();
        var registry2 = serviceProvider.GetService<ITypeMapperRegistry>();
        registry1.Should().BeSameAs(registry2);
    }

    [Fact]
    public void AddCoreTypeMapping_ShouldRegisterCoreMappers()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCoreTypeMapping();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        
        // Core mappers should be registered
        registry.TryGetMapper<User, GeneratedUser>(out _).Should().BeTrue();
        registry.TryGetMapper<Company, GeneratedSimpleCompany>(out _).Should().BeTrue();
        
        // Should be able to resolve mappers
        var userMapper = registry.GetMapper<User, GeneratedUser>();
        var companyMapper = registry.GetMapper<Company, GeneratedSimpleCompany>();
        
        userMapper.Should().NotBeNull();
        userMapper.Should().BeOfType<UserTypeMapper>();
        companyMapper.Should().NotBeNull();
        companyMapper.Should().BeOfType<SimpleCompanyTypeMapper>();
    }

    [Fact]
    public void AddExtendedCoreTypeMapping_ShouldRegisterAllCoreMappers()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddExtendedCoreTypeMapping();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        
        // Extended registration should include all core mappers
        registry.TryGetMapper<User, GeneratedUser>(out _).Should().BeTrue();
        registry.TryGetMapper<Company, GeneratedSimpleCompany>(out _).Should().BeTrue();
        
        // Future: When extended mappers are added, verify they're registered here
    }

    #endregion

    #region Multiple Service Providers Tests

    [Fact]
    public void MultipleServiceProviders_ShouldMaintainIndependentRegistries()
    {
        // Arrange
        var services1 = new ServiceCollection();
        var services2 = new ServiceCollection();

        services1.AddCoreTypeMapping();
        services2.AddTypeMapping(); // Only base registration

        // Act
        var provider1 = services1.BuildServiceProvider();
        var provider2 = services2.BuildServiceProvider();

        // Assert
        var registry1 = provider1.GetRequiredService<ITypeMapperRegistry>();
        var registry2 = provider2.GetRequiredService<ITypeMapperRegistry>();

        // Different service providers should have independent registries
        registry1.Should().NotBeSameAs(registry2);
        
        // Registry 1 should have Core mappers
        registry1.TryGetMapper<User, GeneratedUser>(out _).Should().BeTrue();
        
        // Registry 2 should only have base registration (no mappers)
        registry2.TryGetMapper<User, GeneratedUser>(out _).Should().BeFalse();
    }

    #endregion

    #region Registry Functionality Tests

    [Fact]
    public void Registry_GetMapper_ShouldReturnSameInstanceForSameTypes()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Act
        var mapper1 = registry.GetMapper<User, GeneratedUser>();
        var mapper2 = registry.GetMapper<User, GeneratedUser>();

        // Assert
        mapper1.Should().BeSameAs(mapper2, "Registry should return the same instance for the same types");
    }

    [Fact]
    public void Registry_GetMapper_ForUnregisteredTypes_ShouldThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTypeMapping(); // Only base registration, no specific mappers
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Act & Assert
        var action = () => registry.GetMapper<User, GeneratedUser>();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*not registered*");
    }

    [Fact]
    public void Registry_TryGetMapper_ForUnregisteredTypes_ShouldReturnFalse()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTypeMapping(); // Only base registration, no specific mappers
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Act
        var success = registry.TryGetMapper<User, GeneratedUser>(out var mapper);

        // Assert
        success.Should().BeFalse();
        mapper.Should().BeNull();
    }

    [Fact]
    public void Registry_TryGetMapper_ForRegisteredTypes_ShouldReturnTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Act
        var success = registry.TryGetMapper<User, GeneratedUser>(out var mapper);

        // Assert
        success.Should().BeTrue();
        mapper.Should().NotBeNull();
        mapper.Should().BeOfType<UserTypeMapper>();
    }

    [Fact]
    public void Registry_GetAllMappers_ShouldReturnAllRegisteredMappers()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Act
        var allMappers = registry.GetAllMappers().ToList();

        // Assert
        allMappers.Should().HaveCountGreaterOrEqualTo(2); // At least User, Company mappers
        allMappers.Should().Contain(m => m.GetType() == typeof(UserTypeMapper));
        allMappers.Should().Contain(m => m.GetType() == typeof(SimpleCompanyTypeMapper));
    }

    #endregion

    #region End-to-End Integration Tests

    [Fact]
    public void EndToEnd_UserMapping_ThroughDependencyInjection()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        var generatedUser = new GeneratedUser
        {
            Id = 12345,
            EmailAddress = "integration@test.com",
            FirstName = "Integration",
            LastName = "Test",
            IsActive = true
        };

        // Act
        var mapper = registry.GetMapper<User, GeneratedUser>();
        var user = mapper.MapToWrapper(generatedUser);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(12345);
        user.Email.Should().Be("integration@test.com");
        user.FirstName.Should().Be("Integration");
        user.LastName.Should().Be("Test");
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void EndToEnd_CompanyMapping_ThroughDependencyInjection()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "Integration Test Company"
        };

        // Act
        var mapper = registry.GetMapper<Company, GeneratedSimpleCompany>();
        var company = mapper.MapToWrapper(generatedCompany);

        // Assert
        company.Should().NotBeNull();
        company.Name.Should().Be("Integration Test Company");
        company.IsActive.Should().BeTrue();
    }

    [Fact]
    public void EndToEnd_MultipleMappers_ShouldWorkIndependently()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        var generatedUser = new GeneratedUser
        {
            Id = 1,
            EmailAddress = "test1@example.com",
            FirstName = "Test1",
            LastName = "User1"
        };

        var generatedCompany = new GeneratedSimpleCompany
        {
            Name = "Test Company 1"
        };

        // Act
        var userMapper = registry.GetMapper<User, GeneratedUser>();
        var companyMapper = registry.GetMapper<Company, GeneratedSimpleCompany>();

        var user = userMapper.MapToWrapper(generatedUser);
        var company = companyMapper.MapToWrapper(generatedCompany);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(1);
        user.Email.Should().Be("test1@example.com");

        company.Should().NotBeNull();
        company.Name.Should().Be("Test Company 1");

        // Mappers should be independent instances
        userMapper.Should().NotBeSameAs(companyMapper);
    }

    #endregion

    #region Performance Integration Tests

    [Fact]
    public void Registry_MapperResolution_ShouldBeEfficient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Act & Assert - Multiple resolutions should be fast
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        for (int i = 0; i < 1000; i++)
        {
            var mapper = registry.GetMapper<User, GeneratedUser>();
            mapper.Should().NotBeNull();
        }
        
        stopwatch.Stop();
        
        // 1000 mapper resolutions should complete quickly (well under 1 second)
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100, 
            "Mapper resolution should be efficient");
    }

    [Fact]
    public void Registry_WithDIContainer_ShouldMaintainPerformance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCoreTypeMapping();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert - DI resolution should be efficient
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        for (int i = 0; i < 100; i++)
        {
            var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
            var userMapper = registry.GetMapper<User, GeneratedUser>();
            var companyMapper = registry.GetMapper<Company, GeneratedSimpleCompany>();
            
            userMapper.Should().NotBeNull();
            companyMapper.Should().NotBeNull();
        }
        
        stopwatch.Stop();
        
        // Should complete efficiently
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(50, 
            "DI resolution should be efficient");
    }

    #endregion

    #region Error Scenarios Tests

    [Fact]
    public void Registry_DuplicateRegistration_ShouldOverwritePrevious()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTypeMapping();

        // Register the same mapper twice
        services.AddTypeMapper<User, GeneratedUser, UserTypeMapper>();
        services.AddTypeMapper<User, GeneratedUser, UserTypeMapper>();

        // Act
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();

        // Assert - Should not throw and should work normally
        registry.TryGetMapper<User, GeneratedUser>(out _).Should().BeTrue();
        var mapper = registry.GetMapper<User, GeneratedUser>();
        mapper.Should().BeOfType<UserTypeMapper>();
    }

    [Fact]
    public void ServiceProvider_Disposal_ShouldNotAffectMappers()
    {
        // Arrange
        ITypeMapper<User, GeneratedUser>? mapper;
        
        using (var serviceProvider = new ServiceCollection()
            .AddCoreTypeMapping()
            .BuildServiceProvider())
        {
            var registry = serviceProvider.GetRequiredService<ITypeMapperRegistry>();
            mapper = registry.GetMapper<User, GeneratedUser>();
        }

        // Act & Assert - Mapper should still work after service provider disposal
        var generatedUser = new GeneratedUser
        {
            Id = 123,
            EmailAddress = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        var user = mapper.MapToWrapper(generatedUser);
        user.Should().NotBeNull();
        user.Id.Should().Be(123);
    }

    #endregion
}