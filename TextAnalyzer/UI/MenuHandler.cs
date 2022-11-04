using TextAnalyzer.UI;

namespace TextAnalyzer.Analyzer;

public class MenuHandler {

    
    public static void menuHandler()
    {
        IOManager.Write("\nType in menu option number and press <Enter>");
        var selectedMenuOption = Console.ReadKey().KeyChar;
        MainManager mainManager = new MainManager();
        FileManager fileManager = new FileManager();
        CreateNewFiles createNewFiles = new CreateNewFiles();

        switch (selectedMenuOption)
        {
            case '1':
                fileManager.DisplayStoredFiles();
                Ui.ProgressBar();
                mainManager.ReadAndAnalyseFile(fileManager);
                mainManager.SaveFileInDb();
                break;
            
            case '2':
                mainManager.RetriveTitlesOfAnalysedTexts();
                IOManager.Write("");
                Ui.PrintBackToMainMenu();
                var i = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                if (i != 1) {
                    Program.IsProgramRunning = false; 
                } 
                
                break;
            
            case '3':
                break;
            
            case '4':
                mainManager.GenerateTxtFile();
                Ui.PrintSaveOrDiscard();
                Console.ReadLine();
                break;
            
            case '5':
                IOManager.Write("\nExiting...");
                Program.IsProgramRunning = false; 
                break;
        }
    }
}