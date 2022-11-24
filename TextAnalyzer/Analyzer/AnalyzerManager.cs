namespace TextAnalyzer.Analyzer;

public class AnalyzerManager
{
    private Thread[] Threads { get; }
    private AnalyzerResult[] Results { get; }

    private readonly IEnumerator<string> _textStream;

    public AnalyzerManager(IEnumerator<string> textStream, int threadCount = 1)
    {
        Threads = new Thread[threadCount];
        Results = new AnalyzerResult[threadCount];
        _textStream = textStream;
    }

    public AnalyzerResult StartAnalyze()
    {
        for (int i = 0; i < Threads.Length; i++)
        {
            Results[i] = new AnalyzerResult();
            var analyzer = new AnalyzerThread(Results[i], _textStream);

            Threads[i] = new Thread(analyzer.Start);
            Threads[i].Start();
        }

        foreach (var thread in Threads)
        {
            thread.Join();
        }

        var finishedResult = new AnalyzerResult();
        foreach (var result in Results)
        {
            finishedResult += result;
        }

        return finishedResult;
    }
}