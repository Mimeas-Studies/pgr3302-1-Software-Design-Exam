using NUnit.Framework;
using TextAnalyzer;
using TextAnalyzer.Analyzer;

namespace TextAnalyzerTest;

public class Tests {

    [Test]
    public void ShouldRead500Words()
    {
        var lines = FileManager.GetText("Resources/Lorem Ipsum 500.txt");

        int count = 0;
        while (lines.MoveNext())
        {
            count += lines.Current.Split(" ").Length;
        }
        
        Assert.That(count, Is.EqualTo(500));
    }

    [Test]
    public void ShouldReadLargeFile()
    {
        var lines = FileManager.GetText("Resources/Random Ipsum 1 500 131.txt");
        
        int count = 0;
        while (lines.MoveNext())
        {
            count += lines.Current.Split(" ").Length;
        }
        
        Assert.That(count, Is.EqualTo(1_500_131));
    }

    [Test]
    public void Analyze1500131Words()
    {
        var manyWords = FileManager.GetText("Resources/Random Ipsum 1 500 131.txt");

        var manager = new AnalyzerManager(manyWords, 1);
        var result = manager.StartAnalyze();
        
        Assert.AreEqual(1_500_131, result.TotalWordCount);
    }
}