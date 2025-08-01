// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Change_history;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Item;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Potential_timesheet_creator_ids;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Potential_timesheet_creators;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Scoped_cost_codes;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Signatures;
using Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Update_approval;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{project_id}\timesheets
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class TimesheetsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The change_history property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Change_history.Change_historyRequestBuilder Change_history
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Change_history.Change_historyRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The potential_timesheet_creator_ids property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Potential_timesheet_creator_ids.Potential_timesheet_creator_idsRequestBuilder Potential_timesheet_creator_ids
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Potential_timesheet_creator_ids.Potential_timesheet_creator_idsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The potential_timesheet_creators property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Potential_timesheet_creators.Potential_timesheet_creatorsRequestBuilder Potential_timesheet_creators
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Potential_timesheet_creators.Potential_timesheet_creatorsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The scoped_cost_code_ids property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder Scoped_cost_code_ids
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Scoped_cost_code_ids.Scoped_cost_code_idsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The scoped_cost_codes property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Scoped_cost_codes.Scoped_cost_codesRequestBuilder Scoped_cost_codes
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Scoped_cost_codes.Scoped_cost_codesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The signatures property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Signatures.SignaturesRequestBuilder Signatures
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Signatures.SignaturesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The update_approval property</summary>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Update_approval.Update_approvalRequestBuilder Update_approval
        {
            get => new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Update_approval.Update_approvalRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Procore.SDK.FieldProductivity.rest.v10.projects.item.timesheets.item collection</summary>
        /// <param name="position">ID</param>
        /// <returns>A <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Item.TimesheetsItemRequestBuilder"/></returns>
        public global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Item.TimesheetsItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("id", position);
                return new global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.Item.TimesheetsItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.TimesheetsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public TimesheetsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{project_id}/timesheets", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.FieldProductivity.Rest.V10.Projects.Item.Timesheets.TimesheetsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public TimesheetsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{project_id}/timesheets", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
