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
                // getting value written in Console.WriteLine 
                Console.WriteLine("Enter text: ");
                string? text = Console.ReadLine(); 
                // file handling 
 
                StreamWriter writeText = new StreamWriter("Samples/Sample.txt"); 
                writeText.WriteLine(text); 
                writeText.Flush(); 
                writeText.Close(); 
                
                Console.WriteLine("Return to main menu...");

                break;
            
            case "5":
                Console.WriteLine("Exiting...");
                Program.IsProgramRunning = false; 
                break;
            default:// do nothing;
                break;
        }
    }
}