// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Compliance_documentsPatchRequestBody_attributes : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Existing attachments to preserve on the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? AttachmentIds { get; set; }
#nullable restore
#else
        public List<int?> AttachmentIds { get; set; }
#endif
        /// <summary>Drawing Revisions to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? DrawingRevisionIds { get; set; }
#nullable restore
#else
        public List<int?> DrawingRevisionIds { get; set; }
#endif
        /// <summary>The effective_at property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? EffectiveAt { get; set; }
#nullable restore
#else
        public string EffectiveAt { get; set; }
#endif
        /// <summary>The expires_at property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ExpiresAt { get; set; }
#nullable restore
#else
        public string ExpiresAt { get; set; }
#endif
        /// <summary>File Versions to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? FileVersionIds { get; set; }
#nullable restore
#else
        public List<int?> FileVersionIds { get; set; }
#endif
        /// <summary>Forms to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? FormIds { get; set; }
#nullable restore
#else
        public List<int?> FormIds { get; set; }
#endif
        /// <summary>Images to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? ImageIds { get; set; }
#nullable restore
#else
        public List<int?> ImageIds { get; set; }
#endif
        /// <summary>The name property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The notes property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Notes { get; set; }
#nullable restore
#else
        public string Notes { get; set; }
#endif
        /// <summary>The send_expiration_notification property</summary>
        public bool? SendExpirationNotification { get; set; }
        /// <summary>The status property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes_status? Status { get; set; }
        /// <summary>The type property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes_type? Type { get; set; }
        /// <summary>Uploads to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? UploadIds { get; set; }
#nullable restore
#else
        public List<string> UploadIds { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes"/> and sets the default values.
        /// </summary>
        public Compliance_documentsPatchRequestBody_attributes()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "attachment_ids", n => { AttachmentIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "drawing_revision_ids", n => { DrawingRevisionIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "effective_at", n => { EffectiveAt = n.GetStringValue(); } },
                { "expires_at", n => { ExpiresAt = n.GetStringValue(); } },
                { "file_version_ids", n => { FileVersionIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "form_ids", n => { FormIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "image_ids", n => { ImageIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "notes", n => { Notes = n.GetStringValue(); } },
                { "send_expiration_notification", n => { SendExpirationNotification = n.GetBoolValue(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes_status>(); } },
                { "type", n => { Type = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes_type>(); } },
                { "upload_ids", n => { UploadIds = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfPrimitiveValues<int?>("attachment_ids", AttachmentIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("drawing_revision_ids", DrawingRevisionIds);
            writer.WriteStringValue("effective_at", EffectiveAt);
            writer.WriteStringValue("expires_at", ExpiresAt);
            writer.WriteCollectionOfPrimitiveValues<int?>("file_version_ids", FileVersionIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("form_ids", FormIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("image_ids", ImageIds);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("notes", Notes);
            writer.WriteBoolValue("send_expiration_notification", SendExpirationNotification);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes_status>("status", Status);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Purchase_order_contracts.Item.Compliance_documents.Item.Compliance_documentsPatchRequestBody_attributes_type>("type", Type);
            writer.WriteCollectionOfPrimitiveValues<string>("upload_ids", UploadIds);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
