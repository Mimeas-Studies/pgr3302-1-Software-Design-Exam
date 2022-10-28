namespace TextAnalyzer.Analyzer;

//This class is going to be deleted after multithreading is implemented. 
public class AnalyzerResultMultithreadingTmp {
    
    public AnalyzerResult Result { get; set; }
    private int Threads { get; set; }
    private Queue<string> Text { get; set; }
    private string Word { get; set; }

    public AnalyzerResultMultithreadingTmp(Queue<string> text, int threads) {
        Result = new AnalyzerResult();
        Threads = threads;
        Text = text;
        Word = "";
    }

    public AnalyzerResult StartAnalyze() {
        
        while (Text.Count > 0) {
            Word = Text.Dequeue();
            
            TotalWordCount();
            TotalCharCount();
            CheckLongestWord();
            HeatmapWord();
            HeatmapChar();
        }

        return Result;
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
    