#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Generates Kiota clients for Procore API resource groups

.DESCRIPTION
    This script generates separate Kiota clients for different Procore API resource groups
    by filtering the large OpenAPI specification file by path patterns.

.PARAMETER ResourceGroup
    Specific resource group to generate (core, project-management, quality-safety, 
    construction-financials, field-productivity, resource-management) or "all" to generate all

.PARAMETER OpenApiSpec
    Path to the OpenAPI specification file (default: docs/rest_OAS_all.json)

.PARAMETER ValidateOnly
    Only validate the generated code without compilation

.PARAMETER Clean
    Clean existing generated code before generating new code

.EXAMPLE
    ./Generate-Clients.ps1 -ResourceGroup core
    
.EXAMPLE
    ./Generate-Clients.ps1 -ResourceGroup all -Clean
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("all", "core", "project-management", "quality-safety", "construction-financials", "field-productivity", "resource-management")]
    [string]$ResourceGroup = "all",
    
    [Parameter(Mandatory = $false)]
    [string]$OpenApiSpec = "docs/rest_OAS_all.json",
    
    [Parameter(Mandatory = $false)]
    [switch]$ValidateOnly,
    
    [Parameter(Mandatory = $false)]
    [switch]$Clean
)

# Color output functions
function Write-Success { param($Message) Write-Host "✅ $Message" -ForegroundColor Green }
function Write-Warning { param($Message) Write-Host "⚠️  $Message" -ForegroundColor Yellow }
function Write-Error { param($Message) Write-Host "❌ $Message" -ForegroundColor Red }
function Write-Info { param($Message) Write-Host "ℹ️  $Message" -ForegroundColor Cyan }

# Configuration for each resource group
$ResourceConfigs = @{
    "core" = @{
        Paths = @(
            "**/companies/**",
            "**/company_users/**", 
            "**/users/**",
            "**/folders-and-files/**",
            "**/custom-fields/**",
            "**/configurable-field-sets/**"
        )
        Namespace = "Procore.SDK.Core"
        ClassName = "CoreClient"
        Description = "Core functionality: companies, users, documents, custom fields"
    }
    "project-management" = @{
        Paths = @(
            "**/projects/**",
            "**/workflows/**",
            "**/task-items/**",
            "**/project-assignments/**",
            "**/project-users/**"
        )
        Namespace = "Procore.SDK.ProjectManagement"
        ClassName = "ProjectManagementClient"
        Description = "Project management: projects, workflows, tasks, assignments"
    }
    "quality-safety" = @{
        Paths = @(
            "**/inspections/**",
            "**/observations/**",
            "**/incidents/**",
            "**/safety/**",
            "**/quality/**",
            "**/punch/**"
        )
        Namespace = "Procore.SDK.QualitySafety"
        ClassName = "QualitySafetyClient"
        Description = "Quality & safety: inspections, observations, incidents, punch lists"
    }
    "construction-financials" = @{
        Paths = @(
            "**/contracts/**",
            "**/purchase-orders/**",
            "**/budgets/**",
            "**/cost-codes/**",
            "**/change-orders/**",
            "**/invoices/**",
            "**/payments/**"
        )
        Namespace = "Procore.SDK.ConstructionFinancials"
        ClassName = "ConstructionFinancialsClient"
        Description = "Financial management: contracts, POs, budgets, change orders, invoices"
    }
    "field-productivity" = @{
        Paths = @(
            "**/daily-logs/**",
            "**/timecards/**",
            "**/equipment/**",
            "**/manpower/**",
            "**/deliveries/**"
        )
        Namespace = "Procore.SDK.FieldProductivity"
        ClassName = "FieldProductivityClient"
        Description = "Field operations: daily logs, timecards, equipment, manpower tracking"
    }
    "resource-management" = @{
        Paths = @(
            "**/workforce/**",
            "**/resources/**",
            "**/assignments/**"
        )
        Namespace = "Procore.SDK.ResourceManagement"
        ClassName = "ResourceManagementClient"
        Description = "Resource management: workforce, resources, assignments"
    }
}

function Test-Prerequisites {
    Write-Info "Checking prerequisites..."
    
    # Check if Kiota is installed
    try {
        $kiotaVersion = & kiota --version 2>$null
        Write-Success "Kiota CLI found (version: $kiotaVersion)"
    }
    catch {
        Write-Error "Kiota CLI not found. Install with: dotnet tool install --global Microsoft.OpenApi.Kiota"
        return $false
    }
    
    # Check if OpenAPI spec exists
    if (!(Test-Path $OpenApiSpec)) {
        Write-Error "OpenAPI specification not found at: $OpenApiSpec"
        return $false
    }
    
    $specSize = (Get-Item $OpenApiSpec).Length / 1MB
    Write-Success "OpenAPI specification found ($([math]::Round($specSize, 1)) MB)"
    
    return $true
}

