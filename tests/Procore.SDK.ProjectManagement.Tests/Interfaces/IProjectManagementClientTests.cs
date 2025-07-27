using FluentAssertions;
using FluentAssertions.Types;
using Procore.SDK.ProjectManagement.Tests.Models;
using System.Reflection;

namespace Procore.SDK.ProjectManagement.Tests.Interfaces;

/// <summary>
/// Tests that define the expected interface contract for the ProjectManagement client wrapper.
/// These tests serve as specifications for the wrapper API that should hide the complexity 
/// of the generated Kiota client.
/// </summary>
public class IProjectManagementClientTests
{
    private readonly Type _interfaceType = typeof(IProjectManagementClient);

    [Fact]
    public void IProjectManagementClient_Should_Inherit_From_IDisposable()
    {
        // Assert
        _interfaceType.Should().Implement<IDisposable>();
    }

    [Fact]
    public void IProjectManagementClient_Should_Have_RawClient_Property()
    {
        // Assert
        _interfaceType.Should().HaveProperty("RawClient")
            .Which.PropertyType.Should().Be<object>();
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Project_Operations()
    {
        // Assert - Core CRUD operations
        _interfaceType.Should().HaveMethod("GetProjectsAsync", new[] { typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetProjectAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateProjectAsync", new[] { typeof(int), typeof(CreateProjectRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("UpdateProjectAsync", new[] { typeof(int), typeof(int), typeof(UpdateProjectRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("DeleteProjectAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Budget_Operations()
    {
        // Assert - Budget management operations
        _interfaceType.Should().HaveMethod("GetBudgetLineItemsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetBudgetLineItemAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateBudgetChangeAsync", new[] { typeof(int), typeof(int), typeof(CreateBudgetChangeRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetBudgetChangesAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Contract_Operations()
    {
        // Assert - Contract and change order operations
        _interfaceType.Should().HaveMethod("GetCommitmentContractsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetCommitmentContractAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateChangeOrderAsync", new[] { typeof(int), typeof(int), typeof(CreateChangeOrderRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetChangeOrdersAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Workflow_Operations()
    {
        // Assert - Workflow management operations
        _interfaceType.Should().HaveMethod("GetWorkflowInstancesAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetWorkflowInstanceAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("RestartWorkflowAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("TerminateWorkflowAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Meeting_Operations()
    {
        // Assert - Meeting management operations
        _interfaceType.Should().HaveMethod("GetMeetingsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetMeetingAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateMeetingAsync", new[] { typeof(int), typeof(int), typeof(CreateMeetingRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("UpdateMeetingAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CreateMeetingRequest), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Convenience_Methods()
    {
        // Assert - Convenience methods for common operations
        _interfaceType.Should().HaveMethod("GetActiveProjectsAsync", new[] { typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetProjectByNameAsync", new[] { typeof(int), typeof(string), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetProjectBudgetTotalAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetBudgetVariancesAsync", new[] { typeof(int), typeof(int), typeof(decimal), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Should_Define_Pagination_Support()
    {
        // Assert - Pagination methods for large datasets
        _interfaceType.Should().HaveMethod("GetProjectsPagedAsync", new[] { typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetBudgetLineItemsPagedAsync", new[] { typeof(int), typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetCommitmentContractsPagedAsync", new[] { typeof(int), typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
    }

    [Fact]
    public void IProjectManagementClient_Methods_Should_Return_Appropriate_Types()
    {
        // Arrange & Act - Get all methods that return Tasks
        var asyncMethods = _interfaceType.GetMethods()
            .Where(m => m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            .ToList();

        // Assert - Check specific return types
        var getProjectsMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetProjectsAsync");
        getProjectsMethod.Should().NotBeNull();
        getProjectsMethod!.ReturnType.Should().Be(typeof(Task<IEnumerable<Project>>));

        var getProjectMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetProjectAsync");
        getProjectMethod.Should().NotBeNull();
        getProjectMethod!.ReturnType.Should().Be(typeof(Task<Project>));

        var getBudgetLineItemsMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetBudgetLineItemsAsync");
        getBudgetLineItemsMethod.Should().NotBeNull();
        getBudgetLineItemsMethod!.ReturnType.Should().Be(typeof(Task<IEnumerable<BudgetLineItem>>));

        var getProjectsPagedMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetProjectsPagedAsync");
        getProjectsPagedMethod.Should().NotBeNull();
        getProjectsPagedMethod!.ReturnType.Should().Be(typeof(Task<PagedResult<Project>>));
    }

    [Fact]
    public void IProjectManagementClient_Methods_Should_Have_CancellationToken_Parameters()
    {
        // Arrange & Act - Get all async methods
        var asyncMethods = _interfaceType.GetMethods()
            .Where(m => m.Name.EndsWith("Async"))
            .ToList();

        // Assert - All async methods should have CancellationToken parameter
        foreach (var method in asyncMethods)
        {
            var parameters = method.GetParameters();
            parameters.Should().Contain(p => p.ParameterType == typeof(CancellationToken),
                $"Method {method.Name} should have a CancellationToken parameter");
            
            // CancellationToken should be the last parameter and have a default value
            var cancellationTokenParam = parameters.LastOrDefault(p => p.ParameterType == typeof(CancellationToken));
            cancellationTokenParam.Should().NotBeNull($"Method {method.Name} should have a CancellationToken parameter");
            cancellationTokenParam!.HasDefaultValue.Should().BeTrue($"CancellationToken parameter in {method.Name} should have a default value");
        }
    }

    [Fact]
    public void IProjectManagementClient_Create_Methods_Should_Require_Request_Objects()
    {
        // Arrange & Act - Get all create methods
        var createMethods = _interfaceType.GetMethods()
            .Where(m => m.Name.StartsWith("Create") && m.Name.EndsWith("Async"))
            .ToList();

        // Assert - Create methods should have request parameters
        createMethods.Should().NotBeEmpty("There should be create methods defined");

        foreach (var method in createMethods)
        {
            var parameters = method.GetParameters();
            parameters.Should().Contain(p => p.ParameterType.Name.EndsWith("Request"),
                $"Create method {method.Name} should have a request parameter");
        }
    }

    [Fact]
    public void IProjectManagementClient_Should_Have_Consistent_Parameter_Patterns()
    {
        // Arrange & Act - Get methods that work with specific projects
        var projectSpecificMethods = _interfaceType.GetMethods()
            .Where(m => m.GetParameters().Any(p => p.Name == "projectId"))
            .ToList();

        // Assert - Methods that work with projects should also require companyId
        foreach (var method in projectSpecificMethods)
        {
            var parameters = method.GetParameters();
            parameters.Should().Contain(p => p.Name == "companyId" && p.ParameterType == typeof(int),
                $"Method {method.Name} should have a companyId parameter");
            parameters.Should().Contain(p => p.Name == "projectId" && p.ParameterType == typeof(int),
                $"Method {method.Name} should have a projectId parameter");
        }
    }

    [Fact]
    public void IProjectManagementClient_Update_Methods_Should_Allow_Partial_Updates()
    {
        // Arrange & Act - Get update methods
        var updateMethods = _interfaceType.GetMethods()
            .Where(m => m.Name.StartsWith("Update") && m.Name.EndsWith("Async"))
            .ToList();

        // Assert - Update methods should exist and have appropriate request types
        updateMethods.Should().NotBeEmpty("There should be update methods defined");

        var updateProjectMethod = updateMethods.FirstOrDefault(m => m.Name == "UpdateProjectAsync");
        updateProjectMethod.Should().NotBeNull("UpdateProjectAsync method should exist");

        var requestParameter = updateProjectMethod!.GetParameters()
            .FirstOrDefault(p => p.ParameterType == typeof(UpdateProjectRequest));
        requestParameter.Should().NotBeNull("UpdateProjectAsync should have UpdateProjectRequest parameter");
    }

    [Fact]
    public void IProjectManagementClient_Should_Support_Both_Collection_And_Paged_Results()
    {
        // Arrange - Define methods that should have both collection and paged variants
        var collectionMethods = new[]
        {
            "GetProjectsAsync",
            "GetBudgetLineItemsAsync",
            "GetCommitmentContractsAsync"
        };

        // Act & Assert - Check that both variants exist
        foreach (var methodName in collectionMethods)
        {
            // Check collection method exists
            var collectionMethod = _interfaceType.GetMethod(methodName);
            collectionMethod.Should().NotBeNull($"Collection method {methodName} should exist");

            // Check paged method exists
            var pagedMethodName = methodName.Replace("Async", "PagedAsync");
            var pagedMethod = _interfaceType.GetMethod(pagedMethodName);
            pagedMethod.Should().NotBeNull($"Paged method {pagedMethodName} should exist");

            // Verify paged method returns PagedResult<T>
            pagedMethod!.ReturnType.Should().Match(t => 
                t.IsGenericType && 
                t.GetGenericTypeDefinition() == typeof(Task<>) &&
                t.GetGenericArguments()[0].IsGenericType &&
                t.GetGenericArguments()[0].GetGenericTypeDefinition() == typeof(PagedResult<>),
                $"Paged method {pagedMethodName} should return Task<PagedResult<T>>");
        }
    }
}

/// <summary>
/// Extension methods for FluentAssertions to support method validation
/// </summary>
public static class TypeAssertionExtensions
{
    public static AndConstraint<TypeAssertions> HaveMethod(this TypeAssertions assertions, string methodName, Type[] parameterTypes)
    {
        var method = assertions.Subject.GetMethod(methodName, parameterTypes);
        method.Should().NotBeNull($"Type {assertions.Subject.Name} should have method {methodName} with specified parameter types");
        return new AndConstraint<TypeAssertions>(assertions);
    }
}