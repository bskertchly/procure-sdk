// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email
{
    /// <summary>
    /// Builds and executes requests for operations under \rest\v1.0\projects\{-id}\drawing_revision_emails\{id}\send_email
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Send_emailRequestBuilder : BaseRequestBuilder
    {
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Send_emailRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/drawing_revision_emails/{id}/send_email", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Send_emailRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/rest/v1.0/projects/{%2Did}/drawing_revision_emails/{id}/send_email", rawUrl)
        {
        }
        /// <summary>
        /// Sends an email with an associated Drawing Revision. The text of theemail and recipients are specified in the request body.
        /// </summary>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email400Error">When receiving a 400 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email401Error">When receiving a 401 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email403Error">When receiving a 403 status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email4XXError">When receiving a 4XX status code</exception>
        /// <exception cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email5XXError">When receiving a 5XX status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task PostAsync(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email400Error.CreateFromDiscriminatorValue },
                { "401", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email401Error.CreateFromDiscriminatorValue },
                { "403", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email403Error.CreateFromDiscriminatorValue },
                { "4XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email4XXError.CreateFromDiscriminatorValue },
                { "5XX", global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_email5XXError.CreateFromDiscriminatorValue },
            };
            await RequestAdapter.SendNoContentAsync(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Sends an email with an associated Drawing Revision. The text of theemail and recipients are specified in the request body.
        /// </summary>
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailPostRequestBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = new RequestInformation(Method.POST, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json");
            requestInfo.SetContentFromParsable(RequestAdapter, "application/json", body);
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Drawing_revision_emails.Item.Send_email.Send_emailRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
