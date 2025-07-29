#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Validates NuGet package configuration for all Procore SDK projects
.DESCRIPTION
    This script validates package metadata, compilation, and packaging for all SDK projects
.PARAMETER Configuration
    Build configuration (Debug or Release)
.PARAMETER OutputPath
    Output path for generated packages
.EXAMPLE
    ./validate-packages.ps1 -Configuration Release
#>

param(
    [string]$Configuration = "Release",
    [string]$OutputPath = "./artifacts/packages"
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

# Colors for output
$Red = "`e[31m"
$Green = "`e[32m"
$Yellow = "`e[33m"
$Blue = "`e[34m"
$Reset = "`e[0m"

function Write-Header($Message) {
    Write-Host "${Blue}========================================${Reset}"
    Write-Host "${Blue}$Message${Reset}"
    Write-Host "${Blue}========================================${Reset}"
}

function Write-Success($Message) {
    Write-Host "${Green}✓ $Message${Reset}"
}

function Write-Warning($Message) {
    Write-Host "${Yellow}⚠ $Message${Reset}"
}

function Write-Error($Message) {
    Write-Host "${Red}✗ $Message${Reset}"
}

function Write-Info($Message) {
    Write-Host "${Blue}ℹ $Message${Reset}"
}

# Get solution root
$SolutionRoot = Resolve-Path "$PSScriptRoot/.."
$SrcPath = Join-Path $SolutionRoot "src"

# SDK Projects to validate
$SdkProjects = @(
    "Procore.SDK",
    "Procore.SDK.Core", 
    "Procore.SDK.Shared",
    "Procore.SDK.ProjectManagement",
    "Procore.SDK.QualitySafety",
    "Procore.SDK.ConstructionFinancials",
    "Procore.SDK.FieldProductivity",
    "Procore.SDK.ResourceManagement"
)

# Validation results
$ValidationResults = @()

Write-Header "Procore SDK Package Validation"

# Create output directory
$OutputDir = Join-Path $SolutionRoot $OutputPath
if (!(Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
    Write-Info "Created output directory: $OutputDir"
}

# Step 1: Validate Project Files
Write-Header "Step 1: Validating Project Files"

foreach ($Project in $SdkProjects) {
    $ProjectPath = Join-Path $SrcPath "$Project/$Project.csproj"
    
    if (!(Test-Path $ProjectPath)) {
        Write-Error "Project file not found: $ProjectPath"
        $ValidationResults += @{
            Project = $Project
            Step = "ProjectFile"
            Status = "Failed"
            Message = "Project file not found"
        }
        continue
    }
    
    # Load and validate project XML
    try {
        $ProjectXml = [xml](Get-Content $ProjectPath)
        $Properties = $ProjectXml.Project.PropertyGroup
        
        # Check required properties
        $RequiredProperties = @("PackageId", "Description")
        $MissingProperties = @()
        
        foreach ($PropGroup in $Properties) {
            foreach ($RequiredProp in $RequiredProperties) {
                if ($PropGroup.$RequiredProp) {
                    $RequiredProperties = $RequiredProperties | Where-Object { $_ -ne $RequiredProp }
                }
            }
        }
        
        if ($RequiredProperties.Count -eq 0) {
            Write-Success "Project validation passed: $Project"
            $ValidationResults += @{
                Project = $Project
                Step = "ProjectFile"
                Status = "Passed"
                Message = "All required properties present"
            }
        } else {
            Write-Warning "Project validation warnings: $Project - Missing: $($RequiredProperties -join ', ')"
            $ValidationResults += @{
                Project = $Project
                Step = "ProjectFile"
                Status = "Warning"
                Message = "Missing properties: $($RequiredProperties -join ', ')"
            }
        }
    }
    catch {
        Write-Error "Failed to parse project file: $Project - $($_.Exception.Message)"
        $ValidationResults += @{
            Project = $Project
            Step = "ProjectFile"
            Status = "Failed"
            Message = "Failed to parse project file: $($_.Exception.Message)"
        }
    }
}

# Step 2: Build Validation
Write-Header "Step 2: Build Validation"

foreach ($Project in $SdkProjects) {
    $ProjectPath = Join-Path $SrcPath "$Project/$Project.csproj"
    
    if (!(Test-Path $ProjectPath)) {
        continue
    }
    
    Write-Info "Building project: $Project"
    
    try {
        $BuildOutput = dotnet build $ProjectPath --configuration $Configuration --verbosity quiet --nologo 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Build passed: $Project"
            $ValidationResults += @{
                Project = $Project
                Step = "Build"
                Status = "Passed"
                Message = "Build successful"
            }
        } else {
            Write-Error "Build failed: $Project"
            Write-Host $BuildOutput
            $ValidationResults += @{
                Project = $Project
                Step = "Build"
                Status = "Failed"
                Message = "Build failed with exit code $LASTEXITCODE"
            }
        }
    }
    catch {
        Write-Error "Build error: $Project - $($_.Exception.Message)"
        $ValidationResults += @{
            Project = $Project
            Step = "Build"
            Status = "Failed"
            Message = "Build error: $($_.Exception.Message)"
        }
    }
}

# Step 3: Package Generation
Write-Header "Step 3: Package Generation"

foreach ($Project in $SdkProjects) {
    $ProjectPath = Join-Path $SrcPath "$Project/$Project.csproj"
    
    if (!(Test-Path $ProjectPath)) {
        continue
    }
    
    Write-Info "Creating package: $Project"
    
    try {
        $PackOutput = dotnet pack $ProjectPath --configuration $Configuration --output $OutputDir --verbosity quiet --nologo 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Package created: $Project"
            $ValidationResults += @{
                Project = $Project
                Step = "Package"
                Status = "Passed"
                Message = "Package created successfully"
            }
        } else {
            Write-Error "Package creation failed: $Project"
            Write-Host $PackOutput
            $ValidationResults += @{
                Project = $Project
                Step = "Package"
                Status = "Failed"
                Message = "Package creation failed with exit code $LASTEXITCODE"
            }
        }
    }
    catch {
        Write-Error "Package error: $Project - $($_.Exception.Message)"
        $ValidationResults += @{
            Project = $Project
            Step = "Package"
            Status = "Failed"
            Message = "Package error: $($_.Exception.Message)"
        }
    }
}

