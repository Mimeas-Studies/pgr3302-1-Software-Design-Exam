using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.UI;

namespace TextAnalyzer; 

public class MainManager {
    private AnalyzerManager _analyzerManager;
    private AnalyzerResult analyzerResult;
    private SqliteDb sqliteDb = new SqliteDb();

    public void start(string filename) {
        Console.WriteLine(filename);
        Queue<string> textQueue = FileManager.GetText(filename);
        _analyzerManager = new AnalyzerManager(textQueue, 0);
        analyzerResult = _analyzerManager.StartAnalyze();
        Console.WriteLine();
    }

    public void SaveFileInDB() {
        var option = Console.ReadLine();
        switch (option) {
            case "1":
                Console.WriteLine(analyzerResult.ToString());
                sqliteDb.SaveData(analyzerResult);
                Console.WriteLine("Data is stored");
                break;
            case "2":
                Console.WriteLine("Data discarded");
                Console.Clear();
                break;
            
        }

    }
    public void GenerateTxtFile()
    { 
        CreateNewFiles.CreateTxtFiles();
    }
}