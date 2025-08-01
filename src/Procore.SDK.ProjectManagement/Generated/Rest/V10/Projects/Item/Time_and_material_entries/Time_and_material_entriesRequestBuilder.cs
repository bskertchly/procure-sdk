// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Bulk_update;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Configurable_field_sets;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Create_equipment;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Search;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Signatures;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\time_and_material_entries
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Time_and_material_entriesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The bulk_update property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Bulk_update.Bulk_updateRequestBuilder Bulk_update
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Bulk_update.Bulk_updateRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The configurable_field_sets property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Configurable_field_sets.Configurable_field_setsRequestBuilder Configurable_field_sets
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Configurable_field_sets.Configurable_field_setsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The create_equipment property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Create_equipment.Create_equipmentRequestBuilder Create_equipment
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Create_equipment.Create_equipmentRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The search property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Search.SearchRequestBuilder Search
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Search.SearchRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The signatures property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Signatures.SignaturesRequestBuilder Signatures
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Signatures.SignaturesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.time_and_material_entries.item collection</summary>
        /// <param name="position">Id of Time And Material Entry</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Item.Time_and_material_entriesItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Time_and_material_entriesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/time_and_material_entries{?page*,per_page*,run_configurable_validations*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Time_and_material_entriesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/time_and_material_entries{?page*,per_page*,run_configurable_validations*}", rawUrl)
        {
        }
        /// <summary>
        /// Return a list of all Time And Material Entry associated with the specified project
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Create a new Time And Material Entry associated with the specified project
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries422Error">When receiving a 422 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostResponse?> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderPostQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostResponse> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderPostQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries422Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entries5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Return a list of all Time And Material Entry associated with the specified project
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Create a new Time And Material Entry associated with the specified project
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderPostQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder.Time_and_material_entriesRequestBuilderPostQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Time_and_material_entries.Time_and_material_entriesRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Return a list of all Time And Material Entry associated with the specified project
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Time_and_material_entriesRequestBuilderGetQueryParameters 
        {
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
        }
        /// <summary>
        /// Create a new Time And Material Entry associated with the specified project
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Time_and_material_entriesRequestBuilderPostQueryParameters 
        {
            /// <summary>If true, validations are run for the corresponding Configurable Field Set.</summary>
            [QueryParameter("run_configurable_validations")]
            public bool? RunConfigurableValidations { get; set; }
        }
    }
}
#pragma warning restore CS0618
