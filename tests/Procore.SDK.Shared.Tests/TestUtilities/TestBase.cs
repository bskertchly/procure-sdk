using Microsoft.Extensions.Logging;
using NSubstitute;
using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.Shared.Tests.TestUtilities;

/// <summary>
/// Base class for all test classes providing common test infrastructure
/// Provides consistent setup and teardown patterns across all test projects
/// </summary>
public abstract class TestBase : IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Protected logger for use in derived test classes
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Testable HTTP message handler for mocking HTTP requests
    /// </summary>
    protected TestableHttpMessageHandler HttpHandler { get; }

    /// <summary>
    /// HTTP client configured with the testable handler
    /// </summary>
    protected HttpClient HttpClient { get; }

    /// <summary>
    /// Initializes the test base with common infrastructure
    /// </summary>
    protected TestBase()
    {
        Logger = Substitute.For<ILogger>();
        HttpHandler = new TestableHttpMessageHandler();
        HttpClient = new HttpClient(HttpHandler);
    }

    /// <summary>
    /// Sets up common test infrastructure - override in derived classes for additional setup
    /// </summary>
    protected virtual void Setup()
    {
        // Override in derived classes for specific setup
    }

    /// <summary>
    /// Tears down test infrastructure - override in derived classes for additional cleanup
    /// </summary>
    protected virtual void Teardown()
    {
        // Override in derived classes for specific cleanup
    }

    /// <summary>
    /// Creates a mock logger for the specified type
    /// </summary>
    /// <typeparam name="T">Type for the logger</typeparam>
    /// <returns>Mock logger instance</returns>
    protected ILogger<T> CreateMockLogger<T>()
    {
        return Substitute.For<ILogger<T>>();
    }

    /// <summary>
    /// Asserts that a condition is true, with optional message
    /// </summary>
    /// <param name="condition">Condition to assert</param>
    /// <param name="message">Optional failure message</param>
    protected static void Assert(bool condition, string? message = null)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message ?? "Assertion failed");
        }
    }

    /// <summary>
    /// Asserts that two values are equal
    /// </summary>
    /// <typeparam name="T">Type of values to compare</typeparam>
    /// <param name="expected">Expected value</param>
    /// <param name="actual">Actual value</param>
    /// <param name="message">Optional failure message</param>
    protected static void AssertEqual<T>(T expected, T actual, string? message = null)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException(
                message ?? $"Expected: {expected}, Actual: {actual}");
        }
    }

    /// <summary>
    /// Asserts that a value is not null
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    /// <param name="value">Value to check</param>
    /// <param name="message">Optional failure message</param>
    protected static void AssertNotNull<T>(T value, string? message = null) where T : class
    {
        if (value == null)
        {
            throw new InvalidOperationException(message ?? "Value should not be null");
        }
    }

    /// <summary>
    /// Creates a cancellation token that will be cancelled after the specified delay
    /// Useful for testing timeout scenarios
    /// </summary>
    /// <param name="delay">Delay before cancellation</param>
    /// <returns>Cancellation token</returns>
    protected static CancellationToken CreateTimeoutToken(TimeSpan delay)
    {
        var cts = new CancellationTokenSource(delay);
        return cts.Token;
    }

    /// <summary>
    /// Creates an already-cancelled cancellation token
    /// Useful for testing cancellation scenarios
    /// </summary>
    /// <returns>Cancelled cancellation token</returns>
    protected static CancellationToken CreateCancelledToken()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        return cts.Token;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            try
            {
                Teardown();
            }
            finally
            {
                HttpClient?.Dispose();
                HttpHandler?.Dispose();
                _disposed = true;
            }
        }
    }
}

/// <summary>
/// Base class for authentication-related tests
/// Provides common authentication test infrastructure
/// </summary>
public abstract class AuthenticationTestBase : TestBase
{
    /// <summary>
    /// Mock token storage for authentication testing
    /// </summary>
    protected ITokenStorage MockTokenStorage { get; }

    /// <summary>
    /// Mock token manager for authentication testing
    /// </summary>
    protected ITokenManager MockTokenManager { get; }

    /// <summary>
    /// Test authentication options
    /// </summary>
    protected ProcoreAuthOptions AuthOptions { get; }

    /// <summary>
    /// Initializes authentication test infrastructure
    /// </summary>
    protected AuthenticationTestBase()
    {
        MockTokenStorage = Substitute.For<ITokenStorage>();
        MockTokenManager = Substitute.For<ITokenManager>();
        
        AuthOptions = TestDataBuilder.ProcoreAuthOptions()
            .WithClientId("test-client-id")
            .WithClientSecret("test-client-secret")
            .WithTokenEndpoint(new Uri("https://api.procore.com/oauth/token"))
            .WithRedirectUri("http://localhost:8080/callback")
            .WithScopes("read", "write")
            .Build();
    }

    /// <summary>
    /// Creates a valid access token for testing
    /// </summary>
    /// <returns>Valid access token</returns>
    protected AccessToken CreateValidToken()
    {
        return TestDataBuilder.AccessToken()
            .WithToken("valid-access-token")
            .ExpiresIn(TimeSpan.FromHours(1))
            .WithRefreshToken("valid-refresh-token")
            .WithScopes("read", "write")
            .Build();
    }

    /// <summary>
    /// Creates an expired access token for testing
    /// </summary>
    /// <returns>Expired access token</returns>
    protected AccessToken CreateExpiredToken()
    {
        return TestDataBuilder.AccessToken()
            .WithToken("expired-access-token")
            .Expired()
            .WithRefreshToken("expired-refresh-token")
            .Build();
    }

    /// <summary>
    /// Creates a token that expires soon for testing refresh scenarios
    /// </summary>
    /// <returns>Token that expires soon</returns>
    protected AccessToken CreateSoonToExpireToken()
    {
        return TestDataBuilder.AccessToken()
            .WithToken("soon-to-expire-token")
            .ExpiresSoon()
            .WithRefreshToken("soon-to-expire-refresh-token")
            .Build();
    }
}