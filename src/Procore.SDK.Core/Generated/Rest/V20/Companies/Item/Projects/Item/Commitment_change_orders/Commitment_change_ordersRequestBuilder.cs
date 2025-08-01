// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\commitment_change_orders
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Commitment_change_ordersRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.Core.rest.v20.companies.item.projects.item.commitment_change_orders.item collection</summary>
        /// <param name="position">Unique identifier for the Commitment Change Order.</param>
        /// <returns>A <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders.Item.WithCommitment_change_order_ItemRequestBuilder"/></returns>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders.Item.WithCommitment_change_order_ItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("commitment_change_order_id", position);
                return new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders.Item.WithCommitment_change_order_ItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders.Commitment_change_ordersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Commitment_change_ordersRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/commitment_change_orders", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Commitment_change_orders.Commitment_change_ordersRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Commitment_change_ordersRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/commitment_change_orders", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
