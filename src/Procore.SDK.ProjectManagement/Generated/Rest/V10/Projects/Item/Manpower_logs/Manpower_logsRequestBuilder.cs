// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Contact_options;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Daily_totals;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Item;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Vendor_options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\manpower_logs
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Manpower_logsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The contact_options property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Contact_options.Contact_optionsRequestBuilder Contact_options
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Contact_options.Contact_optionsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The daily_totals property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Daily_totals.Daily_totalsRequestBuilder Daily_totals
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Daily_totals.Daily_totalsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The vendor_options property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Vendor_options.Vendor_optionsRequestBuilder Vendor_options
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Vendor_options.Vendor_optionsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.manpower_logs.item collection</summary>
        /// <param name="position">Manpower Log ID</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Item.Manpower_logsItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Item.Manpower_logsItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Item.Manpower_logsItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Manpower_logsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/manpower_logs{?end_date*,filters%5Bcreated_by_id%5D,filters%5Blocation_id%5D,filters%5Bsearch%5D*,filters%5Bstatus%5D*,filters%5Bvendor_id%5D,log_date*,page*,per_page*,run_configurable_validations*,start_date*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Manpower_logsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/manpower_logs{?end_date*,filters%5Bcreated_by_id%5D,filters%5Blocation_id%5D,filters%5Bsearch%5D*,filters%5Bstatus%5D*,filters%5Bvendor_id%5D,log_date*,page*,per_page*,run_configurable_validations*,start_date*}", rawUrl)
        {
        }
        /// <summary>
        /// Returns all approved Manpower Logs for the current date.See [Working with Daily Logs](https://developers.procore.com/documentation/daily-logs) for information on filtering the response using the log\_date, start\_date, and end\_date parameters. Note that if none of the date parameters are provided in the call, only logs from the current date are returned.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs403Error">When receiving a 403 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs403Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Creates single Manpower Log.#### See - [Daily Log guide](https://developers.procore.com/documentation/daily-logs) - for additional info on* Attachments* Locations
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs422Error">When receiving a 422 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostResponse?> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderPostQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostResponse> PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderPostQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs400Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs403Error.CreateFromDiscriminatorValue },
                { "422", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logs422Error.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostResponse>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostResponse.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns all approved Manpower Logs for the current date.See [Working with Daily Logs](https://developers.procore.com/documentation/daily-logs) for information on filtering the response using the log\_date, start\_date, and end\_date parameters. Note that if none of the date parameters are provided in the call, only logs from the current date are returned.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderGetQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            return requestInfo;
        }
        /// <summary>
        /// Creates single Manpower Log.#### See - [Daily Log guide](https://developers.procore.com/documentation/daily-logs) - for additional info on* Attachments* Locations
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderPostQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsPostRequestBody body, Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder.Manpower_logsRequestBuilderPostQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Manpower_logs.Manpower_logsRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Returns all approved Manpower Logs for the current date.See [Working with Daily Logs](https://developers.procore.com/documentation/daily-logs) for information on filtering the response using the log\_date, start\_date, and end\_date parameters. Note that if none of the date parameters are provided in the call, only logs from the current date are returned.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Manpower_logsRequestBuilderGetQueryParameters 
        {
            /// <summary>End date of specific logs desired in YYYY-MM-DD format (use together with start_date)</summary>
            [QueryParameter("end_date")]
            public Date? EndDate { get; set; }
            /// <summary>Returns item(s) created by the specified User IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bcreated_by_id%5D")]
            public int?[]? FilterscreatedById { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bcreated_by_id%5D")]
            public int?[] FilterscreatedById { get; set; }
#endif
            /// <summary>Return item(s) with the specified Location IDs.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Blocation_id%5D")]
            public int?[]? FilterslocationId { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Blocation_id%5D")]
            public int?[] FilterslocationId { get; set; }
#endif
            /// <summary>Returns item(s) matching the specified search query string.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bsearch%5D")]
            public string? Filterssearch { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bsearch%5D")]
            public string Filterssearch { get; set; }
#endif
            /// <summary>Filter on status for &quot;pending&quot; or &quot;approved&quot; or &quot;all&quot;</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("filters%5Bstatus%5D")]
            public string? Filtersstatus { get; set; }
#nullable restore
#else
            [QueryParameter("filters%5Bstatus%5D")]
            public string Filtersstatus { get; set; }
#endif
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
            /// <summary>Date of specific logs desired in YYYY-MM-DD format</summary>
            [QueryParameter("log_date")]
            public Date? LogDate { get; set; }
            /// <summary>Page</summary>
            [QueryParameter("page")]
            public int? Page { get; set; }
            /// <summary>Elements per page</summary>
            [QueryParameter("per_page")]
            public int? PerPage { get; set; }
            /// <summary>Start date of specific logs desired in YYYY-MM-DD format (use together with end_date)</summary>
            [QueryParameter("start_date")]
            public Date? StartDate { get; set; }
        }
        /// <summary>
        /// Creates single Manpower Log.#### See - [Daily Log guide](https://developers.procore.com/documentation/daily-logs) - for additional info on* Attachments* Locations
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class Manpower_logsRequestBuilderPostQueryParameters 
        {
            /// <summary>If true, validations are run for the corresponding Configurable Field Set.</summary>
            [QueryParameter("run_configurable_validations")]
            public bool? RunConfigurableValidations { get; set; }
        }
    }
}
#pragma warning restore CS0618
