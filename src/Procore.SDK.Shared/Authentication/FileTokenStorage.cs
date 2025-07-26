using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Procore.SDK.Shared.Authentication;

/// <summary>
/// File-based token storage with encryption
/// Stores tokens in an encrypted JSON file for persistence across application restarts
/// </summary>
public sealed class FileTokenStorage : ITokenStorage, IDisposable
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly byte[] _entropy; // Additional entropy for encryption
    private bool _disposed;

    /// <summary>
    /// Creates a new FileTokenStorage instance
    /// </summary>
    /// <param name="filePath">Path to the file where tokens will be stored</param>
    public FileTokenStorage(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        _filePath = filePath;

        // Generate consistent entropy based on file path for encryption
        using var sha256 = SHA256.Create();
        _entropy = sha256.ComputeHash(Encoding.UTF8.GetBytes(_filePath + Environment.MachineName));
    }

    /// <inheritdoc />
    public async Task<AccessToken?> GetTokenAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_filePath))
                return null;

            var encryptedData = await File.ReadAllBytesAsync(_filePath, cancellationToken);
            var decryptedJson = DecryptData(encryptedData);

            var tokenData = JsonSerializer.Deserialize<Dictionary<string, TokenData>>(decryptedJson);

            if (tokenData != null && tokenData.TryGetValue(key, out var data))
            {
                return new AccessToken(
                    data.Token,
                    data.TokenType,
                    data.ExpiresAt,
                    data.RefreshToken,
                    data.Scopes);
            }

            return null;
        }
        catch (JsonException)
        {
            // File is corrupted, treat as empty
            return null;
        }
        catch (CryptographicException)
        {
            // Encryption failed, treat as empty
            return null;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task StoreTokenAsync(string key, AccessToken token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        ArgumentNullException.ThrowIfNull(token);

        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Load existing tokens
            var tokenData = new Dictionary<string, TokenData>();
            if (File.Exists(_filePath))
            {
                try
                {
                    var existingEncryptedData = await File.ReadAllBytesAsync(_filePath, cancellationToken);
                    var existingDecryptedJson = DecryptData(existingEncryptedData);
                    var existingTokenData = JsonSerializer.Deserialize<Dictionary<string, TokenData>>(existingDecryptedJson);
                    if (existingTokenData != null)
                    {
                        tokenData = existingTokenData;
                    }
                }
                catch (JsonException)
                {
                    // File is corrupted, start fresh
                }
                catch (CryptographicException)
                {
                    // Encryption failed, start fresh
                }
            }

            // Add or update the token
            tokenData[key] = new TokenData
            {
                Token = token.Token,
                TokenType = token.TokenType,
                ExpiresAt = token.ExpiresAt,
                RefreshToken = token.RefreshToken,
                Scopes = token.Scopes
            };

            // Save encrypted data
            var json = JsonSerializer.Serialize(tokenData);
            var encryptedData = EncryptData(json);
            await File.WriteAllBytesAsync(_filePath, encryptedData, cancellationToken);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task DeleteTokenAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_filePath))
                return;

            var encryptedData = await File.ReadAllBytesAsync(_filePath, cancellationToken);
            var decryptedJson = DecryptData(encryptedData);

            var tokenData = JsonSerializer.Deserialize<Dictionary<string, TokenData>>(decryptedJson);

            if (tokenData != null && tokenData.Remove(key))
            {
                if (tokenData.Count == 0)
                {
                    // Delete file if no tokens remain
                    File.Delete(_filePath);
                }
                else
                {
                    // Save updated data
                    var json = JsonSerializer.Serialize(tokenData);
                    var newEncryptedData = EncryptData(json);
                    await File.WriteAllBytesAsync(_filePath, newEncryptedData, cancellationToken);
                }
            }
        }
        catch (JsonException)
        {
            // File is corrupted, ignore
        }
        catch (CryptographicException)
        {
            // Encryption failed, ignore
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private byte[] EncryptData(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);

        if (OperatingSystem.IsWindows())
        {
            // Use DPAPI on Windows for additional security
            return ProtectedData.Protect(dataBytes, _entropy, DataProtectionScope.CurrentUser);
        }
        else
        {
            // On non-Windows platforms, use basic XOR encryption with entropy
            // This is not strong encryption but provides basic obfuscation
            var result = new byte[dataBytes.Length];
            for (int i = 0; i < dataBytes.Length; i++)
            {
                result[i] = (byte)(dataBytes[i] ^ _entropy[i % _entropy.Length]);
            }

            return result;
        }
    }

    private string DecryptData(byte[] encryptedData)
    {
        if (OperatingSystem.IsWindows())
        {
            // Use DPAPI on Windows
            var decryptedBytes = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        else
        {
            // On non-Windows platforms, reverse the XOR encryption
            var result = new byte[encryptedData.Length];
            for (int i = 0; i < encryptedData.Length; i++)
            {
                result[i] = (byte)(encryptedData[i] ^ _entropy[i % _entropy.Length]);
            }

            return Encoding.UTF8.GetString(result);
        }
    }

    /// <summary>
    /// Internal representation of token data for JSON serialization
    /// </summary>
    private sealed class TokenData
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public string[]? Scopes { get; set; }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected virtual dispose method to follow proper dispose pattern
    /// </summary>
    /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _fileLock.Dispose();
            _disposed = true;
        }
    }
}