namespace TextAnalyzer; 

public class AnalyzerManager {

    private Queue<string> Text { get; set; }
    private AnalyzerResult Result { get; set; }

    public AnalyzerManager() {
        Result = new AnalyzerResult();
    }

    public AnalyzerResult StartAnalyze(Queue<string> text) {
        Text = text;

        while (Text.Count > 0) {
            
        }

        return Result;
    }
    
    
    
}