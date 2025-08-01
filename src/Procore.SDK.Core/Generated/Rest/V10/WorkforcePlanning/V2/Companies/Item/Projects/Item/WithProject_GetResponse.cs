// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item
{
    /// <summary>
    /// Response schema for a single Project.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithProject_GetResponse : IAdditionalDataHolder, IParsable
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The first part of the Project&apos;s address.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Address1 { get; set; }
#nullable restore
#else
        public string Address1 { get; set; }
#endif
        /// <summary>The second part of the Project&apos;s address.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Address2 { get; set; }
#nullable restore
#else
        public string Address2 { get; set; }
#endif
        /// <summary>Project bid rate.</summary>
        public double? BidRate { get; set; }
        /// <summary>List of Categories and their Subcategories within the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_categories>? Categories { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_categories> Categories { get; set; }
#endif
        /// <summary>The city or town where the Project is located.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CityTown { get; set; }
#nullable restore
#else
        public string CityTown { get; set; }
#endif
        /// <summary>If the Project is closed, this is the closed date in Epoch time.</summary>
        public long? ClosedDate { get; set; }
        /// <summary>Hexadecimal color code for categorization.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Color { get; set; }
#nullable restore
#else
        public string Color { get; set; }
#endif
        /// <summary>The country where the Project is located.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Country { get; set; }
#nullable restore
#else
        public string Country { get; set; }
#endif
        /// <summary>Timestamp when the Project was created.</summary>
        public long? CreatedAt { get; set; }
        /// <summary>The name of the customer for the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CustomerName { get; set; }
#nullable restore
#else
        public string CustomerName { get; set; }
#endif
        /// <summary>Default end time for the Project&apos;s workday.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? DailyEndTime { get; set; }
#nullable restore
#else
        public string DailyEndTime { get; set; }
#endif
        /// <summary>Default start time for the Project&apos;s workday.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? DailyStartTime { get; set; }
#nullable restore
#else
        public string DailyStartTime { get; set; }
#endif
        /// <summary>Estimated end date for the Project in Epoch time.</summary>
        public long? EstEndDate { get; set; }
        /// <summary>List of UUIDs representing the Groups associated with the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<Guid?>? GroupIds { get; set; }
#nullable restore
#else
        public List<Guid?> GroupIds { get; set; }
#endif
        /// <summary>Unique identifier for the Project.</summary>
        public Guid? Id { get; set; }
        /// <summary>The name of the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Percentage of the Project completed.</summary>
        public double? PercentComplete { get; set; }
        /// <summary>Unique identifier for the Project used internally.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectNumber { get; set; }
#nullable restore
#else
        public string ProjectNumber { get; set; }
#endif
        /// <summary>Internal categorical classification for the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectType { get; set; }
#nullable restore
#else
        public string ProjectType { get; set; }
#endif
        /// <summary>List of people assigned to roles in the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_roles>? Roles { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_roles> Roles { get; set; }
#endif
        /// <summary>The stage of the Project .</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Stage { get; set; }
#nullable restore
#else
        public string Stage { get; set; }
#endif
        /// <summary>The ID of the project stage.</summary>
        public int? StageId { get; set; }
        /// <summary>Project&apos;s start date in Epoch time.</summary>
        public long? StartDate { get; set; }
        /// <summary>The state or province where the Project is located.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? StateProvince { get; set; }
#nullable restore
#else
        public string StateProvince { get; set; }
#endif
        /// <summary>The status of the Project, controlling visibility and filtering.</summary>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_status? Status { get; set; }
        /// <summary>List of Tags associated with the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_tag_instances>? TagInstances { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_tag_instances> TagInstances { get; set; }
#endif
        /// <summary>Timezone used for scheduling outbound messages.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Timezone { get; set; }
#nullable restore
#else
        public string Timezone { get; set; }
#endif
        /// <summary>Timestamp when the Project was last updated.</summary>
        public long? UpdatedAt { get; set; }
        /// <summary>List of Wage Overrides set for specific Job Titles on this Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_wage_overrides>? WageOverrides { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_wage_overrides> WageOverrides { get; set; }
#endif
        /// <summary>The postal/zip code of the Project.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Zipcode { get; set; }
#nullable restore
#else
        public string Zipcode { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse"/> and sets the default values.
        /// </summary>
        public WithProject_GetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse();
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
                { "bid_rate", n => { BidRate = n.GetDoubleValue(); } },
                { "categories", n => { Categories = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_categories>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_categories.CreateFromDiscriminatorValue)?.AsList(); } },
                { "city_town", n => { CityTown = n.GetStringValue(); } },
                { "closed_date", n => { ClosedDate = n.GetLongValue(); } },
                { "color", n => { Color = n.GetStringValue(); } },
                { "country", n => { Country = n.GetStringValue(); } },
                { "created_at", n => { CreatedAt = n.GetLongValue(); } },
                { "customer_name", n => { CustomerName = n.GetStringValue(); } },
                { "daily_end_time", n => { DailyEndTime = n.GetStringValue(); } },
                { "daily_start_time", n => { DailyStartTime = n.GetStringValue(); } },
                { "est_end_date", n => { EstEndDate = n.GetLongValue(); } },
                { "group_ids", n => { GroupIds = n.GetCollectionOfPrimitiveValues<Guid?>()?.AsList(); } },
                { "id", n => { Id = n.GetGuidValue(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "percent_complete", n => { PercentComplete = n.GetDoubleValue(); } },
                { "project_number", n => { ProjectNumber = n.GetStringValue(); } },
                { "project_type", n => { ProjectType = n.GetStringValue(); } },
                { "roles", n => { Roles = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_roles>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_roles.CreateFromDiscriminatorValue)?.AsList(); } },
                { "stage", n => { Stage = n.GetStringValue(); } },
                { "stage_id", n => { StageId = n.GetIntValue(); } },
                { "start_date", n => { StartDate = n.GetLongValue(); } },
                { "state_province", n => { StateProvince = n.GetStringValue(); } },
                { "status", n => { Status = n.GetEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_status>(); } },
                { "tag_instances", n => { TagInstances = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_tag_instances>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_tag_instances.CreateFromDiscriminatorValue)?.AsList(); } },
                { "timezone", n => { Timezone = n.GetStringValue(); } },
                { "updated_at", n => { UpdatedAt = n.GetLongValue(); } },
                { "wage_overrides", n => { WageOverrides = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_wage_overrides>(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_wage_overrides.CreateFromDiscriminatorValue)?.AsList(); } },
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
            writer.WriteDoubleValue("bid_rate", BidRate);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_categories>("categories", Categories);
            writer.WriteStringValue("city_town", CityTown);
            writer.WriteLongValue("closed_date", ClosedDate);
            writer.WriteStringValue("color", Color);
            writer.WriteStringValue("country", Country);
            writer.WriteLongValue("created_at", CreatedAt);
            writer.WriteStringValue("customer_name", CustomerName);
            writer.WriteStringValue("daily_end_time", DailyEndTime);
            writer.WriteStringValue("daily_start_time", DailyStartTime);
            writer.WriteLongValue("est_end_date", EstEndDate);
            writer.WriteCollectionOfPrimitiveValues<Guid?>("group_ids", GroupIds);
            writer.WriteGuidValue("id", Id);
            writer.WriteStringValue("name", Name);
            writer.WriteDoubleValue("percent_complete", PercentComplete);
            writer.WriteStringValue("project_number", ProjectNumber);
            writer.WriteStringValue("project_type", ProjectType);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_roles>("roles", Roles);
            writer.WriteStringValue("stage", Stage);
            writer.WriteIntValue("stage_id", StageId);
            writer.WriteLongValue("start_date", StartDate);
            writer.WriteStringValue("state_province", StateProvince);
            writer.WriteEnumValue<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_status>("status", Status);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_tag_instances>("tag_instances", TagInstances);
            writer.WriteStringValue("timezone", Timezone);
            writer.WriteLongValue("updated_at", UpdatedAt);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.WithProject_GetResponse_wage_overrides>("wage_overrides", WageOverrides);
            writer.WriteStringValue("zipcode", Zipcode);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
