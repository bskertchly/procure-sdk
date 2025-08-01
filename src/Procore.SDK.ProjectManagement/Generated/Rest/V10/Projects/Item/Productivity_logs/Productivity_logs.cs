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
    public partial class Productivity_logs : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Name of Company</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Company { get; set; }
#nullable restore
#else
        public string Company { get; set; }
#endif
        /// <summary>Approved Commitment Contract title</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Contract { get; set; }
#nullable restore
#else
        public string Contract { get; set; }
#endif
        /// <summary>Created at</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_created_by CreatedBy { get; set; }
#endif
        /// <summary>Date of record</summary>
        public Date? Date { get; set; }
        /// <summary>Estimated UTC datetime of record</summary>
        public DateTimeOffset? Datetime { get; set; }
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Description of the Line Item</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LineItemDescription { get; set; }
#nullable restore
#else
        public string LineItemDescription { get; set; }
#endif
        /// <summary>Object that the Line Item belongs to (WorkOrderContract, PurchaseOrderContract, PotentialChangeOrder)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_line_item_holder? LineItemHolder { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_line_item_holder LineItemHolder { get; set; }
#endif
        /// <summary>ID of the Line Item from the approved Commitment Contract</summary>
        public int? LineItemId { get; set; }
        /// <summary>The location property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_location? Location { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_location Location { get; set; }
#endif
        /// <summary>Additional notes</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Notes { get; set; }
#nullable restore
#else
        public string Notes { get; set; }
#endif
        /// <summary>Order in which this entry was recorded</summary>
        public int? Position { get; set; }
        /// <summary>Number of materials that were previously delivered on site</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PreviouslyDelivered { get; set; }
#nullable restore
#else
        public string PreviouslyDelivered { get; set; }
#endif
        /// <summary>Number of materials previously put in place on site</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PreviouslyUsed { get; set; }
#nullable restore
#else
        public string PreviouslyUsed { get; set; }
#endif
        /// <summary>Number of materials delivered on site</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? QuantityDelivered { get; set; }
#nullable restore
#else
        public string QuantityDelivered { get; set; }
#endif
        /// <summary>Number of materials put in place on site</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? QuantityUsed { get; set; }
#nullable restore
#else
        public string QuantityUsed { get; set; }
#endif
        /// <summary>Updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs"/> and sets the default values.
        /// </summary>
        public Productivity_logs()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "company", n => { Company = n.GetStringValue(); } },
                { "contract", n => { Contract = n.GetStringValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_created_by>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_created_by.CreateFromDiscriminatorValue); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "datetime", n => { Datetime = n.GetDateTimeOffsetValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "line_item_description", n => { LineItemDescription = n.GetStringValue(); } },
                { "line_item_holder", n => { LineItemHolder = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_line_item_holder>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_line_item_holder.CreateFromDiscriminatorValue); } },
                { "line_item_id", n => { LineItemId = n.GetIntValue(); } },
                { "location", n => { Location = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_location>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_location.CreateFromDiscriminatorValue); } },
                { "notes", n => { Notes = n.GetStringValue(); } },
                { "position", n => { Position = n.GetIntValue(); } },
                { "previously_delivered", n => { PreviouslyDelivered = n.GetStringValue(); } },
                { "previously_used", n => { PreviouslyUsed = n.GetStringValue(); } },
                { "quantity_delivered", n => { QuantityDelivered = n.GetStringValue(); } },
                { "quantity_used", n => { QuantityUsed = n.GetStringValue(); } },
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
            writer.WriteStringValue("company", Company);
            writer.WriteStringValue("contract", Contract);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_created_by>("created_by", CreatedBy);
            writer.WriteDateValue("date", Date);
            writer.WriteDateTimeOffsetValue("datetime", Datetime);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("line_item_description", LineItemDescription);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_line_item_holder>("line_item_holder", LineItemHolder);
            writer.WriteIntValue("line_item_id", LineItemId);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Productivity_logs.Productivity_logs_location>("location", Location);
            writer.WriteStringValue("notes", Notes);
            writer.WriteIntValue("position", Position);
            writer.WriteStringValue("previously_delivered", PreviouslyDelivered);
            writer.WriteStringValue("previously_used", PreviouslyUsed);
            writer.WriteStringValue("quantity_delivered", QuantityDelivered);
            writer.WriteStringValue("quantity_used", QuantityUsed);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
