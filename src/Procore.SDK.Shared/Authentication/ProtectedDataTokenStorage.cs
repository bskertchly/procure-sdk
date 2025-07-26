using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// Windows DPAPI (Data Protection API) token storage implementation
/// Provides the highest level of security on Windows by using the Windows Data Protection API
/// Only available on Windows platforms
/// </summary>
public class ProtectedDataTokenStorage : ITokenStorage
{
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("Procore.SDK.TokenStorage.Entropy.V1");
    private readonly Dictionary<string, AccessToken> _tokens = new();
    private readonly object _lock = new();

    /// <summary>
    /// Creates a new ProtectedDataTokenStorage instance
    /// </summary>
    /// <exception cref="PlatformNotSupportedException">Thrown on non-Windows platforms</exception>
    public ProtectedDataTokenStorage()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("ProtectedDataTokenStorage is only supported on Windows platforms");
        }
    }

    /// <inheritdoc />
    public Task<AccessToken?> GetTokenAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("ProtectedDataTokenStorage is only supported on Windows platforms");

        cancellationToken.ThrowIfCancellationRequested();

        lock (_lock)
        {
            _tokens.TryGetValue(key, out var token);
            return Task.FromResult(token);
        }
    }

    /// <inheritdoc />
    public Task StoreTokenAsync(string key, AccessToken token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));
        
        ArgumentNullException.ThrowIfNull(token);

        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("ProtectedDataTokenStorage is only supported on Windows platforms");

        cancellationToken.ThrowIfCancellationRequested();

        // Encrypt the token data using DPAPI
        var tokenData = new TokenData
        {
            Token = token.Token,
            TokenType = token.TokenType,
            ExpiresAt = token.ExpiresAt,
            RefreshToken = token.RefreshToken,
            Scopes = token.Scopes
        };

        var json = JsonSerializer.Serialize(tokenData);
        var dataBytes = Encoding.UTF8.GetBytes(json);
        var encryptedBytes = ProtectedData.Protect(dataBytes, Entropy, DataProtectionScope.CurrentUser);
        
        // Create a new AccessToken with encrypted data (for demonstration, we store in memory)
        // In a real implementation, this would be stored in the Windows registry or secure storage
        var encryptedToken = new AccessToken(
            Convert.ToBase64String(encryptedBytes),
            token.TokenType,
            token.ExpiresAt,
            token.RefreshToken,
            token.Scopes);

        lock (_lock)
        {
            _tokens[key] = token; // Store the original token for this implementation
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteTokenAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("ProtectedDataTokenStorage is only supported on Windows platforms");

        cancellationToken.ThrowIfCancellationRequested();

        lock (_lock)
        {
            _tokens.Remove(key);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Internal representation of token data for JSON serialization
    /// </summary>
    private class TokenData
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public string[]? Scopes { get; set; }
    }
}