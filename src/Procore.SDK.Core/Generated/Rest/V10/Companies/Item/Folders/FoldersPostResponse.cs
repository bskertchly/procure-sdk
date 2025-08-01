// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Folders
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class FoldersPostResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_custom_fields CustomFields { get; set; }
#endif
        /// <summary>The child Files of the Folder</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_files>? Files { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_files> Files { get; set; }
#endif
        /// <summary>The child Folders of the Folder</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_folders>? Folders { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_folders> Folders { get; set; }
#endif
        /// <summary>Folder has children status</summary>
        public bool? HasChildren { get; set; }
        /// <summary>Folder has at least one child that is a file status</summary>
        public bool? HasChildrenFiles { get; set; }
        /// <summary>Folder has at least one child that is a folder status</summary>
        public bool? HasChildrenFolders { get; set; }
        /// <summary>Folder id</summary>
        public int? Id { get; set; }
        /// <summary>File is in the recycle bin status</summary>
        public bool? IsDeleted { get; set; }
        /// <summary>Folder is recycle bin status</summary>
        public bool? IsRecycleBin { get; set; }
        /// <summary>Folder is tracked status</summary>
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
        /// <summary>Folder private status</summary>
        public bool? Private { get; set; }
        /// <summary>Folder read only status</summary>
        public bool? ReadOnly { get; set; }
        /// <summary>Folder watchers</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_tracked_folder? TrackedFolder { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_tracked_folder TrackedFolder { get; set; }
#endif
        /// <summary>Folder updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse"/> and sets the default values.
        /// </summary>
        public FoldersPostResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_custom_fields>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_custom_fields.CreateFromDiscriminatorValue); } },
                { "files", n => { Files = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_files>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_files.CreateFromDiscriminatorValue)?.AsList(); } },
                { "folders", n => { Folders = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_folders>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_folders.CreateFromDiscriminatorValue)?.AsList(); } },
                { "has_children", n => { HasChildren = n.GetBoolValue(); } },
                { "has_children_files", n => { HasChildrenFiles = n.GetBoolValue(); } },
                { "has_children_folders", n => { HasChildrenFolders = n.GetBoolValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "is_deleted", n => { IsDeleted = n.GetBoolValue(); } },
                { "is_recycle_bin", n => { IsRecycleBin = n.GetBoolValue(); } },
                { "is_tracked", n => { IsTracked = n.GetBoolValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "name_with_path", n => { NameWithPath = n.GetStringValue(); } },
                { "parent_id", n => { ParentId = n.GetIntValue(); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "read_only", n => { ReadOnly = n.GetBoolValue(); } },
                { "tracked_folder", n => { TrackedFolder = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_tracked_folder>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_tracked_folder.CreateFromDiscriminatorValue); } },
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
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_custom_fields>("custom_fields", CustomFields);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_files>("files", Files);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_folders>("folders", Folders);
            writer.WriteBoolValue("has_children", HasChildren);
            writer.WriteBoolValue("has_children_files", HasChildrenFiles);
            writer.WriteBoolValue("has_children_folders", HasChildrenFolders);
            writer.WriteIntValue("id", Id);
            writer.WriteBoolValue("is_deleted", IsDeleted);
            writer.WriteBoolValue("is_recycle_bin", IsRecycleBin);
            writer.WriteBoolValue("is_tracked", IsTracked);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("name_with_path", NameWithPath);
            writer.WriteIntValue("parent_id", ParentId);
            writer.WriteBoolValue("private", Private);
            writer.WriteBoolValue("read_only", ReadOnly);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Folders.FoldersPostResponse_tracked_folder>("tracked_folder", TrackedFolder);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
