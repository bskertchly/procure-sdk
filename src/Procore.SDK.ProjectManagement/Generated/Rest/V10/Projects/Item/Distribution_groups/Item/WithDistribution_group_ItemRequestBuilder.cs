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
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\distribution_groups\{distribution_group_id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithDistribution_group_ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithDistribution_group_ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/distribution_groups/{distribution_group_id}{?domain_id*,include_ancestors*,min_ual*,ual*,view*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithDistribution_group_ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/distribution_groups/{distribution_group_id}{?domain_id*,include_ancestors*,min_ual*,ual*,view*}", rawUrl)
        {
        }
        /// <summary>
        /// Delete a Distribution Group associated with the given Project.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_404Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_422Error.CreateFromDiscriminatorValue },
            };
            await RequestAdapter.SendNoContentAsync(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Show detail on a specified Project Distribution Group
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_404Error">When receiving a 404 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_ItemRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_ItemRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_404Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Update a Distribution Group associated with the given Project.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_404Error">When receiving a 404 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchResponse?> PatchAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchResponse> PatchAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPatchRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_403Error.CreateFromDiscriminatorValue },
                { "404", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_404Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Delete a Distribution Group associated with the given Project.
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
        /// Show detail on a specified Project Distribution Group
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_ItemRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_ItemRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Update a Distribution Group associated with the given Project.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPatchRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_PatchRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Composed type wrapper for classes <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember1"/>, <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember2"/>
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class WithDistribution_group_GetResponse : IComposedTypeWrapper, IParsable
        {
            /// <summary>Composed type representation for type <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember1"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember1? WithDistributionGroupGetResponseMember1 { get; set; }
#nullable restore
#else
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember1 WithDistributionGroupGetResponseMember1 { get; set; }
#endif
            /// <summary>Composed type representation for type <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember2"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember2? WithDistributionGroupGetResponseMember2 { get; set; }
#nullable restore
#else
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember2 WithDistributionGroupGetResponseMember2 { get; set; }
#endif
            /// <summary>
            /// Creates a new instance of the appropriate class based on discriminator value
            /// </summary>
            /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse"/></returns>
            /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
            public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse CreateFromDiscriminatorValue(IParseNode parseNode)
            {
                _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
                var mappingValue = parseNode.GetChildNode("")?.GetStringValue();
                var result = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_ItemRequestBuilder.WithDistribution_group_GetResponse();
                if("".Equals(mappingValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.WithDistributionGroupGetResponseMember1 = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember1();
                }
                else if("".Equals(mappingValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.WithDistributionGroupGetResponseMember2 = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember2();
                }
                return result;
            }
            /// <summary>
            /// The deserialization information for the current model
            /// </summary>
            /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
            public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
            {
                if(WithDistributionGroupGetResponseMember1 != null)
                {
                    return WithDistributionGroupGetResponseMember1.GetFieldDeserializers();
                }
                else if(WithDistributionGroupGetResponseMember2 != null)
                {
                    return WithDistributionGroupGetResponseMember2.GetFieldDeserializers();
                }
                return new Dictionary<string, Action<IParseNode>>();
            }
            /// <summary>
            /// Serializes information the current object
            /// </summary>
            /// <param name="writer">Serialization writer to use to serialize this model</param>
            public virtual void Serialize(ISerializationWriter writer)
            {
                _ = writer ?? throw new ArgumentNullException(nameof(writer));
                if(WithDistributionGroupGetResponseMember1 != null)
                {
                    writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember1>(null, WithDistributionGroupGetResponseMember1);
                }
                else if(WithDistributionGroupGetResponseMember2 != null)
                {
                    writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.WithDistribution_group_GetResponseMember2>(null, WithDistributionGroupGetResponseMember2);
                }
            }
        }
        /// <summary>
        /// Show detail on a specified Project Distribution Group
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class WithDistribution_group_ItemRequestBuilderGetQueryParameters 
        {
            /// <summary>Parameter affecting the scope for the Distribution Groups, by default it is the Domain ID of the Submittals Tool. Will return only Distributions Groups who users that have access to the Tool specified by the domain_id. Only applies to requests that also have include_ancestors set to &apos;true&apos;.</summary>
            [QueryParameter("domain_id")]
            public int? DomainId { get; set; }
            /// <summary>Parameter affecting what groups can be returned from this endpoint. When &apos;true&apos;, this endpoint will only return distribution groups with users that match the provided (or default) `domain_id` and `min_ual` / `ual`. Company level distribution groups can be viewed as well if they have users matching the provided (or default) permissions.  If `extended` view is requested, only the users with matching permissions will be returned in the response. When `include_ancestors` is false or absent, only Project level groups can be viewed.</summary>
            [QueryParameter("include_ancestors")]
            public bool? IncludeAncestors { get; set; }
            /// <summary>Parameter affecting the scope for the Distribution Groups, by default it is the &apos;read&apos; user access level. Will return only Distributions Groups who users that have the min ual specified by the &apos;min_ual&apos;. Only applies to requests that also have include_ancestors set to &apos;true&apos;.</summary>
            [QueryParameter("min_ual")]
            public int? MinUal { get; set; }
            /// <summary>Parameter affecting the scope for the Distribution Groups.  Will return only Distributions Groups who users that have the exact ual specified by the &apos;ual&apos;. If provided, this will take precendence over min_ual.  Only applies to requests that also have include_ancestors set to &apos;true&apos;.</summary>
            [QueryParameter("ual")]
            public int? Ual { get; set; }
            /// <summary>Parameter affecting what level of detail will be returned from the endpoint. &apos;extended&apos; will include the users in the distribution group.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Distribution_groups.Item.GetViewQueryParameterType? View { get; set; }
        }
    }
}
#pragma warning restore CS0618
