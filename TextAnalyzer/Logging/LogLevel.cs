namespace TextAnalyzer.Logging;

public enum LogLevel
{
    Error,
    Warn,
    Info,
    Debug,
    Trace
}

static class LogLevelMethods
{
    public static string ToString(this LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => "DEBUG",
            LogLevel.Info => "INFO",
            LogLevel.Warn => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Trace => "TRACE",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}