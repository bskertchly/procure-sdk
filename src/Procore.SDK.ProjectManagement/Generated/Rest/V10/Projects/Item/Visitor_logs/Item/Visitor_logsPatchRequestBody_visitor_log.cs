// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Visitor_logsPatchRequestBody_visitor_log : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Time of visitation - hour</summary>
        public int? BeginHour { get; set; }
        /// <summary>Time of visitation - hour</summary>
        public int? BeginMinute { get; set; }
        /// <summary>Value of the custom field. The data type of the value passed in corresponds with the data_type of the Custom Field Definition.For a lov_entry data_type the value passed in should be the ID of one of the Custom Field Definition&apos;s LOV Entries. For a lov_entries data_type the value passed in should be an array of IDs of the Custom Field Definition&apos;s LOV Entries.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id? CustomFieldCustomFieldDefinitionId { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id CustomFieldCustomFieldDefinitionId { get; set; }
#endif
        /// <summary>Format: YYYY-MM-DD Example: 2016-04-19</summary>
        public Date? Date { get; set; }
        /// <summary>Datetime of record. Mutually exclusive with the date property.</summary>
        public DateTimeOffset? Datetime { get; set; }
        /// <summary>Details of visit</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Details { get; set; }
#nullable restore
#else
        public string Details { get; set; }
#endif
        /// <summary>Time that the visitation ended - hour</summary>
        public int? EndHour { get; set; }
        /// <summary>Time that the visitation ended - minute</summary>
        public int? EndMinute { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log"/> and sets the default values.
        /// </summary>
        public Visitor_logsPatchRequestBody_visitor_log()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "begin_hour", n => { BeginHour = n.GetIntValue(); } },
                { "begin_minute", n => { BeginMinute = n.GetIntValue(); } },
                { "custom_field_%{custom_field_definition_id}", n => { CustomFieldCustomFieldDefinitionId = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id.CreateFromDiscriminatorValue); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "datetime", n => { Datetime = n.GetDateTimeOffsetValue(); } },
                { "details", n => { Details = n.GetStringValue(); } },
                { "end_hour", n => { EndHour = n.GetIntValue(); } },
                { "end_minute", n => { EndMinute = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("begin_hour", BeginHour);
            writer.WriteIntValue("begin_minute", BeginMinute);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id>("custom_field_%{custom_field_definition_id}", CustomFieldCustomFieldDefinitionId);
            writer.WriteDateValue("date", Date);
            writer.WriteDateTimeOffsetValue("datetime", Datetime);
            writer.WriteStringValue("details", Details);
            writer.WriteIntValue("end_hour", EndHour);
            writer.WriteIntValue("end_minute", EndMinute);
            writer.WriteAdditionalData(AdditionalData);
        }
        /// <summary>
        /// Composed type wrapper for classes <see cref="bool"/>, <see cref="double"/>, <see cref="string"/>, List&lt;int&gt;
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id : IComposedTypeWrapper, IParsable
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
            /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id"/></returns>
            /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
            public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id CreateFromDiscriminatorValue(IParseNode parseNode)
            {
                _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
                var mappingValue = parseNode.GetChildNode("")?.GetStringValue();
                var result = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Visitor_logs.Item.Visitor_logsPatchRequestBody_visitor_log.Visitor_logsPatchRequestBody_visitor_log_custom_field_Custom_field_definition_id();
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
