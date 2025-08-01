// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\project_regions
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Project_regionsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.Core.rest.v10.companies.item.project_regions.item collection</summary>
        /// <param name="position">ID of the Project Region</param>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Item.Project_regionsItemRequestBuilder"/></returns>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Item.Project_regionsItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Item.Project_regionsItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Project_regionsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/project_regions{?page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Project_regionsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/project_regions{?page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of Project Regions.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder.Project_regionsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder.Project_regionsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions5XXError.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Create a new Project Region in the specified Company.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions422Error">When receiving a 422 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostResponse?> PostAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostResponse> PostAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions404Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions422Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regions5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Return a list of Project Regions.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder.Project_regionsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder.Project_regionsRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Create a new Project Region in the specified Company.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Project_regions.Project_regionsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of Project Regions.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Project_regionsRequestBuilderGetQueryParameters 
        {
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
