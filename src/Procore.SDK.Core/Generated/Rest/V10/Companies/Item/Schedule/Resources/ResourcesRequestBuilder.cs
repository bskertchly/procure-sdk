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
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\schedule\resources
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ResourcesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ResourcesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/schedule/resources{?filters%5Bname%5D,page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ResourcesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/schedule/resources{?filters%5Bname%5D,page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns all resources for a given company
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.Resources401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.Resources403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.Resources404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder.ResourcesRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder.ResourcesRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.Resources401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.Resources403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.Resources404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesGetResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns all resources for a given company
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder.ResourcesRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder.ResourcesRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Schedule.Resources.ResourcesRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns all resources for a given company
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class ResourcesRequestBuilderGetQueryParameters 
        {
            /// <summary>Filter item(s) with matching name.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bname%5D")]
            public string[]? Filtersname { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bname%5D")]
            public string[] Filtersname { get; set; }
#endif
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
