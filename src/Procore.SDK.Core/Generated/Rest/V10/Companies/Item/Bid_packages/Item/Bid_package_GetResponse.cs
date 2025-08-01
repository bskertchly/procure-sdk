// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Bid_package_GetResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
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
        /// <summary>Allow lump sum bidding</summary>
        public bool? AllowBidderSum { get; set; }
        /// <summary>Anticipated award date</summary>
        public Date? AnticipatedAwardDate { get; set; }
        /// <summary>Streaming URL to download all attachments. It&apos;s an optional parameter when the require non-disclosure agreement is on.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? AttachmentsZipStreamingUrl { get; set; }
#nullable restore
#else
        public string AttachmentsZipStreamingUrl { get; set; }
#endif
        /// <summary>Days before sending countdown email</summary>
        public int? BiddingCountdownEmailDays { get; set; }
        /// <summary>The bid_docs_manifest property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_bid_docs_manifest? BidDocsManifest { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_bid_docs_manifest BidDocsManifest { get; set; }
#endif
        /// <summary>Due date</summary>
        public DateTimeOffset? BidDueDate { get; set; }
        /// <summary>Information displayed in emails for Bidders</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BidEmailMessage { get; set; }
#nullable restore
#else
        public string BidEmailMessage { get; set; }
#endif
        /// <summary>Include link to download zipped bidding documents in the Bid Invitation email</summary>
        public bool? BidEmailsIncludeLinkToBiddingDocuments { get; set; }
        /// <summary>Bid Form Sections Enabled</summary>
        public bool? BidFormSectionsEnabled { get; set; }
        /// <summary>Bid Package submission confirmation text</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BidSubmissionConfirmation { get; set; }
#nullable restore
#else
        public string BidSubmissionConfirmation { get; set; }
#endif
        /// <summary>Bid Package instructions for Bidder</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BidWebMessage { get; set; }
#nullable restore
#else
        public string BidWebMessage { get; set; }
#endif
        /// <summary>Enable blind bidding</summary>
        public bool? BlindBidding { get; set; }
        /// <summary>The created_by property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_created_by? CreatedBy { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_created_by CreatedBy { get; set; }
#endif
        /// <summary>Display Project Name</summary>
        public bool? DisplayProjectName { get; set; }
        /// <summary>Array of distribution member ids</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<int?>? DistributionMemberIds { get; set; }
#nullable restore
#else
        public List<int?> DistributionMemberIds { get; set; }
#endif
        /// <summary>Array of the distribution members</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_distribution_members>? DistributionMembers { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_distribution_members> DistributionMembers { get; set; }
#endif
        /// <summary>Enable countdown emails</summary>
        public bool? EnableCountdownEmails { get; set; }
        /// <summary>Enable prebid RFI deadline</summary>
        public bool? EnablePrebidRfiDeadline { get; set; }
        /// <summary>Enable prebid walkthrough</summary>
        public bool? EnablePrebidWalkthrough { get; set; }
        /// <summary>Flexible Response Types Enabled</summary>
        public bool? FlexibleResponseTypesEnabled { get; set; }
        /// <summary>Has any Bid been Invited</summary>
        public bool? HasAnyBidInvited { get; set; }
        /// <summary>Have any Bidders been sent NDA</summary>
        public bool? HasAnyBidsSentNda { get; set; }
        /// <summary>Has no Non-Disclosure Agreement activity</summary>
        public bool? HasNoNdaActivity { get; set; }
        /// <summary>Whether or not the bid package has been recycled</summary>
        public bool? Hidden { get; set; }
        /// <summary>ID</summary>
        public int? Id { get; set; }
        /// <summary>Links that can be used by Frontend</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_links? Links { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_links Links { get; set; }
#endif
        /// <summary>Lump Sum Bidding Enabled</summary>
        public bool? LumpSumBidding { get; set; }
        /// <summary>The manager property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_manager? Manager { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_manager Manager { get; set; }
#endif
        /// <summary>The nda_attachments property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_nda_attachments>? NdaAttachments { get; set; }
