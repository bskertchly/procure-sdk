// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Counts;
using Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Weather_logs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.1\projects\{project_id}\daily_logs
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Daily_logsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The counts property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Counts.CountsRequestBuilder Counts
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Counts.CountsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The weather_logs property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Weather_logs.Weather_logsRequestBuilder Weather_logs
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Weather_logs.Weather_logsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Daily_logsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Daily_logsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/daily_logs", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V11.Projects.Item.Daily_logs.Daily_logsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Daily_logsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1/projects/{project_id}/daily_logs", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
