// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item
{
    /// <summary>
    /// Request body schema for updating a Tag.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithTag_PostRequestBody : IAdditionalDataHolder, IParsable
    {
        /// <summary>A 5-character max String representing the abbreviation that will appear in most Tag views. Defaults to the first 5 characters of the name if not provided.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Abbreviation { get; set; }
#nullable restore
#else
        public string Abbreviation { get; set; }
#endif
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Array of Tag Categories this Tag should be available to, if Tag Categories are enabled.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? Categories { get; set; }
#nullable restore
#else
        public List<string> Categories { get; set; }
#endif
        /// <summary>Hexadecimal color code for the Tag, used for categorization and visual distinction.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Color { get; set; }
#nullable restore
#else
        public string Color { get; set; }
#endif
        /// <summary>Unique identifier for the company. NOTE - this is a Laborchart company ID.</summary>
        public int? CompanyId { get; set; }
        /// <summary>Number of days before expiration when the Tag should be in &quot;warning&quot; mode. Only relevant if `require_expr_date` is true.</summary>
        public int? ExprDaysWarning { get; set; }
        /// <summary>Controls whether the Tag should be globally available to all current and future Groups.</summary>
        public bool? GloballyAccessible { get; set; }
        /// <summary>Array of UUIDs for which Groups this Tag should be available to or be removed from depending on context. For adding availability, if `globally_accessible` is true, this can be an empty array.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<Guid?>? GroupIds { get; set; }
#nullable restore
#else
        public List<Guid?> GroupIds { get; set; }
#endif
        /// <summary>The Tag&apos;s name.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Controls whether the Tag should require an expiration date when applied to a Person.</summary>
        public bool? RequireExprDate { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody"/> and sets the default values.
        /// </summary>
        public WithTag_PostRequestBody()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "abbreviation", n => { Abbreviation = n.GetStringValue(); } },
                { "categories", n => { Categories = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "color", n => { Color = n.GetStringValue(); } },
                { "company_id", n => { CompanyId = n.GetIntValue(); } },
                { "expr_days_warning", n => { ExprDaysWarning = n.GetIntValue(); } },
                { "globally_accessible", n => { GloballyAccessible = n.GetBoolValue(); } },
                { "group_ids", n => { GroupIds = n.GetCollectionOfPrimitiveValues<Guid?>()?.AsList(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "require_expr_date", n => { RequireExprDate = n.GetBoolValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("abbreviation", Abbreviation);
            writer.WriteCollectionOfPrimitiveValues<string>("categories", Categories);
            writer.WriteStringValue("color", Color);
            writer.WriteIntValue("company_id", CompanyId);
            writer.WriteIntValue("expr_days_warning", ExprDaysWarning);
            writer.WriteBoolValue("globally_accessible", GloballyAccessible);
            writer.WriteCollectionOfPrimitiveValues<Guid?>("group_ids", GroupIds);
            writer.WriteStringValue("name", Name);
            writer.WriteBoolValue("require_expr_date", RequireExprDate);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