#nullable restore
#else
        public List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_nda_attachments> NdaAttachments { get; set; }
#endif
        /// <summary>Bids count with Non-Disclosure Agreement activity like viewed, declined or downloaded</summary>
        public int? NdaInvitedBidsWithActivityCount { get; set; }
        /// <summary>Bid Package Number</summary>
        public int? Number { get; set; }
        /// <summary>Whether or not the bid package is active</summary>
        public bool? Open { get; set; }
        /// <summary>The point_of_contact property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_point_of_contact? PointOfContact { get; set; }
#nullable restore
#else
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_point_of_contact PointOfContact { get; set; }
#endif
        /// <summary>Point of contact ID</summary>
        public int? PointOfContactLoginId { get; set; }
        /// <summary>Prebid RFI deadline date</summary>
        public DateTimeOffset? PreBidRfiDeadlineDate { get; set; }
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
        /// <summary>The display format of the project&apos;s currency (e.g., &apos;$&apos;, &apos;€&apos;)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectCurrencyDisplay { get; set; }
#nullable restore
#else
        public string ProjectCurrencyDisplay { get; set; }
#endif
        /// <summary>The ISO code of the project&apos;s currency (e.g., &apos;USD&apos;, &apos;EUR&apos;)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectCurrencyIsoCode { get; set; }
#nullable restore
#else
        public string ProjectCurrencyIsoCode { get; set; }
#endif
        /// <summary>Unique identifier for the project.</summary>
        public int? ProjectId { get; set; }
        /// <summary>Link to project image</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectImageUrl { get; set; }
#nullable restore
#else
        public string ProjectImageUrl { get; set; }
#endif
        /// <summary>Latitude of project location</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectLatitude { get; set; }
#nullable restore
#else
        public string ProjectLatitude { get; set; }
#endif
        /// <summary>Address of bid package project</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectLocation { get; set; }
#nullable restore
#else
        public string ProjectLocation { get; set; }
#endif
        /// <summary>Longtitude of project location</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectLongitude { get; set; }
#nullable restore
#else
        public string ProjectLongitude { get; set; }
#endif
        /// <summary>Name of bid package project</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ProjectName { get; set; }
#nullable restore
#else
        public string ProjectName { get; set; }
