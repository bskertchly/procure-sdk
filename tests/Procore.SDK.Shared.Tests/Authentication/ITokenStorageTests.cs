using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Shared.Tests.Authentication;

/// <summary>
/// TDD Tests for ITokenStorage interface specification
/// These tests define the expected behavior for token storage implementations
/// </summary>
public class ITokenStorageTests
{
    [Fact]
    public void ITokenStorage_ShouldDefineGetTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenStorage).GetMethods();
        var getTokenMethod = methods.FirstOrDefault(m => m.Name == "GetTokenAsync");

        // Assert
        getTokenMethod.Should().NotBeNull("ITokenStorage must define GetTokenAsync method");
        getTokenMethod!.ReturnType.Should().Be(typeof(Task<AccessToken?>),
            "GetTokenAsync should return Task<AccessToken?>");

        var parameters = getTokenMethod.GetParameters();
        parameters.Should().HaveCount(2, "GetTokenAsync should have two parameters");
        parameters[0].Name.Should().Be("key");
        parameters[0].ParameterType.Should().Be(typeof(string));
        parameters[1].Name.Should().Be("cancellationToken");
        parameters[1].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[1].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }

    [Fact]
    public void ITokenStorage_ShouldDefineStoreTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenStorage).GetMethods();
        var storeTokenMethod = methods.FirstOrDefault(m => m.Name == "StoreTokenAsync");

        // Assert
        storeTokenMethod.Should().NotBeNull("ITokenStorage must define StoreTokenAsync method");
        storeTokenMethod!.ReturnType.Should().Be(typeof(Task),
            "StoreTokenAsync should return Task");

        var parameters = storeTokenMethod.GetParameters();
        parameters.Should().HaveCount(3, "StoreTokenAsync should have three parameters");
        parameters[0].Name.Should().Be("key");
        parameters[0].ParameterType.Should().Be(typeof(string));
        parameters[1].Name.Should().Be("token");
        parameters[1].ParameterType.Should().Be(typeof(AccessToken));
        parameters[2].Name.Should().Be("cancellationToken");
        parameters[2].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[2].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }

    [Fact]
    public void ITokenStorage_ShouldDefineDeleteTokenMethod()
    {
        // Arrange & Act
        var methods = typeof(ITokenStorage).GetMethods();
        var deleteTokenMethod = methods.FirstOrDefault(m => m.Name == "DeleteTokenAsync");

        // Assert
        deleteTokenMethod.Should().NotBeNull("ITokenStorage must define DeleteTokenAsync method");
        deleteTokenMethod!.ReturnType.Should().Be(typeof(Task),
            "DeleteTokenAsync should return Task");

        var parameters = deleteTokenMethod.GetParameters();
        parameters.Should().HaveCount(2, "DeleteTokenAsync should have two parameters");
        parameters[0].Name.Should().Be("key");
        parameters[0].ParameterType.Should().Be(typeof(string));
        parameters[1].Name.Should().Be("cancellationToken");
        parameters[1].ParameterType.Should().Be(typeof(CancellationToken));
        parameters[1].HasDefaultValue.Should().BeTrue("CancellationToken should have default value");
    }
}

/// <summary>
/// TDD Tests for InMemoryTokenStorage implementation
/// These tests define the expected behavior for in-memory token storage
/// </summary>
public class InMemoryTokenStorageTests
{
    private readonly InMemoryTokenStorage _storage;

    public InMemoryTokenStorageTests()
    {
        _storage = new InMemoryTokenStorage();
    }

    [Fact]
    public void InMemoryTokenStorage_ShouldImplementITokenStorage()
    {
        // Arrange & Act
        var storage = new InMemoryTokenStorage();

        // Assert
        storage.Should().BeAssignableTo<ITokenStorage>();
    }

    [Fact]
    public async Task GetTokenAsync_WhenTokenDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var key = "non-existent-key";

