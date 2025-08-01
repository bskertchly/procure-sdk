// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Equipment_logsPatchResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Equipment Log Attachments are not viewable or used on web</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_attachments>? Attachments { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_attachments> Attachments { get; set; }
#endif
        /// <summary>The cost_code property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_cost_code? CostCode { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_cost_code CostCode { get; set; }
#endif
        /// <summary>Created at</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_created_by CreatedBy { get; set; }
#endif
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_custom_fields CustomFields { get; set; }
#endif
        /// <summary>Date of record</summary>
        public Date? Date { get; set; }
        /// <summary>Estimated UTC datetime of record</summary>
        public DateTimeOffset? Datetime { get; set; }
        /// <summary>Equipment of Equipment (Legacy) Tool. Can&apos;t be present at the same time with &apos;equipment_register&apos; field</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment? Equipment { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment Equipment { get; set; }
#endif
        /// <summary>Equipment of Equipment Register (Beta) Tool. Can&apos;t be present at the same time with &apos;equipment&apos; field</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment_register? EquipmentRegister { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment_register EquipmentRegister { get; set; }
#endif
        /// <summary>Number of hours the Equipment was idle</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? HoursIdle { get; set; }
#nullable restore
#else
        public string HoursIdle { get; set; }
#endif
        /// <summary>Number of hours the Equipment was in operation</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? HoursOperating { get; set; }
#nullable restore
#else
        public string HoursOperating { get; set; }
#endif
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Equipment was inspected before operation</summary>
        public bool? Inspected { get; set; }
        /// <summary>Time of inspection - hour</summary>
        public int? InspectionHour { get; set; }
        /// <summary>Time of Inspection - minute</summary>
        public int? InspectionMinute { get; set; }
        /// <summary>The location property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_location? Location { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_location Location { get; set; }
#endif
        /// <summary>Additional Notes</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Notes { get; set; }
#nullable restore
#else
        public string Notes { get; set; }
#endif
        /// <summary>TBD</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_permissions? Permissions { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_permissions Permissions { get; set; }
#endif
        /// <summary>Order in which this entry was recorded for the day</summary>
        public int? Position { get; set; }
        /// <summary>Updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>The vendor property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_vendor? Vendor { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_vendor Vendor { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse"/> and sets the default values.
        /// </summary>
        public Equipment_logsPatchResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "attachments", n => { Attachments = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_attachments>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_attachments.CreateFromDiscriminatorValue)?.AsList(); } },
                { "cost_code", n => { CostCode = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_cost_code>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_cost_code.CreateFromDiscriminatorValue); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_created_by>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_created_by.CreateFromDiscriminatorValue); } },
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_custom_fields>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_custom_fields.CreateFromDiscriminatorValue); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "datetime", n => { Datetime = n.GetDateTimeOffsetValue(); } },
                { "equipment", n => { Equipment = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment.CreateFromDiscriminatorValue); } },
                { "equipment_register", n => { EquipmentRegister = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment_register>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment_register.CreateFromDiscriminatorValue); } },
                { "hours_idle", n => { HoursIdle = n.GetStringValue(); } },
                { "hours_operating", n => { HoursOperating = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "inspected", n => { Inspected = n.GetBoolValue(); } },
                { "inspection_hour", n => { InspectionHour = n.GetIntValue(); } },
                { "inspection_minute", n => { InspectionMinute = n.GetIntValue(); } },
                { "location", n => { Location = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_location>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_location.CreateFromDiscriminatorValue); } },
                { "notes", n => { Notes = n.GetStringValue(); } },
                { "permissions", n => { Permissions = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_permissions>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_permissions.CreateFromDiscriminatorValue); } },
                { "position", n => { Position = n.GetIntValue(); } },
                { "updated_at", n => { UpdatedAt = n.GetDateTimeOffsetValue(); } },
                { "vendor", n => { Vendor = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_vendor>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_vendor.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_attachments>("attachments", Attachments);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_cost_code>("cost_code", CostCode);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_created_by>("created_by", CreatedBy);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_custom_fields>("custom_fields", CustomFields);
            writer.WriteDateValue("date", Date);
            writer.WriteDateTimeOffsetValue("datetime", Datetime);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment>("equipment", Equipment);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_equipment_register>("equipment_register", EquipmentRegister);
            writer.WriteStringValue("hours_idle", HoursIdle);
            writer.WriteStringValue("hours_operating", HoursOperating);
            writer.WriteIntValue("id", Id);
            writer.WriteBoolValue("inspected", Inspected);
            writer.WriteIntValue("inspection_hour", InspectionHour);
            writer.WriteIntValue("inspection_minute", InspectionMinute);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_location>("location", Location);
            writer.WriteStringValue("notes", Notes);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_permissions>("permissions", Permissions);
            writer.WriteIntValue("position", Position);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Equipment_logs.Item.Equipment_logsPatchResponse_vendor>("vendor", Vendor);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
