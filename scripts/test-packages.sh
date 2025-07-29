#!/bin/bash

# Test Script for NuGet Package Quality and Performance
# Validates package functionality, performance, and quality metrics

set -e

# Configuration
CONFIGURATION="${1:-Release}"
ARTIFACTS_PATH="${2:-./artifacts/packages}"
SOLUTION_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
TEST_ROOT="$SOLUTION_ROOT/test-temp"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Counters
TESTS_PASSED=0
TESTS_FAILED=0
TESTS_WARNING=0

function write_header() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}========================================${NC}"
}

function write_success() {
    echo -e "${GREEN}✓ $1${NC}"
    ((TESTS_PASSED++))
}

function write_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
    ((TESTS_WARNING++))
}

function write_error() {
    echo -e "${RED}✗ $1${NC}"
    ((TESTS_FAILED++))
}

function write_info() {
    echo -e "${BLUE}ℹ $1${NC}"
}

function cleanup() {
    if [[ -d "$TEST_ROOT" ]]; then
        rm -rf "$TEST_ROOT"
    fi
}

# Cleanup on exit
trap cleanup EXIT

write_header "NuGet Package Quality & Performance Tests"

# Create test environment
mkdir -p "$TEST_ROOT"
cd "$TEST_ROOT"

# Test 1: Package Installation Test
write_header "Test 1: Package Installation & Dependency Resolution"

for TARGET_FRAMEWORK in "net6.0" "net8.0"; do
    write_info "Testing installation on $TARGET_FRAMEWORK"
    
    # Create test project
    TEST_PROJECT_DIR="test-install-$TARGET_FRAMEWORK"
    mkdir -p "$TEST_PROJECT_DIR"
    cd "$TEST_PROJECT_DIR"
    
    # Create test project file (without central package management)
    cat > "TestProject.csproj" << EOF
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>$TARGET_FRAMEWORK</TargetFramework>
    <OutputType>Exe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <ManagePackageVersionsCentrally>false</ManagePackageVersionsCentrally>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Procore.SDK.Core" Version="1.0.0" />
  </ItemGroup>
</Project>
EOF
    
    # Create test program
    cat > "Program.cs" << EOF
using Procore.SDK.Core;

// Test basic SDK functionality
var config = new ProcoreConfiguration
{
    BaseUrl = "https://api.procore.com",
    ClientId = "test-client-id"
};

Console.WriteLine("Procore SDK loaded successfully!");
Console.WriteLine(\$"Base URL: {config.BaseUrl}");
return 0;
EOF
    
    # Add local package source
    if [[ -f "$SOLUTION_ROOT/$ARTIFACTS_PATH/Procore.SDK.Core.1.0.0.nupkg" ]]; then
        dotnet nuget add source "$SOLUTION_ROOT/$ARTIFACTS_PATH" --name "LocalPackages" 2>/dev/null || true
        
        # Test restore
        if dotnet restore --verbosity quiet 2>/dev/null; then
            write_success "Package restore successful for $TARGET_FRAMEWORK"
            
            # Test build
            if dotnet build --verbosity quiet --no-restore 2>/dev/null; then
                write_success "Package build successful for $TARGET_FRAMEWORK"
                
                # Test run
                if timeout 30s dotnet run --no-build --verbosity quiet 2>/dev/null; then
                    write_success "Package execution successful for $TARGET_FRAMEWORK"
                else
                    write_error "Package execution failed for $TARGET_FRAMEWORK"
                fi
            else
                write_error "Package build failed for $TARGET_FRAMEWORK"
            fi
        else
            write_error "Package restore failed for $TARGET_FRAMEWORK"
        fi
    else
        write_warning "Package not found for testing: Procore.SDK.Core.1.0.0.nupkg"
    fi
    
    cd "$TEST_ROOT"
done

# Test 2: Package Size and Performance
write_header "Test 2: Package Size & Performance Analysis"

