using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// In-memory token storage implementation for development and testing
/// Thread-safe but tokens are lost when the application restarts
/// </summary>
public class InMemoryTokenStorage : ITokenStorage
{
    private readonly ConcurrentDictionary<string, AccessToken> _tokens = new();

    /// <inheritdoc />
    public Task<AccessToken?> GetTokenAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        _tokens.TryGetValue(key, out var token);
        return Task.FromResult(token);
    }

    /// <inheritdoc />
    public Task StoreTokenAsync(string key, AccessToken token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        ArgumentNullException.ThrowIfNull(token);

        cancellationToken.ThrowIfCancellationRequested();

        _tokens.AddOrUpdate(key, token, (_, _) => token);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteTokenAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        _tokens.TryRemove(key, out _);
        return Task.CompletedTask;
    }
}