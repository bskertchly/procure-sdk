// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item
{
    /// <summary>
    /// Line Item Type
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Line_itemsGetResponse_cost_codeMember2_line_item_types : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Base type</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types_base_type? BaseType { get; set; }
        /// <summary>Code for the Line Item Type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Code { get; set; }
#nullable restore
#else
        public string Code { get; set; }
#endif
        /// <summary>Unique identifier for the Line Item Type</summary>
        public int? Id { get; set; }
        /// <summary>Name for the Line Item Type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Origin data</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? OriginData { get; set; }
#nullable restore
#else
        public string OriginData { get; set; }
#endif
        /// <summary>Origin ID</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? OriginId { get; set; }
#nullable restore
#else
        public string OriginId { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types"/> and sets the default values.
        /// </summary>
        public Line_itemsGetResponse_cost_codeMember2_line_item_types()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "base_type", n => { BaseType = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types_base_type>(); } },
                { "code", n => { Code = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "origin_data", n => { OriginData = n.GetStringValue(); } },
                { "origin_id", n => { OriginId = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Item.Line_items.Item.Line_itemsGetResponse_cost_codeMember2_line_item_types_base_type>("base_type", BaseType);
            writer.WriteStringValue("code", Code);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("origin_data", OriginData);
            writer.WriteStringValue("origin_id", OriginId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
