// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Drawing_tilesGetResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Max zoom level</summary>
        public int? MaxZoomLevel { get; set; }
        /// <summary>Array of drawing tiles</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse_tiles>? Tiles { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse_tiles> Tiles { get; set; }
#endif
        /// <summary>Array of tile width and height</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? TileSize { get; set; }
#nullable restore
#else
        public List<int?> TileSize { get; set; }
#endif
        /// <summary>ZIP url</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ZipUrl { get; set; }
#nullable restore
#else
        public string ZipUrl { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse"/> and sets the default values.
        /// </summary>
        public Drawing_tilesGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "max_zoom_level", n => { MaxZoomLevel = n.GetIntValue(); } },
                { "tile_size", n => { TileSize = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "tiles", n => { Tiles = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse_tiles>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse_tiles.CreateFromDiscriminatorValue)?.AsList(); } },
                { "zip_url", n => { ZipUrl = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("max_zoom_level", MaxZoomLevel);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_tiles.Drawing_tilesGetResponse_tiles>("tiles", Tiles);
            writer.WriteCollectionOfPrimitiveValues<int?>("tile_size", TileSize);
            writer.WriteStringValue("zip_url", ZipUrl);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
