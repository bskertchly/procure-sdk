// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions
{
    /// <summary>
    /// Drawing Revision
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Drawing_revisions : IAdditionalDataHolder, IParsable
    {
        /// <summary>Activity stream last viewed at, currently not available</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ActivityStreamLastViewedAt { get; set; }
#nullable restore
#else
        public string ActivityStreamLastViewedAt { get; set; }
#endif
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>More recent Connectability Drawing Revision available to publish</summary>
        public bool? ConnectabilityOutdated { get; set; }
        /// <summary>Current Drawing Revision</summary>
        public bool? Current { get; set; }
        /// <summary>The custom_fields property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_custom_fields? CustomFields { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_custom_fields CustomFields { get; set; }
#endif
        /// <summary>Drawing discipline</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_discipline? Discipline { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_discipline Discipline { get; set; }
#endif
        /// <summary>Drawing Area</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_area? DrawingArea { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_area DrawingArea { get; set; }
#endif
        /// <summary>Drawing date</summary>
        public Date? DrawingDate { get; set; }
        /// <summary>Drawing ID</summary>
        public int? DrawingId { get; set; }
        /// <summary>Drawing Set</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_set? DrawingSet { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_set DrawingSet { get; set; }
#endif
        /// <summary>Amount of drawing sketches</summary>
        public int? DrawingSketchesCount { get; set; }
        /// <summary>Revision floorplan status</summary>
        public bool? Floorplan { get; set; }
        /// <summary>Revision has drawing sketches status</summary>
        public bool? HasDrawingSketches { get; set; }
        /// <summary>Has public markup layer elements status</summary>
        public bool? HasPublicMarkupLayerElements { get; set; }
        /// <summary>Height</summary>
        public int? Height { get; set; }
        /// <summary>Revision ID</summary>
        public int? Id { get; set; }
        /// <summary>Large thumbnail url</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LargeThumbnailUrl { get; set; }
#nullable restore
#else
        public string LargeThumbnailUrl { get; set; }
#endif
        /// <summary>Drawing Revision number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Number { get; set; }
#nullable restore
#else
        public string Number { get; set; }
#endif
        /// <summary>Revision of an obsolete Drawing</summary>
        public bool? Obsolete { get; set; }
        /// <summary>The order_in_drawing property</summary>
        public int? OrderInDrawing { get; set; }
        /// <summary>PDF file size</summary>
        public int? PdfSize { get; set; }
        /// <summary>PDF url address</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PdfUrl { get; set; }
#nullable restore
#else
        public string PdfUrl { get; set; }
#endif
        /// <summary>PNG file size</summary>
        public int? PngSize { get; set; }
        /// <summary>PNG url address</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PngUrl { get; set; }
#nullable restore
#else
        public string PngUrl { get; set; }
#endif
        /// <summary>Drawing Revision position</summary>
        public int? Position { get; set; }
        /// <summary>Received date</summary>
        public Date? ReceivedDate { get; set; }
        /// <summary>Revision number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? RevisionNumber { get; set; }
#nullable restore
#else
        public string RevisionNumber { get; set; }
#endif
        /// <summary>The status property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Status { get; set; }
#nullable restore
#else
        public string Status { get; set; }
#endif
        /// <summary>Thumbnail url</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ThumbnailUrl { get; set; }
#nullable restore
#else
        public string ThumbnailUrl { get; set; }
#endif
        /// <summary>Title</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>Revision updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>Width</summary>
        public int? Width { get; set; }
        /// <summary>ZIP url of Drawing Tiles</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ZipUrl { get; set; }
#nullable restore
#else
        public string ZipUrl { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions"/> and sets the default values.
        /// </summary>
        public Drawing_revisions()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "activity_stream_last_viewed_at", n => { ActivityStreamLastViewedAt = n.GetStringValue(); } },
                { "connectability_outdated", n => { ConnectabilityOutdated = n.GetBoolValue(); } },
                { "current", n => { Current = n.GetBoolValue(); } },
                { "custom_fields", n => { CustomFields = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_custom_fields>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_custom_fields.CreateFromDiscriminatorValue); } },
                { "discipline", n => { Discipline = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_discipline>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_discipline.CreateFromDiscriminatorValue); } },
                { "drawing_area", n => { DrawingArea = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_area>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_area.CreateFromDiscriminatorValue); } },
                { "drawing_date", n => { DrawingDate = n.GetDateValue(); } },
                { "drawing_id", n => { DrawingId = n.GetIntValue(); } },
                { "drawing_set", n => { DrawingSet = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_set>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_set.CreateFromDiscriminatorValue); } },
                { "drawing_sketches_count", n => { DrawingSketchesCount = n.GetIntValue(); } },
                { "floorplan", n => { Floorplan = n.GetBoolValue(); } },
                { "has_drawing_sketches", n => { HasDrawingSketches = n.GetBoolValue(); } },
                { "has_public_markup_layer_elements", n => { HasPublicMarkupLayerElements = n.GetBoolValue(); } },
                { "height", n => { Height = n.GetIntValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "large_thumbnail_url", n => { LargeThumbnailUrl = n.GetStringValue(); } },
                { "number", n => { Number = n.GetStringValue(); } },
                { "obsolete", n => { Obsolete = n.GetBoolValue(); } },
                { "order_in_drawing", n => { OrderInDrawing = n.GetIntValue(); } },
                { "pdf_size", n => { PdfSize = n.GetIntValue(); } },
                { "pdf_url", n => { PdfUrl = n.GetStringValue(); } },
                { "png_size", n => { PngSize = n.GetIntValue(); } },
                { "png_url", n => { PngUrl = n.GetStringValue(); } },
                { "position", n => { Position = n.GetIntValue(); } },
                { "received_date", n => { ReceivedDate = n.GetDateValue(); } },
                { "revision_number", n => { RevisionNumber = n.GetStringValue(); } },
                { "status", n => { Status = n.GetStringValue(); } },
                { "thumbnail_url", n => { ThumbnailUrl = n.GetStringValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
                { "updated_at", n => { UpdatedAt = n.GetDateTimeOffsetValue(); } },
                { "width", n => { Width = n.GetIntValue(); } },
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
            writer.WriteStringValue("activity_stream_last_viewed_at", ActivityStreamLastViewedAt);
            writer.WriteBoolValue("connectability_outdated", ConnectabilityOutdated);
            writer.WriteBoolValue("current", Current);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_custom_fields>("custom_fields", CustomFields);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_discipline>("discipline", Discipline);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_area>("drawing_area", DrawingArea);
            writer.WriteDateValue("drawing_date", DrawingDate);
            writer.WriteIntValue("drawing_id", DrawingId);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions_drawing_set>("drawing_set", DrawingSet);
            writer.WriteIntValue("drawing_sketches_count", DrawingSketchesCount);
            writer.WriteBoolValue("floorplan", Floorplan);
            writer.WriteBoolValue("has_drawing_sketches", HasDrawingSketches);
            writer.WriteBoolValue("has_public_markup_layer_elements", HasPublicMarkupLayerElements);
            writer.WriteIntValue("height", Height);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("large_thumbnail_url", LargeThumbnailUrl);
            writer.WriteStringValue("number", Number);
            writer.WriteBoolValue("obsolete", Obsolete);
            writer.WriteIntValue("order_in_drawing", OrderInDrawing);
            writer.WriteIntValue("pdf_size", PdfSize);
            writer.WriteStringValue("pdf_url", PdfUrl);
            writer.WriteIntValue("png_size", PngSize);
            writer.WriteStringValue("png_url", PngUrl);
            writer.WriteIntValue("position", Position);
            writer.WriteDateValue("received_date", ReceivedDate);
            writer.WriteStringValue("revision_number", RevisionNumber);
            writer.WriteStringValue("status", Status);
            writer.WriteStringValue("thumbnail_url", ThumbnailUrl);
            writer.WriteStringValue("title", Title);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteIntValue("width", Width);
            writer.WriteStringValue("zip_url", ZipUrl);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
