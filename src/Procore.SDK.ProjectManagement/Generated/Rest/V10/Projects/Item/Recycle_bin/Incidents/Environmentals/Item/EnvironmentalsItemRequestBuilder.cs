// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Restore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\recycle_bin\incidents\environmentals\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class EnvironmentalsItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The restore property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Restore.RestoreRequestBuilder Restore
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Restore.RestoreRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public EnvironmentalsItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/recycle_bin/incidents/environmentals/{id}{?incident_id*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public EnvironmentalsItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/recycle_bin/incidents/environmentals/{id}{?incident_id*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns specified Recycled Environmental record
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Environmentals401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Environmentals403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder.EnvironmentalsItemRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder.EnvironmentalsItemRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Environmentals401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.Environmentals403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsGetResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns specified Recycled Environmental record
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder.EnvironmentalsItemRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder.EnvironmentalsItemRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recycle_bin.Incidents.Environmentals.Item.EnvironmentalsItemRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns specified Recycled Environmental record
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class EnvironmentalsItemRequestBuilderGetQueryParameters 
        {
            /// <summary>Incident ID</summary>
            [QueryParameter("incident_id")]
            public int? IncidentId { get; set; }
        }
    }
}
#pragma warning restore CS0618
