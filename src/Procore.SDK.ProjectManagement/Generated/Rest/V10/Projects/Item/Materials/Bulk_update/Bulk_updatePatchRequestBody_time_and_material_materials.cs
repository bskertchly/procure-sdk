// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Materials.Bulk_update
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Bulk_updatePatchRequestBody_time_and_material_materials : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Description of the material</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Name of the material</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Quantity of the material</summary>
        public float? Quantity { get; set; }
        /// <summary>Time &amp; Material Entry Id the material is associated with</summary>
        public int? TimeAndMaterialEntryId { get; set; }
        /// <summary>Unit of measure for the material</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Uom { get; set; }
#nullable restore
#else
        public string Uom { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Materials.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_materials"/> and sets the default values.
        /// </summary>
        public Bulk_updatePatchRequestBody_time_and_material_materials()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Materials.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_materials"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Materials.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_materials CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Materials.Bulk_update.Bulk_updatePatchRequestBody_time_and_material_materials();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "description", n => { Description = n.GetStringValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "quantity", n => { Quantity = n.GetFloatValue(); } },
                { "time_and_material_entry_id", n => { TimeAndMaterialEntryId = n.GetIntValue(); } },
                { "uom", n => { Uom = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("description", Description);
            writer.WriteStringValue("name", Name);
            writer.WriteFloatValue("quantity", Quantity);
            writer.WriteIntValue("time_and_material_entry_id", TimeAndMaterialEntryId);
            writer.WriteStringValue("uom", Uom);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
