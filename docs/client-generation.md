# Kiota Client Generation Guide

This document explains how to generate and maintain the Kiota clients for the Procore SDK.

## Overview

The Procore SDK uses Microsoft Kiota to generate strongly-typed C# clients from the Procore OpenAPI specification. Due to the large size of the Procore API (34MB, 1,439 endpoints), we use a strategic filtering approach to generate separate clients for different resource groups.

## Prerequisites

1. **Kiota CLI Tool**: Install globally with:
   ```bash
   dotnet tool install --global Microsoft.OpenApi.Kiota
   ```

2. **OpenAPI Specification**: The Procore OpenAPI spec should be located at `docs/rest_OAS_all.json` (34MB file)

3. **PowerShell Core or Bash**: For running the generation scripts

## Client Architecture

### Resource Groups

The SDK is organized into 6 resource-specific clients:

| Client | Resource Group | API Paths Covered |
|--------|----------------|-------------------|
| **Core** | Foundation | companies, users, documents, custom fields |
| **ProjectManagement** | Project Operations | projects, workflows, tasks, assignments |
| **QualitySafety** | Quality & Safety | inspections, observations, incidents, safety |
| **ConstructionFinancials** | Financial | contracts, purchase orders, budgets, invoices |
| **FieldProductivity** | Field Operations | daily logs, timecards, equipment, manpower |
| **ResourceManagement** | Resources | workforce, resources, assignments |

### Benefits of This Approach

- **Manageable Size**: Each client is focused and smaller than a monolithic SDK
- **Faster Compilation**: Reduced compilation time and better IntelliSense performance
- **Modular Dependencies**: Use only the resource clients you need
- **Independent Versioning**: Update clients independently as needed

## Generation Scripts

### PowerShell Script (Windows/Cross-platform)

```powershell
# Generate all clients
./tools/Generate-Clients.ps1

# Generate specific client
./tools/Generate-Clients.ps1 -ResourceGroup core

# Clean and regenerate
./tools/Generate-Clients.ps1 -Clean

# Validate only (no compilation)
./tools/Generate-Clients.ps1 -ValidateOnly
```

### Bash Script (Linux/macOS)

```bash
# Generate all clients
./tools/generate-clients.sh

# Generate specific client
./tools/generate-clients.sh --resource-group core

# Clean and regenerate
./tools/generate-clients.sh --clean

# Validate only
./tools/generate-clients.sh --validate-only
```

## Manual Generation

If you need to generate a client manually, use the Kiota CLI directly:

```bash
# Example: Core client generation
kiota generate \
  --openapi docs/rest_OAS_all.json \
  --language CSharp \
  --class-name CoreClient \
  --namespace-name Procore.SDK.Core \
  --output src/Procore.SDK.Core/Generated \
  --include-path "**/companies/**" \
  --include-path "**/users/**" \
  --include-path "**/custom-fields/**" \
  --exclude-backward-compatible \
  --clean-output
```

### Path Filtering Patterns

Each client uses specific path patterns to filter the OpenAPI specification:

#### Core Client Paths
```
**/companies/**
**/company_users/**
**/users/**
**/folders-and-files/**
**/custom-fields/**
**/configurable-field-sets/**
```

#### Project Management Client Paths
```
**/projects/**
**/workflows/**
**/task-items/**
**/project-assignments/**
**/project-users/**
```

#### Quality & Safety Client Paths
```
**/inspections/**
**/observations/**
**/incidents/**
**/safety/**
**/quality/**
**/punch/**
```

#### Construction Financials Client Paths
```
**/contracts/**
**/purchase-orders/**
**/budgets/**
**/cost-codes/**
**/change-orders/**
**/invoices/**
**/payments/**
```

#### Field Productivity Client Paths
```
**/daily-logs/**
**/timecards/**
**/equipment/**
**/manpower/**
**/deliveries/**
```

#### Resource Management Client Paths
```
**/workforce/**
**/resources/**
**/assignments/**
```

## Generated Code Structure

Each generated client follows this structure:

```
src/Procore.SDK.{ResourceGroup}/
├── Generated/
│   ├── {ResourceGroup}Client.cs      # Main client class
│   ├── Rest/                         # API endpoint classes
│   ├── Models/                       # Request/response models
│   └── kiota-lock.json              # Generation metadata
├── {ResourceGroup}Client.cs          # Wrapper class (manual)
├── I{ResourceGroup}Client.cs         # Interface (manual)
└── Extensions/                       # Extension methods (manual)
```

