namespace TextAnalyzer.Analyzer;

//This class is going to be deleted after multithreading is implemented. 
public class AnalyzerResultMultithreadingTmp {

    private readonly AnalyzerResult _result = new AnalyzerResult();
    
    private Queue<string> Text { get; set; } = null!;
    private string Word { get; set; } = "";

    private Thread[] Threads { get; set; }
    
    private AnalyzerResultMultithreadingTmp(Thread[] threads) {
        Threads = threads;
    }
    
    public AnalyzerResultMultithreadingTmp(Queue<string> text, Thread[] threads1, int threads = 1) {
        // User input shall be 1-8, in case of wrong make default 1. 
        if (threads < 1 || threads > 8) threads = 1;
        Text = text;
        Threads = new Thread[threads];
        
        for (int thread = 0; thread < threads; thread++) {
            // Threads[thread] = new Thread(new ThreadStart(new AnalyzerResultMultithreadingTmp()));
        }
        
    }

    public AnalyzerResult StartAnalyze() {

        var result = new AnalyzerResult();
        
        while (Text.Count > 0) {
            Word = Text.Dequeue();
            
            TotalWordCount();
            TotalCharCount();
            CheckLongestWord();
            HeatmapWord();
            HeatmapChar();
        }

        return _result;
    }


    private void TotalWordCount() {
        _result.TotalWordCount++;
    }

    private void TotalCharCount() {
        var array = Word.ToCharArray();
        _result.TotalCharCount += array.Length;
    }

    private void CheckLongestWord() {
        if (Word.Length > _result.LongestWord.Length) {
            _result.LongestWord += Word;
        }
    }

    private void HeatmapWord() {
        if (_result.HeatmapWord.ContainsKey(Word)) {
            _result.HeatmapWord[Word]++;
        }
        
        else {
            _result.HeatmapWord.Add(Word, 1);
        }
    }

    private void HeatmapChar() {
        var wordArray = Word.ToCharArray();
        foreach (var ch in wordArray) {
            if (_result.HeatmapChar.ContainsKey(ch.ToString())) {
                _result.HeatmapChar[ch.ToString()]++;
            }
            else {
                _result.HeatmapChar.Add(ch.ToString(), 1);
            }
        }
    }
    
}
    