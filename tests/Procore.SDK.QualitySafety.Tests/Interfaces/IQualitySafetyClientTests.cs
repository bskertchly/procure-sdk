using Procore.SDK.QualitySafety.Tests.Models;
using System.Reflection;

namespace Procore.SDK.QualitySafety.Tests.Interfaces;

/// <summary>
/// Tests that define the expected interface contract for the QualitySafety client wrapper.
/// These tests serve as specifications for the wrapper API that should hide the complexity 
/// of the generated Kiota client.
/// </summary>
public class IQualitySafetyClientTests
{
    private readonly Type _interfaceType = typeof(IQualitySafetyClient);

    [Fact]
    public void IQualitySafetyClient_Should_Inherit_From_IDisposable()
    {
        // Assert
        _interfaceType.Should().Implement<IDisposable>();
    }

    [Fact]
    public void IQualitySafetyClient_Should_Have_RawClient_Property()
    {
        // Assert
        _interfaceType.Should().HaveProperty("RawClient")
            .Which.PropertyType.Should().Be<object>();
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Observation_Operations()
    {
        // Assert - Core observation CRUD operations
        _interfaceType.Should().HaveMethod("GetObservationsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetObservationAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateObservationAsync", new[] { typeof(int), typeof(int), typeof(CreateObservationRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("UpdateObservationAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(UpdateObservationRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("DeleteObservationAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Inspection_Template_Operations()
    {
        // Assert - Inspection template management operations
        _interfaceType.Should().HaveMethod("GetInspectionTemplatesAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetInspectionTemplateAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateInspectionTemplateAsync", new[] { typeof(int), typeof(int), typeof(CreateInspectionTemplateRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("UpdateInspectionTemplateAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CreateInspectionTemplateRequest), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Inspection_Item_Operations()
    {
        // Assert - Inspection item operations
        _interfaceType.Should().HaveMethod("GetInspectionItemsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetInspectionItemAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("UpdateInspectionItemAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(string), typeof(InspectionItemStatus), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Safety_Incident_Operations()
    {
        // Assert - Safety incident management operations
        _interfaceType.Should().HaveMethod("GetSafetyIncidentsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetSafetyIncidentAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateSafetyIncidentAsync", new[] { typeof(int), typeof(int), typeof(CreateSafetyIncidentRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("UpdateSafetyIncidentAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(IncidentStatus), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Compliance_Operations()
    {
        // Assert - Compliance check operations
        _interfaceType.Should().HaveMethod("GetComplianceChecksAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetComplianceCheckAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CreateComplianceCheckAsync", new[] { typeof(int), typeof(int), typeof(CreateComplianceCheckRequest), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("CompleteComplianceCheckAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(ComplianceStatus), typeof(string), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Convenience_Methods()
    {
        // Assert - Convenience methods for common safety operations
        _interfaceType.Should().HaveMethod("GetOpenObservationsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetCriticalObservationsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetOverdueObservationsAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetRecentIncidentsAsync", new[] { typeof(int), typeof(int), typeof(int), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetObservationSummaryAsync", new[] { typeof(int), typeof(int), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Should_Define_Pagination_Support()
    {
        // Assert - Pagination methods for large datasets
        _interfaceType.Should().HaveMethod("GetObservationsPagedAsync", new[] { typeof(int), typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetInspectionTemplatesPagedAsync", new[] { typeof(int), typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
        _interfaceType.Should().HaveMethod("GetSafetyIncidentsPagedAsync", new[] { typeof(int), typeof(int), typeof(PaginationOptions), typeof(CancellationToken) });
    }

    [Fact]
    public void IQualitySafetyClient_Methods_Should_Return_Appropriate_Types()
    {
        // Arrange & Act - Get all methods that return Tasks
        var asyncMethods = _interfaceType.GetMethods()
            .Where(m => m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            .ToList();

        // Assert - Check specific return types
        var getObservationsMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetObservationsAsync");
        getObservationsMethod.Should().NotBeNull();
        getObservationsMethod!.ReturnType.Should().Be(typeof(Task<IEnumerable<Observation>>));

        var getObservationMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetObservationAsync");
        getObservationMethod.Should().NotBeNull();
        getObservationMethod!.ReturnType.Should().Be(typeof(Task<Observation>));

        var getObservationSummaryMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetObservationSummaryAsync");
        getObservationSummaryMethod.Should().NotBeNull();
        getObservationSummaryMethod!.ReturnType.Should().Be(typeof(Task<Dictionary<string, int>>));

        var getObservationsPagedMethod = asyncMethods.FirstOrDefault(m => m.Name == "GetObservationsPagedAsync");
        getObservationsPagedMethod.Should().NotBeNull();
        getObservationsPagedMethod!.ReturnType.Should().Be(typeof(Task<PagedResult<Observation>>));
    }

    [Fact]
    public void IQualitySafetyClient_Methods_Should_Have_CancellationToken_Parameters()
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
    public void IQualitySafetyClient_Should_Have_Consistent_Parameter_Patterns()
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
    public void IQualitySafetyClient_Should_Support_Safety_Priority_Filtering()
    {
        // Assert - Convenience methods should support priority-based filtering
        var criticalObservationsMethod = _interfaceType.GetMethod("GetCriticalObservationsAsync");
        criticalObservationsMethod.Should().NotBeNull("Should have method to get critical observations");
        criticalObservationsMethod!.ReturnType.Should().Be(typeof(Task<IEnumerable<Observation>>));

        var overdueObservationsMethod = _interfaceType.GetMethod("GetOverdueObservationsAsync");
        overdueObservationsMethod.Should().NotBeNull("Should have method to get overdue observations");
        overdueObservationsMethod!.ReturnType.Should().Be(typeof(Task<IEnumerable<Observation>>));
    }

    [Fact]
    public void IQualitySafetyClient_Should_Support_Time_Based_Queries()
    {
        // Assert - Should support time-based queries for incidents
        var recentIncidentsMethod = _interfaceType.GetMethod("GetRecentIncidentsAsync");
        recentIncidentsMethod.Should().NotBeNull("Should have method to get recent incidents");
        
        var parameters = recentIncidentsMethod!.GetParameters();
        parameters.Should().Contain(p => p.Name == "days" && p.ParameterType == typeof(int),
            "GetRecentIncidentsAsync should have a days parameter");
    }

    [Fact]
    public void IQualitySafetyClient_Should_Support_Inspection_Workflow()
    {
        // Assert - Should support complete inspection workflow
        var getTemplatesMethod = _interfaceType.GetMethod("GetInspectionTemplatesAsync");
        getTemplatesMethod.Should().NotBeNull("Should support getting inspection templates");

        var getItemsMethod = _interfaceType.GetMethod("GetInspectionItemsAsync");
        getItemsMethod.Should().NotBeNull("Should support getting inspection items");

        var updateItemMethod = _interfaceType.GetMethod("UpdateInspectionItemAsync");
        updateItemMethod.Should().NotBeNull("Should support updating inspection items");

        // Verify update method has correct parameters
        var updateParameters = updateItemMethod!.GetParameters();
        updateParameters.Should().Contain(p => p.Name == "response" && p.ParameterType == typeof(string));
        updateParameters.Should().Contain(p => p.Name == "status" && p.ParameterType == typeof(InspectionItemStatus));
    }

    [Fact]
    public void IQualitySafetyClient_Should_Support_Compliance_Workflow()
    {
        // Assert - Should support complete compliance workflow
        var createCheckMethod = _interfaceType.GetMethod("CreateComplianceCheckAsync");
        createCheckMethod.Should().NotBeNull("Should support creating compliance checks");

        var completeCheckMethod = _interfaceType.GetMethod("CompleteComplianceCheckAsync");
        completeCheckMethod.Should().NotBeNull("Should support completing compliance checks");

        // Verify complete method has correct parameters
        var completeParameters = completeCheckMethod!.GetParameters();
        completeParameters.Should().Contain(p => p.Name == "status" && p.ParameterType == typeof(ComplianceStatus));
        completeParameters.Should().Contain(p => p.Name == "notes" && p.ParameterType == typeof(string));
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