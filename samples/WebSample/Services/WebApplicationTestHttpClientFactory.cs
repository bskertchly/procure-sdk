namespace WebSample.Services;

/// <summary>
/// HTTP client factory specifically for web application testing
/// Handles session context and cookie management for test scenarios
/// </summary>
public class WebApplicationTestHttpClientFactory : IHttpClientFactory
{
    private readonly HttpMessageHandler _handler;
    private readonly IServiceProvider _serviceProvider;

    public WebApplicationTestHttpClientFactory(HttpMessageHandler handler, IServiceProvider serviceProvider)
    {
        _handler = handler;
        _serviceProvider = serviceProvider;
    }

    public HttpClient CreateClient(string name)
    {
        var client = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.procore.com")
        };

        // Add any web-specific configuration
        client.DefaultRequestHeaders.Add("User-Agent", "Procore-SDK-WebSample/1.0");
        
        return client;
    }
}