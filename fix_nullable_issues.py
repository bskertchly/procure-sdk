#!/usr/bin/env python3
"""
Script to fix nullable reference type issues in Kiota-generated C# code.
This script addresses the pattern matching issue where List<int?> cannot be handled by pattern of type List<int>.
"""

import os
import re
import glob

def fix_nullable_issues_in_file(file_path):
    """Fix nullable reference type issues in a single C# file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Fix pattern matching issues: List<int?> cannot be handled by pattern of type List<int>
        # Pattern: else if(parseNode.GetCollectionOfPrimitiveValues<int?>()?.AsList() is List<int> integerValue)
        # Replace with: else if(parseNode.GetCollectionOfPrimitiveValues<int?>()?.AsList() is List<int?> integerValue)
        content = re.sub(
            r'else if\(parseNode\.GetCollectionOfPrimitiveValues<int\?>\(\)\?\.AsList\(\) is List<int> integerValue\)',
            r'else if(parseNode.GetCollectionOfPrimitiveValues<int?>()?.AsList() is List<int?> integerValue)',
            content
        )
        
        # Fix enum member value issues with unescaped quotes
        # Pattern: [EnumMember(Value = "Any value present in the Company list of Units of Measure except those categorized as "Time"")]
        # Replace with: [EnumMember(Value = "Any value present in the Company list of Units of Measure except those categorized as Time")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)"([^"]*)"([^"]*)"\)\]',
            r'[EnumMember(Value = "\1\2\3")]',
            content
        )
        
        # Fix malformed XML comments in exception cref attributes
        # Pattern: /// <exception cref="List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_items.Bulk_update.Bulk_update>">When receiving a 422 status code</exception>
        # Replace with: /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_items.Bulk_update.Bulk_update">When receiving a 422 status code</exception>
        content = re.sub(
            r'/// <exception cref="List<([^>]+)>">',
            r'/// <exception cref="\1">',
            content
        )
        
        # Only write if content changed
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            print(f"Fixed: {file_path}")
            return True
        else:
            print(f"No changes needed: {file_path}")
            return False
            
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def main():
    """Main function to process all generated C# files."""
    generated_dir = "src/Procore.SDK.Core/Generated"
    
    if not os.path.exists(generated_dir):
        print(f"Generated directory not found: {generated_dir}")
        return
    
    # Find all C# files in the generated directory
    csharp_files = glob.glob(f"{generated_dir}/**/*.cs", recursive=True)
    
    if not csharp_files:
        print("No C# files found in generated directory")
        return
    
    print(f"Found {len(csharp_files)} C# files to process")
    
    fixed_count = 0
    for file_path in csharp_files:
        if fix_nullable_issues_in_file(file_path):
            fixed_count += 1
    
    print(f"\nProcessing complete. Fixed {fixed_count} files.")

if __name__ == "__main__":
    main() 