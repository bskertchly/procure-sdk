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
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\timesheets\scoped_cost_code_ids
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Scoped_cost_code_idsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Scoped_cost_code_idsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/timesheets/scoped_cost_code_ids{?sub_job_id*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Scoped_cost_code_idsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/timesheets/scoped_cost_code_ids{?sub_job_id*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns a list of Cost Codes Ids for Timesheets.
        /// </summary>
        /// <returns>A List&lt;int&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_ids401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_ids403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<int?>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder.Scoped_cost_code_idsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<int?>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder.Scoped_cost_code_idsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_ids401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_ids403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendPrimitiveCollectionAsync<int?>(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Returns a list of Cost Codes Ids for Timesheets.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder.Scoped_cost_code_idsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder.Scoped_cost_code_idsRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns a list of Cost Codes Ids for Timesheets.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Scoped_cost_code_idsRequestBuilderGetQueryParameters 
        {
            /// <summary>Sub Job ID</summary>
            [QueryParameter("sub_job_id")]
            public int? SubJobId { get; set; }
        }
    }
}
#pragma warning restore CS0618
