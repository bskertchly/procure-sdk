#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Integrates Kiota client generation into the build process

.DESCRIPTION
    This script can be used to automatically generate Kiota clients as part of the build process.
    It checks if the OpenAPI spec has been updated and regenerates clients if needed.

.PARAMETER Force
    Force regeneration even if OpenAPI spec hasn't changed

.PARAMETER SkipGeneration
    Skip client generation and only build existing code

.PARAMETER Configuration
    Build configuration (Debug/Release, default: Debug)

.EXAMPLE
    ./Build-Integration.ps1
    
.EXAMPLE
    ./Build-Integration.ps1 -Force -Configuration Release
#>

param(
    [Parameter(Mandatory = $false)]
    [switch]$Force,
    
    [Parameter(Mandatory = $false)]
    [switch]$SkipGeneration,
    
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

# Color output functions
function Write-Success { param($Message) Write-Host "✅ $Message" -ForegroundColor Green }
function Write-Warning { param($Message) Write-Host "⚠️  $Message" -ForegroundColor Yellow }
function Write-Error { param($Message) Write-Host "❌ $Message" -ForegroundColor Red }
function Write-Info { param($Message) Write-Host "ℹ️  $Message" -ForegroundColor Cyan }

# Configuration
$OpenApiSpec = "docs/rest_OAS_all.json"
$GenerationMarkerFile = ".last-generation"
$GenerationScript = "tools/Generate-Clients.ps1"

function Test-ShouldRegenerate {
    Write-Info "Checking if regeneration is needed..."
    
    if ($Force) {
        Write-Info "Force flag specified - regeneration required"
        return $true
    }
    
    if (!(Test-Path $OpenApiSpec)) {
        Write-Error "OpenAPI specification not found: $OpenApiSpec"
        return $false
    }
    
    if (!(Test-Path $GenerationMarkerFile)) {
        Write-Info "No previous generation found - regeneration required"
        return $true
    }
    
    $specLastWrite = (Get-Item $OpenApiSpec).LastWriteTime
    $markerLastWrite = (Get-Item $GenerationMarkerFile).LastWriteTime
    
    if ($specLastWrite -gt $markerLastWrite) {
        Write-Info "OpenAPI spec has been updated - regeneration required"
        return $true
    }
    
    Write-Info "OpenAPI spec hasn't changed - skipping regeneration"
    return $false
}

function Invoke-ClientGeneration {
    Write-Info "Starting Kiota client generation..."
    
    if (!(Test-Path $GenerationScript)) {
        Write-Error "Generation script not found: $GenerationScript"
        return $false
    }
    
    try {
        $result = & pwsh -File $GenerationScript -ResourceGroup all
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Client generation completed successfully"
            # Update generation marker
            Set-Content $GenerationMarkerFile -Value (Get-Date).ToString()
            return $true
        }
        else {
            Write-Error "Client generation failed"
            return $false
        }
    }
    catch {
        Write-Error "Exception during client generation: $($_.Exception.Message)"
        return $false
    }
}

function Invoke-SolutionBuild {
    Write-Info "Building solution with configuration: $Configuration"
    
    try {
        $buildArgs = @(
            "build"
            "--configuration", $Configuration
            "--verbosity", "normal"
        )
        
        $result = & dotnet @buildArgs
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Solution build completed successfully"
            return $true
        }
        else {
            Write-Error "Solution build failed"
            return $false
        }
    }
    catch {
        Write-Error "Exception during solution build: $($_.Exception.Message)"
        return $false
    }
}

function Invoke-TestExecution {
    Write-Info "Running tests..."
    
    try {
        $testArgs = @(
            "test"
            "--configuration", $Configuration
            "--verbosity", "normal"
            "--logger", "console;verbosity=normal"
        )
        
        $result = & dotnet @testArgs
        if ($LASTEXITCODE -eq 0) {
            Write-Success "All tests passed"
            return $true
        }
        else {
            Write-Warning "Some tests failed - check output above"
            return $false
        }
    }
    catch {
        Write-Error "Exception during test execution: $($_.Exception.Message)"
        return $false
    }
}

function Get-BuildSummary {
    Write-Info ""
    Write-Info "═══════════════════════════════════════════════════════════"
    Write-Info "Build Summary"
    Write-Info "═══════════════════════════════════════════════════════════"
    
    # Check which clients exist
    $clientProjects = @(
        "src/Procore.SDK.Core",
        "src/Procore.SDK.ProjectManagement", 
        "src/Procore.SDK.QualitySafety",
        "src/Procore.SDK.ConstructionFinancials",
        "src/Procore.SDK.FieldProductivity",
        "src/Procore.SDK.ResourceManagement"
    )
    
    $generatedClients = 0
    foreach ($project in $clientProjects) {
        $generatedPath = "$project/Generated"
        if (Test-Path $generatedPath) {
            $fileCount = (Get-ChildItem $generatedPath -Recurse -File | Measure-Object).Count
            if ($fileCount -gt 0) {
                $generatedClients++
                Write-Info "  ✅ $(Split-Path $project -Leaf): $fileCount files"
            }
        }
    }
    
    Write-Info "Generated clients: $generatedClients/$($clientProjects.Count)"
    
    # Check build artifacts
    $buildArtifacts = Get-ChildItem "src/*/bin/$Configuration" -Directory -ErrorAction SilentlyContinue
    if ($buildArtifacts) {
        Write-Info "Build artifacts: $($buildArtifacts.Count) projects"
    }
    
    return $generatedClients -eq $clientProjects.Count
}

# Main execution
function Main {
    Write-Info "Procore SDK Build Integration"
    Write-Info "Configuration: $Configuration"
    Write-Info "Skip Generation: $SkipGeneration"
    Write-Info "Force Generation: $Force"
    
    $success = $true
    
    # Step 1: Client Generation (if needed)
    if (!$SkipGeneration) {
        if (Test-ShouldRegenerate) {
            if (!(Invoke-ClientGeneration)) {
                $success = $false
            }
        }
    } else {
        Write-Info "Skipping client generation as requested"
    }
    
    # Step 2: Solution Build
    if ($success) {
        if (!(Invoke-SolutionBuild)) {
            $success = $false
        }
    }
    
    # Step 3: Test Execution (optional - continue even if tests fail)
    if ($success) {
        Invoke-TestExecution | Out-Null
    }
    
    # Step 4: Build Summary
    $allClientsGenerated = Get-BuildSummary
    
    if ($success -and $allClientsGenerated) {
        Write-Success "🎉 Build integration completed successfully!"
        exit 0
    } elseif ($success) {
        Write-Warning "Build completed but some clients may not be generated"
        exit 0
    } else {
        Write-Error "Build integration failed"
        exit 1
    }
}

# Run the main function
Main