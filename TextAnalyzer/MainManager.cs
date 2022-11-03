﻿using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.UI;

namespace TextAnalyzer; 

public class MainManager {
    private AnalyzerManager analyzerManager;
    private AnalyzerResult analyzerResult;
    private SqliteDb sqliteDb = new SqliteDb();
    private FileManager FileManager = new FileManager();

    public void Start(FileManager fileManager) {
        Queue<string> textQueue = FileManager.GetText(fileManager.getSelectedFile());
        analyzerManager = new AnalyzerManager(textQueue, 0);
        
        analyzerResult = analyzerManager.StartAnalyze();
        analyzerResult.SourceName = fileManager.retriveAllFileNames();
    }

    public void SaveFileInDb() {
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

    public void RetriveTitlesOfAnalysedTexts() {
        Console.Clear();
        Console.WriteLine("Names of analysed text.");
        var counter = 0;
        var analyzerResultsList = sqliteDb.GetAll();
        for (var i = 0; i < analyzerResultsList.Count; i++) {
            counter++;
            Console.WriteLine(counter+". "+analyzerResultsList[i].SourceName);
        }
        Console.WriteLine("\nType in menu number to see stats and press <Enter>");
        var selectedTxtFile = Convert.ToInt32(Console.ReadLine());
        Console.Clear();
        Console.WriteLine("Stats of analysed text:");
        Console.WriteLine(analyzerResultsList[selectedTxtFile - 1]);
        Console.WriteLine();

    }
}