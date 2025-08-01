// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.1\projects\{project_id}\schedule\lookaheads
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class LookaheadsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v11.projects.item.schedule.lookaheads.item collection</summary>
        /// <param name="position">Lookahead ID</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Item.LookaheadsItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Item.LookaheadsItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Item.LookaheadsItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public LookaheadsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/schedule/lookaheads", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public LookaheadsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/schedule/lookaheads", rawUrl)
        {
        }
        /// <summary>
        /// Returns all Lookaheads for the project.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads>?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads>> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Create a new Lookahead for the project
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostResponse?> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostResponse> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.Lookaheads422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns all Lookaheads for the project.
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
        /// Create a new Lookahead for the project
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Schedule.Lookaheads.LookaheadsRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
