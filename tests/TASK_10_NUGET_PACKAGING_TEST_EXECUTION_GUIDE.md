# Task 10: NuGet Package Configuration & Publishing - Test Execution Guide

## Quick Start

This guide provides step-by-step instructions for executing the comprehensive NuGet packaging test strategy across all Procore SDK projects.

## Prerequisites

### Development Environment
- **.NET 8.0 SDK** - Primary development framework
- **.NET 6.0 and 7.0 SDKs** - For multi-targeting verification
- **PowerShell Core 7+** - For automation scripts
- **Git** - For source linking validation
- **NuGet CLI** - For package validation tools

### Required Tools
```bash
# Install .NET SDKs
winget install Microsoft.DotNet.SDK.8
winget install Microsoft.DotNet.SDK.7  
winget install Microsoft.DotNet.SDK.6

# Install NuGet CLI (Windows)
winget install Microsoft.NuGet

# Install PowerShell Core (if needed)
winget install Microsoft.PowerShell

# Verify installations
dotnet --list-sdks
nuget help
pwsh --version
```

### Environment Variables
```bash
# Required for source linking tests
export PROCORE_SDK_REPO_URL="https://github.com/procore/procore-sdk-dotnet"
export PROCORE_SDK_COMMIT_SHA=$(git rev-parse HEAD)

# Optional: Test NuGet server (for publishing tests)
export TEST_NUGET_SERVER="https://apitest.nugettest.org/v3/index.json"
export TEST_NUGET_API_KEY="your-test-api-key"
```

## Test Execution Phases

### Phase 1: Pre-Execution Validation

#### 1.1 Project Structure Validation
```bash
# Navigate to repository root
cd /path/to/procore-sdk

# Verify all SDK projects exist
ls src/
# Expected output:
# Procore.SDK/
# Procore.SDK.Core/
# Procore.SDK.Shared/
# Procore.SDK.ConstructionFinancials/
# Procore.SDK.ProjectManagement/
# Procore.SDK.QualitySafety/
# Procore.SDK.FieldProductivity/
# Procore.SDK.ResourceManagement/

# Verify build configuration
ls Directory.Build.props Directory.Packages.props
```

#### 1.2 Clean Build Environment
```bash
# Clean all build artifacts
dotnet clean
rm -rf **/bin **/obj

# Restore packages
dotnet restore
```

### Phase 2: Package Metadata Validation Tests

#### 2.1 Execute Metadata Validation
```bash
# Run package metadata validation tests
dotnet test tests/Procore.SDK.Tests/ -k PackageMetadataValidationTests --logger "console;verbosity=detailed"

# Expected test cases:
# ✅ PackageMetadata_ShouldHaveRequiredFields
# ✅ PackageId_ShouldFollowNamingConvention  
# ✅ PackageDescription_ShouldMeetQualityStandards
# ✅ PackageVersions_ShouldBeConsistentAcrossProjects
```

#### 2.2 Manual Metadata Verification
```bash
# Check specific project metadata
cd src/Procore.SDK.Core
dotnet pack --configuration Release --output ../../packages

# Extract and inspect package metadata
cd ../../packages  
unzip -l Procore.SDK.Core.*.nupkg | grep -E '\.nuspec|\.dll|\.xml'

# Validate .nuspec content
unzip -p Procore.SDK.Core.*.nupkg Procore.SDK.Core.nuspec | xmllint --format -
```

### Phase 3: Multi-Targeting Verification Tests

#### 3.1 Configure Multi-Targeting
```xml
<!-- Add to Directory.Build.props for testing -->
<PropertyGroup Condition="'$(TestMultiTargeting)' == 'true'">
  <TargetFrameworks>net6.0;net7.0;net8.0</TargetFrameworks>
</PropertyGroup>
```

#### 3.2 Execute Multi-Targeting Tests
```bash
# Enable multi-targeting for tests
export TestMultiTargeting=true

# Build all projects with multi-targeting
dotnet build --configuration Release

# Run multi-targeting validation tests
dotnet test tests/Procore.SDK.Tests/ -k MultiTargetingValidationTests --logger "console;verbosity=detailed"

# Verify framework-specific builds
find src/*/bin/Release -name "*.dll" | grep -E 'net[678]\.0'
```

