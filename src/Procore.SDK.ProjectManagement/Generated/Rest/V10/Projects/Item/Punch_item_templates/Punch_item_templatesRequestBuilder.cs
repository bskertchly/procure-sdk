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
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\punch_item_templates
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Punch_item_templatesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Punch_item_templatesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/punch_item_templates{?filters%5Bactive%5D*,filters%5Bid%5D,filters%5Bupdated_at%5D*,page*,per_page*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Punch_item_templatesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/punch_item_templates{?filters%5Bactive%5D*,filters%5Bid%5D,filters%5Bupdated_at%5D*,page*,per_page*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of all Project Punch Item Templates associated with a Project.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder.Punch_item_templatesRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder.Punch_item_templatesRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templates.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Return a list of all Project Punch Item Templates associated with a Project.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder.Punch_item_templatesRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder.Punch_item_templatesRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Punch_item_templates.Punch_item_templatesRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of all Project Punch Item Templates associated with a Project.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Punch_item_templatesRequestBuilderGetQueryParameters 
        {
            /// <summary>If true, returns item(s) with a status of &apos;active&apos;.</summary>
            [QueryParameter("filters%5Bactive%5D")]
            public bool? Filtersactive { get; set; }
            /// <summary>Return item(s) with the specified IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bid%5D")]
            public int?[]? Filtersid { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bid%5D")]
            public int?[] Filtersid { get; set; }
#endif
            /// <summary>Return item(s) within a specific updated at iso8601 datetime range</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bupdated_at%5D")]
            public string? FiltersupdatedAt { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bupdated_at%5D")]
            public string FiltersupdatedAt { get; set; }
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
