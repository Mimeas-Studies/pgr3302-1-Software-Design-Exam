using TextAnalyzer.UI;

namespace TextAnalyzer.Analyzer;

public class MenuHandler {

    
    public static void menuHandler()
    {
        Console.WriteLine("\nType in menu option number and press <Enter>");
        var selectedMenuOption = Console.ReadLine();
        MainManager mainManager = new MainManager();
        FileManager fileManager = new FileManager();
        CreateNewFiles createNewFiles = new CreateNewFiles();

        switch (selectedMenuOption)
        {
            case "1":
                fileManager.displayStoredFiles();
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
                mainManager.start(fileManager.getSelectedFile());
                Ui.PrintSaveOrDiscard();
                mainManager.SaveFileInDB();
                
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                mainManager.GenerateTxtFile();
                Console.WriteLine("\n Type in (1) to save.\n Type in (2) to discard.");
                Console.ReadLine();
                break;
            
            case "5":
                Console.WriteLine("Exiting...");
                Program.IsProgramRunning = false; 
                break;
        }
    }
}