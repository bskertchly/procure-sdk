#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Validates quality of generated Kiota client code

.DESCRIPTION
    This script validates the generated Kiota client code for quality, compilation,
    and adherence to coding standards.

.PARAMETER ResourceGroup
    Specific resource group to validate or "all" to validate all

.PARAMETER DetailedOutput
    Show detailed validation results

.EXAMPLE
    ./Validate-GeneratedCode.ps1 -ResourceGroup core -DetailedOutput
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("all", "core", "project-management", "quality-safety", "construction-financials", "field-productivity", "resource-management")]
    [string]$ResourceGroup = "all",
    
    [Parameter(Mandatory = $false)]
    [switch]$DetailedOutput
)

# Color output functions
function Write-Success { param($Message) Write-Host "✅ $Message" -ForegroundColor Green }
function Write-Warning { param($Message) Write-Host "⚠️  $Message" -ForegroundColor Yellow }
function Write-Error { param($Message) Write-Host "❌ $Message" -ForegroundColor Red }
function Write-Info { param($Message) Write-Host "ℹ️  $Message" -ForegroundColor Cyan }

# Resource configurations (same as generation script)
$ResourceConfigs = @{
    "core" = @{
        Namespace = "Procore.SDK.Core"
        ClassName = "CoreClient"
    }
    "project-management" = @{
        Namespace = "Procore.SDK.ProjectManagement"
        ClassName = "ProjectManagementClient"
    }
    "quality-safety" = @{
        Namespace = "Procore.SDK.QualitySafety"
        ClassName = "QualitySafetyClient"
    }
    "construction-financials" = @{
        Namespace = "Procore.SDK.ConstructionFinancials"
        ClassName = "ConstructionFinancialsClient"
    }
    "field-productivity" = @{
        Namespace = "Procore.SDK.FieldProductivity"
        ClassName = "FieldProductivityClient"
    }
    "resource-management" = @{
        Namespace = "Procore.SDK.ResourceManagement"
        ClassName = "ResourceManagementClient"
    }
}

function Test-GeneratedDirectoryStructure {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    $outputPath = "src/$($Config.Namespace)/Generated"
    $issues = @()
    
    Write-Info "Validating directory structure for $Name..."
    
    if (!(Test-Path $outputPath)) {
        $issues += "Generated directory not found: $outputPath"
        return $issues
    }
    
    # Check for main client file
    $clientFile = Join-Path $outputPath "$($Config.ClassName).cs"
    if (!(Test-Path $clientFile)) {
        $issues += "Main client file not found: $clientFile"
    }
    
    # Check for models directory
    $modelsPath = Join-Path $outputPath "Models"
    if (!(Test-Path $modelsPath)) {
        $issues += "Models directory not found: $modelsPath"
    }
    
    # Count files
    $totalFiles = (Get-ChildItem $outputPath -Recurse -File | Measure-Object).Count
    if ($totalFiles -eq 0) {
        $issues += "No files generated in: $outputPath"
    }
    
    if ($DetailedOutput) {
        Write-Info "  Total files generated: $totalFiles"
        if (Test-Path $modelsPath) {
            $modelFiles = (Get-ChildItem $modelsPath -File | Measure-Object).Count
            Write-Info "  Model files: $modelFiles"
        }
    }
    
    return $issues
}

function Test-CodeQuality {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    $outputPath = "src/$($Config.Namespace)/Generated"
    $issues = @()
    
    Write-Info "Checking code quality for $Name..."
    
    if (!(Test-Path $outputPath)) {
        $issues += "Generated directory not found: $outputPath"
        return $issues
    }
    
    # Get all C# files
    $csFiles = Get-ChildItem $outputPath -Recurse -Filter "*.cs"
    
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw
        
        # Check for common code quality issues
        if ($content -match "public class.*\{[\s\S]*\}[\s\S]*\}" -and $content.Length -lt 50) {
            $issues += "Suspiciously small class file: $($file.Name)"
        }
        
        # Check for proper namespace
        if ($content -notmatch "namespace $([regex]::Escape($Config.Namespace))") {
            $issues += "Incorrect namespace in file: $($file.Name)"
        }
        
        # Check for TODO comments (indication of incomplete generation)
        if ($content -match "TODO|FIXME|HACK") {
            $issues += "TODO/FIXME comments found in: $($file.Name)"
        }
        
        # Check for compilation errors in syntax
        if ($content -match "{\s*}.*{\s*}") {
            $issues += "Potential syntax issues in: $($file.Name)"
        }
    }
    
    if ($DetailedOutput) {
        Write-Info "  C# files analyzed: $($csFiles.Count)"
        $totalLines = ($csFiles | ForEach-Object { (Get-Content $_.FullName | Measure-Object -Line).Lines } | Measure-Object -Sum).Sum
        Write-Info "  Total lines of code: $totalLines"
    }
    
    return $issues
}

function Test-Compilation {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    Write-Info "Testing compilation for $Name..."
    
    $projectPath = "src/$($Config.Namespace)/$($Config.Namespace).csproj"
    
    if (!(Test-Path $projectPath)) {
        return @("Project file not found: $projectPath")
    }
    
    try {
        $result = & dotnet build $projectPath --verbosity quiet 2>&1
        if ($LASTEXITCODE -eq 0) {
            if ($DetailedOutput) {
                Write-Info "  Compilation successful"
            }
            return @()
        }
        else {
            return @("Compilation failed for $Name", $result)
        }
    }
    catch {
        return @("Exception during compilation test for $Name : $($_.Exception.Message)")
    }
}

