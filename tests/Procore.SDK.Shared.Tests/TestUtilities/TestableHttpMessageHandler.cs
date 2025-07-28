using System.Net;
using System.Text;

namespace Procore.SDK.Shared.Tests.TestUtilities;

/// <summary>
/// A testable HTTP message handler that allows for mocking HTTP responses
/// Used across all test projects for consistent HTTP mocking
/// </summary>
public class TestableHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();
    private readonly List<HttpRequestMessage> _requests = new();
    private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responseSequence = new();

    /// <summary>
    /// Function to provide custom response logic
    /// </summary>
    public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? SendAsyncFunc { get; set; }

    /// <summary>
    /// Gets all captured requests
    /// </summary>
    public IReadOnlyList<HttpRequestMessage> Requests => _requests.AsReadOnly();

    /// <summary>
    /// Gets the most recent request
    /// </summary>
    public HttpRequestMessage? LastRequest => _requests.LastOrDefault();

    /// <summary>
    /// Sets up a response to be returned for the next request
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="content">Response content</param>
    /// <param name="contentType">Content type header</param>
    public void SetupResponse(HttpStatusCode statusCode, string content = "", string contentType = "application/json")
    {
        var response = new HttpResponseMessage(statusCode);
        if (!string.IsNullOrEmpty(content))
        {
            response.Content = new StringContent(content, Encoding.UTF8, contentType);
        }
        _responses.Enqueue(response);
    }

    /// <summary>
    /// Sets up multiple responses to be returned in sequence
    /// </summary>
    /// <param name="responses">Array of response configurations</param>
    public void SetupResponses(params (HttpStatusCode StatusCode, string Content)[] responses)
    {
        foreach (var (statusCode, content) in responses)
        {
            SetupResponse(statusCode, content);
        }
    }

    /// <summary>
    /// Sets up a sequence of custom response functions to be called in order
    /// </summary>
    /// <param name="responses">Array of response functions</param>
    public void SetupSequence(params Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>[] responses)
    {
        _responseSequence.Clear();
        foreach (var response in responses)
            _responseSequence.Enqueue(response);
    }

    /// <summary>
    /// Sets up a response with custom headers
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="content">Response content</param>
    /// <param name="headers">Custom headers to add</param>
    public void SetupResponseWithHeaders(HttpStatusCode statusCode, string content, Dictionary<string, string> headers)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        foreach (var (key, value) in headers)
        {
            if (key.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))
            {
                response.Content.Headers.TryAddWithoutValidation(key, value);
            }
            else
            {
                response.Headers.TryAddWithoutValidation(key, value);
            }
        }

        _responses.Enqueue(response);
    }

    /// <summary>
    /// Clears all queued responses and captured requests
    /// </summary>
    public void Reset()
    {
        _responses.Clear();
        _requests.Clear();
        _responseSequence.Clear();
        SendAsyncFunc = null;
    }

    /// <summary>
    /// Verifies that a request was made with the specified method and URI
    /// </summary>
    /// <param name="method">Expected HTTP method</param>
    /// <param name="uri">Expected URI</param>
    /// <returns>True if matching request was found</returns>
    public bool VerifyRequestMade(HttpMethod method, string uri)
    {
        return _requests.Any(r => r.Method == method && r.RequestUri?.ToString().Contains(uri) == true);
    }

    /// <summary>
    /// Gets the count of requests made with the specified method
    /// </summary>
    /// <param name="method">HTTP method to count</param>
    /// <returns>Number of matching requests</returns>
    public int GetRequestCount(HttpMethod method)
    {
        return _requests.Count(r => r.Method == method);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Capture the request for verification
        _requests.Add(request);

        // Use sequence function if available
        if (_responseSequence.Count > 0)
        {
            var func = _responseSequence.Dequeue();
            return await func(request, cancellationToken);
        }

        // Use custom function if provided
        if (SendAsyncFunc != null)
        {
            return await SendAsyncFunc(request, cancellationToken);
        }

        // Return queued response or default
        if (_responses.Count > 0)
        {
            return _responses.Dequeue();
        }

        // Default response
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Reset();
        }
        base.Dispose(disposing);
    }
}