// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Productivity_logsPostRequestBody_productivity_log : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Format: YYYY-MM-DD Example: 2016-04-19</summary>
        public Date? Date { get; set; }
        /// <summary>Datetime of record. Mutually exclusive with the date property.</summary>
        public DateTimeOffset? Datetime { get; set; }
        /// <summary>Line Item ID of an approved contract</summary>
        public int? LineItemId { get; set; }
        /// <summary>The ID of the Location</summary>
        public int? LocationId { get; set; }
        /// <summary>Notes</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Notes { get; set; }
#nullable restore
#else
        public string Notes { get; set; }
#endif
        /// <summary>Total number of materials delivered</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? QuantityDelivered { get; set; }
#nullable restore
#else
        public string QuantityDelivered { get; set; }
#endif
        /// <summary>Total number of materials used</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? QuantityUsed { get; set; }
#nullable restore
#else
        public string QuantityUsed { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logsPostRequestBody_productivity_log"/> and sets the default values.
        /// </summary>
        public Productivity_logsPostRequestBody_productivity_log()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logsPostRequestBody_productivity_log"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logsPostRequestBody_productivity_log CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logsPostRequestBody_productivity_log();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "date", n => { Date = n.GetDateValue(); } },
                { "datetime", n => { Datetime = n.GetDateTimeOffsetValue(); } },
                { "line_item_id", n => { LineItemId = n.GetIntValue(); } },
                { "location_id", n => { LocationId = n.GetIntValue(); } },
                { "notes", n => { Notes = n.GetStringValue(); } },
                { "quantity_delivered", n => { QuantityDelivered = n.GetStringValue(); } },
                { "quantity_used", n => { QuantityUsed = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteDateValue("date", Date);
            writer.WriteDateTimeOffsetValue("datetime", Datetime);
            writer.WriteIntValue("line_item_id", LineItemId);
            writer.WriteIntValue("location_id", LocationId);
            writer.WriteStringValue("notes", Notes);
            writer.WriteStringValue("quantity_delivered", QuantityDelivered);
            writer.WriteStringValue("quantity_used", QuantityUsed);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
