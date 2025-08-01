// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Rfis : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The assignee property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignee? Assignee { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignee Assignee { get; set; }
#endif
        /// <summary>RFI Assignees</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignees>? Assignees { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignees> Assignees { get; set; }
#endif
        /// <summary>The ball_in_court property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_court? BallInCourt { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_court BallInCourt { get; set; }
#endif
        /// <summary>Ball In Courts</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_courts>? BallInCourts { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_courts> BallInCourts { get; set; }
#endif
        /// <summary>The cost_code property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_code? CostCode { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_code CostCode { get; set; }
#endif
        /// <summary>The cost_impact property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_impact? CostImpact { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_impact CostImpact { get; set; }
#endif
        /// <summary>Date created</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_created_by CreatedBy { get; set; }
#endif
        /// <summary>Designate whether or not this RFI is the latest revision [This is a Beta feature]</summary>
        public bool? CurrentRevision { get; set; }
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_custom_fields CustomFields { get; set; }
#endif
        /// <summary>Deleted status (this is only shown on deleted records)</summary>
        public bool? Deleted { get; set; }
        /// <summary>Time deleted (this is only shown on deleted records)</summary>
        public DateTimeOffset? DeletedAt { get; set; }
        /// <summary>Due Date</summary>
        public Date? DueDate { get; set; }
        /// <summary>Full Number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FullNumber { get; set; }
#nullable restore
#else
        public string FullNumber { get; set; }
#endif
        /// <summary>Designate whether or not this RFI has other revisions [This is a Beta feature]</summary>
        public bool? HasRevisions { get; set; }
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Date initiated</summary>
        public DateTimeOffset? InitiatedAt { get; set; }
        /// <summary>Web link to resource</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Link { get; set; }
#nullable restore
#else
        public string Link { get; set; }
#endif
        /// <summary>The location property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_location? Location { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_location Location { get; set; }
#endif
        /// <summary>ID of the associated Location</summary>
        public int? LocationId { get; set; }
        /// <summary>Number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Number { get; set; }
#nullable restore
#else
        public string Number { get; set; }
#endif
        /// <summary>Prefix</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Prefix { get; set; }
#nullable restore
#else
        public string Prefix { get; set; }
#endif
        /// <summary>Private Status</summary>
        public bool? Private { get; set; }
        /// <summary>The project_stage property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_project_stage? ProjectStage { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_project_stage ProjectStage { get; set; }
#endif
        /// <summary>RFI Questions</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_questions>? Questions { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_questions> Questions { get; set; }
#endif
        /// <summary>The received_from property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_received_from? ReceivedFrom { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_received_from ReceivedFrom { get; set; }
#endif
        /// <summary>Reference</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Reference { get; set; }
#nullable restore
#else
        public string Reference { get; set; }
#endif
        /// <summary>The responsible_contractor property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_responsible_contractor? ResponsibleContractor { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_responsible_contractor ResponsibleContractor { get; set; }
#endif
        /// <summary>Revision Number [This is a Beta feature]</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Revision { get; set; }
#nullable restore
#else
        public string Revision { get; set; }
#endif
        /// <summary>The rfi_manager property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_rfi_manager? RfiManager { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_rfi_manager RfiManager { get; set; }
#endif
        /// <summary>The schedule_impact property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_schedule_impact? ScheduleImpact { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_schedule_impact ScheduleImpact { get; set; }
#endif
        /// <summary>The ID of The Root RFI Revision [This is a Beta feature]</summary>
        public int? SourceRfiHeaderId { get; set; }
        /// <summary>ID of the associated Specification Section</summary>
        public int? SpecificationSectionId { get; set; }
        /// <summary>Status</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_status? Status { get; set; }
        /// <summary>Subject</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Subject { get; set; }
#nullable restore
#else
        public string Subject { get; set; }
#endif
        /// <summary>The sub_job property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_sub_job? SubJob { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_sub_job SubJob { get; set; }
#endif
        /// <summary>Time RFI was closed</summary>
        public DateTimeOffset? TimeResolved { get; set; }
        /// <summary>Translated RFI status</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? TranslatedStatus { get; set; }
#nullable restore
#else
        public string TranslatedStatus { get; set; }
