#!/bin/bash

# Multi-Solution Migration Script
# Migrates Procore SDK from monolithic to multi-solution architecture

set -e

echo "🚀 Procore SDK Multi-Solution Migration"
echo "======================================="

# Configuration
GENERATED_SOLUTION="Procore.SDK.Generated"
MAIN_SOLUTION="Procore.SDK"
COMPLETE_SOLUTION="Procore.SDK.Complete"

# Function to create directory if it doesn't exist
create_dir() {
    if [ ! -d "$1" ]; then
        mkdir -p "$1"
        echo "✅ Created directory: $1"
    fi
}

# Phase 1: Create Generated Solution Structure
echo ""
echo "📋 Phase 1: Creating Generated Code Solution"
echo "============================================="

# Create generated solution directory
create_dir "../$GENERATED_SOLUTION"
cd "../$GENERATED_SOLUTION"

# Create new solution file
if [ ! -f "Procore.SDK.Generated.sln" ]; then
    dotnet new sln -n "Procore.SDK.Generated"
    echo "✅ Created Procore.SDK.Generated.sln"
fi

# Create generated projects structure
projects=(
    "Procore.SDK.Generated.Core"
    "Procore.SDK.Generated.ProjectManagement" 
    "Procore.SDK.Generated.QualitySafety"
    "Procore.SDK.Generated.FieldProductivity"
    "Procore.SDK.Generated.ConstructionFinancials"
    "Procore.SDK.Generated.ResourceManagement"
)

for project in "${projects[@]}"; do
    if [ ! -d "$project" ]; then
        echo "📦 Creating $project..."
        dotnet new classlib -n "$project" -f "net8.0;net6.0"
        
        # Add to solution
        dotnet sln add "$project/$project.csproj"
        
        # Remove default Class1.cs
        rm -f "$project/Class1.cs"
        
        echo "✅ Created $project"
    fi
done

# Create Directory.Build.props for generated solution
cat > Directory.Build.props << 'EOF'
<Project>
  <PropertyGroup>
    <!-- Multi-targeting for generated code -->
    <TargetFrameworks>net6.0;net8.0</TargetFrameworks>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    
    <!-- Generated Code Optimizations -->
    <IsGeneratedCode>true</IsGeneratedCode>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    
    <!-- Disable expensive analysis for generated code -->
    <EnableNETAnalyzers>false</EnableNETAnalyzers>
    <RunAnalyzersDuringBuild>false</RunAnalyzersDuringBuild>
    <EnforceCodeStyleInBuild>false</EnforceCodeStyleInBuild>
    <RunNullableAnnotations>false</RunNullableAnnotations>
    <EnableSecurityCodeScan>false</EnableSecurityCodeScan>
    
    <!-- Performance optimizations -->
    <UseSharedCompilation>true</UseSharedCompilation>
    <BuildInParallel>true</BuildInParallel>
    <RestoreParallel>true</RestoreParallel>
    <MultiProcessorCompilation>true</MultiProcessorCompilation>
    
    <!-- Package metadata -->
    <Company>Bryan Skertchly</Company>
    <Authors>Bryan Skertchly</Authors>
    <Product>Procore SDK Generated Clients</Product>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <RepositoryUrl>https://github.com/bskertchly/procore-sdk</RepositoryUrl>
    <PackageTags>procore;construction;api;sdk;generated;kiota</PackageTags>
  </PropertyGroup>
</Project>
EOF

echo "✅ Created optimized Directory.Build.props for generated code"

# Create generation script
cat > scripts/generate-all-clients.sh << 'EOF'
#!/bin/bash

echo "🤖 Regenerating all Kiota clients..."
echo "===================================="

# Add your Kiota generation commands here
# Example:
# kiota generate -l CSharp -c ProjectManagementClient -n Procore.SDK.Generated.ProjectManagement -o ./Procore.SDK.Generated.ProjectManagement/

echo "⚠️  TODO: Add your specific Kiota generation commands"
echo "📝 Edit this file: scripts/generate-all-clients.sh"

echo "✅ Generation complete!"
EOF

create_dir "scripts"
chmod +x scripts/generate-all-clients.sh
echo "✅ Created generation automation script"

cd "../procore-sdk"

echo ""
echo "🎉 Phase 1 Complete!"
echo "==================="
echo ""
echo "📁 Created Generated Solution Structure:"
echo "   📂 ../$GENERATED_SOLUTION/"
echo "   ├── 📋 Procore.SDK.Generated.sln"
echo "   ├── 📦 6 generated project stubs"
echo "   ├── 🔧 Optimized Directory.Build.props"
echo "   └── 🤖 scripts/generate-all-clients.sh"
echo ""
echo "🚀 Next Steps:"
echo "1. 📝 Update scripts/generate-all-clients.sh with your Kiota commands"
echo "2. 🔄 Move Generated/ folders from current projects to new solution"
echo "3. 📦 Set up NuGet packaging"
echo "4. 🔗 Update main solution to reference generated packages"
echo ""
echo "💡 Run this script with '--phase2' to continue with main solution updates"

# TODO: Add Phase 2 implementation
if [ "$1" = "--phase2" ]; then
    echo "🚧 Phase 2 implementation coming soon..."
    echo "   This will update the main solution to reference generated packages"
fi