// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class ScheduleGetResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The active_features property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_active_features? ActiveFeatures { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_active_features ActiveFeatures { get; set; }
#endif
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The data datetime of the last imported schedule.</summary>
        public DateTimeOffset? DataDate { get; set; }
        /// <summary>The last_calendar_view property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LastCalendarView { get; set; }
#nullable restore
#else
        public string LastCalendarView { get; set; }
#endif
        /// <summary>The Lookahead&apos;s data datetime.</summary>
        public DateTimeOffset? LookaheadDataDate { get; set; }
        /// <summary>The number of pending Requested Changes for the given user.</summary>
        public double? NumberOfPendingRequestedChanges { get; set; }
        /// <summary>Office</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_office? Office { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_office Office { get; set; }
#endif
        /// <summary>Project</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_project? Project { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_project Project { get; set; }
#endif
        /// <summary>The schedule_crud_beta_agreement property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_schedule_crud_beta_agreement? ScheduleCrudBetaAgreement { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_schedule_crud_beta_agreement ScheduleCrudBetaAgreement { get; set; }
#endif
        /// <summary>The schedule_present property</summary>
        public bool? SchedulePresent { get; set; }
        /// <summary>The schedule_processing property</summary>
        public bool? ScheduleProcessing { get; set; }
        /// <summary>The schedule_tasks_edited_manually property</summary>
        public bool? ScheduleTasksEditedManually { get; set; }
        /// <summary>Timestamp of the most recent change to any task in the Schedule.</summary>
        public DateTimeOffset? ScheduleTasksLastUpdatedAt { get; set; }
        /// <summary>The type property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_type? Type { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_type Type { get; set; }
#endif
        /// <summary>The upload datetime of the last imported schedule.</summary>
        public DateTimeOffset? UploadedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse"/> and sets the default values.
        /// </summary>
        public ScheduleGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "active_features", n => { ActiveFeatures = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_active_features>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_active_features.CreateFromDiscriminatorValue); } },
                { "data_date", n => { DataDate = n.GetDateTimeOffsetValue(); } },
                { "last_calendar_view", n => { LastCalendarView = n.GetStringValue(); } },
                { "lookahead_data_date", n => { LookaheadDataDate = n.GetDateTimeOffsetValue(); } },
                { "number_of_pending_requested_changes", n => { NumberOfPendingRequestedChanges = n.GetDoubleValue(); } },
                { "office", n => { Office = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_office>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_office.CreateFromDiscriminatorValue); } },
                { "project", n => { Project = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_project>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_project.CreateFromDiscriminatorValue); } },
                { "schedule_crud_beta_agreement", n => { ScheduleCrudBetaAgreement = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_schedule_crud_beta_agreement>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_schedule_crud_beta_agreement.CreateFromDiscriminatorValue); } },
                { "schedule_present", n => { SchedulePresent = n.GetBoolValue(); } },
                { "schedule_processing", n => { ScheduleProcessing = n.GetBoolValue(); } },
                { "schedule_tasks_edited_manually", n => { ScheduleTasksEditedManually = n.GetBoolValue(); } },
                { "schedule_tasks_last_updated_at", n => { ScheduleTasksLastUpdatedAt = n.GetDateTimeOffsetValue(); } },
                { "type", n => { Type = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_type>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_type.CreateFromDiscriminatorValue); } },
                { "uploaded_at", n => { UploadedAt = n.GetDateTimeOffsetValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_active_features>("active_features", ActiveFeatures);
            writer.WriteDateTimeOffsetValue("data_date", DataDate);
            writer.WriteStringValue("last_calendar_view", LastCalendarView);
            writer.WriteDateTimeOffsetValue("lookahead_data_date", LookaheadDataDate);
            writer.WriteDoubleValue("number_of_pending_requested_changes", NumberOfPendingRequestedChanges);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_office>("office", Office);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_project>("project", Project);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_schedule_crud_beta_agreement>("schedule_crud_beta_agreement", ScheduleCrudBetaAgreement);
            writer.WriteBoolValue("schedule_present", SchedulePresent);
            writer.WriteBoolValue("schedule_processing", ScheduleProcessing);
            writer.WriteBoolValue("schedule_tasks_edited_manually", ScheduleTasksEditedManually);
            writer.WriteDateTimeOffsetValue("schedule_tasks_last_updated_at", ScheduleTasksLastUpdatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.ScheduleGetResponse_type>("type", Type);
            writer.WriteDateTimeOffsetValue("uploaded_at", UploadedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
