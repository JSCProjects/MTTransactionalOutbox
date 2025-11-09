using Microsoft.Extensions.Logging;

namespace MTTransactionalOutbox.Infrastructure.Logging;

public class FileLoggerProvider(string path) : ILoggerProvider
{
    private readonly object _lock = new();

    public ILogger CreateLogger(string categoryName) => new FileLogger(path, _lock);

    public void Dispose() { }

    private class FileLogger(string path, object locker) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}";
            lock (locker)
            {
                File.AppendAllText(path, message + Environment.NewLine);
            }
        }
    }
}