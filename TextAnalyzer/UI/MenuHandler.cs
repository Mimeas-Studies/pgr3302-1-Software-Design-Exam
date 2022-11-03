using TextAnalyzer.UI;

namespace TextAnalyzer.Analyzer;

public class MenuHandler {

    
    public static void menuHandler()
    {
        Console.WriteLine("\nType in menu option number and press <Enter>");
        var selectedMenuOption = Console.ReadKey().KeyChar;
        MainManager mainManager = new MainManager();
        FileManager fileManager = new FileManager();
        CreateNewFiles createNewFiles = new CreateNewFiles();

        switch (selectedMenuOption)
        {
            case '1':
                fileManager.displayStoredFiles();
                Ui.ProgressBar();
                mainManager.start(fileManager.getSelectedFile());
                Ui.PrintSaveOrDiscard();
                mainManager.SaveFileInDB();
                break;
            
            case '2':
                break;
            
            case '3':
                break;
            
            case '4':
                mainManager.GenerateTxtFile();
                Ui.PrintSaveOrDiscard();
                Console.ReadLine();
                break;
            
            case '5':
                Console.WriteLine("\nExiting...");
                Program.IsProgramRunning = false; 
                break;
        }
    }
}