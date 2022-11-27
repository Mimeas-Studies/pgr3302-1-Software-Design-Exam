namespace TextAnalyzer.UI;

/// <summary>
/// IOManager, user-provided objects, stores assets, outputs and loads them as inputs to downstream assets
/// </summary>
public static class IoManager
{
//Input, writes user specified data to standard input stream, and reads the characters from the input stream
    public static string? Input(string text = "")
    {
        Console.WriteLine(text);
        var input = Console.ReadLine();
        return input;
    }

//Standard writeline, writes specified data to input stream
    public static void Write(string text)
    {
        Console.WriteLine(text);
    }

//Clears console screen and buffer of display information 
    public static void ClearConsole()
    {
        Console.Clear();
    }
}