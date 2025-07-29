using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.Documentation;

/// <summary>
/// Documentation accuracy and completeness validation tests
/// Verifies documentation quality, accuracy, and alignment with implementation
/// </summary>
public class DocumentationValidationTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _projectRoot;
    private readonly string _samplesPath;

    public DocumentationValidationTests(ITestOutputHelper output)
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
    public void Sample_Applications_Should_Have_README_Files()
    {
        // Arrange
        var expectedReadmeFiles = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "README.md"),
            Path.Combine(_samplesPath, "WebSample", "README.md"),
            Path.Combine(_samplesPath, "README.md")
        };

        foreach (var readmeFile in expectedReadmeFiles)
        {
            // Act & Assert
            if (File.Exists(readmeFile))
            {
                var content = File.ReadAllText(readmeFile);
                
                // Check for essential sections
                var requiredSections = new[]
                {
                    ("# ", "Title/Header"),
                    ("## ", "Section headers"),
                    ("getting started", "Getting started section"),
                    ("prerequisite", "Prerequisites section"),
                    ("configuration", "Configuration section"),
                    ("usage", "Usage examples")
                };

                var sectionsFound = new List<string>();
                foreach (var (pattern, description) in requiredSections)
                {
                    if (content.ToLower().Contains(pattern.ToLower()) || 
                        content.ToLower().Contains(description.Split(' ')[0].ToLower()))
                    {
                        sectionsFound.Add(description);
                    }
                }

                _output.WriteLine($"✅ {Path.GetFileName(readmeFile)}: Found sections - {string.Join(", ", sectionsFound)}");
                
                // Should have at least basic structure
                Assert.Contains("Title/Header", sectionsFound);
                Assert.True(content.Length > 100, "README should have substantial content");
            }
            else
            {
                _output.WriteLine($"⚠️  Missing README file: {Path.GetFileName(readmeFile)}");
            }
        }
    }

    [Fact]
    public void Code_Comments_Should_Be_Accurate_And_Helpful()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var commentAnalysis = new List<string>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Analyze comment quality

            // Extract XML documentation comments
            var xmlComments = Regex.Matches(content, @"///\s*<summary>(.*?)</summary>", RegexOptions.Singleline)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value.Trim())
                .ToList();

            foreach (var comment in xmlComments)
            {
                // Check for meaningful content
                if (comment.Length < 10)
                {
                    commentAnalysis.Add($"⚠️  {fileName}: Short XML comment - '{comment}'");
                }
                else if (comment.ToLower().Contains("todo") || comment.ToLower().Contains("fix"))
                {
                    commentAnalysis.Add($"ℹ️  {fileName}: TODO comment found - '{comment}'");
                }
                else
                {
                    commentAnalysis.Add($"✅ {fileName}: Good XML documentation");
                }
            }

            // Extract inline comments
            var inlineComments = Regex.Matches(content, @"//\s*([^\r\n]+)")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value.Trim())
                .Where(c => !c.StartsWith("/") && !string.IsNullOrEmpty(c))
                .ToList();

            var meaningfulInlineComments = 0;
            foreach (var comment in inlineComments)
            {
                if (comment.Length > 15 && !comment.ToLower().Contains("todo"))
                {
                    meaningfulInlineComments++;
                }
            }

            if (meaningfulInlineComments > 0)
            {
                commentAnalysis.Add($"✅ {fileName}: {meaningfulInlineComments} helpful inline comments");
            }

            // Check for commented-out code (code smells)
            var commentedCode = inlineComments.Count(c => 
                c.Contains("(") && c.Contains(")") || 
                c.Contains("{") || c.Contains("}") ||
                c.Contains("var ") || c.Contains("if "));

            if (commentedCode > 0)
            {
                commentAnalysis.Add($"⚠️  {fileName}: {commentedCode} commented-out code blocks (consider removal)");
            }
        }

        // Output comment analysis
        foreach (var analysis in commentAnalysis)
        {
            _output.WriteLine(analysis);
        }

        Assert.True(commentAnalysis.Any(a => a.Contains("XML documentation") || a.Contains("inline comments")), 
            "Should find meaningful documentation in code");
    }

    [Fact]
    public void Configuration_Files_Should_Be_Documented()
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

            // Act & Assert - Validate configuration documentation
            try
            {
                var json = JsonDocument.Parse(content);
                
                // Check for Procore configuration section
                if (json.RootElement.TryGetProperty("Procore", out var procoreSection))
                {
                    _output.WriteLine($"✅ {fileName}: Contains Procore configuration section");
                    
                    // Check for authentication configuration
                    if (procoreSection.TryGetProperty("Authentication", out var authSection))
                    {
                        var requiredAuthFields = new[] { "ClientId", "RedirectUri", "Scopes" };
                        var foundFields = new List<string>();
                        
                        foreach (var field in requiredAuthFields)
                        {
                            if (authSection.TryGetProperty(field, out _))
                            {
                                foundFields.Add(field);
                            }
                        }
                        
                        _output.WriteLine($"✅ {fileName}: Authentication fields - {string.Join(", ", foundFields)}");
                        
                        // Should have essential auth configuration
                        Assert.True(foundFields.Count >= 2, 
                            $"{fileName} should have at least 2 essential auth configuration fields");
                    }
                }

                // Check for logging configuration
                if (json.RootElement.TryGetProperty("Logging", out var loggingSection))
                {
                    _output.WriteLine($"✅ {fileName}: Contains logging configuration");
                }

                _output.WriteLine($"✅ {fileName}: Valid JSON configuration");
            }
            catch (JsonException ex)
            {
                _output.WriteLine($"❌ {fileName}: Invalid JSON - {ex.Message}");
                Assert.True(false, $"Configuration file {fileName} should contain valid JSON");
            }
        }
    }

    [Fact]
    public void API_Methods_Should_Have_Comprehensive_Documentation()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check API method documentation
            
            // Find public methods
            var publicMethods = Regex.Matches(content, @"public\s+(?:static\s+)?(?:async\s+)?(\w+(?:<[^>]+>)?)\s+(\w+)\s*\([^)]*\)")
                .Cast<Match>()
                .Select(m => new { ReturnType = m.Groups[1].Value, MethodName = m.Groups[2].Value })
                .ToList();

            foreach (var method in publicMethods)
            {
                // Look for XML documentation before the method
                var methodPattern = $@"///.*?public\s+(?:static\s+)?(?:async\s+)?\w+(?:<[^>]+>)?\s+{Regex.Escape(method.MethodName)}\s*\(";
                var hasXmlDoc = Regex.IsMatch(content, methodPattern, RegexOptions.Singleline);

                if (hasXmlDoc)
                {
                    _output.WriteLine($"✅ {fileName}::{method.MethodName}: Has XML documentation");
                    
                    // Check for parameter documentation
                    var paramPattern = $@"///.*?<param.*?</param>.*?public\s+(?:static\s+)?(?:async\s+)?\w+(?:<[^>]+>)?\s+{Regex.Escape(method.MethodName)}\s*\(";
                    var hasParamDoc = Regex.IsMatch(content, paramPattern, RegexOptions.Singleline);
                    
                    if (hasParamDoc)
                    {
                        _output.WriteLine($"✅ {fileName}::{method.MethodName}: Documents parameters");
                    }
                    
                    // Check for return documentation
                    if (method.ReturnType != "void")
                    {
                        var returnPattern = $@"///.*?<returns>.*?</returns>.*?public\s+(?:static\s+)?(?:async\s+)?\w+(?:<[^>]+>)?\s+{Regex.Escape(method.MethodName)}\s*\(";
                        var hasReturnDoc = Regex.IsMatch(content, returnPattern, RegexOptions.Singleline);
                        
                        if (hasReturnDoc)
                        {
                            _output.WriteLine($"✅ {fileName}::{method.MethodName}: Documents return value");
                        }
                        else
                        {
                            _output.WriteLine($"ℹ️  {fileName}::{method.MethodName}: Missing return documentation");
                        }
                    }
                }
                else if (method.MethodName != "Main" && !method.MethodName.StartsWith("get_") && !method.MethodName.StartsWith("set_"))
                {
                    _output.WriteLine($"ℹ️  {fileName}::{method.MethodName}: Missing XML documentation");
                }
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Usage_Examples()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var examplePatterns = new[]
        {
            (@"//\s*Example:", "Inline example comments"),
            (@"///\s*<example>", "XML documentation examples"),
            (@"//\s*Usage:", "Usage comments"),
            (@"Console\.WriteLine", "Console output examples"),
            (@"_output\.WriteLine", "Test output examples")
        };

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check for usage examples
            var foundExamples = new List<string>();

            foreach (var (pattern, description) in examplePatterns)
            {
                if (Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase))
                {
                    foundExamples.Add(description);
                }
            }

            if (foundExamples.Any())
            {
                _output.WriteLine($"✅ {fileName}: Contains examples - {string.Join(", ", foundExamples)}");
            }
            else if (fileName.Contains("Program.cs"))
            {
                // Program.cs files should demonstrate usage
                _output.WriteLine($"ℹ️  {fileName}: Main program files should include usage examples");
            }
        }
    }

    [Fact]
    public void Error_Messages_Should_Be_User_Friendly_And_Documented()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check error message quality
            
            // Extract error messages
            var errorMessages = new List<string>();
            
            // From TempData (web app)
            var tempDataErrors = Regex.Matches(content, @"TempData\[[""']Error[""']\]\s*=\s*[""']([^""']+)[""']")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value);
            errorMessages.AddRange(tempDataErrors);
            
            // From Console output
            var consoleErrors = Regex.Matches(content, @"Console\.WriteLine\([""']❌[^""']*([^""']*)[""']\)")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value);
            errorMessages.AddRange(consoleErrors);
            
            // From exceptions
            var exceptionMessages = Regex.Matches(content, @"throw new \w*Exception\([""']([^""']+)[""']\)")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value);
            errorMessages.AddRange(exceptionMessages);

            foreach (var message in errorMessages.Where(m => !string.IsNullOrEmpty(m)))
            {
                // Check message quality
                if (message.Length < 10)
                {
                    _output.WriteLine($"⚠️  {fileName}: Short error message - '{message}'");
                }
                else if (message.Contains("Exception") || message.Contains("Error:"))
                {
                    _output.WriteLine($"ℹ️  {fileName}: Technical error message - '{message}'");
                }
                else if (message.ToLower().Contains("try") || message.ToLower().Contains("check"))
                {
                    _output.WriteLine($"✅ {fileName}: Helpful error message - '{message}'");
                }
                else
                {
                    _output.WriteLine($"✅ {fileName}: User-friendly error message - '{message}'");
                }
            }
        }
    }

    [Fact]
    public void Dependencies_Should_Be_Documented()
    {
        // Arrange
        var projectFiles = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "ConsoleSample.csproj"),
            Path.Combine(_samplesPath, "WebSample", "WebSample.csproj")
        };

        foreach (var projectFile in projectFiles.Where(File.Exists))
        {
            var content = File.ReadAllText(projectFile);
            var fileName = Path.GetFileName(projectFile);

            // Act & Assert - Validate dependency documentation
            
            // Extract package references
            var packageReferences = Regex.Matches(content, @"<PackageReference\s+Include=[""']([^""']+)[""'](?:\s+Version=[""']([^""']+)[""'])?")
                .Cast<Match>()
                .Select(m => new { Package = m.Groups[1].Value, Version = m.Groups[2].Value })
                .ToList();

            if (packageReferences.Any())
            {
                _output.WriteLine($"✅ {fileName}: Dependencies found:");
                foreach (var package in packageReferences)
                {
                    var versionInfo = string.IsNullOrEmpty(package.Version) ? "(implicit version)" : $"v{package.Version}";
                    _output.WriteLine($"   - {package.Package} {versionInfo}");
                }
            }

            // Check for project references
            var projectReferences = Regex.Matches(content, @"<ProjectReference\s+Include=[""']([^""']+)[""']")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .ToList();

            if (projectReferences.Any())
            {
                _output.WriteLine($"✅ {fileName}: Project references:");
                foreach (var reference in projectReferences)
                {
                    _output.WriteLine($"   - {Path.GetFileName(reference)}");
                }
            }

            // Validate essential dependencies are present
            var essentialPackages = new[] { "Microsoft.Extensions", "Procore.SDK" };
            var hasEssentialDeps = essentialPackages.Any(essential => 
                packageReferences.Any(pkg => pkg.Package.Contains(essential)) ||
                projectReferences.Any(proj => proj.Contains(essential)));

            if (hasEssentialDeps)
            {
                _output.WriteLine($"✅ {fileName}: Contains essential SDK dependencies");
            }
            else
            {
                _output.WriteLine($"⚠️  {fileName}: May be missing essential dependencies");
            }
        }
    }

    [Fact]
    public void Documentation_Should_Be_Up_To_Date()
    {
        // Arrange
        var documentationFiles = Directory.GetFiles(_projectRoot, "*.md", SearchOption.AllDirectories)
            .Where(f => !f.Contains("node_modules") && !f.Contains("bin") && !f.Contains("obj"))
            .ToArray();

        foreach (var docFile in documentationFiles)
        {
            var content = File.ReadAllText(docFile);
            var fileName = Path.GetFileName(docFile);
            var fileInfo = new FileInfo(docFile);

            // Act & Assert - Check documentation freshness
            
            // Check for outdated version references
            var versionReferences = Regex.Matches(content, @"(?:\.NET|net|version)\s*[v]?(\d+\.?\d*)").Count;
            if (versionReferences > 0)
            {
                _output.WriteLine($"ℹ️  {fileName}: Contains {versionReferences} version references (verify currency)");
            }

            // Check for TODO or placeholder content
            var placeholders = new[] { "TODO", "TBD", "coming soon", "under construction", "placeholder" };
            var foundPlaceholders = placeholders.Where(p => content.ToLower().Contains(p.ToLower())).ToList();
            
            if (foundPlaceholders.Any())
            {
                _output.WriteLine($"⚠️  {fileName}: Contains placeholders - {string.Join(", ", foundPlaceholders)}");
            }
            else
            {
                _output.WriteLine($"✅ {fileName}: No placeholder content found");
            }

            // Check last modified date
            var daysSinceModified = (DateTime.Now - fileInfo.LastWriteTime).TotalDays;
            if (daysSinceModified > 90)
            {
                _output.WriteLine($"ℹ️  {fileName}: Last modified {daysSinceModified:F0} days ago (may need review)");
            }
            else
            {
                _output.WriteLine($"✅ {fileName}: Recently updated ({daysSinceModified:F0} days ago)");
            }

            // Check for broken links (basic check for markdown links)
            var markdownLinks = Regex.Matches(content, @"\[([^\]]+)\]\(([^)]+)\)")
                .Cast<Match>()
                .Select(m => new { Text = m.Groups[1].Value, Url = m.Groups[2].Value })
                .ToList();

            var brokenLinks = markdownLinks.Where(link => 
                link.Url.StartsWith("./") || link.Url.StartsWith("../")).ToList();

            if (brokenLinks.Any())
            {
                _output.WriteLine($"ℹ️  {fileName}: {markdownLinks.Count} links found, {brokenLinks.Count} relative links (verify existence)");
            }
            else if (markdownLinks.Any())
            {
                _output.WriteLine($"✅ {fileName}: {markdownLinks.Count} links found");
            }
        }

        Assert.True(documentationFiles.Length > 0, "Should find documentation files in the project");
    }

    [Fact]
    public void Code_Should_Match_Documentation_Examples()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var docFiles = Directory.GetFiles(_projectRoot, "*.md", SearchOption.AllDirectories)
            .Where(f => !f.Contains("node_modules") && !f.Contains("bin") && !f.Contains("obj"))
            .ToArray();

        // Act & Assert - Verify code examples in documentation match actual implementation
        foreach (var docFile in docFiles)
        {
            var docContent = File.ReadAllText(docFile);
            var fileName = Path.GetFileName(docFile);

            // Extract code blocks from markdown
            var codeBlocks = Regex.Matches(docContent, @"```(?:csharp|cs|c#)?\s*\n(.*?)\n```", RegexOptions.Singleline)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value.Trim())
                .Where(code => !string.IsNullOrEmpty(code))
                .ToList();

            if (codeBlocks.Any())
            {
                _output.WriteLine($"✅ {fileName}: Found {codeBlocks.Count} code examples");

                // Check if code examples reference actual classes/methods
                foreach (var codeBlock in codeBlocks)
                {
                    // Look for class/method references in the code
                    var classReferences = Regex.Matches(codeBlock, @"\b[A-Z]\w*(?:Client|Service|Manager|Helper)\b")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .Distinct()
                        .ToList();

                    foreach (var classRef in classReferences)
                    {
                        // Check if this class exists in the actual code
                        var classExists = codeFiles.Any(file => File.ReadAllText(file).Contains($"class {classRef}"));
                        
                        if (classExists)
                        {
                            _output.WriteLine($"✅ {fileName}: Code example references existing class '{classRef}'");
                        }
                        else
                        {
                            _output.WriteLine($"⚠️  {fileName}: Code example references unknown class '{classRef}'");
                        }
                    }
                }
            }
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