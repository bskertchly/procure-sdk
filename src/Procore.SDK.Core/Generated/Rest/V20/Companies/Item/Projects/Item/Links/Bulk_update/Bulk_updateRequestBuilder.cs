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
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\links\bulk_update
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Bulk_updateRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Bulk_updateRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/links/bulk_update", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Bulk_updateRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/links/bulk_update", rawUrl)
        {
        }
        /// <summary>
        /// Create and update multiple Home Links on a given project based on present and missing ID.The order of the links in the request will be the order stored in the position field.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updatePatchResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updatePatchResponse?> PatchAsync(List<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update> body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updatePatchResponse> PatchAsync(List<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update> body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updatePatchResponse>(requestInfo, global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updatePatchResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Create and update multiple Home Links on a given project based on present and missing ID.The order of the links in the request will be the order stored in the position field.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(List<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update> body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(List<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_update> body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Links.Bulk_update.Bulk_updateRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
