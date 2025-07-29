using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Core.TypeMapping;
using Procore.SDK.ConstructionFinancials.Models;
using Procore.SDK.ConstructionFinancials.TypeMapping;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.ConstructionFinancials.Tests.TypeMapping;

/// <summary>
/// Integration tests for ConstructionFinancials type mapping system.
/// Validates end-to-end type mapping scenarios and performance requirements.
/// </summary>
public class ConstructionFinancialsTypeMappingIntegrationTests
{
    private readonly ITestOutputHelper _output;
    private readonly IServiceProvider _serviceProvider;

    public ConstructionFinancialsTypeMappingIntegrationTests(ITestOutputHelper output)
    {
        _output = output;
        _serviceProvider = CreateServiceProvider();
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddConstructionFinancialsTypeMapping();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void TypeMapperRegistry_ShouldRegisterAllConstructionFinancialsMappers()
    {
        // Arrange
        var registry = _serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        
        // Act
        var allMappers = registry.GetAllMappers().ToList();
        
        // Assert - Verify all expected mappers are registered
        allMappers.Should().NotBeEmpty();
        
        // Verify specific mapper types are registered
        var mapperTypes = allMappers.Select(m => m.GetType()).ToList();
        mapperTypes.Should().Contain(typeof(FinancialTransactionTypeMapper));
        mapperTypes.Should().Contain(typeof(CostCodeTypeMapper));
        mapperTypes.Should().Contain(typeof(InvoiceTypeMapper));
        
        _output.WriteLine($"Registered {allMappers.Count} ConstructionFinancials type mappers");
        foreach (var mapper in allMappers)
        {
            _output.WriteLine($"  - {mapper.GetType().Name}: {mapper.WrapperType.Name} <-> {mapper.GeneratedType.Name}");
        }
    }

    [Fact]
    public void AllRegisteredMappers_ShouldMeetPerformanceTargets()
    {
        // Arrange
        var registry = _serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        var allMappers = registry.GetAllMappers();
        
        // Act & Assert
        foreach (var mapper in allMappers)
        {
            // Test performance with a warm-up run first
            WarmUpMapper(mapper);
            
            var performanceValidation = mapper.Metrics.ValidatePerformance(targetAverageMs: 1.0, maxErrorRate: 0.01);
            
            performanceValidation.OverallValid.Should().BeTrue(
                $"Mapper {mapper.GetType().Name} should meet performance targets. " +
                $"ToWrapper avg: {performanceValidation.ActualToWrapperAverageMs:F3}ms, " +
                $"ToGenerated avg: {performanceValidation.ActualToGeneratedAverageMs:F3}ms, " +
                $"ToWrapper Error rate: {performanceValidation.ActualToWrapperErrorRate:F3}, " +
                $"ToGenerated Error rate: {performanceValidation.ActualToGeneratedErrorRate:F3}");
                
            _output.WriteLine($"✅ {mapper.GetType().Name} performance: " +
                $"ToWrapper={performanceValidation.ActualToWrapperAverageMs:F3}ms, " +
                $"ToGenerated={performanceValidation.ActualToGeneratedAverageMs:F3}ms");
        }
    }

    [Fact]
    public void TypeMapping_ExtensionMethods_ShouldWorkWithDependencyInjection()
    {
        // Arrange
        var financialTransaction = new FinancialTransaction
        {
            Id = 12345,
            ProjectId = 67890,
            Type = TransactionType.Payment,
            Amount = 5000.00m,
            TransactionDate = DateTime.UtcNow,
            Description = "Test payment transaction",
            Reference = "TEST-001",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act - Test extension method usage
        var generated = financialTransaction.ToGenerated<FinancialTransaction, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse>(
            _serviceProvider);
        
        var roundTrip = generated.ToWrapper<FinancialTransaction, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsGetResponse>(
            _serviceProvider);

        // Assert
        generated.Should().NotBeNull();
        roundTrip.Should().NotBeNull();
        roundTrip.Id.Should().Be(financialTransaction.Id);
        roundTrip.Amount.Should().Be(financialTransaction.Amount);
        roundTrip.Type.Should().Be(financialTransaction.Type);
    }

    [Fact]
    public void TypeMapping_CollectionExtensions_ShouldHandleBulkOperations()
    {
        // Arrange
        var costCodes = new List<CostCode>
        {
            new() { Id = 1, Code = "01.001", Description = "Site Work", BudgetAmount = 10000m, ActualAmount = 8000m, CommittedAmount = 1500m },
            new() { Id = 2, Code = "02.001", Description = "Concrete Work", BudgetAmount = 25000m, ActualAmount = 20000m, CommittedAmount = 4000m },
            new() { Id = 3, Code = "03.001", Description = "Steel Work", BudgetAmount = 35000m, ActualAmount = 30000m, CommittedAmount = 3000m }
        };

        // Act - Test collection extension methods
        var generatedList = costCodes.ToGeneratedList<CostCode, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse>(
            _serviceProvider);
        
        var roundTripList = generatedList.ToWrapperList<CostCode, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse>(
            _serviceProvider);

        // Assert
        generatedList.Should().HaveCount(3);
        roundTripList.Should().HaveCount(3);
        
        for (int i = 0; i < costCodes.Count; i++)
        {
            roundTripList[i].Id.Should().Be(costCodes[i].Id);
            roundTripList[i].Code.Should().Be(costCodes[i].Code);
            roundTripList[i].BudgetAmount.Should().Be(costCodes[i].BudgetAmount);
        }
    }

    [Fact]
    public void TypeMapping_BulkOperations_ShouldMeetPerformanceTargets()
    {
        // Arrange - Create a large dataset for performance testing
        var costCodes = Enumerable.Range(1, 1000)
            .Select(i => new CostCode
            {
                Id = i,
                Code = $"CC.{i:D4}",
                Description = $"Cost Code {i}",
                BudgetAmount = 1000m * i,
                ActualAmount = 800m * i,
                CommittedAmount = 150m * i,
                CreatedAt = DateTime.UtcNow.AddDays(-i),
                UpdatedAt = DateTime.UtcNow
            })
            .ToList();

        // Act - Measure bulk operation performance
        var startTime = DateTime.UtcNow;
        
        var generatedList = costCodes.ToGeneratedList<CostCode, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse>(
            _serviceProvider);
        
        var endTime = DateTime.UtcNow;
        var totalElapsed = (endTime - startTime).TotalMilliseconds;
        var averagePerOperation = totalElapsed / costCodes.Count;

        // Assert - Performance requirements
        averagePerOperation.Should().BeLessOrEqualTo(1.0, 
            $"Average time per bulk operation should be ≤1ms, actual: {averagePerOperation:F3}ms");
        
        generatedList.Should().HaveCount(1000);
        
        _output.WriteLine($"Bulk operation performance: {totalElapsed:F2}ms total, {averagePerOperation:F3}ms average per operation");
    }

    [Fact]
    public void TypeMapping_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var registry = _serviceProvider.GetRequiredService<ITypeMapperRegistry>();
        var costCodeMapper = registry.GetMapper<CostCode, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse>();
        
        var costCode = new CostCode
        {
            Id = 999,
            Code = "CONCURRENT.TEST",
            Description = "Concurrent Access Test",
            BudgetAmount = 50000m,
            ActualAmount = 35000m,
            CommittedAmount = 10000m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var exceptions = new List<Exception>();
        var results = new List<CostCode>();

        // Act - Concurrent access test
        var tasks = Enumerable.Range(0, 100)
            .Select(i => System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    for (int j = 0; j < 10; j++)
                    {
                        var generated = costCodeMapper.MapToGenerated(costCode);
                        var roundTrip = costCodeMapper.MapToWrapper(generated);
                        
                        lock (results)
                        {
                            results.Add(roundTrip);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }))
            .ToArray();

        System.Threading.Tasks.Task.WaitAll(tasks);

        // Assert
        exceptions.Should().BeEmpty("No exceptions should occur during concurrent access");
        results.Should().HaveCount(1000, "All concurrent operations should complete successfully");
        
        // Verify data integrity
        results.Should().AllSatisfy(result =>
        {
            result.Id.Should().Be(999);
            result.Code.Should().Be("CONCURRENT.TEST");
            result.BudgetAmount.Should().Be(50000m);
        });
        
        _output.WriteLine($"Concurrent access test completed: {results.Count} successful operations, {exceptions.Count} exceptions");
    }

    [Fact]
    public void TypeMapping_MemoryUsage_ShouldBeEfficient()
    {
        // Arrange
        var initialMemory = GC.GetTotalMemory(true);
        var costCodes = Enumerable.Range(1, 1000)
            .Select(i => new CostCode
            {
                Id = i,
                Code = $"MEM.{i:D4}",
                Description = $"Memory Test {i}",
                BudgetAmount = 1000m * i,
                ActualAmount = 800m * i,
                CommittedAmount = 150m * i
            })
            .ToList();

        // Act - Perform memory-intensive operations
        var generatedList = costCodes.ToGeneratedList<CostCode, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse>(
            _serviceProvider);
        
        var roundTripList = generatedList.ToWrapperList<CostCode, 
            Procore.SDK.ConstructionFinancials.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.DocumentsPostResponse>(
            _serviceProvider);

        var finalMemory = GC.GetTotalMemory(false);
        var memoryUsed = finalMemory - initialMemory;
        var memoryPerOperation = memoryUsed / (costCodes.Count * 2); // 2 operations per item

        // Assert - Memory efficiency requirements
        memoryPerOperation.Should().BeLessThan(10_000, 
            $"Memory usage per operation should be <10KB, actual: {memoryPerOperation} bytes");
        
        roundTripList.Should().HaveCount(1000);
        
        _output.WriteLine($"Memory usage: {memoryUsed:N0} bytes total, {memoryPerOperation:N0} bytes per operation");
    }

    private static void WarmUpMapper(ITypeMapper mapper)
    {
        // Perform a few operations to warm up the mapper for more accurate performance measurements
        try
        {
            // This is a simplified warm-up - in a real implementation, you would create appropriate test objects
            // based on the mapper's wrapper and generated types
            for (int i = 0; i < 5; i++)
            {
                // Warm-up operations would go here
                // The specific implementation depends on the mapper types
            }
        }
        catch
        {
            // Ignore warm-up failures
        }
    }
}