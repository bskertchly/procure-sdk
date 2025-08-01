// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Remove_alternative_response_set;
using Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Sections;
using Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Use_alternative_response_set;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\checklist\list_templates\{-id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The remove_alternative_response_set property</summary>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Remove_alternative_response_set.Remove_alternative_response_setRequestBuilder Remove_alternative_response_set
        {
            get => new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Remove_alternative_response_set.Remove_alternative_response_setRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The sections property</summary>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Sections.SectionsRequestBuilder Sections
        {
            get => new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Sections.SectionsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The use_alternative_response_set property</summary>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Use_alternative_response_set.Use_alternative_response_setRequestBuilder Use_alternative_response_set
        {
            get => new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.Use_alternative_response_set.Use_alternative_response_setRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/checklist/list_templates/{%2Did}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/checklist/list_templates/{%2Did}", rawUrl)
        {
        }
        /// <summary>
        /// Delete a Company Checklist Template
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroOneError">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroThreeError">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroFourError">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourTwoTwoError">When receiving a 422 status code</exception>
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
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroOneError.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroThreeError.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroFourError.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourTwoTwoError.CreateFromDiscriminatorValue },
            };
            await RequestAdapter.SendNoContentAsync(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns the details for a specified Company Checklist Template
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.GetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroOneError">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroThreeError">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.GetResponse?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.GetResponse> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroOneError.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroThreeError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.GetResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.GetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Updates a Company Checklist Template for a specified Company.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroZeroError">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroOneError">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroThreeError">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchResponse?> PatchAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchResponse> PatchAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroZeroError.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroOneError.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.FourZeroThreeError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Delete a Company Checklist Template
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
        /// Returns the details for a specified Company Checklist Template
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
        /// Updates a Company Checklist Template for a specified Company.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Checklist.List_templates.Item.ItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
