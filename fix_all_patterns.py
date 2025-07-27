#!/usr/bin/env python3
"""
Comprehensive script to fix all malformed patterns in Kiota-generated C# code.
This script addresses various syntax issues found in the generated code.
"""

import os
import re
import glob

def fix_patterns_in_file(file_path):
    """Fix malformed patterns in a single C# file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Fix various malformed patterns
        replacements = [
            # Fix enum member values missing closing quotes
            ('[EnumMember(Value = "active)]', '[EnumMember(Value = "active")]'),
            ('[EnumMember(Value = "pending)]', '[EnumMember(Value = "pending")]'),
            ('[EnumMember(Value = "inactive)]', '[EnumMember(Value = "inactive")]'),
            ('[EnumMember(Value = "rfi)]', '[EnumMember(Value = "rfi")]'),
            ('[EnumMember(Value = "submittal_package)]', '[EnumMember(Value = "submittal_package")]'),
            ('[EnumMember(Value = "task)]', '[EnumMember(Value = "task")]'),
            ('[EnumMember(Value = "purchase_order_contract)]', '[EnumMember(Value = "purchase_order_contract")]'),
            ('[EnumMember(Value = "prime_contract)]', '[EnumMember(Value = "prime_contract")]'),
            ('[EnumMember(Value = "payment_application)]', '[EnumMember(Value = "payment_application")]'),
            ('[EnumMember(Value = "generic_tool_item)]', '[EnumMember(Value = "generic_tool_item")]'),
            ('[EnumMember(Value = "form)]', '[EnumMember(Value = "form")]'),
            ('[EnumMember(Value = "direct_cost_item)]', '[EnumMember(Value = "direct_cost_item")]'),
            ('[EnumMember(Value = "punch_item)]', '[EnumMember(Value = "punch_item")]'),
            
            # Fix other common patterns that might be malformed
            ('[EnumMember(Value = "undecided)]', '[EnumMember(Value = "undecided")]'),
            ('[EnumMember(Value = "will_not_bid)]', '[EnumMember(Value = "will_not_bid")]'),
            ('[EnumMember(Value = "will_bid)]', '[EnumMember(Value = "will_bid")]'),
            ('[EnumMember(Value = "not_invited)]', '[EnumMember(Value = "not_invited")]'),
            ('[EnumMember(Value = "submitted)]', '[EnumMember(Value = "submitted")]'),
        ]
        
        # Apply all replacements
        for old_pattern, new_pattern in replacements:
            content = content.replace(old_pattern, new_pattern)
        
        # Only write if content changed
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            print(f"Fixed: {file_path}")
            return True
        else:
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
        if fix_patterns_in_file(file_path):
            fixed_count += 1
    
    print(f"\nProcessing complete. Fixed {fixed_count} files.")

if __name__ == "__main__":
    main() 