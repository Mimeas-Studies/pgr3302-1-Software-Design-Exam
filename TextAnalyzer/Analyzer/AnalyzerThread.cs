using System.Text.RegularExpressions;

namespace TextAnalyzer.Analyzer; 

public class AnalyzerThread {
    
    private AnalyzerResult Result { get; set; }
    private static Queue<string> Text { get; set; } = null!;
    private string Word { get; set; } = "";

    public AnalyzerThread(AnalyzerResult result, Queue<string> text) {
        Result = result;
        Text = text;
    }

    public void Start() {
        Console.Write("I started");
        while (Text.Count > 0) {
            lock (Text) {
                Word = Text.Dequeue();
            }

            TotalWordCount();
            TotalCharCount();
            CheckLongestWord();
            HeatmapWord();
            HeatmapChar();
        }

    }


    private void TotalWordCount() {
        Result.TotalWordCount++;
    }

    private void TotalCharCount() {
        var array = Word.ToCharArray();
        Result.TotalCharCount += array.Length;
    }

    private void CheckLongestWord() {
        var regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        var word = regex.Replace(Word, String.Empty);
        
        if (word.Length > Result.LongestWord.Length) {
            Result.LongestWord = word;
        }
    }

    private void HeatmapWord() {
        if (Result.HeatmapWord.ContainsKey(Word)) {
            Result.HeatmapWord[Word]++;
        }

        else {
            Result.HeatmapWord.Add(Word, 1);
        }
    }

    private void HeatmapChar() {
        var wordArray = Word.ToCharArray();
        foreach (var ch in wordArray) {
            if (Result.HeatmapChar.ContainsKey(ch.ToString())) {
                Result.HeatmapChar[ch.ToString()]++;
            }
            else {
                Result.HeatmapChar.Add(ch.ToString(), 1);
            }
        }
    }
    
}