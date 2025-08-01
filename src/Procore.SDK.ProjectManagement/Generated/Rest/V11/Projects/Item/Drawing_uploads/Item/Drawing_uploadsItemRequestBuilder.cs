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
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.1\projects\{project_id}\drawing_uploads\{id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Drawing_uploadsItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Drawing_uploadsItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/drawing_uploads/{id}{?view*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Drawing_uploadsItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/drawing_uploads/{id}{?view*}", rawUrl)
        {
        }
        /// <summary>
        /// Delete an unreviewed Drawing Upload.
        /// </summary>
        /// <returns>A <see cref="Stream"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<Stream?> DeleteAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderDeleteQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<Stream> DeleteAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderDeleteQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendPrimitiveAsync<Stream>(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Get the details of a single Drawing Upload
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploads5XXError.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsGetResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Delete an unreviewed Drawing Upload.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderDeleteQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderDeleteQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.DELETE, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Get the details of a single Drawing Upload
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder.Drawing_uploadsItemRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.Drawing_uploadsItemRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Delete an unreviewed Drawing Upload.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Drawing_uploadsItemRequestBuilderDeleteQueryParameters 
        {
            /// <summary>Specifies the level of detail returned in the response.The &apos;with_drawing_log_imports&apos; view provides additional data as shown below.The &apos;normal&apos; view is the default if not specified.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.DeleteViewQueryParameterType? View { get; set; }
        }
        /// <summary>
        /// Get the details of a single Drawing Upload
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Drawing_uploadsItemRequestBuilderGetQueryParameters 
        {
            /// <summary>Specifies the level of detail returned in the response.The &apos;with_drawing_log_imports&apos; view provides additional data as shown below.The &apos;normal&apos; view is the default if not specified.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Drawing_uploads.Item.GetViewQueryParameterType? View { get; set; }
        }
    }
}
#pragma warning restore CS0618
