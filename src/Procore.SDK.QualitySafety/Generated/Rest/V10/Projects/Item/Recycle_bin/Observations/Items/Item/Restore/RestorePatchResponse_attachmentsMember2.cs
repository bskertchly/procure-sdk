// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class RestorePatchResponse_attachmentsMember2 : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Attached to item ID</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? AttachedToItemId { get; set; }
#nullable restore
#else
        public string AttachedToItemId { get; set; }
#endif
        /// <summary>Attached to item type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? AttachedToItemType { get; set; }
#nullable restore
#else
        public string AttachedToItemType { get; set; }
#endif
        /// <summary>Can be viewed</summary>
        public bool? CanBeViewed { get; set; }
        /// <summary>Base name of the file without its path</summary>
        [Obsolete("")]
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Filename { get; set; }
#nullable restore
#else
        public string Filename { get; set; }
#endif
        /// <summary>File ID</summary>
        public int? Id { get; set; }
        /// <summary>Base name of the file without its path</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>URL to download the attached file. HTTP client should be prepared to follow redirects to successfully download the file.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Url { get; set; }
#nullable restore
#else
        public string Url { get; set; }
#endif
        /// <summary>Viewable</summary>
        public bool? Viewable { get; set; }
        /// <summary>Unified viewer link</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ViewerUrl { get; set; }
#nullable restore
#else
        public string ViewerUrl { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore.RestorePatchResponse_attachmentsMember2"/> and sets the default values.
        /// </summary>
        public RestorePatchResponse_attachmentsMember2()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore.RestorePatchResponse_attachmentsMember2"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore.RestorePatchResponse_attachmentsMember2 CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore.RestorePatchResponse_attachmentsMember2();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "attached_to_item_id", n => { AttachedToItemId = n.GetStringValue(); } },
                { "attached_to_item_type", n => { AttachedToItemType = n.GetStringValue(); } },
                { "can_be_viewed", n => { CanBeViewed = n.GetBoolValue(); } },
                { "filename", n => { Filename = n.GetStringValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "url", n => { Url = n.GetStringValue(); } },
                { "viewable", n => { Viewable = n.GetBoolValue(); } },
                { "viewer_url", n => { ViewerUrl = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("attached_to_item_id", AttachedToItemId);
            writer.WriteStringValue("attached_to_item_type", AttachedToItemType);
            writer.WriteBoolValue("can_be_viewed", CanBeViewed);
            writer.WriteStringValue("filename", Filename);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("url", Url);
            writer.WriteBoolValue("viewable", Viewable);
            writer.WriteStringValue("viewer_url", ViewerUrl);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
