// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations;
using Procore.SDK.Core.Rest.V20.Companies.Item.Budget_views;
using Procore.SDK.Core.Rest.V20.Companies.Item.Company_security_settings;
using Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items;
using Procore.SDK.Core.Rest.V20.Companies.Item.People;
using Procore.SDK.Core.Rest.V20.Companies.Item.Project_status_snapshots;
using Procore.SDK.Core.Rest.V20.Companies.Item.Projects;
using Procore.SDK.Core.Rest.V20.Companies.Item.Support_pins;
using Procore.SDK.Core.Rest.V20.Companies.Item.Uoms;
using Procore.SDK.Core.Rest.V20.Companies.Item.Users;
using Procore.SDK.Core.Rest.V20.Companies.Item.Workflows;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithCompany_ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The async_operations property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder Async_operations
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Async_operations.Async_operationsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The budget_views property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Budget_views.Budget_viewsRequestBuilder Budget_views
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Budget_views.Budget_viewsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The company_security_settings property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Company_security_settings.Company_security_settingsRequestBuilder Company_security_settings
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Company_security_settings.Company_security_settingsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The inspection_template_items property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Inspection_template_itemsRequestBuilder Inspection_template_items
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Inspection_template_items.Inspection_template_itemsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The people property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.People.PeopleRequestBuilder People
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.People.PeopleRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The project_status_snapshots property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Project_status_snapshots.Project_status_snapshotsRequestBuilder Project_status_snapshots
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Project_status_snapshots.Project_status_snapshotsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The projects property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.ProjectsRequestBuilder Projects
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.ProjectsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The support_pins property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Support_pins.Support_pinsRequestBuilder Support_pins
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Support_pins.Support_pinsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The uoms property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Uoms.UomsRequestBuilder Uoms
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Uoms.UomsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The users property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.UsersRequestBuilder Users
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.UsersRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The workflows property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.WorkflowsRequestBuilder Workflows
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Workflows.WorkflowsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.WithCompany_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithCompany_ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.WithCompany_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithCompany_ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
