// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class List_templates : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Company Description</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CompanyDescription { get; set; }
#nullable restore
#else
        public string CompanyDescription { get; set; }
#endif
        /// <summary>Timestamp of creation</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>Description</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Display Conditions</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_display_conditions>? DisplayConditions { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_display_conditions> DisplayConditions { get; set; }
#endif
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>The inspection_type property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_inspection_type? InspectionType { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_inspection_type InspectionType { get; set; }
#endif
        /// <summary>Name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Number { get; set; }
#nullable restore
#else
        public string Number { get; set; }
#endif
        /// <summary>Parent Inspection Item ID</summary>
        public int? ParentInspectionItemId { get; set; }
        /// <summary>Relative Position</summary>
        public int? RelativePosition { get; set; }
        /// <summary>The synced_to property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_synced_to? SyncedTo { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_synced_to SyncedTo { get; set; }
#endif
        /// <summary>The trade property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_trade? Trade { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_trade Trade { get; set; }
#endif
        /// <summary>Timestamp of last update</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates"/> and sets the default values.
        /// </summary>
        public List_templates()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "company_description", n => { CompanyDescription = n.GetStringValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "display_conditions", n => { DisplayConditions = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_display_conditions>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_display_conditions.CreateFromDiscriminatorValue)?.AsList(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "inspection_type", n => { InspectionType = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_inspection_type>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_inspection_type.CreateFromDiscriminatorValue); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "number", n => { Number = n.GetStringValue(); } },
                { "parent_inspection_item_id", n => { ParentInspectionItemId = n.GetIntValue(); } },
                { "relative_position", n => { RelativePosition = n.GetIntValue(); } },
                { "synced_to", n => { SyncedTo = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_synced_to>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_synced_to.CreateFromDiscriminatorValue); } },
                { "trade", n => { Trade = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_trade>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_trade.CreateFromDiscriminatorValue); } },
                { "updated_at", n => { UpdatedAt = n.GetDateTimeOffsetValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("company_description", CompanyDescription);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteStringValue("description", Description);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_display_conditions>("display_conditions", DisplayConditions);
            writer.WriteIntValue("id", Id);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_inspection_type>("inspection_type", InspectionType);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("number", Number);
            writer.WriteIntValue("parent_inspection_item_id", ParentInspectionItemId);
            writer.WriteIntValue("relative_position", RelativePosition);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_synced_to>("synced_to", SyncedTo);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Checklist.List_templates.List_templates_trade>("trade", Trade);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
