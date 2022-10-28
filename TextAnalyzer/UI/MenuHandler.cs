namespace TextAnalyzer.Analyzer;

public class MenuHandler
{
    internal static void menuHandler()
    {
        Console.WriteLine("\nType in menu option number and press <Enter>");
        var selectedMenuOption = Console.ReadLine();

        switch (selectedMenuOption)
        {
            case "1":
                Console.WriteLine("Analyzing Text ...");
                using (var progress = new ProgressBar())
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        progress.Report((double)i / 100);
                        Thread.Sleep(20);
                    }
                }
                break;
            case "2":
                
                break;
            case "3":
                break;
            case "4":
                Console.WriteLine("Exiting...");
                Program.IsProgramRunning = false; 
                break;
        }
    }
}