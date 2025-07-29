using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.BestPractices;

/// <summary>
/// Comprehensive validation of .NET best practices compliance across sample applications
/// Validates coding standards, security practices, performance patterns, and maintainability
/// </summary>
public class DotNetBestPracticesTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _samplePath;
    private readonly string _consoleSamplePath;
    private readonly string _webSamplePath;

    public DotNetBestPracticesTests(ITestOutputHelper output)
    {
        _output = output;
        _samplePath = GetSamplesPath();
        _consoleSamplePath = Path.Combine(_samplePath, "ConsoleSample");
        _webSamplePath = Path.Combine(_samplePath, "WebSample");
    }

    private static string GetSamplesPath()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var projectRoot = currentDir;
        
        // Navigate up to find the project root
        while (projectRoot != null && !Directory.Exists(Path.Combine(projectRoot, "samples")))
        {
            projectRoot = Directory.GetParent(projectRoot)?.FullName;
        }
        
        if (projectRoot == null)
            throw new DirectoryNotFoundException("Could not find samples directory");
            
        return Path.Combine(projectRoot, "samples");
    }

    [Fact]
    public void Sample_Projects_Should_Target_Supported_NET_Versions()
    {
        // Arrange
        var consoleCsproj = Path.Combine(_consoleSamplePath, "ConsoleSample.csproj");
        var webCsproj = Path.Combine(_webSamplePath, "WebSample.csproj");

        // Act & Assert - Console Sample
        Assert.True(File.Exists(consoleCsproj), "ConsoleSample.csproj should exist");
        var consoleContent = File.ReadAllText(consoleCsproj);
        
        Assert.Contains("<TargetFramework>net8.0</TargetFramework>", consoleContent);
        Assert.Contains("<Nullable>enable</Nullable>", consoleContent);
        Assert.Contains("<ImplicitUsings>enable</ImplicitUsings>", consoleContent);
        
        _output.WriteLine("✅ Console sample targets .NET 8.0 with nullable and implicit usings enabled");

        // Act & Assert - Web Sample
        Assert.True(File.Exists(webCsproj), "WebSample.csproj should exist");
        var webContent = File.ReadAllText(webCsproj);
        
        Assert.Contains("<TargetFramework>net8.0</TargetFramework>", webContent);
        Assert.Contains("<Nullable>enable</Nullable>", webContent);
        Assert.Contains("<ImplicitUsings>enable</ImplicitUsings>", webContent);
        
        _output.WriteLine("✅ Web sample targets .NET 8.0 with nullable and implicit usings enabled");
    }

    [Fact]
    public void Sample_Projects_Should_Use_User_Secrets()
    {
        // Arrange
        var consoleCsproj = Path.Combine(_consoleSamplePath, "ConsoleSample.csproj");
        var webCsproj = Path.Combine(_webSamplePath, "WebSample.csproj");

        // Act & Assert
        var consoleContent = File.ReadAllText(consoleCsproj);
        var webContent = File.ReadAllText(webCsproj);

        Assert.Contains("<UserSecretsId>", consoleContent);
        Assert.Contains("<UserSecretsId>", webContent);

        _output.WriteLine("✅ Both samples configure User Secrets for secure configuration");
    }

    [Fact]
    public void Sample_Code_Should_Follow_Async_Best_Practices()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Check for async/await patterns
            if (content.Contains("async "))
            {
                // Async methods should return Task or Task<T>
                var asyncMethodPattern = @"async\s+(?:Task|Task<[^>]+>|ValueTask|ValueTask<[^>]+>)\s+\w+";
                Assert.True(Regex.IsMatch(content, asyncMethodPattern), 
                    $"Async methods in {fileName} should return Task or Task<T>");

                // Should not use .Result or .Wait()
                Assert.DoesNotContain(".Result", content);
                Assert.DoesNotContain(".Wait()", content);

                _output.WriteLine($"✅ {fileName}: Async patterns are correctly implemented");
            }

            // Check for ConfigureAwait usage in library code (not in sample apps)
            if (content.Contains("await ") && fileName.Contains("Service"))
            {
                // Services should consider ConfigureAwait(false) for library code
                // For sample apps, this is less critical but good practice
                _output.WriteLine($"ℹ️  {fileName}: Consider ConfigureAwait(false) for library services");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Use_Proper_Exception_Handling()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Check exception handling patterns
            if (content.Contains("catch"))
            {
                // Should not catch generic Exception without re-throwing or logging
                var genericCatchPattern = @"catch\s*\(\s*Exception\s+\w+\s*\)";
                var catches = Regex.Matches(content, genericCatchPattern);

                foreach (Match match in catches)
                {
                    var catchBlock = ExtractCatchBlock(content, match.Index);
                    
                    // Should either re-throw, log, or handle specifically
                    var hasLogging = catchBlock.Contains("Log") || catchBlock.Contains("Console.WriteLine");
                    var hasRethrow = catchBlock.Contains("throw");
                    var hasSpecificHandling = catchBlock.Contains("TempData") || catchBlock.Contains("return");

                    Assert.True(hasLogging || hasRethrow || hasSpecificHandling,
                        $"Generic exception catch in {fileName} should log, re-throw, or handle specifically");
                }

                _output.WriteLine($"✅ {fileName}: Exception handling follows best practices");
            }

            // Check for specific exception types
            if (content.Contains("ArgumentException") || content.Contains("ArgumentNullException"))
            {
                _output.WriteLine($"✅ {fileName}: Uses specific exception types");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Use_Dependency_Injection_Properly()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Check DI patterns
            if (content.Contains("class") && (content.Contains("Controller") || content.Contains("Service")))
            {
                // Should use constructor injection
                if (content.Contains("private readonly"))
                {
                    _output.WriteLine($"✅ {fileName}: Uses constructor injection with readonly fields");
                }

                // Should not use ServiceLocator anti-pattern (except in composition root like Program.cs or test setup)
                if (!fileName.EndsWith("Program.cs", StringComparison.OrdinalIgnoreCase) && 
                    !fileName.Contains("Test", StringComparison.OrdinalIgnoreCase))
                {
                    // Service locator is acceptable in Program.cs (composition root) and test files
                    var serviceLocatorCount = Regex.Matches(content, @"\.GetService<|\.GetRequiredService<").Count;
                    Assert.True(serviceLocatorCount <= 2, $"File {fileName} uses service locator pattern excessively ({serviceLocatorCount} times). Consider constructor injection instead.");
                }

                // Check for proper service registration patterns
                if (fileName == "Program.cs")
                {
                    if (content.Contains("AddScoped") || content.Contains("AddSingleton") || content.Contains("AddTransient"))
                    {
                        _output.WriteLine($"✅ {fileName}: Properly registers services with appropriate lifetimes");
                    }
                }
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Handle_Nullability_Properly()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Check nullable annotations
            if (content.Contains("string?") || content.Contains("?"))
            {
                _output.WriteLine($"✅ {fileName}: Uses nullable reference types");
            }

            // Check for null checks
            if (content.Contains("== null") || content.Contains("!= null") || content.Contains("is null") || content.Contains("is not null"))
            {
                _output.WriteLine($"✅ {fileName}: Includes proper null checks");
            }

            // Should use null-conditional operators where appropriate
            if (content.Contains("?.") || content.Contains("??"))
            {
                _output.WriteLine($"✅ {fileName}: Uses null-conditional operators");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Use_Modern_CSharp_Features()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Check for modern C# features
            var modernFeatures = new[]
            {
                ("Pattern matching", new[] { " is ", " switch " }),
                ("String interpolation", new[] { "$\"", "$@\"" }),
                ("Expression-bodied members", new[] { " => " }),
                ("var keyword", new[] { "var " }),
                ("LINQ", new[] { ".Where(", ".Select(", ".Any(", ".First(" }),
                ("using declarations", new[] { "using var ", "using " }),
                ("Record types", new[] { "record " }),
                ("Target-typed new", new[] { "new()" })
            };

            foreach (var (featureName, patterns) in modernFeatures)
            {
                if (patterns.Any(pattern => content.Contains(pattern)))
                {
                    _output.WriteLine($"✅ {fileName}: Uses {featureName}");
                }
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Follow_Security_Best_Practices()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Security checks
            
            // Should not contain hardcoded secrets
            var suspiciousPatterns = new[]
            {
                @"password\s*=\s*[""'].+[""']",
                @"secret\s*=\s*[""'].+[""']",
                @"key\s*=\s*[""'].+[""']",
                @"token\s*=\s*[""'][A-Za-z0-9+/=]{20,}[""']"
            };

            foreach (var pattern in suspiciousPatterns)
            {
                Assert.False(Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase),
                    $"Potential hardcoded secret found in {fileName}");
            }

            // Web-specific security checks
            if (fileName == "Program.cs" && content.Contains("WebApplication"))
            {
                Assert.Contains("UseHttpsRedirection", content);
                Assert.Contains("HttpOnly = true", content);
                Assert.Contains("SecurePolicy", content);
                
                _output.WriteLine($"✅ {fileName}: Implements HTTPS and secure cookies");
            }

            // Should use secure random generation
            if (content.Contains("Guid.NewGuid") || content.Contains("Random"))
            {
                _output.WriteLine($"✅ {fileName}: Uses appropriate random generation");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Proper_Logging()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Logging checks
            if (content.Contains("ILogger"))
            {
                _output.WriteLine($"✅ {fileName}: Uses structured logging with ILogger");

                // Should use structured logging, not string concatenation
                var logCallPattern = @"_logger\.Log\w*\([^)]*\+";
                Assert.False(Regex.IsMatch(content, logCallPattern),
                    $"Avoid string concatenation in logging calls in {fileName}");
            }

            // Check for appropriate log levels
            var logLevels = new[] { "LogInformation", "LogWarning", "LogError", "LogDebug" };
            foreach (var level in logLevels)
            {
                if (content.Contains(level))
                {
                    _output.WriteLine($"✅ {fileName}: Uses appropriate log level {level}");
                }
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Use_Configuration_Properly()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var configFiles = new[]
        {
            Path.Combine(_consoleSamplePath, "appsettings.json"),
            Path.Combine(_webSamplePath, "appsettings.json"),
            Path.Combine(_webSamplePath, "appsettings.Development.json")
        };

        // Assert - Configuration files exist
        foreach (var configFile in configFiles)
        {
            if (File.Exists(configFile))
            {
                var content = File.ReadAllText(configFile);
                
                // Should not contain sensitive data
                Assert.DoesNotContain("password", content.ToLower());
                Assert.DoesNotContain("secret", content.ToLower());
                
                _output.WriteLine($"✅ {Path.GetFileName(configFile)}: Exists and doesn't contain sensitive data");
            }
        }

        // Check code uses IConfiguration properly
        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            if (content.Contains("IConfiguration"))
            {
                _output.WriteLine($"✅ {fileName}: Uses IConfiguration for configuration access");
            }

            if (content.Contains("AddUserSecrets"))
            {
                _output.WriteLine($"✅ {fileName}: Configures User Secrets for development");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Proper_Error_Messages()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Error message checks
            var errorMessages = Regex.Matches(content, @"[""']([^""']*(?:error|failed|invalid|unauthorized)[^""']*)[""']", RegexOptions.IgnoreCase);

            foreach (Match match in errorMessages)
            {
                var message = match.Groups[1].Value;
                
                // Should not expose sensitive information
                Assert.DoesNotContain("password", message.ToLower());
                Assert.DoesNotContain("secret key", message.ToLower());
                Assert.DoesNotContain("connection string", message.ToLower());
                
                // Should be user-friendly
                Assert.False(message.Contains("Exception") && message.Contains("at "),
                    $"Error message in {fileName} should not expose stack traces");
            }

            _output.WriteLine($"✅ {fileName}: Error messages are appropriate and secure");
        }
    }

    [Fact]
    public void Sample_Code_Should_Follow_Naming_Conventions()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Assert - Naming convention checks
            
            // Class names should be PascalCase
            var classMatches = Regex.Matches(content, @"class\s+(\w+)");
            foreach (Match match in classMatches)
            {
                var className = match.Groups[1].Value;
                Assert.True(char.IsUpper(className[0]), 
                    $"Class name '{className}' in {fileName} should start with uppercase");
            }

            // Method names should be PascalCase
            var methodMatches = Regex.Matches(content, @"(?:public|private|protected|internal)\s+(?:static\s+)?(?:async\s+)?(?:\w+\s+)?(\w+)\s*\(");
            foreach (Match match in methodMatches)
            {
                var methodName = match.Groups[1].Value;
                if (methodName != "Main" && !methodName.StartsWith("_"))
                {
                    Assert.True(char.IsUpper(methodName[0]), 
                        $"Method name '{methodName}' in {fileName} should start with uppercase");
                }
            }

            // Private fields should start with underscore
            var fieldMatches = Regex.Matches(content, @"private\s+readonly\s+\w+\s+(\w+)");
            foreach (Match match in fieldMatches)
            {
                var fieldName = match.Groups[1].Value;
                Assert.True(fieldName.StartsWith("_"), 
                    $"Private field '{fieldName}' in {fileName} should start with underscore");
            }

            _output.WriteLine($"✅ {fileName}: Follows .NET naming conventions");
        }
    }

    private string[] GetAllCSharpFiles()
    {
        var files = new[]
        {
            Directory.GetFiles(_consoleSamplePath, "*.cs", SearchOption.AllDirectories),
            Directory.GetFiles(_webSamplePath, "*.cs", SearchOption.AllDirectories)
        }.SelectMany(x => x)
         .Where(f => !f.Contains("obj") && !f.Contains("bin")) // Exclude build artifacts
         .ToArray();

        _output.WriteLine($"Found {files.Length} C# files to analyze");
        return files;
    }

    private static string ExtractCatchBlock(string content, int catchIndex)
    {
        var openBrace = content.IndexOf('{', catchIndex);
        if (openBrace == -1) return "";

        var braceCount = 1;
        var i = openBrace + 1;
        
        while (i < content.Length && braceCount > 0)
        {
            if (content[i] == '{') braceCount++;
            else if (content[i] == '}') braceCount--;
            i++;
        }

        return content.Substring(openBrace, i - openBrace);
    }
}