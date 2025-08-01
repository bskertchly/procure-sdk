// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item
{
    /// <summary>
    /// Time and Material Entry Object
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Time_and_material_entriesPatchRequestBody_time_and_material_entry : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The ID associate with company&apos;s signature</summary>
        public int? CompanySignatureId { get; set; }
        /// <summary>The ID associate with company&apos;s signature party</summary>
        public int? CompanySigneePartyId { get; set; }
        /// <summary>The ID associate with customer&apos;s signature</summary>
        public int? CustomerSignatureId { get; set; }
        /// <summary>The ID associate with customer&apos;s signature party</summary>
        public int? CustomerSigneePartyId { get; set; }
        /// <summary>The description of job</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>Drawing Revisions to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? DrawingRevisionIds { get; set; }
#nullable restore
#else
        public List<int?> DrawingRevisionIds { get; set; }
#endif
        /// <summary>File Versions to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? FileVersionIds { get; set; }
#nullable restore
#else
        public List<int?> FileVersionIds { get; set; }
#endif
        /// <summary>Forms to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? FormIds { get; set; }
#nullable restore
#else
        public List<int?> FormIds { get; set; }
#endif
        /// <summary>Images to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? ImageIds { get; set; }
#nullable restore
#else
        public List<int?> ImageIds { get; set; }
#endif
        /// <summary>The title of T&amp;M ticket</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Unique number for the T&amp;M ticket</summary>
        public int? Number { get; set; }
        /// <summary>If the T&amp;M ticket is private</summary>
        public bool? Private { get; set; }
        /// <summary>The refrence number associate with T&amp;M ticket</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ReferenceNumber { get; set; }
#nullable restore
#else
        public string ReferenceNumber { get; set; }
#endif
        /// <summary>Current status of T&amp;M ticket</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Status { get; set; }
#nullable restore
#else
        public string Status { get; set; }
#endif
        /// <summary>The specified array of upload ids is saved as Time And Material Entry Attachments.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? UploadIds { get; set; }
#nullable restore
#else
        public List<string> UploadIds { get; set; }
#endif
        /// <summary>Date work performed on</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? WorkPerformedOnDate { get; set; }
#nullable restore
#else
        public string WorkPerformedOnDate { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesPatchRequestBody_time_and_material_entry"/> and sets the default values.
        /// </summary>
        public Time_and_material_entriesPatchRequestBody_time_and_material_entry()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesPatchRequestBody_time_and_material_entry"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesPatchRequestBody_time_and_material_entry CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesPatchRequestBody_time_and_material_entry();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "company_signature_id", n => { CompanySignatureId = n.GetIntValue(); } },
                { "company_signee_party_id", n => { CompanySigneePartyId = n.GetIntValue(); } },
                { "customer_signature_id", n => { CustomerSignatureId = n.GetIntValue(); } },
                { "customer_signee_party_id", n => { CustomerSigneePartyId = n.GetIntValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "drawing_revision_ids", n => { DrawingRevisionIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "file_version_ids", n => { FileVersionIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "form_ids", n => { FormIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "image_ids", n => { ImageIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "number", n => { Number = n.GetIntValue(); } },
                { "private", n => { Private = n.GetBoolValue(); } },
                { "reference_number", n => { ReferenceNumber = n.GetStringValue(); } },
                { "status", n => { Status = n.GetStringValue(); } },
                { "upload_ids", n => { UploadIds = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "work_performed_on_date", n => { WorkPerformedOnDate = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("company_signature_id", CompanySignatureId);
            writer.WriteIntValue("company_signee_party_id", CompanySigneePartyId);
            writer.WriteIntValue("customer_signature_id", CustomerSignatureId);
            writer.WriteIntValue("customer_signee_party_id", CustomerSigneePartyId);
            writer.WriteStringValue("description", Description);
            writer.WriteCollectionOfPrimitiveValues<int?>("drawing_revision_ids", DrawingRevisionIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("file_version_ids", FileVersionIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("form_ids", FormIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("image_ids", ImageIds);
            writer.WriteStringValue("name", Name);
            writer.WriteIntValue("number", Number);
            writer.WriteBoolValue("private", Private);
            writer.WriteStringValue("reference_number", ReferenceNumber);
            writer.WriteStringValue("status", Status);
            writer.WriteCollectionOfPrimitiveValues<string>("upload_ids", UploadIds);
            writer.WriteStringValue("work_performed_on_date", WorkPerformedOnDate);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
