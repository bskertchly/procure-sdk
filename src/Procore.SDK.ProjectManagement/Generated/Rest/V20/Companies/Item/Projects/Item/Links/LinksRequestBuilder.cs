// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\links
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class LinksRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The bulk_update property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder Bulk_update
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v20.companies.item.projects.item.links.item collection</summary>
        /// <param name="position">Link unique identifier</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Item.LinksItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Item.LinksItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Item.LinksItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public LinksRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/links{?page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public LinksRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/links{?page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns a list of Home Links on a given project.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Links401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Links403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder.LinksRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder.LinksRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Links401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.Links403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksGetResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns a list of Home Links on a given project.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder.LinksRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder.LinksRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Links.LinksRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns a list of Home Links on a given project.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class LinksRequestBuilderGetQueryParameters 
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
