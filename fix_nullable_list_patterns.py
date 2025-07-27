#!/usr/bin/env python3
"""
Fix nullable list pattern matching issues in generated Kiota code.
Changes List<int?> pattern matches from List<int> to List<int?>.
"""

import os
import re
import glob

def fix_nullable_list_patterns(file_path):
    """Fix nullable list pattern matching in a single file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Fix the pattern: List<int?> is List<int> -> List<int?> is List<int?>
        pattern = r'(\w+\.GetCollectionOfPrimitiveValues<int\?>\(\)\?\.AsList\(\) is List<int>)'
        replacement = r'\1'.replace('List<int>', 'List<int?>')
        
        # More specific pattern matching
        fixed_content = re.sub(
            r'(GetCollectionOfPrimitiveValues<int\?>\(\)\?\.AsList\(\) is List<int>)',
            r'GetCollectionOfPrimitiveValues<int?>()?.AsList() is List<int?>',
            content
        )
        
        if fixed_content != content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(fixed_content)
            print(f"Fixed: {file_path}")
            return True
        
        return False
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def main():
    """Fix all generated files with nullable list pattern issues."""
    # Find all generated C# files
    pattern = "src/Procore.SDK.Core/Generated/**/*.cs"
    files = glob.glob(pattern, recursive=True)
    
    fixed_count = 0
    for file_path in files:
        if fix_nullable_list_patterns(file_path):
            fixed_count += 1
    
    print(f"Fixed {fixed_count} files with nullable list pattern issues.")

if __name__ == "__main__":
    main()