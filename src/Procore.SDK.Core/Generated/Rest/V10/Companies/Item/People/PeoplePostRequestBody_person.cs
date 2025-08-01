// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.People
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class PeoplePostRequestBody_person : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The active status of the Company Person</summary>
        public bool? Active { get; set; }
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The Employee ID of the Company Person</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? EmployeeId { get; set; }
#nullable restore
#else
        public string EmployeeId { get; set; }
#endif
        /// <summary>The First Name of the Company Person</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FirstName { get; set; }
#nullable restore
#else
        public string FirstName { get; set; }
#endif
        /// <summary>The Employee status of the Company Person</summary>
        public bool? IsEmployee { get; set; }
        /// <summary>The Last Name of the Company Person</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LastName { get; set; }
#nullable restore
#else
        public string LastName { get; set; }
#endif
        /// <summary>The Origin ID of the Company User</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? OriginId { get; set; }
#nullable restore
#else
        public string OriginId { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.People.PeoplePostRequestBody_person"/> and sets the default values.
        /// </summary>
        public PeoplePostRequestBody_person()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.People.PeoplePostRequestBody_person"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.People.PeoplePostRequestBody_person CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.People.PeoplePostRequestBody_person();
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
                { "employee_id", n => { EmployeeId = n.GetStringValue(); } },
                { "first_name", n => { FirstName = n.GetStringValue(); } },
                { "is_employee", n => { IsEmployee = n.GetBoolValue(); } },
                { "last_name", n => { LastName = n.GetStringValue(); } },
                { "origin_id", n => { OriginId = n.GetStringValue(); } },
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
            writer.WriteStringValue("employee_id", EmployeeId);
            writer.WriteStringValue("first_name", FirstName);
            writer.WriteBoolValue("is_employee", IsEmployee);
            writer.WriteStringValue("last_name", LastName);
            writer.WriteStringValue("origin_id", OriginId);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
