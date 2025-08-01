// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.ResourceRequests
{
    /// <summary>
    /// Object defining working days.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ResourceRequestsPostResponse_work_days : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The Five property</summary>
        public bool? Five { get; set; }
        /// <summary>The Four property</summary>
        public bool? Four { get; set; }
        /// <summary>The One property</summary>
        public bool? One { get; set; }
        /// <summary>The Six property</summary>
        public bool? Six { get; set; }
        /// <summary>The Three property</summary>
        public bool? Three { get; set; }
        /// <summary>The Two property</summary>
        public bool? Two { get; set; }
        /// <summary>The Zero property</summary>
        public bool? Zero { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.ResourceRequests.ResourceRequestsPostResponse_work_days"/> and sets the default values.
        /// </summary>
        public ResourceRequestsPostResponse_work_days()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.ResourceRequests.ResourceRequestsPostResponse_work_days"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.ResourceRequests.ResourceRequestsPostResponse_work_days CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.ResourceRequests.ResourceRequestsPostResponse_work_days();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "5", n => { Five = n.GetBoolValue(); } },
                { "4", n => { Four = n.GetBoolValue(); } },
                { "1", n => { One = n.GetBoolValue(); } },
                { "6", n => { Six = n.GetBoolValue(); } },
                { "3", n => { Three = n.GetBoolValue(); } },
                { "2", n => { Two = n.GetBoolValue(); } },
                { "0", n => { Zero = n.GetBoolValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("5", Five);
            writer.WriteBoolValue("4", Four);
            writer.WriteBoolValue("1", One);
            writer.WriteBoolValue("6", Six);
            writer.WriteBoolValue("3", Three);
            writer.WriteBoolValue("2", Two);
            writer.WriteBoolValue("0", Zero);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
