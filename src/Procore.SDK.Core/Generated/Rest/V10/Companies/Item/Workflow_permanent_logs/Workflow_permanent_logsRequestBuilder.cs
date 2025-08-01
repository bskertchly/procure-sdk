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
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\workflow_permanent_logs
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Workflow_permanent_logsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Workflow_permanent_logsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/workflow_permanent_logs?filters[workflowed_object_id]={filters%5Bworkflowed_object_id%5D}&filters[workflowed_object_type]={filters%5Bworkflowed_object_type%5D}{&page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Workflow_permanent_logsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/workflow_permanent_logs?filters[workflowed_object_id]={filters%5Bworkflowed_object_id%5D}&filters[workflowed_object_type]={filters%5Bworkflowed_object_type%5D}{&page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of workflow permanent logs. Any resource using workflow should have log of activities and events related to the workflow.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder.Workflow_permanent_logsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder.Workflow_permanent_logsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logs.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Return a list of workflow permanent logs. Any resource using workflow should have log of activities and events related to the workflow.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder.Workflow_permanent_logsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder.Workflow_permanent_logsRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Workflow_permanent_logs.Workflow_permanent_logsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of workflow permanent logs. Any resource using workflow should have log of activities and events related to the workflow.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Workflow_permanent_logsRequestBuilderGetQueryParameters 
        {
            /// <summary>Filter log(s) with matching workflowed object id</summary>
            [QueryParameter("filters%5Bworkflowed_object_id%5D")]
            public int? FiltersworkflowedObjectId { get; set; }
            /// <summary>Filter log(s) with matching workflowed object type</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bworkflowed_object_type%5D")]
            public string? FiltersworkflowedObjectType { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bworkflowed_object_type%5D")]
            public string FiltersworkflowedObjectType { get; set; }
#endif
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
