// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings
{
    /// <summary>
    /// Schedule Project Settings
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SettingsGetResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Calendar people filters enabled</summary>
        public bool? CalendarPeopleFiltersEnabled { get; set; }
        /// <summary>Company</summary>
        public int? CompanyId { get; set; }
        /// <summary>Create calendar item enabled</summary>
        public bool? CreateCalendarItemEnabled { get; set; }
        /// <summary>Display task names with full outline path</summary>
        public bool? DisplayTaskNamesWithFullOutlinePath { get; set; }
        /// <summary>Email settings</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse_email_settings? EmailSettings { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse_email_settings EmailSettings { get; set; }
#endif
        /// <summary>Primavera schedule</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PrimaveraScheduleId { get; set; }
#nullable restore
#else
        public string PrimaveraScheduleId { get; set; }
#endif
        /// <summary>Project</summary>
        public int? ProjectId { get; set; }
        /// <summary>Project integration</summary>
        public bool? ProjectIntegration { get; set; }
        /// <summary>Schedule allow task updates</summary>
        public bool? ScheduleAllowTaskUpdates { get; set; }
        /// <summary>Schedule file pattern</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ScheduleFilePattern { get; set; }
#nullable restore
#else
        public string ScheduleFilePattern { get; set; }
#endif
        /// <summary>Schedule show resources on calendar</summary>
        public bool? ScheduleShowResourcesOnCalendar { get; set; }
        /// <summary>Schedule task auto formatting</summary>
        public bool? ScheduleTaskAutoFormatting { get; set; }
        /// <summary>Schedule type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ScheduleType { get; set; }
#nullable restore
#else
        public string ScheduleType { get; set; }
#endif
        /// <summary>Schedule use project admin working days</summary>
        public bool? ScheduleUseProjectAdminWorkingDays { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse"/> and sets the default values.
        /// </summary>
        public SettingsGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "calendar_people_filters_enabled", n => { CalendarPeopleFiltersEnabled = n.GetBoolValue(); } },
                { "company_id", n => { CompanyId = n.GetIntValue(); } },
                { "create_calendar_item_enabled", n => { CreateCalendarItemEnabled = n.GetBoolValue(); } },
                { "display_task_names_with_full_outline_path", n => { DisplayTaskNamesWithFullOutlinePath = n.GetBoolValue(); } },
                { "email_settings", n => { EmailSettings = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse_email_settings>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse_email_settings.CreateFromDiscriminatorValue); } },
                { "primavera_schedule_id", n => { PrimaveraScheduleId = n.GetStringValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
                { "project_integration", n => { ProjectIntegration = n.GetBoolValue(); } },
                { "schedule_allow_task_updates", n => { ScheduleAllowTaskUpdates = n.GetBoolValue(); } },
                { "schedule_file_pattern", n => { ScheduleFilePattern = n.GetStringValue(); } },
                { "schedule_show_resources_on_calendar", n => { ScheduleShowResourcesOnCalendar = n.GetBoolValue(); } },
                { "schedule_task_auto_formatting", n => { ScheduleTaskAutoFormatting = n.GetBoolValue(); } },
                { "schedule_type", n => { ScheduleType = n.GetStringValue(); } },
                { "schedule_use_project_admin_working_days", n => { ScheduleUseProjectAdminWorkingDays = n.GetBoolValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("calendar_people_filters_enabled", CalendarPeopleFiltersEnabled);
            writer.WriteIntValue("company_id", CompanyId);
            writer.WriteBoolValue("create_calendar_item_enabled", CreateCalendarItemEnabled);
            writer.WriteBoolValue("display_task_names_with_full_outline_path", DisplayTaskNamesWithFullOutlinePath);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Schedule.Settings.SettingsGetResponse_email_settings>("email_settings", EmailSettings);
            writer.WriteStringValue("primavera_schedule_id", PrimaveraScheduleId);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteBoolValue("project_integration", ProjectIntegration);
            writer.WriteBoolValue("schedule_allow_task_updates", ScheduleAllowTaskUpdates);
            writer.WriteStringValue("schedule_file_pattern", ScheduleFilePattern);
            writer.WriteBoolValue("schedule_show_resources_on_calendar", ScheduleShowResourcesOnCalendar);
            writer.WriteBoolValue("schedule_task_auto_formatting", ScheduleTaskAutoFormatting);
            writer.WriteStringValue("schedule_type", ScheduleType);
            writer.WriteBoolValue("schedule_use_project_admin_working_days", ScheduleUseProjectAdminWorkingDays);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