        // Act
        var result = await _storage.GetTokenAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task StoreTokenAsync_ShouldStoreToken()
    {
        // Arrange
        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act
        await _storage.StoreTokenAsync(key, token);
        var result = await _storage.GetTokenAsync(key);

        // Assert
        result.Should().Be(token);
    }

    [Fact]
    public async Task StoreTokenAsync_WithSameKey_ShouldOverwriteToken()
    {
        // Arrange
        var key = "test-key";
        var oldToken = new AccessToken("old-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        var newToken = new AccessToken("new-token", "Bearer", DateTimeOffset.UtcNow.AddHours(2));

        // Act
        await _storage.StoreTokenAsync(key, oldToken);
        await _storage.StoreTokenAsync(key, newToken);
        var result = await _storage.GetTokenAsync(key);

        // Assert
        result.Should().Be(newToken);
        result.Should().NotBe(oldToken);
    }

    [Fact]
    public async Task DeleteTokenAsync_WhenTokenExists_ShouldRemoveToken()
    {
        // Arrange
        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        await _storage.StoreTokenAsync(key, token);

        // Act
        await _storage.DeleteTokenAsync(key);
        var result = await _storage.GetTokenAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteTokenAsync_WhenTokenDoesNotExist_ShouldNotThrow()
    {
        // Arrange
        var key = "non-existent-key";

        // Act & Assert
        await _storage.Invoking(s => s.DeleteTokenAsync(key))
                     .Should().NotThrowAsync();
    }

    [Fact]
    public async Task InMemoryTokenStorage_ShouldSupportMultipleKeys()
    {
        // Arrange
        var key1 = "key1";
        var key2 = "key2";
        var token1 = new AccessToken("token1", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        var token2 = new AccessToken("token2", "Bearer", DateTimeOffset.UtcNow.AddHours(2));

        // Act
        await _storage.StoreTokenAsync(key1, token1);
        await _storage.StoreTokenAsync(key2, token2);

        // Assert
        var result1 = await _storage.GetTokenAsync(key1);
        var result2 = await _storage.GetTokenAsync(key2);

        result1.Should().Be(token1);
        result2.Should().Be(token2);
    }

    [Fact]
    public async Task InMemoryTokenStorage_ShouldRespectCancellation()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert
        await _storage.Invoking(s => s.GetTokenAsync(key, cts.Token))
                     .Should().ThrowAsync<OperationCanceledException>();

        await _storage.Invoking(s => s.StoreTokenAsync(key, token, cts.Token))
                     .Should().ThrowAsync<OperationCanceledException>();

        await _storage.Invoking(s => s.DeleteTokenAsync(key, cts.Token))
                     .Should().ThrowAsync<OperationCanceledException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task StoreTokenAsync_WithInvalidKey_ShouldThrowArgumentException(string? invalidKey)
    {
        // Arrange
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert
        await _storage.Invoking(s => s.StoreTokenAsync(invalidKey!, token))
                     .Should().ThrowAsync<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task GetTokenAsync_WithInvalidKey_ShouldThrowArgumentException(string? invalidKey)
    {
        // Act & Assert
        await _storage.Invoking(s => s.GetTokenAsync(invalidKey!))
                     .Should().ThrowAsync<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task DeleteTokenAsync_WithInvalidKey_ShouldThrowArgumentException(string? invalidKey)
    {
        // Act & Assert
        await _storage.Invoking(s => s.DeleteTokenAsync(invalidKey!))
                     .Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task InMemoryTokenStorage_ShouldBeThreadSafe()
    {
        // This test verifies that concurrent operations don't cause race conditions
        // Arrange
        var tasks = new List<Task>();
        var random = new Random();

        // Act - Perform concurrent operations
        for (int i = 0; i < 100; i++)
        {
            var key = $"key-{i}";
            var token = new AccessToken($"token-{i}", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

            tasks.Add(Task.Run(async () =>
            {
                await _storage.StoreTokenAsync(key, token);
                await Task.Delay(random.Next(1, 10)); // Small random delay
                var retrieved = await _storage.GetTokenAsync(key);
                retrieved.Should().Be(token);
            }));
        }

        await Task.WhenAll(tasks);

        // Assert - Verify all tokens are correctly stored
        for (int i = 0; i < 100; i++)
        {
            var key = $"key-{i}";
            var result = await _storage.GetTokenAsync(key);
            result.Should().NotBeNull();
            result!.Token.Should().Be($"token-{i}");
        }
    }
}

/// <summary>
/// TDD Tests for FileTokenStorage implementation
/// These tests define the expected behavior for file-based token storage
/// </summary>
public class FileTokenStorageTests : IDisposable
{
    private readonly string _tempFilePath;
    private readonly FileTokenStorage _storage;

    public FileTokenStorageTests()
    {
        _tempFilePath = Path.GetTempFileName();
        _storage = new FileTokenStorage(_tempFilePath);
    }

    [Fact]
    public void FileTokenStorage_ShouldImplementITokenStorage()
    {
        // Arrange & Act
        using var storage = new FileTokenStorage("test-path");

        // Assert
        storage.Should().BeAssignableTo<ITokenStorage>();
    }

    [Fact]
    public async Task GetTokenAsync_WhenFileDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentPath = Path.Combine(Path.GetTempPath(), "non-existent-file.json");
        using var storage = new FileTokenStorage(nonExistentPath);
        var key = "test-key";

        // Act
        var result = await storage.GetTokenAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task StoreTokenAsync_ShouldCreateFileAndStoreToken()
    {
        // Arrange
        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act
        await _storage.StoreTokenAsync(key, token);
        var result = await _storage.GetTokenAsync(key);

        // Assert
        result.Should().Be(token);
        File.Exists(_tempFilePath).Should().BeTrue("File should be created");
    }

    [Fact]
    public async Task StoreTokenAsync_WithMultipleTokens_ShouldStoreAllTokens()
    {
        // Arrange
        var key1 = "key1";
        var key2 = "key2";
        var token1 = new AccessToken("token1", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        var token2 = new AccessToken("token2", "Bearer", DateTimeOffset.UtcNow.AddHours(2));

        // Act
        await _storage.StoreTokenAsync(key1, token1);
        await _storage.StoreTokenAsync(key2, token2);

        // Assert
        var result1 = await _storage.GetTokenAsync(key1);
        var result2 = await _storage.GetTokenAsync(key2);

        result1.Should().Be(token1);
        result2.Should().Be(token2);
    }

    [Fact]
    public async Task DeleteTokenAsync_ShouldRemoveTokenFromFile()
    {
        // Arrange
        var key1 = "key1";
        var key2 = "key2";
        var token1 = new AccessToken("token1", "Bearer", DateTimeOffset.UtcNow.AddHours(1));
        var token2 = new AccessToken("token2", "Bearer", DateTimeOffset.UtcNow.AddHours(2));

        await _storage.StoreTokenAsync(key1, token1);
        await _storage.StoreTokenAsync(key2, token2);

        // Act
        await _storage.DeleteTokenAsync(key1);

        // Assert
        var result1 = await _storage.GetTokenAsync(key1);
        var result2 = await _storage.GetTokenAsync(key2);

        result1.Should().BeNull();
        result2.Should().Be(token2);
    }

    [Fact]
    public async Task FileTokenStorage_ShouldPersistAcrossInstances()
    {
        // Arrange
        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act - Store with first instance
        await _storage.StoreTokenAsync(key, token);

        // Create new instance pointing to same file
        var newStorage = new FileTokenStorage(_tempFilePath);
        var result = await newStorage.GetTokenAsync(key);

        // Assert
        result.Should().Be(token);
    }

    [Fact]
    public async Task FileTokenStorage_WithInvalidPath_ShouldThrowException()
    {
        // Arrange
        var invalidPath = "/invalid/path/that/does/not/exist/token.json";
        var storage = new FileTokenStorage(invalidPath);
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert - Expect IOException on macOS/Linux, DirectoryNotFoundException on Windows
        await storage.Invoking(s => s.StoreTokenAsync("key", token))
                     .Should().ThrowAsync<IOException>();
    }

    [Fact]
    public async Task FileTokenStorage_ShouldHandleFileEncryption()
    {
        // This test ensures that the file content is not stored in plain text
        // Arrange
        var key = "test-key";
        var token = new AccessToken("secret-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act
        await _storage.StoreTokenAsync(key, token);

        // Assert
        var fileContent = await File.ReadAllTextAsync(_tempFilePath);
        fileContent.Should().NotContain("secret-token", 
            "Token should be encrypted and not stored in plain text");
    }

    [Fact]
    public async Task FileTokenStorage_ShouldHandleCorruptedFile()
    {
        // Arrange
        await File.WriteAllTextAsync(_tempFilePath, "corrupted json content");
        var key = "test-key";

        // Act & Assert
        var result = await _storage.GetTokenAsync(key);
        result.Should().BeNull("Corrupted file should be treated as empty");
    }

    [Fact]
    public async Task FileTokenStorage_ShouldRespectCancellation()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act & Assert
        await _storage.Invoking(s => s.GetTokenAsync(key, cts.Token))
                     .Should().ThrowAsync<OperationCanceledException>();

        await _storage.Invoking(s => s.StoreTokenAsync(key, token, cts.Token))
                     .Should().ThrowAsync<OperationCanceledException>();

        await _storage.Invoking(s => s.DeleteTokenAsync(key, cts.Token))
                     .Should().ThrowAsync<OperationCanceledException>();
    }

    public void Dispose()
    {
        _storage?.Dispose();
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }
}

/// <summary>
/// TDD Tests for ProtectedDataTokenStorage implementation (Windows DPAPI)
/// These tests define the expected behavior for Windows-specific encrypted token storage
/// </summary>
public class ProtectedDataTokenStorageTests
{
    [Fact]
    public void ProtectedDataTokenStorage_ShouldImplementITokenStorage()
    {
        // Skip on non-Windows platforms where construction throws
        if (!OperatingSystem.IsWindows())
        {
            // Verify interface implementation through type checking instead
            typeof(ProtectedDataTokenStorage).Should().BeAssignableTo<ITokenStorage>();
            return;
        }

        // Arrange & Act - On Windows, can instantiate and test
        var storage = new ProtectedDataTokenStorage();

        // Assert
        storage.Should().BeAssignableTo<ITokenStorage>();
    }

    [Fact]
    public async Task ProtectedDataTokenStorage_OnWindows_ShouldStoreAndRetrieveToken()
    {
        // This test should only run on Windows where DPAPI is available
        if (!OperatingSystem.IsWindows())
        {
            return; // Skip test on non-Windows platforms
        }

        // Arrange
        var storage = new ProtectedDataTokenStorage();
        var key = "test-key";
        var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1));

        // Act
        await storage.StoreTokenAsync(key, token);
        var result = await storage.GetTokenAsync(key);

        // Assert
        result.Should().Be(token);
    }

    [Fact]
    public void ProtectedDataTokenStorage_OnNonWindows_ShouldThrowPlatformNotSupportedException()
    {
        // This test verifies that the storage throws appropriate exception on non-Windows platforms
        if (OperatingSystem.IsWindows())
        {
            return; // Skip test on Windows
        }

        // Act & Assert - Constructor should throw on non-Windows platforms
        Assert.Throws<PlatformNotSupportedException>(() => new ProtectedDataTokenStorage());
    }

    [Fact]
    public void ProtectedDataTokenStorage_ShouldHaveParameterlessConstructor()
    {
        // Skip on non-Windows platforms where construction throws
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Arrange & Act
        var constructors = typeof(ProtectedDataTokenStorage).GetConstructors();
        var parameterlessConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);

        // Assert
        parameterlessConstructor.Should().NotBeNull(
            "ProtectedDataTokenStorage should have a parameterless constructor for DI");

        // Verify it can be instantiated on Windows
        var instance = Activator.CreateInstance<ProtectedDataTokenStorage>();
        instance.Should().NotBeNull();
    }
}