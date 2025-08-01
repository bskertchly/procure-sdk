// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Bulk_update
{
    /// <summary>
    /// Managed Equipment Type Object
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Bulk_updatePatchRequestBody_managed_equipment_type : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>If the equipment type is active</summary>
        public bool? IsActive { get; set; }
        /// <summary>The category id the Managed Equipment Type is associated with</summary>
        public int? ManagedEquipmentCategoryId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Bulk_update.Bulk_updatePatchRequestBody_managed_equipment_type"/> and sets the default values.
        /// </summary>
        public Bulk_updatePatchRequestBody_managed_equipment_type()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Bulk_update.Bulk_updatePatchRequestBody_managed_equipment_type"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Bulk_update.Bulk_updatePatchRequestBody_managed_equipment_type CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Bulk_update.Bulk_updatePatchRequestBody_managed_equipment_type();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "is_active", n => { IsActive = n.GetBoolValue(); } },
                { "managed_equipment_category_id", n => { ManagedEquipmentCategoryId = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("is_active", IsActive);
            writer.WriteIntValue("managed_equipment_category_id", ManagedEquipmentCategoryId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
