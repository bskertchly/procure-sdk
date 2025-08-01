// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Inactive;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\users
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class UsersRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The inactive property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Inactive.InactiveRequestBuilder Inactive
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Inactive.InactiveRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The pdf property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Pdf.PdfRequestBuilder Pdf
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Pdf.PdfRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.users.item collection</summary>
        /// <param name="position">ID of the user</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.UsersItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.UsersItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.UsersItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public UsersRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/users{?filters%5Bcreated_at%5D*,filters%5Bemployee%5D*,filters%5Bid%5D*,filters%5Borigin_id%5D*,filters%5Bpermission_template%5D*,filters%5Bsearch%5D*,filters%5Btrade_id%5D%5B%5D*,filters%5Bupdated_at%5D*,filters%5Bvendor_id%5D,page*,per_page*,run_configurable_validations*,sort*,view*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public UsersRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/users{?filters%5Bcreated_at%5D*,filters%5Bemployee%5D*,filters%5Bid%5D*,filters%5Borigin_id%5D*,filters%5Bpermission_template%5D*,filters%5Bsearch%5D*,filters%5Btrade_id%5D%5B%5D*,filters%5Bupdated_at%5D*,filters%5Bvendor_id%5D,page*,per_page*,run_configurable_validations*,sort*,view*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns a list of active users associated with a project.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Creates a new user in the specified project.#### Country and State codesThe `country_code` and `state_code` parameter values must conform to the ISO-3166 Alpha-2 specification.See [Working with Country Codes](/documentation/country-codes) for additional information.#### Created ResponseFor null values, the key won&apos;t be returned
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostResponse?> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderPostQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostResponse> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderPostQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Users403Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns a list of active users associated with a project.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Creates a new user in the specified project.#### Country and State codesThe `country_code` and `state_code` parameter values must conform to the ISO-3166 Alpha-2 specification.See [Working with Country Codes](/documentation/country-codes) for additional information.#### Created ResponseFor null values, the key won&apos;t be returned
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderPostQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.UsersRequestBuilderPostQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Composed type wrapper for classes <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember1"/>, <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember2"/>
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Users : IComposedTypeWrapper, IParsable
        {
            /// <summary>Composed type representation for type <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember1"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember1? UsersMember1 { get; set; }
#nullable restore
#else
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember1 UsersMember1 { get; set; }
#endif
            /// <summary>Composed type representation for type <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember2"/></summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember2? UsersMember2 { get; set; }
#nullable restore
#else
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember2 UsersMember2 { get; set; }
#endif
            /// <summary>
            /// Creates a new instance of the appropriate class based on discriminator value
            /// </summary>
            /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users"/></returns>
            /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
            public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users CreateFromDiscriminatorValue(IParseNode parseNode)
            {
                _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
                var mappingValue = parseNode.GetChildNode("")?.GetStringValue();
                var result = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersRequestBuilder.Users();
                if("".Equals(mappingValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.UsersMember1 = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember1();
                }
                else if("".Equals(mappingValue, StringComparison.OrdinalIgnoreCase))
                {
                    result.UsersMember2 = new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember2();
                }
                return result;
            }
            /// <summary>
            /// The deserialization information for the current model
            /// </summary>
            /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
            public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
            {
                if(UsersMember1 != null)
                {
                    return UsersMember1.GetFieldDeserializers();
                }
                else if(UsersMember2 != null)
                {
                    return UsersMember2.GetFieldDeserializers();
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
                if(UsersMember1 != null)
                {
                    writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember1>(null, UsersMember1);
                }
                else if(UsersMember2 != null)
                {
                    writer.WriteObjectValue<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.UsersMember2>(null, UsersMember2);
                }
            }
        }
        /// <summary>
        /// Returns a list of active users associated with a project.See [Filtering on List Actions](https://developers.procore.com/documentation/filtering-on-list-actions) for information on using the filtering capabilities provided by this endpoint.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class UsersRequestBuilderGetQueryParameters 
        {
            /// <summary>Return item(s) created within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bcreated_at%5D")]
            public Date? FilterscreatedAt { get; set; }
            /// <summary>Returns users whose is_employee attribute matches the parameter.</summary>
            [QueryParameter("filters%5Bemployee%5D")]
            public bool? Filtersemployee { get; set; }
            /// <summary>Returns users whose id attribute matches the parameter.</summary>
            [QueryParameter("filters%5Bid%5D")]
            public int? Filtersid { get; set; }
            /// <summary>Origin ID. Returns item(s) with the specified Origin ID.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Borigin_id%5D")]
            public string? FiltersoriginId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Borigin_id%5D")]
            public string FiltersoriginId { get; set; }
#endif
            /// <summary>Permission Template ID. Returns item(s) assiociated with the specified Permission Template ID.</summary>
            [QueryParameter("filters%5Bpermission_template%5D")]
            public int? FilterspermissionTemplate { get; set; }
            /// <summary>Returns users where the search string matches the user&apos;s first name, last name, email address, keywords, job title, or company name</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bsearch%5D")]
            public string? Filterssearch { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bsearch%5D")]
            public string Filterssearch { get; set; }
#endif
            /// <summary>Returns users whose vendor record is associated with the specified trade id(s).</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Btrade_id%5D%5B%5D")]
            public int?[]? FilterstradeId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Btrade_id%5D%5B%5D")]
            public int?[] FilterstradeId { get; set; }
#endif
            /// <summary>Return item(s) last updated within the specified ISO 8601 datetime range.Formats:`YYYY-MM-DD`...`YYYY-MM-DD` - Date`YYYY-MM-DDTHH:MM:SSZ`...`YYYY-MM-DDTHH:MM:SSZ` - DateTime with UTC Offset`YYYY-MM-DDTHH:MM:SS+XX:00`...`YYYY-MM-DDTHH:MM:SS+XX:00` - Datetime with Custom Offset</summary>
            [QueryParameter("filters%5Bupdated_at%5D")]
            public Date? FiltersupdatedAt { get; set; }
            /// <summary>Return item(s) with the specified Vendor IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bvendor_id%5D")]
            public int?[]? FiltersvendorId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bvendor_id%5D")]
            public int?[] FiltersvendorId { get; set; }
#endif
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
            /// <summary>Returns items with the specified sort.</summary>
            [QueryParameter("sort")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.GetSortQueryParameterType? Sort { get; set; }
            /// <summary>Specifies which view of the resource to return (which attributes should be present in the response). Users without read permissions to Directory are limited to the compact view. Otherwise, the default view is normal.</summary>
            [QueryParameter("view")]
            public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.GetViewQueryParameterType? View { get; set; }
        }
        /// <summary>
        /// Creates a new user in the specified project.#### Country and State codesThe `country_code` and `state_code` parameter values must conform to the ISO-3166 Alpha-2 specification.See [Working with Country Codes](/documentation/country-codes) for additional information.#### Created ResponseFor null values, the key won&apos;t be returned
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class UsersRequestBuilderPostQueryParameters 
        {
            /// <summary>If true, validations are run for the corresponding Configurable Field Set.</summary>
            [QueryParameter("run_configurable_validations")]
            public bool? RunConfigurableValidations { get; set; }
        }
    }
}
#pragma warning restore CS0618
