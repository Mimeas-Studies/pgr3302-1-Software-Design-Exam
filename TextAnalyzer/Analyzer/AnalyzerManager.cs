namespace TextAnalyzer.Analyzer; 

public class AnalyzerManager {

    private Queue<string> Text { get; set; } = null!;
    private AnalyzerResult Result { get; set; }

    public AnalyzerManager() {
        Result = new AnalyzerResult();
    }

    public AnalyzerResult StartAnalyze(Queue<string> text) {
        Text = text;

        while (Text.Count > 0) {
            TotalWordCount();
            TotalCharCount();
            CheckLongestWord();
            HeatmapWord();
            HeatmapChar();
        }

        return Result;
    }


    private void TotalWordCount() {
        throw new NotImplementedException();
    }

    private void TotalCharCount() {
        throw new NotImplementedException();
    }

    private void CheckLongestWord() {
        throw new NotImplementedException();
    }

    private void HeatmapWord() {
        throw new NotImplementedException();
    }

    private void HeatmapChar() {
        throw new NotImplementedException();
    }
    
}