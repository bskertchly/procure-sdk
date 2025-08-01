// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests
{
    /// <summary>
    /// A paginated collection of Resource Requests.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ResourceRequestsGetResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The current page index (0-based).</summary>
        public int? CurrentPage { get; set; }
        /// <summary>The list of Resource Requests on the current page.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse_data>? Data { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse_data> Data { get; set; }
#endif
        /// <summary>The total number of pages available for pagination.</summary>
        public int? PossiblePages { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse"/> and sets the default values.
        /// </summary>
        public ResourceRequestsGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "current_page", n => { CurrentPage = n.GetIntValue(); } },
                { "data", n => { Data = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse_data>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse_data.CreateFromDiscriminatorValue)?.AsList(); } },
                { "possible_pages", n => { PossiblePages = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("current_page", CurrentPage);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.ResourceRequests.ResourceRequestsGetResponse_data>("data", Data);
            writer.WriteIntValue("possible_pages", PossiblePages);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
