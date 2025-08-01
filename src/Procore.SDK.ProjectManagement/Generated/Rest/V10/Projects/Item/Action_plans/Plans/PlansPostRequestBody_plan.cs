// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class PlansPostRequestBody_plan : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Value of the custom field. The data type of the value passed in corresponds with the data_type of the Custom Field Definition.For a lov_entry data_type the value passed in should be the ID of one of the Custom Field Definition&apos;s LOV Entries. For a lov_entries data_type the value passed in should be an array of IDs of the Custom Field Definition&apos;s LOV Entries.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id? CustomFieldCustomFieldDefinitionId { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id CustomFieldCustomFieldDefinitionId { get; set; }
#endif
        /// <summary>Description of the Action Plan</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Location ID to be set on the Action Plan</summary>
        public int? LocationId { get; set; }
        /// <summary>Party Person ID of the Action Plan Manager</summary>
        public int? ManagerId { get; set; }
        /// <summary>The plan_approvers_attributes property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_approvers_attributes>? PlanApproversAttributes { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_approvers_attributes> PlanApproversAttributes { get; set; }
#endif
        /// <summary>The plan_receivers_attributes property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_receivers_attributes>? PlanReceiversAttributes { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_receivers_attributes> PlanReceiversAttributes { get; set; }
#endif
        /// <summary>Plan Type ID to be set on the Action Plan</summary>
        public int? PlanTypeId { get; set; }
        /// <summary>Privacy flag of the Action Plan</summary>
        public bool? Private { get; set; }
        /// <summary>Title of the Action Plan</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan"/> and sets the default values.
        /// </summary>
        public PlansPostRequestBody_plan()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "custom_field_%{custom_field_definition_id}", n => { CustomFieldCustomFieldDefinitionId = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id.CreateFromDiscriminatorValue); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "location_id", n => { LocationId = n.GetIntValue(); } },
                { "manager_id", n => { ManagerId = n.GetIntValue(); } },
                { "plan_approvers_attributes", n => { PlanApproversAttributes = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_approvers_attributes>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_approvers_attributes.CreateFromDiscriminatorValue)?.AsList(); } },
                { "plan_receivers_attributes", n => { PlanReceiversAttributes = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_receivers_attributes>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_receivers_attributes.CreateFromDiscriminatorValue)?.AsList(); } },
                { "plan_type_id", n => { PlanTypeId = n.GetIntValue(); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id>("custom_field_%{custom_field_definition_id}", CustomFieldCustomFieldDefinitionId);
            writer.WriteStringValue("description", Description);
            writer.WriteIntValue("location_id", LocationId);
            writer.WriteIntValue("manager_id", ManagerId);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_approvers_attributes>("plan_approvers_attributes", PlanApproversAttributes);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan_plan_receivers_attributes>("plan_receivers_attributes", PlanReceiversAttributes);
            writer.WriteIntValue("plan_type_id", PlanTypeId);
            writer.WriteBoolValue("private", Private);
            writer.WriteStringValue("title", Title);
            writer.WriteAdditionalData(AdditionalData);
        }
        /// <summary>
        /// Composed type wrapper for classes <see cref="bool"/>, <see cref="double"/>, <see cref="string"/>, List&lt;int&gt;
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class PlansPostRequestBody_plan_custom_field_Custom_field_definition_id : IComposedTypeWrapper, IParsable
        {
            /// <summary>Composed type representation for type <see cref="bool"/></summary>
            public bool? Boolean { get; set; }
            /// <summary>Composed type representation for type <see cref="double"/></summary>
            public double? Double { get; set; }
            /// <summary>Composed type representation for type List&lt;int&gt;</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public List<int?>? Integer { get; set; }
#nullable restore
#else
            public List<int?> Integer { get; set; }
#endif
            /// <summary>Composed type representation for type <see cref="string"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public string? String { get; set; }
#nullable restore
#else
            public string String { get; set; }
#endif
            /// <summary>
            /// Creates a new instance of the appropriate class based on discriminator value
            /// </summary>
            /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id"/></returns>
            /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
            public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id CreateFromDiscriminatorValue(IParseNode parseNode)
            {
                _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
                var mappingValue = parseNode.GetChildNode("")?.GetStringValue();
                var result = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plans.PlansPostRequestBody_plan.PlansPostRequestBody_plan_custom_field_Custom_field_definition_id();
                if(parseNode.GetBoolValue() is bool booleanValue)
                {
                    result.Boolean = booleanValue;
                }
                else if(parseNode.GetDoubleValue() is double doubleValue)
                {
                    result.Double = doubleValue;
                }
                else if(parseNode.GetStringValue() is string stringValue)
                {
                    result.String = stringValue;
                }
                else if(parseNode.GetCollectionOfPrimitiveValues<int?>()?.AsList() is List<int?> integerValue)
                {
                    result.Integer = integerValue;
                }
                return result;
            }
            /// <summary>
            /// The deserialization information for the current model
            /// </summary>
            /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
            public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
            {
                return new Dictionary<string, Action<IParseNode>>();
            }
            /// <summary>
            /// Serializes information the current object
            /// </summary>
            /// <param name="writer">Serialization writer to use to serialize this model</param>
            public virtual void Serialize(ISerializationWriter writer)
            {
                _ = writer ?? throw new ArgumentNullException(nameof(writer));
                if(Boolean != null)
                {
                    writer.WriteBoolValue(null, Boolean);
                }
                else if(Double != null)
                {
                    writer.WriteDoubleValue(null, Double);
                }
                else if(String != null)
                {
                    writer.WriteStringValue(null, String);
                }
                else if(Integer != null)
                {
                    writer.WriteCollectionOfPrimitiveValues<int?>(null, Integer);
                }
            }
        }
    }
}
#pragma warning restore CS0618
