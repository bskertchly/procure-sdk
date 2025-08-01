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
namespace Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\timesheets\filters\approval_status
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Approval_statusRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_statusRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Approval_statusRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/timesheets/filters/approval_status", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_statusRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Approval_statusRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/timesheets/filters/approval_status", rawUrl)
        {
        }
        /// <summary>
        /// Show timesheet approval status filters
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status>?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status>> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status400Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status404Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status>(requestInfo, global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_status.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Show timesheet approval status filters
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
        /// <returns>A <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_statusRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_statusRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.FieldProductivity.Rest.V10.Companies.Item.Timesheets.Filters.Approval_status.Approval_statusRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
