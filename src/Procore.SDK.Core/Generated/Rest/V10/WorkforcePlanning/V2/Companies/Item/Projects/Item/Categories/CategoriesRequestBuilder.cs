// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\workforce-planning\v2\companies\{company_id}\projects\{project_id}\categories
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class CategoriesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.Core.rest.v10.workforcePlanning.v2.companies.item.projects.item.categories.item collection</summary>
        /// <param name="position">Unique identifier for the Category.</param>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.Item.WithCategory_ItemRequestBuilder"/></returns>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.Item.WithCategory_ItemRequestBuilder this[Guid position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("category_id", position);
                return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.Item.WithCategory_ItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public CategoriesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/projects/{project_id}/categories", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public CategoriesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/projects/{project_id}/categories", rawUrl)
        {
        }
        /// <summary>
        /// Add a new Category to a Project. Subcategories can also be provided. A successful response returns the new Category UUID.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostResponse"/></returns>
        /// <param name="body">Request body schema for adding a Category to a Project.</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.Categories401Error">When receiving a 401 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostResponse?> PostAsync(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostResponse> PostAsync(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.Categories401Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Add a new Category to a Project. Subcategories can also be provided. A successful response returns the new Category UUID.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">Request body schema for adding a Category to a Project.</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Categories.CategoriesRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
