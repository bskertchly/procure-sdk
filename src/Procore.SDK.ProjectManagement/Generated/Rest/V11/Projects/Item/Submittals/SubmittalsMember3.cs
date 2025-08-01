// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals
{
    /// <summary>
    /// Submittal response when using serializer_view=minimal_list (Dynamic Submittal Plan (DSP) disabled).  Includes format optimized for list displays with essential fields only.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SubmittalsMember3 : IAdditionalDataHolder, IParsable
    {
        /// <summary>Actual delivery date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? ActualDeliveryDate { get; set; }
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Anticipated delivery date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? AnticipatedDeliveryDate { get; set; }
        /// <summary>The approvers property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_approvers>? Approvers { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_approvers> Approvers { get; set; }
#endif
        /// <summary>The attachments_count property</summary>
        public int? AttachmentsCount { get; set; }
        /// <summary>The ball_in_court property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_ball_in_court>? BallInCourt { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_ball_in_court> BallInCourt { get; set; }
#endif
        /// <summary>Buffer time in days (only visible if user has permission)</summary>
        public int? BufferTime { get; set; }
        /// <summary>Confirmed delivery date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? ConfirmedDeliveryDate { get; set; }
        /// <summary>The created_at property</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_created_by CreatedBy { get; set; }
#endif
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_custom_fields CustomFields { get; set; }
#endif
        /// <summary>The distributed_at property</summary>
        public DateTimeOffset? DistributedAt { get; set; }
        /// <summary>Due date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>The formatted_number property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FormattedNumber { get; set; }
#nullable restore
#else
        public string FormattedNumber { get; set; }
#endif
        /// <summary>The for_record_only property</summary>
        public bool? ForRecordOnly { get; set; }
        /// <summary>The id property</summary>
        public int? Id { get; set; }
        /// <summary>Issue date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? IssueDate { get; set; }
        /// <summary>The location property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_location? Location { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_location Location { get; set; }
#endif
        /// <summary>The number property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Number { get; set; }
#nullable restore
#else
        public string Number { get; set; }
#endif
        /// <summary>The private property</summary>
        public bool? Private { get; set; }
        /// <summary>Received date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? ReceivedDate { get; set; }
        /// <summary>The received_from property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_received_from? ReceivedFrom { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_received_from ReceivedFrom { get; set; }
#endif
        /// <summary>Required on site date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? RequiredOnSiteDate { get; set; }
        /// <summary>The responsible_contractor property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_responsible_contractor? ResponsibleContractor { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_responsible_contractor ResponsibleContractor { get; set; }
#endif
        /// <summary>The revision property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Revision { get; set; }
#nullable restore
#else
        public string Revision { get; set; }
#endif
        /// <summary>The specification_section property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_specification_section? SpecificationSection { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_specification_section SpecificationSection { get; set; }
#endif
        /// <summary>The status property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_status? Status { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_status Status { get; set; }
#endif
        /// <summary>The sub_job property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_sub_job? SubJob { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_sub_job SubJob { get; set; }
#endif
        /// <summary>Submit by date (datetime at noon in project timezone)</summary>
        public DateTimeOffset? SubmitBy { get; set; }
        /// <summary>The submittal_manager property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_manager? SubmittalManager { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_manager SubmittalManager { get; set; }
#endif
        /// <summary>The submittal_workflow_template property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_workflow_template? SubmittalWorkflowTemplate { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_workflow_template SubmittalWorkflowTemplate { get; set; }
#endif
        /// <summary>The submittal_workflow_template_applied_at property</summary>
        public DateTimeOffset? SubmittalWorkflowTemplateAppliedAt { get; set; }
        /// <summary>The title property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>The type property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_type? Type { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_type Type { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3"/> and sets the default values.
        /// </summary>
        public SubmittalsMember3()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3 CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "actual_delivery_date", n => { ActualDeliveryDate = n.GetDateTimeOffsetValue(); } },
                { "anticipated_delivery_date", n => { AnticipatedDeliveryDate = n.GetDateTimeOffsetValue(); } },
                { "approvers", n => { Approvers = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_approvers>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_approvers.CreateFromDiscriminatorValue)?.AsList(); } },
                { "attachments_count", n => { AttachmentsCount = n.GetIntValue(); } },
                { "ball_in_court", n => { BallInCourt = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_ball_in_court>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_ball_in_court.CreateFromDiscriminatorValue)?.AsList(); } },
                { "buffer_time", n => { BufferTime = n.GetIntValue(); } },
                { "confirmed_delivery_date", n => { ConfirmedDeliveryDate = n.GetDateTimeOffsetValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_created_by>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_created_by.CreateFromDiscriminatorValue); } },
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_custom_fields>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_custom_fields.CreateFromDiscriminatorValue); } },
                { "distributed_at", n => { DistributedAt = n.GetDateTimeOffsetValue(); } },
                { "due_date", n => { DueDate = n.GetDateTimeOffsetValue(); } },
                { "for_record_only", n => { ForRecordOnly = n.GetBoolValue(); } },
                { "formatted_number", n => { FormattedNumber = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "issue_date", n => { IssueDate = n.GetDateTimeOffsetValue(); } },
                { "location", n => { Location = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_location>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_location.CreateFromDiscriminatorValue); } },
                { "number", n => { Number = n.GetStringValue(); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "received_date", n => { ReceivedDate = n.GetDateTimeOffsetValue(); } },
                { "received_from", n => { ReceivedFrom = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_received_from>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_received_from.CreateFromDiscriminatorValue); } },
                { "required_on_site_date", n => { RequiredOnSiteDate = n.GetDateTimeOffsetValue(); } },
                { "responsible_contractor", n => { ResponsibleContractor = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_responsible_contractor>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_responsible_contractor.CreateFromDiscriminatorValue); } },
                { "revision", n => { Revision = n.GetStringValue(); } },
                { "specification_section", n => { SpecificationSection = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_specification_section>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_specification_section.CreateFromDiscriminatorValue); } },
                { "status", n => { Status = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_status>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_status.CreateFromDiscriminatorValue); } },
                { "sub_job", n => { SubJob = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_sub_job>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_sub_job.CreateFromDiscriminatorValue); } },
                { "submit_by", n => { SubmitBy = n.GetDateTimeOffsetValue(); } },
                { "submittal_manager", n => { SubmittalManager = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_manager>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_manager.CreateFromDiscriminatorValue); } },
                { "submittal_workflow_template", n => { SubmittalWorkflowTemplate = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_workflow_template>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_workflow_template.CreateFromDiscriminatorValue); } },
                { "submittal_workflow_template_applied_at", n => { SubmittalWorkflowTemplateAppliedAt = n.GetDateTimeOffsetValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
                { "type", n => { Type = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_type>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_type.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteDateTimeOffsetValue("actual_delivery_date", ActualDeliveryDate);
            writer.WriteDateTimeOffsetValue("anticipated_delivery_date", AnticipatedDeliveryDate);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_approvers>("approvers", Approvers);
            writer.WriteIntValue("attachments_count", AttachmentsCount);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_ball_in_court>("ball_in_court", BallInCourt);
            writer.WriteIntValue("buffer_time", BufferTime);
            writer.WriteDateTimeOffsetValue("confirmed_delivery_date", ConfirmedDeliveryDate);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_created_by>("created_by", CreatedBy);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_custom_fields>("custom_fields", CustomFields);
            writer.WriteDateTimeOffsetValue("distributed_at", DistributedAt);
            writer.WriteDateTimeOffsetValue("due_date", DueDate);
            writer.WriteStringValue("formatted_number", FormattedNumber);
            writer.WriteBoolValue("for_record_only", ForRecordOnly);
            writer.WriteIntValue("id", Id);
            writer.WriteDateTimeOffsetValue("issue_date", IssueDate);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_location>("location", Location);
            writer.WriteStringValue("number", Number);
            writer.WriteBoolValue("private", Private);
            writer.WriteDateTimeOffsetValue("received_date", ReceivedDate);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_received_from>("received_from", ReceivedFrom);
            writer.WriteDateTimeOffsetValue("required_on_site_date", RequiredOnSiteDate);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_responsible_contractor>("responsible_contractor", ResponsibleContractor);
            writer.WriteStringValue("revision", Revision);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_specification_section>("specification_section", SpecificationSection);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_status>("status", Status);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_sub_job>("sub_job", SubJob);
            writer.WriteDateTimeOffsetValue("submit_by", SubmitBy);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_manager>("submittal_manager", SubmittalManager);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_submittal_workflow_template>("submittal_workflow_template", SubmittalWorkflowTemplate);
            writer.WriteDateTimeOffsetValue("submittal_workflow_template_applied_at", SubmittalWorkflowTemplateAppliedAt);
            writer.WriteStringValue("title", Title);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Submittals.SubmittalsMember3_type>("type", Type);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
