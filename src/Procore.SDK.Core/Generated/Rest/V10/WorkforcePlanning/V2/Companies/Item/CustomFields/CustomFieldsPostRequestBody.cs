// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields
{
    /// <summary>
    /// Request body for creating a Custom Field.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class CustomFieldsPostRequestBody : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>If true, allows this field to be used as a filter.</summary>
        public bool? CanFilter { get; set; }
        /// <summary>A description to help Admin users understand the field’s purpose.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>If true, only integrations can update this field.</summary>
        public bool? IntegrationOnly { get; set; }
        /// <summary>The name of the Custom Field that appears in the UI.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>If true, the field is available on People.</summary>
        public bool? OnPeople { get; set; }
        /// <summary>If true, the field is available on Projects.</summary>
        public bool? OnProjects { get; set; }
        /// <summary>Only applicable for `select` or `multi-select` fields. Controls sorting of dropdown values. `alpha` sorts alphabetically, while `listed` maintains the provided order.</summary>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_sort_by? SortBy { get; set; }
        /// <summary>The type of Custom Field. Determines the kind of data it will store. The type cannot be changed once created.</summary>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_type? Type { get; set; }
        /// <summary>Only applicable for `select` or `multi-select` fields. List of values that will be options in the field&apos;s dropdown.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? Values { get; set; }
#nullable restore
#else
        public List<string> Values { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody"/> and sets the default values.
        /// </summary>
        public CustomFieldsPostRequestBody()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "can_filter", n => { CanFilter = n.GetBoolValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "integration_only", n => { IntegrationOnly = n.GetBoolValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "on_people", n => { OnPeople = n.GetBoolValue(); } },
                { "on_projects", n => { OnProjects = n.GetBoolValue(); } },
                { "sort_by", n => { SortBy = n.GetEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_sort_by>(); } },
                { "type", n => { Type = n.GetEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_type>(); } },
                { "values", n => { Values = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("can_filter", CanFilter);
            writer.WriteStringValue("description", Description);
            writer.WriteBoolValue("integration_only", IntegrationOnly);
            writer.WriteStringValue("name", Name);
            writer.WriteBoolValue("on_people", OnPeople);
            writer.WriteBoolValue("on_projects", OnProjects);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_sort_by>("sort_by", SortBy);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.CustomFields.CustomFieldsPostRequestBody_type>("type", Type);
            writer.WriteCollectionOfPrimitiveValues<string>("values", Values);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
