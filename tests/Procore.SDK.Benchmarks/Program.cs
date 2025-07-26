using BenchmarkDotNet.Running;

namespace Procore.SDK.Benchmarks;

/// <summary>
/// Entry point for running performance benchmarks
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<AuthenticationBenchmarks>(args: args);
    }
}