using TextAnalyzer.Analyzer;

namespace TextAnalyzer;

public class Program
{
    internal static bool IsProgramRunning = true;

    public static void Main(String[] args)
    {
        while (IsProgramRunning)
        {
            Ui.PrintMenu();
            MenuHandler.menuHandler();
            Console.Clear();
        }
    }
}