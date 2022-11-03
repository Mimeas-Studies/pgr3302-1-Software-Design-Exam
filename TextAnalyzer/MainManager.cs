using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.UI;

namespace TextAnalyzer; 

public class MainManager {
    private AnalyzerManager analyzerManager;
    private AnalyzerResult analyzerResult;
    private SqliteDb sqliteDb = new SqliteDb();

    public void start(string filename) {
        Console.WriteLine(filename);
        Queue<string> textQueue = FileManager.GetText(filename);
        analyzerManager = new AnalyzerManager(textQueue, 0);
        analyzerResult = analyzerManager.StartAnalyze();
        Console.WriteLine();
    }

    public void SaveFileInDB() {
        Console.WriteLine(analyzerResult.ToString());
        Ui.PrintSaveOrDiscard();
        var option = Console.ReadLine();
        switch (option) {
            case "1":
                sqliteDb.SaveData(analyzerResult);
                Console.Clear();
                Console.WriteLine("Data stored\n");
                break;
            case "2":
                Console.Clear();
                Console.WriteLine("Data discarded\n");
                break;
            
        }

    }
    public void GenerateTxtFile()
    { 
        CreateNewFiles.CreateTxtFiles();
    }
}