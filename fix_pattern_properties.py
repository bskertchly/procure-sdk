#!/usr/bin/env python3
"""
Script to fix patternProperties issues in OpenAPI specification.
patternProperties is not a valid property in OpenAPI 3.0 schemas.
This script converts them to additionalProperties or removes them.
"""

import json
import sys
from typing import Dict, Any, List

def fix_pattern_properties(obj: Any) -> Any:
    """
    Recursively fix patternProperties in the JSON object.
    Converts patternProperties to additionalProperties or removes them.
    """
    if isinstance(obj, dict):
        # Check if this object has patternProperties
        if "patternProperties" in obj:
            pattern_props = obj["patternProperties"]
            
            # If patternProperties has a single pattern like "^.*$", convert to additionalProperties
            if len(pattern_props) == 1 and "^.*$" in pattern_props:
                obj["additionalProperties"] = pattern_props["^.*$"]
                del obj["patternProperties"]
                print(f"Converted patternProperties to additionalProperties")
            else:
                # For other cases, just remove patternProperties
                del obj["patternProperties"]
                print(f"Removed patternProperties with patterns: {list(pattern_props.keys())}")
        
        # Recursively process all values in the dictionary
        for key, value in obj.items():
            obj[key] = fix_pattern_properties(value)
            
    elif isinstance(obj, list):
        # Recursively process all items in the list
        return [fix_pattern_properties(item) for item in obj]
    
    return obj

def main():
    input_file = "docs/rest_OAS_all.json"
    output_file = "docs/rest_OAS_all_fixed.json"
    
    try:
        print(f"Reading {input_file}...")
        with open(input_file, 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        print("Fixing patternProperties issues...")
        fixed_data = fix_pattern_properties(data)
        
        print(f"Writing fixed specification to {output_file}...")
        with open(output_file, 'w', encoding='utf-8') as f:
            json.dump(fixed_data, f, indent=2, ensure_ascii=False)
        
        print("✅ Successfully fixed patternProperties issues!")
        print(f"Fixed specification saved to: {output_file}")
        
        # Verify no patternProperties remain
        remaining_pattern_props = []
        def check_pattern_props(obj, path=""):
            if isinstance(obj, dict):
                if "patternProperties" in obj:
                    remaining_pattern_props.append(f"{path}.patternProperties")
                for key, value in obj.items():
                    check_pattern_props(value, f"{path}.{key}" if path else key)
            elif isinstance(obj, list):
                for i, item in enumerate(obj):
                    check_pattern_props(item, f"{path}[{i}]")
        
        check_pattern_props(fixed_data)
        
        if remaining_pattern_props:
            print(f"⚠️  Warning: {len(remaining_pattern_props)} patternProperties still found:")
            for path in remaining_pattern_props[:10]:  # Show first 10
                print(f"  - {path}")
            if len(remaining_pattern_props) > 10:
                print(f"  ... and {len(remaining_pattern_props) - 10} more")
        else:
            print("✅ No remaining patternProperties found!")
            
    except FileNotFoundError:
        print(f"❌ Error: File {input_file} not found!")
        sys.exit(1)
    except json.JSONDecodeError as e:
        print(f"❌ Error: Invalid JSON in {input_file}: {e}")
        sys.exit(1)
    except Exception as e:
        print(f"❌ Error: {e}")
        sys.exit(1)

if __name__ == "__main__":
    main() 