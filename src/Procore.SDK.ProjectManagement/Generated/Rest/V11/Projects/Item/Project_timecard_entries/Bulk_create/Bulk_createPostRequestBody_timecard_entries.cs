// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Project_timecard_entries.Bulk_create
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Bulk_createPostRequestBody_timecard_entries : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The approval status for this Timecard Entry.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ApprovalStatus { get; set; }
#nullable restore
#else
        public string ApprovalStatus { get; set; }
#endif
        /// <summary>The Billable status of the Timecard Entry</summary>
        public bool? Billable { get; set; }
        /// <summary>If so configured, the ID of the clock in GPS record</summary>
        public int? ClockInId { get; set; }
        /// <summary>If so configured, the datetime a timecard clock in was punched</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ClockInTime { get; set; }
#nullable restore
#else
        public string ClockInTime { get; set; }
#endif
        /// <summary>If so configured, the ID of the clock out GPS record</summary>
        public int? ClockOutId { get; set; }
        /// <summary>If so configured, the datetime a timecard clock out was punched</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ClockOutTime { get; set; }
#nullable restore
#else
        public string ClockOutTime { get; set; }
#endif
        /// <summary>The ID of the Cost Code of the Timecard Entry. DO NOT provide if providing a wbs_code_id.</summary>
        public int? CostCodeId { get; set; }
        /// <summary>The Date of the Timecard Entry</summary>
        public Date? Date { get; set; }
        /// <summary>The Description of the Timecard Entry</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>If so configured, the hours of the Timecard Entry</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Hours { get; set; }
#nullable restore
#else
        public string Hours { get; set; }
#endif
        /// <summary>The ID of the Line Item Type of the Timecard Entry. DO NOT provide if providing a wbs_code_id.</summary>
        public int? LineItemTypeId { get; set; }
        /// <summary>If so configured, the ID of the lunch clock in GPS record</summary>
        public int? LunchClockInId { get; set; }
        /// <summary>If so configured, the ID of the lunch clock out GPS record</summary>
        public int? LunchClockOutId { get; set; }
        /// <summary>If so configured, the datetime when lunch started</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LunchStartTime { get; set; }
#nullable restore
#else
        public string LunchStartTime { get; set; }
#endif
        /// <summary>If so configured, the datetime when lunch stopped</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LunchStopTime { get; set; }
#nullable restore
#else
        public string LunchStopTime { get; set; }
#endif
        /// <summary>Value of related external data</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? OriginData { get; set; }
#nullable restore
#else
        public string OriginData { get; set; }
#endif
        /// <summary>ID of related external data</summary>
        public int? OriginId { get; set; }
        /// <summary>The ID of the Party for the Timecard Entry</summary>
        public int? PartyId { get; set; }
        /// <summary>The ID of the project to which this Timecard Entry belongs.</summary>
        public int? ProjectId { get; set; }
        /// <summary>If so configured and a truthy value supplied, the timecard will have its timecard_time_type_id set automatically and may be split.</summary>
        public bool? SetTimecardTimeTypeAutomatically { get; set; }
        /// <summary>The ID of the Timecard Time Type of the Timecard Entry.</summary>
        public int? TimecardTimeTypeId { get; set; }
        /// <summary>The ID of the timesheet to associate this record with.</summary>
        public int? TimesheetId { get; set; }
        /// <summary>The ID of the Task Code for the Timecard Entry.</summary>
        public int? WbsCodeId { get; set; }
        /// <summary>The ID of the work classification for this Timecard Entry.</summary>
        public int? WorkClassificationId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Project_timecard_entries.Bulk_create.Bulk_createPostRequestBody_timecard_entries"/> and sets the default values.
        /// </summary>
        public Bulk_createPostRequestBody_timecard_entries()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Project_timecard_entries.Bulk_create.Bulk_createPostRequestBody_timecard_entries"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Project_timecard_entries.Bulk_create.Bulk_createPostRequestBody_timecard_entries CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Project_timecard_entries.Bulk_create.Bulk_createPostRequestBody_timecard_entries();
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
                { "billable", n => { Billable = n.GetBoolValue(); } },
                { "clock_in_id", n => { ClockInId = n.GetIntValue(); } },
                { "clock_in_time", n => { ClockInTime = n.GetStringValue(); } },
                { "clock_out_id", n => { ClockOutId = n.GetIntValue(); } },
                { "clock_out_time", n => { ClockOutTime = n.GetStringValue(); } },
                { "cost_code_id", n => { CostCodeId = n.GetIntValue(); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "hours", n => { Hours = n.GetStringValue(); } },
                { "line_item_type_id", n => { LineItemTypeId = n.GetIntValue(); } },
                { "lunch_clock_in_id", n => { LunchClockInId = n.GetIntValue(); } },
                { "lunch_clock_out_id", n => { LunchClockOutId = n.GetIntValue(); } },
                { "lunch_start_time", n => { LunchStartTime = n.GetStringValue(); } },
                { "lunch_stop_time", n => { LunchStopTime = n.GetStringValue(); } },
                { "origin_data", n => { OriginData = n.GetStringValue(); } },
                { "origin_id", n => { OriginId = n.GetIntValue(); } },
                { "party_id", n => { PartyId = n.GetIntValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
                { "set_timecard_time_type_automatically", n => { SetTimecardTimeTypeAutomatically = n.GetBoolValue(); } },
                { "timecard_time_type_id", n => { TimecardTimeTypeId = n.GetIntValue(); } },
                { "timesheet_id", n => { TimesheetId = n.GetIntValue(); } },
                { "wbs_code_id", n => { WbsCodeId = n.GetIntValue(); } },
                { "work_classification_id", n => { WorkClassificationId = n.GetIntValue(); } },
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
            writer.WriteBoolValue("billable", Billable);
            writer.WriteIntValue("clock_in_id", ClockInId);
            writer.WriteStringValue("clock_in_time", ClockInTime);
            writer.WriteIntValue("clock_out_id", ClockOutId);
            writer.WriteStringValue("clock_out_time", ClockOutTime);
            writer.WriteIntValue("cost_code_id", CostCodeId);
            writer.WriteDateValue("date", Date);
            writer.WriteStringValue("description", Description);
            writer.WriteStringValue("hours", Hours);
            writer.WriteIntValue("line_item_type_id", LineItemTypeId);
            writer.WriteIntValue("lunch_clock_in_id", LunchClockInId);
            writer.WriteIntValue("lunch_clock_out_id", LunchClockOutId);
            writer.WriteStringValue("lunch_start_time", LunchStartTime);
            writer.WriteStringValue("lunch_stop_time", LunchStopTime);
            writer.WriteStringValue("origin_data", OriginData);
            writer.WriteIntValue("origin_id", OriginId);
            writer.WriteIntValue("party_id", PartyId);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteBoolValue("set_timecard_time_type_automatically", SetTimecardTimeTypeAutomatically);
            writer.WriteIntValue("timecard_time_type_id", TimecardTimeTypeId);
            writer.WriteIntValue("timesheet_id", TimesheetId);
            writer.WriteIntValue("wbs_code_id", WbsCodeId);
            writer.WriteIntValue("work_classification_id", WorkClassificationId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
