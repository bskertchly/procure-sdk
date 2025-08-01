// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\async_operations
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Async_operationsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.Core.rest.v20.companies.item.async_operations.item collection</summary>
        /// <param name="position">Unique identifier for the operation.</param>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Item.WithOperation_ItemRequestBuilder"/></returns>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Item.WithOperation_ItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("operation_id", position);
                return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Item.WithOperation_ItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Async_operationsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/async_operations{?filters%5Boperation_type%5D*,filters%5Bstarted_after%5D*,filters%5Bstarted_before%5D*,filters%5Bstatus%5D*,page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Async_operationsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/async_operations{?filters%5Boperation_type%5D*,filters%5Bstarted_after%5D*,filters%5Bstarted_before%5D*,filters%5Bstatus%5D*,page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// List all asynchronous operations for the company
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder.Async_operationsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder.Async_operationsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operations5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsGetResponse>(requestInfo, global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// List all asynchronous operations for the company
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder.Async_operationsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder.Async_operationsRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// List all asynchronous operations for the company
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Async_operationsRequestBuilderGetQueryParameters 
        {
            /// <summary>Return operations of the specified type.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Boperation_type%5D")]
            public string? FiltersoperationType { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Boperation_type%5D")]
            public string FiltersoperationType { get; set; }
#endif
            /// <summary>Return operations that started after the started_after time.</summary>
            [QueryParameter("filters%5Bstarted_after%5D")]
            public DateTimeOffset? FiltersstartedAfter { get; set; }
            /// <summary>Return operations that started before the started_before time.</summary>
            [QueryParameter("filters%5Bstarted_before%5D")]
            public DateTimeOffset? FiltersstartedBefore { get; set; }
            /// <summary>Return operations with the specified status.</summary>
            [QueryParameter("filters%5Bstatus%5D")]
            public global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.GetFiltersStatusQueryParameterType? Filtersstatus { get; set; }
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
