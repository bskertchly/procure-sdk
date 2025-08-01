// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.Response_logs
{
    /// <summary>
    /// Response Log body
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Response_logsPostRequestBody_response_log : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The Comments of the Response Log</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Comment { get; set; }
#nullable restore
#else
        public string Comment { get; set; }
#endif
        /// <summary>PDM document revision IDs to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? DocumentManagementDocumentRevisionIds { get; set; }
#nullable restore
#else
        public List<string> DocumentManagementDocumentRevisionIds { get; set; }
#endif
        /// <summary>An array of the Attachment IDs</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? ProstoreFileIds { get; set; }
#nullable restore
#else
        public List<int?> ProstoreFileIds { get; set; }
#endif
        /// <summary>An array of the Attachment Upload IDs</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? UploadIds { get; set; }
#nullable restore
#else
        public List<string> UploadIds { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.Response_logs.Response_logsPostRequestBody_response_log"/> and sets the default values.
        /// </summary>
        public Response_logsPostRequestBody_response_log()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.Response_logs.Response_logsPostRequestBody_response_log"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.Response_logs.Response_logsPostRequestBody_response_log CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Item.Response_logs.Response_logsPostRequestBody_response_log();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "comment", n => { Comment = n.GetStringValue(); } },
                { "document_management_document_revision_ids", n => { DocumentManagementDocumentRevisionIds = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "prostore_file_ids", n => { ProstoreFileIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "upload_ids", n => { UploadIds = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("comment", Comment);
            writer.WriteCollectionOfPrimitiveValues<string>("document_management_document_revision_ids", DocumentManagementDocumentRevisionIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("prostore_file_ids", ProstoreFileIds);
            writer.WriteCollectionOfPrimitiveValues<string>("upload_ids", UploadIds);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
