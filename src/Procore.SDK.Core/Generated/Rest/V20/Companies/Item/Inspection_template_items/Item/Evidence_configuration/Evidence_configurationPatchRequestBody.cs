// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Evidence_configurationPatchRequestBody : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The evidence_configuration property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody_evidence_configuration? EvidenceConfiguration { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody_evidence_configuration EvidenceConfiguration { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody"/> and sets the default values.
        /// </summary>
        public Evidence_configurationPatchRequestBody()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "evidence_configuration", n => { EvidenceConfiguration = n.GetObjectValue<global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody_evidence_configuration>(global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody_evidence_configuration.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Item.Evidence_configuration.Evidence_configurationPatchRequestBody_evidence_configuration>("evidence_configuration", EvidenceConfiguration);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
