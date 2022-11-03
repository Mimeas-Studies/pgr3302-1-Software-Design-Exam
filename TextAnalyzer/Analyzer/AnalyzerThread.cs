namespace TextAnalyzer.Analyzer; 

public class AnalyzerThread {

    public AnalyzerResult Result { get; set; } = null!;
    private static Queue<string> Text { get; } = null!;
    private string Word { get; set; } = "";

    public AnalyzerThread(AnalyzerResult result) {
        Result = result;
    }

    public void Start() {
        
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
        if (Word.Length > Result.LongestWord.Length) {
            Result.LongestWord += Word;
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