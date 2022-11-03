namespace TextAnalyzer.UI;

public class IOManager
{
    public static string? Input(string text = "")
    {
        Console.Write(text);
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

    public static char QueryConsole(char[] expected, string text = "")
    {
        var fullText = text + " " + "(" + string.Join(", ", expected) + "): ";
        var input = GetKey(fullText);

        while (!expected.Contains(char.ToLower(input)))
        {
            input = GetKey(fullText);
        }
        
        return input;
    }

    private static char GetKey(string text = "")
    {
        Console.Write(text);
        var input = Console.ReadKey().KeyChar;
        return input;
    }
    
}