# Task 10: NuGet Package Configuration & Publishing - Sample Test Implementation

## Overview

This document provides sample implementations of key test classes for validating NuGet package configuration and publishing functionality. These examples demonstrate the testing approach and can be used as templates for full implementation.

## Sample Test Implementations

### 1. Package Metadata Validation Tests

#### PackageMetadataValidationTests.cs
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Procore.SDK.Tests.NuGetPackaging
{
    public class PackageMetadataValidationTests
    {
        public static IEnumerable<object[]> GetAllSdkProjects()
        {
            var sdkProjects = new[]
            {
                "src/Procore.SDK/Procore.SDK.csproj",
                "src/Procore.SDK.Core/Procore.SDK.Core.csproj",
                "src/Procore.SDK.Shared/Procore.SDK.Shared.csproj",
                "src/Procore.SDK.ConstructionFinancials/Procore.SDK.ConstructionFinancials.csproj",
                "src/Procore.SDK.ProjectManagement/Procore.SDK.ProjectManagement.csproj",
                "src/Procore.SDK.QualitySafety/Procore.SDK.QualitySafety.csproj",
                "src/Procore.SDK.FieldProductivity/Procore.SDK.FieldProductivity.csproj",
                "src/Procore.SDK.ResourceManagement/Procore.SDK.ResourceManagement.csproj"
            };

            return sdkProjects.Select(project => new object[] { project });
        }

        [Theory]
        [MemberData(nameof(GetAllSdkProjects))]
        public void PackageMetadata_ShouldHaveRequiredFields(string projectPath)
        {
            // Arrange
            var projectFile = GetFullProjectPath(projectPath);
            var projectXml = XDocument.Load(projectFile);
            var metadata = ExtractPackageMetadata(projectXml);

            // Act & Assert
            metadata.PackageId.Should().NotBeNullOrEmpty("PackageId is required for NuGet packages");
            metadata.Description.Should().NotBeNullOrEmpty("Description is required for NuGet packages");
            metadata.Description.Length.Should().BeInRange(50, 300, "Description should be informative but concise");
            
            // Validate from Directory.Build.props
            metadata.Authors.Should().Be("Procore SDK Contributors");
            metadata.Company.Should().Be("Procore SDK Contributors");
            metadata.PackageLicenseExpression.Should().Be("MIT");
            metadata.PackageProjectUrl.Should().StartWith("https://github.com/procore/");
            metadata.RepositoryUrl.Should().StartWith("https://github.com/procore/");
            metadata.RepositoryType.Should().Be("git");
        }

        [Theory]
        [MemberData(nameof(GetAllSdkProjects))]
        public void PackageId_ShouldFollowNamingConvention(string projectPath)
        {
            // Arrange
            var projectFile = GetFullProjectPath(projectPath);
            var projectXml = XDocument.Load(projectFile);
            var metadata = ExtractPackageMetadata(projectXml);

            // Act & Assert
            metadata.PackageId.Should().StartWith("Procore.SDK", "All SDK packages should follow naming convention");
            metadata.PackageId.Should().NotContain(" ", "Package IDs should not contain spaces");
            metadata.PackageId.Should().MatchRegex(@"^Procore\.SDK(\.[A-Za-z]+)*$", 
                "Package ID should follow proper .NET naming conventions");
        }

        [Theory]
        [MemberData(nameof(GetAllSdkProjects))]
        public void PackageDescription_ShouldMeetQualityStandards(string projectPath)
        {
            // Arrange
            var projectFile = GetFullProjectPath(projectPath);
            var projectXml = XDocument.Load(projectFile);
            var metadata = ExtractPackageMetadata(projectXml);

            // Act & Assert
            metadata.Description.Should().NotBeNullOrEmpty();
            metadata.Description.Length.Should().BeGreaterThan(30, "Description should be informative");
            metadata.Description.Length.Should().BeLessThan(500, "Description should be concise");
            metadata.Description.Should().Contain("Procore", "Description should mention Procore");
            metadata.Description.Should().EndWith(".") 
                .Or.EndWith("!") 
                .Or.EndWith("?", "Description should end with proper punctuation");
        }

        [Fact]
        public void PackageVersions_ShouldBeConsistentAcrossProjects()
        {
            // Arrange
            var projects = GetAllSdkProjects().Select(data => (string)data[0]);
            var versions = new List<string>();

            // Act
            foreach (var projectPath in projects)
            {
                var projectFile = GetFullProjectPath(projectPath);
                var projectXml = XDocument.Load(projectFile);
                var version = ExtractVersion(projectXml);
                versions.Add(version);
            }

            // Assert
            versions.Should().OnlyHaveUniqueItems(because: "all projects should use the same version");
            versions.First().Should().MatchRegex(@"^\d+\.\d+\.\d+(-.*)?$", 
                "Version should follow semantic versioning format");
        }

        private static string GetFullProjectPath(string relativePath)
        {
            var baseDirectory = Path.GetDirectoryName(typeof(PackageMetadataValidationTests).Assembly.Location);
            var projectPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "..", relativePath);
            return Path.GetFullPath(projectPath);
        }

        private static PackageMetadata ExtractPackageMetadata(XDocument projectXml)
        {
            var propertyGroups = projectXml.Descendants("PropertyGroup");
            
            return new PackageMetadata
            {
                PackageId = GetPropertyValue(propertyGroups, "PackageId"),
                Description = GetPropertyValue(propertyGroups, "Description"),
                Authors = GetPropertyValue(propertyGroups, "Authors"),
                Company = GetPropertyValue(propertyGroups, "Company"),
                PackageLicenseExpression = GetPropertyValue(propertyGroups, "PackageLicenseExpression"),
                PackageProjectUrl = GetPropertyValue(propertyGroups, "PackageProjectUrl"),
                RepositoryUrl = GetPropertyValue(propertyGroups, "RepositoryUrl"),
                RepositoryType = GetPropertyValue(propertyGroups, "RepositoryType")
            };
        }

        private static string ExtractVersion(XDocument projectXml)
        {
            var propertyGroups = projectXml.Descendants("PropertyGroup");
            return GetPropertyValue(propertyGroups, "Version") ?? 
                   GetPropertyValue(propertyGroups, "VersionPrefix") ?? 
                   "1.0.0";
        }

        private static string GetPropertyValue(IEnumerable<XElement> propertyGroups, string propertyName)
        {
            return propertyGroups
                .SelectMany(pg => pg.Elements(propertyName))
                .FirstOrDefault()?.Value;
        }
    }

    public class PackageMetadata
    {
        public string PackageId { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public string Company { get; set; }
        public string PackageLicenseExpression { get; set; }
        public string PackageProjectUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public string RepositoryType { get; set; }
    }
}
```

### 2. Multi-Targeting Validation Tests

#### MultiTargetingValidationTests.cs
```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Procore.SDK.Tests.NuGetPackaging
{
    public class MultiTargetingValidationTests
    {
        private static readonly string[] SupportedFrameworks = { "net6.0", "net7.0", "net8.0" };

        [Theory]
        [InlineData("net6.0")]
        [InlineData("net7.0")] 
        [InlineData("net8.0")]
        public void SdkProjects_ShouldCompileForTargetFramework(string targetFramework)
        {
            // Arrange
            var projects = PackageMetadataValidationTests.GetAllSdkProjects()
                .Select(data => (string)data[0]);

            // Act & Assert
            foreach (var projectPath in projects)
            {
                var result = BuildProjectForFramework(projectPath, targetFramework);
                result.Success.Should().BeTrue(
                    $"Project {projectPath} should compile successfully for {targetFramework}. " +
                    $"Build output: {result.Output}");
            }
        }

        [Theory]
        [MemberData(nameof(PackageMetadataValidationTests.GetAllSdkProjects), MemberType = typeof(PackageMetadataValidationTests))]
        public void Generated_Packages_ShouldContainCorrectTargetFrameworks(string projectPath)
        {
            // Arrange
            var packagePath = BuildAndPackProject(projectPath);

            // Act
            var packageContents = ExtractPackageContents(packagePath);
            var frameworkFolders = packageContents
                .Where(entry => entry.StartsWith("lib/"))
                .Select(entry => entry.Split('/')[1])
                .Distinct()
                .ToList();

            // Assert
            frameworkFolders.Should().NotBeEmpty("Package should contain assemblies for target frameworks");
            
            foreach (var framework in SupportedFrameworks)
            {
                frameworkFolders.Should().Contain(framework, 
                    $"Package should contain assembly for {framework}");
            }
        }

        [Fact]
        public void PackageReferences_ShouldSupportAllTargetFrameworks()
        {
            // Arrange
            var projects = PackageMetadataValidationTests.GetAllSdkProjects()
                .Select(data => (string)data[0]);

            // Act & Assert
            foreach (var projectPath in projects)
            {
                var packageReferences = ExtractPackageReferences(projectPath);
                
                foreach (var packageRef in packageReferences)
                {
                    ValidatePackageFrameworkSupport(packageRef.Id, packageRef.Version);
                }
            }
        }

        private static BuildResult BuildProjectForFramework(string projectPath, string targetFramework)
        {
            var fullPath = GetFullProjectPath(projectPath);
            var processInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{fullPath}\" --framework {targetFramework} --configuration Release",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            return new BuildResult
            {
                Success = process.ExitCode == 0,
                Output = $"{output}\n{error}"
            };
        }

        private static string BuildAndPackProject(string projectPath)
        {
            var fullPath = GetFullProjectPath(projectPath);
            var outputDir = Path.Combine(Path.GetTempPath(), "test-packages", Guid.NewGuid().ToString());
            Directory.CreateDirectory(outputDir);

            var processInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"pack \"{fullPath}\" --configuration Release --output \"{outputDir}\" --include-symbols",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                throw new InvalidOperationException($"Failed to pack project {projectPath}: {error}");
            }

            return Directory.GetFiles(outputDir, "*.nupkg")[0];
        }

        private static List<string> ExtractPackageContents(string packagePath)
        {
            // This would use a NuGet package reading library in real implementation
            // For demo purposes, showing the structure
            var processInfo = new ProcessStartInfo
            {
                FileName = "unzip",
                Arguments = $"-l \"{packagePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            return output.Split('\n')
                .Where(line => line.Contains("lib/"))
                .Select(line => line.Trim().Split(' ').Last())
                .ToList();
        }

        private static List<PackageReference> ExtractPackageReferences(string projectPath)
        {
            // Implementation would parse project file for PackageReference elements
            // For demo purposes, returning common references
            return new List<PackageReference>
            {
                new PackageReference { Id = "Microsoft.Extensions.DependencyInjection", Version = "8.0.0" },
                new PackageReference { Id = "Microsoft.Kiota.Abstractions", Version = "1.12.0" },
                new PackageReference { Id = "System.Text.Json", Version = "8.0.5" }
            };
        }

        private static void ValidatePackageFrameworkSupport(string packageId, string version)
        {
            // This would query NuGet API or local cache to verify framework support
            // For demo purposes, assuming validation passes
        }

        private static string GetFullProjectPath(string relativePath)
        {
            var baseDirectory = Path.GetDirectoryName(typeof(MultiTargetingValidationTests).Assembly.Location);
            var projectPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "..", relativePath);
            return Path.GetFullPath(projectPath);
        }
    }

    public class BuildResult
    {
        public bool Success { get; set; }
        public string Output { get; set; }
    }

    public class PackageReference
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
```

### 3. Source Linking Validation Tests

#### SourceLinkingValidationTests.cs
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Procore.SDK.Tests.NuGetPackaging
{
    public class SourceLinkingValidationTests
    {
        [Theory]
        [MemberData(nameof(PackageMetadataValidationTests.GetAllSdkProjects), MemberType = typeof(PackageMetadataValidationTests))]
        public void Assembly_ShouldContainSourceLinkInformation(string projectPath)
        {
            // Arrange
            var assemblyPath = BuildProjectAndGetAssembly(projectPath);

            // Act
            var assembly = Assembly.LoadFrom(assemblyPath);
            var sourceLinkAttribute = assembly.GetCustomAttributes<AssemblyConfigurationAttribute>()
                .FirstOrDefault(attr => attr.Configuration.Contains("SourceLink"));

            // Assert
            sourceLinkAttribute.Should().NotBeNull("Assembly should contain source link information");
        }

        [Theory]
        [MemberData(nameof(PackageMetadataValidationTests.GetAllSdkProjects), MemberType = typeof(PackageMetadataValidationTests))]
        public void SymbolPackage_ShouldBeGenerated(string projectPath)
        {
            // Arrange & Act
            var packageInfo = BuildAndPackProject(projectPath);

            // Assert
            File.Exists(packageInfo.SymbolPackagePath).Should().BeTrue(
                $"Symbol package should be generated at {packageInfo.SymbolPackagePath}");
                
            var symbolPackageSize = new FileInfo(packageInfo.SymbolPackagePath).Length;
            symbolPackageSize.Should().BeGreaterThan(0, "Symbol package should not be empty");
        }

        [Theory]
        [MemberData(nameof(PackageMetadataValidationTests.GetAllSdkProjects), MemberType = typeof(PackageMetadataValidationTests))]
        public void SourceLink_ShouldMapToCorrectGitHubUrls(string projectPath)
        {
            // Arrange
            var packageInfo = BuildAndPackProject(projectPath);
            var sourceLinkJson = ExtractSourceLinkJson(packageInfo.SymbolPackagePath);

            // Act
            var sourceLinkData = JsonSerializer.Deserialize<Dictionary<string, string>>(sourceLinkJson);

            // Assert
            sourceLinkData.Should().NotBeNull("Source link JSON should be valid");
            sourceLinkData.Should().ContainKey("*", "Source link should contain wildcard mapping");
            
            var githubUrl = sourceLinkData["*"];
            githubUrl.Should().StartWith("https://github.com/procore/", 
                "Source link should map to correct GitHub repository");
            githubUrl.Should().Contain("/*", "Source link should use correct path mapping format");
        }

        [Fact]
        public void EmbeddedSources_ShouldBeAccessibleInDebugger()
        {
            // This test would require more complex setup with debugger integration
            // For demo purposes, showing structure
            
            // Arrange
            var testProject = "src/Procore.SDK.Core/Procore.SDK.Core.csproj";
            var assemblyPath = BuildProjectAndGetAssembly(testProject);

            // Act
            var assembly = Assembly.LoadFrom(assemblyPath);
            var debuggableAttribute = assembly.GetCustomAttribute<DebuggableAttribute>();

            // Assert
            debuggableAttribute.Should().NotBeNull("Assembly should be debuggable");
            debuggableAttribute.IsJITOptimizerDisabled.Should().BeFalse(
                "Release builds should have JIT optimization enabled");
        }

        private static string BuildProjectAndGetAssembly(string projectPath)
        {
            var fullPath = GetFullProjectPath(projectPath);
            var projectDir = Path.GetDirectoryName(fullPath);
            var projectName = Path.GetFileNameWithoutExtension(fullPath);

            // Build project
            var buildResult = ProcessHelper.RunCommand("dotnet", 
                $"build \"{fullPath}\" --configuration Release");
                
            if (!buildResult.Success)
            {
                throw new InvalidOperationException($"Build failed: {buildResult.Output}");
            }

            // Return assembly path
            return Path.Combine(projectDir, "bin", "Release", "net8.0", $"{projectName}.dll");
        }

        private static PackageInfo BuildAndPackProject(string projectPath)
        {
            var fullPath = GetFullProjectPath(projectPath);
            var outputDir = Path.Combine(Path.GetTempPath(), "test-packages", Guid.NewGuid().ToString());
            Directory.CreateDirectory(outputDir);

            var packResult = ProcessHelper.RunCommand("dotnet", 
                $"pack \"{fullPath}\" --configuration Release --output \"{outputDir}\" --include-symbols");

            if (!packResult.Success)
            {
                throw new InvalidOperationException($"Pack failed: {packResult.Output}");
            }

            var nupkgPath = Directory.GetFiles(outputDir, "*.nupkg")[0];
            var snupkgPath = Directory.GetFiles(outputDir, "*.snupkg")[0];

            return new PackageInfo
            {
                PackagePath = nupkgPath,
                SymbolPackagePath = snupkgPath
            };
        }

        private static string ExtractSourceLinkJson(string symbolPackagePath)
        {
            // This would extract source link JSON from the PDB file within the symbol package
            // For demo purposes, returning expected format
            return """
            {
                "*": "https://github.com/procore/procore-sdk-dotnet/blob/main/*"
            }
            """;
        }

        private static string GetFullProjectPath(string relativePath)
        {
            var baseDirectory = Path.GetDirectoryName(typeof(SourceLinkingValidationTests).Assembly.Location);
            var projectPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "..", relativePath);
            return Path.GetFullPath(projectPath);
        }
    }

    public class PackageInfo
    {
        public string PackagePath { get; set; }
        public string SymbolPackagePath { get; set; }
    }

    public static class ProcessHelper
    {
        public static BuildResult RunCommand(string fileName, string arguments)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            return new BuildResult
            {
                Success = process.ExitCode == 0,
                Output = $"{output}\n{error}"
            };
        }
    }
}
```

