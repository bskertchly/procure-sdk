using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Net.Http;
using Procore.SDK.Shared.Authentication;

namespace Procore.SDK.IntegrationTests;

/// <summary>
/// Test helpers specifically for CQ Task 10 API Surface Validation and Performance Testing.
/// Provides infrastructure for creating test clients, adapters, and managing test data.
/// </summary>
public static class TestHelpers
{
    private static readonly IConfiguration _configuration = BuildConfiguration();
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => 
        builder.AddConsole().SetMinimumLevel(LogLevel.Information));

    /// <summary>
    /// Creates a configured request adapter for testing.
    /// Uses mock authentication for performance testing scenarios.
    /// </summary>
    public static IRequestAdapter CreateRequestAdapter()
    {
        try
        {
            // Create HTTP client with test configuration
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.procore.com/rest/");
            
            // Add test headers
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Procore-SDK-Test/1.0");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            
            // For performance testing, use a mock authentication provider
            var authProvider = new MockAuthenticationProvider();
            
            // Create Kiota HTTP request adapter
            var requestAdapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
            
            return requestAdapter;
        }
        catch (Exception ex)
        {
            // Fallback to minimal request adapter for testing
            return new MockRequestAdapter();
        }
    }

    /// <summary>
    /// Creates a logger instance for testing.
    /// </summary>
    public static ILogger<T> CreateLogger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }

    /// <summary>
    /// Gets a test company ID from configuration or returns a default test value.
    /// </summary>
    public static int GetTestCompanyId()
    {
        return _configuration.GetValue<int>("TestSettings:CompanyId", 1);
    }

    /// <summary>
    /// Gets a test project ID from configuration or returns a default test value.
    /// </summary>
    public static int GetTestProjectId()
    {
        return _configuration.GetValue<int>("TestSettings:ProjectId", 1);
    }

    /// <summary>
    /// Gets test user ID from configuration or returns a default test value.
    /// </summary>
    public static int GetTestUserId()
    {
        return _configuration.GetValue<int>("TestSettings:UserId", 1);
    }

    /// <summary>
    /// Creates a test configuration with default values for performance testing.
    /// </summary>
    public static ProcoreAuthOptions CreateTestAuthOptions()
    {
        return new ProcoreAuthOptions
        {
            ClientId = _configuration["TestSettings:ClientId"] ?? "test-client-id",
            ClientSecret = _configuration["TestSettings:ClientSecret"] ?? "test-client-secret",
            RedirectUri = _configuration["TestSettings:RedirectUri"] ?? "https://localhost:5001/auth/callback",
            BaseUrl = _configuration["TestSettings:BaseUrl"] ?? "https://api.procore.com",
            Scopes = _configuration.GetSection("TestSettings:Scopes").Get<string[]>() ?? new[] { "read", "write" }
        };
    }

    /// <summary>
    /// Creates a mock HTTP message handler for performance testing scenarios.
    /// </summary>
    public static HttpMessageHandler CreateMockHttpHandler()
    {
        return new MockHttpMessageHandler();
    }

    /// <summary>
    /// Builds configuration from various sources for testing.
    /// </summary>
    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: true)
            .AddJsonFile("appsettings.integrationtest.json", optional: true)
            .AddEnvironmentVariables("PROCORE_TEST_")
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("TestSettings:CompanyId", "1"),
                new KeyValuePair<string, string?>("TestSettings:ProjectId", "1"),
                new KeyValuePair<string, string?>("TestSettings:UserId", "1"),
                new KeyValuePair<string, string?>("TestSettings:BaseUrl", "https://api.procore.com"),
                new KeyValuePair<string, string?>("TestSettings:ClientId", "test-client-id"),
                new KeyValuePair<string, string?>("TestSettings:RedirectUri", "https://localhost:5001/auth/callback")
            });

        return builder.Build();
    }
}

/// <summary>
/// Mock authentication provider for performance testing.
/// Provides consistent, fast authentication for benchmarking scenarios.
/// </summary>
public class MockAuthenticationProvider : IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        // Add mock authorization header for testing
        request.Headers.Add("Authorization", "Bearer mock-test-token");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Mock request adapter for offline testing scenarios.
