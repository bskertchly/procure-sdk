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
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\prime_contracts\{prime_contract_id}\line_items\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Line_itemsItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Line_itemsItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/prime_contracts/{prime_contract_id}/line_items/{id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Line_itemsItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/prime_contracts/{prime_contract_id}/line_items/{id}", rawUrl)
        {
        }
        /// <summary>
        /// Deletes a specified prime contract line item.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items500Error">When receiving a 500 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items5XXError">When receiving a 5XX status code</exception>
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
                { "400", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items404Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items4XXError.CreateFromDiscriminatorValue },
                { "500", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items500Error.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items5XXError.CreateFromDiscriminatorValue },
            };
            await RequestAdapter.SendNoContentAsync(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Get a specified line item for a given prime contract.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items500Error">When receiving a 500 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsGetResponse?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsGetResponse> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items404Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items4XXError.CreateFromDiscriminatorValue },
                { "500", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items500Error.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsGetResponse>(requestInfo, global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Updates a line item for a given prime contract.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items409Error">When receiving a 409 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items500Error">When receiving a 500 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchResponse?> PatchAsync(global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchResponse> PatchAsync(global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items403Error.CreateFromDiscriminatorValue },
                { "409", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items409Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items4XXError.CreateFromDiscriminatorValue },
                { "500", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items500Error.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_items5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchResponse>(requestInfo, global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Deletes a specified prime contract line item.
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
        /// Get a specified line item for a given prime contract.
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
        /// Updates a line item for a given prime contract.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsPatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Item.Line_itemsItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
