// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_template_references.Bulk_create
{
    /// <summary>
    /// One of attachment, drawing_revision_id, file_version_id, specification_section_id, submittal_log_id, generic_tool_item_id, form_id, image_id, meeting_id, or observation_item_id is accepted depending on the type provided
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Bulk_createPostRequestBody_plan_template_references_payload : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Drawing Revision ID</summary>
        public int? DrawingRevisionId { get; set; }
        /// <summary>File Version ID</summary>
        public int? FileVersionId { get; set; }
        /// <summary>Form ID</summary>
        public int? FormId { get; set; }
        /// <summary>Generic Tool Item (Correspondence) ID</summary>
        public int? GenericToolItemId { get; set; }
        /// <summary>Image ID</summary>
        public int? ImageId { get; set; }
        /// <summary>Meeting ID</summary>
        public int? MeetingId { get; set; }
        /// <summary>Observation Item ID</summary>
        public int? ObservationItemId { get; set; }
        /// <summary>Specification Section ID</summary>
        public int? SpecificationSectionId { get; set; }
        /// <summary>Submittal Log ID</summary>
        public int? SubmittalLogId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_template_references.Bulk_create.Bulk_createPostRequestBody_plan_template_references_payload"/> and sets the default values.
        /// </summary>
        public Bulk_createPostRequestBody_plan_template_references_payload()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_template_references.Bulk_create.Bulk_createPostRequestBody_plan_template_references_payload"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_template_references.Bulk_create.Bulk_createPostRequestBody_plan_template_references_payload CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_template_references.Bulk_create.Bulk_createPostRequestBody_plan_template_references_payload();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "drawing_revision_id", n => { DrawingRevisionId = n.GetIntValue(); } },
                { "file_version_id", n => { FileVersionId = n.GetIntValue(); } },
                { "form_id", n => { FormId = n.GetIntValue(); } },
                { "generic_tool_item_id", n => { GenericToolItemId = n.GetIntValue(); } },
                { "image_id", n => { ImageId = n.GetIntValue(); } },
                { "meeting_id", n => { MeetingId = n.GetIntValue(); } },
                { "observation_item_id", n => { ObservationItemId = n.GetIntValue(); } },
                { "specification_section_id", n => { SpecificationSectionId = n.GetIntValue(); } },
                { "submittal_log_id", n => { SubmittalLogId = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("drawing_revision_id", DrawingRevisionId);
            writer.WriteIntValue("file_version_id", FileVersionId);
            writer.WriteIntValue("form_id", FormId);
            writer.WriteIntValue("generic_tool_item_id", GenericToolItemId);
            writer.WriteIntValue("image_id", ImageId);
            writer.WriteIntValue("meeting_id", MeetingId);
            writer.WriteIntValue("observation_item_id", ObservationItemId);
            writer.WriteIntValue("specification_section_id", SpecificationSectionId);
            writer.WriteIntValue("submittal_log_id", SubmittalLogId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
