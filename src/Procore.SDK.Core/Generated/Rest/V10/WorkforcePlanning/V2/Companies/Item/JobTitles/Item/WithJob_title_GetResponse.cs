// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item
{
    /// <summary>
    /// Schema representing a single Job Title.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithJob_title_GetResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Hexadecimal color code for the Job Title. Helps with categorization and visual distinction.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Color { get; set; }
#nullable restore
#else
        public string Color { get; set; }
#endif
        /// <summary>Controls whether the Job Title is globally available to all current and future Groups.</summary>
        public bool? GloballyAccessible { get; set; }
        /// <summary>List of Group UUIDs where this Job Title is available.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<Guid?>? GroupIds { get; set; }
#nullable restore
#else
        public List<Guid?> GroupIds { get; set; }
#endif
        /// <summary>Hourly wage rate for the Job Title. Required if type is `hourly`.</summary>
        public double? HourlyRate { get; set; }
        /// <summary>Unique identifier for the Job Title.</summary>
        public Guid? Id { get; set; }
        /// <summary>Name of the Job Title.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Specifies the Job Title type. - `hourly` - Hourly wage-based job title. - `salaried` - Fixed salary job title.</summary>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse_type? Type { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse"/> and sets the default values.
        /// </summary>
        public WithJob_title_GetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "color", n => { Color = n.GetStringValue(); } },
                { "globally_accessible", n => { GloballyAccessible = n.GetBoolValue(); } },
                { "group_ids", n => { GroupIds = n.GetCollectionOfPrimitiveValues<Guid?>()?.AsList(); } },
                { "hourly_rate", n => { HourlyRate = n.GetDoubleValue(); } },
                { "id", n => { Id = n.GetGuidValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "type", n => { Type = n.GetEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse_type>(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("color", Color);
            writer.WriteBoolValue("globally_accessible", GloballyAccessible);
            writer.WriteCollectionOfPrimitiveValues<Guid?>("group_ids", GroupIds);
            writer.WriteDoubleValue("hourly_rate", HourlyRate);
            writer.WriteGuidValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.JobTitles.Item.WithJob_title_GetResponse_type>("type", Type);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
