// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item
{
    /// <summary>
    /// Existing Custom Fields to be edited for this Configurable Field Set
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1 : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Column the Field is position on the Form</summary>
        public double? Column { get; set; }
        /// <summary>How many columns the field spans on the Form</summary>
        public double? ColumnWidth { get; set; }
        /// <summary>Custom Field Definition ID</summary>
        public int? CustomFieldDefinitionId { get; set; }
        /// <summary>Data type of the Custom Field</summary>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1_data_type? DataType { get; set; }
        /// <summary>The description of the Custom Field Definition</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Custom Field Metadatum ID</summary>
        public int? Id { get; set; }
        /// <summary>The label of the Custom Field Definition</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Label { get; set; }
#nullable restore
#else
        public string Label { get; set; }
#endif
        /// <summary>The name of the Custom Field</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The display position of the Custom Field, which is sorted ascending, lowest position is visually the top left of the page on a grid basis (used in conjunction with column_width property to calculate row and column properties).</summary>
        public int? Position { get; set; }
        /// <summary>Whether or not the Field is required</summary>
        public bool? Required { get; set; }
        /// <summary>Row the Field is position on the Form</summary>
        public double? Row { get; set; }
        /// <summary>Whether or not the Custom Field is visible</summary>
        public bool? Visible { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1"/> and sets the default values.
        /// </summary>
        public Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1 CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "column", n => { Column = n.GetDoubleValue(); } },
                { "column_width", n => { ColumnWidth = n.GetDoubleValue(); } },
                { "custom_field_definition_id", n => { CustomFieldDefinitionId = n.GetIntValue(); } },
                { "data_type", n => { DataType = n.GetEnumValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1_data_type>(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "label", n => { Label = n.GetStringValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "position", n => { Position = n.GetIntValue(); } },
                { "required", n => { Required = n.GetBoolValue(); } },
                { "row", n => { Row = n.GetDoubleValue(); } },
                { "visible", n => { Visible = n.GetBoolValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteDoubleValue("column", Column);
            writer.WriteDoubleValue("column_width", ColumnWidth);
            writer.WriteIntValue("custom_field_definition_id", CustomFieldDefinitionId);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Configurable_field_sets.Item.Configurable_field_setsPatchRequestBody_configurable_field_set_fields_custom_field_1_data_type>("data_type", DataType);
            writer.WriteStringValue("description", Description);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("label", Label);
            writer.WriteStringValue("name", Name);
            writer.WriteIntValue("position", Position);
            writer.WriteBoolValue("required", Required);
            writer.WriteDoubleValue("row", Row);
            writer.WriteBoolValue("visible", Visible);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
