// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups
{
    /// <summary>
    /// Response schema for a single Group.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Groups : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The first part of the Group&apos;s address.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Address1 { get; set; }
#nullable restore
#else
        public string Address1 { get; set; }
#endif
        /// <summary>The second part of the Group&apos;s address (e.g., Building, Suite, Unit).</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Address2 { get; set; }
#nullable restore
#else
        public string Address2 { get; set; }
#endif
        /// <summary>The city or town where the Group is located.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CityTown { get; set; }
#nullable restore
#else
        public string CityTown { get; set; }
#endif
        /// <summary>Hexadecimal color code for the Group.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Color { get; set; }
#nullable restore
#else
        public string Color { get; set; }
#endif
        /// <summary>Email address for the Group’s Point of Contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ContactEmail { get; set; }
#nullable restore
#else
        public string ContactEmail { get; set; }
#endif
        /// <summary>The primary Point of Contact (P.O.C.) for the Group.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ContactName { get; set; }
#nullable restore
#else
        public string ContactName { get; set; }
#endif
        /// <summary>Phone number for the Group’s Point of Contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ContactNumber { get; set; }
#nullable restore
#else
        public string ContactNumber { get; set; }
#endif
        /// <summary>The country where the Group is located.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Country { get; set; }
#nullable restore
#else
        public string Country { get; set; }
#endif
        /// <summary>Unique identifier for the Group.</summary>
        public Guid? Id { get; set; }
        /// <summary>The name of the Group.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The state or province where the Group is located.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? StateProvince { get; set; }
#nullable restore
#else
        public string StateProvince { get; set; }
#endif
        /// <summary>The timezone associated with the Group.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Timezone { get; set; }
#nullable restore
#else
        public string Timezone { get; set; }
#endif
        /// <summary>The postal/zip code for the Group.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Zipcode { get; set; }
#nullable restore
#else
        public string Zipcode { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Groups"/> and sets the default values.
        /// </summary>
        public Groups()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Groups"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Groups CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Groups.Groups();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "address_1", n => { Address1 = n.GetStringValue(); } },
                { "address_2", n => { Address2 = n.GetStringValue(); } },
                { "city_town", n => { CityTown = n.GetStringValue(); } },
                { "color", n => { Color = n.GetStringValue(); } },
                { "contact_email", n => { ContactEmail = n.GetStringValue(); } },
                { "contact_name", n => { ContactName = n.GetStringValue(); } },
                { "contact_number", n => { ContactNumber = n.GetStringValue(); } },
                { "country", n => { Country = n.GetStringValue(); } },
                { "id", n => { Id = n.GetGuidValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "state_province", n => { StateProvince = n.GetStringValue(); } },
                { "timezone", n => { Timezone = n.GetStringValue(); } },
                { "zipcode", n => { Zipcode = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("address_1", Address1);
            writer.WriteStringValue("address_2", Address2);
            writer.WriteStringValue("city_town", CityTown);
            writer.WriteStringValue("color", Color);
            writer.WriteStringValue("contact_email", ContactEmail);
            writer.WriteStringValue("contact_name", ContactName);
            writer.WriteStringValue("contact_number", ContactNumber);
            writer.WriteStringValue("country", Country);
            writer.WriteGuidValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteStringValue("state_province", StateProvince);
            writer.WriteStringValue("timezone", Timezone);
            writer.WriteStringValue("zipcode", Zipcode);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
