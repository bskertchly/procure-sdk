#!/usr/bin/env python3
"""
Comprehensive script to fix all remaining malformed enum member values that are missing closing quotes.
This script addresses the pattern: [EnumMember(Value = "some_value)] -> [EnumMember(Value = "some_value")]
"""

import os
import re
import glob

def fix_missing_quotes_in_file(file_path):
    """Fix missing closing quotes in enum member values in a single C# file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Use regex to find and fix all patterns where the closing quote is missing
        # Pattern: [EnumMember(Value = "some_value)] - missing closing quote
        # Replace with: [EnumMember(Value = "some_value")]
        pattern = r'\[EnumMember\(Value = "([^"]*)\]"'
        replacement = r'[EnumMember(Value = "\1")]'
        
        # Apply the regex replacement
        content = re.sub(pattern, replacement, content)
        
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
        if fix_missing_quotes_in_file(file_path):
            fixed_count += 1
    
    print(f"\nProcessing complete. Fixed {fixed_count} files.")

if __name__ == "__main__":
    main() 