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
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\document_custom_tags\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Document_custom_tagsItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Document_custom_tagsItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/document_custom_tags/{id}?document_id={document_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Document_custom_tagsItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/document_custom_tags/{id}?document_id={document_id}", rawUrl)
        {
        }
        /// <summary>
        /// Delete a Custom Tag From a specified Folder or File
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsDeleteResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsDeleteResponse?> DeleteAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder.Document_custom_tagsItemRequestBuilderDeleteQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsDeleteResponse> DeleteAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder.Document_custom_tagsItemRequestBuilderDeleteQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tags5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsDeleteResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsDeleteResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Delete a Custom Tag From a specified Folder or File
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder.Document_custom_tagsItemRequestBuilderDeleteQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder.Document_custom_tagsItemRequestBuilderDeleteQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.DELETE, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Document_custom_tags.Item.Document_custom_tagsItemRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Delete a Custom Tag From a specified Folder or File
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Document_custom_tagsItemRequestBuilderDeleteQueryParameters 
        {
            /// <summary>ID of the Folder or File to remove the Custom Tag from</summary>
            [QueryParameter("document_id")]
            public int? DocumentId { get; set; }
        }
    }
}
#pragma warning restore CS0618
