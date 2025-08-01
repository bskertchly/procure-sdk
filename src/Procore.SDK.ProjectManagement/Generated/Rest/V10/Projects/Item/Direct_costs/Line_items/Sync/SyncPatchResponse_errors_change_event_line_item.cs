// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync
{
    /// <summary>
    /// Change Event Line Item
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SyncPatchResponse_errors_change_event_line_item : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The cost_code property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code? CostCode { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code CostCode { get; set; }
#endif
        /// <summary>Change Event Line Item Cost ROM</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CostRom { get; set; }
#nullable restore
#else
        public string CostRom { get; set; }
#endif
        /// <summary>Change Event ID</summary>
        public int? EventId { get; set; }
        /// <summary>Change Event Line Item ID</summary>
        public int? Id { get; set; }
        /// <summary>Line Item Type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_line_item_type? LineItemType { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_line_item_type LineItemType { get; set; }
#endif
        /// <summary>Change Event Line Item Revenue ROM</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? RevenueRom { get; set; }
#nullable restore
#else
        public string RevenueRom { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item"/> and sets the default values.
        /// </summary>
        public SyncPatchResponse_errors_change_event_line_item()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "cost_code", n => { CostCode = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code.CreateFromDiscriminatorValue); } },
                { "cost_rom", n => { CostRom = n.GetStringValue(); } },
                { "event_id", n => { EventId = n.GetIntValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "line_item_type", n => { LineItemType = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_line_item_type>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_line_item_type.CreateFromDiscriminatorValue); } },
                { "revenue_rom", n => { RevenueRom = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code>("cost_code", CostCode);
            writer.WriteStringValue("cost_rom", CostRom);
            writer.WriteIntValue("event_id", EventId);
            writer.WriteIntValue("id", Id);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_line_item_type>("line_item_type", LineItemType);
            writer.WriteStringValue("revenue_rom", RevenueRom);
            writer.WriteAdditionalData(AdditionalData);
        }
        /// <summary>
        /// Composed type wrapper for classes <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember1"/>, <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember2"/>
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class SyncPatchResponse_errors_change_event_line_item_cost_code : IComposedTypeWrapper, IParsable
        {
            /// <summary>Composed type representation for type <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember1"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember1? SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1 { get; set; }
#nullable restore
#else
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember1 SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1 { get; set; }
#endif
            /// <summary>Composed type representation for type <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember2"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember2? SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2 { get; set; }
#nullable restore
#else
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember2 SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2 { get; set; }
#endif
            /// <summary>
            /// Creates a new instance of the appropriate class based on discriminator value
            /// </summary>
            /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code"/></returns>
            /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
            public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code CreateFromDiscriminatorValue(IParseNode parseNode)
            {
                _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
                var mappingValue = parseNode.GetChildNode("")?.GetStringValue();
                var result = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item.SyncPatchResponse_errors_change_event_line_item_cost_code();
                if("".Equals(mappingValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1 = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember1();
                }
                else if("".Equals(mappingValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2 = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember2();
                }
                return result;
            }
            /// <summary>
            /// The deserialization information for the current model
            /// </summary>
            /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
            public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
            {
                if(SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1 != null)
                {
                    return SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1.GetFieldDeserializers();
                }
                else if(SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2 != null)
                {
                    return SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2.GetFieldDeserializers();
                }
                return new Dictionary<string, Action<IParseNode>>();
            }
            /// <summary>
            /// Serializes information the current object
            /// </summary>
            /// <param name="writer">Serialization writer to use to serialize this model</param>
            public virtual void Serialize(ISerializationWriter writer)
            {
                _ = writer ?? throw new ArgumentNullException(nameof(writer));
                if(SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1 != null)
                {
                    writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember1>(null, SyncPatchResponseErrorsChangeEventLineItemCostCodeMember1);
                }
                else if(SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2 != null)
                {
                    writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncPatchResponse_errors_change_event_line_item_cost_codeMember2>(null, SyncPatchResponseErrorsChangeEventLineItemCostCodeMember2);
                }
            }
        }
    }
}
#pragma warning restore CS0618
