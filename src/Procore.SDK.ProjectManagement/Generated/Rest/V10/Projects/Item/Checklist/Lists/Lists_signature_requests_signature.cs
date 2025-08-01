// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Lists_signature_requests_signature : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The attachment property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_attachment? Attachment { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_attachment Attachment { get; set; }
#endif
        /// <summary>Timestamp of creation</summary>
        public DateTimeOffset? CapturedAt { get; set; }
        /// <summary>The captured_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_captured_by? CapturedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_captured_by CapturedBy { get; set; }
#endif
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature"/> and sets the default values.
        /// </summary>
        public Lists_signature_requests_signature()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "attachment", n => { Attachment = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_attachment>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_attachment.CreateFromDiscriminatorValue); } },
                { "captured_at", n => { CapturedAt = n.GetDateTimeOffsetValue(); } },
                { "captured_by", n => { CapturedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_captured_by>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_captured_by.CreateFromDiscriminatorValue); } },
                { "id", n => { Id = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_attachment>("attachment", Attachment);
            writer.WriteDateTimeOffsetValue("captured_at", CapturedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.Lists.Lists_signature_requests_signature_captured_by>("captured_by", CapturedBy);
            writer.WriteIntValue("id", Id);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
