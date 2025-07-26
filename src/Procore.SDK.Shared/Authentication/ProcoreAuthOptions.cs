using System;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// Configuration options for Procore OAuth 2.0 authentication
/// </summary>
public class ProcoreAuthOptions
{
    /// <summary>
    /// OAuth 2.0 client ID provided by Procore
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// OAuth 2.0 client secret provided by Procore
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Redirect URI registered with Procore for OAuth callbacks
    /// </summary>
    public string RedirectUri { get; set; } = string.Empty;

    /// <summary>
    /// OAuth 2.0 scopes to request access for
    /// </summary>
    public string[] Scopes { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Procore OAuth 2.0 authorization endpoint
    /// </summary>
    public Uri AuthorizationEndpoint { get; set; } = new("https://app.procore.com/oauth/authorize");

    /// <summary>
    /// Procore OAuth 2.0 token endpoint
    /// </summary>
    public Uri TokenEndpoint { get; set; } = new("https://api.procore.com/oauth/token");

    /// <summary>
    /// Time margin before token expiration to trigger automatic refresh
    /// </summary>
    public TimeSpan TokenRefreshMargin { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to use PKCE (Proof Key for Code Exchange) for additional security
    /// </summary>
    public bool UsePkce { get; set; } = true;
}