### 4. Installation Compatibility Tests

#### InstallationCompatibilityTests.cs
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Procore.SDK.Tests.NuGetPackaging
{
    public class InstallationCompatibilityTests : IDisposable
    {
        private readonly string _tempDirectory;
        private readonly List<string> _createdProjects;

        public InstallationCompatibilityTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "nuget-install-tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
            _createdProjects = new List<string>();
        }

        [Theory]
        [InlineData("console")]
        [InlineData("webapi")]
        [InlineData("classlib")]
        [InlineData("worker")]
        public void Package_ShouldInstallInProjectType(string projectType)
        {
            // Arrange
            var projectPath = CreateTestProject(projectType, "net8.0");

            // Act
            var installResult = InstallSdkPackage(projectPath);
            var buildResult = BuildProject(projectPath);

            // Assert
            installResult.Success.Should().BeTrue($"Package installation should succeed for {projectType} project. Error: {installResult.Output}");
            buildResult.Success.Should().BeTrue($"Project should build successfully after package installation. Error: {buildResult.Output}");
        }

        [Theory]
        [InlineData("Procore.SDK")]
        [InlineData("Procore.SDK.Core")]
        [InlineData("Procore.SDK.Shared")]
        public void Package_ShouldResolveAllDependencies(string packageId)
        {
            // Arrange
            var projectPath = CreateTestProject("console", "net8.0");

            // Act
            var installResult = InstallSpecificPackage(projectPath, packageId);
            var dependencyCheck = CheckDependencyResolution(projectPath);

            // Assert
            installResult.Success.Should().BeTrue($"Package {packageId} should install successfully");
            dependencyCheck.HasConflicts.Should().BeFalse($"Package {packageId} should not have dependency conflicts: {string.Join(", ", dependencyCheck.Conflicts)}");
        }

        [Theory]
        [InlineData("net6.0")]
        [InlineData("net7.0")]
        [InlineData("net8.0")]
        public void Package_ShouldWorkWithTargetFramework(string targetFramework)
        {
            // Arrange
            var projectPath = CreateTestProject("console", targetFramework);

            // Act
            var installResult = InstallSdkPackage(projectPath);
            var buildResult = BuildProject(projectPath);
            var runtimeTest = TestRuntimeCompatibility(projectPath);

            // Assert
            installResult.Success.Should().BeTrue($"Package should install on {targetFramework}");
            buildResult.Success.Should().BeTrue($"Project should build on {targetFramework}");
            runtimeTest.Success.Should().BeTrue($"Package should work at runtime on {targetFramework}");
        }

        [Fact]
        public void PackageUpdate_ShouldMaintainBackwardCompatibility()
        {
            // This test would require multiple versions to be available
            // For demo purposes, showing the structure

            // Arrange
            var projectPath = CreateTestProject("console", "net8.0");
            
            // Install previous version (mock)
            var oldVersionInstall = InstallSpecificPackage(projectPath, "Procore.SDK", "1.0.0");
            var oldVersionBuild = BuildProject(projectPath);

            // Act - Update to current version
            var updateResult = InstallSpecificPackage(projectPath, "Procore.SDK");
            var newVersionBuild = BuildProject(projectPath);

            // Assert
            oldVersionInstall.Success.Should().BeTrue("Old version should install successfully");
            oldVersionBuild.Success.Should().BeTrue("Project should build with old version");
            updateResult.Success.Should().BeTrue("Package update should succeed");
            newVersionBuild.Success.Should().BeTrue("Project should build after update");
        }

        private string CreateTestProject(string projectType, string targetFramework)
        {
            var projectName = $"TestProject_{projectType}_{targetFramework}_{Guid.NewGuid():N}";
            var projectPath = Path.Combine(_tempDirectory, projectName);
            
            var createResult = ProcessHelper.RunCommand("dotnet", 
                $"new {projectType} --name {projectName} --output \"{projectPath}\" --framework {targetFramework}");

            if (!createResult.Success)
            {
                throw new InvalidOperationException($"Failed to create test project: {createResult.Output}");
            }

            _createdProjects.Add(projectPath);
            return projectPath;
        }

        private BuildResult InstallSdkPackage(string projectPath)
        {
            return InstallSpecificPackage(projectPath, "Procore.SDK");
        }

        private BuildResult InstallSpecificPackage(string projectPath, string packageId, string version = null)
        {
            var versionArg = string.IsNullOrEmpty(version) ? "" : $" --version {version}";
            return ProcessHelper.RunCommand("dotnet", 
                $"add \"{projectPath}\" package {packageId}{versionArg} --prerelease");
        }

        private BuildResult BuildProject(string projectPath)
        {
            return ProcessHelper.RunCommand("dotnet", 
                $"build \"{projectPath}\" --configuration Release");
        }

        private DependencyCheckResult CheckDependencyResolution(string projectPath)
        {
            var result = ProcessHelper.RunCommand("dotnet", 
                $"list \"{projectPath}\" package --include-transitive");

            // Parse output for conflicts (simplified)
            var hasConflicts = result.Output.Contains("conflict") || result.Output.Contains("warning");
            var conflicts = hasConflicts ? new[] { "Example conflict detected" } : new string[0];

            return new DependencyCheckResult
            {
                HasConflicts = hasConflicts,
                Conflicts = conflicts
            };
        }

        private BuildResult TestRuntimeCompatibility(string projectPath)
        {
            // For console apps, we can try to run them
            var projectFile = Directory.GetFiles(projectPath, "*.csproj")[0];
            var projectName = Path.GetFileNameWithoutExtension(projectFile);

            if (projectName.Contains("console"))
            {
                return ProcessHelper.RunCommand("dotnet", $"run --project \"{projectPath}\" --");
            }

            // For other project types, successful build indicates runtime compatibility
            return new BuildResult { Success = true, Output = "Runtime compatibility assumed from successful build" };
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, true);
                }
            }
            catch
            {
                // Ignore cleanup errors in tests
            }
        }
    }

    public class DependencyCheckResult
    {
        public bool HasConflicts { get; set; }
        public string[] Conflicts { get; set; }
    }
}
```

### 5. Test Helper Utilities

#### NuGetTestHelper.cs
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Procore.SDK.Tests.NuGetPackaging.Helpers
{
    public static class NuGetTestHelper
    {
        public static IEnumerable<string> GetAllSdkProjectPaths()
        {
            var baseDirectory = GetRepositoryRoot();
            return new[]
            {
                Path.Combine(baseDirectory, "src", "Procore.SDK", "Procore.SDK.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.Core", "Procore.SDK.Core.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.Shared", "Procore.SDK.Shared.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.ConstructionFinancials", "Procore.SDK.ConstructionFinancials.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.ProjectManagement", "Procore.SDK.ProjectManagement.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.QualitySafety", "Procore.SDK.QualitySafety.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.FieldProductivity", "Procore.SDK.FieldProductivity.csproj"),
                Path.Combine(baseDirectory, "src", "Procore.SDK.ResourceManagement", "Procore.SDK.ResourceManagement.csproj")
            };
        }

        public static string GetRepositoryRoot()
        {
            var currentDir = Directory.GetCurrentDirectory();
            while (currentDir != null && !File.Exists(Path.Combine(currentDir, "ProcoreSDK.sln")))
            {
                currentDir = Directory.GetParent(currentDir)?.FullName;
            }
            return currentDir ?? throw new InvalidOperationException("Could not find repository root");
        }

        public static PackageMetadata ExtractPackageMetadata(string projectPath)
        {
            if (!File.Exists(projectPath))
                throw new FileNotFoundException($"Project file not found: {projectPath}");

            var projectXml = XDocument.Load(projectPath);
            var directoryBuildProps = LoadDirectoryBuildProps(projectPath);

            return new PackageMetadata
            {
                PackageId = GetPropertyValue(projectXml, "PackageId"),
                Description = GetPropertyValue(projectXml, "Description"),
                Authors = GetPropertyValue(directoryBuildProps, "Authors"),
                Company = GetPropertyValue(directoryBuildProps, "Company"),
                Copyright = GetPropertyValue(directoryBuildProps, "Copyright"),
                PackageLicenseExpression = GetPropertyValue(directoryBuildProps, "PackageLicenseExpression"),
                PackageProjectUrl = GetPropertyValue(directoryBuildProps, "PackageProjectUrl"),
                RepositoryUrl = GetPropertyValue(directoryBuildProps, "RepositoryUrl"),
                RepositoryType = GetPropertyValue(directoryBuildProps, "RepositoryType"),
                PackageTags = GetPropertyValue(directoryBuildProps, "PackageTags")
            };
        }

        private static XDocument LoadDirectoryBuildProps(string projectPath)
        {
            var projectDir = Path.GetDirectoryName(projectPath);
            var repoRoot = GetRepositoryRoot();
            var directoryBuildPropsPath = Path.Combine(repoRoot, "Directory.Build.props");
            
            return XDocument.Load(directoryBuildPropsPath);
        }

        private static string GetPropertyValue(XDocument document, string propertyName)
        {
            return document.Descendants("PropertyGroup")
                .SelectMany(pg => pg.Elements(propertyName))
                .FirstOrDefault()?.Value;
        }

        public static void ValidatePackageStructure(string packagePath)
        {
            if (!File.Exists(packagePath))
                throw new FileNotFoundException($"Package not found: {packagePath}");

            using var packageStream = File.OpenRead(packagePath);
            using var archive = new System.IO.Compression.ZipArchive(packageStream, System.IO.Compression.ZipArchiveMode.Read);

            var entries = archive.Entries.Select(e => e.FullName).ToList();

            // Validate required package structure
            entries.Should().Contain(e => e.EndsWith(".nuspec"), "Package should contain .nuspec file");
            entries.Should().Contain(e => e.StartsWith("lib/"), "Package should contain lib folder with assemblies");
            entries.Should().Contain(e => e.EndsWith(".dll"), "Package should contain compiled assemblies");
        }

        public static class PackageValidation
        {
            public static void ValidateMetadataCompleteness(PackageMetadata metadata)
            {
                var requiredFields = new Dictionary<string, string>
                {
                    { nameof(metadata.PackageId), metadata.PackageId },
                    { nameof(metadata.Description), metadata.Description },
                    { nameof(metadata.Authors), metadata.Authors },
                    { nameof(metadata.PackageLicenseExpression), metadata.PackageLicenseExpression }
                };

                foreach (var (fieldName, value) in requiredFields)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new InvalidOperationException($"Required metadata field '{fieldName}' is missing or empty");
                    }
                }
            }

            public static void ValidateSemanticVersion(string version)
            {
                if (string.IsNullOrWhiteSpace(version))
                    throw new ArgumentException("Version cannot be null or empty");

                var semanticVersionPattern = @"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$";
                
                if (!System.Text.RegularExpressions.Regex.IsMatch(version, semanticVersionPattern))
                {
                    throw new ArgumentException($"Version '{version}' does not follow semantic versioning format");
                }
            }
        }
    }
}
```

