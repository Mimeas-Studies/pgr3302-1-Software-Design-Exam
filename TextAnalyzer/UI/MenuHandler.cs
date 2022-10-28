namespace TextAnalyzer.Analyzer;

public class MenuHandler {

    
    public static void menuHandler()
    {
        Console.WriteLine("\nType in menu option number and press <Enter>");
        var selectedMenuOption = Console.ReadLine();
        MainManager mainManager = new MainManager();

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
                Console.Clear();
                break;
            case "2":
                mainManager.start();
                break;
            case "3":
                break;
            case "4":
                try
                {
                    StreamWriter sw = new StreamWriter(
                        "Samples/Sample.txt"
                        );
                
                    Console.WriteLine("Enter text: ");
                    string text = Console.ReadLine();
                    sw.WriteLine(text);
                    sw.Flush();
                    sw.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
                break;
            
            case "5":
                Console.WriteLine("Exiting...");
                Program.IsProgramRunning = false; 
                break;
        }
    }
}