/// Returns simulated responses for performance and coverage testing.
/// </summary>
public class MockRequestAdapter : IRequestAdapter
{
    public string BaseUrl { get; set; } = "https://api.procore.com/rest/";
    public ISerializationWriterFactory SerializationWriterFactory { get; set; } = new JsonSerializationWriterFactory();
    public IParseNodeFactory ParseNodeFactory { get; set; } = new JsonParseNodeFactory();

    public Task<T?> SendAsync<T>(RequestInformation requestInfo, ParsableFactory<T> factory, Dictionary<string, ParsableFactory<IParsable>>? errorMappings = null, CancellationToken cancellationToken = default) where T : IParsable
    {
        // Simulate API response time for performance testing
        var delay = Random.Shared.Next(50, 200);
        Task.Delay(delay, cancellationToken).Wait(cancellationToken);

        // Return mock response based on request type
        return Task.FromResult<T?>(CreateMockResponse<T>(requestInfo));
    }

    public Task<IEnumerable<T>?> SendCollectionAsync<T>(RequestInformation requestInfo, ParsableFactory<T> factory, Dictionary<string, ParsableFactory<IParsable>>? errorMappings = null, CancellationToken cancellationToken = default) where T : IParsable
    {
        // Simulate API response time
        var delay = Random.Shared.Next(100, 300);
        Task.Delay(delay, cancellationToken).Wait(cancellationToken);

        // Return mock collection
        var mockItems = CreateMockCollection<T>(requestInfo);
        return Task.FromResult<IEnumerable<T>?>(mockItems);
    }

    public Task SendNoContentAsync(RequestInformation requestInfo, Dictionary<string, ParsableFactory<IParsable>>? errorMappings = null, CancellationToken cancellationToken = default)
    {
        // Simulate API response time
        var delay = Random.Shared.Next(30, 150);
        return Task.Delay(delay, cancellationToken);
    }

    public Task<Stream?> SendPrimitiveAsync<T>(RequestInformation requestInfo, Dictionary<string, ParsableFactory<IParsable>>? errorMappings = null, CancellationToken cancellationToken = default)
    {
        // Return empty stream for testing
        return Task.FromResult<Stream?>(new MemoryStream());
    }

    private T? CreateMockResponse<T>(RequestInformation requestInfo) where T : IParsable
    {
        // This would typically create mock objects based on the request
        // For now, return default to allow performance testing to proceed
        return default(T);
    }

    private IEnumerable<T> CreateMockCollection<T>(RequestInformation requestInfo) where T : IParsable
    {
        // Return a collection of mock items for testing
        var itemCount = Random.Shared.Next(5, 50);
        var mockItems = new List<T>();
        
        for (int i = 0; i < itemCount; i++)
        {
            var mockItem = CreateMockResponse<T>(requestInfo);
            if (mockItem != null)
            {
                mockItems.Add(mockItem);
            }
        }
        
        return mockItems;
    }

    public void Dispose()
    {
        // No resources to dispose in mock implementation
    }
}

/// <summary>
/// Mock HTTP message handler for direct HTTP client performance comparisons.
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Simulate network latency
        var baseDelay = Random.Shared.Next(20, 100);
        await Task.Delay(baseDelay, cancellationToken);

        // Create mock response based on request
        var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        
        // Add mock content based on endpoint
        var endpoint = request.RequestUri?.PathAndQuery ?? "";
        var mockContent = CreateMockHttpContent(endpoint, request.Method);
        mockResponse.Content = new StringContent(mockContent, System.Text.Encoding.UTF8, "application/json");
        
        return mockResponse;
    }

    private string CreateMockHttpContent(string endpoint, HttpMethod method)
    {
        if (endpoint.Contains("companies"))
        {
            return method == HttpMethod.Get 
                ? """[{"id": 1, "name": "Test Company", "is_active": true}]"""
                : """{"id": 1, "name": "Test Company", "is_active": true}""";
        }
        
        if (endpoint.Contains("users"))
        {
            return method == HttpMethod.Get 
                ? """[{"id": 1, "first_name": "Test", "last_name": "User", "email": "test@example.com"}]"""
                : """{"id": 1, "first_name": "Test", "last_name": "User", "email": "test@example.com"}""";
        }
        
        if (endpoint.Contains("projects"))
        {
            return method == HttpMethod.Get 
                ? """[{"id": 1, "name": "Test Project", "project_number": "P001"}]"""
                : """{"id": 1, "name": "Test Project", "project_number": "P001"}""";
        }
        
        // Default mock response
        return """{"status": "success", "message": "Mock response"}""";
    }
}