function Test-ApiSurface {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    Write-Info "Analyzing API surface for $Name..."
    
    $outputPath = "src/$($Config.Namespace)/Generated"
    $issues = @()
    
    if (!(Test-Path $outputPath)) {
        return @("Generated directory not found: $outputPath")
    }
    
    $clientFile = Join-Path $outputPath "$($Config.ClassName).cs"
    if (!(Test-Path $clientFile)) {
        return @("Main client file not found: $clientFile")
    }
    
    $content = Get-Content $clientFile -Raw
    
    # Check for expected client structure
    if ($content -notmatch "class $([regex]::Escape($Config.ClassName))") {
        $issues += "Main client class not found in: $clientFile"
    }
    
    # Check for HTTP methods
    $httpMethods = @("GetAsync", "PostAsync", "PutAsync", "PatchAsync", "DeleteAsync")
    $foundMethods = @()
    foreach ($method in $httpMethods) {
        if ($content -match $method) {
            $foundMethods += $method
        }
    }
    
    if ($foundMethods.Count -eq 0) {
        $issues += "No HTTP method implementations found in main client"
    }
    
    if ($DetailedOutput) {
        Write-Info "  HTTP methods found: $($foundMethods -join ', ')"
        
        # Count public methods
        $publicMethods = [regex]::Matches($content, "public.*?(?:async\s+)?(?:Task<.*?>|void|[\w<>]+)\s+\w+\s*\(").Count
        Write-Info "  Public methods: $publicMethods"
    }
    
    return $issues
}

function Test-Dependencies {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    Write-Info "Checking dependencies for $Name..."
    
    $projectPath = "src/$($Config.Namespace)/$($Config.Namespace).csproj"
    $issues = @()
    
    if (!(Test-Path $projectPath)) {
        return @("Project file not found: $projectPath")
    }
    
    $projectContent = Get-Content $projectPath -Raw
    
    # Check for required Kiota dependencies
    $requiredPackages = @("Microsoft.Kiota.Abstractions", "Microsoft.Kiota.Http.HttpClientLibrary", "Microsoft.Kiota.Serialization.Json")
    
    foreach ($package in $requiredPackages) {
        if ($projectContent -notmatch [regex]::Escape($package)) {
            $issues += "Missing required package reference: $package"
        }
    }
    
    if ($DetailedOutput) {
        $packageRefs = [regex]::Matches($projectContent, '<PackageReference\s+Include="([^"]+)"').Captures
        $packages = $packageRefs | ForEach-Object { $_.Groups[1].Value }
        Write-Info "  Package references: $($packages -join ', ')"
    }
    
    return $issues
}

# Main validation function
function Invoke-ValidationSuite {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    Write-Info ""
    Write-Info "═══════════════════════════════════════════════════════════"
    Write-Info "Validating: $Name"
    Write-Info "═══════════════════════════════════════════════════════════"
    
    $allIssues = @()
    
    # Run all validation tests
    $allIssues += Test-GeneratedDirectoryStructure -Name $Name -Config $Config
    $allIssues += Test-CodeQuality -Name $Name -Config $Config
    $allIssues += Test-Compilation -Name $Name -Config $Config
    $allIssues += Test-ApiSurface -Name $Name -Config $Config
    $allIssues += Test-Dependencies -Name $Name -Config $Config
    
    if ($allIssues.Count -eq 0) {
        Write-Success "$Name validation passed ✨"
        return $true
    }
    else {
        Write-Warning "$Name validation found $($allIssues.Count) issue(s):"
        foreach ($issue in $allIssues) {
            Write-Error "  • $issue"
        }
        return $false
    }
}

# Main execution
function Main {
    Write-Info "Procore SDK Generated Code Validation"
    Write-Info "Resource Group: $ResourceGroup"
    Write-Info "Detailed Output: $DetailedOutput"
    
    $passedCount = 0
    $totalCount = 0
    
    # Determine which resources to validate
    $resourcesToValidate = if ($ResourceGroup -eq "all") { 
        $ResourceConfigs.Keys 
    } else { 
        @($ResourceGroup) 
    }
    
    foreach ($name in $resourcesToValidate) {
        $config = $ResourceConfigs[$name]
        if (!$config) {
            Write-Error "Unknown resource group: $name"
            continue
        }
        
        $totalCount++
        
        if (Invoke-ValidationSuite -Name $name -Config $config) {
            $passedCount++
        }
    }
    
    Write-Info ""
    Write-Info "═══════════════════════════════════════════════════════════"
    Write-Info "Validation Summary"
    Write-Info "═══════════════════════════════════════════════════════════"
    Write-Info "Validation passed: $passedCount/$totalCount clients"
    
    if ($passedCount -eq $totalCount) {
        Write-Success "🎉 All validations passed!"
        exit 0
    } else {
        Write-Error "Some validations failed"
        exit 1
    }
}

# Run the main function
Main