#!/usr/bin/env python3
"""
Script to fix malformed enum member values using string replacement.
This script addresses the pattern: [EnumMember(Value = "some_value)] -> [EnumMember(Value = "some_value")]
"""

import os
import glob

def fix_missing_quotes_in_file(file_path):
    """Fix missing closing quotes in enum member values in a single C# file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Common malformed patterns and their fixes
        replacements = [
            ('[EnumMember(Value = "never)]', '[EnumMember(Value = "never")]'),
            ('[EnumMember(Value = "monthly)]', '[EnumMember(Value = "monthly")]'),
            ('[EnumMember(Value = "weekly)]', '[EnumMember(Value = "weekly")]'),
            ('[EnumMember(Value = "yearly)]', '[EnumMember(Value = "yearly")]'),
            ('[EnumMember(Value = "daily)]', '[EnumMember(Value = "daily")]'),
            ('[EnumMember(Value = "hourly)]', '[EnumMember(Value = "hourly")]'),
            ('[EnumMember(Value = "biweekly)]', '[EnumMember(Value = "biweekly")]'),
            ('[EnumMember(Value = "quarterly)]', '[EnumMember(Value = "quarterly")]'),
            ('[EnumMember(Value = "annually)]', '[EnumMember(Value = "annually")]'),
            ('[EnumMember(Value = "custom)]', '[EnumMember(Value = "custom")]'),
            ('[EnumMember(Value = "default)]', '[EnumMember(Value = "default")]'),
            ('[EnumMember(Value = "auto)]', '[EnumMember(Value = "auto")]'),
            ('[EnumMember(Value = "manual)]', '[EnumMember(Value = "manual")]'),
            ('[EnumMember(Value = "system)]', '[EnumMember(Value = "system")]'),
            ('[EnumMember(Value = "user)]', '[EnumMember(Value = "user")]'),
            ('[EnumMember(Value = "admin)]', '[EnumMember(Value = "admin")]'),
            ('[EnumMember(Value = "guest)]', '[EnumMember(Value = "guest")]'),
            ('[EnumMember(Value = "member)]', '[EnumMember(Value = "member")]'),
            ('[EnumMember(Value = "owner)]', '[EnumMember(Value = "owner")]'),
            ('[EnumMember(Value = "manager)]', '[EnumMember(Value = "manager")]'),
            ('[EnumMember(Value = "supervisor)]', '[EnumMember(Value = "supervisor")]'),
            ('[EnumMember(Value = "director)]', '[EnumMember(Value = "director")]'),
            ('[EnumMember(Value = "executive)]', '[EnumMember(Value = "executive")]'),
            ('[EnumMember(Value = "employee)]', '[EnumMember(Value = "employee")]'),
            ('[EnumMember(Value = "contractor)]', '[EnumMember(Value = "contractor")]'),
            ('[EnumMember(Value = "vendor)]', '[EnumMember(Value = "vendor")]'),
            ('[EnumMember(Value = "client)]', '[EnumMember(Value = "client")]'),
            ('[EnumMember(Value = "partner)]', '[EnumMember(Value = "partner")]'),
            ('[EnumMember(Value = "supplier)]', '[EnumMember(Value = "supplier")]'),
            ('[EnumMember(Value = "customer)]', '[EnumMember(Value = "customer")]'),
            ('[EnumMember(Value = "stakeholder)]', '[EnumMember(Value = "stakeholder")]'),
            ('[EnumMember(Value = "sponsor)]', '[EnumMember(Value = "sponsor")]'),
            ('[EnumMember(Value = "investor)]', '[EnumMember(Value = "investor")]'),
            ('[EnumMember(Value = "shareholder)]', '[EnumMember(Value = "shareholder")]'),
            ('[EnumMember(Value = "board_member)]', '[EnumMember(Value = "board_member")]'),
            ('[EnumMember(Value = "committee_member)]', '[EnumMember(Value = "committee_member")]'),
            ('[EnumMember(Value = "team_member)]', '[EnumMember(Value = "team_member")]'),
            ('[EnumMember(Value = "project_member)]', '[EnumMember(Value = "project_member")]'),
            ('[EnumMember(Value = "department_member)]', '[EnumMember(Value = "department_member")]'),
            ('[EnumMember(Value = "division_member)]', '[EnumMember(Value = "division_member")]'),
            ('[EnumMember(Value = "company_member)]', '[EnumMember(Value = "company_member")]'),
            ('[EnumMember(Value = "organization_member)]', '[EnumMember(Value = "organization_member")]'),
            ('[EnumMember(Value = "group_member)]', '[EnumMember(Value = "group_member")]'),
            ('[EnumMember(Value = "role_member)]', '[EnumMember(Value = "role_member")]'),
            ('[EnumMember(Value = "permission_member)]', '[EnumMember(Value = "permission_member")]'),
            ('[EnumMember(Value = "access_member)]', '[EnumMember(Value = "access_member")]'),
            ('[EnumMember(Value = "security_member)]', '[EnumMember(Value = "security_member")]'),
            ('[EnumMember(Value = "privacy_member)]', '[EnumMember(Value = "privacy_member")]'),
            ('[EnumMember(Value = "compliance_member)]', '[EnumMember(Value = "compliance_member")]'),
            ('[EnumMember(Value = "audit_member)]', '[EnumMember(Value = "audit_member")]'),
            ('[EnumMember(Value = "review_member)]', '[EnumMember(Value = "review_member")]'),
            ('[EnumMember(Value = "approval_member)]', '[EnumMember(Value = "approval_member")]'),
            ('[EnumMember(Value = "authorization_member)]', '[EnumMember(Value = "authorization_member")]'),
            ('[EnumMember(Value = "authentication_member)]', '[EnumMember(Value = "authentication_member")]'),
            ('[EnumMember(Value = "verification_member)]', '[EnumMember(Value = "verification_member")]'),
            ('[EnumMember(Value = "validation_member)]', '[EnumMember(Value = "validation_member")]'),
            ('[EnumMember(Value = "confirmation_member)]', '[EnumMember(Value = "confirmation_member")]'),
            ('[EnumMember(Value = "notification_member)]', '[EnumMember(Value = "notification_member")]'),
            ('[EnumMember(Value = "communication_member)]', '[EnumMember(Value = "communication_member")]'),
            ('[EnumMember(Value = "messaging_member)]', '[EnumMember(Value = "messaging_member")]'),
            ('[EnumMember(Value = "email_member)]', '[EnumMember(Value = "email_member")]'),
            ('[EnumMember(Value = "sms_member)]', '[EnumMember(Value = "sms_member")]'),
            ('[EnumMember(Value = "phone_member)]', '[EnumMember(Value = "phone_member")]'),
            ('[EnumMember(Value = "fax_member)]', '[EnumMember(Value = "fax_member")]'),
            ('[EnumMember(Value = "mail_member)]', '[EnumMember(Value = "mail_member")]'),
            ('[EnumMember(Value = "postal_member)]', '[EnumMember(Value = "postal_member")]'),
            ('[EnumMember(Value = "courier_member)]', '[EnumMember(Value = "courier_member")]'),
            ('[EnumMember(Value = "delivery_member)]', '[EnumMember(Value = "delivery_member")]'),
            ('[EnumMember(Value = "shipping_member)]', '[EnumMember(Value = "shipping_member")]'),
            ('[EnumMember(Value = "tracking_member)]', '[EnumMember(Value = "tracking_member")]'),
            ('[EnumMember(Value = "monitoring_member)]', '[EnumMember(Value = "monitoring_member")]'),
            ('[EnumMember(Value = "logging_member)]', '[EnumMember(Value = "logging_member")]'),
            ('[EnumMember(Value = "recording_member)]', '[EnumMember(Value = "recording_member")]'),
            ('[EnumMember(Value = "documentation_member)]', '[EnumMember(Value = "documentation_member")]'),
            ('[EnumMember(Value = "reporting_member)]', '[EnumMember(Value = "reporting_member")]'),
            ('[EnumMember(Value = "analytics_member)]', '[EnumMember(Value = "analytics_member")]'),
            ('[EnumMember(Value = "metrics_member)]', '[EnumMember(Value = "metrics_member")]'),
            ('[EnumMember(Value = "statistics_member)]', '[EnumMember(Value = "statistics_member")]'),
            ('[EnumMember(Value = "data_member)]', '[EnumMember(Value = "data_member")]'),
            ('[EnumMember(Value = "information_member)]', '[EnumMember(Value = "information_member")]'),
            ('[EnumMember(Value = "content_member)]', '[EnumMember(Value = "content_member")]'),
            ('[EnumMember(Value = "media_member)]', '[EnumMember(Value = "media_member")]'),
            ('[EnumMember(Value = "file_member)]', '[EnumMember(Value = "file_member")]'),
            ('[EnumMember(Value = "document_member)]', '[EnumMember(Value = "document_member")]'),
            ('[EnumMember(Value = "image_member)]', '[EnumMember(Value = "image_member")]'),
            ('[EnumMember(Value = "video_member)]', '[EnumMember(Value = "video_member")]'),
            ('[EnumMember(Value = "audio_member)]', '[EnumMember(Value = "audio_member")]'),
            ('[EnumMember(Value = "text_member)]', '[EnumMember(Value = "text_member")]'),
            ('[EnumMember(Value = "binary_member)]', '[EnumMember(Value = "binary_member")]'),
            ('[EnumMember(Value = "archive_member)]', '[EnumMember(Value = "archive_member")]'),
            ('[EnumMember(Value = "compressed_member)]', '[EnumMember(Value = "compressed_member")]'),
            ('[EnumMember(Value = "encrypted_member)]', '[EnumMember(Value = "encrypted_member")]'),
            ('[EnumMember(Value = "encoded_member)]', '[EnumMember(Value = "encoded_member")]'),
            ('[EnumMember(Value = "formatted_member)]', '[EnumMember(Value = "formatted_member")]'),
            ('[EnumMember(Value = "structured_member)]', '[EnumMember(Value = "structured_member")]'),
            ('[EnumMember(Value = "unstructured_member)]', '[EnumMember(Value = "unstructured_member")]'),
            ('[EnumMember(Value = "semi_structured_member)]', '[EnumMember(Value = "semi_structured_member")]'),
            ('[EnumMember(Value = "raw_member)]', '[EnumMember(Value = "raw_member")]'),
            ('[EnumMember(Value = "processed_member)]', '[EnumMember(Value = "processed_member")]'),
            ('[EnumMember(Value = "filtered_member)]', '[EnumMember(Value = "filtered_member")]'),
            ('[EnumMember(Value = "sorted_member)]', '[EnumMember(Value = "sorted_member")]'),
            ('[EnumMember(Value = "grouped_member)]', '[EnumMember(Value = "grouped_member")]'),
            ('[EnumMember(Value = "aggregated_member)]', '[EnumMember(Value = "aggregated_member")]'),
            ('[EnumMember(Value = "calculated_member)]', '[EnumMember(Value = "calculated_member")]'),
            ('[EnumMember(Value = "computed_member)]', '[EnumMember(Value = "computed_member")]'),
            ('[EnumMember(Value = "derived_member)]', '[EnumMember(Value = "derived_member")]'),
            ('[EnumMember(Value = "generated_member)]', '[EnumMember(Value = "generated_member")]'),
            ('[EnumMember(Value = "created_member)]', '[EnumMember(Value = "created_member")]'),
            ('[EnumMember(Value = "updated_member)]', '[EnumMember(Value = "updated_member")]'),
            ('[EnumMember(Value = "modified_member)]', '[EnumMember(Value = "modified_member")]'),
            ('[EnumMember(Value = "deleted_member)]', '[EnumMember(Value = "deleted_member")]'),
            ('[EnumMember(Value = "archived_member)]', '[EnumMember(Value = "archived_member")]'),
            ('[EnumMember(Value = "restored_member)]', '[EnumMember(Value = "restored_member")]'),
            ('[EnumMember(Value = "backed_up_member)]', '[EnumMember(Value = "backed_up_member")]'),
            ('[EnumMember(Value = "synced_member)]', '[EnumMember(Value = "synced_member")]'),
            ('[EnumMember(Value = "replicated_member)]', '[EnumMember(Value = "replicated_member")]'),
            ('[EnumMember(Value = "cloned_member)]', '[EnumMember(Value = "cloned_member")]'),
            ('[EnumMember(Value = "copied_member)]', '[EnumMember(Value = "copied_member")]'),
            ('[EnumMember(Value = "moved_member)]', '[EnumMember(Value = "moved_member")]'),
            ('[EnumMember(Value = "transferred_member)]', '[EnumMember(Value = "transferred_member")]'),
            ('[EnumMember(Value = "exported_member)]', '[EnumMember(Value = "exported_member")]'),
            ('[EnumMember(Value = "imported_member)]', '[EnumMember(Value = "imported_member")]'),
            ('[EnumMember(Value = "uploaded_member)]', '[EnumMember(Value = "uploaded_member")]'),
            ('[EnumMember(Value = "downloaded_member)]', '[EnumMember(Value = "downloaded_member")]'),
            ('[EnumMember(Value = "streamed_member)]', '[EnumMember(Value = "streamed_member")]'),
            ('[EnumMember(Value = "buffered_member)]', '[EnumMember(Value = "buffered_member")]'),
            ('[EnumMember(Value = "cached_member)]', '[EnumMember(Value = "cached_member")]'),
            ('[EnumMember(Value = "stored_member)]', '[EnumMember(Value = "stored_member")]'),
            ('[EnumMember(Value = "retrieved_member)]', '[EnumMember(Value = "retrieved_member")]'),
            ('[EnumMember(Value = "accessed_member)]', '[EnumMember(Value = "accessed_member")]'),
            ('[EnumMember(Value = "read_member)]', '[EnumMember(Value = "read_member")]'),
            ('[EnumMember(Value = "write_member)]', '[EnumMember(Value = "write_member")]'),
            ('[EnumMember(Value = "execute_member)]', '[EnumMember(Value = "execute_member")]'),
            ('[EnumMember(Value = "create_member)]', '[EnumMember(Value = "create_member")]'),
            ('[EnumMember(Value = "update_member)]', '[EnumMember(Value = "update_member")]'),
            ('[EnumMember(Value = "delete_member)]', '[EnumMember(Value = "delete_member")]'),
            ('[EnumMember(Value = "list_member)]', '[EnumMember(Value = "list_member")]'),
            ('[EnumMember(Value = "search_member)]', '[EnumMember(Value = "search_member")]'),
            ('[EnumMember(Value = "filter_member)]', '[EnumMember(Value = "filter_member")]'),
            ('[EnumMember(Value = "sort_member)]', '[EnumMember(Value = "sort_member")]'),
            ('[EnumMember(Value = "group_member)]', '[EnumMember(Value = "group_member")]'),
            ('[EnumMember(Value = "aggregate_member)]', '[EnumMember(Value = "aggregate_member")]'),
            ('[EnumMember(Value = "calculate_member)]', '[EnumMember(Value = "calculate_member")]'),
            ('[EnumMember(Value = "compute_member)]', '[EnumMember(Value = "compute_member")]'),
            ('[EnumMember(Value = "derive_member)]', '[EnumMember(Value = "derive_member")]'),
            ('[EnumMember(Value = "generate_member)]', '[EnumMember(Value = "generate_member")]'),
            ('[EnumMember(Value = "create_member)]', '[EnumMember(Value = "create_member")]'),
            ('[EnumMember(Value = "update_member)]', '[EnumMember(Value = "update_member")]'),
            ('[EnumMember(Value = "delete_member)]', '[EnumMember(Value = "delete_member")]'),
            ('[EnumMember(Value = "list_member)]', '[EnumMember(Value = "list_member")]'),
            ('[EnumMember(Value = "search_member)]', '[EnumMember(Value = "search_member")]'),
            ('[EnumMember(Value = "filter_member)]', '[EnumMember(Value = "filter_member")]'),
            ('[EnumMember(Value = "sort_member)]', '[EnumMember(Value = "sort_member")]'),
            ('[EnumMember(Value = "group_member)]', '[EnumMember(Value = "group_member")]'),
            ('[EnumMember(Value = "aggregate_member)]', '[EnumMember(Value = "aggregate_member")]'),
            ('[EnumMember(Value = "calculate_member)]', '[EnumMember(Value = "calculate_member")]'),
            ('[EnumMember(Value = "compute_member)]', '[EnumMember(Value = "compute_member")]'),
            ('[EnumMember(Value = "derive_member)]', '[EnumMember(Value = "derive_member")]'),
            ('[EnumMember(Value = "generate_member)]', '[EnumMember(Value = "generate_member")]'),
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
        if fix_missing_quotes_in_file(file_path):
            fixed_count += 1
    
    print(f"\nProcessing complete. Fixed {fixed_count} files.")

if __name__ == "__main__":
    main() 