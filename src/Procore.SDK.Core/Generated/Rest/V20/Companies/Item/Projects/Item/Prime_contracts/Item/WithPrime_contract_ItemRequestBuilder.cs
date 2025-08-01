// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items;
using Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v2.0\companies\{company_id}\projects\{project_id}\prime_contracts\{prime_contract_id}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithPrime_contract_ItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The line_items property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Line_itemsRequestBuilder Line_items
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Line_items.Line_itemsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The pdf property</summary>
        public global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Pdf.PdfRequestBuilder Pdf
        {
            get => new global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.Pdf.PdfRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.WithPrime_contract_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithPrime_contract_ItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/prime_contracts/{prime_contract_id}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V20.Companies.Item.Projects.Item.Prime_contracts.Item.WithPrime_contract_ItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithPrime_contract_ItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v2.0/companies/{company_id}/projects/{project_id}/prime_contracts/{prime_contract_id}", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
