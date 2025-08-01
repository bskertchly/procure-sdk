// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class SignaturesGetResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse_created_by CreatedBy { get; set; }
#endif
        /// <summary>File Name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FileName { get; set; }
#nullable restore
#else
        public string FileName { get; set; }
#endif
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>URL</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LargeThumbnailUrl { get; set; }
#nullable restore
#else
        public string LargeThumbnailUrl { get; set; }
#endif
        /// <summary>URL</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? MediumThumbnailUrl { get; set; }
#nullable restore
#else
        public string MediumThumbnailUrl { get; set; }
#endif
        /// <summary>Acknowedgement text the signature was signed against.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SignatureText { get; set; }
#nullable restore
#else
        public string SignatureText { get; set; }
#endif
        /// <summary>URL</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Url { get; set; }
#nullable restore
#else
        public string Url { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse"/> and sets the default values.
        /// </summary>
        public SignaturesGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse_created_by>(global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse_created_by.CreateFromDiscriminatorValue); } },
                { "file_name", n => { FileName = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "large_thumbnail_url", n => { LargeThumbnailUrl = n.GetStringValue(); } },
                { "medium_thumbnail_url", n => { MediumThumbnailUrl = n.GetStringValue(); } },
                { "signature_text", n => { SignatureText = n.GetStringValue(); } },
                { "url", n => { Url = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesGetResponse_created_by>("created_by", CreatedBy);
            writer.WriteStringValue("file_name", FileName);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("large_thumbnail_url", LargeThumbnailUrl);
            writer.WriteStringValue("medium_thumbnail_url", MediumThumbnailUrl);
            writer.WriteStringValue("signature_text", SignatureText);
            writer.WriteStringValue("url", Url);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
