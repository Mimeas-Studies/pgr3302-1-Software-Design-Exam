using System.Collections.Generic;
using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest;

public class DbManagerTest
{

    [Test]
    public void SaveRetrieveTest()
    {
        DbManager dbm = new DbManager();

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
        
        dbm.SaveData("testData", testResult);
        var retrieved = dbm.GetAllScans("testData");
        Assert.That(testResult, Is.SubsetOf(retrieved));
    }
}