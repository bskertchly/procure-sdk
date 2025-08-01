// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class TerminatePostResponse_data_responsible_group_memberships : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>List of assignees in the group</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships_assignees>? Assignees { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships_assignees> Assignees { get; set; }
#endif
        /// <summary>UUID of the responsible group</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ResponsibleGroupUuid { get; set; }
#nullable restore
#else
        public string ResponsibleGroupUuid { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships"/> and sets the default values.
        /// </summary>
        public TerminatePostResponse_data_responsible_group_memberships()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "assignees", n => { Assignees = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships_assignees>(global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships_assignees.CreateFromDiscriminatorValue)?.AsList(); } },
                { "responsible_group_uuid", n => { ResponsibleGroupUuid = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.Instances.Item.Terminate.TerminatePostResponse_data_responsible_group_memberships_assignees>("assignees", Assignees);
            writer.WriteStringValue("responsible_group_uuid", ResponsibleGroupUuid);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
