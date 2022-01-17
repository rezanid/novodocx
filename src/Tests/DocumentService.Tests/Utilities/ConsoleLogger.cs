using Microsoft.Extensions.Logging;
using System;

namespace Novo.DocumentService.Tests.Utilities;
public class ConsoleLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => new NullLogScope<TState>(state);
    public bool IsEnabled(LogLevel logLevel) =>  throw new NotImplementedException();
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) => throw new NotImplementedException();
}

public class NullLogScope<TState> : IDisposable
{
    private readonly TState _state;
    public NullLogScope(TState  state)
    {
        _state = state;
        Console.WriteLine($"Begin scope: {_state}");
    }
    public void Dispose() => Console.WriteLine($"End of scope {_state}");
}
