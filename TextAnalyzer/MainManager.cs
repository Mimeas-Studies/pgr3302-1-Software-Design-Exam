using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.UI;

namespace TextAnalyzer; 

/// <summary>
/// Used as a facade, creates and instance of many classes in the project and make them cooperate.
/// </summary>
public class MainManager {
    private FileManager? _fileManager = new FileManager();
    private AnalyzerManager? _analyzerManager;
    private AnalyzerResult? _analyzerResult;
    private readonly IDbManager? _dbManager = new SqliteDb();
    internal static bool IsProgramRunning = true;

    public void ReadAndAnalyseFile(FileManager fileManager) {
        IEnumerator<string> textStream = FileManager.GetText(fileManager.GetSelectedFile());
        _analyzerManager = new AnalyzerManager(textStream, 8);
        
        _analyzerResult = _analyzerManager.StartAnalyze();
        _analyzerResult.SourceName = fileManager.RetriveAllFileNames();
    }

    private void SaveFileInDb() {
        Console.WriteLine(_analyzerResult?.ToString());
        Ui.PrintSaveOrDiscard();
        var option = Console.ReadLine();
        switch (option) {
            case "1":
                _dbManager?.SaveData(_analyzerResult);
                Console.Clear();
                IOManager.Write("Data stored\n");
                break;
            case "2":
                Console.Clear();
                IOManager.Write("Data discarded\n");
                break;
        }

    }
    private void GenerateTxtFile()
    { 
        CreateNewFiles.CreateTxtFiles();
    }

    private void RetrieveTitlesOfAnalysedTexts() {
        IOManager.ClearConsole();
        IOManager.Write("Names of analysed text.");
        var counter = 0;
        var analyzerResultsList = _dbManager?.GetAll();
        for (var i = 0; i < analyzerResultsList.Count; i++) {
            counter++;
            IOManager.Write(counter+". "+analyzerResultsList[i].SourceName);
        }
        IOManager.Write("\nType in menu number to see stats and press <Enter>");
        var selectedTxtFile = Convert.ToInt32(Console.ReadLine());
        Console.Clear();
        IOManager.Write("Stats of analysed text:");
        Console.WriteLine(analyzerResultsList[selectedTxtFile - 1]);
        Console.WriteLine();

    }

    private void ShowAnalysedTexts() {
        _fileManager.DisplayStoredFiles();
        Ui.ProgressBar();
        ReadAndAnalyseFile(_fileManager);
        SaveFileInDb();
    }

    private void RetrieveTextStats() {
        RetrieveTitlesOfAnalysedTexts();
        Ui.PrintBackToMainMenu();
        var i = Convert.ToInt32(Console.ReadLine());
        Console.Clear();
        if (i != 1) {
            IsProgramRunning = false; 
        }
    }

    private void WriteYourOwnText() {
        GenerateTxtFile();
        Ui.PrintSaveOrDiscard();
        Console.ReadLine();
    }

    private void EndProgram() {
        IOManager.Write("\nExiting...");
        IsProgramRunning = false; 
    }

    public void Menu() {
        IOManager.Write("\nType in menu option number");
        Ui.PrintMenu();
        var selectedMenuOption = Console.ReadKey().KeyChar;
        switch (selectedMenuOption)
        {
            case '1':
                ShowAnalysedTexts();
                break;
            
            case '2':
                RetrieveTextStats();
                break;
            
            case '3':
                WriteYourOwnText();
                break;
            
            case '4':
                EndProgram();
                break;
        }
    }
    
    public static void Main(String[] args) {
        Logger.SetLevel(LogLevel.DEBUG);
        
        Logger.Debug("Initializing Application");
        //Infinite while loop of the main menu switch case, while isProgramRunning set to true,
        //false value set to five in switch case, exiting program '
        var mainManager = new MainManager();
        
        Logger.Debug("Application started");
        while (IsProgramRunning)
        {
            mainManager.Menu();
        }
        
        Logger.Info("Exited Application");
    }
    
}