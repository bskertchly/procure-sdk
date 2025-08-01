// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Possible_assignees;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Presets;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Workflow_managers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\workflows
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WorkflowsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The instances property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.InstancesRequestBuilder Instances
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Instances.InstancesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The possible_assignees property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Possible_assignees.Possible_assigneesRequestBuilder Possible_assignees
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Possible_assignees.Possible_assigneesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The presets property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Presets.PresetsRequestBuilder Presets
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Presets.PresetsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The workflow_managers property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Workflow_managers.Workflow_managersRequestBuilder Workflow_managers
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.Workflow_managers.Workflow_managersRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.WorkflowsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WorkflowsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/workflows", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Workflows.WorkflowsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WorkflowsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/workflows", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
