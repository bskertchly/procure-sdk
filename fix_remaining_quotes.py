#!/usr/bin/env python3
"""
Simple script to fix remaining malformed enum member values.
"""

import os
import glob

def fix_quotes_in_file(file_path):
    """Fix missing closing quotes in enum member values."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Common malformed patterns and their fixes
        replacements = [
            ('[EnumMember(Value = "created_at)]', '[EnumMember(Value = "created_at")]'),
            ('[EnumMember(Value = "updated_at)]', '[EnumMember(Value = "updated_at")]'),
            ('[EnumMember(Value = "name)]', '[EnumMember(Value = "name")]'),
            ('[EnumMember(Value = "id)]', '[EnumMember(Value = "id")]'),
            ('[EnumMember(Value = "status)]', '[EnumMember(Value = "status")]'),
            ('[EnumMember(Value = "type)]', '[EnumMember(Value = "type")]'),
            ('[EnumMember(Value = "date)]', '[EnumMember(Value = "date")]'),
            ('[EnumMember(Value = "description)]', '[EnumMember(Value = "description")]'),
            ('[EnumMember(Value = "title)]', '[EnumMember(Value = "title")]'),
            ('[EnumMember(Value = "code)]', '[EnumMember(Value = "code")]'),
            ('[EnumMember(Value = "value)]', '[EnumMember(Value = "value")]'),
            ('[EnumMember(Value = "label)]', '[EnumMember(Value = "label")]'),
            ('[EnumMember(Value = "key)]', '[EnumMember(Value = "key")]'),
            ('[EnumMember(Value = "text)]', '[EnumMember(Value = "text")]'),
            ('[EnumMember(Value = "data)]', '[EnumMember(Value = "data")]'),
            ('[EnumMember(Value = "info)]', '[EnumMember(Value = "info")]'),
            ('[EnumMember(Value = "detail)]', '[EnumMember(Value = "detail")]'),
            ('[EnumMember(Value = "content)]', '[EnumMember(Value = "content")]'),
            ('[EnumMember(Value = "format)]', '[EnumMember(Value = "format")]'),
            ('[EnumMember(Value = "version)]', '[EnumMember(Value = "version")]'),
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
        if fix_quotes_in_file(file_path):
            fixed_count += 1
    
    print(f"\nProcessing complete. Fixed {fixed_count} files.")

if __name__ == "__main__":
    main() 