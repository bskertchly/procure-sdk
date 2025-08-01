// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates
{
    /// <summary>
    /// Company Action Plan Template (Show)
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Plan_templates : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Timestamp of creation</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>Description in rich text form</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Description in plain text form</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? DescriptionPlainText { get; set; }
#nullable restore
#else
        public string DescriptionPlainText { get; set; }
#endif
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>The plan_type property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_plan_type? PlanType { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_plan_type PlanType { get; set; }
#endif
        /// <summary>Flag for if the Action Plan Template is private</summary>
        public bool? Private { get; set; }
        /// <summary>Template&apos;s provider type (company/project)</summary>
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_provider_type? ProviderType { get; set; }
        /// <summary>Current state of the Company Action Plan Template</summary>
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_status? Status { get; set; }
        /// <summary>Title</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>Timestamp of last update</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates"/> and sets the default values.
        /// </summary>
        public Plan_templates()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "description_plain_text", n => { DescriptionPlainText = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "plan_type", n => { PlanType = n.GetObjectValue<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_plan_type>(global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_plan_type.CreateFromDiscriminatorValue); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "provider_type", n => { ProviderType = n.GetEnumValue<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_provider_type>(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_status>(); } },
                { "title", n => { Title = n.GetStringValue(); } },
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
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteStringValue("description", Description);
            writer.WriteStringValue("description_plain_text", DescriptionPlainText);
            writer.WriteIntValue("id", Id);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_plan_type>("plan_type", PlanType);
            writer.WriteBoolValue("private", Private);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_provider_type>("provider_type", ProviderType);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Plan_templates_status>("status", Status);
            writer.WriteStringValue("title", Title);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
