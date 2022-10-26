using NUnit.Framework;
using TextAnalyzer;

namespace TextAnalyzerTest;

public class Tests {

    [Test]
    public void ShouldRead500Words()
    {
        var words = FileManager.GetText("Lorem Ipsum 500.txt");
        
        Assert.That(words.Count, Is.EqualTo(500));
    }
}