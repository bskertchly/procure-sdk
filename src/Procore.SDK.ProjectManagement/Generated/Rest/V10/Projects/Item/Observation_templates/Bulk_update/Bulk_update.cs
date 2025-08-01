// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Bulk_update : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Flag denoting if the Observation Template is available for use</summary>
        public bool? Active { get; set; }
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The assignee property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_assignee? Assignee { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_assignee Assignee { get; set; }
#endif
        /// <summary>Company Observation Template that the Project Observation Template was created from</summary>
        public int? CompanyObservationTemplateId { get; set; }
        /// <summary>Project Observation Template ID</summary>
        public int? Id { get; set; }
        /// <summary>Title to be used for Observations created from this template</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ObservationTitle { get; set; }
#nullable restore
#else
        public string ObservationTitle { get; set; }
#endif
        /// <summary>The observation_type property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_observation_type? ObservationType { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_observation_type ObservationType { get; set; }
#endif
        /// <summary>The trade property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_trade? Trade { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_trade Trade { get; set; }
#endif
        /// <summary>Date updated</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update"/> and sets the default values.
        /// </summary>
        public Bulk_update()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "active", n => { Active = n.GetBoolValue(); } },
                { "assignee", n => { Assignee = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_assignee>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_assignee.CreateFromDiscriminatorValue); } },
                { "company_observation_template_id", n => { CompanyObservationTemplateId = n.GetIntValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "observation_title", n => { ObservationTitle = n.GetStringValue(); } },
                { "observation_type", n => { ObservationType = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_observation_type>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_observation_type.CreateFromDiscriminatorValue); } },
                { "trade", n => { Trade = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_trade>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_trade.CreateFromDiscriminatorValue); } },
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
            writer.WriteBoolValue("active", Active);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_assignee>("assignee", Assignee);
            writer.WriteIntValue("company_observation_template_id", CompanyObservationTemplateId);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("observation_title", ObservationTitle);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_observation_type>("observation_type", ObservationType);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Observation_templates.Bulk_update.Bulk_update_trade>("trade", Trade);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