#endif
        /// <summary>Updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis"/> and sets the default values.
        /// </summary>
        public Rfis()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "assignee", n => { Assignee = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignee>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignee.CreateFromDiscriminatorValue); } },
                { "assignees", n => { Assignees = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignees>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignees.CreateFromDiscriminatorValue)?.AsList(); } },
                { "ball_in_court", n => { BallInCourt = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_court>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_court.CreateFromDiscriminatorValue); } },
                { "ball_in_courts", n => { BallInCourts = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_courts>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_courts.CreateFromDiscriminatorValue)?.AsList(); } },
                { "cost_code", n => { CostCode = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_code>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_code.CreateFromDiscriminatorValue); } },
                { "cost_impact", n => { CostImpact = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_impact>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_impact.CreateFromDiscriminatorValue); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_created_by>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_created_by.CreateFromDiscriminatorValue); } },
                { "current_revision", n => { CurrentRevision = n.GetBoolValue(); } },
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_custom_fields>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_custom_fields.CreateFromDiscriminatorValue); } },
                { "deleted", n => { Deleted = n.GetBoolValue(); } },
                { "deleted_at", n => { DeletedAt = n.GetDateTimeOffsetValue(); } },
                { "due_date", n => { DueDate = n.GetDateValue(); } },
                { "full_number", n => { FullNumber = n.GetStringValue(); } },
                { "has_revisions", n => { HasRevisions = n.GetBoolValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "initiated_at", n => { InitiatedAt = n.GetDateTimeOffsetValue(); } },
                { "link", n => { Link = n.GetStringValue(); } },
                { "location", n => { Location = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_location>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_location.CreateFromDiscriminatorValue); } },
                { "location_id", n => { LocationId = n.GetIntValue(); } },
                { "number", n => { Number = n.GetStringValue(); } },
                { "prefix", n => { Prefix = n.GetStringValue(); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "project_stage", n => { ProjectStage = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_project_stage>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_project_stage.CreateFromDiscriminatorValue); } },
                { "questions", n => { Questions = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_questions>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_questions.CreateFromDiscriminatorValue)?.AsList(); } },
                { "received_from", n => { ReceivedFrom = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_received_from>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_received_from.CreateFromDiscriminatorValue); } },
                { "reference", n => { Reference = n.GetStringValue(); } },
                { "responsible_contractor", n => { ResponsibleContractor = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_responsible_contractor>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_responsible_contractor.CreateFromDiscriminatorValue); } },
                { "revision", n => { Revision = n.GetStringValue(); } },
                { "rfi_manager", n => { RfiManager = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_rfi_manager>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_rfi_manager.CreateFromDiscriminatorValue); } },
                { "schedule_impact", n => { ScheduleImpact = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_schedule_impact>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_schedule_impact.CreateFromDiscriminatorValue); } },
                { "source_rfi_header_id", n => { SourceRfiHeaderId = n.GetIntValue(); } },
                { "specification_section_id", n => { SpecificationSectionId = n.GetIntValue(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_status>(); } },
                { "sub_job", n => { SubJob = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_sub_job>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_sub_job.CreateFromDiscriminatorValue); } },
                { "subject", n => { Subject = n.GetStringValue(); } },
                { "time_resolved", n => { TimeResolved = n.GetDateTimeOffsetValue(); } },
                { "translated_status", n => { TranslatedStatus = n.GetStringValue(); } },
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
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignee>("assignee", Assignee);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_assignees>("assignees", Assignees);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_court>("ball_in_court", BallInCourt);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_ball_in_courts>("ball_in_courts", BallInCourts);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_code>("cost_code", CostCode);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_cost_impact>("cost_impact", CostImpact);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_created_by>("created_by", CreatedBy);
            writer.WriteBoolValue("current_revision", CurrentRevision);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_custom_fields>("custom_fields", CustomFields);
            writer.WriteBoolValue("deleted", Deleted);
            writer.WriteDateTimeOffsetValue("deleted_at", DeletedAt);
            writer.WriteDateValue("due_date", DueDate);
            writer.WriteStringValue("full_number", FullNumber);
            writer.WriteBoolValue("has_revisions", HasRevisions);
            writer.WriteIntValue("id", Id);
            writer.WriteDateTimeOffsetValue("initiated_at", InitiatedAt);
            writer.WriteStringValue("link", Link);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_location>("location", Location);
            writer.WriteIntValue("location_id", LocationId);
            writer.WriteStringValue("number", Number);
            writer.WriteStringValue("prefix", Prefix);
            writer.WriteBoolValue("private", Private);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_project_stage>("project_stage", ProjectStage);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_questions>("questions", Questions);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_received_from>("received_from", ReceivedFrom);
            writer.WriteStringValue("reference", Reference);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_responsible_contractor>("responsible_contractor", ResponsibleContractor);
            writer.WriteStringValue("revision", Revision);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_rfi_manager>("rfi_manager", RfiManager);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_schedule_impact>("schedule_impact", ScheduleImpact);
            writer.WriteIntValue("source_rfi_header_id", SourceRfiHeaderId);
            writer.WriteIntValue("specification_section_id", SpecificationSectionId);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_status>("status", Status);
            writer.WriteStringValue("subject", Subject);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Rfis.Rfis_sub_job>("sub_job", SubJob);
            writer.WriteDateTimeOffsetValue("time_resolved", TimeResolved);
            writer.WriteStringValue("translated_status", TranslatedStatus);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
