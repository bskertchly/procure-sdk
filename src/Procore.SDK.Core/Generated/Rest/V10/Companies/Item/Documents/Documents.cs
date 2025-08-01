// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Documents
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Documents : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>if folder is implicitly tracked, reflects the folder that is the cause</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_children? Children { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_children Children { get; set; }
#endif
        /// <summary>Folder created at</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_created_by CreatedBy { get; set; }
#endif
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_custom_fields CustomFields { get; set; }
#endif
        /// <summary>Folder or File</summary>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_document_type? DocumentType { get; set; }
        /// <summary>will be filled if document_type is a file</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_file? File { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_file File { get; set; }
#endif
        /// <summary>Folder id</summary>
        public int? Id { get; set; }
        /// <summary>Folder is in the recycle bin status</summary>
        public bool? IsDeleted { get; set; }
        /// <summary>Folder is recycle bin status</summary>
        public bool? IsRecycleBin { get; set; }
        /// <summary>Status whether Folder is explicitly tracked</summary>
        public bool? IsTracked { get; set; }
        /// <summary>Folder name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Full file path with folder name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? NameWithPath { get; set; }
#nullable restore
#else
        public string NameWithPath { get; set; }
#endif
        /// <summary>Folder parent id</summary>
        public int? ParentId { get; set; }
        /// <summary>Status whether Folder is explicitly private</summary>
        public bool? Private { get; set; }
        /// <summary>The private_parent property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_private_parent? PrivateParent { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_private_parent PrivateParent { get; set; }
#endif
        /// <summary>File is read_only (only updatable via Schedule)</summary>
        public bool? ReadOnly { get; set; }
        /// <summary>The tracked_folder property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_tracked_folder? TrackedFolder { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_tracked_folder TrackedFolder { get; set; }
#endif
        /// <summary>Folder updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents"/> and sets the default values.
        /// </summary>
        public Documents()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "children", n => { Children = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_children>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_children.CreateFromDiscriminatorValue); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_created_by>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_created_by.CreateFromDiscriminatorValue); } },
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_custom_fields>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_custom_fields.CreateFromDiscriminatorValue); } },
                { "document_type", n => { DocumentType = n.GetEnumValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_document_type>(); } },
                { "file", n => { File = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_file>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_file.CreateFromDiscriminatorValue); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "is_deleted", n => { IsDeleted = n.GetBoolValue(); } },
                { "is_recycle_bin", n => { IsRecycleBin = n.GetBoolValue(); } },
                { "is_tracked", n => { IsTracked = n.GetBoolValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "name_with_path", n => { NameWithPath = n.GetStringValue(); } },
                { "parent_id", n => { ParentId = n.GetIntValue(); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "private_parent", n => { PrivateParent = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_private_parent>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_private_parent.CreateFromDiscriminatorValue); } },
                { "read_only", n => { ReadOnly = n.GetBoolValue(); } },
                { "tracked_folder", n => { TrackedFolder = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_tracked_folder>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_tracked_folder.CreateFromDiscriminatorValue); } },
                { "updated_at", n => { UpdatedAt = n.GetDateTimeOffsetValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_children>("children", Children);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_created_by>("created_by", CreatedBy);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_custom_fields>("custom_fields", CustomFields);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_document_type>("document_type", DocumentType);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_file>("file", File);
            writer.WriteIntValue("id", Id);
            writer.WriteBoolValue("is_deleted", IsDeleted);
            writer.WriteBoolValue("is_recycle_bin", IsRecycleBin);
            writer.WriteBoolValue("is_tracked", IsTracked);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("name_with_path", NameWithPath);
            writer.WriteIntValue("parent_id", ParentId);
            writer.WriteBoolValue("private", Private);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_private_parent>("private_parent", PrivateParent);
            writer.WriteBoolValue("read_only", ReadOnly);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Documents.Documents_tracked_folder>("tracked_folder", TrackedFolder);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
