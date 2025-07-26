using System;

namespace Procore.SDK.Extensions;

/// <summary>
/// Temporary interface to allow compilation while Core client has issues
/// This will be removed once the Core client compilation is fixed
/// </summary>
public interface ICoreClient : IDisposable
{
    /// <summary>
    /// Provides access to the underlying generated Kiota client for advanced scenarios.
    /// </summary>
    object RawClient { get; }
}

/// <summary>
/// Temporary implementation to allow compilation while Core client has issues
/// This will be removed once the Core client compilation is fixed
/// </summary>
internal sealed class TemporaryCoreClient : ICoreClient
{
    public object RawClient => new object();

    public void Dispose()
    {
        // No-op for temporary implementation
    }
}