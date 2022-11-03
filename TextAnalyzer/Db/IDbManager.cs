using TextAnalyzer.Analyzer;

namespace TextAnalyzer.Db;

public interface IDbManager
{
    void SaveData(AnalyzerResult result);
    AnalyzerResult? GetScan(string sourceName, DateTime scanTime);

    List<AnalyzerResult> GetAll();
    
    List<AnalyzerResult> GetWithSource(string scanName);
}