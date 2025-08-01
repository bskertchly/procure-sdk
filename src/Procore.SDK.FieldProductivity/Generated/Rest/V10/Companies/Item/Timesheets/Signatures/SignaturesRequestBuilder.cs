// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\timesheets\signatures
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SignaturesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.FieldProductivity.rest.v10.companies.item.timesheets.signatures.item collection</summary>
        /// <param name="position">ID</param>
        /// <returns>A <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesItemRequestBuilder"/></returns>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Item.SignaturesItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SignaturesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/timesheets/signatures", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SignaturesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/timesheets/signatures", rawUrl)
        {
        }
        /// <summary>
        /// Create new Signature associated with the specified Company.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures422Error">When receiving a 422 status code</exception>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostResponse?> PostAsync(global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostResponse> PostAsync(global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures422Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.Signatures5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostResponse>(requestInfo, global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Create new Signature associated with the specified Company.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Signatures.SignaturesRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
