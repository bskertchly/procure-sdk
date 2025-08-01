// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{project_id}\recycle_bin\observations\items\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ItemsItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The restore property</summary>
        public global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore.RestoreRequestBuilder Restore
        {
            get => new global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Restore.RestoreRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ItemsItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{project_id}/recycle_bin/observations/items/{id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ItemsItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{project_id}/recycle_bin/observations/items/{id}", rawUrl)
        {
        }
        /// <summary>
        /// Returns the specified Recycled Observation.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Items404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsGetResponse?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsGetResponse> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Items403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.Items404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsGetResponse>(requestInfo, global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns the specified Recycled Observation.
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
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.QualitySafety.Rest.V10.Projects.Item.Recycle_bin.Observations.Items.Item.ItemsItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
