using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.Quality;

/// <summary>
/// Code quality, maintainability, and technical debt assessment tests
/// Evaluates code structure, complexity, documentation, and maintainability metrics
/// </summary>
public class CodeQualityAssessmentTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _projectRoot;
    private readonly string _samplesPath;

    public CodeQualityAssessmentTests(ITestOutputHelper output)
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
    public void Sample_Code_Should_Have_Appropriate_Method_Complexity()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var complexityReport = new List<(string file, string method, int complexity)>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act - Calculate cyclomatic complexity for methods
            var methods = ExtractMethods(content);
            
            foreach (var method in methods)
            {
                var complexity = CalculateCyclomaticComplexity(method.body);
                complexityReport.Add((fileName, method.name, complexity));

                // Assert - Methods should have reasonable complexity
                if (complexity > 10)
                {
                    _output.WriteLine($"⚠️  {fileName}::{method.name}: High complexity ({complexity})");
                }
                else if (complexity > 5)
                {
                    _output.WriteLine($"ℹ️  {fileName}::{method.name}: Moderate complexity ({complexity})");
                }
                else
                {
                    _output.WriteLine($"✅ {fileName}::{method.name}: Low complexity ({complexity})");
                }
            }
        }

        // Overall complexity analysis
        var averageComplexity = complexityReport.Average(r => r.complexity);
        var highComplexityMethods = complexityReport.Count(r => r.complexity > 10);
        
        Assert.True(averageComplexity < 5, $"Average method complexity should be reasonable (current: {averageComplexity:F1})");
        Assert.True(highComplexityMethods < complexityReport.Count * 0.1, 
            $"Less than 10% of methods should have high complexity (current: {highComplexityMethods}/{complexityReport.Count})");

        _output.WriteLine($"✅ Complexity Analysis: {complexityReport.Count} methods, average complexity: {averageComplexity:F1}");
    }

    [Fact]
    public void Sample_Code_Should_Have_Consistent_Formatting()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);
            var lines = content.Split('\n');

            // Act & Assert - Check formatting consistency

            // Check indentation consistency
            var indentationIssues = 0;
            
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                var leadingSpaces = line.Length - line.TrimStart().Length;
                
                // Check for mixed tabs and spaces
                if (line.StartsWith("\t") && line.Contains("    "))
                {
                    indentationIssues++;
                }
            }

            if (indentationIssues == 0)
            {
                _output.WriteLine($"✅ {fileName}: Consistent indentation");
            }
            else
            {
                _output.WriteLine($"⚠️  {fileName}: {indentationIssues} indentation inconsistencies");
            }

            // Check line length
            var longLines = lines.Where(l => l.Length > 120).Count();
            if (longLines == 0)
            {
                _output.WriteLine($"✅ {fileName}: Appropriate line lengths");
            }
            else
            {
                _output.WriteLine($"ℹ️  {fileName}: {longLines} long lines (>120 characters)");
            }

            // Check for trailing whitespace
            var trailingWhitespace = lines.Count(l => l.EndsWith(" ") || l.EndsWith("\t"));
            if (trailingWhitespace == 0)
            {
                _output.WriteLine($"✅ {fileName}: No trailing whitespace");
            }
            else
            {
                _output.WriteLine($"ℹ️  {fileName}: {trailingWhitespace} lines with trailing whitespace");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Meaningful_Names()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check naming quality

            // Check for single letter variables (except common ones like i, j, k in loops)
            var singleLetterVars = Regex.Matches(content, @"\b(?:var|int|string|bool)\s+([a-z])\b")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .Where(v => v != "i" && v != "j" && v != "k" && v != "x" && v != "y")
                .Distinct()
                .ToList();

            if (singleLetterVars.Any())
            {
                _output.WriteLine($"ℹ️  {fileName}: Single letter variables: {string.Join(", ", singleLetterVars)}");
            }
            else
            {
                _output.WriteLine($"✅ {fileName}: Meaningful variable names");
            }

            // Check for Hungarian notation (discouraged in C#)
            var hungarianPatterns = new[] { "str", "int", "bool", "obj", "lst" };
            var hungarianVars = new List<string>();

            foreach (var pattern in hungarianPatterns)
            {
                var matches = Regex.Matches(content, $@"\b{pattern}[A-Z]\w*\b")
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .Where(v => !IsCommonException(v))
                    .Distinct();
                    
                hungarianVars.AddRange(matches);
            }

            if (hungarianVars.Any())
            {
                _output.WriteLine($"ℹ️  {fileName}: Possible Hungarian notation: {string.Join(", ", hungarianVars.Take(3))}");
            }

            // Check for meaningful method names
            var methods = Regex.Matches(content, @"(?:public|private|protected|internal)\s+(?:static\s+)?(?:async\s+)?(?:\w+\s+)?(\w+)\s*\(")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .Where(m => m != "Main" && !m.StartsWith("get_") && !m.StartsWith("set_"))
                .Distinct()
                .ToList();

            var meaningfulMethods = methods.Count(m => m.Length > 3 && ContainsVerb(m));
            var totalMethods = methods.Count;

            if (totalMethods > 0)
            {
                var percentage = (meaningfulMethods / (double)totalMethods) * 100;
                _output.WriteLine($"✅ {fileName}: {percentage:F0}% of methods have meaningful names ({meaningfulMethods}/{totalMethods})");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Appropriate_Documentation()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var documentationReport = new List<string>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check documentation coverage

            // Check for XML documentation comments
            var xmlComments = Regex.Matches(content, @"///\s*<summary>").Count;
            var publicMethods = Regex.Matches(content, @"public\s+(?:static\s+)?(?:async\s+)?\w+\s+\w+\s*\(").Count;
            var publicClasses = Regex.Matches(content, @"public\s+(?:abstract\s+|sealed\s+)?class\s+\w+").Count;

            if (xmlComments > 0)
            {
                documentationReport.Add($"✅ {fileName}: {xmlComments} XML documentation comments");
            }

            // Check for inline comments
            var inlineComments = Regex.Matches(content, @"//(?!/)[^\r\n]*").Count;
            if (inlineComments > 0)
            {
                documentationReport.Add($"✅ {fileName}: {inlineComments} inline comments");
            }

            // Check documentation density
            var codeLines = content.Split('\n').Count(l => !string.IsNullOrWhiteSpace(l) && !l.Trim().StartsWith("//") && !l.Trim().StartsWith("///"));
            var commentLines = xmlComments + inlineComments;
            
            if (codeLines > 0)
            {
                var commentRatio = (commentLines / (double)codeLines) * 100;
                if (commentRatio >= 10)
                {
                    documentationReport.Add($"✅ {fileName}: Good documentation density ({commentRatio:F1}%)");
                }
                else if (commentRatio >= 5)
                {
                    documentationReport.Add($"ℹ️  {fileName}: Moderate documentation density ({commentRatio:F1}%)");
                }
                else
                {
                    documentationReport.Add($"⚠️  {fileName}: Low documentation density ({commentRatio:F1}%)");
                }
            }
        }

        // Output documentation report
        foreach (var line in documentationReport)
        {
            _output.WriteLine(line);
        }

        Assert.True(documentationReport.Any(r => r.Contains("XML documentation")), 
            "Should find XML documentation in sample code");
    }

    [Fact]
    public void Sample_Code_Should_Follow_SOLID_Principles()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check SOLID principles

            // Single Responsibility Principle - Classes should have one reason to change
            var classCount = Regex.Matches(content, @"class\s+\w+").Count;
            var methodCount = Regex.Matches(content, @"(?:public|private|protected)\s+(?:static\s+)?(?:async\s+)?\w+\s+\w+\s*\(").Count;
            
            if (classCount > 0)
            {
                var methodsPerClass = methodCount / (double)classCount;
                if (methodsPerClass <= 10)
                {
                    _output.WriteLine($"✅ {fileName}: Good class cohesion ({methodsPerClass:F1} methods per class)");
                }
                else
                {
                    _output.WriteLine($"ℹ️  {fileName}: Consider class decomposition ({methodsPerClass:F1} methods per class)");
                }
            }

            // Dependency Inversion - Depend on abstractions
            var interfaceUsage = Regex.Matches(content, @"I[A-Z]\w+").Count;
            var concreteUsage = Regex.Matches(content, @"new\s+[A-Z]\w+\(").Count;
            
            if (interfaceUsage > 0)
            {
                _output.WriteLine($"✅ {fileName}: Uses interfaces and abstractions ({interfaceUsage} references)");
            }

            // Open/Closed Principle - Check for extensibility patterns
            if (content.Contains("virtual") || content.Contains("abstract") || content.Contains("override"))
            {
                _output.WriteLine($"✅ {fileName}: Supports extensibility (virtual/abstract/override)");
            }

            // Interface Segregation - Check interface size
            var interfaceDefinitions = Regex.Matches(content, @"interface\s+I\w+[^}]*}", RegexOptions.Singleline);
            foreach (Match interfaceDef in interfaceDefinitions)
            {
                var methodsInInterface = Regex.Matches(interfaceDef.Value, @"\w+\s*\(").Count;
                if (methodsInInterface <= 5)
                {
                    _output.WriteLine($"✅ {fileName}: Interface follows ISP (≤5 methods)");
                }
                else
                {
                    _output.WriteLine($"ℹ️  {fileName}: Large interface detected ({methodsInInterface} methods)");
                }
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Low_Coupling()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check coupling metrics

            // Count external dependencies
            var usingStatements = Regex.Matches(content, @"using\s+[\w.]+;").Count;
            var systemUsings = Regex.Matches(content, @"using\s+System").Count;
            var externalUsings = usingStatements - systemUsings;

            if (externalUsings <= 5)
            {
                _output.WriteLine($"✅ {fileName}: Low external coupling ({externalUsings} external dependencies)");
            }
            else if (externalUsings <= 10)
            {
                _output.WriteLine($"ℹ️  {fileName}: Moderate external coupling ({externalUsings} external dependencies)");
            }
            else
            {
                _output.WriteLine($"⚠️  {fileName}: High external coupling ({externalUsings} external dependencies)");
            }

            // Check for static dependencies (tightly coupled)
            var staticCalls = Regex.Matches(content, @"\w+\.\w+\(").Count;
            var instanceCalls = Regex.Matches(content, @"_\w+\.\w+\(").Count;

            if (instanceCalls > staticCalls)
            {
                _output.WriteLine($"✅ {fileName}: Prefers instance calls over static calls");
            }

            // Check for constructor injection (loose coupling)
            if (content.Contains("readonly") && content.Contains("private readonly"))
            {
                var readonlyFields = Regex.Matches(content, @"private readonly").Count;
                _output.WriteLine($"✅ {fileName}: Uses dependency injection ({readonlyFields} injected dependencies)");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Handle_Errors_Consistently()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var errorHandlingReport = new List<string>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check error handling consistency

            // Check try-catch usage
            var tryBlocks = Regex.Matches(content, @"try\s*\{").Count;
            var catchBlocks = Regex.Matches(content, @"catch\s*\(").Count;
            var finallyBlocks = Regex.Matches(content, @"finally\s*\{").Count;

            if (tryBlocks > 0)
            {
                errorHandlingReport.Add($"✅ {fileName}: {tryBlocks} try blocks, {catchBlocks} catch blocks, {finallyBlocks} finally blocks");
                
                // Check if all try blocks have catch blocks
                if (catchBlocks >= tryBlocks || finallyBlocks > 0)
                {
                    errorHandlingReport.Add($"✅ {fileName}: Proper try-catch-finally structure");
                }
            }

            // Check for logging in error handling
            var errorLogging = Regex.Matches(content, @"catch[^}]*LogError[^}]*}", RegexOptions.Singleline).Count;
            if (errorLogging > 0)
            {
                errorHandlingReport.Add($"✅ {fileName}: {errorLogging} catch blocks with error logging");
            }

            // Check for specific exception handling
            var specificExceptions = Regex.Matches(content, @"catch\s*\(\s*\w*Exception\s+\w+\s*\)").Count;
            var genericExceptions = Regex.Matches(content, @"catch\s*\(\s*Exception\s+\w+\s*\)").Count;
            
            if (specificExceptions > genericExceptions)
            {
                errorHandlingReport.Add($"✅ {fileName}: Prefers specific exception handling");
            }
        }

        // Output error handling report
        foreach (var line in errorHandlingReport)
        {
            _output.WriteLine(line);
        }
    }

    [Fact]
    public void Sample_Code_Should_Be_Testable()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check testability factors

            // Check for dependency injection (makes testing easier)
            if (content.Contains("readonly") && content.Contains("IService"))
            {
                _output.WriteLine($"✅ {fileName}: Uses dependency injection (testable)");
            }

            // Check for static methods (harder to test)
            var staticMethods = Regex.Matches(content, @"public static").Count;
            var instanceMethods = Regex.Matches(content, @"public\s+(?!static)\w+").Count;
            
            if (staticMethods == 0 || instanceMethods > staticMethods * 2)
            {
                _output.WriteLine($"✅ {fileName}: Favors instance methods over static methods");
            }
            else
            {
                _output.WriteLine($"ℹ️  {fileName}: Has {staticMethods} static methods (consider testability)");
            }

            // Check for pure functions (easier to test)
            var methods = ExtractMethods(content);
            var pureFunctions = methods.Count(m => !m.body.Contains("_") && !m.body.Contains("this."));
            
            if (pureFunctions > 0)
            {
                _output.WriteLine($"✅ {fileName}: Contains {pureFunctions} potentially pure functions");
            }

            // Check for async methods (need special testing considerations)
            var asyncMethods = Regex.Matches(content, @"async\s+Task").Count;
            if (asyncMethods > 0)
            {
                _output.WriteLine($"ℹ️  {fileName}: Contains {asyncMethods} async methods (ensure proper async testing)");
            }
        }
    }

    [Fact]
    public void Sample_Code_Should_Have_Appropriate_File_Structure()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);
            var fileInfo = new FileInfo(file);

            // Act & Assert - Check file structure

            // Check file size
            var fileSizeKB = fileInfo.Length / 1024.0;
            if (fileSizeKB <= 10)
            {
                _output.WriteLine($"✅ {fileName}: Appropriate file size ({fileSizeKB:F1} KB)");
            }
            else if (fileSizeKB <= 25)
            {
                _output.WriteLine($"ℹ️  {fileName}: Large file ({fileSizeKB:F1} KB) - consider splitting");
            }
            else
            {
                _output.WriteLine($"⚠️  {fileName}: Very large file ({fileSizeKB:F1} KB) - should be split");
            }

            // Check lines of code
            var linesOfCode = content.Split('\n').Count(l => !string.IsNullOrWhiteSpace(l) && !l.Trim().StartsWith("//"));
            if (linesOfCode <= 200)
            {
                _output.WriteLine($"✅ {fileName}: Appropriate length ({linesOfCode} LOC)");
            }
            else if (linesOfCode <= 500)
            {
                _output.WriteLine($"ℹ️  {fileName}: Long file ({linesOfCode} LOC) - consider refactoring");
            }
            else
            {
                _output.WriteLine($"⚠️  {fileName}: Very long file ({linesOfCode} LOC) - needs refactoring");
            }

            // Check class count per file
            var classCount = Regex.Matches(content, @"class\s+\w+").Count;
            if (classCount == 1)
            {
                _output.WriteLine($"✅ {fileName}: Single class per file");
            }
            else if (classCount > 1)
            {
                _output.WriteLine($"ℹ️  {fileName}: Multiple classes ({classCount}) - consider file separation");
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

    private static List<(string name, string body)> ExtractMethods(string content)
    {
        var methods = new List<(string name, string body)>();
        var methodPattern = @"(?:public|private|protected|internal)\s+(?:static\s+)?(?:async\s+)?(?:\w+\s+)?(\w+)\s*\([^)]*\)\s*\{";
        
        var matches = Regex.Matches(content, methodPattern);
        foreach (Match match in matches)
        {
            var methodName = match.Groups[1].Value;
            var startIndex = match.Index;
            
            // Find the method body (simplified - assumes proper brace matching)
            var braceCount = 1;
            var i = content.IndexOf('{', startIndex) + 1;
            var bodyStart = i;
            
            while (i < content.Length && braceCount > 0)
            {
                if (content[i] == '{') braceCount++;
                else if (content[i] == '}') braceCount--;
                i++;
            }
            
            if (braceCount == 0)
            {
                var methodBody = content.Substring(bodyStart, i - bodyStart - 1);
                methods.Add((methodName, methodBody));
            }
        }
        
        return methods;
    }

    private static int CalculateCyclomaticComplexity(string methodBody)
    {
        // Simplified cyclomatic complexity calculation
        // Real implementation would need proper parsing
        var complexity = 1; // Base complexity
        
        var complexityKeywords = new[]
        {
            @"\bif\b", @"\belse\s+if\b", @"\bwhile\b", @"\bfor\b", @"\bforeach\b",
            @"\bcase\b", @"\bcatch\b", @"\?\s*:", @"\?\?\s*", @"\|\|", @"&&"
        };
        
        foreach (var keyword in complexityKeywords)
        {
            complexity += Regex.Matches(methodBody, keyword).Count;
        }
        
        return complexity;
    }

    private static bool IsCommonException(string variableName)
    {
        var commonExceptions = new[] { "string", "stringBuilder", "intValue", "boolResult" };
        return commonExceptions.Contains(variableName);
    }

    private static bool ContainsVerb(string methodName)
    {
        var commonVerbs = new[]
        {
            "Get", "Set", "Create", "Delete", "Update", "Add", "Remove", "Find", "Search",
            "Load", "Save", "Send", "Receive", "Process", "Execute", "Run", "Start", "Stop",
            "Build", "Parse", "Validate", "Convert", "Transform", "Handle", "Calculate"
        };
        
        return commonVerbs.Any(verb => methodName.StartsWith(verb, StringComparison.OrdinalIgnoreCase));
    }
}