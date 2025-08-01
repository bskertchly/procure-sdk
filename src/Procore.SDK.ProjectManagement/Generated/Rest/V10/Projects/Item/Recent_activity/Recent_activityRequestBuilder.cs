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
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\recent_activity
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Recent_activityRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Recent_activityRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/recent_activity{?page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Recent_activityRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/recent_activity{?page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of items that have been recently updated by the users.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder.Recent_activityRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder.Recent_activityRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity5XXError.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activity.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Return a list of items that have been recently updated by the users.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder.Recent_activityRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder.Recent_activityRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Recent_activity.Recent_activityRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of items that have been recently updated by the users.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Recent_activityRequestBuilderGetQueryParameters 
        {
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
