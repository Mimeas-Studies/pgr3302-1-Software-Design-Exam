using TextAnalyzer.UI;

namespace TextAnalyzer.Analyzer;

public class MenuHandler {

    /// <summary>
    /// Switch, multiway branch statement, transfer execution to different parts of a code based on the stated value expression
    /// Using type and and Console.ReadKey().KeyChar, used to obtain the character key pressed by a user, KeyChar to read
    /// a single character, without waiting for the enter key to be pressed
    /// </summary>
    public static void menuHandler()
    {
        IOManager.Write("\nType in menu option number");
        var selectedMenuOption = Console.ReadKey().KeyChar;
        MainManager mainManager = new MainManager();
        FileManager fileManager = new FileManager();
        CreateNewFiles createNewFiles = new CreateNewFiles();
        switch (selectedMenuOption)
        {
            case '1':
                fileManager.displayStoredFiles();
                Ui.ProgressBar();
                mainManager.Start(fileManager);
                mainManager.SaveFileInDb();
                break;
            
            case '2':
                mainManager.RetriveTitlesOfAnalysedTexts();
                IOManager.Write("");
                Ui.PrintBackToMainMenu();
                selectedMenuOption = Console.ReadKey().KeyChar;
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