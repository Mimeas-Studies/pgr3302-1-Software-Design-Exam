namespace TextAnalyzer.Analyzer; 

public class AnalyzerManager {
    
    private static Queue<string> Text { get; set; } = null!;
    private Thread[] Threads { get; }
    private AnalyzerResult[] Results { get; }

    public AnalyzerManager(Queue<string> text, int threadCount = 1) {
        Threads = new Thread[threadCount];
        Results = new AnalyzerResult[threadCount];
        Text = text;
    }

    public AnalyzerResult StartAnalyze() {
        
        for (int thread = 0; thread < Threads.Length; thread++) {
            var analyzer = new AnalyzerThread(Results[thread]);
            Threads[thread] = new Thread(analyzer.Start);
        }

        // while (Text.Count > 0) {}
        
        var finishedResult = new AnalyzerResult();
        foreach (var result in Results) {
            finishedResult += result;
        }
        
        return finishedResult;
    }

}