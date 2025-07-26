using System.Collections.Concurrent;

namespace Procore.SDK.Samples.Tests.Shared.TestHelpers;

/// <summary>
/// Testable HTTP message handler that allows mocking responses and capturing requests
/// Provides comprehensive control over HTTP interactions for testing
/// </summary>
public class TestableHttpMessageHandler : HttpMessageHandler
{
    private readonly ConcurrentQueue<HttpRequestMessage> _capturedRequests = new();
    private readonly List<(Func<HttpRequestMessage, bool> Predicate, HttpResponseMessage Response)> _responses = new();
    private readonly List<(Func<HttpRequestMessage, bool> Predicate, Exception Exception)> _exceptions = new();
    private readonly object _lock = new();

    public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? SendAsyncFunc { get; set; }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Capture the request for later verification
        _capturedRequests.Enqueue(CloneRequest(request));

        // Check for custom send function (highest priority)
        if (SendAsyncFunc != null)
        {
            return await SendAsyncFunc(request, cancellationToken);
        }

        // Check for configured exceptions
        lock (_lock)
        {
            var exception = _exceptions.FirstOrDefault(e => e.Predicate(request));
            if (exception.Exception != null)
            {
                throw exception.Exception;
            }
        }

        // Check for configured responses
        lock (_lock)
        {
            var response = _responses.FirstOrDefault(r => r.Predicate(request));
            if (response.Response != null)
            {
                return CloneResponse(response.Response);
            }
        }

        // Default response for unmatched requests
        return new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent($"No mock response configured for: {request.Method} {request.RequestUri}")
        };
    }

    /// <summary>
    /// Sets a response for requests matching the predicate
    /// </summary>
    public void SetResponse(Func<HttpRequestMessage, bool> predicate, HttpResponseMessage response)
    {
        lock (_lock)
        {
            _responses.Add((predicate, response));
        }
    }

    /// <summary>
    /// Sets an exception to throw for requests matching the predicate
    /// </summary>
    public void SetException(Func<HttpRequestMessage, bool> predicate, Exception exception)
    {
        lock (_lock)
        {
            _exceptions.Add((predicate, exception));
        }
    }

    /// <summary>
    /// Gets all captured requests for verification
    /// </summary>
    public IReadOnlyList<HttpRequestMessage> GetCapturedRequests()
    {
        return _capturedRequests.ToArray();
    }

    /// <summary>
    /// Gets the last captured request
    /// </summary>
    public HttpRequestMessage? GetLastRequest()
    {
        return _capturedRequests.TryPeek(out var request) ? request : null;
    }

    /// <summary>
    /// Gets captured requests matching a predicate
    /// </summary>
    public IEnumerable<HttpRequestMessage> GetCapturedRequests(Func<HttpRequestMessage, bool> predicate)
    {
        return _capturedRequests.Where(predicate);
    }

    /// <summary>
    /// Clears all captured requests and configured responses
    /// </summary>
    public void Reset()
    {
        _capturedRequests.Clear();
        lock (_lock)
        {
            _responses.Clear();
            _exceptions.Clear();
        }
        SendAsyncFunc = null;
    }

    /// <summary>
    /// Verifies that a request was made matching the predicate
    /// </summary>
    public void VerifyRequest(Func<HttpRequestMessage, bool> predicate, string? message = null)
    {
        var matchingRequest = _capturedRequests.FirstOrDefault(predicate);
        if (matchingRequest == null)
        {
            throw new AssertionException(
                message ?? "Expected request was not captured");
        }
    }

    /// <summary>
    /// Verifies the number of requests made
    /// </summary>
    public void VerifyRequestCount(int expectedCount, string? message = null)
    {
        var actualCount = _capturedRequests.Count;
        if (actualCount != expectedCount)
        {
            throw new AssertionException(
                message ?? $"Expected {expectedCount} requests, but {actualCount} were made");
        }
    }

    /// <summary>
    /// Helper method to set up OAuth token endpoint responses
    /// </summary>
    public void SetupTokenEndpoint(HttpStatusCode statusCode, object responseBody)
    {
        var json = JsonSerializer.Serialize(responseBody);
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        SetResponse(request =>
            request.RequestUri?.AbsolutePath.Contains("/oauth/token") == true &&
            request.Method == HttpMethod.Post, response);
    }

    /// <summary>
    /// Helper method to set up API endpoint responses
    /// </summary>
    public void SetupApiEndpoint(string endpoint, HttpStatusCode statusCode, object responseBody)
    {
        var json = JsonSerializer.Serialize(responseBody);
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        SetResponse(request =>
            request.RequestUri?.AbsolutePath.Contains(endpoint) == true, response);
    }

    /// <summary>
    /// Helper method to simulate network timeouts
    /// </summary>
    public void SimulateTimeout()
    {
        SetException(request => true, new TaskCanceledException("The operation was canceled."));
    }

    /// <summary>
    /// Helper method to simulate server errors
    /// </summary>
    public void SimulateServerError()
    {
        SetResponse(request => true, new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Internal Server Error", Encoding.UTF8, "text/plain")
        });
    }

    private static HttpRequestMessage CloneRequest(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri);
        
        foreach (var header in original.Headers)
        {
            clone.Headers.Add(header.Key, header.Value);
        }

        if (original.Content != null)
        {
            var contentBytes = original.Content.ReadAsByteArrayAsync().Result;
            clone.Content = new ByteArrayContent(contentBytes);

            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.Add(header.Key, header.Value);
            }
        }

        foreach (var property in original.Options)
        {
            clone.Options.Set(property.Key, property.Value);
        }

        return clone;
    }

    private static HttpResponseMessage CloneResponse(HttpResponseMessage original)
    {
        var clone = new HttpResponseMessage(original.StatusCode);
        
        foreach (var header in original.Headers)
        {
            clone.Headers.Add(header.Key, header.Value);
        }

        if (original.Content != null)
        {
            var contentBytes = original.Content.ReadAsByteArrayAsync().Result;
            clone.Content = new ByteArrayContent(contentBytes);

            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.Add(header.Key, header.Value);
            }
        }

        clone.ReasonPhrase = original.ReasonPhrase;

        return clone;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Reset();
            
            // Dispose any stored responses
            lock (_lock)
            {
                foreach (var (_, response) in _responses)
                {
                    response?.Dispose();
                }
                _responses.Clear();
            }
        }
        
        base.Dispose(disposing);
    }
}

/// <summary>
/// Custom assertion exception for test verification
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
    public AssertionException(string message, Exception innerException) : base(message, innerException) { }
}