function New-GeneratedDirectory {
    param($OutputPath)
    
    if ($Clean -and (Test-Path $OutputPath)) {
        Write-Info "Cleaning existing generated code at: $OutputPath"
        Remove-Item $OutputPath -Recurse -Force
    }
    
    if (!(Test-Path $OutputPath)) {
        New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
        Write-Info "Created output directory: $OutputPath"
    }
}

function Invoke-KiotaGeneration {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    Write-Info "Generating $Name client: $($Config.Description)"
    
    $outputPath = "src/$($Config.Namespace)/Generated"
    New-GeneratedDirectory -OutputPath $outputPath
    
    # Build include path arguments
    $includeArgs = @()
    foreach ($path in $Config.Paths) {
        $includeArgs += "--include-path"
        $includeArgs += "`"$path`""
    }
    
    # Build Kiota command arguments
    $args = @(
        "generate"
        "--openapi", "`"$OpenApiSpec`""
        "--language", "CSharp"
        "--class-name", $Config.ClassName
        "--namespace-name", $Config.Namespace
        "--output", "`"$outputPath`""
        "--exclude-backward-compatible"
        "--clean-output"
    ) + $includeArgs
    
    Write-Info "Running: kiota $($args -join ' ')"
    
    try {
        $process = Start-Process -FilePath "kiota" -ArgumentList $args -Wait -PassThru -NoNewWindow -RedirectStandardOutput "temp_output.txt" -RedirectStandardError "temp_error.txt"
        
        $output = Get-Content "temp_output.txt" -Raw -ErrorAction SilentlyContinue
        $errorOutput = Get-Content "temp_error.txt" -Raw -ErrorAction SilentlyContinue
        
        # Clean up temp files
        Remove-Item "temp_output.txt", "temp_error.txt" -ErrorAction SilentlyContinue
        
        if ($process.ExitCode -eq 0) {
            Write-Success "Successfully generated $name client"
            if ($output) { Write-Host $output }
            return $true
        }
        else {
            Write-Error "Failed to generate $name client (Exit code: $($process.ExitCode))"
            if ($errorOutput) { Write-Host $errorOutput -ForegroundColor Red }
            return $false
        }
    }
    catch {
        Write-Error "Exception during generation of $name client: $($_.Exception.Message)"
        return $false
    }
}

function Test-GeneratedCode {
    param(
        [string]$Name,
        [hashtable]$Config
    )
    
    $outputPath = "src/$($Config.Namespace)/Generated"
    
    if (!(Test-Path $outputPath)) {
        Write-Error "Generated code directory not found: $outputPath"
        return $false
    }
    
    # Check if main client file was generated
    $clientFile = Join-Path $outputPath "$($Config.ClassName).cs"
    if (!(Test-Path $clientFile)) {
        Write-Warning "Main client file not found: $clientFile"
        return $false
    }
    
    # Count generated files
    $generatedFiles = Get-ChildItem $outputPath -Recurse -File | Measure-Object
    Write-Success "$name client: $($generatedFiles.Count) files generated"
    
    return $true
}

function Test-Compilation {
    Write-Info "Testing compilation of generated code..."
    
    try {
        $result = & dotnet build --verbosity quiet 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Success "All generated code compiles successfully"
            return $true
        }
        else {
            Write-Error "Compilation failed:"
            Write-Host $result -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Error "Exception during compilation test: $($_.Exception.Message)"
        return $false
    }
}

# Main execution
function Main {
    Write-Info "Procore SDK Kiota Client Generation Script"
    Write-Info "Resource Group: $ResourceGroup"
    
    if (!(Test-Prerequisites)) {
        exit 1
    }
    
    $successCount = 0
    $totalCount = 0
    
    # Determine which resources to generate
    $resourcesToGenerate = if ($ResourceGroup -eq "all") { 
        $ResourceConfigs.Keys 
    } else { 
        @($ResourceGroup) 
    }
    
    foreach ($name in $resourcesToGenerate) {
        $config = $ResourceConfigs[$name]
        if (!$config) {
            Write-Error "Unknown resource group: $name"
            continue
        }
        
        $totalCount++
        
        Write-Info ""
        Write-Info "═══════════════════════════════════════════════════════════"
        Write-Info "Processing: $name"
        Write-Info "═══════════════════════════════════════════════════════════"
        
        if (Invoke-KiotaGeneration -Name $name -Config $config) {
            if (Test-GeneratedCode -Name $name -Config $config) {
                $successCount++
            }
        }
    }
    
    Write-Info ""
    Write-Info "═══════════════════════════════════════════════════════════"
    Write-Info "Generation Summary"
    Write-Info "═══════════════════════════════════════════════════════════"
    Write-Info "Successfully generated: $successCount/$totalCount clients"
    
    if (!$ValidateOnly -and $successCount -gt 0) {
        if (Test-Compilation) {
            Write-Success "🎉 All operations completed successfully!"
        } else {
            Write-Warning "Generation succeeded but compilation failed"
            exit 1
        }
    }
    
    if ($successCount -eq $totalCount) {
        Write-Success "🎉 All client generations completed successfully!"
        exit 0
    } else {
        Write-Error "Some client generations failed"
        exit 1
    }
}

# Run the main function
Main