#!/bin/bash

# Procore SDK Package Validation Script
# Validates NuGet package configuration for all SDK projects

set -e

# Configuration
CONFIGURATION="${1:-Release}"
OUTPUT_PATH="${2:-./artifacts/packages}"
SOLUTION_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SRC_PATH="$SOLUTION_ROOT/src"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# SDK Projects to validate
SDK_PROJECTS=(
    "Procore.SDK"
    "Procore.SDK.Core"
    "Procore.SDK.Shared"
    "Procore.SDK.ProjectManagement"
    "Procore.SDK.QualitySafety"
    "Procore.SDK.ConstructionFinancials"
    "Procore.SDK.FieldProductivity"
    "Procore.SDK.ResourceManagement"
)

# Validation results
VALIDATION_RESULTS=()
PASSED=0
WARNINGS=0
FAILED=0

function write_header() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}========================================${NC}"
}

function write_success() {
    echo -e "${GREEN}✓ $1${NC}"
    ((PASSED++))
}

function write_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
    ((WARNINGS++))
}

function write_error() {
    echo -e "${RED}✗ $1${NC}"
    ((FAILED++))
}

function write_info() {
    echo -e "${BLUE}ℹ $1${NC}"
}

write_header "Procore SDK Package Validation"

# Create output directory
OUTPUT_DIR="$SOLUTION_ROOT/$OUTPUT_PATH"
mkdir -p "$OUTPUT_DIR"
write_info "Output directory: $OUTPUT_DIR"

# Step 1: Validate Project Files
write_header "Step 1: Validating Project Files"

for PROJECT in "${SDK_PROJECTS[@]}"; do
    PROJECT_PATH="$SRC_PATH/$PROJECT/$PROJECT.csproj"
    
    if [[ ! -f "$PROJECT_PATH" ]]; then
        write_error "Project file not found: $PROJECT_PATH"
        continue
    fi
    
    # Check for required elements using xmllint or simple grep
    if command -v xmllint >/dev/null 2>&1; then
        # Use xmllint for proper XML parsing
        PACKAGE_ID=$(xmllint --xpath "//PackageId/text()" "$PROJECT_PATH" 2>/dev/null || echo "")
        DESCRIPTION=$(xmllint --xpath "//Description/text()" "$PROJECT_PATH" 2>/dev/null || echo "")
    else
        # Fallback to grep for basic validation
        PACKAGE_ID=$(grep -o '<PackageId>[^<]*</PackageId>' "$PROJECT_PATH" | sed 's/<PackageId>//;s/<\/PackageId>//' || echo "")
        DESCRIPTION=$(grep -o '<Description>[^<]*</Description>' "$PROJECT_PATH" | sed 's/<Description>//;s/<\/Description>//' || echo "")
    fi
    
    if [[ -n "$PACKAGE_ID" && -n "$DESCRIPTION" ]]; then
        write_success "Project validation passed: $PROJECT"
    else
        MISSING=()
        [[ -z "$PACKAGE_ID" ]] && MISSING+=("PackageId")
        [[ -z "$DESCRIPTION" ]] && MISSING+=("Description")
        write_warning "Project validation warnings: $PROJECT - Missing: ${MISSING[*]}"
    fi
done

# Step 2: Build Validation
write_header "Step 2: Build Validation"

for PROJECT in "${SDK_PROJECTS[@]}"; do
    PROJECT_PATH="$SRC_PATH/$PROJECT/$PROJECT.csproj"
    
    if [[ ! -f "$PROJECT_PATH" ]]; then
        continue
    fi
    
    write_info "Building project: $PROJECT"
    
    if dotnet build "$PROJECT_PATH" --configuration "$CONFIGURATION" --verbosity quiet --nologo >/dev/null 2>&1; then
        write_success "Build passed: $PROJECT"
    else
        write_error "Build failed: $PROJECT"
        # Show build errors
        dotnet build "$PROJECT_PATH" --configuration "$CONFIGURATION" --verbosity minimal 2>&1 | head -20
    fi
done

# Step 3: Package Generation
write_header "Step 3: Package Generation"

for PROJECT in "${SDK_PROJECTS[@]}"; do
    PROJECT_PATH="$SRC_PATH/$PROJECT/$PROJECT.csproj"
    
    if [[ ! -f "$PROJECT_PATH" ]]; then
        continue
    fi
    
    write_info "Creating package: $PROJECT"
    
    if dotnet pack "$PROJECT_PATH" --configuration "$CONFIGURATION" --output "$OUTPUT_DIR" --verbosity quiet --nologo >/dev/null 2>&1; then
        write_success "Package created: $PROJECT"
    else
        write_error "Package creation failed: $PROJECT"
        # Show pack errors
        dotnet pack "$PROJECT_PATH" --configuration "$CONFIGURATION" --output "$OUTPUT_DIR" --verbosity minimal 2>&1 | head -10
    fi
