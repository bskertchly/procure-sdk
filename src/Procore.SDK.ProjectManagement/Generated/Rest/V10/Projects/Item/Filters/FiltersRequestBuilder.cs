// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\filters
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class FiltersRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.filters.item collection</summary>
        /// <param name="position">Filter name</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Item.WithFilter_nameItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Item.WithFilter_nameItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("filter_name", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Item.WithFilter_nameItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public FiltersRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/filters?tab={tab}&tool={tool}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public FiltersRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/filters?tab={tab}&tool={tool}", rawUrl)
        {
        }
        /// <summary>
        /// List all filters customer can use for the project and tool
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder.FiltersRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder.FiltersRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters404Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.Filters5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersGetResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// List all filters customer can use for the project and tool
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder.FiltersRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder.FiltersRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.FiltersRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// List all filters customer can use for the project and tool
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class FiltersRequestBuilderGetQueryParameters 
        {
            /// <summary>Tab name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("tab")]
            public string? Tab { get; set; }
#nullable restore
#else
            [QueryParameter("tab")]
            public string Tab { get; set; }
#endif
            /// <summary>Tool name</summary>
            [QueryParameter("tool")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Filters.GetToolQueryParameterType? Tool { get; set; }
        }
    }
}
#pragma warning restore CS0618
