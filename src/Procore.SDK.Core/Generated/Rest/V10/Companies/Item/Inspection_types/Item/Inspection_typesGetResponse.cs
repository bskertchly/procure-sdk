// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Inspection_types.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Inspection_typesGetResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Timestamp of audit</summary>
        public DateTimeOffset? AuditTransactionTimestamp { get; set; }
        /// <summary>Company ID</summary>
        public int? CompanyId { get; set; }
        /// <summary>Timestamp of creation</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>Timestamp of deletion</summary>
        public DateTimeOffset? DeletedAt { get; set; }
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Is deletable</summary>
        public bool? IsDeletable { get; set; }
        /// <summary>Name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The source_id property</summary>
        public int? SourceId { get; set; }
        /// <summary>Timestamp of last update</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Inspection_types.Item.Inspection_typesGetResponse"/> and sets the default values.
        /// </summary>
        public Inspection_typesGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Inspection_types.Item.Inspection_typesGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Inspection_types.Item.Inspection_typesGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Inspection_types.Item.Inspection_typesGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "audit_transaction_timestamp", n => { AuditTransactionTimestamp = n.GetDateTimeOffsetValue(); } },
                { "company_id", n => { CompanyId = n.GetIntValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "deleted_at", n => { DeletedAt = n.GetDateTimeOffsetValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "is_deletable", n => { IsDeletable = n.GetBoolValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "source_id", n => { SourceId = n.GetIntValue(); } },
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
            writer.WriteDateTimeOffsetValue("audit_transaction_timestamp", AuditTransactionTimestamp);
            writer.WriteIntValue("company_id", CompanyId);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteDateTimeOffsetValue("deleted_at", DeletedAt);
            writer.WriteIntValue("id", Id);
            writer.WriteBoolValue("is_deletable", IsDeletable);
            writer.WriteStringValue("name", Name);
            writer.WriteIntValue("source_id", SourceId);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
