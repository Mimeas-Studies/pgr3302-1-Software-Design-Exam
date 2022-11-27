using System.Text.RegularExpressions;

namespace TextAnalyzer.Analyzer;

/// <summary>
/// Class made for multithreading. Initialised from AnalyzerManager and get fed words to analyze.
/// </summary>
public class AnalyzerThread
{
    private AnalyzerResult Result { get; }
    private Queue<string> Text { get; set; }
    private string Word { get; set; } = "";
    private readonly IEnumerator<string> _textStream;

    private static readonly Regex Regex = new("(?:[^a-z0-9 ]|(?<=['\"])s)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    /// <summary>
    /// Initialise the class with DTO from the outside and IEnumerator<string/> for feeding words. 
    /// </summary>
    /// <param name="result">Empty AnalyzerResult</param>
    /// <param name="textStream">Active IEnumerator<string/></param>
    public AnalyzerThread(AnalyzerResult result, IEnumerator<string> textStream)
    {
        Result = result;
        Text = new Queue<string>();
        _textStream = textStream;
    }

    public void Start()
    {
        bool hasMore = GetNextWord();
        while (hasMore)
        {
            string tmpWord = Regex.Replace(Word, string.Empty);
            Word = tmpWord;
            TotalWordCount();
            TotalCharCount();
            CheckLongestWord();
            HeatmapWord();
            HeatmapChar();

            // Get next word
            hasMore = GetNextWord();
        }
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
        else
        {
            lock (_textStream)
            {
                if (!_textStream.MoveNext()) return hasMore;
                string line = _textStream.Current;
                Text = new Queue<string>(line.Split(" "));
                goto get_word;
            }
        }

        return hasMore;
    }


    private void TotalWordCount()
    {
        string word = Regex.Replace(Word, string.Empty);
        if (word != "")
        {
            Result.TotalWordCount++;
        }
    }

    private void TotalCharCount()
    {
        char[] array = Word.ToCharArray();
        Result.TotalCharCount += array.Length;
    }

    private void CheckLongestWord()
    {
        if (Word.Length > Result.LongestWord.Length)
        {
            Result.LongestWord = Word;
        }
    }

    private void HeatmapWord()
    {
        if (Result.HeatmapWord.ContainsKey(Word))
        {
            Result.HeatmapWord[Word]++;
        }
        else
        {
            Result.HeatmapWord.Add(Word, 1);
        }
    }

    private void HeatmapChar()
    {
        char[] wordArray = Word.ToCharArray();
        foreach (char ch in wordArray)
        {
            if (Result.HeatmapChar.ContainsKey(ch.ToString()))
            {
                Result.HeatmapChar[ch.ToString()]++;
            }
            else
            {
                Result.HeatmapChar.Add(ch.ToString(), 1);
            }
        }
    }
}