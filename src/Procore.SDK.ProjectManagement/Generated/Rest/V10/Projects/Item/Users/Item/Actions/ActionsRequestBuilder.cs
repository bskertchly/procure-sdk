// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.Add;
using Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.Remove;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\users\{id}\actions
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ActionsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The add property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.Add.AddRequestBuilder Add
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.Add.AddRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The remove property</summary>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.Remove.RemoveRequestBuilder Remove
        {
            get => new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.Remove.RemoveRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.ActionsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ActionsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/users/{id}/actions", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Users.Item.Actions.ActionsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ActionsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/users/{id}/actions", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
