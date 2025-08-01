// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Bulk_create;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\action_plans\plan_references
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Plan_referencesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The bulk_create property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Bulk_create.Bulk_createRequestBuilder Bulk_create
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Bulk_create.Bulk_createRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.action_plans.plan_references.item collection</summary>
        /// <param name="position">Action Plan Reference ID</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Item.Plan_referencesItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Item.Plan_referencesItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Item.Plan_referencesItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Plan_referencesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/action_plans/plan_references{?filters%5Bcreated_at%5D*,filters%5Bid%5D,filters%5Bplan_id%5D,filters%5Bplan_item_id%5D,filters%5Bupdated_at%5D*,page*,per_page*,sort*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Plan_referencesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/action_plans/plan_references{?filters%5Bcreated_at%5D*,filters%5Bid%5D,filters%5Bplan_id%5D,filters%5Bplan_item_id%5D,filters%5Bupdated_at%5D*,page*,per_page*,sort*}", rawUrl)
        {
        }
        /// <summary>
        /// List of all Action Plan References
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder.Plan_referencesRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder.Plan_referencesRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Create an Action Plan Reference
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostResponse?> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostResponse> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_references422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// List of all Action Plan References
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder.Plan_referencesRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder.Plan_referencesRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Create an Action Plan Reference
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.Plan_referencesRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// List of all Action Plan References
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Plan_referencesRequestBuilderGetQueryParameters 
        {
            /// <summary>Return item(s) created within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bcreated_at%5D")]
            public Date? FilterscreatedAt { get; set; }
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
            /// <summary>Return item(s) associated with the specified Action Plan ID(s)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bplan_id%5D")]
            public int?[]? FiltersplanId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bplan_id%5D")]
            public int?[] FiltersplanId { get; set; }
#endif
            /// <summary>Return item(s) associated with the specified Action Plan Item ID(s).</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bplan_item_id%5D")]
            public int?[]? FiltersplanItemId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bplan_item_id%5D")]
            public int?[] FiltersplanItemId { get; set; }
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
            /// <summary>Direction (asc/desc) can be controlled by the presence or absence of &apos;-&apos; before the sort parameter.</summary>
            [QueryParameter("sort")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Action_plans.Plan_references.GetSortQueryParameterType? Sort { get; set; }
        }
    }
}
#pragma warning restore CS0618
