# Procore SDK Quality Assurance Test Runner
# Executes comprehensive quality assessment across all dimensions

param(
    [string]$Configuration = "Debug",
    [string]$OutputFormat = "detailed",
    [switch]$GenerateReport
)

Write-Host "🏗️  Procore SDK Quality Assurance Test Suite" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Green
Write-Host ""

# Test categories and their descriptions
$TestCategories = @{
    "OAuth" = "OAuth Flow Quality Assurance Tests"
    "BestPractices" = ".NET Best Practices Compliance Tests"
    "ErrorHandling" = "Error Handling and Exception Management Tests"
    "Compatibility" = "Cross-Version Compatibility Tests"
    "Security" = "Security Audit and Vulnerability Assessment Tests (including PKCE RFC 7636 compliance)"
    "Performance" = "Performance Testing and Optimization Analysis Tests"
    "Quality" = "Code Quality, Maintainability, and Technical Debt Tests"
    "Documentation" = "Documentation Accuracy and Completeness Tests"
    "Integration" = "Integration Points and API Endpoint Functionality Tests"
}

$TotalTests = 0
$PassedTests = 0
$FailedTests = 0
$TestResults = @()

# Run each test category
foreach ($Category in $TestCategories.Keys) {
    Write-Host "📋 Running $($TestCategories[$Category])..." -ForegroundColor Yellow
    Write-Host "   Category: $Category" -ForegroundColor Gray
    
    try {
        $FilterPattern = "*$Category*Tests"
        $TestCommand = "dotnet test --configuration $Configuration --filter FullyQualifiedName~QualityAssurance.$Category --logger console --verbosity normal"
        
        $Result = Invoke-Expression $TestCommand
        $ExitCode = $LASTEXITCODE
        
        if ($ExitCode -eq 0) {
            Write-Host "   ✅ $Category tests PASSED" -ForegroundColor Green
            $PassedTests++
        } else {
            Write-Host "   ❌ $Category tests FAILED" -ForegroundColor Red
            $FailedTests++
        }
        
        $TestResults += @{
            Category = $Category
            Description = $TestCategories[$Category]
            Status = if ($ExitCode -eq 0) { "PASSED" } else { "FAILED" }
            ExitCode = $ExitCode
        }
        
        $TotalTests++
    }
    catch {
        Write-Host "   ⚠️  Error running $Category tests: $($_.Exception.Message)" -ForegroundColor Red
        $FailedTests++
        $TotalTests++
        
        $TestResults += @{
            Category = $Category
            Description = $TestCategories[$Category]
            Status = "ERROR"
            ExitCode = -1
            Error = $_.Exception.Message
        }
    }
    
    Write-Host ""
}

# Generate summary report
Write-Host "📊 Quality Assurance Summary Report" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host ""

$SuccessRate = if ($TotalTests -gt 0) { ($PassedTests / $TotalTests) * 100 } else { 0 }

Write-Host "Total Test Categories: $TotalTests" -ForegroundColor White
Write-Host "Passed Categories: $PassedTests" -ForegroundColor Green
Write-Host "Failed Categories: $FailedTests" -ForegroundColor Red
Write-Host "Success Rate: $($SuccessRate.ToString('F1'))%" -ForegroundColor $(if ($SuccessRate -ge 90) { 'Green' } elseif ($SuccessRate -ge 75) { 'Yellow' } else { 'Red' })
Write-Host ""

# Detailed results
Write-Host "Detailed Results:" -ForegroundColor White
Write-Host "-----------------" -ForegroundColor White

