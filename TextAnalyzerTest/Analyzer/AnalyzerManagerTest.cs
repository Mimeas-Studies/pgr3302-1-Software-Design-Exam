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
    
}