PACKAGES_DIR="$SOLUTION_ROOT/$ARTIFACTS_PATH"
if [[ -d "$PACKAGES_DIR" ]]; then
    for PACKAGE in "$PACKAGES_DIR"/*.nupkg; do
        if [[ -f "$PACKAGE" ]]; then
            PACKAGE_NAME=$(basename "$PACKAGE")
            PACKAGE_SIZE=$(du -h "$PACKAGE" | cut -f1)
            PACKAGE_SIZE_BYTES=$(stat -f %z "$PACKAGE" 2>/dev/null || stat -c %s "$PACKAGE" 2>/dev/null || echo "0")
            
            write_info "Analyzing $PACKAGE_NAME ($PACKAGE_SIZE)"
            
            # Size checks
            if [[ $PACKAGE_SIZE_BYTES -lt 52428800 ]]; then  # 50MB
                write_success "Package size acceptable: $PACKAGE_SIZE"
            elif [[ $PACKAGE_SIZE_BYTES -lt 104857600 ]]; then  # 100MB
                write_warning "Package size large: $PACKAGE_SIZE"
            else
                write_error "Package size too large: $PACKAGE_SIZE"
            fi
            
            # Extract and analyze contents
            EXTRACT_DIR="extract-$(basename "$PACKAGE" .nupkg)"
            mkdir -p "$EXTRACT_DIR"
            
            if unzip -q "$PACKAGE" -d "$EXTRACT_DIR" 2>/dev/null; then
                # Count assemblies
                ASSEMBLY_COUNT=$(find "$EXTRACT_DIR/lib" -name "*.dll" 2>/dev/null | wc -l || echo "0")
                if [[ $ASSEMBLY_COUNT -gt 0 ]]; then
                    write_success "Package contains $ASSEMBLY_COUNT assemblies"
                else
                    write_warning "Package contains no assemblies"
                fi
                
                # Check for documentation
                DOC_COUNT=$(find "$EXTRACT_DIR/lib" -name "*.xml" 2>/dev/null | wc -l || echo "0")
                if [[ $DOC_COUNT -gt 0 ]]; then
                    write_success "Package includes XML documentation"
                else
                    write_warning "Package missing XML documentation"
                fi
                
                # Check for multi-targeting
                FRAMEWORK_COUNT=$(find "$EXTRACT_DIR/lib" -mindepth 1 -maxdepth 1 -type d 2>/dev/null | wc -l || echo "0")
                if [[ $FRAMEWORK_COUNT -gt 1 ]]; then
                    write_success "Package supports multiple target frameworks ($FRAMEWORK_COUNT)"
                elif [[ $FRAMEWORK_COUNT -eq 1 ]]; then
                    write_success "Package supports single target framework"
                else
                    write_warning "Package target frameworks unclear"
                fi
                
                rm -rf "$EXTRACT_DIR"
            else
                write_error "Failed to extract package: $PACKAGE_NAME"
            fi
        fi
    done
else
    write_warning "Package directory not found: $PACKAGES_DIR"
fi

# Test 3: Symbol Package Validation
write_header "Test 3: Symbol Package Validation"

for SYMBOL_PACKAGE in "$PACKAGES_DIR"/*.snupkg; do
    if [[ -f "$SYMBOL_PACKAGE" ]]; then
        SYMBOL_NAME=$(basename "$SYMBOL_PACKAGE")
        SYMBOL_SIZE=$(du -h "$SYMBOL_PACKAGE" | cut -f1)
        
        write_info "Analyzing symbol package: $SYMBOL_NAME ($SYMBOL_SIZE)"
        
        # Extract and check contents
        SYMBOL_EXTRACT_DIR="symbol-extract-$(basename "$SYMBOL_PACKAGE" .snupkg)"
        mkdir -p "$SYMBOL_EXTRACT_DIR"
        
        if unzip -q "$SYMBOL_PACKAGE" -d "$SYMBOL_EXTRACT_DIR" 2>/dev/null; then
            # Check for PDB files
            PDB_COUNT=$(find "$SYMBOL_EXTRACT_DIR" -name "*.pdb" 2>/dev/null | wc -l || echo "0")
            if [[ $PDB_COUNT -gt 0 ]]; then
                write_success "Symbol package contains $PDB_COUNT PDB files"
            else
                write_warning "Symbol package contains no PDB files"
            fi
            
            rm -rf "$SYMBOL_EXTRACT_DIR"
        else
            write_error "Failed to extract symbol package: $SYMBOL_NAME"
        fi
    fi
done

# Test 4: Metadata Validation
write_header "Test 4: Package Metadata Validation"

for PACKAGE in "$PACKAGES_DIR"/*.nupkg; do
    if [[ -f "$PACKAGE" ]]; then
        PACKAGE_NAME=$(basename "$PACKAGE")
        write_info "Validating metadata for: $PACKAGE_NAME"
        
        # Extract nuspec
        METADATA_DIR="metadata-$(basename "$PACKAGE" .nupkg)"
        mkdir -p "$METADATA_DIR"
        
        if unzip -q "$PACKAGE" -d "$METADATA_DIR" 2>/dev/null; then
            NUSPEC_FILE=$(find "$METADATA_DIR" -name "*.nuspec" | head -1)
            if [[ -n "$NUSPEC_FILE" ]]; then
                # Check required metadata
                if grep -q "<id>" "$NUSPEC_FILE" && \
                   grep -q "<version>" "$NUSPEC_FILE" && \
                   grep -q "<authors>" "$NUSPEC_FILE" && \
                   grep -q "<description>" "$NUSPEC_FILE"; then
                    write_success "Package metadata validation passed: $PACKAGE_NAME"
                else
                    write_error "Package metadata validation failed: $PACKAGE_NAME"
                fi
                
                # Check for license
                if grep -q "<license" "$NUSPEC_FILE" || grep -q "<licenseUrl>" "$NUSPEC_FILE"; then
                    write_success "Package includes license information"
                else
                    write_warning "Package missing license information"
                fi
                
                # Check for project URL
                if grep -q "<projectUrl>" "$NUSPEC_FILE"; then
                    write_success "Package includes project URL"
                else
                    write_warning "Package missing project URL"
                fi
            else
                write_error "Nuspec file not found in package: $PACKAGE_NAME"
            fi
            
            rm -rf "$METADATA_DIR"
        fi
    fi
done

# Results Summary
write_header "Test Results Summary"

TOTAL_TESTS=$((TESTS_PASSED + TESTS_WARNING + TESTS_FAILED))
write_info "Total tests: $TOTAL_TESTS"
echo -e "${GREEN}Passed: $TESTS_PASSED${NC}"
echo -e "${YELLOW}Warnings: $TESTS_WARNING${NC}"
echo -e "${RED}Failed: $TESTS_FAILED${NC}"

# Calculate success rate
if [[ $TOTAL_TESTS -gt 0 ]]; then
    SUCCESS_RATE=$(( (TESTS_PASSED * 100) / TOTAL_TESTS ))
    write_info "Success rate: ${SUCCESS_RATE}%"
    
    if [[ $SUCCESS_RATE -ge 90 ]]; then
        write_success "Package quality: EXCELLENT"
        exit_code=0
    elif [[ $SUCCESS_RATE -ge 75 ]]; then
        write_success "Package quality: GOOD"
        exit_code=0
    elif [[ $SUCCESS_RATE -ge 60 ]]; then
        write_warning "Package quality: ACCEPTABLE"
        exit_code=0
    else
        write_error "Package quality: POOR"
        exit_code=1
    fi
else
    write_warning "No tests were executed"
    exit_code=1
fi

# Performance recommendations
if [[ $TESTS_WARNING -gt 0 ]]; then
    write_header "Recommendations"
    echo -e "${YELLOW}Consider addressing the warnings above to improve package quality${NC}"
fi

exit $exit_code