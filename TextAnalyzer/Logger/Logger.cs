namespace TextAnalyzer.Logger;

public class Logger
{
    private static Logger? _logger;


    private Logger()
    {
    }
    
    public Logger Instance()
    {
        return _logger ??= new Logger();
    }

    public void Info(string text)
    {
        string message = $"[{DateTime.Now: u}]INFO: {text}";
        
        Console.WriteLine(message);
    }
    
}