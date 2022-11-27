﻿namespace TextAnalyzer.UI;

public static class Ui
{
    internal static void PrintMenu()
    {
        IoManager.Write("1. Analyze Texts");
        IoManager.Write("2. Get text stats");
        IoManager.Write("3. Write your own text");
        IoManager.Write("4. Exit");
    }

    internal static void PrintSaveOrDiscard()
    {
        IoManager.Write("1. Save data");
        IoManager.Write("2. Discard data");
    }

    internal static void ProgressBar()
    {
        IoManager.Write("Analyzing Text ...");
        using (var progress = new ProgressBar())
        {
            for (int i = 0; i <= 100; i++)
            {
                progress.Report((double)i / 100);
                Thread.Sleep(20);
            }
        }

        IoManager.ClearConsole();
    }

    internal static void PrintBackToMainMenu()
    {
        IoManager.Write("1. Back to main menu");
    }
}