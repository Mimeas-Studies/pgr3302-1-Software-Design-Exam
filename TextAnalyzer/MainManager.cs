using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.FileReader;
using TextAnalyzer.UI;
using TextAnalyzer.Logging;

namespace TextAnalyzer;

/// <summary>
/// Used as a facade, creates and instance of many classes in the project and make them cooperate.
/// </summary>
public class MainManager
{
    private readonly FileManager? _fileManager = new FileManager();
    private AnalyzerManager? _analyzerManager;
    private AnalyzerResult? _analyzerResult;
    private readonly IDbManager? _dbManager = new SqliteDb();
    private static bool _isProgramRunning = true;

    private void ReadAndAnalyseFile(FileManager fileManager)
    {
        IEnumerator<string> textStream = FileManager.GetText(fileManager.GetSelectedFile());
        _analyzerManager = new AnalyzerManager(textStream, 8);

        _analyzerResult = _analyzerManager.StartAnalyze();
        _analyzerResult.SourceName = fileManager.RetrieveAllFileNames();
    }

    private void SaveFileInDb()
    {
        IoManager.Write(_analyzerResult?.ToString());
        Ui.PrintSaveOrDiscard();
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                _dbManager?.SaveData(_analyzerResult!);
                IoManager.ClearConsole();
                IoManager.Write("Data stored\n");
                break;
            case "2":
                IoManager.ClearConsole();
                IoManager.Write("Data discarded\n");
                break;
        }
    }

    private void GenerateTxtFile()
    {
        CreateNewFiles.CreateTxtFiles();
    }

    private bool RetrieveTitlesOfAnalysedTexts()
    {
        var retrieveData = false;
        IoManager.ClearConsole();
        IoManager.Write("Names of analysed text.");
        var counter = 0;
        var analyzerResultsList = _dbManager?.GetAll();
        for (var i = 0; i < analyzerResultsList!.Count; i++)
        {
            counter++;
            IoManager.Write(counter + ". " + analyzerResultsList[i].SourceName);
        }

        IoManager.Write("\nType in menu number to see stats and press <Enter>");
        IoManager.Write("Type in <B> to go back press <Enter>");
        var selectedTxtFile = Console.ReadLine();
        if (selectedTxtFile.ToUpper() == "B")
        {
            return retrieveData;
        }
        else if (selectedTxtFile.Any(char.IsLetter))
        {
            return retrieveData;
        }

        retrieveData = true;
        IoManager.ClearConsole();
        IoManager.Write("Stats of analysed text:");
        Console.WriteLine(analyzerResultsList[Convert.ToInt32(selectedTxtFile) - 1]);
        return retrieveData;
    }

    private void ShowAnalysedTexts()
    {
        IoManager.ClearConsole();
        if (_fileManager.DisplayStoredFiles())
        {
            Ui.ProgressBar();
            ReadAndAnalyseFile(_fileManager);
            SaveFileInDb();
        }
    }

    private void RetrieveTextStats()
    {
        IoManager.ClearConsole();
        if (RetrieveTitlesOfAnalysedTexts())
        {
            Ui.PrintBackToMainMenu();
            var i = Convert.ToInt32(Console.ReadLine());
            IoManager.ClearConsole();
            if (i != 1)
            {
                _isProgramRunning = false;
            }
        }
    }

    private void WriteYourOwnText()
    {
        GenerateTxtFile();
    }

    private void EndProgram()
    {
        IoManager.Write("\nExiting...");
        _isProgramRunning = false;
    }

    private void Menu()
    {
        IoManager.ClearConsole();
        IoManager.Write("\nType in menu option number");
        Ui.PrintMenu();
        var selectedMenuOption = Console.ReadKey().KeyChar;
        switch (selectedMenuOption)
        {
            case '1':
                IoManager.ClearConsole();
                ShowAnalysedTexts();
                break;

            case '2':
                IoManager.ClearConsole();
                RetrieveTextStats();
                break;

            case '3':
                IoManager.ClearConsole();
                WriteYourOwnText();
                
                break;

            case '4':
                IoManager.ClearConsole();
                EndProgram();
                break;
        }
    }

    public static void Main(string[] args)
    {
        Logger.SetLevel(LogLevel.Info);
        Logger.Info("Initializing Application");

        MainManager mainManager = new();
        
        //Infinite while loop of the main menu switch case, while isProgramRunning set to true,
        //false value set to five in switch case, exiting program '
        Logger.Info("Running main loop");
        while (_isProgramRunning)
        {
            mainManager.Menu();
        }

        Logger.Info("Exited Application");
    }
}