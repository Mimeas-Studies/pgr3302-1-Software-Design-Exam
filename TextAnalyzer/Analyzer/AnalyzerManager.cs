namespace TextAnalyzer.Analyzer;

/// <summary>
/// MainClass controlling multithreading text-analyzing. Initialise empty result depending on thread counts,
/// add result together and returns them.  
/// </summary>
public class AnalyzerManager
{
    private Thread[] Threads { get; }
    private AnalyzerResult[] Results { get; }
    private readonly IEnumerator<string> _textStream;

    /// <summary>
    /// Needs active IEnumerator<string/> and count for threads. (Default will be 1)
    /// </summary>
    /// <param name="textStream">active IEnumerator<string/></param>
    /// <param name="threadCount">int amount for threads</param>
    public AnalyzerManager(IEnumerator<string> textStream, int threadCount = 1)
    {
        Threads = new Thread[threadCount];
        Results = new AnalyzerResult[threadCount];
        _textStream = textStream;
    }

    /// <summary>
    /// Start with making empty AnalyzerResult based on threads.
    /// Initiate threads and feed from common IEnumerator<string/>
    /// </summary>
    /// <returns>Complete AnalyzerResult from fed text</returns>
    public AnalyzerResult StartAnalyze()
    {
        for (int i = 0; i < Threads.Length; i++)
        {
            Results[i] = new AnalyzerResult();
            AnalyzerThread analyzer = new(Results[i], _textStream);

            Threads[i] = new Thread(analyzer.Start);
            Threads[i].Start();
        }

        foreach (Thread thread in Threads)
        {
            thread.Join();
        }

        AnalyzerResult finishedResult = new();

        var test = Results.Aggregate(finishedResult, (current, result) => current + result);
        return test;
    }
}