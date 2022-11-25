using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
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
        IOManager.Write(_analyzerResult?.ToString());
        Ui.PrintSaveOrDiscard();
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                _dbManager?.SaveData(_analyzerResult!);
                IOManager.ClearConsole();
                IOManager.Write("Data stored\n");
                break;
            case "2":
                IOManager.ClearConsole();
                IOManager.Write("Data discarded\n");
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
        IOManager.ClearConsole();
        IOManager.Write("Names of analysed text.");
        var counter = 0;
        var analyzerResultsList = _dbManager?.GetAll();
        for (var i = 0; i < analyzerResultsList!.Count; i++)
        {
            counter++;
            IOManager.Write(counter + ". " + analyzerResultsList[i].SourceName);
        }

        IOManager.Write("\nType in menu number to see stats and press <Enter>");
        IOManager.Write("Type in <B> to go back press <Enter>");
        var selectedTxtFile = Console.ReadLine();
        if (selectedTxtFile.ToUpper() == "B")
        {
            return retrieveData;
        }
        else if (selectedTxtFile.Any((x) => char.IsLetter(x)))
        {
            return retrieveData;
        }

        retrieveData = true;
        IOManager.ClearConsole();
        IOManager.Write("Stats of analysed text:");
        Console.WriteLine(analyzerResultsList[Convert.ToInt32(selectedTxtFile) - 1]);
        return retrieveData;
    }

    private void ShowAnalysedTexts()
    {
        IOManager.ClearConsole();
        if (_fileManager.DisplayStoredFiles())
        {
            Ui.ProgressBar();
            ReadAndAnalyseFile(_fileManager);
            SaveFileInDb();
        }
    }

    private void RetrieveTextStats()
    {
        IOManager.ClearConsole();
        if (RetrieveTitlesOfAnalysedTexts())
        {
            Ui.PrintBackToMainMenu();
            var i = Convert.ToInt32(Console.ReadLine());
            IOManager.ClearConsole();
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
        IOManager.Write("\nExiting...");
        _isProgramRunning = false;
    }

    private void Menu()
    {
        IOManager.ClearConsole();
        IOManager.Write("\nType in menu option number");
        Ui.PrintMenu();
        var selectedMenuOption = Console.ReadKey().KeyChar;
        switch (selectedMenuOption)
        {
            case '1':
                IOManager.ClearConsole();
                ShowAnalysedTexts();
                break;

            case '2':
                IOManager.ClearConsole();
                RetrieveTextStats();
                break;

            case '3':
                IOManager.ClearConsole();
                WriteYourOwnText();
                
                break;

            case '4':
                IOManager.ClearConsole();
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