## Dependencies

Each client project requires these NuGet packages:

```xml
<PackageReference Include="Microsoft.Kiota.Abstractions" />
<PackageReference Include="Microsoft.Kiota.Http.HttpClientLibrary" />
<PackageReference Include="Microsoft.Kiota.Serialization.Json" />
<PackageReference Include="Microsoft.Kiota.Serialization.Form" />
<PackageReference Include="Microsoft.Kiota.Serialization.Text" />
<PackageReference Include="Microsoft.Kiota.Serialization.Multipart" />
```

## Build Integration

### Automatic Generation

The `tools/Build-Integration.ps1` script can automatically regenerate clients when the OpenAPI spec changes:

```powershell
# Regenerate if spec changed, then build
./tools/Build-Integration.ps1

# Force regeneration
./tools/Build-Integration.ps1 -Force

# Skip generation, just build
./tools/Build-Integration.ps1 -SkipGeneration
```

### Generation Marker

The build integration uses a `.last-generation` file to track when clients were last generated. This prevents unnecessary regeneration when the OpenAPI spec hasn't changed.

## Quality Validation

Use the validation script to check generated code quality:

```powershell
# Validate all clients
./tools/Validate-GeneratedCode.ps1

# Validate specific client with details
./tools/Validate-GeneratedCode.ps1 -ResourceGroup core -DetailedOutput
```

The validation checks:
- Directory structure correctness
- Code quality and syntax
- Compilation success
- API surface analysis
- Required dependencies

## Common Issues and Solutions

### Compilation Errors

The generated code may have compilation errors due to OpenAPI spec issues:

1. **Nullable Type Conflicts**: These are common with large specs and usually don't affect functionality
2. **Missing Discriminators**: Polymorphic types without discriminators cause warnings
3. **XML Documentation Errors**: Malformed XML comments in the OpenAPI spec

Most of these can be safely ignored or worked around in wrapper classes.

### Large Generation Times

- Core client generation takes ~30 seconds due to the filtered scope
- Full generation of all clients may take 3-5 minutes
- Use specific resource group generation during development

### Path Pattern Updates

If you need to modify which endpoints are included:

1. Update the path patterns in the generation scripts
2. Regenerate the affected client(s)
3. Test compilation and functionality

## Troubleshooting

### Kiota Not Found
```bash
# Ensure Kiota is installed and in PATH
dotnet tool install --global Microsoft.OpenApi.Kiota
export PATH="$PATH:$HOME/.dotnet/tools"
```

### Generation Fails
1. Check that `docs/rest_OAS_all.json` exists and is valid
2. Verify path patterns match actual API paths in the spec
3. Check available disk space (generation creates many files)

### Compilation Fails
1. Ensure all required Kiota packages are referenced
2. Check for project reference issues
3. Try cleaning and rebuilding: `dotnet clean && dotnet build`

## Maintenance

### Updating to New OpenAPI Specs

1. Download the new Procore OpenAPI specification
2. Replace `docs/rest_OAS_all.json`
3. Run generation scripts to update all clients
4. Test compilation and basic functionality
5. Update any wrapper classes if needed

### Adding New Resource Groups

1. Add new project to the solution
2. Update generation scripts with new resource configuration
3. Add appropriate path patterns for the new resource
4. Generate and test the new client

### Performance Optimization

- Use `--exclude-backward-compatible` to reduce generated code size
- Consider further path filtering if clients are still too large
- Use `--clean-output` to ensure clean regeneration

## Best Practices

1. **Always use the generation scripts** rather than manual Kiota commands
2. **Test compilation after regeneration** to catch breaking changes early
3. **Keep wrapper classes minimal** - let Kiota handle the heavy lifting
4. **Version control generated code** for easier debugging and rollbacks
5. **Document any manual modifications** to generated code (though this should be avoided)

## Support

For issues with:
- **Kiota itself**: See [Microsoft Kiota documentation](https://learn.microsoft.com/en-us/openapi/kiota/)
- **Procore API**: See [Procore Developer Portal](https://developers.procore.com/)
- **SDK Generation**: Check this guide or raise an issue in the project repository