# Step 4: Package Content Validation
Write-Header "Step 4: Package Content Validation"

$PackageFiles = Get-ChildItem -Path $OutputDir -Filter "*.nupkg" | Where-Object { $_.Name -notlike "*symbols*" }

foreach ($PackageFile in $PackageFiles) {
    $PackageName = [System.IO.Path]::GetFileNameWithoutExtension($PackageFile.Name)
    Write-Info "Validating package content: $PackageName"
    
    try {
        # Extract package contents (nupkg is a zip file)
        $TempDir = Join-Path $env:TEMP ([Guid]::NewGuid().ToString())
        Expand-Archive -Path $PackageFile.FullName -DestinationPath $TempDir -Force
        
        # Check for required files
        $NuspecFile = Get-ChildItem -Path $TempDir -Filter "*.nuspec" | Select-Object -First 1
        if ($NuspecFile) {
            $NuspecContent = [xml](Get-Content $NuspecFile.FullName)
            $Metadata = $NuspecContent.package.metadata
            
            # Validate metadata
            $RequiredMetadata = @("id", "version", "authors", "description")
            $MissingMetadata = @()
            
            foreach ($RequiredField in $RequiredMetadata) {
                if (!$Metadata.$RequiredField -or [string]::IsNullOrWhiteSpace($Metadata.$RequiredField)) {
                    $MissingMetadata += $RequiredField
                }
            }
            
            if ($MissingMetadata.Count -eq 0) {
                Write-Success "Package metadata validation passed: $PackageName"
                $ValidationResults += @{
                    Project = $PackageName
                    Step = "PackageContent"
                    Status = "Passed"
                    Message = "All required metadata present"
                }
            } else {
                Write-Warning "Package metadata warnings: $PackageName - Missing: $($MissingMetadata -join ', ')"
                $ValidationResults += @{
                    Project = $PackageName
                    Step = "PackageContent"
                    Status = "Warning"
                    Message = "Missing metadata: $($MissingMetadata -join ', ')"
                }
            }
            
            # Check for lib folder and assemblies
            $LibFolder = Join-Path $TempDir "lib"
            if (Test-Path $LibFolder) {
                $Assemblies = Get-ChildItem -Path $LibFolder -Filter "*.dll" -Recurse
                if ($Assemblies.Count -gt 0) {
                    Write-Success "Package assemblies found: $PackageName ($($Assemblies.Count) assemblies)"
                } else {
                    Write-Warning "No assemblies found in package: $PackageName"
                }
            } else {
                Write-Warning "No lib folder found in package: $PackageName"
            }
        } else {
            Write-Error "No nuspec file found in package: $PackageName"
            $ValidationResults += @{
                Project = $PackageName
                Step = "PackageContent"
                Status = "Failed"
                Message = "No nuspec file found"
            }
        }
        
        # Cleanup
        Remove-Item -Path $TempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
    catch {
        Write-Error "Package content validation error: $PackageName - $($_.Exception.Message)"
        $ValidationResults += @{
            Project = $PackageName
            Step = "PackageContent"
            Status = "Failed"
            Message = "Package content validation error: $($_.Exception.Message)"
        }
    }
}

# Step 5: Multi-targeting Validation
Write-Header "Step 5: Multi-targeting Validation"

foreach ($Project in $SdkProjects) {
    $ProjectPath = Join-Path $SrcPath "$Project/$Project.csproj"
    
    if (!(Test-Path $ProjectPath)) {
        continue
    }
    
    try {
        $ProjectXml = [xml](Get-Content $ProjectPath)
        $Properties = $ProjectXml.Project.PropertyGroup
        
        $HasMultiTargeting = $false
        foreach ($PropGroup in $Properties) {
            if ($PropGroup.TargetFrameworks -or $PropGroup.TargetFramework) {
                $HasMultiTargeting = $true
                break
            }
        }
        
        if ($HasMultiTargeting) {
            Write-Success "Multi-targeting configured: $Project"
            $ValidationResults += @{
                Project = $Project
                Step = "MultiTargeting"
                Status = "Passed"
                Message = "Multi-targeting configured"
            }
        } else {
            Write-Warning "No target framework specified: $Project"
            $ValidationResults += @{
                Project = $Project
                Step = "MultiTargeting"
                Status = "Warning"
                Message = "No target framework specified"
            }
        }
    }
    catch {
        Write-Error "Multi-targeting validation error: $Project - $($_.Exception.Message)"
        $ValidationResults += @{
            Project = $Project
            Step = "MultiTargeting"
            Status = "Failed"
            Message = "Multi-targeting validation error: $($_.Exception.Message)"
        }
    }
}

# Results Summary
Write-Header "Validation Results Summary"

$Passed = ($ValidationResults | Where-Object { $_.Status -eq "Passed" }).Count
$Warnings = ($ValidationResults | Where-Object { $_.Status -eq "Warning" }).Count  
$Failed = ($ValidationResults | Where-Object { $_.Status -eq "Failed" }).Count
$Total = $ValidationResults.Count

Write-Info "Total validations: $Total"
Write-Success "Passed: $Passed"
Write-Warning "Warnings: $Warnings"
Write-Error "Failed: $Failed"

# Detailed results
if ($Failed -gt 0 -or $Warnings -gt 0) {
    Write-Header "Detailed Results"
    
    foreach ($Result in $ValidationResults | Where-Object { $_.Status -ne "Passed" }) {
        $Status = $Result.Status
        $Color = if ($Status -eq "Failed") { $Red } elseif ($Status -eq "Warning") { $Yellow } else { $Green }
        Write-Host "${Color}[$Status] $($Result.Project) - $($Result.Step): $($Result.Message)${Reset}"
    }
}

# Package information
if (Test-Path $OutputDir) {
    Write-Header "Generated Packages"
    $Packages = Get-ChildItem -Path $OutputDir -Filter "*.nupkg"
    foreach ($Package in $Packages) {
        $Size = [math]::Round($Package.Length / 1MB, 2)
        Write-Info "$($Package.Name) ($Size MB)"
    }
    
    Write-Info "Packages saved to: $OutputDir"
}

# Exit with appropriate code
if ($Failed -gt 0) {
    Write-Error "Validation failed with $Failed failures"
    exit 1
} elseif ($Warnings -gt 0) {
    Write-Warning "Validation completed with $Warnings warnings"
    exit 0
} else {
    Write-Success "All validations passed successfully!"
    exit 0
}