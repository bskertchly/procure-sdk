// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types
{
    /// <summary>
    /// Equipment Category
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Managed_equipment_typesPostResponse_managed_equipment_category : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Company ID</summary>
        public int? CompanyId { get; set; }
        /// <summary>Date the equipment category was created</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category_created_by CreatedBy { get; set; }
#endif
        /// <summary>Date the equipment category was deleted</summary>
        public DateTimeOffset? DeletedAt { get; set; }
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>If the category is currently active</summary>
        public bool? IsActive { get; set; }
        /// <summary>Name of the equipment category</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Date the equipment category was updated</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category"/> and sets the default values.
        /// </summary>
        public Managed_equipment_typesPostResponse_managed_equipment_category()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "company_id", n => { CompanyId = n.GetIntValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category_created_by>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category_created_by.CreateFromDiscriminatorValue); } },
                { "deleted_at", n => { DeletedAt = n.GetDateTimeOffsetValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "is_active", n => { IsActive = n.GetBoolValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
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
            writer.WriteIntValue("company_id", CompanyId);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Managed_equipment_types.Managed_equipment_typesPostResponse_managed_equipment_category_created_by>("created_by", CreatedBy);
            writer.WriteDateTimeOffsetValue("deleted_at", DeletedAt);
            writer.WriteIntValue("id", Id);
            writer.WriteBoolValue("is_active", IsActive);
            writer.WriteStringValue("name", Name);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