#### 3.3 API Compatibility Verification
```bash
# Install Microsoft.DotNet.ApiCompat (if needed)
dotnet tool install -g Microsoft.DotNet.ApiCompat

# Compare API surface across frameworks
for project in src/Procore.SDK.*/*.csproj; do
    echo "Checking API compatibility for $(basename $(dirname $project))"
    # Compare net6.0 vs net8.0 builds
    dotnet run --project tools/ApiCompatibilityChecker.csproj -- \
        "$(dirname $project)/bin/Release/net6.0/*.dll" \
        "$(dirname $project)/bin/Release/net8.0/*.dll"
done
```

### Phase 4: Source Linking Functionality Tests

#### 4.1 Execute Source Linking Tests
```bash
# Run source linking validation tests
dotnet test tests/Procore.SDK.Tests/ -k SourceLinkingValidationTests --logger "console;verbosity=detailed"

# Expected test cases:
# ✅ Assembly_ShouldContainSourceLinkInformation
# ✅ SymbolPackage_ShouldBeGenerated
# ✅ SourceLink_ShouldMapToCorrectGitHubUrls
# ✅ EmbeddedSources_ShouldBeAccessibleInDebugger
```

#### 4.2 Manual Source Link Verification
```bash
# Build with symbols
dotnet pack src/Procore.SDK.Core/ --configuration Release --include-symbols --output packages/

# Verify symbol package creation
ls packages/*.snupkg

# Install sourcelink CLI tool
dotnet tool install -g sourcelink

# Test source link information
sourcelink test packages/Procore.SDK.Core.*.nupkg
```

### Phase 5: Symbols and Documentation Tests

#### 5.1 Execute Documentation Tests
```bash
# Run documentation validation tests
dotnet test tests/Procore.SDK.Tests/ -k SymbolsAndDocumentationTests --logger "console;verbosity=detailed"

# Check XML documentation generation
find src/*/bin/Release -name "*.xml" | head -5
```

#### 5.2 Manual Documentation Verification
```bash
# Extract and verify XML documentation
cd packages
unzip -l Procore.SDK.Core.*.nupkg | grep "\.xml$"

# Check documentation completeness
unzip -p Procore.SDK.Core.*.nupkg lib/net8.0/Procore.SDK.Core.xml | \
    grep -c "<member name=" || echo "No documentation found"
```

#### 5.3 Symbol Package Verification
```bash
# Verify symbol package contents
unzip -l Procore.SDK.Core.*.snupkg

# Check PDB files
unzip -p Procore.SDK.Core.*.snupkg lib/net8.0/Procore.SDK.Core.pdb | \
    file - | grep "Microsoft Roslyn"
```

### Phase 6: Validation Pipeline Tests

#### 6.1 Execute Pipeline Tests
```bash
# Run validation pipeline tests
dotnet test tests/Procore.SDK.Tests/ -k ValidationPipelineTests --logger "console;verbosity=detailed"

# Expected test stages:
# ✅ PreBuildValidation_ShouldValidateProjectConfiguration
# ✅ BuildValidation_ShouldSucceedWithoutWarnings
# ✅ PackValidation_ShouldGenerateValidPackages
# ✅ PackageValidation_ShouldPassNuGetValidation
```

#### 6.2 Manual Pipeline Execution
```bash
# Execute full build pipeline
pwsh tools/Build-Integration.ps1 -Configuration Release

# Verify package generation
ls packages/*.nupkg | wc -l
# Expected: 8 packages (one for each SDK project)

# Run NuGet package validation
for package in packages/*.nupkg; do
    echo "Validating $package"
    nuget verify -Signatures $package
done
```

### Phase 7: Publishing Workflow Tests

#### 7.1 Execute Publishing Tests
```bash
# Run publishing workflow tests (with mocked endpoints)
dotnet test tests/Procore.SDK.Tests/ -k PublishingWorkflowTests --logger "console;verbosity=detailed"

# Expected test cases:
# ✅ LocalPack_ShouldGenerateValidPackagesForAllProjects
# ✅ PackageUpload_ShouldSucceedWithValidCredentials
# ✅ VersionIncrement_ShouldUpdateAllRelatedProjects
# ✅ SemanticVersioning_ShouldIncrementCorrectly
# ✅ SecurityScanning_ShouldIdentifyVulnerabilities
```

