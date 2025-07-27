#!/bin/bash
# 
# Generates Kiota clients for Procore API resource groups
#
# This script generates separate Kiota clients for different Procore API resource groups
# by filtering the large OpenAPI specification file by path patterns.
#

set -euo pipefail

# Color output functions
print_success() { echo -e "\033[32m✅ $1\033[0m"; }
print_warning() { echo -e "\033[33m⚠️  $1\033[0m"; }
print_error() { echo -e "\033[31m❌ $1\033[0m"; }
print_info() { echo -e "\033[36mℹ️  $1\033[0m"; }

# Default values
RESOURCE_GROUP="all"
OPENAPI_SPEC="docs/rest_OAS_all.json"
VALIDATE_ONLY=false
CLEAN=false

# Show usage
show_usage() {
    cat << EOF
Usage: $0 [OPTIONS]

Generates Kiota clients for Procore API resource groups

OPTIONS:
    -r, --resource-group RESOURCE    Resource group to generate (default: all)
                                   Options: all, core, project-management, quality-safety,
                                   construction-financials, field-productivity, resource-management
    -s, --spec PATH                 Path to OpenAPI spec (default: docs/rest_OAS_all.json)
    -v, --validate-only            Only validate generated code without compilation
    -c, --clean                    Clean existing generated code before generating
    -h, --help                     Show this help message

EXAMPLES:
    $0 --resource-group core
    $0 --resource-group all --clean
    $0 --validate-only
EOF
}

# Parse command line arguments
parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case $1 in
            -r|--resource-group)
                RESOURCE_GROUP="$2"
                shift 2
                ;;
            -s|--spec)
                OPENAPI_SPEC="$2"
                shift 2
                ;;
            -v|--validate-only)
                VALIDATE_ONLY=true
                shift
                ;;
            -c|--clean)
                CLEAN=true
                shift
                ;;
            -h|--help)
                show_usage
                exit 0
                ;;
            *)
                print_error "Unknown option: $1"
                show_usage
                exit 1
                ;;
        esac
    done
}

# Resource configurations using functions for compatibility
get_resource_config() {
    local resource="$1"
    local key="$2"
    
    case "$resource.$key" in
        "core.paths") echo "**/companies,**/companies/**,**/company_users/**,**/users/**,**/folders-and-files/**,**/custom-fields/**,**/configurable-field-sets/**" ;;
        "core.namespace") echo "Procore.SDK.Core" ;;
        "core.classname") echo "CoreClient" ;;
        "core.description") echo "Core functionality: companies, users, documents, custom fields" ;;
        
        "project-management.paths") echo "**/projects/**,**/workflows/**,**/task-items/**,**/project-assignments/**,**/project-users/**" ;;
        "project-management.namespace") echo "Procore.SDK.ProjectManagement" ;;
        "project-management.classname") echo "ProjectManagementClient" ;;
        "project-management.description") echo "Project management: projects, workflows, tasks, assignments" ;;
        
        "quality-safety.paths") echo "**/inspections/**,**/observations/**,**/incidents/**,**/safety/**,**/quality/**,**/punch/**" ;;
        "quality-safety.namespace") echo "Procore.SDK.QualitySafety" ;;
        "quality-safety.classname") echo "QualitySafetyClient" ;;
        "quality-safety.description") echo "Quality & safety: inspections, observations, incidents, punch lists" ;;
        
        "construction-financials.paths") echo "**/contracts/**,**/purchase-orders/**,**/budgets/**,**/cost-codes/**,**/change-orders/**,**/invoices/**,**/payments/**" ;;
        "construction-financials.namespace") echo "Procore.SDK.ConstructionFinancials" ;;
        "construction-financials.classname") echo "ConstructionFinancialsClient" ;;
        "construction-financials.description") echo "Financial management: contracts, POs, budgets, change orders, invoices" ;;
        
        "field-productivity.paths") echo "**/project_timecard_entries/**,**/timecard_entries/**,**/timecard_time_types/**,**/timesheets/**,**/project_timesheet_timecard_entries/**" ;;
        "field-productivity.namespace") echo "Procore.SDK.FieldProductivity" ;;
        "field-productivity.classname") echo "FieldProductivityClient" ;;
        "field-productivity.description") echo "Field operations: daily logs, timecards, equipment, manpower tracking" ;;
        
        "resource-management.paths") echo "**/workforce/**,**/resources/**,**/assignments/**" ;;
        "resource-management.namespace") echo "Procore.SDK.ResourceManagement" ;;
        "resource-management.classname") echo "ResourceManagementClient" ;;
        "resource-management.description") echo "Resource management: workforce, resources, assignments" ;;
        
        *) echo "" ;;
    esac
}

