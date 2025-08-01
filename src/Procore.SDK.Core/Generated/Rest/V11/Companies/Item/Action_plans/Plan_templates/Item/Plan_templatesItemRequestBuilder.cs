// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Publish;
using Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Revise;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.1\companies\{company_id}\action_plans\plan_templates\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Plan_templatesItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The publish property</summary>
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Publish.PublishRequestBuilder Publish
        {
            get => new global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Publish.PublishRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The revise property</summary>
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Revise.ReviseRequestBuilder Revise
        {
            get => new global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Revise.ReviseRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Plan_templatesItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/companies/{company_id}/action_plans/plan_templates/{id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Plan_templatesItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/companies/{company_id}/action_plans/plan_templates/{id}", rawUrl)
        {
        }
        /// <summary>
        /// Delete a Company Action Plan Template
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates404Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates422Error.CreateFromDiscriminatorValue },
            };
            await RequestAdapter.SendNoContentAsync(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Details of a single Company Action Plan Template
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesGetResponse?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesGetResponse> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesGetResponse>(requestInfo, global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Update a Company Action Plan Template
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchResponse?> PatchAsync(global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchResponse> PatchAsync(global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates404Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templates422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchResponse>(requestInfo, global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Delete a Company Action Plan Template
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
        /// Details of a single Company Action Plan Template
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
        /// Update a Company Action Plan Template
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V11.Companies.Item.Action_plans.Plan_templates.Item.Plan_templatesItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
