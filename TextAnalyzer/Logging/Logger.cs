using System.Reflection;
using TextAnalyzer.Logging;

namespace TextAnalyzer;

public class Logger
{
    private static Logger? _logger;
    private LogLevel _level;
    private StreamWriter _logfile;

    private Logger()
    {
        _level = LogLevel.Info;

        string path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath) ?? ".";
        path += "/log.txt";
        _logfile = new StreamWriter(path, append: true);
        _logfile.AutoFlush = true;
        
        // add spacing to logfile for clarity
        _logfile.Write("\n\n\n");
    }

    public static void SetLevel(LogLevel level)
    {
        Logger.Instance()._level = level;
    }

    public static void SetLogFile(string path)
    {
        Logger.Instance()._logfile = new StreamWriter(path, append: true);
    }

    // Instancing can be private since the instance only utilized through
    // through the convenience methods bellow
    private static Logger Instance()
    {
        return _logger ??= new Logger();
    }

    // Actual log formatting
    public static void Log(LogLevel level, string text)
    {
        Logger logger = Logger.Instance();
        if (level.CompareTo(logger._level) > 0) return;

        string message = $"[{DateTime.Now.ToString("d HH:mm:ss.fff")}][{level.ToString()}]: {text}";
        logger._logfile.WriteLine(message);
        logger._logfile.Flush();
    }

    #region Covenience methods

    /// <summary>
    ///     Use 'Debug' when logging behavior of a function
    /// </summary>
    /// <param name="text"></param>
    public static void Debug(string text)
    {
        Logger.Log(LogLevel.Debug, text);
    }

    /// <summary>
    ///     Use 'Info' when logging behavior for the program as a whole
    /// </summary>
    /// <param name="text"></param>
    public static void Info(string text)
    {
        Logger.Log(LogLevel.Info, text);
    }

    /// <summary>
    ///     Use 'Warn' when encountering an unexpected but recoverable problem 
    /// </summary>
    /// <param name="text"></param>
    public static void Warn(string text)
    {
        Logger.Log(LogLevel.Warn, text);
    }

    /// <summary>
    ///      Use 'Error' when encountering errors?
    /// </summary>
    /// <param name="text"></param>
    public static void Error(string text)
    {
        Logger.Log(LogLevel.Error, text);
    }

    /// <summary>
    ///     Use 'Trace' to monitor often recurring function or changing variables
    /// </summary>
    /// <param name="text"></param>
    public static void Trace(string text)
    {
        Logger.Log(LogLevel.Trace, text);
    }

    #endregion
}