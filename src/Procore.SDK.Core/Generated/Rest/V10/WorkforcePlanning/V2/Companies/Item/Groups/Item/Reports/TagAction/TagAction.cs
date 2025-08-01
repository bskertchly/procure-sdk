// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction
{
    /// <summary>
    /// Object representing a Person with a Tag requiring action.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class TagAction : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The employee number of the Person.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? EmployeeNumber { get; set; }
#nullable restore
#else
        public string EmployeeNumber { get; set; }
#endif
        /// <summary>The Job Title of the Person.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? JobTitle { get; set; }
#nullable restore
#else
        public string JobTitle { get; set; }
#endif
        /// <summary>The person_name property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_person_name? PersonName { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_person_name PersonName { get; set; }
#endif
        /// <summary>Details about the Tag requiring action.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_tag? Tag { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_tag Tag { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction"/> and sets the default values.
        /// </summary>
        public TagAction()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "employee_number", n => { EmployeeNumber = n.GetStringValue(); } },
                { "job_title", n => { JobTitle = n.GetStringValue(); } },
                { "person_name", n => { PersonName = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_person_name>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_person_name.CreateFromDiscriminatorValue); } },
                { "tag", n => { Tag = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_tag>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_tag.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("employee_number", EmployeeNumber);
            writer.WriteStringValue("job_title", JobTitle);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_person_name>("person_name", PersonName);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Item.Reports.TagAction.TagAction_tag>("tag", Tag);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
