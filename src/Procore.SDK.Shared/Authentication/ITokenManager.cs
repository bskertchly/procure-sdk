using System;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// Manages OAuth 2.0 access tokens including storage, retrieval, and automatic refresh
/// </summary>
public interface ITokenManager
{
    /// <summary>
    /// Gets the current access token, automatically refreshing if needed
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>The current access token or null if no token is available</returns>
    Task<AccessToken?> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the current access token using the refresh token
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>The new refreshed access token</returns>
    /// <exception cref="InvalidOperationException">Thrown when no refresh token is available</exception>
    Task<AccessToken> RefreshTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores an access token for later use
    /// </summary>
    /// <param name="token">The access token to store</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    Task StoreTokenAsync(AccessToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the stored access token
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    Task ClearTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Event raised when a token is refreshed
    /// </summary>
    event EventHandler<TokenRefreshedEventArgs>? TokenRefreshed;
}

/// <summary>
/// Represents an OAuth 2.0 access token with expiration and refresh information
/// </summary>
/// <param name="Token">The access token value</param>
/// <param name="TokenType">The token type (typically "Bearer")</param>
/// <param name="ExpiresAt">When the token expires</param>
/// <param name="RefreshToken">The refresh token used to obtain new access tokens</param>
/// <param name="Scopes">The scopes granted to this token</param>
public record AccessToken(
    string Token,
    string TokenType,
    DateTimeOffset ExpiresAt,
    string? RefreshToken = null,
    string[]? Scopes = null);

/// <summary>
/// Event arguments for the TokenRefreshed event
/// </summary>
public class TokenRefreshedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the new access token that was obtained.
    /// </summary>
    public AccessToken NewToken { get; }

    /// <summary>
    /// Gets the previous access token that was replaced (may be null).
    /// </summary>
    public AccessToken? OldToken { get; }

    /// <summary>
    /// Creates new TokenRefreshedEventArgs
    /// </summary>
    /// <param name="newToken">The new access token</param>
    /// <param name="oldToken">The previous access token (optional)</param>
    public TokenRefreshedEventArgs(AccessToken newToken, AccessToken? oldToken = null)
    {
        NewToken = newToken;
        OldToken = oldToken;
    }
}