using TextAnalyzer.Analyzer;

namespace TextAnalyzer.Db;

/// <summary>
/// A common interface for saving and retrieving scan results from databases
/// </summary>
public interface IDbManager
{
    /// <summary>
    /// Saves an <see cref="AnalyzerResult"/> to the database
    /// </summary>
    /// <param name="result">The <see cref="AnalyzerResult"/> to save</param>
    /// <returns>void</returns>
    void SaveData(AnalyzerResult result);
    
    /// <summary>
    /// Retrieve a specific <see cref="AnalyzerResult"/> based on the <paramref name="sourceName"/>
    /// and <paramref name="scanTime"/>
    /// </summary>
    /// <param name="sourceName">Name of the source or source type</param>
    /// <param name="scanTime">The time the scan was generated</param>
    /// <returns>
    /// Returns an <see cref="AnalyzerResult"/> if present or void if not found
    /// </returns>
    AnalyzerResult? GetScan(string sourceName, DateTime scanTime);

    /// <summary>
    /// Get a <see cref="List{T}"/> of all scans stored in the database
    /// </summary>
    /// <returns>A <see cref="List{T}"/> of all <see cref="AnalyzerResult"/> stored in the database</returns>
    List<AnalyzerResult> GetAll();
    
    /// <summary>
    /// Get a list of all scans performed on a specific source from the database 
    /// </summary>
    /// <param name="scanName">The name of the source (either a filename or source description)</param>
    /// <returns>A <see cref="List{T}"/> with all <see cref="AnalyzerResult"/> found that matches <paramref name="scanName"/></returns>
    List<AnalyzerResult> GetWithSource(string scanName);
}