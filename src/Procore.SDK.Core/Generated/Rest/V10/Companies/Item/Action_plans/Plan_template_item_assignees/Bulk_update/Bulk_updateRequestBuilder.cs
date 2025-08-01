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
namespace Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\companies\{company_id}\action_plans\plan_template_item_assignees\bulk_update
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Bulk_updateRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Bulk_updateRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/action_plans/plan_template_item_assignees/bulk_update{?completion_mode*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Bulk_updateRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/companies/{company_id}/action_plans/plan_template_item_assignees/bulk_update{?completion_mode*}", rawUrl)
        {
        }
        /// <summary>
        /// Updates multiple Action Plan Assignees for the selected action plan items
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update&gt;</returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update>?> PatchAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updatePatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder.Bulk_updateRequestBuilderPatchQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update>> PatchAsync(global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updatePatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder.Bulk_updateRequestBuilderPatchQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update404Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update5XXError.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update>(requestInfo, global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_update.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Updates multiple Action Plan Assignees for the selected action plan items
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updatePatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder.Bulk_updateRequestBuilderPatchQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updatePatchRequestBody body, Action<RequestConfiguration<global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder.Bulk_updateRequestBuilderPatchQueryParameters>> requestConfiguration = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = new RequestInformation(Method.PATCH, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            requestInfo.SetContentFromParsable(RequestAdapter, "application/json", body);
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.Bulk_updateRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Updates multiple Action Plan Assignees for the selected action plan items
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Bulk_updateRequestBuilderPatchQueryParameters 
        {
            /// <summary>Whether to update what can be or nothing if one can not be updated. Defaults to &quot;all_or_nothing&quot;</summary>
            [QueryParameter("completion_mode")]
            public global::Procore.SDK.Core.Rest.V10.Companies.Item.Action_plans.Plan_template_item_assignees.Bulk_update.PatchCompletion_modeQueryParameterType? CompletionMode { get; set; }
        }
    }
}
#pragma warning restore CS0618
