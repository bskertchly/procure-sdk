using Microsoft.Extensions.Logging;

namespace Procore.SDK.Core.Tests.Helpers;

/// <summary>
/// Test logger provider that captures log entries for assertion in tests.
/// </summary>
public class TestLoggerProvider : ILoggerProvider
{
    private readonly List<LogEntry> _logEntries = new();
    private readonly object _lock = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(categoryName, _logEntries, _lock);
    }

    public List<LogEntry> GetLogEntries()
    {
        lock (_lock)
        {
            return _logEntries.ToList();
        }
    }

    public void ClearLogs()
    {
        lock (_lock)
        {
            _logEntries.Clear();
        }
    }

    public void Dispose()
    {
        // No resources to dispose
    }
}

/// <summary>
/// Test logger implementation that captures log entries.
/// </summary>
public class TestLogger : ILogger
{
    private readonly string _categoryName;
    private readonly List<LogEntry> _logEntries;
    private readonly object _lock;

    public TestLogger(string categoryName, List<LogEntry> logEntries, object lockObject)
    {
        _categoryName = categoryName;
        _logEntries = logEntries;
        _lock = lockObject;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_lock)
        {
            _logEntries.Add(new LogEntry
            {
                Level = logLevel,
                EventId = eventId,
                CategoryName = _categoryName,
                Message = formatter(state, exception),
                State = state,
                Exception = exception,
                Timestamp = DateTimeOffset.UtcNow
            });
        }
    }
}

/// <summary>
/// Represents a captured log entry.
/// </summary>
public class LogEntry
{
    public LogLevel Level { get; set; }
    public EventId EventId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object? State { get; set; }
    public Exception? Exception { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public override string ToString()
    {
        var result = $"[{Timestamp:HH:mm:ss.fff}] {Level}: {Message}";
        if (Exception != null)
        {
            result += $"\nException: {Exception}";
        }
        return result;
    }
}

/// <summary>
/// Extension methods for easier log entry assertions.
/// </summary>
public static class LogEntryExtensions
{
    public static IEnumerable<LogEntry> WithLevel(this IEnumerable<LogEntry> entries, LogLevel level)
    {
        return entries.Where(e => e.Level == level);
    }

    public static IEnumerable<LogEntry> WithMessage(this IEnumerable<LogEntry> entries, string messageSubstring)
    {
        return entries.Where(e => e.Message.Contains(messageSubstring, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<LogEntry> WithException<T>(this IEnumerable<LogEntry> entries) where T : Exception
    {
        return entries.Where(e => e.Exception is T);
    }

    public static IEnumerable<LogEntry> WithCategory(this IEnumerable<LogEntry> entries, string categoryName)
    {
        return entries.Where(e => e.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
    }
}