init_resource_configs() {
    # Function-based configuration - no initialization needed
    return 0
}

# Test prerequisites
test_prerequisites() {
    print_info "Checking prerequisites..."
    
    # Check if Kiota is installed
    if ! command -v kiota &> /dev/null; then
        print_error "Kiota CLI not found. Install with: dotnet tool install --global Microsoft.OpenApi.Kiota"
        print_info "Make sure to add ~/.dotnet/tools to your PATH"
        return 1
    fi
    
    local kiota_version
    kiota_version=$(kiota --version 2>/dev/null || echo "unknown")
    print_success "Kiota CLI found (version: $kiota_version)"
    
    # Check if OpenAPI spec exists
    if [[ ! -f "$OPENAPI_SPEC" ]]; then
        print_error "OpenAPI specification not found at: $OPENAPI_SPEC"
        return 1
    fi
    
    local spec_size
    spec_size=$(du -h "$OPENAPI_SPEC" | cut -f1)
    print_success "OpenAPI specification found ($spec_size)"
    
    return 0
}

# Create generated directory
create_generated_directory() {
    local output_path="$1"
    
    if [[ "$CLEAN" == true && -d "$output_path" ]]; then
        print_info "Cleaning existing generated code at: $output_path"
        rm -rf "$output_path"
    fi
    
    if [[ ! -d "$output_path" ]]; then
        mkdir -p "$output_path"
        print_info "Created output directory: $output_path"
    fi
}

# Fix nullable pattern matching issues in generated code
fix_nullable_patterns() {
    local output_path="$1"
    
    print_info "Fixing nullable pattern matching issues..."
    
    # Find all C# files in the generated directory
    local cs_files
    cs_files=$(find "$output_path" -name "*.cs" -type f)
    
    if [[ -z "$cs_files" ]]; then
        print_warning "No C# files found to fix"
        return 0
    fi
    
    local fixed_count=0
    
    # Process each file
    while IFS= read -r file; do
        if [[ -f "$file" ]]; then
            # Create a temporary file for the fixed content
            local temp_file="$file.tmp"
            
            # Fix the nullable list pattern matching using sed
            # Pattern: List<int?> collection is List<int> -> List<int?> collection is List<int?>
            if sed 's/List<int?>\([^>]*\) is List<int>/List<int?>\1 is List<int?>/g' "$file" > "$temp_file"; then
                # Check if changes were made
                if ! cmp -s "$file" "$temp_file"; then
                    mv "$temp_file" "$file"
                    ((fixed_count++))
                else
                    rm -f "$temp_file"
                fi
            else
                rm -f "$temp_file"
            fi
        fi
    done <<< "$cs_files"
    
    if [[ $fixed_count -gt 0 ]]; then
        print_success "Fixed nullable patterns in $fixed_count files"
    else
        print_info "No nullable pattern fixes needed"
    fi
}

