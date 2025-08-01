// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\direct_costs\line_items
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Line_itemsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The sync property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncRequestBuilder Sync
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Sync.SyncRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Line_itemsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/direct_costs/line_items{?filters%5Bcost_code_id%5D*,filters%5Bcreated_at%5D*,filters%5Bdirect_cost_id%5D*,filters%5Bid%5D,filters%5Bline_item_type_id%5D*,filters%5Bupdated_at%5D*,page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Line_itemsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/direct_costs/line_items{?filters%5Bcost_code_id%5D*,filters%5Bcreated_at%5D*,filters%5Bdirect_cost_id%5D*,filters%5Bid%5D,filters%5Bline_item_type_id%5D*,filters%5Bupdated_at%5D*,page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of all Direct Cost Line Items.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder.Line_itemsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder.Line_itemsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items5XXError.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_items.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Return a list of all Direct Cost Line Items.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder.Line_itemsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder.Line_itemsRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Direct_costs.Line_items.Line_itemsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of all Direct Cost Line Items.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Line_itemsRequestBuilderGetQueryParameters 
        {
            /// <summary>Cost Code ID. Returns item(s) with the specified Cost Code ID or within the specified range of Cost Code IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bcost_code_id%5D")]
            public string? FilterscostCodeId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bcost_code_id%5D")]
            public string FilterscostCodeId { get; set; }
#endif
            /// <summary>Return item(s) created within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bcreated_at%5D")]
            public Date? FilterscreatedAt { get; set; }
            /// <summary>Return item(s) with the specified Direct Cost ID or range of Direct Cost IDs.</summary>
            [QueryParameter("filters%5Bdirect_cost_id%5D")]
            public int? FiltersdirectCostId { get; set; }
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
            /// <summary>Line Item Type ID. Returns item(s) with the specified Line Item Type ID or range of Line Item Type IDs.</summary>
            [QueryParameter("filters%5Bline_item_type_id%5D")]
            public int? FilterslineItemTypeId { get; set; }
            /// <summary>Return item(s) last updated within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00`...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bupdated_at%5D")]
            public Date? FiltersupdatedAt { get; set; }
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
        }
    }
}
#pragma warning restore CS0618
