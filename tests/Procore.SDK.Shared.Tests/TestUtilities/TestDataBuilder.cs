using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Shared.Tests.TestUtilities;

/// <summary>
/// Builder class for creating test data objects with fluent API
/// Provides consistent test data creation across all test projects
/// </summary>
public static class TestDataBuilder
{
    /// <summary>
    /// Creates a builder for AccessToken test data
    /// </summary>
    /// <returns>AccessTokenBuilder instance</returns>
    public static AccessTokenBuilder AccessToken() => new();

    /// <summary>
    /// Creates a builder for ProcoreAuthOptions test data
    /// </summary>
    /// <returns>ProcoreAuthOptionsBuilder instance</returns>
    public static ProcoreAuthOptionsBuilder ProcoreAuthOptions() => new();
}

/// <summary>
/// Builder for AccessToken test objects
/// </summary>
public class AccessTokenBuilder
{
    private string _token = "test-access-token";
    private string _tokenType = "Bearer";
    private DateTimeOffset _expiresAt = DateTimeOffset.UtcNow.AddHours(1);
    private string? _refreshToken = "test-refresh-token";
    private string[] _scopes = Array.Empty<string>();

    /// <summary>
    /// Sets the access token value
    /// </summary>
    /// <param name="token">Token value</param>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder WithToken(string token)
    {
        _token = token;
        return this;
    }

    /// <summary>
    /// Sets the token type
    /// </summary>
    /// <param name="tokenType">Token type (default: Bearer)</param>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder WithTokenType(string tokenType)
    {
        _tokenType = tokenType;
        return this;
    }

    /// <summary>
    /// Sets the expiration time
    /// </summary>
    /// <param name="expiresAt">Expiration time</param>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder WithExpiresAt(DateTimeOffset expiresAt)
    {
        _expiresAt = expiresAt;
        return this;
    }

    /// <summary>
    /// Sets the expiration to a relative time from now
    /// </summary>
    /// <param name="timeFromNow">Time from now (positive for future, negative for past)</param>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder ExpiresIn(TimeSpan timeFromNow)
    {
        _expiresAt = DateTimeOffset.UtcNow.Add(timeFromNow);
        return this;
    }

    /// <summary>
    /// Creates an expired token (expired 1 minute ago)
    /// </summary>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder Expired()
    {
        _expiresAt = DateTimeOffset.UtcNow.AddMinutes(-1);
        return this;
    }

    /// <summary>
    /// Creates a token that expires soon (in 30 seconds)
    /// </summary>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder ExpiresSoon()
    {
        _expiresAt = DateTimeOffset.UtcNow.AddSeconds(30);
        return this;
    }

    /// <summary>
    /// Sets the refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token value</param>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder WithRefreshToken(string? refreshToken)
    {
        _refreshToken = refreshToken;
        return this;
    }

    /// <summary>
    /// Sets the scopes
    /// </summary>
    /// <param name="scopes">Array of scopes</param>
    /// <returns>Builder instance</returns>
    public AccessTokenBuilder WithScopes(params string[] scopes)
    {
        _scopes = scopes;
        return this;
    }

    /// <summary>
    /// Builds the AccessToken instance
    /// </summary>
    /// <returns>AccessToken instance</returns>
    public AccessToken Build()
    {
        return new AccessToken(_token, _tokenType, _expiresAt, _refreshToken, _scopes);
    }
}

/// <summary>
/// Builder for ProcoreAuthOptions test objects
/// </summary>
public class ProcoreAuthOptionsBuilder
{
    private string _clientId = "test-client-id";
    private string _clientSecret = "test-client-secret";
    private Uri _tokenEndpoint = new("https://api.procore.com/oauth/token");
    private Uri _redirectUri = new("http://localhost:8080/callback");
    private string[] _scopes = new[] { "read", "write" };
    private TimeSpan _tokenRefreshMargin = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Sets the client ID
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <returns>Builder instance</returns>
    public ProcoreAuthOptionsBuilder WithClientId(string clientId)
    {
        _clientId = clientId;
        return this;
    }

    /// <summary>
    /// Sets the client secret
    /// </summary>
    /// <param name="clientSecret">Client secret</param>
    /// <returns>Builder instance</returns>
    public ProcoreAuthOptionsBuilder WithClientSecret(string clientSecret)
    {
        _clientSecret = clientSecret;
        return this;
    }

    /// <summary>
    /// Sets the token endpoint
    /// </summary>
    /// <param name="tokenEndpoint">Token endpoint URI</param>
    /// <returns>Builder instance</returns>
    public ProcoreAuthOptionsBuilder WithTokenEndpoint(Uri tokenEndpoint)
    {
        _tokenEndpoint = tokenEndpoint;
        return this;
    }

    /// <summary>
    /// Sets the redirect URI
    /// </summary>
    /// <param name="redirectUri">Redirect URI</param>
    /// <returns>Builder instance</returns>
    public ProcoreAuthOptionsBuilder WithRedirectUri(string redirectUri)
    {
        _redirectUri = new Uri(redirectUri);
        return this;
    }

    /// <summary>
    /// Sets the scopes
    /// </summary>
    /// <param name="scopes">Array of scopes</param>
    /// <returns>Builder instance</returns>
    public ProcoreAuthOptionsBuilder WithScopes(params string[] scopes)
    {
        _scopes = scopes;
        return this;
    }

    /// <summary>
    /// Sets the token refresh margin
    /// </summary>
    /// <param name="margin">Refresh margin timespan</param>
    /// <returns>Builder instance</returns>
    public ProcoreAuthOptionsBuilder WithTokenRefreshMargin(TimeSpan margin)
    {
        _tokenRefreshMargin = margin;
        return this;
    }

    /// <summary>
    /// Builds the ProcoreAuthOptions instance
    /// </summary>
    /// <returns>ProcoreAuthOptions instance</returns>
    public ProcoreAuthOptions Build()
    {
        return new ProcoreAuthOptions
        {
            ClientId = _clientId,
            ClientSecret = _clientSecret,
            TokenEndpoint = _tokenEndpoint,
            RedirectUri = _redirectUri,
            Scopes = _scopes,
            TokenRefreshMargin = _tokenRefreshMargin
        };
    }
}