namespace TextAnalyzer.UI;

public class IOManager
{
    public static string? Input(string text = "")
    {
        Console.WriteLine(text);
        var input = Console.ReadLine();
        return input;
    }

    public static void Write(string text)
    {
        Console.WriteLine(text); 
    }

    public static void ClearConsole()
    {
        Console.Clear();
    }
    
}