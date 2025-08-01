// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.Groups;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\workforce-planning\v2\companies\{company_id}\tags\{tag_id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithTag_ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The groups property</summary>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.Groups.GroupsRequestBuilder Groups
        {
            get => new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.Groups.GroupsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithTag_ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/tags/{tag_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithTag_ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/tags/{tag_id}", rawUrl)
        {
        }
        /// <summary>
        /// Deletes a Resource Planning Tag
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_>?> DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_>> DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_404Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_>(requestInfo, global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Gets a single Resource Planning Tag
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_GetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_GetResponse?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_GetResponse> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_GetResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_GetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Updates a Resource Planning Tag for the given company
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostResponse"/></returns>
        /// <param name="body">Request body schema for updating a Tag.</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostResponse?> PostAsync(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostResponse> PostAsync(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Deletes a Resource Planning Tag
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.DELETE, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Gets a single Resource Planning Tag
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Updates a Resource Planning Tag for the given company
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">Request body schema for updating a Tag.</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_PostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_ItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_ItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Tags.Item.WithTag_ItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