#### 7.2 Manual Publishing Simulation
```bash
# Simulate package publishing (dry run)
for package in packages/*.nupkg; do
    echo "Simulating publish for $package"
    nuget push $package -Source $TEST_NUGET_SERVER -ApiKey $TEST_NUGET_API_KEY -DryRun
done
```

#### 7.3 Security Scanning
```bash
# Install security scanning tools
dotnet tool install -g Microsoft.CST.DevSkim.CLI

# Scan for security issues
devskim analyze src/ --output-file security-scan-results.json

# Check for vulnerable dependencies
dotnet list package --vulnerable --include-transitive > vulnerability-report.txt
```

### Phase 8: Installation Testing

#### 8.1 Execute Installation Tests
```bash
# Run installation compatibility tests
dotnet test tests/Procore.SDK.Tests/ -k InstallationCompatibilityTests --logger "console;verbosity=detailed"

# Expected test scenarios:
# ✅ Package_ShouldInstallInProjectType[console]
# ✅ Package_ShouldInstallInProjectType[webapi]
# ✅ Package_ShouldInstallInProjectType[classlib]
# ✅ Package_ShouldInstallInProjectType[worker]
# ✅ Package_ShouldResolveAllDependencies
# ✅ Package_ShouldWorkWithTargetFramework
# ✅ PackageUpdate_ShouldMaintainBackwardCompatibility
```

#### 8.2 Manual Installation Testing

##### 8.2.1 Console Application Test
```bash
# Create test console application
mkdir test-installations/console-test
cd test-installations/console-test

dotnet new console --name TestConsoleApp --framework net8.0
cd TestConsoleApp

# Add local package source
dotnet nuget add source ../../../packages --name LocalPackages

# Install Procore SDK
dotnet add package Procore.SDK --prerelease

# Verify installation
dotnet build
dotnet run
```

##### 8.2.2 Web API Test
```bash
# Create test web API
mkdir ../webapi-test
cd ../webapi-test

dotnet new webapi --name TestWebApi --framework net8.0
cd TestWebApi

# Add local package source and install SDK
dotnet nuget add source ../../../packages --name LocalPackages
dotnet add package Procore.SDK --prerelease

# Verify installation and basic functionality
dotnet build
dotnet run --urls http://localhost:5000 &
curl http://localhost:5000/health
pkill -f TestWebApi
```

##### 8.2.3 Class Library Test
```bash
# Create test class library
mkdir ../classlib-test
cd ../classlib-test

dotnet new classlib --name TestClassLib --framework net8.0
cd TestClassLib

# Add local package source and install SDK
dotnet nuget add source ../../../packages --name LocalPackages
dotnet add package Procore.SDK --prerelease

# Verify installation
dotnet build
```

### Phase 9: Performance and Quality Validation

#### 9.1 Performance Benchmarks
```bash
# Run performance benchmarks
dotnet run --project tests/Procore.SDK.Benchmarks/ --configuration Release

# Check package sizes
du -h packages/*.nupkg | sort -hr

# Measure installation time
time dotnet add package Procore.SDK --source packages --prerelease
```

#### 9.2 Quality Metrics Collection
```bash
# Generate test coverage report
dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults/

# Generate coverage report
reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" \
                 -targetdir:"coverage-report" \
                 -reporttypes:"Html;TextSummary"

# Check coverage metrics
cat coverage-report/Summary.txt
```

## Test Results Interpretation

### Success Criteria

#### Package Metadata ✅
- All required metadata fields present and valid
- Consistent versioning across all packages
- Professional descriptions and proper tagging

#### Multi-Targeting ✅
- Successful compilation for all target frameworks
- Consistent API surface across frameworks
- Proper framework-specific dependencies

#### Source Linking ✅
- Source link information embedded in assemblies
- Symbol packages (.snupkg) generated correctly
- GitHub URL mapping functional

#### Documentation ✅
- XML documentation generated for all public APIs
- Documentation files included in packages
- IntelliSense support verified

#### Installation ✅
- Successful installation in various project types
- Dependency resolution without conflicts
- Cross-platform compatibility confirmed

### Common Issues and Troubleshooting

#### Issue: Package Metadata Missing
```bash
# Check project file configuration
grep -r "PackageId\|Description" src/*/

# Verify Directory.Build.props inheritance
dotnet msbuild src/Procore.SDK.Core/ -p:Configuration=Release -t:Pack -v:diagnostic | grep -i package
```

