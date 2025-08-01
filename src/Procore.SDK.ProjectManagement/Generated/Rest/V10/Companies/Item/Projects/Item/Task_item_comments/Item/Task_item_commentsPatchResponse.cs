// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item
{
    /// <summary>
    /// Normal view of task item comment
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Task_item_commentsPatchResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The attachments property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_attachments? Attachments { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_attachments Attachments { get; set; }
#endif
        /// <summary>The actual message of the comment</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Comment { get; set; }
#nullable restore
#else
        public string Comment { get; set; }
#endif
        /// <summary>The UTC datetime for the creation of the resource in ISO 8601 format.</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_created_by CreatedBy { get; set; }
#endif
        /// <summary>Unique ID of the comment</summary>
        public int? Id { get; set; }
        /// <summary>The status of the task item at the time this comment was created</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_status? Status { get; set; }
        /// <summary>The task item id associated with the comment</summary>
        public int? TaskItemId { get; set; }
        /// <summary>The UTC datetime for the last update of the resource in ISO 8601 format.</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse"/> and sets the default values.
        /// </summary>
        public Task_item_commentsPatchResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "attachments", n => { Attachments = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_attachments>(global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_attachments.CreateFromDiscriminatorValue); } },
                { "comment", n => { Comment = n.GetStringValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_created_by>(global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_created_by.CreateFromDiscriminatorValue); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_status>(); } },
                { "task_item_id", n => { TaskItemId = n.GetIntValue(); } },
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
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_attachments>("attachments", Attachments);
            writer.WriteStringValue("comment", Comment);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_created_by>("created_by", CreatedBy);
            writer.WriteIntValue("id", Id);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Companies.Item.Projects.Item.Task_item_comments.Item.Task_item_commentsPatchResponse_status>("status", Status);
            writer.WriteIntValue("task_item_id", TaskItemId);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
