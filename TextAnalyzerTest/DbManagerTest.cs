using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest;

public class DbManagerTest
{
    private const string TestDbPath = "test.db";
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

        var testResult = new AnalyzerResult();
        testResult.SourceName = "testData";
        testResult.ScanTime = DateTime.Now;
        testResult.TotalWordCount = 1;
        testResult.TotalCharCount = 5;
        testResult.LongestWord = "hello";
        testResult.HeatmapWord = wordMap;
        testResult.HeatmapChar = charMap;
        
        manager.SaveData(testResult);
        var retrieved = manager.GetScan("testData", testResult.ScanTime);
        Assert.NotNull(retrieved);
        // You forced me Nunit, this is your fault!
        Assert.AreEqual(testResult.SourceName, retrieved.SourceName);
        Assert.AreEqual(testResult.ScanTime, retrieved.ScanTime);
        Assert.AreEqual(testResult.TotalWordCount, retrieved.TotalWordCount);
        Assert.AreEqual(testResult.TotalCharCount, retrieved.TotalCharCount);
        Assert.AreEqual(testResult.LongestWord, retrieved.LongestWord);
        Assert.AreEqual(testResult.HeatmapWord, retrieved.HeatmapWord);
        Assert.AreEqual(testResult.HeatmapChar, retrieved.HeatmapChar);
    }
}