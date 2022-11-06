using System.Text.RegularExpressions;

namespace TextAnalyzer.Analyzer; 

public class AnalyzerThread {
    
    private AnalyzerResult Result { get; set; }
    private Queue<string> Text { get; set; }
    private string Word { get; set; } = "";
    private int Count { get; set; }
    
    private static Regex _regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
    private readonly IEnumerator<string> _textStream;


    public AnalyzerThread(AnalyzerResult result, Queue<string> text) {
        Result = result;
        Text = text;
    }

    public AnalyzerThread(AnalyzerResult result, IEnumerator<string> textStream)
    {
        Result = result;
        Text = new Queue<string>();
        _textStream = textStream;
    }

    private bool GetNextWord()
    {
        get_word:
        bool hasMore = false;
        if (Text.Count > 0)
        {
            Word = Text.Dequeue();
            hasMore = true;
        }
        else {
            lock (_textStream)
            {
                if (_textStream.MoveNext())
                {
                    string line = _textStream.Current;
                    Text = new Queue<string>(line.Split(" "));
                    goto get_word;
                }
            }
        }

        return hasMore;
    }

    public void Start()
    {

        bool hasMore = GetNextWord();
        while(hasMore)
        {
            TotalWordCount();
            TotalCharCount();
            CheckLongestWord();
            HeatmapWord();
            HeatmapChar();

            // Get next word
            hasMore = GetNextWord();
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
        var word = _regex.Replace(Word, String.Empty);
        
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