// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\drawing_revisions
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Drawing_revisionsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.drawing_revisions.item collection</summary>
        /// <param name="position">ID of the Drawing Revision</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_revision_ItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_revision_ItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("drawing_revision_%2Did", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Item.Drawing_revision_ItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Drawing_revisionsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/drawing_revisions{?drawing_area_id*,drawing_discipline_id*,drawing_id*,drawing_set_id*,filters%5Bids%5D,filters%5Bupdated_at%5D*,id,is_reviewed*,page*,per_page*,query*,sort*,view*,with_obsolete*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Drawing_revisionsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/drawing_revisions{?drawing_area_id*,drawing_discipline_id*,drawing_id*,drawing_set_id*,filters%5Bids%5D,filters%5Bupdated_at%5D*,id,is_reviewed*,page*,per_page*,query*,sort*,view*,with_obsolete*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns a list of all Drawing Revisions in the specified Project.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder.Drawing_revisionsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder.Drawing_revisionsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions5XXError.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisions.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Returns a list of all Drawing Revisions in the specified Project.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder.Drawing_revisionsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder.Drawing_revisionsRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.Drawing_revisionsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns a list of all Drawing Revisions in the specified Project.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Drawing_revisionsRequestBuilderGetQueryParameters 
        {
            /// <summary>Filter by Drawing Area</summary>
            [QueryParameter("drawing_area_id")]
            public int? DrawingAreaId { get; set; }
            /// <summary>Filter by Drawing Discipline</summary>
            [QueryParameter("drawing_discipline_id")]
            public int? DrawingDisciplineId { get; set; }
            /// <summary>Filter by Drawing</summary>
            [QueryParameter("drawing_id")]
            public int? DrawingId { get; set; }
            /// <summary>Filter by Drawing Set.To retreive revisions from current set add `drawing_set_id=current_set` to query</summary>
            [QueryParameter("drawing_set_id")]
            public int? DrawingSetId { get; set; }
            /// <summary>Filter by Drawing Revisions IDTo request specific drawing revision ids add `filters[ids]=[1,2,3]` to filters</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bids%5D")]
            public int?[]? Filtersids { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bids%5D")]
            public int?[] Filtersids { get; set; }
#endif
            /// <summary>Return item(s) updated within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bupdated_at%5D")]
            public Date? FiltersupdatedAt { get; set; }
            /// <summary>Filter by Drawing Revision IDTo request specific drawing revision ids add `id[]=42&amp;id[]=43` to query</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("id")]
            public int?[]? Id { get; set; }
#nullable restore
#else
            [QueryParameter("id")]
            public int?[] Id { get; set; }
#endif
            /// <summary>Filter by `reviewed` status</summary>
            [QueryParameter("is_reviewed")]
            public bool? IsReviewed { get; set; }
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
            /// <summary>Filter by custom query</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("query")]
            public string? Query { get; set; }
#nullable restore
#else
            [QueryParameter("query")]
            public string Query { get; set; }
#endif
            /// <summary>Sort by field</summary>
            [QueryParameter("sort")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.GetSortQueryParameterType? Sort { get; set; }
            /// <summary>Defines the type of view returned. Must be one of &apos;only_pdf_urls&apos;, &apos;only_ids&apos;, &apos;web_index&apos;, &apos;extended_coordinates&apos;, &apos;extended_files&apos;, &apos;extended_dpi&apos; or &apos;android&apos;.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revisions.GetViewQueryParameterType? View { get; set; }
            /// <summary>Include obsolete drawing revisions. Obsolete drawing revisions are filtered by default.</summary>
            [QueryParameter("with_obsolete")]
            public bool? WithObsolete { get; set; }
        }
    }
}
#pragma warning restore CS0618
