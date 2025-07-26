using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Extensions;

/// <summary>
/// Empty authentication provider for Kiota when authentication is handled by HttpMessageHandler
/// </summary>
internal sealed class EmptyAuthenticationProvider : IAuthenticationProvider
{
    /// <summary>
    /// Authenticates the request - no-op since authentication is handled by ProcoreAuthHandler
    /// </summary>
    /// <param name="request">The request to authenticate</param>
    /// <param name="additionalAuthenticationContext">Additional authentication context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A completed task</returns>
    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        // No-op - authentication is handled by ProcoreAuthHandler
        return Task.CompletedTask;
    }
}