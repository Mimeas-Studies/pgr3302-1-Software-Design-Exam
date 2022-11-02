using System.Collections.Generic;
using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest;

public class DbManagerTest
{
    private DbManager manager;
    
    [OneTimeSetUp]
    public void DbSetup()
    {
        manager = new DbManager("test.db");
    }

    [Test]
    public void SaveRetrieveTest()
    {
        var testResult = new AnalyzerResult();
        
        //  Explicit AnalyzerResult creation instead of using the Analyzer
        //  so the test doesn't fail by an error in AnalyzerManager

        var wordMap = new Dictionary<string, int>();
        wordMap.Add("hello", 1);
        
        var charMap = new Dictionary<string, int>();
        foreach (var c in "hello")
        {
            charMap.Add(
                c.ToString(), 
                charMap.GetValueOrDefault(c.ToString(), 0));
        }

        testResult.TotalWordCount = 1;
        testResult.TotalCharCount = 5;
        testResult.HeatmapWord = wordMap;
        testResult.HeatmapChar = charMap;
        
        manager.SaveData("testData", testResult);
        var retrieved = manager.GetAllScans("testData");
        Assert.That(testResult, Is.SubsetOf(retrieved));
    }
}