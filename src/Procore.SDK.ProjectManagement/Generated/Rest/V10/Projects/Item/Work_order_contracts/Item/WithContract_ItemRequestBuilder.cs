// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.Compliance;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.Compliance_documents;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\work_order_contracts\{contract_id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithContract_ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The compliance property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.Compliance.ComplianceRequestBuilder Compliance
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.Compliance.ComplianceRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The compliance_documents property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.Compliance_documents.Compliance_documentsRequestBuilder Compliance_documents
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.Compliance_documents.Compliance_documentsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.WithContract_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithContract_ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/work_order_contracts/{contract_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Work_order_contracts.Item.WithContract_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithContract_ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/work_order_contracts/{contract_id}", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
