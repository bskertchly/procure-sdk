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
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.1\projects\{project_id}\vendors\{id}\actions\add
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class AddRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AddRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/vendors/{id}/actions/add{?view*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AddRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/vendors/{id}/actions/add{?view*}", rawUrl)
        {
        }
        /// <summary>
        /// Add a specified vendor to a Project from the Company Directory.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddPostResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.Add400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.Add401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.Add403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddPostResponse?> PostAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder.AddRequestBuilderPostQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddPostResponse> PostAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder.AddRequestBuilderPostQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToPostRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.Add400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.Add401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.Add403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddPostResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Add a specified vendor to a Project from the Company Directory.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder.AddRequestBuilderPostQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder.AddRequestBuilderPostQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.POST, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.AddRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Add a specified vendor to a Project from the Company Directory.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class AddRequestBuilderPostQueryParameters 
        {
            /// <summary>The normal view provides what is shown below.The extended view is the same as the normal view but includes children_count, legal_name, parent, and bidding.The default view is normal.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Vendors.Item.Actions.Add.PostViewQueryParameterType? View { get; set; }
        }
    }
}
#pragma warning restore CS0618
