// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Timecard_entriesPatchResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Supervisor approval status</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ApprovalStatus { get; set; }
#nullable restore
#else
        public string ApprovalStatus { get; set; }
#endif
        /// <summary>Timecard entries returned with associated object as part of overtime_management</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_automatically_split_timecard_entries>? AutomaticallySplitTimecardEntries { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_automatically_split_timecard_entries> AutomaticallySplitTimecardEntries { get; set; }
#endif
        /// <summary>The billable status of the timecard entry. Must be either true or false.</summary>
        public bool? Billable { get; set; }
        /// <summary>The cost_code property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_cost_code? CostCode { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_cost_code CostCode { get; set; }
#endif
        /// <summary>The date and time when the timecard entry was created.</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_created_by CreatedBy { get; set; }
#endif
        /// <summary>The crew property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_crew? Crew { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_crew Crew { get; set; }
#endif
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_custom_fields CustomFields { get; set; }
#endif
        /// <summary>The date when the timecard was created.</summary>
        public Date? Date { get; set; }
        /// <summary>The estimated UTC date time of record.</summary>
        public DateTimeOffset? Datetime { get; set; }
        /// <summary>The date and time when the timecard entry was deleted.</summary>
        public DateTimeOffset? DeletedAt { get; set; }
        /// <summary>The description for the timecard entry.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Total number of hours the resource was on sight.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Hours { get; set; }
#nullable restore
#else
        public string Hours { get; set; }
#endif
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Whether or not an injury occured during work hours. Must be either true or false.</summary>
        public bool? Injured { get; set; }
        /// <summary>The ID of the line item type of the timecard entry</summary>
        public int? LineItemTypeId { get; set; }
        /// <summary>The location property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_location? Location { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_location Location { get; set; }
#endif
        /// <summary>The login_information property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_login_information? LoginInformation { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_login_information LoginInformation { get; set; }
#endif
        /// <summary>Number of hours taken for lunch</summary>
        public int? LunchTime { get; set; }
        /// <summary>The value of related external data</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? OriginData { get; set; }
#nullable restore
#else
        public string OriginData { get; set; }
#endif
        /// <summary>The ID of related external data</summary>
        public int? OriginId { get; set; }
        /// <summary>The party property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_party? Party { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_party Party { get; set; }
#endif
        /// <summary>The procore_signature property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_procore_signature? ProcoreSignature { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_procore_signature ProcoreSignature { get; set; }
#endif
        /// <summary>Whether or not the timecard has been signed. Must be either true or false.</summary>
        public bool? Signed { get; set; }
        /// <summary>The sub_job property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_sub_job? SubJob { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_sub_job SubJob { get; set; }
#endif
        /// <summary>The timecard_time_type property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timecard_time_type? TimecardTimeType { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timecard_time_type TimecardTimeType { get; set; }
#endif
        /// <summary>The date and time the timecard was last updated</summary>
        public Date? TimeIn { get; set; }
        /// <summary>The date and time the timecard was last updated</summary>
        public Date? TimeOut { get; set; }
        /// <summary>The timesheet property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timesheet? Timesheet { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timesheet Timesheet { get; set; }
#endif
        /// <summary>Deprecated. Reference status property.</summary>
        [Obsolete("")]
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? TimesheetStatus { get; set; }
#nullable restore
#else
        public string TimesheetStatus { get; set; }
#endif
        /// <summary>The date and time when the timesheet was updated.</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse"/> and sets the default values.
        /// </summary>
        public Timecard_entriesPatchResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "approval_status", n => { ApprovalStatus = n.GetStringValue(); } },
                { "automatically_split_timecard_entries", n => { AutomaticallySplitTimecardEntries = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_automatically_split_timecard_entries>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_automatically_split_timecard_entries.CreateFromDiscriminatorValue)?.AsList(); } },
                { "billable", n => { Billable = n.GetBoolValue(); } },
                { "cost_code", n => { CostCode = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_cost_code>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_cost_code.CreateFromDiscriminatorValue); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_created_by>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_created_by.CreateFromDiscriminatorValue); } },
                { "crew", n => { Crew = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_crew>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_crew.CreateFromDiscriminatorValue); } },
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_custom_fields>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_custom_fields.CreateFromDiscriminatorValue); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "datetime", n => { Datetime = n.GetDateTimeOffsetValue(); } },
                { "deleted_at", n => { DeletedAt = n.GetDateTimeOffsetValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "hours", n => { Hours = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "injured", n => { Injured = n.GetBoolValue(); } },
                { "line_item_type_id", n => { LineItemTypeId = n.GetIntValue(); } },
                { "location", n => { Location = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_location>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_location.CreateFromDiscriminatorValue); } },
                { "login_information", n => { LoginInformation = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_login_information>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_login_information.CreateFromDiscriminatorValue); } },
                { "lunch_time", n => { LunchTime = n.GetIntValue(); } },
                { "origin_data", n => { OriginData = n.GetStringValue(); } },
                { "origin_id", n => { OriginId = n.GetIntValue(); } },
                { "party", n => { Party = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_party>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_party.CreateFromDiscriminatorValue); } },
                { "procore_signature", n => { ProcoreSignature = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_procore_signature>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_procore_signature.CreateFromDiscriminatorValue); } },
                { "signed", n => { Signed = n.GetBoolValue(); } },
                { "sub_job", n => { SubJob = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_sub_job>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_sub_job.CreateFromDiscriminatorValue); } },
                { "time_in", n => { TimeIn = n.GetDateValue(); } },
                { "time_out", n => { TimeOut = n.GetDateValue(); } },
                { "timecard_time_type", n => { TimecardTimeType = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timecard_time_type>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timecard_time_type.CreateFromDiscriminatorValue); } },
                { "timesheet", n => { Timesheet = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timesheet>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timesheet.CreateFromDiscriminatorValue); } },
                { "timesheet_status", n => { TimesheetStatus = n.GetStringValue(); } },
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
            writer.WriteStringValue("approval_status", ApprovalStatus);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_automatically_split_timecard_entries>("automatically_split_timecard_entries", AutomaticallySplitTimecardEntries);
            writer.WriteBoolValue("billable", Billable);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_cost_code>("cost_code", CostCode);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_created_by>("created_by", CreatedBy);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_crew>("crew", Crew);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_custom_fields>("custom_fields", CustomFields);
            writer.WriteDateValue("date", Date);
            writer.WriteDateTimeOffsetValue("datetime", Datetime);
            writer.WriteDateTimeOffsetValue("deleted_at", DeletedAt);
            writer.WriteStringValue("description", Description);
            writer.WriteStringValue("hours", Hours);
            writer.WriteIntValue("id", Id);
            writer.WriteBoolValue("injured", Injured);
            writer.WriteIntValue("line_item_type_id", LineItemTypeId);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_location>("location", Location);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_login_information>("login_information", LoginInformation);
            writer.WriteIntValue("lunch_time", LunchTime);
            writer.WriteStringValue("origin_data", OriginData);
            writer.WriteIntValue("origin_id", OriginId);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_party>("party", Party);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_procore_signature>("procore_signature", ProcoreSignature);
            writer.WriteBoolValue("signed", Signed);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_sub_job>("sub_job", SubJob);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timecard_time_type>("timecard_time_type", TimecardTimeType);
            writer.WriteDateValue("time_in", TimeIn);
            writer.WriteDateValue("time_out", TimeOut);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timecard_entries.Item.Timecard_entriesPatchResponse_timesheet>("timesheet", Timesheet);
            writer.WriteStringValue("timesheet_status", TimesheetStatus);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
