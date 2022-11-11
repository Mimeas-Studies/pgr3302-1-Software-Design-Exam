using TextAnalyzer.UI;

namespace TextAnalyzer;

public class Ui
{
    internal static void PrintMenu()
    {
        Console.WriteLine("1. Analyze Texts");
        Console.WriteLine("2. Get text stats");
        Console.WriteLine("3. Write your own text");
        Console.WriteLine("4. Exit");
    }

    internal static void PrintSaveOrDiscard() {
        IOManager.Write("1. Save data");
        IOManager.Write("2. Discard data");
    }

    internal static void ProgressBar()
    {
        Console.WriteLine("Analyzing Text ...");
        using (var progress = new ProgressBar())
        {
            for (int i = 0; i <= 100; i++)
            {
                progress.Report((double)i / 100);
                Thread.Sleep(20);
            }
        }
        Console.Clear();
    }

    internal static void PrintBackToMainMenu() {
        Console.WriteLine("1. Back to main menu");
    }
}