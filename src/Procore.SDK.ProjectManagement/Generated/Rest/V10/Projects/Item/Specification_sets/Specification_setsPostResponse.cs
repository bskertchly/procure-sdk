// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Specification_sets
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Specification_setsPostResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The company_id property</summary>
        public int? CompanyId { get; set; }
        /// <summary>The created_at property</summary>
        public Date? CreatedAt { get; set; }
        /// <summary>The date property</summary>
        public Date? Date { get; set; }
        /// <summary>The deleted_at property</summary>
        public Date? DeletedAt { get; set; }
        /// <summary>The id property</summary>
        public int? Id { get; set; }
        /// <summary>The name property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The position property</summary>
        public int? Position { get; set; }
        /// <summary>The project_id property</summary>
        public int? ProjectId { get; set; }
        /// <summary>The updated_at property</summary>
        public Date? UpdatedAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Specification_sets.Specification_setsPostResponse"/> and sets the default values.
        /// </summary>
        public Specification_setsPostResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Specification_sets.Specification_setsPostResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Specification_sets.Specification_setsPostResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Specification_sets.Specification_setsPostResponse();
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
                { "created_at", n => { CreatedAt = n.GetDateValue(); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "deleted_at", n => { DeletedAt = n.GetDateValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "position", n => { Position = n.GetIntValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
                { "updated_at", n => { UpdatedAt = n.GetDateValue(); } },
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
            writer.WriteDateValue("created_at", CreatedAt);
            writer.WriteDateValue("date", Date);
            writer.WriteDateValue("deleted_at", DeletedAt);
            writer.WriteIntValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteIntValue("position", Position);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteDateValue("updated_at", UpdatedAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
