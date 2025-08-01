// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling
{
    /// <summary>
    /// A row of bid items for a cost code
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The bid_items property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_bid_items>? BidItems { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_bid_items> BidItems { get; set; }
#endif
        /// <summary>Cost code ID</summary>
        public int? CostCodeId { get; set; }
        /// <summary>Description</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>The Cost code Name &amp; Number in one string</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FullCode { get; set; }
#nullable restore
#else
        public string FullCode { get; set; }
#endif
        /// <summary>Bid Form Item ID</summary>
        public int? Id { get; set; }
        /// <summary>Line Item Type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ItemType { get; set; }
#nullable restore
#else
        public string ItemType { get; set; }
#endif
        /// <summary>Cost Code Name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Cost Code Number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Number { get; set; }
#nullable restore
#else
        public string Number { get; set; }
#endif
        /// <summary>Bid Form Items can have various response types. This property determines which one is used.</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_response_type? ResponseType { get; set; }
        /// <summary>Plain text subject</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Subject { get; set; }
#nullable restore
#else
        public string Subject { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items"/> and sets the default values.
        /// </summary>
        public Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "bid_items", n => { BidItems = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_bid_items>(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_bid_items.CreateFromDiscriminatorValue)?.AsList(); } },
                { "cost_code_id", n => { CostCodeId = n.GetIntValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "full_code", n => { FullCode = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "item_type", n => { ItemType = n.GetStringValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "number", n => { Number = n.GetStringValue(); } },
                { "response_type", n => { ResponseType = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_response_type>(); } },
                { "subject", n => { Subject = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_bid_items>("bid_items", BidItems);
            writer.WriteIntValue("cost_code_id", CostCodeId);
            writer.WriteStringValue("description", Description);
            writer.WriteStringValue("full_code", FullCode);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("item_type", ItemType);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("number", Number);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Bid_packages.Item.Bid_forms.Item.Bid_leveling.Bid_levelingGetResponseMember1_base_bid_section_sub_sections_bid_form_items_response_type>("response_type", ResponseType);
            writer.WriteStringValue("subject", Subject);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
