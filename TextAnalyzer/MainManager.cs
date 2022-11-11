using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.UI;

namespace TextAnalyzer; 

public class MainManager {
    private AnalyzerManager? _analyzerManager;
    private AnalyzerResult analyzerResult;
    private IDbManager dbManager = new SqliteDb();

    public void ReadAndAnalyseFile(FileManager fileManager) {
        IEnumerator<string> text = FileManager.GetText(fileManager.GetSelectedFile());
        _analyzerManager = new AnalyzerManager(text, 0);
        
        analyzerResult = _analyzerManager.StartAnalyze();
        analyzerResult.SourceName = fileManager.RetriveAllFileNames();
    }

    public void SaveFileInDb() {
        Console.WriteLine(analyzerResult.ToString());
        Ui.PrintSaveOrDiscard();
        var option = Console.ReadLine();
        switch (option) {
            case "1":
                dbManager.SaveData(analyzerResult);
                Console.Clear();
                IOManager.Write("Data stored\n");
                break;
            case "2":
                Console.Clear();
                IOManager.Write("Data discarded\n");
                break;
        }

    }
    public void GenerateTxtFile()
    { 
        CreateNewFiles.CreateTxtFiles();
    }

    public void RetriveTitlesOfAnalysedTexts() {
        IOManager.ClearConsole();
        IOManager.Write("Names of analysed text.");
        var counter = 0;
        var analyzerResultsList = dbManager.GetAll();
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
}