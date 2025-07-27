using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Serilog.Context;

namespace Procore.SDK.Core.Resilience;

/// <summary>
/// HTTP message handler that applies resilience policies to Procore API requests.
/// </summary>
public class ProcoreResilienceHandler : DelegatingHandler
{
    private readonly PolicyFactory _policyFactory;
    private readonly ILogger<ProcoreResilienceHandler> _logger;
    private readonly ResilienceOptions _options;

    public ProcoreResilienceHandler(
        PolicyFactory policyFactory,
        ILogger<ProcoreResilienceHandler> logger,
        IOptions<ResilienceOptions> options)
    {
        _policyFactory = policyFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Create resilience context for this operation
        var operation = $"{request.Method} {request.RequestUri?.PathAndQuery}";
        var correlationId = GetOrCreateCorrelationId(request);
        var context = new ResilienceContext(operation, correlationId);

        // Add correlation ID to logging context
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Get the appropriate resilience policy
            var policy = _policyFactory.CreateHttpPolicy(context);

            try
            {
                // Execute the request with resilience policies
                var response = await policy.ExecuteAsync(async () =>
                {
                    var startTime = DateTimeOffset.UtcNow;
                    
                    try
                    {
                        var httpResponse = await base.SendAsync(request, cancellationToken);
                        
                        // Log successful request metrics
                        if (_options.Logging.LogPerformanceMetrics)
                        {
                            var duration = DateTimeOffset.UtcNow - startTime;
                            LogRequestMetrics(context, httpResponse.StatusCode, duration, true);
                        }
                        
                        return httpResponse;
                    }
                    catch (Exception ex)
                    {
                        // Log failed request metrics
                        if (_options.Logging.LogPerformanceMetrics)
                        {
                            var duration = DateTimeOffset.UtcNow - startTime;
                            LogRequestMetrics(context, null, duration, false);
                        }
                        
                        context.LastException = ex;
                        throw;
                    }
                });

                return response;
            }
            catch (BrokenCircuitException ex)
            {
                _logger.LogError(ex,
                    "Circuit breaker is open for operation {Operation} (CorrelationId: {CorrelationId})",
                    context.Operation, context.CorrelationId);
                
                throw new Models.ServiceUnavailableException(
                    "Service is temporarily unavailable due to repeated failures", 
                    TimeSpan.FromSeconds(_options.CircuitBreaker.DurationOfBreakInSeconds),
                    correlationId);
            }
            catch (TimeoutRejectedException ex)
            {
                _logger.LogError(ex,
                    "Operation {Operation} (CorrelationId: {CorrelationId}) timed out after {ElapsedTime}ms",
                    context.Operation, context.CorrelationId, context.ElapsedTime.TotalMilliseconds);
                
                throw new TimeoutException($"Request timed out after {context.ElapsedTime.TotalSeconds:F1} seconds", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "HTTP request failed for operation {Operation} (CorrelationId: {CorrelationId}) after {AttemptNumber} attempts",
                    context.Operation, context.CorrelationId, context.AttemptNumber + 1);
                
                // Let the error mapper handle HTTP exceptions
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error in operation {Operation} (CorrelationId: {CorrelationId}) after {ElapsedTime}ms",
                    context.Operation, context.CorrelationId, context.ElapsedTime.TotalMilliseconds);
                
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the correlation ID from the request headers or creates a new one.
    /// </summary>
    private static string GetOrCreateCorrelationId(HttpRequestMessage request)
    {
        const string correlationIdHeader = "X-Correlation-ID";
        
        if (request.Headers.TryGetValues(correlationIdHeader, out var values))
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
        }

        // Create a new correlation ID and add it to the request
        var correlationId = Guid.NewGuid().ToString();
        request.Headers.Add(correlationIdHeader, correlationId);
        return correlationId;
    }

    /// <summary>
    /// Logs request performance metrics.
    /// </summary>
    private void LogRequestMetrics(
        ResilienceContext context,
        System.Net.HttpStatusCode? statusCode,
        TimeSpan duration,
        bool success)
    {
        if (success && statusCode.HasValue)
        {
            _logger.LogInformation(
                "Request completed successfully for operation {Operation} (CorrelationId: {CorrelationId}) " +
                "in {DurationMs}ms with status {StatusCode} after {AttemptNumber} attempts",
                context.Operation,
                context.CorrelationId,
                duration.TotalMilliseconds,
                (int)statusCode.Value,
                context.AttemptNumber + 1);
        }
        else
        {
            _logger.LogWarning(
                "Request failed for operation {Operation} (CorrelationId: {CorrelationId}) " +
                "after {DurationMs}ms on attempt {AttemptNumber}",
                context.Operation,
                context.CorrelationId,
                duration.TotalMilliseconds,
                context.AttemptNumber + 1);
        }
    }
}