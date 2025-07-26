using System;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// Interface for storing and retrieving access tokens
/// </summary>
public interface ITokenStorage
{
    /// <summary>
    /// Gets a stored token by key
    /// </summary>
    /// <param name="key">The key to look up the token</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>The stored token or null if not found</returns>
    Task<AccessToken?> GetTokenAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores a token with the given key
    /// </summary>
    /// <param name="key">The key to store the token under</param>
    /// <param name="token">The token to store</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    Task StoreTokenAsync(string key, AccessToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a stored token by key
    /// </summary>
    /// <param name="key">The key of the token to delete</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    Task DeleteTokenAsync(string key, CancellationToken cancellationToken = default);
}