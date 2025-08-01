// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk;
using Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_add;
using Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_remove;
using Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_remove_project_details;
using Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_update_project_details;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Users
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\users
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class UsersRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The bulk property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk.BulkRequestBuilder Bulk
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk.BulkRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The bulk_add property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_add.Bulk_addRequestBuilder Bulk_add
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_add.Bulk_addRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The bulk_remove property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_remove.Bulk_removeRequestBuilder Bulk_remove
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_remove.Bulk_removeRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The bulk_remove_project_details property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_remove_project_details.Bulk_remove_project_detailsRequestBuilder Bulk_remove_project_details
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_remove_project_details.Bulk_remove_project_detailsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The bulk_update_project_details property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_update_project_details.Bulk_update_project_detailsRequestBuilder Bulk_update_project_details
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.Bulk_update_project_details.Bulk_update_project_detailsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.UsersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public UsersRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/users", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Users.UsersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public UsersRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/users", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
