// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\checklist\list_templates
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class List_templatesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.Core.rest.v10.companies.item.checklist.list_templates.item collection</summary>
        /// <param name="position">Company Checklist Template ID</param>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder"/></returns>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("%2Did", position);
                return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public List_templatesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/checklist/list_templates{?filters%5Bid%5D,filters%5Binspection_type_id%5D,filters%5Bquery%5D*,filters%5Bresponse_set_id%5D,filters%5Btrade_id%5D*,filters%5Bupdated_at%5D*,page*,per_page*,sort*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public List_templatesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/checklist/list_templates{?filters%5Bid%5D,filters%5Binspection_type_id%5D,filters%5Bquery%5D*,filters%5Bresponse_set_id%5D,filters%5Btrade_id%5D*,filters%5Bupdated_at%5D*,page*,per_page*,sort*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns a collection of Company Checklist Templates for a specified Company.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder.List_templatesRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder.List_templatesRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Creates a Company Checklist Template for a specified Company.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostResponse?> PostAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostResponse> PostAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templates403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns a collection of Company Checklist Templates for a specified Company.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder.List_templatesRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder.List_templatesRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Creates a Company Checklist Template for a specified Company.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.List_templatesRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns a collection of Company Checklist Templates for a specified Company.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class List_templatesRequestBuilderGetQueryParameters 
        {
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
            /// <summary>Array of Inspection Type IDs. Return item(s) associated with the specified Inspection Type IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Binspection_type_id%5D")]
            public int?[]? FiltersinspectionTypeId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Binspection_type_id%5D")]
            public int?[] FiltersinspectionTypeId { get; set; }
#endif
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
            /// <summary>Array of Item Response Set IDs. Return list template(s) whose items are associated with the given Response Set IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bresponse_set_id%5D")]
            public int?[]? FiltersresponseSetId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bresponse_set_id%5D")]
            public int?[] FiltersresponseSetId { get; set; }
#endif
            /// <summary>Trade ID</summary>
            [QueryParameter("filters%5Btrade_id%5D")]
            public int? FilterstradeId { get; set; }
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
            public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.GetSortQueryParameterType? Sort { get; set; }
        }
    }
}
#pragma warning restore CS0618