#### Issue: Source Linking Not Working
```bash
# Verify source link package reference
grep -r "Microsoft.SourceLink.GitHub" .

# Check repository URL configuration
git remote -v

# Validate source link data
sourcelink print-json packages/Procore.SDK.Core.*.nupkg
```

#### Issue: Multi-Targeting Compilation Errors
```bash
# Check framework-specific code issues
dotnet build --framework net6.0 --verbosity detailed
dotnet build --framework net8.0 --verbosity detailed

# Verify package references support all frameworks
dotnet restore --verbosity detailed | grep -i "framework"
```

#### Issue: Installation Failures
```bash
# Check dependency conflicts
dotnet add package Procore.SDK --dry-run --verbosity detailed

# Verify package structure
unzip -l packages/Procore.SDK.*.nupkg | grep -E "lib/|dependencies"

# Test with clean project
rm -rf ~/.nuget/packages/Procore.SDK*
dotnet nuget locals all --clear
```

## Automated Test Execution

### Continuous Integration Script
```bash
#!/bin/bash
# ci-test-nuget-packaging.sh

set -e

echo "🚀 Starting NuGet Package Testing Pipeline"

# Phase 1: Environment Setup
echo "📋 Phase 1: Environment Setup"
dotnet --version
nuget help | head -1

# Phase 2: Build and Pack
echo "📦 Phase 2: Build and Pack"
dotnet clean
dotnet restore
dotnet build --configuration Release
dotnet pack --configuration Release --output packages --include-symbols

# Phase 3: Package Validation
echo "✅ Phase 3: Package Validation"
dotnet test tests/Procore.SDK.Tests/ -k "PackageMetadata or MultiTargeting or SourceLinking" \
    --logger "trx;LogFileName=package-validation-results.trx"

# Phase 4: Installation Testing
echo "🔧 Phase 4: Installation Testing"
dotnet test tests/Procore.SDK.Tests/ -k "InstallationCompatibilityTests" \
    --logger "trx;LogFileName=installation-test-results.trx"

# Phase 5: Security Scanning
echo "🛡️ Phase 5: Security Scanning"
dotnet list package --vulnerable --include-transitive > vulnerability-report.txt

echo "✨ NuGet Package Testing Pipeline Completed Successfully"
```

### PowerShell Automation Script
```powershell
# NuGet-Package-Tests.ps1
param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("All", "Metadata", "MultiTarget", "SourceLink", "Installation")]
    [string]$TestCategory = "All",
    
    [Parameter(Mandatory = $false)]
    [switch]$GenerateReport
)

function Write-TestPhase($Message) {
    Write-Host "🔍 $Message" -ForegroundColor Cyan
}

function Write-Success($Message) {
    Write-Host "✅ $Message" -ForegroundColor Green
}

function Write-Warning($Message) {
    Write-Host "⚠️ $Message" -ForegroundColor Yellow
}

# Main execution
Write-TestPhase "Starting NuGet Package Testing"

switch ($TestCategory) {
    "All" { 
        & dotnet test tests/Procore.SDK.Tests/ --logger "console;verbosity=detailed"
    }
    "Metadata" { 
        & dotnet test tests/Procore.SDK.Tests/ -k PackageMetadataValidationTests
    }
    "MultiTarget" { 
        & dotnet test tests/Procore.SDK.Tests/ -k MultiTargetingValidationTests
    }
    "SourceLink" { 
        & dotnet test tests/Procore.SDK.Tests/ -k SourceLinkingValidationTests
    }
    "Installation" { 
        & dotnet test tests/Procore.SDK.Tests/ -k InstallationCompatibilityTests
    }
}

if ($GenerateReport) {
    Write-TestPhase "Generating Test Report"
    & reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:"test-report" -reporttypes:"Html"
    Write-Success "Test report generated in test-report/"
}

Write-Success "NuGet Package Testing Completed"
```

## Conclusion

This test execution guide provides comprehensive instructions for validating NuGet package configuration and publishing functionality. The guide covers both automated and manual testing approaches, enabling thorough validation of package quality, compatibility, and installation scenarios.

Regular execution of these tests ensures the reliability and quality of the Procore SDK NuGet packages across all supported platforms and frameworks.