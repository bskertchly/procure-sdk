using System;
using System.Collections.Generic;
using System.Linq;

namespace Procore.SDK.QualityAnalysis.Tests
{
    /// <summary>
    /// Configuration for quality gate thresholds and validation rules
    /// </summary>
    public class QualityGateConfiguration
    {
        public double MinimumOverallCoverage { get; set; } = 0.80;
        public double MinimumCriticalPathCoverage { get; set; } = 0.90;
        public double MinimumBranchCoverage { get; set; } = 0.75;
        public double MaxComplexityViolationRate { get; set; } = 0.05;
        public double MaxAverageComplexity { get; set; } = 8.0;
        public int MaxHighSeveritySecurityIssues { get; set; } = 2;
        public double MaxAverageResponseTimeMs { get; set; } = 2.0;
        public double MaxErrorRate { get; set; } = 0.01;
        public double MinThroughputPerSecond { get; set; } = 500.0;
        public int MaxCompilationWarnings { get; set; } = 10;
        public double MaxMemoryGrowthMB { get; set; } = 10.0;
        public int MaxGen2Collections { get; set; } = 5;

        public static QualityGateConfiguration Default => new();

        public static QualityGateConfiguration Strict => new()
        {
            MinimumOverallCoverage = 0.90,
            MinimumCriticalPathCoverage = 0.95,
            MinimumBranchCoverage = 0.85,
            MaxComplexityViolationRate = 0.02,
            MaxAverageComplexity = 6.0,
            MaxHighSeveritySecurityIssues = 0,
            MaxAverageResponseTimeMs = 1.0,
            MaxErrorRate = 0.005,
            MinThroughputPerSecond = 1000.0,
            MaxCompilationWarnings = 5,
            MaxMemoryGrowthMB = 5.0,
            MaxGen2Collections = 3
        };
    }

    /// <summary>
    /// Comprehensive data container for quality assessment across all dimensions
    /// </summary>
    public class QualityAssessmentData
    {
        public string ClientName { get; set; } = string.Empty;
        public CoverageResult? CoverageResult { get; set; }
        public ComplexityAnalysisResult? ComplexityResult { get; set; }
        public SecurityAnalysisResult? SecurityResult { get; set; }
        public CompilationResult? CompilationResult { get; set; }
        public IList<TypeMapperPerformanceResult> PerformanceResults { get; set; } = new List<TypeMapperPerformanceResult>();
        public IList<MemoryAnalysisResult> MemoryResults { get; set; } = new List<MemoryAnalysisResult>();
        public IList<RetryPolicyResult> ResilienceResults { get; set; } = new List<RetryPolicyResult>();
        public DateTime AssessmentTime { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new();

        public void AddMetric(string key, object value)
        {
            AdditionalMetrics[key] = value;
        }

        public T? GetMetric<T>(string key)
        {
            return AdditionalMetrics.TryGetValue(key, out var value) && value is T typedValue ? typedValue : default;
        }
    }

    /// <summary>
    /// Result of a single quality gate validation
    /// </summary>
    public class QualityGateResult
    {
        public string GateName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public double Score { get; set; } // 0-100 scale
        public string Details { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new();
        public TimeSpan ValidationTime { get; set; }
        public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Metadata { get; set; } = new();

        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public override string ToString()
        {
            return $"{GateName}: {(Passed ? "PASS" : "FAIL")} (Score: {Score:F1})";
        }
    }

    /// <summary>
    /// Comprehensive results from all quality gate validations
    /// </summary>
    public class QualityGateResults
    {
        public string ClientName { get; set; } = string.Empty;
        public DateTime AssessmentStartTime { get; set; }
        public TimeSpan TotalValidationTime { get; set; }
        public List<QualityGateResult> GateResults { get; set; } = new();
        public double OverallQualityScore { get; set; } // 0-100 scale
        public QualityGrade OverallGrade { get; set; }
        public bool PassedAllGates { get; set; }
        public string? ValidationError { get; set; }
        public Dictionary<string, object> SummaryMetrics { get; set; } = new();

        public int PassedGatesCount => GateResults.Count(g => g.Passed);
        public int FailedGatesCount => GateResults.Count(g => !g.Passed);
        public int TotalGatesCount => GateResults.Count;
        public double PassRate => TotalGatesCount > 0 ? (double)PassedGatesCount / TotalGatesCount : 0;