#endif
        /// <summary>Require Non-Disclosure Agreement</summary>
        public bool? RequireNda { get; set; }
        /// <summary>Enabled sealed bidding</summary>
        public bool? Sealed { get; set; }
        /// <summary>Show bid info</summary>
        public bool? ShowBidInfo { get; set; }
        /// <summary>Number bids submitted</summary>
        public int? SubmittedBidsCount { get; set; }
        /// <summary>Title</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse"/> and sets the default values.
        /// </summary>
        public Bid_package_GetResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse();
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
                { "allow_bidder_sum", n => { AllowBidderSum = n.GetBoolValue(); } },
                { "anticipated_award_date", n => { AnticipatedAwardDate = n.GetDateValue(); } },
                { "attachments_zip_streaming_url", n => { AttachmentsZipStreamingUrl = n.GetStringValue(); } },
                { "bid_docs_manifest", n => { BidDocsManifest = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_bid_docs_manifest>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_bid_docs_manifest.CreateFromDiscriminatorValue); } },
                { "bid_due_date", n => { BidDueDate = n.GetDateTimeOffsetValue(); } },
                { "bid_email_message", n => { BidEmailMessage = n.GetStringValue(); } },
                { "bid_emails_include_link_to_bidding_documents", n => { BidEmailsIncludeLinkToBiddingDocuments = n.GetBoolValue(); } },
                { "bid_form_sections_enabled", n => { BidFormSectionsEnabled = n.GetBoolValue(); } },
                { "bid_submission_confirmation", n => { BidSubmissionConfirmation = n.GetStringValue(); } },
                { "bid_web_message", n => { BidWebMessage = n.GetStringValue(); } },
                { "bidding_countdown_email_days", n => { BiddingCountdownEmailDays = n.GetIntValue(); } },
                { "blind_bidding", n => { BlindBidding = n.GetBoolValue(); } },
                { "created_by", n => { CreatedBy = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_created_by>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_created_by.CreateFromDiscriminatorValue); } },
                { "display_project_name", n => { DisplayProjectName = n.GetBoolValue(); } },
                { "distribution_member_ids", n => { DistributionMemberIds = n.GetCollectionOfPrimitiveValues<int?>()?.AsList(); } },
                { "distribution_members", n => { DistributionMembers = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_distribution_members>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_distribution_members.CreateFromDiscriminatorValue)?.AsList(); } },
                { "enable_countdown_emails", n => { EnableCountdownEmails = n.GetBoolValue(); } },
                { "enable_prebid_rfi_deadline", n => { EnablePrebidRfiDeadline = n.GetBoolValue(); } },
                { "enable_prebid_walkthrough", n => { EnablePrebidWalkthrough = n.GetBoolValue(); } },
                { "flexible_response_types_enabled", n => { FlexibleResponseTypesEnabled = n.GetBoolValue(); } },
                { "has_any_bid_invited", n => { HasAnyBidInvited = n.GetBoolValue(); } },
                { "has_any_bids_sent_nda", n => { HasAnyBidsSentNda = n.GetBoolValue(); } },
                { "has_no_nda_activity", n => { HasNoNdaActivity = n.GetBoolValue(); } },
                { "hidden", n => { Hidden = n.GetBoolValue(); } },
                { "id", n => { Id = n.GetIntValue(); } },
                { "links", n => { Links = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_links>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_links.CreateFromDiscriminatorValue); } },
                { "lump_sum_bidding", n => { LumpSumBidding = n.GetBoolValue(); } },
                { "manager", n => { Manager = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_manager>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_manager.CreateFromDiscriminatorValue); } },
                { "nda_attachments", n => { NdaAttachments = n.GetCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_nda_attachments>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_nda_attachments.CreateFromDiscriminatorValue)?.AsList(); } },
                { "nda_invited_bids_with_activity_count", n => { NdaInvitedBidsWithActivityCount = n.GetIntValue(); } },
                { "number", n => { Number = n.GetIntValue(); } },
                { "open", n => { Open = n.GetBoolValue(); } },
                { "point_of_contact", n => { PointOfContact = n.GetObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_point_of_contact>(global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_point_of_contact.CreateFromDiscriminatorValue); } },
                { "point_of_contact_login_id", n => { PointOfContactLoginId = n.GetIntValue(); } },
                { "pre_bid_rfi_deadline_date", n => { PreBidRfiDeadlineDate = n.GetDateTimeOffsetValue(); } },
                { "pre_bid_walk_through_date", n => { PreBidWalkThroughDate = n.GetDateTimeOffsetValue(); } },
                { "pre_bid_walk_through_notes", n => { PreBidWalkThroughNotes = n.GetStringValue(); } },
                { "project_currency_display", n => { ProjectCurrencyDisplay = n.GetStringValue(); } },
                { "project_currency_iso_code", n => { ProjectCurrencyIsoCode = n.GetStringValue(); } },
                { "project_id", n => { ProjectId = n.GetIntValue(); } },
                { "project_image_url", n => { ProjectImageUrl = n.GetStringValue(); } },
                { "project_latitude", n => { ProjectLatitude = n.GetStringValue(); } },
                { "project_location", n => { ProjectLocation = n.GetStringValue(); } },
                { "project_longitude", n => { ProjectLongitude = n.GetStringValue(); } },
                { "project_name", n => { ProjectName = n.GetStringValue(); } },
                { "require_nda", n => { RequireNda = n.GetBoolValue(); } },
                { "sealed", n => { Sealed = n.GetBoolValue(); } },
                { "show_bid_info", n => { ShowBidInfo = n.GetBoolValue(); } },
                { "submitted_bids_count", n => { SubmittedBidsCount = n.GetIntValue(); } },
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
            writer.WriteBoolValue("allow_bidder_sum", AllowBidderSum);
            writer.WriteDateValue("anticipated_award_date", AnticipatedAwardDate);
            writer.WriteStringValue("attachments_zip_streaming_url", AttachmentsZipStreamingUrl);
            writer.WriteIntValue("bidding_countdown_email_days", BiddingCountdownEmailDays);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_bid_docs_manifest>("bid_docs_manifest", BidDocsManifest);
            writer.WriteDateTimeOffsetValue("bid_due_date", BidDueDate);
            writer.WriteStringValue("bid_email_message", BidEmailMessage);
            writer.WriteBoolValue("bid_emails_include_link_to_bidding_documents", BidEmailsIncludeLinkToBiddingDocuments);
            writer.WriteBoolValue("bid_form_sections_enabled", BidFormSectionsEnabled);
            writer.WriteStringValue("bid_submission_confirmation", BidSubmissionConfirmation);
            writer.WriteStringValue("bid_web_message", BidWebMessage);
            writer.WriteBoolValue("blind_bidding", BlindBidding);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_created_by>("created_by", CreatedBy);
            writer.WriteBoolValue("display_project_name", DisplayProjectName);
            writer.WriteCollectionOfPrimitiveValues<int?>("distribution_member_ids", DistributionMemberIds);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_distribution_members>("distribution_members", DistributionMembers);
            writer.WriteBoolValue("enable_countdown_emails", EnableCountdownEmails);
            writer.WriteBoolValue("enable_prebid_rfi_deadline", EnablePrebidRfiDeadline);
            writer.WriteBoolValue("enable_prebid_walkthrough", EnablePrebidWalkthrough);
            writer.WriteBoolValue("flexible_response_types_enabled", FlexibleResponseTypesEnabled);
            writer.WriteBoolValue("has_any_bid_invited", HasAnyBidInvited);
            writer.WriteBoolValue("has_any_bids_sent_nda", HasAnyBidsSentNda);
            writer.WriteBoolValue("has_no_nda_activity", HasNoNdaActivity);
            writer.WriteBoolValue("hidden", Hidden);
            writer.WriteIntValue("id", Id);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_links>("links", Links);
            writer.WriteBoolValue("lump_sum_bidding", LumpSumBidding);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_manager>("manager", Manager);
            writer.WriteCollectionOfObjectValues<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_nda_attachments>("nda_attachments", NdaAttachments);
            writer.WriteIntValue("nda_invited_bids_with_activity_count", NdaInvitedBidsWithActivityCount);
            writer.WriteIntValue("number", Number);
            writer.WriteBoolValue("open", Open);
            writer.WriteObjectValue<global::Procore.SDK.Core.Rest.V10.Companies.Item.Bid_packages.Item.Bid_package_GetResponse_point_of_contact>("point_of_contact", PointOfContact);
            writer.WriteIntValue("point_of_contact_login_id", PointOfContactLoginId);
            writer.WriteDateTimeOffsetValue("pre_bid_rfi_deadline_date", PreBidRfiDeadlineDate);
            writer.WriteDateTimeOffsetValue("pre_bid_walk_through_date", PreBidWalkThroughDate);
            writer.WriteStringValue("pre_bid_walk_through_notes", PreBidWalkThroughNotes);
            writer.WriteStringValue("project_currency_display", ProjectCurrencyDisplay);
            writer.WriteStringValue("project_currency_iso_code", ProjectCurrencyIsoCode);
            writer.WriteIntValue("project_id", ProjectId);
            writer.WriteStringValue("project_image_url", ProjectImageUrl);
            writer.WriteStringValue("project_latitude", ProjectLatitude);
            writer.WriteStringValue("project_location", ProjectLocation);
            writer.WriteStringValue("project_longitude", ProjectLongitude);
            writer.WriteStringValue("project_name", ProjectName);
            writer.WriteBoolValue("require_nda", RequireNda);
            writer.WriteBoolValue("sealed", Sealed);
            writer.WriteBoolValue("show_bid_info", ShowBidInfo);
            writer.WriteIntValue("submitted_bids_count", SubmittedBidsCount);
            writer.WriteStringValue("title", Title);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
