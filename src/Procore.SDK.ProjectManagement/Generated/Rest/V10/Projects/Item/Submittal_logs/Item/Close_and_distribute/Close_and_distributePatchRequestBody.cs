// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Close_and_distributePatchRequestBody : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The message property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Message { get; set; }
#nullable restore
#else
        public string Message { get; set; }
#endif
        /// <summary>The prostore_file_ids property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? ProstoreFileIds { get; set; }
#nullable restore
#else
        public List<int?> ProstoreFileIds { get; set; }
#endif
        /// <summary>The recipient_ids property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? RecipientIds { get; set; }
#nullable restore
#else
        public List<int?> RecipientIds { get; set; }
#endif
        /// <summary>The selected_approvers property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody_selected_approvers>? SelectedApprovers { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody_selected_approvers> SelectedApprovers { get; set; }
#endif
        /// <summary>The submittal_description property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SubmittalDescription { get; set; }
#nullable restore
#else
        public string SubmittalDescription { get; set; }
#endif
        /// <summary>The submittal_log_status_id property</summary>
        public int? SubmittalLogStatusId { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody"/> and sets the default values.
        /// </summary>
        public Close_and_distributePatchRequestBody()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "message", n => { Message = n.GetStringValue(); } },
                { "prostore_file_ids", n => { ProstoreFileIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "recipient_ids", n => { RecipientIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "selected_approvers", n => { SelectedApprovers = n.GetCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody_selected_approvers>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody_selected_approvers.CreateFromDiscriminatorValue)?.AsList(); } },
                { "submittal_description", n => { SubmittalDescription = n.GetStringValue(); } },
                { "submittal_log_status_id", n => { SubmittalLogStatusId = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("message", Message);
            writer.WriteCollectionOfPrimitiveValues<int?>("prostore_file_ids", ProstoreFileIds);
            writer.WriteCollectionOfPrimitiveValues<int?>("recipient_ids", RecipientIds);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Submittal_logs.Item.Close_and_distribute.Close_and_distributePatchRequestBody_selected_approvers>("selected_approvers", SelectedApprovers);
            writer.WriteStringValue("submittal_description", SubmittalDescription);
            writer.WriteIntValue("submittal_log_status_id", SubmittalLogStatusId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
