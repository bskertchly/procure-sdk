// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10;
using Procore.SDK.ProjectManagement.Rest.V11;
using Procore.SDK.ProjectManagement.Rest.V20;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest
{
    /// <summary>
    /// Builds and executes requests for operations under \rest
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class RestRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The v10 property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.V10RequestBuilder V10
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.V10RequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The v11 property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V11.V11RequestBuilder V11
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V11.V11RequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The v20 property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V20.V20RequestBuilder V20
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V20.V20RequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.RestRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public RestRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.RestRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public RestRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
