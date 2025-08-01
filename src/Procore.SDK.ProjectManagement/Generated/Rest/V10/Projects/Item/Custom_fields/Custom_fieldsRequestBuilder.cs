// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\custom_fields
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Custom_fieldsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Procore.SDK.ProjectManagement.rest.v10.projects.item.custom_fields.item collection</summary>
        /// <param name="position">Tool name identifier</param>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.WithTool_nameItemRequestBuilder"/></returns>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.WithTool_nameItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("tool_name", position);
                return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.WithTool_nameItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Custom_fieldsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Custom_fieldsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/custom_fields", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Custom_fieldsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Custom_fieldsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/custom_fields", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
