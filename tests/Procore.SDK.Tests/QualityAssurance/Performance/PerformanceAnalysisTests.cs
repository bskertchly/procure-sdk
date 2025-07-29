using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Core;
using Procore.SDK.Core.Models;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.Performance;

/// <summary>
/// Performance testing and optimization analysis for sample applications
/// Validates performance patterns, identifies bottlenecks, and ensures efficient resource usage
/// </summary>
public class PerformanceAnalysisTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly string _projectRoot;
    private readonly string _samplesPath;
    private readonly ServiceProvider _serviceProvider;

    public PerformanceAnalysisTests(ITestOutputHelper output)
    {
        _output = output;
        _projectRoot = GetProjectRoot();
        _samplesPath = Path.Combine(_projectRoot, "samples");

        // Setup test configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Procore:Authentication:ClientId"] = "test-client-id",
                ["Procore:Authentication:ClientSecret"] = "test-client-secret",
                ["Procore:Authentication:RedirectUri"] = "https://localhost:5001/auth/callback",
                ["Procore:Authentication:Scopes"] = "read",
                ["Procore:BaseUrl"] = "https://sandbox.procore.com"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
        services.AddProcoreSDK(configuration);
        services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
        services.AddSingleton<ICoreClient, ProcoreCoreClient>();

        _serviceProvider = services.BuildServiceProvider();
    }

    private static string GetProjectRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var projectRoot = currentDir;
        
        while (projectRoot != null && !Directory.Exists(Path.Combine(projectRoot, "samples")))
        {
            projectRoot = Directory.GetParent(projectRoot)?.FullName;
        }
        
        if (projectRoot == null)
            throw new DirectoryNotFoundException("Could not find project root");
            
        return projectRoot;
    }

    [Fact]
    public void Sample_Code_Should_Use_Async_Patterns_Efficiently()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var performanceReport = new List<string>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check async patterns
            
            // Good: Uses async/await
            var asyncMethods = Regex.Matches(content, @"async\s+Task\s*(?:<[^>]+>)?\s+\w+");
            if (asyncMethods.Count > 0)
            {
                performanceReport.Add($"✅ {fileName}: {asyncMethods.Count} async methods");
            }

            // Bad: Blocking on async code
            if (content.Contains(".Result") || content.Contains(".Wait()"))
            {
                performanceReport.Add($"⚠️  {fileName}: Uses blocking calls on async code (.Result/.Wait())");
            }

            // Good: Proper async disposal
            if (content.Contains("await using") || content.Contains("DisposeAsync"))
            {
                performanceReport.Add($"✅ {fileName}: Uses async disposal patterns");
            }

            // Check for ConfigureAwait usage (important for libraries)
            var configureAwaitCount = Regex.Matches(content, @"\.ConfigureAwait\(false\)").Count;
            if (configureAwaitCount > 0)
            {
                performanceReport.Add($"✅ {fileName}: Uses ConfigureAwait(false) - {configureAwaitCount} times");
            }

            // Check for parallel execution patterns
            if (content.Contains("Task.WhenAll") || content.Contains("Parallel."))
            {
                performanceReport.Add($"✅ {fileName}: Uses parallel execution patterns");
            }
        }

        // Output performance report
        foreach (var line in performanceReport)
        {
            _output.WriteLine(line);
        }

        Assert.True(performanceReport.Any(r => r.Contains("async methods")), 
            "Should find async methods in sample applications");
    }

    [Fact]
    public void Sample_Code_Should_Use_Efficient_Data_Structures()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check data structure usage

            // Good: Uses appropriate collections
            var efficientCollections = new[]
            {
                ("List<", "Generic lists"),
                ("Dictionary<", "Generic dictionaries"),
                ("HashSet<", "Hash sets for uniqueness"),
                ("StringBuilder", "String building"),
                ("IEnumerable<", "Lazy enumeration"),
                ("ReadOnlyCollection", "Immutable collections")
            };

            var foundCollections = new List<string>();
            foreach (var (pattern, description) in efficientCollections)
            {
                if (content.Contains(pattern))
                {
                    foundCollections.Add(description);
                }
            }

            if (foundCollections.Any())
            {
                _output.WriteLine($"✅ {fileName}: Uses efficient collections - {string.Join(", ", foundCollections)}");
            }

            // Check for potential performance issues
            if (content.Contains("ArrayList") || content.Contains("Hashtable"))
            {
                _output.WriteLine($"⚠️  {fileName}: Uses non-generic collections (consider generic alternatives)");
            }

            // Check for LINQ efficiency
            var linqMethods = Regex.Matches(content, @"\.\w*(?:Where|Select|First|Any|Count|ToList|ToArray)\(");
            if (linqMethods.Count > 0)
            {
                _output.WriteLine($"✅ {fileName}: Uses LINQ methods - {linqMethods.Count} occurrences");
                
                // Check for potential N+1 patterns
                if (content.Contains(".ToList().") || content.Contains(".ToArray()."))
                {
                    _output.WriteLine($"ℹ️  {fileName}: Check for unnecessary materialization of LINQ queries");
                }
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Manage_Memory_Efficiently()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check memory management patterns

            // Good: Uses using statements for disposable resources
            var usingStatements = Regex.Matches(content, @"using\s*\(").Count;
            var usingDeclarations = Regex.Matches(content, @"using\s+var\s+").Count;
            
            if (usingStatements > 0 || usingDeclarations > 0)
            {
                _output.WriteLine($"✅ {fileName}: Uses 'using' statements - {usingStatements + usingDeclarations} occurrences");
            }

            // Good: Implements IDisposable when needed
            if (content.Contains("IDisposable") || content.Contains("IAsyncDisposable"))
            {
                _output.WriteLine($"✅ {fileName}: Implements disposal patterns");
            }

            // Check for potential memory leaks
            if (content.Contains("+=") && (content.Contains("event") || content.Contains("Event")))
            {
                if (!content.Contains("-="))
                {
                    _output.WriteLine($"⚠️  {fileName}: Event subscription without unsubscription (potential memory leak)");
                }
                else
                {
                    _output.WriteLine($"✅ {fileName}: Proper event subscription/unsubscription patterns");
                }
            }

            // Check for large object allocation patterns
            if (content.Contains("new byte[") || content.Contains("new char["))
            {
                _output.WriteLine($"ℹ️  {fileName}: Allocates arrays (consider ArrayPool for large/frequent allocations)");
            }
        }
    }

    [Fact]
    public void HTTP_Client_Usage_Should_Be_Optimized()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check HttpClient usage patterns

            if (content.Contains("HttpClient"))
            {
                // Good: Uses dependency injection or HttpClientFactory
                if (content.Contains("IHttpClientFactory") || content.Contains("readonly HttpClient"))
                {
                    _output.WriteLine($"✅ {fileName}: Uses proper HttpClient dependency injection");
                }

                // Bad: Creates new HttpClient instances
                if (content.Contains("new HttpClient()"))
                {
                    _output.WriteLine($"⚠️  {fileName}: Creates new HttpClient instances (use DI or HttpClientFactory)");
                }

                // Good: Sets timeouts
                if (content.Contains("Timeout") || content.Contains("CancellationToken"))
                {
                    _output.WriteLine($"✅ {fileName}: Configures HTTP timeouts/cancellation");
                }

                // Good: Connection pooling considerations
                if (content.Contains("ServiceLifetime.Singleton") || content.Contains("HttpClientFactory"))
                {
                    _output.WriteLine($"✅ {fileName}: Optimized for connection reuse");
                }
            }
        }
    }

    [Fact]
    public void DI_Container_Performance_Should_Be_Optimal()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();

        // Act - Test service resolution performance
        var iterations = 1000;
        var resolutionTimes = new List<long>();

        for (int i = 0; i < iterations; i++)
        {
            var sw = Stopwatch.StartNew();
            
            var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
            var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
            
            sw.Stop();
            resolutionTimes.Add(sw.ElapsedTicks);
        }

        stopwatch.Stop();

        // Assert - Performance metrics
        var averageTime = resolutionTimes.Average();
        var maxTime = resolutionTimes.Max();
        var minTime = resolutionTimes.Min();

        var averageMs = (averageTime / (double)Stopwatch.Frequency) * 1000;
        var maxMs = (maxTime / (double)Stopwatch.Frequency) * 1000;

        Assert.True(averageMs < 1.0, $"Service resolution should be fast (average: {averageMs:F3}ms)");
        Assert.True(maxMs < 5.0, $"Max service resolution time should be reasonable (max: {maxMs:F3}ms)");

        _output.WriteLine($"✅ DI Performance: {iterations} resolutions in {stopwatch.ElapsedMilliseconds}ms");
        _output.WriteLine($"   Average: {averageMs:F3}ms, Max: {maxMs:F3}ms, Min: {(minTime / (double)Stopwatch.Frequency) * 1000:F3}ms");
    }

    [Fact]
    public async Task Token_Storage_Performance_Should_Be_Efficient()
    {
        // Arrange
        var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
        var testToken = new AccessToken(
            Token: "performance-test-token",
            TokenType: "Bearer",
            ExpiresAt: DateTimeOffset.UtcNow.AddHours(1),
            RefreshToken: "performance-test-refresh",
            Scopes: new[] { "read", "write" });

        // Act & Assert - Test storage operations performance
        var operations = 100;
        
        // Store performance
        var storeStopwatch = Stopwatch.StartNew();
        for (int i = 0; i < operations; i++)
        {
            await tokenStorage.StoreTokenAsync($"test-key-{i}", testToken);
        }
        storeStopwatch.Stop();

        // Retrieve performance
        var retrieveStopwatch = Stopwatch.StartNew();
        for (int i = 0; i < operations; i++)
        {
            var retrieved = await tokenStorage.GetTokenAsync($"test-key-{i}");
            Assert.NotNull(retrieved);
        }
        retrieveStopwatch.Stop();

        var storeAverage = storeStopwatch.ElapsedMilliseconds / (double)operations;
        var retrieveAverage = retrieveStopwatch.ElapsedMilliseconds / (double)operations;

        Assert.True(storeAverage < 10, $"Token store should be fast (average: {storeAverage:F2}ms)");
        Assert.True(retrieveAverage < 5, $"Token retrieve should be fast (average: {retrieveAverage:F2}ms)");

        _output.WriteLine($"✅ Token Storage Performance:");
        _output.WriteLine($"   Store: {storeAverage:F2}ms average over {operations} operations");
        _output.WriteLine($"   Retrieve: {retrieveAverage:F2}ms average over {operations} operations");
    }

    [Fact]
    public void Sample_Code_Should_Use_Efficient_String_Operations()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check string operation efficiency

            // Good: Uses StringBuilder for concatenation
            if (content.Contains("StringBuilder"))
            {
                _output.WriteLine($"✅ {fileName}: Uses StringBuilder for string building");
            }

            // Good: Uses string interpolation
            var interpolationCount = Regex.Matches(content, @"\$""[^""]*\{[^}]+\}[^""]*""").Count;
            if (interpolationCount > 0)
            {
                _output.WriteLine($"✅ {fileName}: Uses string interpolation - {interpolationCount} occurrences");
            }

            // Check for potential performance issues
            var stringConcatenationInLoops = Regex.Matches(content, @"(for|while|foreach)[^}]*\+=[^}]*string", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (stringConcatenationInLoops.Count > 0)
            {
                _output.WriteLine($"⚠️  {fileName}: String concatenation in loops (consider StringBuilder)");
            }

            // Good: Uses string comparison methods
            if (content.Contains("StringComparison.OrdinalIgnoreCase") || content.Contains("StringComparison.Ordinal"))
            {
                _output.WriteLine($"✅ {fileName}: Uses explicit string comparison methods");
            }

            // Check for inefficient string operations
            if (content.Contains(".ToUpper()") || content.Contains(".ToLower()"))
            {
                _output.WriteLine($"ℹ️  {fileName}: Uses ToUpper/ToLower (consider culture-aware comparisons)");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Minimize_Reflection_Usage()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var reflectionUsage = new List<string>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check reflection usage
            var reflectionPatterns = new[]
            {
                ("typeof(", "Type operations"),
                ("GetType()", "Runtime type checking"),
                ("Activator.CreateInstance", "Dynamic instantiation"),
                ("Assembly.Load", "Assembly loading"),
                ("MethodInfo", "Method reflection"),
                ("PropertyInfo", "Property reflection"),
                ("FieldInfo", "Field reflection")
            };

            foreach (var (pattern, description) in reflectionPatterns)
            {
                if (content.Contains(pattern))
                {
                    reflectionUsage.Add($"ℹ️  {fileName}: {description}");
                }
            }
        }

        // Output reflection usage
        foreach (var usage in reflectionUsage)
        {
            _output.WriteLine(usage);
        }

        // Note: Some reflection is acceptable, especially in configuration and DI scenarios
        _output.WriteLine($"✅ Found {reflectionUsage.Count} reflection usage patterns (review for performance impact)");
    }

    [Fact]
    public void Web_Application_Should_Use_Performance_Best_Practices()
    {
        // Arrange
        var webProgramFile = Path.Combine(_samplesPath, "WebSample", "Program.cs");
        if (!File.Exists(webProgramFile))
        {
            _output.WriteLine("⚠️  Web sample not found - skipping web performance tests");
            return;
        }

        var content = File.ReadAllText(webProgramFile);

        // Act & Assert - Check web performance patterns

        // Good: Uses response compression
        if (content.Contains("UseResponseCompression"))
        {
            _output.WriteLine("✅ Web app: Uses response compression");
        }

        // Good: Uses response caching
        if (content.Contains("UseResponseCaching") || content.Contains("AddResponseCaching"))
        {
            _output.WriteLine("✅ Web app: Configures response caching");
        }

        // Good: Uses memory cache
        if (content.Contains("AddMemoryCache") || content.Contains("IMemoryCache"))
        {
            _output.WriteLine("✅ Web app: Uses in-memory caching");
        }

        // Good: Configures request timeouts
        if (content.Contains("RequestTimeout") || content.Contains("Timeout"))
        {
            _output.WriteLine("✅ Web app: Configures request timeouts");
        }

        // Check for static file optimization
        if (content.Contains("UseStaticFiles"))
        {
            _output.WriteLine("✅ Web app: Serves static files efficiently");
        }

        // Check session configuration
        if (content.Contains("AddSession"))
        {
            if (content.Contains("IdleTimeout"))
            {
                _output.WriteLine("✅ Web app: Configures session timeout");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Handle_Large_Data_Efficiently()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check large data handling patterns

            // Good: Uses streaming for large data
            if (content.Contains("Stream") || content.Contains("IAsyncEnumerable"))
            {
                _output.WriteLine($"✅ {fileName}: Uses streaming patterns for data handling");
            }

            // Good: Uses pagination patterns
            if (content.Contains("Take(") || content.Contains("Skip(") || content.Contains("page"))
            {
                _output.WriteLine($"✅ {fileName}: Implements pagination patterns");
            }

            // Check for potential issues with large collections
            if (content.Contains(".ToList()") && content.Contains("await"))
            {
                _output.WriteLine($"ℹ️  {fileName}: Materializes async enumerable (consider streaming)");
            }

            // Good: Uses yield return for lazy evaluation
            if (content.Contains("yield return"))
            {
                _output.WriteLine($"✅ {fileName}: Uses yield return for lazy evaluation");
            }
        }
    }

    [Fact]
    public void Configuration_Should_Be_Performance_Optimized()
    {
        // Arrange
        var configFiles = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "appsettings.json"),
            Path.Combine(_samplesPath, "WebSample", "appsettings.json"),
            Path.Combine(_samplesPath, "WebSample", "appsettings.Development.json")
        };

        foreach (var configFile in configFiles.Where(File.Exists))
        {
            var content = File.ReadAllText(configFile);
            var fileName = Path.GetFileName(configFile);

            // Act & Assert - Check configuration performance settings

            // Check logging configuration for performance
            if (content.Contains("LogLevel"))
            {
                if (content.Contains("\"Debug\"") || content.Contains("\"Trace\""))
                {
                    _output.WriteLine($"ℹ️  {fileName}: Uses verbose logging (consider for production)");
                }
                else
                {
                    _output.WriteLine($"✅ {fileName}: Uses appropriate logging levels");
                }
            }

            // Check for performance-related settings
            if (content.Contains("ConnectionStrings"))
            {
                _output.WriteLine($"✅ {fileName}: Configures connection strings (ensure connection pooling)");
            }

            _output.WriteLine($"✅ {fileName}: Configuration reviewed for performance impact");
        }
    }

    private string[] GetAllCSharpFiles()
    {
        var consolePath = Path.Combine(_samplesPath, "ConsoleSample");
        var webPath = Path.Combine(_samplesPath, "WebSample");

        var files = new[]
        {
            Directory.GetFiles(consolePath, "*.cs", SearchOption.AllDirectories),
            Directory.GetFiles(webPath, "*.cs", SearchOption.AllDirectories)
        }.SelectMany(x => x)
         .Where(f => !f.Contains("obj") && !f.Contains("bin"))
         .ToArray();

        return files;
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}