        public QualityGateResult? GetGateResult(string gateName)
        {
            return GateResults.FirstOrDefault(g => g.GateName.Equals(gateName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<QualityGateResult> GetFailedGates()
        {
            return GateResults.Where(g => !g.Passed);
        }

        public IEnumerable<string> GetAllRecommendations()
        {
            return GateResults.SelectMany(g => g.Recommendations).Distinct();
        }

        public void AddSummaryMetric(string key, object value)
        {
            SummaryMetrics[key] = value;
        }

        public string GenerateQualityReport()
        {
            var report = new System.Text.StringBuilder();
            
            report.AppendLine($"=== Quality Assessment Report for {ClientName} ===");
            report.AppendLine($"Assessment Time: {AssessmentStartTime:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine($"Total Validation Time: {TotalValidationTime.TotalMilliseconds:F0}ms");
            report.AppendLine($"Overall Score: {OverallQualityScore:F1}/100");
            report.AppendLine($"Overall Grade: {OverallGrade}");
            report.AppendLine($"Gates Passed: {PassedGatesCount}/{TotalGatesCount} ({PassRate:P})");
            report.AppendLine();

            report.AppendLine("=== Gate Results ===");
            foreach (var gate in GateResults.OrderByDescending(g => g.Score))
            {
                var status = gate.Passed ? "✓ PASS" : "✗ FAIL";
                report.AppendLine($"{status} {gate.GateName}: {gate.Score:F1} - {gate.Details}");
                
                if (gate.Recommendations.Any())
                {
                    foreach (var recommendation in gate.Recommendations)
                    {
                        report.AppendLine($"  → {recommendation}");
                    }
                }
            }

            var allRecommendations = GetAllRecommendations().ToList();
            if (allRecommendations.Any())
            {
                report.AppendLine();
                report.AppendLine("=== Summary Recommendations ===");
                foreach (var recommendation in allRecommendations)
                {
                    report.AppendLine($"• {recommendation}");
                }
            }

            if (!string.IsNullOrEmpty(ValidationError))
            {
                report.AppendLine();
                report.AppendLine($"=== Validation Error ===");
                report.AppendLine(ValidationError);
            }

            return report.ToString();
        }
    }

    /// <summary>
    /// Quality metrics aggregator for comprehensive analysis across all clients
    /// </summary>
    public class QualityMetricsAggregator
    {
        private readonly List<QualityGateResults> _allResults = new();

        public void AddResult(QualityGateResults result)
        {
            _allResults.Add(result);
        }

        public QualityMetricsSummary GenerateSummary()
        {
            if (!_allResults.Any())
            {
                return new QualityMetricsSummary
                {
                    TotalClientsAnalyzed = 0,
                    GeneratedAt = DateTime.UtcNow
                };
            }

            var summary = new QualityMetricsSummary
            {
                TotalClientsAnalyzed = _allResults.Count,
                ClientsPassingAllGates = _allResults.Count(r => r.PassedAllGates),
                AverageQualityScore = _allResults.Average(r => r.OverallQualityScore),
                HighestQualityScore = _allResults.Max(r => r.OverallQualityScore),
                LowestQualityScore = _allResults.Min(r => r.OverallQualityScore),
                TotalValidationTime = TimeSpan.FromMilliseconds(_allResults.Sum(r => r.TotalValidationTime.TotalMilliseconds)),
                GeneratedAt = DateTime.UtcNow
            };

            // Calculate grade distribution
            foreach (var result in _allResults)
            {
                summary.GradeDistribution[result.OverallGrade] = 
                    summary.GradeDistribution.GetValueOrDefault(result.OverallGrade) + 1;
            }

            // Find most common failing gates
            var allFailedGates = _allResults.SelectMany(r => r.GetFailedGates());
            summary.MostCommonFailures = allFailedGates
                .GroupBy(g => g.GateName)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Count());

            // Aggregate recommendations
            summary.TopRecommendations = _allResults
                .SelectMany(r => r.GetAllRecommendations())
                .GroupBy(rec => rec)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select(g => g.Key)
                .ToList();

            return summary;
        }

        public void Reset()
        {
            _allResults.Clear();
        }
    }

    /// <summary>
    /// Summary of quality metrics across all analyzed clients
    /// </summary>
    public class QualityMetricsSummary
    {
        public int TotalClientsAnalyzed { get; set; }
        public int ClientsPassingAllGates { get; set; }
        public double AverageQualityScore { get; set; }
        public double HighestQualityScore { get; set; }
        public double LowestQualityScore { get; set; }
        public TimeSpan TotalValidationTime { get; set; }
        public DateTime GeneratedAt { get; set; }
        public Dictionary<QualityGrade, int> GradeDistribution { get; set; } = new();
        public Dictionary<string, int> MostCommonFailures { get; set; } = new();
        public List<string> TopRecommendations { get; set; } = new();

        public double PassRate => TotalClientsAnalyzed > 0 ? 
            (double)ClientsPassingAllGates / TotalClientsAnalyzed : 0;

        public string GenerateExecutiveSummary()
        {
            var summary = new System.Text.StringBuilder();
            
            summary.AppendLine("=== Executive Quality Summary ===");
            summary.AppendLine($"Generated: {GeneratedAt:yyyy-MM-dd HH:mm:ss}");
            summary.AppendLine($"Clients Analyzed: {TotalClientsAnalyzed}");
            summary.AppendLine($"Overall Pass Rate: {PassRate:P} ({ClientsPassingAllGates}/{TotalClientsAnalyzed})");
            summary.AppendLine($"Average Quality Score: {AverageQualityScore:F1}/100");
            summary.AppendLine($"Quality Range: {LowestQualityScore:F1} - {HighestQualityScore:F1}");
            summary.AppendLine($"Total Analysis Time: {TotalValidationTime.TotalSeconds:F1} seconds");
            summary.AppendLine();

            if (GradeDistribution.Any())
            {
                summary.AppendLine("=== Grade Distribution ===");
                foreach (var grade in Enum.GetValues<QualityGrade>().OrderByDescending(g => g))
                {
                    var count = GradeDistribution.GetValueOrDefault(grade);
                    if (count > 0)
                    {
                        summary.AppendLine($"{grade}: {count} ({(double)count / TotalClientsAnalyzed:P})");
                    }
                }
                summary.AppendLine();
            }

            if (MostCommonFailures.Any())
            {
                summary.AppendLine("=== Most Common Gate Failures ===");
                foreach (var failure in MostCommonFailures)
                {
                    summary.AppendLine($"{failure.Key}: {failure.Value} failures");
                }
                summary.AppendLine();
            }

            if (TopRecommendations.Any())
            {
                summary.AppendLine("=== Top Recommendations ===");
                foreach (var recommendation in TopRecommendations.Take(5))
                {
                    summary.AppendLine($"• {recommendation}");
                }
            }

            return summary.ToString();
        }
    }
}