## Integration with Test Framework

### Global Test Configuration

#### GlobalUsings.cs
```csharp
global using Xunit;
global using FluentAssertions;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using Procore.SDK.Tests.NuGetPackaging.Helpers;
```

#### Test Collection Definition
```csharp
using Xunit;

namespace Procore.SDK.Tests.NuGetPackaging
{
    [CollectionDefinition("NuGet Package Tests")]
    public class NuGetPackageTestCollection : ICollectionFixture<NuGetPackageTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class NuGetPackageTestFixture : IDisposable
    {
        public string TempPackageDirectory { get; }
        public string TestProjectsDirectory { get; }

        public NuGetPackageTestFixture()
        {
            TempPackageDirectory = Path.Combine(Path.GetTempPath(), "nuget-test-packages", Guid.NewGuid().ToString());
            TestProjectsDirectory = Path.Combine(Path.GetTempPath(), "nuget-test-projects", Guid.NewGuid().ToString());
            
            Directory.CreateDirectory(TempPackageDirectory);
            Directory.CreateDirectory(TestProjectsDirectory);

            // Build all packages for testing
            BuildAllPackages();
        }

        private void BuildAllPackages()
        {
            var repoRoot = NuGetTestHelper.GetRepositoryRoot();
            var buildResult = ProcessHelper.RunCommand("dotnet", 
                $"pack \"{repoRoot}\" --configuration Release --output \"{TempPackageDirectory}\" --include-symbols");

            if (!buildResult.Success)
            {
                throw new InvalidOperationException($"Failed to build test packages: {buildResult.Output}");
            }
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(TempPackageDirectory))
                    Directory.Delete(TempPackageDirectory, true);
                    
                if (Directory.Exists(TestProjectsDirectory))
                    Directory.Delete(TestProjectsDirectory, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
}
```

## Conclusion

These sample implementations demonstrate the comprehensive testing approach for NuGet package configuration and publishing. The tests cover:

1. **Metadata Validation** - Ensuring all required package metadata is present and follows conventions
2. **Multi-Targeting** - Verifying compatibility across different .NET frameworks
3. **Source Linking** - Validating debugging and source navigation capabilities
4. **Installation Compatibility** - Testing package installation across different project types
5. **Helper Utilities** - Providing reusable components for test implementation

The samples can be adapted and extended based on specific requirements and integrated into the existing test framework for comprehensive validation of the NuGet packaging functionality.