// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.User_options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\custom_fields\{tool_name}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithTool_nameItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The user_options property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.User_options.User_optionsRequestBuilder User_options
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.User_options.User_optionsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.WithTool_nameItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithTool_nameItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/custom_fields/{tool_name}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Custom_fields.Item.WithTool_nameItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithTool_nameItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/custom_fields/{tool_name}", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
