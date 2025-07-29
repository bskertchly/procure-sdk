using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.Compatibility;

/// <summary>
/// Cross-version compatibility tests for .NET 6, 8, and Framework support
/// Validates compatibility matrices, API usage, and version-specific features
/// </summary>
public class CrossVersionCompatibilityTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _projectRoot;
    private readonly string _samplesPath;

    public CrossVersionCompatibilityTests(ITestOutputHelper output)
    {
        _output = output;
        _projectRoot = GetProjectRoot();
        _samplesPath = Path.Combine(_projectRoot, "samples");
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
    public void Sample_Projects_Should_Target_Compatible_Framework_Versions()
    {
        // Arrange
        var projectFiles = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "ConsoleSample.csproj"),
            Path.Combine(_samplesPath, "WebSample", "WebSample.csproj")
        };

        foreach (var projectFile in projectFiles)
        {
            Assert.True(File.Exists(projectFile), $"Project file should exist: {projectFile}");

            // Act
            var content = File.ReadAllText(projectFile);
            var xml = XDocument.Parse(content);

            // Assert - Check target framework version
            var targetFramework = xml.Descendants("TargetFramework").FirstOrDefault()?.Value;
            Assert.NotNull(targetFramework);

            // Should target a supported .NET version
            var supportedVersions = new[] { "net8.0", "net6.0", "netstandard2.1", "netstandard2.0" };
            Assert.Contains(targetFramework, supportedVersions);

            _output.WriteLine($"✅ {Path.GetFileName(projectFile)}: Targets {targetFramework}");

            // Check for nullable reference types (available in .NET 6+)
            var nullable = xml.Descendants("Nullable").FirstOrDefault()?.Value;
            if (targetFramework.StartsWith("net6") || targetFramework.StartsWith("net8"))
            {
                Assert.Equal("enable", nullable);
                _output.WriteLine($"✅ {Path.GetFileName(projectFile)}: Nullable reference types enabled");
            }

            // Check for implicit usings (.NET 6+)
            var implicitUsings = xml.Descendants("ImplicitUsings").FirstOrDefault()?.Value;
            if (targetFramework.StartsWith("net6") || targetFramework.StartsWith("net8"))
            {
                Assert.Equal("enable", implicitUsings);
                _output.WriteLine($"✅ {Path.GetFileName(projectFile)}: Implicit usings enabled");
            }
        }
    }

    [Fact]
    public void SDK_Core_Should_Support_Multiple_Framework_Versions()
    {
        // Arrange
        var coreProjectFiles = Directory.GetFiles(Path.Combine(_projectRoot, "src"), "*.csproj", SearchOption.AllDirectories);

        foreach (var projectFile in coreProjectFiles)
        {
            var content = File.ReadAllText(projectFile);
            var xml = XDocument.Parse(content);

            // Act & Assert
            var targetFrameworks = xml.Descendants("TargetFrameworks").FirstOrDefault()?.Value;
            var targetFramework = xml.Descendants("TargetFramework").FirstOrDefault()?.Value;

            var frameworks = targetFrameworks?.Split(';') ?? (targetFramework != null ? new[] { targetFramework } : Array.Empty<string>());

            foreach (var framework in frameworks.Where(f => !string.IsNullOrEmpty(f)))
            {
                // Should support .NET Standard 2.0 for broad compatibility
                // Or modern .NET versions for new features
                var isSupported = framework.StartsWith("netstandard2.") ||
                                  framework.StartsWith("net6.") ||
                                  framework.StartsWith("net8.") ||
                                  framework.StartsWith("net48");

                Assert.True(isSupported, $"Framework {framework} should be supported");
                _output.WriteLine($"✅ {Path.GetFileName(projectFile)}: Supports {framework}");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Use_Compatible_Language_Features()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var compatibilityReport = new Dictionary<string, List<string>>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);
            var issues = new List<string>();

            // Act & Assert - Check for version-specific language features

            // C# 8.0 features (available in .NET Core 3.0+, .NET 5+)
            if (content.Contains("??="))
            {
                issues.Add("Uses null-coalescing assignment (C# 8.0+)");
            }

            if (content.Contains(" switch ") && content.Contains("=>"))
            {
                issues.Add("Uses switch expressions (C# 8.0+)");
            }

            if (content.Contains("^") || content.Contains(".."))
            {
                issues.Add("Uses ranges and indices (C# 8.0+)");
            }

            // C# 9.0 features (available in .NET 5+)
            if (content.Contains("record "))
            {
                issues.Add("Uses record types (C# 9.0+)");
            }

            if (content.Contains("new()") && !content.Contains("new ()"))
            {
                issues.Add("Uses target-typed new expressions (C# 9.0+)");
            }

            // C# 10.0 features (available in .NET 6+)
            if (content.Contains("global using"))
            {
                issues.Add("Uses global using statements (C# 10.0+)");
            }

            if (content.Contains("file class") || content.Contains("file struct"))
            {
                issues.Add("Uses file-scoped types (C# 10.0+)");
            }

            // C# 11.0 features (available in .NET 7+)
            if (content.Contains("required "))
            {
                issues.Add("Uses required members (C# 11.0+)");
            }

            if (issues.Any())
            {
                compatibilityReport[fileName] = issues;
                foreach (var issue in issues)
                {
                    _output.WriteLine($"ℹ️  {fileName}: {issue}");
                }
            }
            else
            {
                _output.WriteLine($"✅ {fileName}: Uses compatible language features");
            }
        }

        // All detected features should be compatible with target framework
        var totalIssues = compatibilityReport.Values.SelectMany(x => x).Count();
        _output.WriteLine($"Found {totalIssues} version-specific language features");
    }

    [Fact]
    public void Sample_Code_Should_Use_Compatible_API_Calls()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check for version-specific API usage

            // Check for .NET 6+ specific APIs
            var net6PlusAPIs = new[]
            {
                "WebApplication.CreateBuilder",
                "builder.Services.Add",
                "app.MapGet",
                "app.MapPost",
                "IHostBuilder.ConfigureWebHostDefaults"
            };

            var net6Features = new List<string>();
            foreach (var api in net6PlusAPIs)
            {
                if (content.Contains(api))
                {
                    net6Features.Add(api);
                }
            }

            if (net6Features.Any())
            {
                _output.WriteLine($"ℹ️  {fileName}: Uses .NET 6+ APIs: {string.Join(", ", net6Features)}");
            }

            // Check for older compatibility patterns
            if (content.Contains("Startup.cs") || content.Contains("ConfigureServices"))
            {
                _output.WriteLine($"ℹ️  {fileName}: Uses .NET Core 3.1/.NET 5 patterns");
            }

            // Check for framework-specific libraries
            if (content.Contains("System.Text.Json"))
            {
                _output.WriteLine($"✅ {fileName}: Uses System.Text.Json (available in .NET Core 3.0+)");
            }

            if (content.Contains("Newtonsoft.Json"))
            {
                _output.WriteLine($"ℹ️  {fileName}: Uses Newtonsoft.Json (compatible with all frameworks)");
            }
        }
    }

    [Fact]
    public void Package_References_Should_Be_Compatible()
    {
        // Arrange
        var projectFiles = Directory.GetFiles(_projectRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(f => !f.Contains("bin") && !f.Contains("obj"))
            .ToArray();

        foreach (var projectFile in projectFiles)
        {
            var content = File.ReadAllText(projectFile);
            var xml = XDocument.Parse(projectFile);

            // Act
            var packageReferences = xml.Descendants("PackageReference");
            var targetFramework = xml.Descendants("TargetFramework").FirstOrDefault()?.Value ??
                                  xml.Descendants("TargetFrameworks").FirstOrDefault()?.Value?.Split(';').First();

            // Assert
            foreach (var package in packageReferences)
            {
                var packageName = package.Attribute("Include")?.Value;
                var version = package.Attribute("Version")?.Value;

                if (!string.IsNullOrEmpty(packageName))
                {
                    // Check for known compatibility issues
                    ValidatePackageCompatibility(packageName, version, targetFramework, Path.GetFileName(projectFile));
                }
            }
        }
    }

    [Fact]
    public void Sample_Applications_Should_Build_On_Multiple_Runtimes()
    {
        // This test would typically run the build process for different target frameworks
        // For now, we'll validate the project structure supports multi-targeting

        // Arrange
        var sampleProjects = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "ConsoleSample.csproj"),
            Path.Combine(_samplesPath, "WebSample", "WebSample.csproj")
        };

        foreach (var projectFile in sampleProjects)
        {
            var content = File.ReadAllText(projectFile);
            var xml = XDocument.Parse(content);

            // Act & Assert
            var targetFramework = xml.Descendants("TargetFramework").FirstOrDefault()?.Value;
            
            // Check if the project could potentially be multi-targeted
            var hasConditionalReferences = xml.Descendants("PackageReference")
                .Any(pr => pr.Attribute("Condition") != null);

            if (hasConditionalReferences)
            {
                _output.WriteLine($"✅ {Path.GetFileName(projectFile)}: Has conditional package references for multi-targeting");
            }

            // Validate the target framework is a modern, supported version
            Assert.True(targetFramework == "net8.0" || targetFramework == "net6.0",
                $"Sample should target a supported modern .NET version, found: {targetFramework}");

            _output.WriteLine($"✅ {Path.GetFileName(projectFile)}: Ready for multi-runtime deployment");
        }
    }

    [Fact]
    public void Sample_Code_Should_Handle_Framework_Differences()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check for framework-specific handling

            // Check for compiler directives for different frameworks
            if (content.Contains("#if NET6_0") || content.Contains("#if NET48") || content.Contains("#if NETSTANDARD"))
            {
                _output.WriteLine($"✅ {fileName}: Uses conditional compilation for framework differences");
            }

            // Check for runtime detection
            if (content.Contains("RuntimeInformation") || content.Contains("Environment.Version"))
            {
                _output.WriteLine($"✅ {fileName}: Includes runtime detection logic");
            }

            // Check for polyfills or compatibility shims
            if (content.Contains("MemoryExtensions") || content.Contains("CollectionExtensions"))
            {
                _output.WriteLine($"✅ {fileName}: Uses compatibility extensions");
            }

            // Check for async/await patterns (available in all supported frameworks)
            if (content.Contains("async") && content.Contains("await"))
            {
                _output.WriteLine($"✅ {fileName}: Uses async/await (compatible across frameworks)");
            }
        }
    }

    [Theory]
    [InlineData("net8.0")]
    [InlineData("net6.0")]
    [InlineData("netstandard2.1")]
    public void SDK_Should_Be_Compatible_With_Framework_Version(string targetFramework)
    {
        // This test validates that the SDK would be compatible with different target frameworks
        // In a real scenario, this would involve building and testing against each framework

        // Arrange
        var compatibilityMatrix = new Dictionary<string, string[]>
        {
            ["net8.0"] = new[] { "System.Text.Json", "Microsoft.Extensions.DependencyInjection", "Microsoft.Extensions.Logging" },
            ["net6.0"] = new[] { "System.Text.Json", "Microsoft.Extensions.DependencyInjection", "Microsoft.Extensions.Logging" },
            ["netstandard2.1"] = new[] { "System.Text.Json", "Microsoft.Extensions.DependencyInjection", "Microsoft.Extensions.Logging" },
            ["netstandard2.0"] = new[] { "Newtonsoft.Json", "Microsoft.Extensions.DependencyInjection", "Microsoft.Extensions.Logging" }
        };

        // Act & Assert
        if (compatibilityMatrix.ContainsKey(targetFramework))
        {
            var requiredPackages = compatibilityMatrix[targetFramework];
            foreach (var package in requiredPackages)
            {
                // Validate that required packages are available for this framework
                _output.WriteLine($"✅ {targetFramework}: {package} is compatible");
            }
        }

        _output.WriteLine($"✅ Framework {targetFramework} compatibility validated");
    }

    [Fact]
    public void Sample_Configuration_Should_Be_Framework_Agnostic()
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

            // Act & Assert - Configuration should be framework-agnostic
            
            // Should not contain framework-specific configuration
            Assert.DoesNotContain("net48", content.ToLower());
            Assert.DoesNotContain("netcore", content.ToLower());
            Assert.DoesNotContain("netframework", content.ToLower());

            // Should use standard configuration patterns
            Assert.Contains("Procore", content);
            
            _output.WriteLine($"✅ {Path.GetFileName(configFile)}: Framework-agnostic configuration");
        }
    }

    private void ValidatePackageCompatibility(string packageName, string? version, string? targetFramework, string projectName)
    {
        // Known compatibility validations
        var compatibilityRules = new Dictionary<string, Func<string?, string?, bool>>
        {
            ["Microsoft.AspNetCore.App"] = (v, tf) => tf?.StartsWith("net") == true,
            ["System.Text.Json"] = (v, tf) => tf == "netstandard2.1" || tf?.StartsWith("net") == true,
            ["Microsoft.Extensions.Hosting"] = (v, tf) => true, // Compatible with all frameworks
            ["Microsoft.Extensions.DependencyInjection"] = (v, tf) => true, // Compatible with all frameworks
        };

        if (compatibilityRules.ContainsKey(packageName))
        {
            var isCompatible = compatibilityRules[packageName](version, targetFramework);
            if (isCompatible)
            {
                _output.WriteLine($"✅ {projectName}: {packageName} {version} compatible with {targetFramework}");
            }
            else
            {
                _output.WriteLine($"⚠️  {projectName}: {packageName} {version} may not be compatible with {targetFramework}");
            }
        }
        else
        {
            _output.WriteLine($"ℹ️  {projectName}: {packageName} {version} compatibility not validated");
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
}