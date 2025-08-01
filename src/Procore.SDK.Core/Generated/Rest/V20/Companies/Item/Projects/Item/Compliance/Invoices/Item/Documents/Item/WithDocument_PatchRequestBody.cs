// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.Item
{
    /// <summary>
    /// Compliance document update parameters
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithDocument_PatchRequestBody : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Effective date of the compliance document</summary>
        public DateTimeOffset? EffectiveAt { get; set; }
        /// <summary>Expiration date of the compliance document</summary>
        public DateTimeOffset? ExpiresAt { get; set; }
        /// <summary>Array of Procore file IDs</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? ProstoreFileIds { get; set; }
#nullable restore
#else
        public List<string> ProstoreFileIds { get; set; }
#endif
        /// <summary>Notes from the reviewer</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ReviewerNotes { get; set; }
#nullable restore
#else
        public string ReviewerNotes { get; set; }
#endif
        /// <summary>Status of the compliance document</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Status { get; set; }
#nullable restore
#else
        public string Status { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.Item.WithDocument_PatchRequestBody"/> and sets the default values.
        /// </summary>
        public WithDocument_PatchRequestBody()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.Item.WithDocument_PatchRequestBody"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.Item.WithDocument_PatchRequestBody CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Compliance.Invoices.Item.Documents.Item.WithDocument_PatchRequestBody();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "effective_at", n => { EffectiveAt = n.GetDateTimeOffsetValue(); } },
                { "expires_at", n => { ExpiresAt = n.GetDateTimeOffsetValue(); } },
                { "prostore_file_ids", n => { ProstoreFileIds = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "reviewer_notes", n => { ReviewerNotes = n.GetStringValue(); } },
                { "status", n => { Status = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteDateTimeOffsetValue("effective_at", EffectiveAt);
            writer.WriteDateTimeOffsetValue("expires_at", ExpiresAt);
            writer.WriteCollectionOfPrimitiveValues<string>("prostore_file_ids", ProstoreFileIds);
            writer.WriteStringValue("reviewer_notes", ReviewerNotes);
            writer.WriteStringValue("status", Status);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