foreach ($Result in $TestResults) {
    $StatusColor = switch ($Result.Status) {
        "PASSED" { 'Green' }
        "FAILED" { 'Red' }
        "ERROR" { 'Magenta' }
        default { 'Gray' }
    }
    
    $StatusIcon = switch ($Result.Status) {
        "PASSED" { '✅' }
        "FAILED" { '❌' }
        "ERROR" { '⚠️' }
        default { '❓' }
    }
    
    Write-Host "$StatusIcon $($Result.Category): $($Result.Status)" -ForegroundColor $StatusColor
    Write-Host "   $($Result.Description)" -ForegroundColor Gray
    
    if ($Result.Error) {
        Write-Host "   Error: $($Result.Error)" -ForegroundColor Red
    }
    
    Write-Host ""
}

# Overall quality assessment
Write-Host "🎯 Overall Quality Assessment" -ForegroundColor Cyan
Write-Host "=============================" -ForegroundColor Cyan

$QualityGrade = switch ($SuccessRate) {
    { $_ -ge 95 } { "A+ (Excellent)" }
    { $_ -ge 90 } { "A (Excellent)" }
    { $_ -ge 85 } { "A- (Very Good)" }
    { $_ -ge 80 } { "B+ (Good)" }
    { $_ -ge 75 } { "B (Good)" }
    { $_ -ge 70 } { "B- (Acceptable)" }
    { $_ -ge 65 } { "C+ (Needs Improvement)" }
    { $_ -ge 60 } { "C (Needs Improvement)" }
    default { "F (Requires Attention)" }
}

$GradeColor = switch ($SuccessRate) {
    { $_ -ge 85 } { 'Green' }
    { $_ -ge 70 } { 'Yellow' }
    default { 'Red' }
}

Write-Host "Quality Grade: $QualityGrade" -ForegroundColor $GradeColor
Write-Host ""

# Recommendations based on results
if ($FailedTests -gt 0) {
    Write-Host "🔧 Recommendations:" -ForegroundColor Yellow
    Write-Host "- Review failed test categories and address identified issues" -ForegroundColor Yellow
    Write-Host "- Run individual test categories for detailed failure analysis" -ForegroundColor Yellow
    Write-Host "- Consult the Comprehensive Quality Assessment Report for specific guidance" -ForegroundColor Yellow
} else {
    Write-Host "🎉 Congratulations! All quality assurance tests passed." -ForegroundColor Green
    Write-Host "The sample applications meet all quality standards." -ForegroundColor Green
}

Write-Host ""

# Generate report if requested
if ($GenerateReport) {
    Write-Host "📄 Generating Quality Assessment Report..." -ForegroundColor Cyan
    
    $ReportPath = Join-Path $PSScriptRoot "QUALITY_ASSESSMENT_RESULTS.md"
    $ReportContent = @"
# Quality Assessment Results
**Generated:** $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
**Configuration:** $Configuration
**Success Rate:** $($SuccessRate.ToString('F1'))%
**Quality Grade:** $QualityGrade

## Test Results Summary

| Category | Status | Description |
|----------|--------|-------------|
"@

    foreach ($Result in $TestResults) {
        $StatusEmoji = switch ($Result.Status) {
            "PASSED" { '✅' }
            "FAILED" { '❌' }
            "ERROR" { '⚠️' }
            default { '❓' }
        }
        $ReportContent += "| $($Result.Category) | $StatusEmoji $($Result.Status) | $($Result.Description) |`n"
    }

    $ReportContent += @"

## Overall Assessment
- **Total Categories:** $TotalTests
- **Passed:** $PassedTests
- **Failed:** $FailedTests
- **Success Rate:** $($SuccessRate.ToString('F1'))%
- **Quality Grade:** $QualityGrade

For detailed analysis and recommendations, see the Comprehensive Quality Assessment Report.
"@

    $ReportContent | Out-File -FilePath $ReportPath -Encoding UTF8
    Write-Host "Report generated: $ReportPath" -ForegroundColor Green
}

# Exit with appropriate code
if ($FailedTests -gt 0) {
    Write-Host "❌ Quality assurance completed with failures." -ForegroundColor Red
    exit 1
} else {
    Write-Host "✅ Quality assurance completed successfully." -ForegroundColor Green
    exit 0
}