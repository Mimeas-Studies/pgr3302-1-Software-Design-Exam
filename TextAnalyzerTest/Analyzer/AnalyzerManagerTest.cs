using System;
using System.Collections.Generic;
using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;
using TextAnalyzer.FileReader;

namespace TextAnalyzerTest.Analyzer;

public class AnalyzerManagerTest
{
    private AnalyzerManager Manager { get; set; } = null!;

    private void InitializeAnalyzer(string text, int threads)
    {
        Queue<string> queue = MakeQueue(text);
        Manager = new AnalyzerManager(queue.GetEnumerator(), threads);
    }

    private Queue<string> MakeQueue(string text)
    {
        Queue<string> tmp = new Queue<string>();
        foreach (var word in text.Split(" "))
        {
            tmp.Enqueue(word);
        }

        return tmp;
    }


    [Test]
    public void OneWordOneThreadTest()
    {
        InitializeAnalyzer("Hello", 1);
        var result = Manager.StartAnalyze();

        Assert.AreEqual(1, result.TotalWordCount);
        Assert.AreEqual(5, result.TotalCharCount);
        Assert.AreEqual("Hello", result.LongestWord);

        Assert.AreEqual(1, result.HeatmapWord.Count);
        Assert.AreEqual(true, result.HeatmapWord.ContainsKey("Hello"));
        Assert.AreEqual(1, result.HeatmapWord["Hello"]);

        Assert.AreEqual(4, result.HeatmapChar.Count);
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("H"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("e"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("l"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("o"));
        Assert.AreEqual(2, result.HeatmapChar["l"]);
    }

    [Test]
    public void ThreeWordOneThreadTest()
    {
        InitializeAnalyzer("Hello world programmer!", 1);
        var result = Manager.StartAnalyze();

        Assert.AreEqual(3, result.TotalWordCount);
        Assert.AreEqual(21, result.TotalCharCount);
        Assert.AreEqual("programmer", result.LongestWord);

        Assert.AreEqual(3, result.HeatmapWord.Count);
        Assert.AreEqual(true, result.HeatmapWord.ContainsKey("world"));
        Assert.AreEqual(1, result.HeatmapWord["world"]);

        Assert.AreEqual(12, result.HeatmapChar.Count);
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("H"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("w"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("l"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("p"));
        Assert.AreEqual(3, result.HeatmapChar["l"]);
    }

    [Test]
    public void OneWordTwoThreadTest()
    {
        InitializeAnalyzer("Hello", 2);
        var result = Manager.StartAnalyze();

        Assert.AreEqual(1, result.TotalWordCount);
        Assert.AreEqual(5, result.TotalCharCount);
        Assert.AreEqual("Hello", result.LongestWord);

        Assert.AreEqual(1, result.HeatmapWord.Count);
        Assert.AreEqual(true, result.HeatmapWord.ContainsKey("Hello"));
        Assert.AreEqual(1, result.HeatmapWord["Hello"]);

        Assert.AreEqual(4, result.HeatmapChar.Count);
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("H"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("e"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("l"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("o"));
        Assert.AreEqual(2, result.HeatmapChar["l"]);
    }

    [Test]
    public void ThreeWordTwoThreadTest()
    {
        InitializeAnalyzer("Hello world programmer!", 2);
        var result = Manager.StartAnalyze();

        Assert.AreEqual(3, result.TotalWordCount);
        Assert.AreEqual(21, result.TotalCharCount);
        Assert.AreEqual("programmer", result.LongestWord);

        Assert.AreEqual(3, result.HeatmapWord.Count);
        Assert.AreEqual(true, result.HeatmapWord.ContainsKey("world"));
        Assert.AreEqual(1, result.HeatmapWord["world"]);

        Assert.AreEqual(12, result.HeatmapChar.Count);
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("H"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("w"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("l"));
        Assert.AreEqual(true, result.HeatmapChar.ContainsKey("p"));
        Assert.AreEqual(3, result.HeatmapChar["l"]);
    }
  
    [Test]
    [Ignore("Testing of files to large for uploading to github")]
    public void Funtest()
    {

        var textFile = FileManager.GetText("Resources/generated.txt");
        var test = new AnalyzerManager(textFile, 4);
        var something = test.StartAnalyze();
        
        Console.WriteLine("analyzed " + something.TotalWordCount + " words");
    }
}