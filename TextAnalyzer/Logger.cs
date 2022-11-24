using System.Reflection;
using System.Text;

namespace TextAnalyzer;

public class Logger
{
    private static Logger? _logger;
    private LogLevel _level;
    private StreamWriter _logfile;

    private Logger()
    {
        _level = LogLevel.WARN;
        
        string path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath) ?? ".";
        path += "/log.txt";
        _logfile = new StreamWriter(path, append: true);
        _logfile.AutoFlush = true;
    }

    public static void SetLevel(LogLevel level)
    {
        Logger.Instance()._level = level;
    }

    public static void SetLogFile(string path)
    {
        Logger.Instance()._logfile = new StreamWriter(path, append: true);
    }
    
    public static Logger Instance()
    {
        return _logger ??= new Logger();
    }

    public static void Log(LogLevel level, string text)
    {
        Logger logger = Logger.Instance();
        if (level.CompareTo(logger._level) > 0) return;
        
        string message = $"[{DateTime.Now.ToString("d HH:mm:ss.fff")}][{level.ToString()}]: {text}";
        logger._logfile.WriteLine(message);
        logger._logfile.Flush();
    }

    public static void Debug(string text)
    {
        Logger.Log(LogLevel.DEBUG, text);
    }
    
    public static void Info(string text)
    {
        Logger.Log(LogLevel.INFO, text);
    }
    
    public static void Warn(string text)
    {
        Logger.Log(LogLevel.WARN, text);
    }
    
    public static void Error(string text)
    {
        Logger.Log(LogLevel.ERROR, text);
    }
}

public enum LogLevel
{
    ERROR,
    WARN,
    INFO,
    DEBUG
}

static class LogLevelToString
{
    public static string ToString(this LogLevel level)
    {
        switch (level)
        {
            case LogLevel.DEBUG:
                return "DEBUG";
            case LogLevel.INFO:
                return "INFO";
            case LogLevel.WARN:
                return "WARN";
            case LogLevel.ERROR:
                return "ERROR";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}