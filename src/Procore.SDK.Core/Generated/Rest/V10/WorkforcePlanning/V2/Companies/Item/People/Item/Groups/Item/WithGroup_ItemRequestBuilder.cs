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
namespace Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\workforce-planning\v2\companies\{company_id}\people\{person_id}\groups\{group_id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithGroup_ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithGroup_ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/people/{person_id}/groups/{group_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithGroup_ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/people/{person_id}/groups/{group_id}", rawUrl)
        {
        }
        /// <summary>
        /// Removes a person from a given group. Note that when removing the last group_id, the Group IDs can not be empty for an assignable resource or a non-admin person. Therefore, the removing the last group id from an assignable resource or a non-admin person is not allowed.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_DeleteResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_DeleteResponse?> DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_DeleteResponse> DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_DeleteResponse>(requestInfo, global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_DeleteResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Removes a person from a given group. Note that when removing the last group_id, the Group IDs can not be empty for an assignable resource or a non-admin person. Therefore, the removing the last group id from an assignable resource or a non-admin person is not allowed.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_ItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_ItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.WorkforcePlanning.V2.Companies.Item.People.Item.Groups.Item.WithGroup_ItemRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
