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
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\daily_logs\delay_log_types\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Delay_log_typesItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Delay_log_typesItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/daily_logs/delay_log_types/{id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Delay_log_typesItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/daily_logs/delay_log_types/{id}", rawUrl)
        {
        }
        /// <summary>
        /// Update the visibility of a Delay Log Type
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_types403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_types422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchResponse?> PatchAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchResponse> PatchAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_types403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_types422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Update the visibility of a Delay Log Type
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Daily_logs.Delay_log_types.Item.Delay_log_typesItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