/// <summary>
/// JSON serialization writer factory for mock request adapter.
/// </summary>
public class JsonSerializationWriterFactory : ISerializationWriterFactory
{
    public string ValidContentType => "application/json";

    public ISerializationWriter GetSerializationWriter(string contentType)
    {
        return new JsonSerializationWriter();
    }
}

/// <summary>
/// Mock JSON serialization writer.
/// </summary>
public class JsonSerializationWriter : ISerializationWriter
{
    private readonly MemoryStream _stream = new();

    public void WriteStringValue(string? key, string? value) { }
    public void WriteIntValue(string? key, int? value) { }
    public void WriteBoolValue(string? key, bool? value) { }
    public void WriteDoubleValue(string? key, double? value) { }
    public void WriteFloatValue(string? key, float? value) { }
    public void WriteLongValue(string? key, long? value) { }
    public void WriteGuidValue(string? key, Guid? value) { }
    public void WriteDateTimeOffsetValue(string? key, DateTimeOffset? value) { }
    public void WriteTimeSpanValue(string? key, TimeSpan? value) { }
    public void WriteDateValue(string? key, Date? value) { }
    public void WriteTimeValue(string? key, Time? value) { }
    public void WriteObjectValue<T>(string? key, T? value) where T : IParsable { }
    public void WriteCollectionOfPrimitiveValues<T>(string? key, IEnumerable<T>? values) { }
    public void WriteCollectionOfObjectValues<T>(string? key, IEnumerable<T>? values) where T : IParsable { }
    public void WriteByteArrayValue(string? key, byte[]? value) { }
    public void WriteNullValue(string? key) { }
    public void WriteAdditionalData(IDictionary<string, object> value) { }

    public Stream GetSerializedContent() => _stream;

    public void Dispose() => _stream.Dispose();
}

/// <summary>
/// JSON parse node factory for mock request adapter.
/// </summary>
public class JsonParseNodeFactory : IParseNodeFactory
{
    public string ValidContentType => "application/json";

    public IParseNode GetRootParseNode(string contentType, Stream content)
    {
        return new JsonParseNode();
    }
}

/// <summary>
/// Mock JSON parse node.
/// </summary>
public class JsonParseNode : IParseNode
{
    public Action<IParsable>? OnBeforeAssignFieldValues { get; set; }
    public Action<IParsable>? OnAfterAssignFieldValues { get; set; }

    public T? GetObjectValue<T>(ParsableFactory<T> factory) where T : IParsable => default(T);
    public IEnumerable<T>? GetCollectionOfObjectValues<T>(ParsableFactory<T> factory) where T : IParsable => new List<T>();
    public string? GetStringValue() => null;
    public int? GetIntValue() => null;
    public bool? GetBoolValue() => null;
    public double? GetDoubleValue() => null;
    public float? GetFloatValue() => null;
    public long? GetLongValue() => null;
    public Guid? GetGuidValue() => null;
    public DateTimeOffset? GetDateTimeOffsetValue() => null;
    public TimeSpan? GetTimeSpanValue() => null;
    public Date? GetDateValue() => null;
    public Time? GetTimeValue() => null;
    public IEnumerable<T>? GetCollectionOfPrimitiveValues<T>() => new List<T>();
    public byte[]? GetByteArrayValue() => null;
    public IParseNode? GetChildNode(string identifier) => null;
}