done

# Step 4: Package Content Validation
write_header "Step 4: Package Content Validation"

if command -v unzip >/dev/null 2>&1; then
    for PACKAGE in "$OUTPUT_DIR"/*.nupkg; do
        if [[ -f "$PACKAGE" ]]; then
            PACKAGE_NAME=$(basename "$PACKAGE" .nupkg)
            write_info "Validating package content: $PACKAGE_NAME"
            
            # Create temp directory
            TEMP_DIR=$(mktemp -d)
            
            # Extract package
            if unzip -q "$PACKAGE" -d "$TEMP_DIR" 2>/dev/null; then
                # Check for nuspec
                NUSPEC_FILE=$(find "$TEMP_DIR" -name "*.nuspec" | head -1)
                if [[ -n "$NUSPEC_FILE" ]]; then
                    # Check for lib folder
                    if [[ -d "$TEMP_DIR/lib" ]]; then
                        ASSEMBLY_COUNT=$(find "$TEMP_DIR/lib" -name "*.dll" | wc -l)
                        if [[ $ASSEMBLY_COUNT -gt 0 ]]; then
                            write_success "Package content validation passed: $PACKAGE_NAME ($ASSEMBLY_COUNT assemblies)"
                        else
                            write_warning "No assemblies found in package: $PACKAGE_NAME"
                        fi
                    else
                        write_warning "No lib folder found in package: $PACKAGE_NAME"
                    fi
                else
                    write_error "No nuspec file found in package: $PACKAGE_NAME"
                fi
            else
                write_error "Failed to extract package: $PACKAGE_NAME"
            fi
            
            # Cleanup
            rm -rf "$TEMP_DIR"
        fi
    done
else
    write_warning "unzip not available - skipping package content validation"
fi

# Step 5: Multi-targeting Validation
write_header "Step 5: Multi-targeting Validation"

for PROJECT in "${SDK_PROJECTS[@]}"; do
    PROJECT_PATH="$SRC_PATH/$PROJECT/$PROJECT.csproj"
    
    if [[ ! -f "$PROJECT_PATH" ]]; then
        continue
    fi
    
    if grep -q "TargetFrameworks\|TargetFramework" "$PROJECT_PATH"; then
        # Check if it's using Directory.Build.props multi-targeting
        if grep -q "<TargetFrameworks>" "$PROJECT_PATH" || grep -q "<TargetFrameworks>" "$SOLUTION_ROOT/Directory.Build.props"; then
            write_success "Multi-targeting configured: $PROJECT"
        else
            write_success "Target framework configured: $PROJECT"
        fi
    else
        write_warning "No target framework specified: $PROJECT"
    fi
done

# Step 6: Source Link Validation
write_header "Step 6: Source Link Validation"

if command -v dotnet >/dev/null 2>&1; then
    # Check if source link packages are referenced
    if grep -q "Microsoft.SourceLink.GitHub" "$SOLUTION_ROOT/Directory.Build.props"; then
        write_success "Source Link configured in Directory.Build.props"
        
        # Check if source link settings are present
        if grep -q "PublishRepositoryUrl.*true" "$SOLUTION_ROOT/Directory.Build.props" && \
           grep -q "EmbedUntrackedSources.*true" "$SOLUTION_ROOT/Directory.Build.props"; then
            write_success "Source Link settings properly configured"
        else
            write_warning "Source Link settings may be incomplete"
        fi
    else
        write_error "Source Link not configured"
    fi
fi

# Results Summary
write_header "Validation Results Summary"

TOTAL=$((PASSED + WARNINGS + FAILED))
write_info "Total validations: $TOTAL"
echo -e "${GREEN}Passed: $PASSED${NC}"
echo -e "${YELLOW}Warnings: $WARNINGS${NC}"
echo -e "${RED}Failed: $FAILED${NC}"

# Package information
if [[ -d "$OUTPUT_DIR" ]]; then
    write_header "Generated Packages"
    for PACKAGE in "$OUTPUT_DIR"/*.nupkg; do
        if [[ -f "$PACKAGE" ]]; then
            SIZE=$(du -h "$PACKAGE" | cut -f1)
            write_info "$(basename "$PACKAGE") ($SIZE)"
        fi
    done
    write_info "Packages saved to: $OUTPUT_DIR"
fi

# Exit with appropriate code
if [[ $FAILED -gt 0 ]]; then
    write_error "Validation failed with $FAILED failures"
    exit 1
elif [[ $WARNINGS -gt 0 ]]; then
    write_warning "Validation completed with $WARNINGS warnings"
    exit 0
else
    write_success "All validations passed successfully!"
    exit 0
fi