// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item
{
    /// <summary>
    /// Checklist Template object
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class PatchRequestBody_list_template : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The ID of an Alternative Response Set</summary>
        public int? AlternativeResponseSetId { get; set; }
        /// <summary>Description</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>The ID of an Inspection Type</summary>
        public int? InspectionTypeId { get; set; }
        /// <summary>Name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The ID of a Trade</summary>
        public int? TradeId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody_list_template"/> and sets the default values.
        /// </summary>
        public PatchRequestBody_list_template()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody_list_template"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody_list_template CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody_list_template();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "alternative_response_set_id", n => { AlternativeResponseSetId = n.GetIntValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "inspection_type_id", n => { InspectionTypeId = n.GetIntValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "trade_id", n => { TradeId = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("alternative_response_set_id", AlternativeResponseSetId);
            writer.WriteStringValue("description", Description);
            writer.WriteIntValue("inspection_type_id", InspectionTypeId);
            writer.WriteStringValue("name", Name);
            writer.WriteIntValue("trade_id", TradeId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
