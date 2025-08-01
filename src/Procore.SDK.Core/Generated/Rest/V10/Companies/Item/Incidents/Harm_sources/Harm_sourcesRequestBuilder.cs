// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Bulk_update;
using Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\incidents\harm_sources
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Harm_sourcesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The bulk_update property</summary>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Bulk_update.Bulk_updateRequestBuilder Bulk_update
        {
            get => new global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Bulk_update.Bulk_updateRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.Core.rest.v10.companies.item.incidents.harm_sources.item collection</summary>
        /// <param name="position">Harm Source ID</param>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Item.Harm_sourcesItemRequestBuilder"/></returns>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Item.Harm_sourcesItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Item.Harm_sourcesItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Harm_sourcesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/incidents/harm_sources{?all*,filters%5Bactive%5D*,filters%5Bid%5D,filters%5Bupdated_at%5D*,page*,per_page*,sort*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Harm_sourcesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/incidents/harm_sources{?all*,filters%5Bactive%5D*,filters%5Bid%5D,filters%5Bupdated_at%5D*,page*,per_page*,sort*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of all Harm Sources associated with a Company.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources401Error">When receiving a 401 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder.Harm_sourcesRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder.Harm_sourcesRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources401Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Creates a Harm Source with the specified name
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostResponse?> PostAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostResponse> PostAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sources422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Return a list of all Harm Sources associated with a Company.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder.Harm_sourcesRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder.Harm_sourcesRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Creates a Harm Source with the specified name
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = new RequestInformation(Method.POST, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            requestInfo.SetContentFromParsable(RequestAdapter, "application/json", body);
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.Harm_sourcesRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of all Harm Sources associated with a Company.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Harm_sourcesRequestBuilderGetQueryParameters 
        {
            /// <summary>Harm Sources</summary>
            [QueryParameter("all")]
            public bool? All { get; set; }
            /// <summary>If true, returns item(s) with a status of &apos;active&apos;.</summary>
            [QueryParameter("filters%5Bactive%5D")]
            public bool? Filtersactive { get; set; }
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
            /// <summary>Return item(s) last updated within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00`...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bupdated_at%5D")]
            public Date? FiltersupdatedAt { get; set; }
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
            [QueryParameter("sort")]
            public global::Procore.SDK.Core.Rest.V10.Companies.Item.Incidents.Harm_sources.GetSortQueryParameterType? Sort { get; set; }
        }
    }
}
#pragma warning restore CS0618
