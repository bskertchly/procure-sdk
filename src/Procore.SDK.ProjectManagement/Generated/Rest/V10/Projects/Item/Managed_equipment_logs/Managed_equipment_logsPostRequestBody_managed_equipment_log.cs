// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Managed_equipment_logs
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Managed_equipment_logsPostRequestBody_managed_equipment_log : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Id of the inspection list the equipment uses</summary>
        public int? InductionChecklistListId { get; set; }
        /// <summary>The number used for equipment induction</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? InductionNumber { get; set; }
#nullable restore
#else
        public string InductionNumber { get; set; }
#endif
        /// <summary>Indicates if the equipemnt has been successfully inspected and allowed to perform work</summary>
        public bool? InductionStatus { get; set; }
        /// <summary>The date the equipment was inspected</summary>
        public Date? InspectionDate { get; set; }
        /// <summary>Equipment Id the log is associated with</summary>
        public int? ManagedEquipmentId { get; set; }
        /// <summary>The Date equipment left the site</summary>
        public Date? Offsite { get; set; }
        /// <summary>The Date equipment arrived on site</summary>
        public Date? Onsite { get; set; }
        /// <summary>ID of the project the equipment was logged for</summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Managed_equipment_logs.Managed_equipment_logsPostRequestBody_managed_equipment_log"/> and sets the default values.
        /// </summary>
        public Managed_equipment_logsPostRequestBody_managed_equipment_log()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Managed_equipment_logs.Managed_equipment_logsPostRequestBody_managed_equipment_log"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Managed_equipment_logs.Managed_equipment_logsPostRequestBody_managed_equipment_log CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Managed_equipment_logs.Managed_equipment_logsPostRequestBody_managed_equipment_log();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "induction_checklist_list_id", n => { InductionChecklistListId = n.GetIntValue(); } },
                { "induction_number", n => { InductionNumber = n.GetStringValue(); } },
                { "induction_status", n => { InductionStatus = n.GetBoolValue(); } },
                { "inspection_date", n => { InspectionDate = n.GetDateValue(); } },
                { "managed_equipment_id", n => { ManagedEquipmentId = n.GetIntValue(); } },
                { "offsite", n => { Offsite = n.GetDateValue(); } },
                { "onsite", n => { Onsite = n.GetDateValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("induction_checklist_list_id", InductionChecklistListId);
            writer.WriteStringValue("induction_number", InductionNumber);
            writer.WriteBoolValue("induction_status", InductionStatus);
            writer.WriteDateValue("inspection_date", InspectionDate);
            writer.WriteIntValue("managed_equipment_id", ManagedEquipmentId);
            writer.WriteDateValue("offsite", Offsite);
            writer.WriteDateValue("onsite", Onsite);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
