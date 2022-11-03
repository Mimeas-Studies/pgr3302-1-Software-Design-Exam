using TextAnalyzer.Analyzer;
using TextAnalyzer.UI;

namespace TextAnalyzer; 

public class MainManager {
    private AnalyzerManager _analyzerManager;

    public void start(String filname) {
        Queue<string> textQueue = FileManager.GetText("C:\\Users\\fredr\\RiderProjects\\pgr3302-1-Software-Design-Exam\\TextAnalyzerTest\\Resources\\Lorem Ipsum 500.txt");
        _analyzerManager = new AnalyzerManager(textQueue, 0);
        Console.WriteLine(_analyzerManager.StartAnalyze().ToString());


    }
    public void GenerateTxtFile()
    { 
        CreateNewFiles.CreateTxtFiles();
    }
}