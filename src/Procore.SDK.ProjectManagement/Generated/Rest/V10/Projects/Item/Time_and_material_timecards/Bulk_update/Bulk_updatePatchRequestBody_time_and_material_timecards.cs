// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_timecards.Bulk_update
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Bulk_updatePatchRequestBody_time_and_material_timecards : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Total hours worked</summary>
        public float? HoursWorked { get; set; }
        /// <summary>ID of the person the timecard is being created for</summary>
        public int? LoginInformationId { get; set; }
        /// <summary>Time &amp; Material Entry Id the timecard is associated with</summary>
        public int? TimeAndMaterialEntryId { get; set; }
        /// <summary>Type id for the type of timecard being created</summary>
        public int? TimecardTimeTypeId { get; set; }
        /// <summary>ID of the worker&apos;s work classification</summary>
        public int? WorkClassificationId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_timecards.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_timecards"/> and sets the default values.
        /// </summary>
        public Bulk_updatePatchRequestBody_time_and_material_timecards()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_timecards.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_timecards"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_timecards.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_timecards CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_timecards.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_timecards();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "hours_worked", n => { HoursWorked = n.GetFloatValue(); } },
                { "login_information_id", n => { LoginInformationId = n.GetIntValue(); } },
                { "time_and_material_entry_id", n => { TimeAndMaterialEntryId = n.GetIntValue(); } },
                { "timecard_time_type_id", n => { TimecardTimeTypeId = n.GetIntValue(); } },
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
            writer.WriteFloatValue("hours_worked", HoursWorked);
            writer.WriteIntValue("login_information_id", LoginInformationId);
            writer.WriteIntValue("time_and_material_entry_id", TimeAndMaterialEntryId);
            writer.WriteIntValue("timecard_time_type_id", TimecardTimeTypeId);
            writer.WriteIntValue("work_classification_id", WorkClassificationId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
