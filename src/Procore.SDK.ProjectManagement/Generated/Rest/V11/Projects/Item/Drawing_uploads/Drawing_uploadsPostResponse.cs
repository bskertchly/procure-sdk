// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads
{
    /// <summary>
    /// Drawing Upload
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Drawing_uploadsPostResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Unique identifier for the company.</summary>
        public int? CompanyId { get; set; }
        /// <summary>Drawing Upload created at</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>ID of creator</summary>
        public int? CreatedById { get; set; }
        /// <summary>Deletion in progress status</summary>
        public bool? DeletionInProgress { get; set; }
        /// <summary>Drawing Area ID</summary>
        public int? DrawingAreaId { get; set; }
        /// <summary>Drawing number contains revision</summary>
        public bool? DrawingNumberContainsRevision { get; set; }
        /// <summary>Error email sent status</summary>
        public bool? ErrorEmailSent { get; set; }
        /// <summary>Get info from filename</summary>
        public bool? GetInfoFromFilename { get; set; }
        /// <summary>Drawing Upload ID</summary>
        public int? Id { get; set; }
        /// <summary>Language for OCR</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Language { get; set; }
#nullable restore
#else
        public string Language { get; set; }
#endif
        /// <summary>Notify on success status</summary>
        public bool? NotifyOnSuccess { get; set; }
        /// <summary>Pre adaptive complete status</summary>
        public bool? PreAdaptiveComplete { get; set; }
        /// <summary>Unique identifier for the project.</summary>
        public int? ProjectId { get; set; }
        /// <summary>The status property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse_status? Status { get; set; }
        /// <summary>Success email sent status</summary>
        public bool? SuccessEmailSent { get; set; }
        /// <summary>Drawing Upload updated at</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse"/> and sets the default values.
        /// </summary>
        public Drawing_uploadsPostResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "company_id", n => { CompanyId = n.GetIntValue(); } },
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "created_by_id", n => { CreatedById = n.GetIntValue(); } },
                { "deletion_in_progress", n => { DeletionInProgress = n.GetBoolValue(); } },
                { "drawing_area_id", n => { DrawingAreaId = n.GetIntValue(); } },
                { "drawing_number_contains_revision", n => { DrawingNumberContainsRevision = n.GetBoolValue(); } },
                { "error_email_sent", n => { ErrorEmailSent = n.GetBoolValue(); } },
                { "get_info_from_filename", n => { GetInfoFromFilename = n.GetBoolValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "language", n => { Language = n.GetStringValue(); } },
                { "notify_on_success", n => { NotifyOnSuccess = n.GetBoolValue(); } },
                { "pre_adaptive_complete", n => { PreAdaptiveComplete = n.GetBoolValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse_status>(); } },
                { "success_email_sent", n => { SuccessEmailSent = n.GetBoolValue(); } },
                { "updated_at", n => { UpdatedAt = n.GetDateTimeOffsetValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("company_id", CompanyId);
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteIntValue("created_by_id", CreatedById);
            writer.WriteBoolValue("deletion_in_progress", DeletionInProgress);
            writer.WriteIntValue("drawing_area_id", DrawingAreaId);
            writer.WriteBoolValue("drawing_number_contains_revision", DrawingNumberContainsRevision);
            writer.WriteBoolValue("error_email_sent", ErrorEmailSent);
            writer.WriteBoolValue("get_info_from_filename", GetInfoFromFilename);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("language", Language);
            writer.WriteBoolValue("notify_on_success", NotifyOnSuccess);
            writer.WriteBoolValue("pre_adaptive_complete", PreAdaptiveComplete);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Drawing_uploadsPostResponse_status>("status", Status);
            writer.WriteBoolValue("success_email_sent", SuccessEmailSent);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
