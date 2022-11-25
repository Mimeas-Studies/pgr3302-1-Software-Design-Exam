namespace TextAnalyzer.UI;

public static class Ui
{
    internal static void PrintMenu()
    {
        IOManager.Write("1. Analyze Texts");
        IOManager.Write("2. Get text stats");
        IOManager.Write("3. Write your own text");
        IOManager.Write("4. Exit");
    }

    internal static void PrintSaveOrDiscard()
    {
        IOManager.Write("1. Save data");
        IOManager.Write("2. Discard data");
    }

    internal static void ProgressBar()
    {
        IOManager.Write("Analyzing Text ...");
        using (var progress = new ProgressBar())
        {
            for (int i = 0; i <= 100; i++)
            {
                progress.Report((double)i / 100);
                Thread.Sleep(20);
            }
        }

        IOManager.ClearConsole();
    }

    internal static void PrintBackToMainMenu()
    {
        IOManager.Write("1. Back to main menu");
    }
}