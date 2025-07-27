namespace Procore.SDK.Resilience.Tests.Helpers;

/// <summary>
/// Test HTTP message handler that allows injection of custom response logic.
/// Supports both synchronous and asynchronous response functions.
/// </summary>
public class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handlerFunc;
    private readonly List<HttpRequestMessage> _sentRequests = new();

    public bool IsDisposed { get; private set; }
    public IReadOnlyList<HttpRequestMessage> SentRequests => _sentRequests.AsReadOnly();

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handlerFunc)
        : this(request => Task.FromResult(handlerFunc(request)))
    {
    }

    public TestHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
    {
        _handlerFunc = handlerFunc ?? throw new ArgumentNullException(nameof(handlerFunc));
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        _sentRequests.Add(request);
        
        try
        {
            return await _handlerFunc(request).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Ensure HttpRequestException includes status code information
            if (ex is HttpRequestException httpEx && !httpEx.Data.Contains("StatusCode"))
            {
                httpEx.Data["StatusCode"] = HttpStatusCode.InternalServerError;
                httpEx.Data["RequestPath"] = request.RequestUri?.PathAndQuery;
                httpEx.Data["RequestMethod"] = request.Method.Method;
            }
            throw;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            IsDisposed = true;
            // Dispose any request content to prevent memory leaks
            foreach (var request in _sentRequests)
            {
                request.Content?.Dispose();
            }
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Gets the cancellation token from the HTTP request message.
    /// This is useful for testing timeout scenarios.
    /// </summary>
    public static CancellationToken GetCancellationToken(HttpRequestMessage request)
    {
        // Try to get cancellation token from request options
        return request.Options.TryGetValue(new HttpRequestOptionsKey<CancellationToken>("CancellationToken"), out var token) 
            ? token 
            : CancellationToken.None;
    }

    /// <summary>
    /// Creates a test handler that simulates network timeouts.
    /// </summary>
    public static TestHttpMessageHandler CreateTimeoutHandler(TimeSpan delay)
    {
        return new TestHttpMessageHandler(async request =>
        {
            var cancellationToken = GetCancellationToken(request);
            await Task.Delay(delay, cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
    }

    /// <summary>
    /// Creates a test handler that fails for the first N requests, then succeeds.
    /// </summary>
    public static TestHttpMessageHandler CreateFailThenSucceedHandler(int failureCount, HttpStatusCode failureStatusCode = HttpStatusCode.InternalServerError)
    {
        var requestCount = 0;
        return new TestHttpMessageHandler(request =>
        {
            Interlocked.Increment(ref requestCount);
            
            if (requestCount <= failureCount)
            {
                var exception = new HttpRequestException($"Request failed (attempt {requestCount})");
                exception.Data["StatusCode"] = failureStatusCode;
                throw exception;
            }
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": true}", System.Text.Encoding.UTF8, "application/json")
            };
        });
    }

    /// <summary>
    /// Creates a test handler that returns specific status codes in sequence.
    /// </summary>
    public static TestHttpMessageHandler CreateStatusCodeSequenceHandler(params HttpStatusCode[] statusCodes)
    {
        var requestCount = 0;
        return new TestHttpMessageHandler(request =>
        {
            var currentCount = Interlocked.Increment(ref requestCount);
            var statusCode = statusCodes[Math.Min(currentCount - 1, statusCodes.Length - 1)];
            
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent($"{{\"status\": \"{statusCode}\"}}", System.Text.Encoding.UTF8, "application/json")
            };
        });
    }
}