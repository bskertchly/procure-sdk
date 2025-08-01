// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item
{
    /// <summary>
    /// business register
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class VendorsGetResponse_business_register : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The id property</summary>
        public int? Id { get; set; }
        /// <summary>Identification code</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Identifier { get; set; }
#nullable restore
#else
        public string Identifier { get; set; }
#endif
        /// <summary>business register type (ABN, EIN)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Type { get; set; }
#nullable restore
#else
        public string Type { get; set; }
#endif
        /// <summary>Verification status (active, cancelled, does_not_exist)</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register_verification_status? VerificationStatus { get; set; }
        /// <summary>Verified at</summary>
        public DateTimeOffset? VerifiedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register"/> and sets the default values.
        /// </summary>
        public VendorsGetResponse_business_register()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "id", n => { Id = n.GetIntValue(); } },
                { "identifier", n => { Identifier = n.GetStringValue(); } },
                { "type", n => { Type = n.GetStringValue(); } },
                { "verification_status", n => { VerificationStatus = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register_verification_status>(); } },
                { "verified_at", n => { VerifiedAt = n.GetDateTimeOffsetValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("identifier", Identifier);
            writer.WriteStringValue("type", Type);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.VendorsGetResponse_business_register_verification_status>("verification_status", VerificationStatus);
            writer.WriteDateTimeOffsetValue("verified_at", VerifiedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
