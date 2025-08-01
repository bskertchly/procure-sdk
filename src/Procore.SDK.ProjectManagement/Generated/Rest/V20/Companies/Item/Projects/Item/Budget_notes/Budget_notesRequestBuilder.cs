// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\budget_notes
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Budget_notesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v20.companies.item.projects.item.budget_notes.item collection</summary>
        /// <param name="position">ID of the WBS code</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes.Item.WithWbs_code_ItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes.Item.WithWbs_code_ItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("wbs_code_id", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes.Item.WithWbs_code_ItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes.Budget_notesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Budget_notesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/budget_notes", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V20.Companies.Item.Projects.Item.Budget_notes.Budget_notesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Budget_notesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/budget_notes", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
