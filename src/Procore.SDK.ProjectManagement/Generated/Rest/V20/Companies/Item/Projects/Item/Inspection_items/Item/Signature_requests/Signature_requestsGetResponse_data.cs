// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Signature_requestsGetResponse_data : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>ID</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Id { get; set; }
#nullable restore
#else
        public string Id { get; set; }
#endif
        /// <summary>Party ID of the user who requested the signature</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? RequestedById { get; set; }
#nullable restore
#else
        public string RequestedById { get; set; }
#endif
        /// <summary>The signatory property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signatory? Signatory { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signatory Signatory { get; set; }
#endif
        /// <summary>The signature property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signature? Signature { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signature Signature { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data"/> and sets the default values.
        /// </summary>
        public Signature_requestsGetResponse_data()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "id", n => { Id = n.GetStringValue(); } },
                { "requested_by_id", n => { RequestedById = n.GetStringValue(); } },
                { "signatory", n => { Signatory = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signatory>(global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signatory.CreateFromDiscriminatorValue); } },
                { "signature", n => { Signature = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signature>(global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signature.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("id", Id);
            writer.WriteStringValue("requested_by_id", RequestedById);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signatory>("signatory", Signatory);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Inspection_items.Item.Signature_requests.Signature_requestsGetResponse_data_signature>("signature", Signature);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
