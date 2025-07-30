using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Procore.SDK.QualityAnalysis.Tests
{
    /// <summary>
    /// Extension methods to support the CQ Task 9 quality analysis tests
    /// </summary>
    public static class TestExtensions
    {

        /// <summary>
        /// Extension method to check if a log entry contains sensitive data
        /// </summary>
        public static bool ContainsSensitiveData(this PerformanceLogEntry entry)
        {
            var sensitiveKeywords = new[] { "password", "secret", "key", "token", "credential" };
            
            var messageContainsSensitive = entry.Operation?.ToLowerInvariant()
                .Split(' ')
                .Any(word => sensitiveKeywords.Contains(word)) ?? false;
                
            var metricsContainsSensitive = entry.Metrics?.Values
                .OfType<string>()
                .Any(value => sensitiveKeywords.Any(keyword => 
                    value.ToLowerInvariant().Contains(keyword))) ?? false;
                    
            return messageContainsSensitive || metricsContainsSensitive;
        }

        /// <summary>
        /// Extension method to check if a log entry contains sensitive data
        /// </summary>
        public static bool ContainsSensitiveData(this LogEntry entry)
        {
            var sensitiveKeywords = new[] { "password", "secret", "key", "token", "credential" };
            
            var messageContainsSensitive = entry.Message?.ToLowerInvariant()
                .Split(' ')
                .Any(word => sensitiveKeywords.Contains(word)) ?? false;
                
            var propertiesContainsSensitive = entry.Properties?.Values
                .OfType<string>()
                .Any(value => sensitiveKeywords.Any(keyword => 
                    value.ToLowerInvariant().Contains(keyword))) ?? false;
                    
            return messageContainsSensitive || propertiesContainsSensitive;
        }

        /// <summary>
        /// Helper method to determine if a ClientInfo supports circuit breaker patterns
        /// </summary>
        public static bool SupportsCircuitBreaker(this ClientInfo client)
        {
            // Simplified logic to determine circuit breaker support
            return client.Name.Contains("Core") || 
                   client.Name.Contains("ProjectManagement") ||
                   client.GetMetadata<bool>("HasCircuitBreaker");
        }

        /// <summary>
        /// Helper method to get async operations from a client
        /// </summary>
        public static IEnumerable<MethodInfo> GetAsyncOperations(this Type clientType)
        {
            return clientType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => typeof(System.Threading.Tasks.Task).IsAssignableFrom(m.ReturnType))
                .Where(m => !m.IsSpecialName);
        }

        /// <summary>
        /// Helper method to simulate realistic type mapping performance
        /// </summary>
        public static TimeSpan SimulateTypeMappingTime(this ITypeMapperInfo mapper)
        {
            // Simulate realistic mapping times based on mapper type
            var baseTime = mapper.Name.Contains("Complex") ? 2.0 : 0.5; // milliseconds
            var jitter = Random.Shared.NextDouble() * 0.5; // Add some randomness
            return TimeSpan.FromMilliseconds(baseTime + jitter);
        }

        /// <summary>
        /// Helper method to categorize quality grade based on numeric score
        /// </summary>
        public static QualityGrade ToQualityGrade(this double score)
        {
            return score switch
            {
                >= 0.95 => QualityGrade.Excellent,
                >= 0.85 => QualityGrade.Good, 
                >= 0.75 => QualityGrade.Satisfactory,
                >= 0.60 => QualityGrade.NeedsImprovement,
                _ => QualityGrade.Poor
            };
        }

        /// <summary>
        /// Helper method to categorize performance grade based on timing
        /// </summary>
        public static PerformanceGrade ToPerformanceGrade(this TimeSpan timing)
        {
            var milliseconds = timing.TotalMilliseconds;
            return milliseconds switch
            {
                <= 0.5 => PerformanceGrade.Excellent,
                <= 1.0 => PerformanceGrade.Good,
                <= 2.0 => PerformanceGrade.Acceptable, 
                <= 5.0 => PerformanceGrade.Poor,
                _ => PerformanceGrade.Unacceptable
            };
        }

        /// <summary>
        /// Helper method to create test correlation IDs
        /// </summary>
        public static string CreateCorrelationId(this string prefix = "test")
        {
            return $"{prefix}-{Guid.NewGuid():N}";
        }

        /// <summary>
        /// Helper to validate log schema consistency
        /// </summary>
        public static bool HasConsistentSchema(this IEnumerable<LogEntry> entries)
        {
            if (!entries.Any()) return true;

            var requiredProperties = new[] { "Timestamp", "ClientName", "Operation" };
            var firstEntry = entries.First();
            var requiredKeys = firstEntry.Properties.Keys.Intersect(requiredProperties).ToList();
            
            return entries.All(entry => 
                requiredKeys.All(key => entry.Properties.ContainsKey(key)));
        }

        /// <summary>
        /// Helper to create mock test data for type mapping
        /// </summary>
        public static object CreateMockData(this string dataType)
        {
            return dataType.ToLowerInvariant() switch
            {
                "company" => new { Id = 1, Name = "Test Company", CreatedAt = DateTime.UtcNow },
                "user" => new { Id = 1, Email = "test@example.com", Name = "Test User" },
                "project" => new { Id = 1, Name = "Test Project", Status = "Active" },
                "document" => new { Id = 1, Title = "Test Document", Content = "Test content" },
                _ => new { Id = 1, Name = "Test Object", Type = dataType }
            };
        }

        /// <summary>
        /// Helper to simulate HTTP errors for testing
        /// </summary>
        public static Exception CreateHttpException(this System.Net.HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                System.Net.HttpStatusCode.NotFound => new System.Net.Http.HttpRequestException("Resource not found"),
                System.Net.HttpStatusCode.Unauthorized => new UnauthorizedAccessException("Access denied"),
                System.Net.HttpStatusCode.InternalServerError => new InvalidOperationException("Server error"),
                System.Net.HttpStatusCode.BadRequest => new ArgumentException("Bad request"),
                System.Net.HttpStatusCode.Forbidden => new UnauthorizedAccessException("Forbidden"),
                _ => new System.Net.Http.HttpRequestException($"HTTP {(int)statusCode} error")
            };
        }
    }
}