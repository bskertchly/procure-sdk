// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Monitoring_resourcesPatchRequestBody_monitoring_resource : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Description</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>End Date, expressed in ISO 8601 date format (YYYY-MM-DD)</summary>
        public Date? EndDate { get; set; }
        /// <summary>Start Date, expressed in ISO 8601 date format (YYYY-MM-DD)</summary>
        public Date? StartDate { get; set; }
        /// <summary>Unit Cost</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? UnitCost { get; set; }
#nullable restore
#else
        public string UnitCost { get; set; }
#endif
        /// <summary>Unit of Measure</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource_unit_of_measure? UnitOfMeasure { get; set; }
        /// <summary>Utilization, expressed as a decimal where 1.0 is 100%</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Utilization { get; set; }
#nullable restore
#else
        public string Utilization { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource"/> and sets the default values.
        /// </summary>
        public Monitoring_resourcesPatchRequestBody_monitoring_resource()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "description", n => { Description = n.GetStringValue(); } },
                { "end_date", n => { EndDate = n.GetDateValue(); } },
                { "start_date", n => { StartDate = n.GetDateValue(); } },
                { "unit_cost", n => { UnitCost = n.GetStringValue(); } },
                { "unit_of_measure", n => { UnitOfMeasure = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource_unit_of_measure>(); } },
                { "utilization", n => { Utilization = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("description", Description);
            writer.WriteDateValue("end_date", EndDate);
            writer.WriteDateValue("start_date", StartDate);
            writer.WriteStringValue("unit_cost", UnitCost);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Monitoring_resources.Item.Monitoring_resourcesPatchRequestBody_monitoring_resource_unit_of_measure>("unit_of_measure", UnitOfMeasure);
            writer.WriteStringValue("utilization", Utilization);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