# Generate client using Kiota
generate_client() {
    local name="$1"
    
    local paths=$(get_resource_config "$name" "paths")
    local namespace=$(get_resource_config "$name" "namespace")
    local classname=$(get_resource_config "$name" "classname")
    local description=$(get_resource_config "$name" "description")
    
    print_info "Generating $name client: $description"
    
    local output_path="src/$namespace/Generated"
    create_generated_directory "$output_path"
    
    # Build include path arguments
    local include_args=()
    IFS=',' read -ra PATH_ARRAY <<< "$paths"
    for path in "${PATH_ARRAY[@]}"; do
        include_args+=("--include-path" "$path")
    done
    
    # Build Kiota command
    local kiota_args=(
        "generate"
        "--openapi" "$OPENAPI_SPEC"
        "--language" "CSharp"
        "--class-name" "$classname"
        "--namespace-name" "$namespace"
        "--output" "$output_path"
        "--exclude-backward-compatible"
        "--clean-output"
        "${include_args[@]}"
    )
    
    print_info "Running: kiota ${kiota_args[*]}"
    
    # Run Kiota generation
    if kiota "${kiota_args[@]}" > "temp_output.txt" 2> "temp_error.txt"; then
        print_success "Successfully generated $name client"
        if [[ -s "temp_output.txt" ]]; then
            cat "temp_output.txt"
        fi
        rm -f "temp_output.txt" "temp_error.txt"
        
        # Fix nullable pattern matching issues in generated code
        fix_nullable_patterns "$output_path"
        
        return 0
    else
        print_error "Failed to generate $name client"
        if [[ -s "temp_error.txt" ]]; then
            cat "temp_error.txt"
        fi
        rm -f "temp_output.txt" "temp_error.txt"
        return 1
    fi
}

# Test generated code
test_generated_code() {
    local name="$1"
    
    local namespace=$(get_resource_config "$name" "namespace")
    local classname=$(get_resource_config "$name" "classname")
    
    local output_path="src/$namespace/Generated"
    
    if [[ ! -d "$output_path" ]]; then
        print_error "Generated code directory not found: $output_path"
        return 1
    fi
    
    # Check if main client file was generated
    local client_file="$output_path/$classname.cs"
    if [[ ! -f "$client_file" ]]; then
        print_warning "Main client file not found: $client_file"
        return 1
    fi
    
    # Count generated files
    local file_count
    file_count=$(find "$output_path" -type f | wc -l)
    print_success "$name client: $file_count files generated"
    
    return 0
}

# Test compilation
test_compilation() {
    print_info "Testing compilation of generated code..."
    
    if dotnet build --verbosity quiet > /dev/null 2>&1; then
        print_success "All generated code compiles successfully"
        return 0
    else
        print_error "Compilation failed:"
        dotnet build --verbosity normal
        return 1
    fi
}

# Get list of resources to generate
get_resources_to_generate() {
    if [[ "$RESOURCE_GROUP" == "all" ]]; then
        echo "core project-management quality-safety construction-financials field-productivity resource-management"
    else
        # Validate resource group
        case "$RESOURCE_GROUP" in
            core|project-management|quality-safety|construction-financials|field-productivity|resource-management)
                echo "$RESOURCE_GROUP"
                ;;
            *)
                print_error "Unknown resource group: $RESOURCE_GROUP"
                print_info "Valid options: all, core, project-management, quality-safety, construction-financials, field-productivity, resource-management"
                exit 1
                ;;
        esac
    fi
}

# Main execution function
main() {
    print_info "Procore SDK Kiota Client Generation Script"
    print_info "Resource Group: $RESOURCE_GROUP"
    
    if ! test_prerequisites; then
        exit 1
    fi
    
    local success_count=0
    local total_count=0
    
    # Get resources to generate
    local resources_to_generate
    resources_to_generate=$(get_resources_to_generate)
    
    for name in $resources_to_generate; do
        ((total_count++))
        
        echo ""
        print_info "═══════════════════════════════════════════════════════════"
        print_info "Processing: $name"
        print_info "═══════════════════════════════════════════════════════════"
        
        if generate_client "$name"; then
            if test_generated_code "$name"; then
                ((success_count++))
            fi
        fi
    done
    
    echo ""
    print_info "═══════════════════════════════════════════════════════════"
    print_info "Generation Summary"
    print_info "═══════════════════════════════════════════════════════════"
    print_info "Successfully generated: $success_count/$total_count clients"
    
    if [[ "$VALIDATE_ONLY" != true && $success_count -gt 0 ]]; then
        if test_compilation; then
            print_success "🎉 All operations completed successfully!"
        else
            print_warning "Generation succeeded but compilation failed"
            exit 1
        fi
    fi
    
    if [[ $success_count -eq $total_count ]]; then
        print_success "🎉 All client generations completed successfully!"
        exit 0
    else
        print_error "Some client generations failed"
        exit 1
    fi
}

# Initialize configurations and parse arguments
init_resource_configs
parse_arguments "$@"

# Run main function
main