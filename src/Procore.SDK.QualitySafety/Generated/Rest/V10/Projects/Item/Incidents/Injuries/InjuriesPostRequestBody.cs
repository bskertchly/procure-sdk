// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class InjuriesPostRequestBody : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The injury property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody_injury? Injury { get; set; }
#nullable restore
#else
        public global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody_injury Injury { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody"/> and sets the default values.
        /// </summary>
        public InjuriesPostRequestBody()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "injury", n => { Injury = n.GetObjectValue<global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody_injury>(global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody_injury.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Incidents.Injuries.InjuriesPostRequestBody_injury>("injury", Injury);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
