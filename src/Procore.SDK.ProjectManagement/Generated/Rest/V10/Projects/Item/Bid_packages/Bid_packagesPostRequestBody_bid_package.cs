// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Bid_packages
{
    /// <summary>
    /// Bid Package Object
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Bid_packagesPostRequestBody_bid_package : IAdditionalDataHolder, IParsable
    {
        /// <summary>Accepts bid post due submissions</summary>
        public bool? AcceptPostDueSubmissions { get; set; }
        /// <summary>Bid package accounting method, either &apos;amount&apos; or &apos;unit&apos;</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? AccountingMethod { get; set; }
#nullable restore
#else
        public string AccountingMethod { get; set; }
#endif
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Anticipated award date</summary>
        public Date? AnticipatedAwardDate { get; set; }
        /// <summary>Due date</summary>
        public DateTimeOffset? BidDueDate { get; set; }
        /// <summary>Bid package email information details</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BidEmailMessage { get; set; }
#nullable restore
#else
        public string BidEmailMessage { get; set; }
#endif
        /// <summary>Bid Package submission confirmation text</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BidSubmissionConfirmation { get; set; }
#nullable restore
#else
        public string BidSubmissionConfirmation { get; set; }
#endif
        /// <summary>Bid package bidding instructions</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BidWebMessage { get; set; }
#nullable restore
#else
        public string BidWebMessage { get; set; }
#endif
        /// <summary>Blind bidding enabled</summary>
        public bool? BlindBidding { get; set; }
        /// <summary>Display project name</summary>
        public bool? DisplayProjectName { get; set; }
        /// <summary>Array of User IDs who will be on the bid package&apos;s distribution list</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? DistributionIds { get; set; }
#nullable restore
#else
        public List<int?> DistributionIds { get; set; }
#endif
        /// <summary>Pre-bid walkthrough enabled</summary>
        public bool? EnablePrebidWalkthrough { get; set; }
        /// <summary>Login Information ID for Manager</summary>
        public int? ManagerId { get; set; }
        /// <summary>Bid package number</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Number { get; set; }
#nullable restore
#else
        public string Number { get; set; }
#endif
        /// <summary>Scheduled pre-bid walkthrough date</summary>
        public DateTimeOffset? PreBidWalkThroughDate { get; set; }
        /// <summary>Pre-bid walkthrough notes</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PreBidWalkThroughNotes { get; set; }
#nullable restore
#else
        public string PreBidWalkThroughNotes { get; set; }
#endif
        /// <summary>Array of Procore File IDs for Non-Disclosure Agreement</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? ProstoreFileIds { get; set; }
#nullable restore
#else
        public List<int?> ProstoreFileIds { get; set; }
#endif
        /// <summary>Require Non-Disclosure Agreement</summary>
        public bool? RequireNda { get; set; }
        /// <summary>Bid package title</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Bid_packages.Bid_packagesPostRequestBody_bid_package"/> and sets the default values.
        /// </summary>
        public Bid_packagesPostRequestBody_bid_package()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Bid_packages.Bid_packagesPostRequestBody_bid_package"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Bid_packages.Bid_packagesPostRequestBody_bid_package CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Bid_packages.Bid_packagesPostRequestBody_bid_package();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "accept_post_due_submissions", n => { AcceptPostDueSubmissions = n.GetBoolValue(); } },
                { "accounting_method", n => { AccountingMethod = n.GetStringValue(); } },
                { "anticipated_award_date", n => { AnticipatedAwardDate = n.GetDateValue(); } },
                { "bid_due_date", n => { BidDueDate = n.GetDateTimeOffsetValue(); } },
                { "bid_email_message", n => { BidEmailMessage = n.GetStringValue(); } },
                { "bid_submission_confirmation", n => { BidSubmissionConfirmation = n.GetStringValue(); } },
                { "bid_web_message", n => { BidWebMessage = n.GetStringValue(); } },
                { "blind_bidding", n => { BlindBidding = n.GetBoolValue(); } },
                { "display_project_name", n => { DisplayProjectName = n.GetBoolValue(); } },
                { "distribution_ids", n => { DistributionIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "enable_prebid_walkthrough", n => { EnablePrebidWalkthrough = n.GetBoolValue(); } },
                { "manager_id", n => { ManagerId = n.GetIntValue(); } },
                { "number", n => { Number = n.GetStringValue(); } },
                { "pre_bid_walk_through_date", n => { PreBidWalkThroughDate = n.GetDateTimeOffsetValue(); } },
                { "pre_bid_walk_through_notes", n => { PreBidWalkThroughNotes = n.GetStringValue(); } },
                { "prostore_file_ids", n => { ProstoreFileIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "require_nda", n => { RequireNda = n.GetBoolValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("accept_post_due_submissions", AcceptPostDueSubmissions);
            writer.WriteStringValue("accounting_method", AccountingMethod);
            writer.WriteDateValue("anticipated_award_date", AnticipatedAwardDate);
            writer.WriteDateTimeOffsetValue("bid_due_date", BidDueDate);
            writer.WriteStringValue("bid_email_message", BidEmailMessage);
            writer.WriteStringValue("bid_submission_confirmation", BidSubmissionConfirmation);
            writer.WriteStringValue("bid_web_message", BidWebMessage);
            writer.WriteBoolValue("blind_bidding", BlindBidding);
            writer.WriteBoolValue("display_project_name", DisplayProjectName);
            writer.WriteCollectionOfPrimitiveValues<int?>("distribution_ids", DistributionIds);
            writer.WriteBoolValue("enable_prebid_walkthrough", EnablePrebidWalkthrough);
            writer.WriteIntValue("manager_id", ManagerId);
            writer.WriteStringValue("number", Number);
            writer.WriteDateTimeOffsetValue("pre_bid_walk_through_date", PreBidWalkThroughDate);
            writer.WriteStringValue("pre_bid_walk_through_notes", PreBidWalkThroughNotes);
            writer.WriteCollectionOfPrimitiveValues<int?>("prostore_file_ids", ProstoreFileIds);
            writer.WriteBoolValue("require_nda", RequireNda);
            writer.WriteStringValue("title", Title);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
