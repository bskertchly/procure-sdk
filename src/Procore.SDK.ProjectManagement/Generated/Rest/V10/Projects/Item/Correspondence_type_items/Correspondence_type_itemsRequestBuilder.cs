// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\correspondence_type_items
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Correspondence_type_itemsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Correspondence_type_itemsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/correspondence_type_items{?completion_mode*,filters%5Bclosed_at%5D*,filters%5Bcreated_at%5D*,filters%5Bcreated_by_id%5D,filters%5Bgeneric_tool_id%5D,filters%5Bid%5D,filters%5Bissued_at%5D*,filters%5Blocation_id%5D*,filters%5Blogin_information_id%5D,filters%5Boverdue%5D*,filters%5Bprivate%5D*,filters%5Bquery%5D*,filters%5Breceived_from_id%5D*,filters%5Brecycle_bin%5D*,filters%5Bstatus%5D,filters%5Bupdated_at%5D*,filters%5Bvendor_id%5D*,group*,page*,per_page*,sort*,view*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Correspondence_type_itemsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/correspondence_type_items{?completion_mode*,filters%5Bclosed_at%5D*,filters%5Bcreated_at%5D*,filters%5Bcreated_by_id%5D,filters%5Bgeneric_tool_id%5D,filters%5Bid%5D,filters%5Bissued_at%5D*,filters%5Blocation_id%5D*,filters%5Blogin_information_id%5D,filters%5Boverdue%5D*,filters%5Bprivate%5D*,filters%5Bquery%5D*,filters%5Breceived_from_id%5D*,filters%5Brecycle_bin%5D*,filters%5Bstatus%5D,filters%5Bupdated_at%5D*,filters%5Bvendor_id%5D*,group*,page*,per_page*,sort*,view*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns a list of all Correspondence Type Items in the specified Project. For more information on Generic Tool and Correspondence Tool endpoints, see [Working with the Correspondence Tool](/documentation/tutorial-correspondence).
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items5XXError.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Update all specified Correspondence Type Items. For more information on Generic Tool and Correspondence Tool endpoints, see [Working with the Correspondence Tool](/documentation/tutorial-correspondence).
        /// </summary>
        /// <returns>A <see cref="UntypedNode"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<UntypedNode?> PatchAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsPatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderPatchQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<UntypedNode> PatchAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsPatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderPatchQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items404Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_items5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<UntypedNode>(requestInfo, UntypedNode.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns a list of all Correspondence Type Items in the specified Project. For more information on Generic Tool and Correspondence Tool endpoints, see [Working with the Correspondence Tool](/documentation/tutorial-correspondence).
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Update all specified Correspondence Type Items. For more information on Generic Tool and Correspondence Tool endpoints, see [Working with the Correspondence Tool](/documentation/tutorial-correspondence).
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsPatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderPatchQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsPatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder.Correspondence_type_itemsRequestBuilderPatchQueryParameters>> requestConfiguration = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = new RequestInformation(Method.PATCH, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            requestInfo.SetContentFromParsable(RequestAdapter, "application/json", body);
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.Correspondence_type_itemsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns a list of all Correspondence Type Items in the specified Project. For more information on Generic Tool and Correspondence Tool endpoints, see [Working with the Correspondence Tool](/documentation/tutorial-correspondence).
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Correspondence_type_itemsRequestBuilderGetQueryParameters 
        {
            /// <summary>Returns item(s) closed within the specified ISO 8601 datetime range.</summary>
            [QueryParameter("filters%5Bclosed_at%5D")]
            public Date? FiltersclosedAt { get; set; }
            /// <summary>Return item(s) created within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bcreated_at%5D")]
            public Date? FilterscreatedAt { get; set; }
            /// <summary>Returns item(s) created by the specified User IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bcreated_by_id%5D")]
            public int?[]? FilterscreatedById { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bcreated_by_id%5D")]
            public int?[] FilterscreatedById { get; set; }
#endif
            /// <summary>Return item(s) within the specified Generic Tool ID(s)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bgeneric_tool_id%5D")]
            public int?[]? FiltersgenericToolId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bgeneric_tool_id%5D")]
            public int?[] FiltersgenericToolId { get; set; }
#endif
            /// <summary>Return item(s) with the specified IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bid%5D")]
            public int?[]? Filtersid { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bid%5D")]
            public int?[] Filtersid { get; set; }
#endif
            /// <summary>Returns item(s) issued within the specified ISO 8601 datetime range.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bissued_at%5D")]
            public string? FiltersissuedAt { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bissued_at%5D")]
            public string FiltersissuedAt { get; set; }
#endif
            /// <summary>Filters by specific location (Note: Use *either* this or location_id_with_sublocations, but not both)</summary>
            [QueryParameter("filters%5Blocation_id%5D")]
            public int? FilterslocationId { get; set; }
            /// <summary>Array of Login Information IDs. Returns item(s) with the specified Login Information ID.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Blogin_information_id%5D")]
            public int?[]? FiltersloginInformationId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Blogin_information_id%5D")]
            public int?[] FiltersloginInformationId { get; set; }
#endif
            /// <summary>If true, returns item(s) that are overdue.</summary>
            [QueryParameter("filters%5Boverdue%5D")]
            public bool? Filtersoverdue { get; set; }
            /// <summary>If true, returns only item(s) with a `private` status.</summary>
            [QueryParameter("filters%5Bprivate%5D")]
            public bool? Filtersprivate { get; set; }
            /// <summary>Return item(s) containing search query</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bquery%5D")]
            public string? Filtersquery { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bquery%5D")]
            public string Filtersquery { get; set; }
#endif
            /// <summary>Received From ID</summary>
            [QueryParameter("filters%5Breceived_from_id%5D")]
            public int? FiltersreceivedFromId { get; set; }
            /// <summary>If true, returns item(s) that have been deleted.</summary>
            [QueryParameter("filters%5Brecycle_bin%5D")]
            public bool? FiltersrecycleBin { get; set; }
            /// <summary>Returns item(s) matching the specified status value.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bstatus%5D")]
            public string[]? Filtersstatus { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bstatus%5D")]
            public string[] Filtersstatus { get; set; }
#endif
            /// <summary>Return item(s) last updated within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00`...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bupdated_at%5D")]
            public Date? FiltersupdatedAt { get; set; }
            /// <summary>Return item(s) with the specified Vendor ID.</summary>
            [QueryParameter("filters%5Bvendor_id%5D")]
            public int? FiltersvendorId { get; set; }
            /// <summary>Controls if the Items are returned sorted by the sort attribute then the Generic Tool&apos;s Title or just the sort attribute. Defaults to &apos;generic_tool_title&apos;.</summary>
            [QueryParameter("group")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.GetGroupQueryParameterType? Group { get; set; }
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
            /// <summary>Field to sort by. If the field is passed with a - (EX: -updated_at) it is sorted in reverse order</summary>
            [QueryParameter("sort")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.GetSortQueryParameterType? Sort { get; set; }
            /// <summary>Defines the type of view returned. Must be one of &apos;extended&apos;, &apos;compact&apos;, &apos;ids_only&apos;, or &apos;flatten_v0&apos;.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.GetViewQueryParameterType? View { get; set; }
        }
        /// <summary>
        /// Update all specified Correspondence Type Items. For more information on Generic Tool and Correspondence Tool endpoints, see [Working with the Correspondence Tool](/documentation/tutorial-correspondence).
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Correspondence_type_itemsRequestBuilderPatchQueryParameters 
        {
            /// <summary>Defines whether to update items that can be, or none if at least one item can not be updated. Defaults to &apos;all_or_nothing&apos;.</summary>
            [QueryParameter("completion_mode")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Correspondence_type_items.PatchCompletion_modeQueryParameterType? CompletionMode { get; set; }
        }
    }
}
#pragma warning restore CS0618
