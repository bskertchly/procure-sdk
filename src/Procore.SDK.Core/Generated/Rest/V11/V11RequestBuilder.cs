// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.Core.Rest.V11.Companies;
using Procore.SDK.Core.Rest.V11.Users;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.Core.Rest.V11
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.1
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class V11RequestBuilder : BaseRequestBuilder
    {
        /// <summary>The companies property</summary>
        public global::Procore.SDK.Core.Rest.V11.Companies.CompaniesRequestBuilder Companies
        {
            get => new global::Procore.SDK.Core.Rest.V11.Companies.CompaniesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The users property</summary>
        public global::Procore.SDK.Core.Rest.V11.Users.UsersRequestBuilder Users
        {
            get => new global::Procore.SDK.Core.Rest.V11.Users.UsersRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V11.V11RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public V11RequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.Core.Rest.V11.V11RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public V11RequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.1", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
