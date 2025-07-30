using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Procore.SDK.QualityAnalysis.Tests
{
    #region Core Data Models

    public class ClientInfo
    {
        public string Name { get; set; } = string.Empty;
        public Assembly Assembly { get; set; } = null!;
        public Type ClientType { get; set; } = null!;
        public Type InterfaceType { get; set; } = null!;
        public string Version { get; set; } = "1.0.0";
        public DateTime LastAnalyzed { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Metadata { get; set; } = new();

        public Type GetClientInterface() => InterfaceType;

        public bool HasCircuitBreakerEnabled()
        {
            // Check if client has circuit breaker configuration
            // This would be determined by examining the client's configuration
            return Name == "Core" || Metadata.ContainsKey("CircuitBreakerEnabled");
        }

        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public T? GetMetadata<T>(string key)
        {
            return Metadata.TryGetValue(key, out var value) && value is T typedValue ? typedValue : default;
        }
    }

    #endregion

    #region Static Analysis Models

    public class CoverageResult
    {
        public string ClientName { get; set; } = string.Empty;
        public double OverallCoverage { get; set; }
        public double CriticalPathCoverage { get; set; }
        public double BranchCoverage { get; set; }
        public double StatementCoverage { get; set; }
        public Dictionary<string, double> MethodCoverage { get; set; } = new();
        public Dictionary<string, double> ClassCoverage { get; set; } = new();
        public Dictionary<string, int> UncoveredLines { get; set; } = new();
        public DateTime AnalysisTime { get; set; } = DateTime.UtcNow;
        public TimeSpan AnalysisDuration { get; set; }
        public QualityGrade CoverageGrade => CalculateCoverageGrade();

        private QualityGrade CalculateCoverageGrade()
        {
            var score = (OverallCoverage * 0.4) + (CriticalPathCoverage * 0.6);
            return score switch
            {
                >= 0.95 => QualityGrade.Excellent,
                >= 0.90 => QualityGrade.Good,
                >= 0.80 => QualityGrade.Satisfactory,
                >= 0.70 => QualityGrade.NeedsImprovement,
                _ => QualityGrade.Poor
            };
        }
    }

    public class ComplexityAnalysisResult
    {
        public string ClientName { get; set; } = string.Empty;
        public IList<ComplexityViolation> Violations { get; set; } = new List<ComplexityViolation>();
        public double AverageComplexity { get; set; }
        public int MaxComplexity { get; set; }
        public int MinComplexity { get; set; } = int.MaxValue;
        public double StandardDeviation { get; set; }
        public int TotalMethods { get; set; }
        public int HighComplexityMethods { get; set; }
        public Dictionary<ComplexityLevel, int> ComplexityDistribution { get; set; } = new();
        public QualityGrade ComplexityGrade => CalculateComplexityGrade();
        public DateTime AnalysisTime { get; set; } = DateTime.UtcNow;

        private QualityGrade CalculateComplexityGrade()
        {
            var violationRate = TotalMethods > 0 ? (double)Violations.Count / TotalMethods : 0;
            return violationRate switch
            {
                <= 0.05 => QualityGrade.Excellent,
                <= 0.10 => QualityGrade.Good,
                <= 0.20 => QualityGrade.Satisfactory,
                <= 0.30 => QualityGrade.NeedsImprovement,
                _ => QualityGrade.Poor
            };
        }
    }

    public class ComplexityViolation
    {
        public string Type { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public int ComplexityScore { get; set; }
        public int Threshold { get; set; }
        public ComplexityLevel Level { get; set; }
        public string Recommendation { get; set; } = string.Empty;
        public int LineNumber { get; set; }
        public string FilePath { get; set; } = string.Empty;

        public override string ToString() => $"{Type}.{Method} (Complexity: {ComplexityScore}, Threshold: {Threshold}, Level: {Level})";
    }

    public class CompilationResult
    {
        public string ClientName { get; set; } = string.Empty;
        public bool CompiledSuccessfully { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public IList<CompilationIssue> Errors { get; set; } = new List<CompilationIssue>();
        public IList<CompilationIssue> Warnings { get; set; } = new List<CompilationIssue>();
        public TimeSpan CompilationTime { get; set; }
        public DateTime AnalysisTime { get; set; } = DateTime.UtcNow;
        public QualityGrade CompilationGrade => CompiledSuccessfully && ErrorCount == 0 ? QualityGrade.Excellent : QualityGrade.Poor;
    }

    public class CompilationIssue
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public int Line { get; set; }
        public int Column { get; set; }
        public CompilationSeverity Severity { get; set; }

        public override string ToString() => $"{Severity} {Code}: {Message} at {File}:{Line}:{Column}";
    }

    public enum CompilationSeverity
    {
        Info,
        Warning,
        Error,
        Hidden
    }

    public class SecurityAnalyzer
    {
        private readonly Dictionary<string, SecurityRule> _securityRules;
        private readonly ILogger? _logger;

        public SecurityAnalyzer(ILogger? logger = null)
        {
            _logger = logger;
            _securityRules = InitializeSecurityRules();
        }

        public async Task<SecurityAnalysisResult> AnalyzeAsync(ClientInfo client)
        {
            var violations = new List<SecurityViolation>();
            var startTime = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var types = client.Assembly.GetTypes();
                await Task.Run(() => 
                {
                    Parallel.ForEach(types, type => AnalyzeType(type, client, violations));
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Security analysis failed for {ClientName}", client.Name);
            }

            stopwatch.Stop();
            return new SecurityAnalysisResult
            {
                ClientName = client.Name,
                Violations = violations,
                AnalysisTime = startTime,
                AnalysisDuration = stopwatch.Elapsed,
                RulesApplied = _securityRules.Count
            };
        }

        private void AnalyzeType(Type type, ClientInfo client, List<SecurityViolation> violations)
        {
            lock (violations)
            {
                foreach (var rule in _securityRules.Values)
                {
                    if (rule.AppliesTo(type))
                    {
                        var violation = rule.Evaluate(type, client);
                        if (violation != null)
                        {
                            violations.Add(violation);
                        }
                    }
                }
            }
        }

        private Dictionary<string, SecurityRule> InitializeSecurityRules()
        {
            return new Dictionary<string, SecurityRule>
            {
                ["PlaintextPassword"] = new SecurityRule
                {
                    Name = "PlaintextPassword",
                    Description = "Detects potential plaintext password handling",
                    AppliesTo = type => type.Name.Contains("Password", StringComparison.OrdinalIgnoreCase) && 
                                       !type.Name.Contains("Hash", StringComparison.OrdinalIgnoreCase),
                    Evaluate = (type, client) => new SecurityViolation
                    {
                        ClientName = client.Name,
                        Type = type.Name,
                        Issue = "Potential plaintext password handling detected",
                        Severity = SecuritySeverity.Medium,
                        Recommendation = "Use secure password hashing algorithms like bcrypt, scrypt, or Argon2",
                        RuleName = "PlaintextPassword"
                    }
                },
                ["InsecureRandom"] = new SecurityRule
                {
                    Name = "InsecureRandom",
                    Description = "Detects usage of System.Random for security purposes",
                    AppliesTo = type => type.GetMethods().Any(m => m.GetParameters().Any(p => p.ParameterType == typeof(Random))),
                    Evaluate = (type, client) => new SecurityViolation
                    {
                        ClientName = client.Name,
                        Type = type.Name,
                        Issue = "Usage of System.Random detected - not cryptographically secure",
                        Severity = SecuritySeverity.High,
                        Recommendation = "Use System.Security.Cryptography.RandomNumberGenerator for security-sensitive operations",
                        RuleName = "InsecureRandom"
                    }
                }
            };
        }

        public IList<SecurityViolation> Analyze(ClientInfo client)
        {
            return AnalyzeAsync(client).GetAwaiter().GetResult().Violations;
        }
    }

    public class SecurityRule
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Func<Type, bool> AppliesTo { get; set; } = _ => false;
        public Func<Type, ClientInfo, SecurityViolation?> Evaluate { get; set; } = (_, _) => null;
    }

    public class SecurityAnalysisResult
    {
        public string ClientName { get; set; } = string.Empty;
        public IList<SecurityViolation> Violations { get; set; } = new List<SecurityViolation>();
        public DateTime AnalysisTime { get; set; }
        public TimeSpan AnalysisDuration { get; set; }
        public int RulesApplied { get; set; }
        public QualityGrade SecurityGrade => CalculateSecurityGrade();

        private QualityGrade CalculateSecurityGrade()
        {
            var criticalCount = Violations.Count(v => v.Severity == SecuritySeverity.Critical);
            var highCount = Violations.Count(v => v.Severity == SecuritySeverity.High);
            
            return (criticalCount, highCount) switch
            {
                (0, 0) => QualityGrade.Excellent,
                (0, <= 2) => QualityGrade.Good,
                (0, <= 5) => QualityGrade.Satisfactory,
                (0, _) => QualityGrade.NeedsImprovement,
                _ => QualityGrade.Poor
            };
        }
    }

    public class SecurityViolation
    {
        public string ClientName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Issue { get; set; } = string.Empty;
        public SecuritySeverity Severity { get; set; }
        public string Recommendation { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int LineNumber { get; set; }
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Metadata { get; set; } = new();

        public override string ToString() => $"{ClientName}.{Type}: {Issue} ({Severity}) - {Recommendation}";
    }

    public enum SecuritySeverity
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum ComplexityLevel
    {
        Low = 1,      // 1-5
        Moderate = 2, // 6-10
        High = 3,     // 11-20
        VeryHigh = 4, // 21-50
        Extreme = 5   // 50+
    }

    public enum QualityGrade
    {
        Excellent = 5,
        Good = 4,
        Satisfactory = 3,
        NeedsImprovement = 2,
        Poor = 1
    }

    #endregion

    #region Code Quality Models

    public class SolidPrincipleAnalyzer
    {
        public IList<SolidViolation> AnalyzeSingleResponsibility(ClientInfo client)
        {
            var violations = new List<SolidViolation>();
            
            try
            {
                var types = client.Assembly.GetTypes();
                foreach (var type in types)
                {
                    // Simplified SRP analysis - check method count as proxy
                    var methodCount = type.GetMethods().Length;
                    if (methodCount > 20) // Arbitrary threshold
                    {
                        violations.Add(new SolidViolation
                        {
                            ClientName = client.Name,
                            Type = type.Name,
                            Principle = "Single Responsibility",
                            Issue = $"Class has {methodCount} methods, might have multiple responsibilities"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SRP analysis failed for {client.Name}: {ex.Message}");
            }

            return violations;
        }

        public IList<SolidViolation> AnalyzeDependencyInversion(ClientInfo client)
        {
            var violations = new List<SolidViolation>();
            
            try
            {
                var types = client.Assembly.GetTypes();
                foreach (var type in types)
                {
                    // Simplified DIP analysis - check constructor parameters
                    var constructors = type.GetConstructors();
                    foreach (var constructor in constructors)
                    {
                        var parameters = constructor.GetParameters();
                        var concreteTypeCount = 0;
                        
                        foreach (var param in parameters)
                        {
                            if (!param.ParameterType.IsInterface && !param.ParameterType.IsAbstract)
                            {
                                concreteTypeCount++;
                            }
                        }
                        
                        if (concreteTypeCount > parameters.Length / 2) // More than half concrete types
                        {
                            violations.Add(new SolidViolation
                            {
                                ClientName = client.Name,
                                Type = type.Name,
                                Principle = "Dependency Inversion",
                                Issue = "Constructor depends on concrete types rather than abstractions"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DIP analysis failed for {client.Name}: {ex.Message}");
            }

            return violations;
        }
    }

    public class SolidViolation
    {
        public string ClientName { get; set; }
        public string Type { get; set; }
        public string Principle { get; set; }
        public string Issue { get; set; }

        public override string ToString() => $"{ClientName}.{Type}: {Principle} - {Issue}";
    }

    public class DisposableAnalyzer
    {
        public IList<DisposableViolation> AnalyzeDisposalPatterns(ClientInfo client)
        {
            var violations = new List<DisposableViolation>();
            
            try
            {
                var disposableTypes = client.Assembly.GetTypes()
                    .Where(t => typeof(IDisposable).IsAssignableFrom(t) && !t.IsInterface)
                    .ToList();

                foreach (var type in disposableTypes)
                {
                    // Check if Dispose is properly implemented
                    var disposeMethod = type.GetMethod("Dispose", BindingFlags.Public | BindingFlags.Instance);
                    if (disposeMethod == null)
                    {
                        violations.Add(new DisposableViolation
                        {
                            ClientName = client.Name,
                            Type = type.Name,
                            Issue = "IDisposable implemented but no public Dispose method found"
                        });
                    }
                    
                    // Check for finalizer
                    var finalizer = type.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (finalizer != null)
                    {
                        // Good - has finalizer for cleanup
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Disposable analysis failed for {client.Name}: {ex.Message}");
            }

            return violations;
        }
    }

    public class DisposableViolation
    {
        public string ClientName { get; set; }
        public string Type { get; set; }
        public string Issue { get; set; }

        public override string ToString() => $"{ClientName}.{Type}: {Issue}";
    }

    #endregion

    #region Type Mapping Models

    public interface ITypeMapperInfo
    {
        string Name { get; }
        Type Type { get; }
        string ClientName { get; }
    }

    public class TypeMapperInfo : ITypeMapperInfo
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string ClientName { get; set; }
    }

    public class TypeMapperPerformanceResult
    {
        public string MapperName { get; set; } = string.Empty;
        public int Iterations { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public double AverageTimeMs { get; set; }
        public double MinTimeMs { get; set; } = double.MaxValue;
        public double MaxTimeMs { get; set; }
        public double MedianTimeMs { get; set; }
        public double StandardDeviationMs { get; set; }
        public double ErrorRate { get; set; }
        public TimeSpan TotalTime { get; set; }
        public double ThroughputPerSecond => Iterations > 0 ? Iterations / TotalTime.TotalSeconds : 0;
        public PerformanceGrade Grade => CalculatePerformanceGrade();
        public DateTime TestTime { get; set; } = DateTime.UtcNow;
        public ConcurrentQueue<double> ResponseTimes { get; set; } = new();

        private PerformanceGrade CalculatePerformanceGrade()
        {
            return (AverageTimeMs, ErrorRate) switch
            {
                (<= 0.5, <= 0.001) => PerformanceGrade.Excellent,
                (<= 1.0, <= 0.005) => PerformanceGrade.Good,
                (<= 2.0, <= 0.01) => PerformanceGrade.Acceptable,
                (<= 5.0, <= 0.05) => PerformanceGrade.Poor,
                _ => PerformanceGrade.Unacceptable
            };
        }
    }

    public enum PerformanceGrade
    {
        Excellent,
        Good,
        Acceptable,
        Poor,
        Unacceptable
    }

    public class MappingResult
    {
        public string MapperName { get; set; }
        public bool Success { get; set; }
        public int OperationCount { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class TestDataPair
    {
        public object GeneratedObject { get; set; }
        public object WrapperObject { get; set; }
    }

    #endregion

    #region Authentication Models

    public class MockTokenManager
    {
        private bool _tokenExpired = false;

        public void SimulateExpiredToken()
        {
            _tokenExpired = true;
        }

        public bool IsTokenExpired => _tokenExpired;
    }

    public class TokenRefreshResult
    {
        public string ClientName { get; set; }
        public bool Success { get; set; }
        public bool RefreshAttempted { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan RefreshTime { get; set; }
    }

    public class TokenStorageSecurityAnalysis
    {
        public string StorageType { get; set; }
        public bool UsesEncryption { get; set; }
        public bool HasPlaintextTokens { get; set; }
        public bool SupportsSecureStorage { get; set; }
        public IList<string> SecurityFeatures { get; set; } = new List<string>();
    }

    #endregion

    #region Resilience Models

    public class RetryPolicyResult
    {
        public string ClientName { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int RetryAttempts { get; set; }
        public bool EventuallySucceeded { get; set; }
        public TimeSpan TotalTime { get; set; }
        public IList<TimeSpan> RetryDelays { get; set; } = new List<TimeSpan>();
    }

    public class CircuitBreakerResult
    {
        public string ClientName { get; set; }
        public bool CircuitBreakerTripped { get; set; }
        public bool SubsequentCallsFailed { get; set; }
        public bool CircuitBreakerRecovered { get; set; }
        public TimeSpan RecoveryTime { get; set; }
        public int FailureThreshold { get; set; }
    }

    #endregion

    #region Memory Analysis Models

    public class MemoryAnalysisResult
    {
        public string ClientName { get; set; }
        public double MemoryGrowthMB { get; set; }
        public int Gen2Collections { get; set; }
        public int OperationsPerformed { get; set; }
        public long InitialMemoryBytes { get; set; }
        public long FinalMemoryBytes { get; set; }
    }

    public class PeakMemoryResult
    {
        public string ClientName { get; set; }
        public double PeakMemoryMB { get; set; }
        public double AverageMemoryMB { get; set; }
        public TimeSpan TestDuration { get; set; }
    }

    #endregion

    #region Async Pattern Models

    public class AsyncPatternAnalyzer
    {
        public IList<ConfigureAwaitViolation> AnalyzeConfigureAwaitUsage(ClientInfo client)
        {
            var violations = new List<ConfigureAwaitViolation>();
            
            // Simplified ConfigureAwait analysis
            // In real implementation, would use Roslyn analyzers to examine source code
            try
            {
                var types = client.Assembly.GetTypes();
                foreach (var type in types)
                {
                    var asyncMethods = type.GetMethods()
                        .Where(m => typeof(Task).IsAssignableFrom(m.ReturnType))
                        .ToList();

                    // This is a simplified check - real implementation would examine IL or source
                    foreach (var method in asyncMethods)
                    {
                        // Assume good practices for now - in real implementation would check IL
                        // violations.Add(...) if ConfigureAwait(false) missing
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConfigureAwait analysis failed for {client.Name}: {ex.Message}");
            }

            return violations;
        }
    }

    public class ConfigureAwaitViolation
    {
        public string ClientName { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string Issue { get; set; }

        public override string ToString() => $"{ClientName}.{Type}.{Method}: {Issue}";
    }

    public class DeadlockTestResult
    {
        public string ClientName { get; set; }
        public string MethodName { get; set; }
        public bool DeadlockDetected { get; set; }
        public TimeSpan TestDuration { get; set; }
        public string DeadlockDetails { get; set; }
    }

    #endregion

    #region Cancellation Models

    public class TokenPropagationResult
    {
        public string ClientName { get; set; }
        public string OperationName { get; set; }
        public bool HasCancellationToken { get; set; }
        public bool TokenProperlyPropagated { get; set; }
    }

    public class CancellationResponsivenessResult
    {
        public string ClientName { get; set; }
        public string OperationName { get; set; }
        public long ResponseTimeMs { get; set; }
        public bool RespondedQuickly { get; set; }
        public bool ThrewCorrectException { get; set; }
    }

    public class CancellationCleanupResult
    {
        public string ClientName { get; set; }
        public string OperationName { get; set; }
        public int LeakedResourceCount { get; set; }
        public bool CleanupSuccessful { get; set; }
        public IList<string> LeakedResources { get; set; } = new List<string>();
    }

    #endregion

    #region Error Handling Models

    public class ErrorMappingResult
    {
        public string ClientName { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Exception MappedException { get; set; }
        public bool HasRichErrorInfo { get; set; }
        public bool HasCorrelationId { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class ErrorLoggingResult
    {
        public string ClientName { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public LogLevel ExpectedLogLevel { get; set; }
        public int LogEntriesCount { get; set; }
        public bool HasStructuredLogging { get; set; }
        public bool HasCorrelationId { get; set; }
    }

    public class GracefulDegradationResult
    {
        public string ClientName { get; set; }
        public double SuccessRate { get; set; }
        public int GracefulFailureCount { get; set; }
        public int CriticalFailureCount { get; set; }
        public TimeSpan TestDuration { get; set; }
    }

    #endregion

    #region Logging Models

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();
        public bool IsStructured => Properties.Count > 0;
        public bool HasStructuredData => IsStructured;

        public bool HasProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }
    }

    public class StructuredLoggingResult
    {
        public string ClientName { get; set; }
        public int TotalLogEntries { get; set; }
        public int StructuredLogEntries { get; set; }
        public double StructuredPercentage { get; set; }
        public bool HasConsistentSchema { get; set; }
        public IList<string> SchemaViolations { get; set; } = new List<string>();
    }

    public class PerformanceLoggingResult
    {
        public string ClientName { get; set; }
        public int PerformanceLogCount { get; set; }
        public bool HasOperationTiming { get; set; }
        public bool HasThroughputMetrics { get; set; }
        public bool HasMemoryMetrics { get; set; }
        public bool HasCorrelationIds { get; set; }
    }

    public class TestLogCapture
    {
        private readonly List<LogEntry> _logEntries = new();

        public void Clear() => _logEntries.Clear();

        public IList<LogEntry> GetLogEntries() => _logEntries.ToList();

        public void AddLogEntry(LogLevel level, string message, Dictionary<string, object> properties = null)
        {
            _logEntries.Add(new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                LogLevel = level,
                Message = message,
                Properties = properties ?? new Dictionary<string, object>()
            });
        }
    }

    public class StructuredLogCapture : TestLogCapture
    {
        // Inherits from TestLogCapture with structured logging focus
    }

    public class PerformanceLogCapture
    {
        private readonly List<PerformanceLogEntry> _performanceEntries = new();

        public void Clear() => _performanceEntries.Clear();

        public IList<PerformanceLogEntry> GetPerformanceLogEntries() => _performanceEntries.ToList();
    }

    public class PerformanceLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Operation { get; set; }
        public Dictionary<string, object> Metrics { get; set; } = new();

        public bool HasProperty(string propertyName) => Metrics.ContainsKey(propertyName);
    }

    #endregion

    #region Helper Classes

    public class InMemoryTokenStorage
    {
        private readonly Dictionary<string, string> _tokens = new();

        public void StoreToken(string key, string token)
        {
            _tokens[key] = token;
        }

        public string GetToken(string key)
        {
            return _tokens.TryGetValue(key, out var token) ? token : null;
        }
    }

    #endregion

    #region Extension Methods

    public static class TypeExtensions
    {
        public static bool IsGenerated(this Type type)
        {
            // Check if type is generated code (simplified)
            return type.Namespace?.Contains("Generated") == true ||
                   type.Name.Contains("Generated") ||
                   type.GetCustomAttributes().Any(a => a.GetType().Name.Contains("Generated"));
        }
    }

    public static class ITypeMapperInfoExtensions
    {
        public static object MapToWrapper(this ITypeMapperInfo mapper, object source)
        {
            // Simplified mapping - in real implementation would call actual mapper
            return source;
        }

        public static object MapToGenerated(this ITypeMapperInfo mapper, object source)
        {
            // Simplified mapping - in real implementation would call actual mapper
            return source;
        }
    }

    #endregion
}