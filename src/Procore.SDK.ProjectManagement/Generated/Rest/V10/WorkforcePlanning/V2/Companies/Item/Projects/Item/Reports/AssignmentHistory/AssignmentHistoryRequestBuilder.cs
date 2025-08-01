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
namespace Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\workforce-planning\v2\companies\{company_id}\projects\{project_id}\reports\assignment-history
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class AssignmentHistoryRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AssignmentHistoryRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/projects/{project_id}/reports/assignment-history{?assignmentEnd*,assignmentStart*,cost_code*,duration*,employeeName*,employee_number*,end_time*,jobTitle*,labels*,start_time*}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AssignmentHistoryRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/workforce-planning/v2/companies/{company_id}/projects/{project_id}/reports/assignment-history{?assignmentEnd*,assignmentStart*,cost_code*,duration*,employeeName*,employee_number*,end_time*,jobTitle*,labels*,start_time*}", rawUrl)
        {
        }
        /// <summary>
        /// Fetches the assignment history for a specific Project, returning records of all assignments linked to it.
        /// </summary>
        /// <returns>A List&lt;global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory&gt;</returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory401Error">When receiving a 401 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory>?> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder.AssignmentHistoryRequestBuilderGetQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<List<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory>> GetAsync(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder.AssignmentHistoryRequestBuilderGetQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory401Error.CreateFromDiscriminatorValue },
            };
            var collectionResult = await RequestAdapter.SendCollectionAsync<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory>(requestInfo, global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistory.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
            return collectionResult?.AsList();
        }
        /// <summary>
        /// Fetches the assignment history for a specific Project, returning records of all assignments linked to it.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder.AssignmentHistoryRequestBuilderGetQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder.AssignmentHistoryRequestBuilderGetQueryParameters>> requestConfiguration = default)
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
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Projects.Item.Reports.AssignmentHistory.AssignmentHistoryRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Fetches the assignment history for a specific Project, returning records of all assignments linked to it.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class AssignmentHistoryRequestBuilderGetQueryParameters 
        {
            /// <summary>Whether to include the assignment end date.</summary>
            [QueryParameter("assignmentEnd")]
            public bool? AssignmentEnd { get; set; }
            /// <summary>Whether to include the assignment start date.</summary>
            [QueryParameter("assignmentStart")]
            public bool? AssignmentStart { get; set; }
            /// <summary>Will return the name and UUID of the Cost Code for each assignment.</summary>
            [QueryParameter("cost_code")]
            public bool? CostCode { get; set; }
            /// <summary>Will return a calculated duration for each listed assignment.</summary>
            [QueryParameter("duration")]
            public bool? Duration { get; set; }
            /// <summary>Determines whether the employee&apos;s name should be included in the response. If set to `true`, the response will include the person&apos;s first and last name. Default is `true`.</summary>
            [QueryParameter("employeeName")]
            public bool? EmployeeName { get; set; }
            /// <summary>Filter results by the exact employee number of the Person.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
            [QueryParameter("employee_number")]
            public string? EmployeeNumber { get; set; }
#nullable restore
#else
            [QueryParameter("employee_number")]
            public string EmployeeNumber { get; set; }
#endif
            /// <summary>Will return the daily end time for each assignment.</summary>
            [QueryParameter("end_time")]
            public bool? EndTime { get; set; }
            /// <summary>Whether to include the person&apos;s Job Title.</summary>
            [QueryParameter("jobTitle")]
            public bool? JobTitle { get; set; }
            /// <summary>Will return the name and UUID of the Label for each assignment.</summary>
            [QueryParameter("labels")]
            public bool? Labels { get; set; }
            /// <summary>Will return the daily start time for each assignment.</summary>
            [QueryParameter("start_time")]
            public bool? StartTime { get; set; }
        }
    }
}
#pragma warning restore CS0618
