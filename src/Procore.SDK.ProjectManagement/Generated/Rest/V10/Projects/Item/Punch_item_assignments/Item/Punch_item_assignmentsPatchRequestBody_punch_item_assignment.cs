// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item
{
    /// <summary>
    /// Punch Item Assignment object
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Punch_item_assignmentsPatchRequestBody_punch_item_assignment : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Resolution status</summary>
        public bool? Approved { get; set; }
        /// <summary>Comment</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Comment { get; set; }
#nullable restore
#else
        public string Comment { get; set; }
#endif
        /// <summary>User ID</summary>
        public int? LoginInformationId { get; set; }
        /// <summary>Punch Item Assignment Status</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment_status? Status { get; set; }
        /// <summary>Uploads to attach to the response</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? UploadIds { get; set; }
#nullable restore
#else
        public List<string> UploadIds { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment"/> and sets the default values.
        /// </summary>
        public Punch_item_assignmentsPatchRequestBody_punch_item_assignment()
        {
            AdditionalData = new Dictionary<string, object>();
            Status = global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment_status.Unresolved;
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "approved", n => { Approved = n.GetBoolValue(); } },
                { "comment", n => { Comment = n.GetStringValue(); } },
                { "login_information_id", n => { LoginInformationId = n.GetIntValue(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment_status>(); } },
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
            writer.WriteBoolValue("approved", Approved);
            writer.WriteStringValue("comment", Comment);
            writer.WriteIntValue("login_information_id", LoginInformationId);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_assignments.Item.Punch_item_assignmentsPatchRequestBody_punch_item_assignment_status>("status", Status);
            writer.WriteCollectionOfPrimitiveValues<string>("upload_ids", UploadIds);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
