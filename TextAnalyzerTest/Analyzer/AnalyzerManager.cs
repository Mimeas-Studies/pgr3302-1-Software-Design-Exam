using System.Collections.Generic;
using NUnit.Framework;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest.Analyzer; 

public class AnalyzerManagerTest {

    private AnalyzerManager Manager { get; set; } = null!;

    private void InitializeAnalyzer(string text, int threads) {
        Queue<string> queue = MakeQueue(text);
        Manager = new AnalyzerManager(queue, threads);
    }

    private Queue<string> MakeQueue(string text) {
        Queue<string> tmp = new Queue<string>();
        foreach (var word in text.Split(" ")) {
            tmp.Enqueue(word);
        }
        return tmp;
    }

    
    [Test]
    public void OneWordTest() {
        InitializeAnalyzer("Hello", 1);
        Manager.StartAnalyze();
        
        Assert.AreEqual(1, Manager.Result.TotalWordCount);
        Assert.AreEqual(5, Manager.Result.TotalCharCount);
        Assert.AreEqual("Hello", Manager.Result.LongestWord);
        
        Assert.AreEqual(1, Manager.Result.HeatmapWord.Count);
        Assert.AreEqual(true, Manager.Result.HeatmapWord.ContainsKey("Hello"));
        Assert.AreEqual(1, Manager.Result.HeatmapWord["Hello"]);
        
        Assert.AreEqual(4, Manager.Result.HeatmapChar.Count);
        Assert.AreEqual(true, Manager.Result.HeatmapChar.ContainsKey('H'));
        Assert.AreEqual(true, Manager.Result.HeatmapChar.ContainsKey('e'));
        Assert.AreEqual(true, Manager.Result.HeatmapChar.ContainsKey('l'));
        Assert.AreEqual(true, Manager.Result.HeatmapChar.ContainsKey('o'));
        Assert.AreEqual(2, Manager.Result.HeatmapChar['l']);
    }
    
}