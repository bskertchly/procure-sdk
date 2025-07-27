#!/usr/bin/env python3
"""
Comprehensive script to fix malformed enum member values in Kiota-generated C# code.
This script addresses all the various malformed patterns found in the generated code.
"""

import os
import re
import glob

def fix_enum_issues_in_file(file_path):
    """Fix enum member value issues in a single C# file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Pattern 1: [EnumMember(Value = "undecided)] - missing closing quote
        # Replace with: [EnumMember(Value = "undecided")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)\]"',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 2: [EnumMember(Value = will_not_bid")] - missing opening quote
        # Replace with: [EnumMember(Value = "will_not_bid")]
        content = re.sub(
            r'\[EnumMember\(Value = ([^"]*)"\)\]',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 3: [EnumMember(Value = "will_bid)] - missing closing quote
        # Replace with: [EnumMember(Value = "will_bid")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)\]"',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 4: [EnumMember(Value = "some_value)] - missing closing quote and parenthesis
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)\]"',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 5: [EnumMember(Value = some_value")] - missing opening quote and parenthesis
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = ([^"]*)"\)\]',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 6: [EnumMember(Value = "some_value)] - missing closing quote and parenthesis
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)\]"',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 7: [EnumMember(Value = some_value)] - missing both quotes
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = ([^"]*)\)\]',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 8: [EnumMember(Value = "some_value)] - missing closing quote and parenthesis
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)\]"',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 9: [EnumMember(Value = some_value")] - missing opening quote and parenthesis
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = ([^"]*)"\)\]',
            r'[EnumMember(Value = "\1")]',
            content
        )
        
        # Pattern 10: [EnumMember(Value = "some_value)] - missing closing quote and parenthesis
        # Replace with: [EnumMember(Value = "some_value")]
        content = re.sub(
            r'\[EnumMember\(Value = "([^"]*)\]"',
            r'[EnumMember(Value = "\1")]',
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
        if fix_enum_issues_in_file(file_path):
            fixed_count += 1
    
    print(f"\nProcessing complete. Fixed {fixed_count} files.")

if __name__ == "__main__":
    main() 