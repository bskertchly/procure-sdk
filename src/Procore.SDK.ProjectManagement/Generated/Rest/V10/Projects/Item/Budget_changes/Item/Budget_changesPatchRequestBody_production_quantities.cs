// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Item
{
    /// <summary>
    /// Budget Change Adjustment Production Quantity
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Budget_changesPatchRequestBody_production_quantities : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>ID of the Change Event Production Quantity that is to be associated with the Budget Change Production Quantity</summary>
        public double? ChangeEventProductionQuantityId { get; set; }
        /// <summary>Comment of the adjustment</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Comment { get; set; }
#nullable restore
#else
        public string Comment { get; set; }
#endif
        /// <summary>Cost Code ID</summary>
        public int? CostCodeId { get; set; }
        /// <summary>Whether this production quantity should be deleted</summary>
        public bool? Delete { get; set; }
        /// <summary>Description of the Production Quantity</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>ID of this Production Quantity</summary>
        public int? Id { get; set; }
        /// <summary>Estimated cost quantity</summary>
        public double? Quantity { get; set; }
        /// <summary>Identifier used to map production quantities in the request to their respective objects or errors in the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Ref { get; set; }
#nullable restore
#else
        public string Ref { get; set; }
#endif
        /// <summary>Unit of measure used</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Uom { get; set; }
#nullable restore
#else
        public string Uom { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Item.Budget_changesPatchRequestBody_production_quantities"/> and sets the default values.
        /// </summary>
        public Budget_changesPatchRequestBody_production_quantities()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Item.Budget_changesPatchRequestBody_production_quantities"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Item.Budget_changesPatchRequestBody_production_quantities CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Item.Budget_changesPatchRequestBody_production_quantities();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "change_event_production_quantity_id", n => { ChangeEventProductionQuantityId = n.GetDoubleValue(); } },
                { "comment", n => { Comment = n.GetStringValue(); } },
                { "cost_code_id", n => { CostCodeId = n.GetIntValue(); } },
                { "_delete", n => { Delete = n.GetBoolValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "quantity", n => { Quantity = n.GetDoubleValue(); } },
                { "ref", n => { Ref = n.GetStringValue(); } },
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
            writer.WriteDoubleValue("change_event_production_quantity_id", ChangeEventProductionQuantityId);
            writer.WriteStringValue("comment", Comment);
            writer.WriteIntValue("cost_code_id", CostCodeId);
            writer.WriteBoolValue("_delete", Delete);
            writer.WriteStringValue("description", Description);
            writer.WriteIntValue("id", Id);
            writer.WriteDoubleValue("quantity", Quantity);
            writer.WriteStringValue("ref", Ref);
            writer.WriteStringValue("uom", Uom);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
