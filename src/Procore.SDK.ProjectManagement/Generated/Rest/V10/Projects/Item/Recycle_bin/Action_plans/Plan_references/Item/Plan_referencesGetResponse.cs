// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item
{
    /// <summary>
    /// Action Plan Reference (Show)
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Plan_referencesGetResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Timestamp of creation</summary>
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>Timestamp of deletion</summary>
        public DateTimeOffset? DeletedAt { get; set; }
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Contains specific attributes depending on the type of Reference</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_payload? Payload { get; set; }
#nullable restore
#else
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_payload Payload { get; set; }
#endif
        /// <summary>Action Plan ID</summary>
        public int? PlanId { get; set; }
        /// <summary>Action Plan Item ID</summary>
        public int? PlanItemId { get; set; }
        /// <summary>Action Plan Reference Type</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_type? Type { get; set; }
        /// <summary>Timestamp of last update</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse"/> and sets the default values.
        /// </summary>
        public Plan_referencesGetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "created_at", n => { CreatedAt = n.GetDateTimeOffsetValue(); } },
                { "deleted_at", n => { DeletedAt = n.GetDateTimeOffsetValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "payload", n => { Payload = n.GetObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_payload>(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_payload.CreateFromDiscriminatorValue); } },
                { "plan_id", n => { PlanId = n.GetIntValue(); } },
                { "plan_item_id", n => { PlanItemId = n.GetIntValue(); } },
                { "type", n => { Type = n.GetEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_type>(); } },
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
            writer.WriteDateTimeOffsetValue("created_at", CreatedAt);
            writer.WriteDateTimeOffsetValue("deleted_at", DeletedAt);
            writer.WriteIntValue("id", Id);
            writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_payload>("payload", Payload);
            writer.WriteIntValue("plan_id", PlanId);
            writer.WriteIntValue("plan_item_id", PlanItemId);
            writer.WriteEnumValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Action_plans.Plan_references.Item.Plan_referencesGetResponse_type>("type", Type);
            writer.WriteDateTimeOffsetValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
