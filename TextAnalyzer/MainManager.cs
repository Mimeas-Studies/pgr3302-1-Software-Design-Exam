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

    private void ReadAndAnalyseFile(string filename)
    {
        IEnumerator<string> textStream = FileManager.GetText(filename);
        _analyzerManager = new AnalyzerManager(textStream, 8);

        _analyzerResult = _analyzerManager.StartAnalyze();
        _analyzerResult.SourceName = filename;
    }

    private void SaveFileInDb()
    {
        while (true)
        {
            IOManager.Write(_analyzerResult?.ToString());
            string? option = IOManager.Input("Save Data?(y/n)")?.ToLower();
            switch (option)
            {
                case "y":
                    _dbManager?.SaveData(_analyzerResult!);
                    return;
                case "n":
                    return;
                default:
                    continue;
            }
        }

    }

    private void GenerateTxtFile()
    {
        CreateNewFiles.CreateTxtFiles();
    }

    private bool RetrieveTitlesOfAnalysedTexts()
    {
        
        var analyzerResultsList = _dbManager?
            .GetAll()
            .Select((result, i)=> (i, result))
            .ToList()
            ?? new List<(int i, AnalyzerResult result)>(); // use empty list if _dbManager is null
        
        while (true)
        {
            IOManager.ClearConsole();
            IOManager.Write("Names of analysed text.");
            foreach ((int i, AnalyzerResult result) in analyzerResultsList)
            {
                IOManager.Write($"{i+1}. {result.SourceName}");
            }
         
            IOManager.Write("\nType in menu number to see stats and press <Enter>");

            string? input = IOManager.Input("Type in <B> to go back press <Enter>");
            if (string.IsNullOrWhiteSpace(input)) continue;

            if (input.Any(c => !char.IsNumber(c))) return false;

            int selected = int.Parse(input) - 1;
            if (selected < 0 || selected >= analyzerResultsList.Count) continue;

            IOManager.ClearConsole();
            IOManager.Write("Stats of analysed text:");
            IOManager.Write(analyzerResultsList[selected].result.ToString());
            return true;
        }

    }

    private void ShowAnalysedTexts()
    {
        IOManager.ClearConsole();
        string? selectedFile = _fileManager.ChooseStoredFile();
        
        if (selectedFile is null) return;
        
        Ui.ProgressBar();
        ReadAndAnalyseFile(selectedFile);
        SaveFileInDb();
    }

    private void RetrieveTextStats()
    {
        IoManager.ClearConsole();
        if (RetrieveTitlesOfAnalysedTexts())
        {
            IOManager.Input("Type enter to go back to main menu");
        }
    }

    private void WriteYourOwnText()
    {
        GenerateTxtFile();
    }

    private void EndProgram()
    {
        IOManager.Write("\nExiting...");
    }

    private void Menu()
    {
        while (true)
        {
            IOManager.ClearConsole();
            Ui.PrintMenu();
            string? selectedMenuOption = IOManager.Input("Type in menu option number");
            switch (selectedMenuOption)
            {
                case "1":
                    IOManager.ClearConsole();
                    ShowAnalysedTexts();
                    break;

                case "2":
                    IOManager.ClearConsole();
                    RetrieveTextStats();
                    break;

                case "3":
                    IOManager.ClearConsole();
                    WriteYourOwnText();
                    break;
                
                case "4":
                    IOManager.ClearConsole();
                    EndProgram();
                    return;
                default:
                    continue;
            }
        }
    }

    public static void Main(string[] args)
    {
        Logger.SetLevel(LogLevel.Info);
        Logger.Info("Initializing Application");

        MainManager mainManager = new();
        
        Logger.Info("Running main loop");
        mainManager.Menu();

        Logger.Info("Exited Application");
    }
}