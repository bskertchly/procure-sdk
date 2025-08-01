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
namespace Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\observations\items\next_available_number
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Next_available_numberRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Next_available_numberRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/observations/items/next_available_number?project_id={project_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Next_available_numberRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/observations/items/next_available_number?project_id={project_id}", rawUrl)
        {
        }
        /// <summary>
        /// Returns the next available number for an observation item in the current Project.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberGetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_number400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_number401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_number403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberGetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder.Next_available_numberRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberGetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder.Next_available_numberRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_number400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_number401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_number403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberGetResponse>(requestInfo, global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberGetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns the next available number for an observation item in the current Project.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder.Next_available_numberRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder.Next_available_numberRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.QualitySafety.Rest.V10.Observations.Items.Next_available_number.Next_available_numberRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns the next available number for an observation item in the current Project.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Next_available_numberRequestBuilderGetQueryParameters 
        {
            /// <summary>Unique identifier for the project.</summary>
            [QueryParameter("project_id")]
            public int? ProjectId { get; set; }
        }
    }
}
#pragma warning restore CS0618
