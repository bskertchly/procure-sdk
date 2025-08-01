// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item
{
    /// <summary>
    /// Budgeted Production Quantity Object
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>CostCode.  DO NOT provide if your project is configured for Task Codes.</summary>
        public int? CostCodeId { get; set; }
        /// <summary>Project</summary>
        public int? ProjectId { get; set; }
        /// <summary>Quantity budgeted for a project cost code</summary>
        public double? Quantity { get; set; }
        /// <summary>Unit of Measure</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity_unit_of_measure? UnitOfMeasure { get; set; }
        /// <summary>The Production Quantity Code for the Budgeted Production Quantity. This is necessary if your project is configured for Task Codes. DO NOT provide if your project is not configured for Task Codes.</summary>
        public int? WbsCodeId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity"/> and sets the default values.
        /// </summary>
        public Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "cost_code_id", n => { CostCodeId = n.GetIntValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
                { "quantity", n => { Quantity = n.GetDoubleValue(); } },
                { "unit_of_measure", n => { UnitOfMeasure = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity_unit_of_measure>(); } },
                { "wbs_code_id", n => { WbsCodeId = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("cost_code_id", CostCodeId);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteDoubleValue("quantity", Quantity);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budgeted_production_quantities.Item.Budgeted_production_quantitiesPatchRequestBody_budgeted_production_quantity_unit_of_measure>("unit_of_measure", UnitOfMeasure);
            writer.WriteIntValue("wbs_code_id", WbsCodeId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
