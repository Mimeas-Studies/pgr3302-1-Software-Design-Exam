using System.Collections.Generic;
using System.IO;
using System.Net;
using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest;

public class DbManagerTest
{
    private const string TestDbPath = "test/test.db";
    private DbManager manager;
    
    [OneTimeSetUp]
    public void DbSetup()
    {
        if (File.Exists(TestDbPath)) File.Delete(TestDbPath);
        manager = new DbManager(TestDbPath);
    }

    [OneTimeTearDown]
    public void DbTearDown()
    {
        // File.Delete(TestDbPath);
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
            if (charMap.ContainsKey(c.ToString()))
            {
                charMap[c.ToString()] += 1;
            }
            else
            {
                charMap.Add(c.ToString(), 1);
            }
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