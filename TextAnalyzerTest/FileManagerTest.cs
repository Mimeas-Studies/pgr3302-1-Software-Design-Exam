using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest;

public class Tests {

    [Test]
    public void ShouldRead500Words()
    {
        var words = FileManager.GetText("Lorem Ipsum 500.txt");
        
        Assert.That(words.Count, Is.EqualTo(500));
    }

    [Test]
    public void ShouldReadLargeFile()
    {
        var manyWords = FileManager.GetText("Random Ipsum 1 500 131.txt");
        
        Assert.That(manyWords.Count, Is.EqualTo(1_500_131));
    }

    [Test]
    public void Analyze1500131Words()
    {
        var manyWords = FileManager.GetText("Random Ipsum 1 500 131.txt");

        var manager = new AnalyzerManager(manyWords, 1);
        var result = manager.StartAnalyze();
        
        Assert.AreEqual(1_500_131, result.TotalWordCount);
    }
}