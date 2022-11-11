namespace TextAnalyzer.Analyzer; 

public class AnalyzerResult {
    
    //Reason for initialise is to have zero null values and to have guard values 
    public AnalyzerResult()
    {
        SourceName = "Not Set";//filename
        ScanTime = DateTime.Now;
        TotalWordCount = 0;
        TotalCharCount = 0;
        LongestWord = "";
        HeatmapWord = new Dictionary<string, int>();
        HeatmapChar = new Dictionary<string, int>();
    }
    
    public string SourceName { get; set; }
    public DateTime ScanTime { get; set; }
    public int TotalWordCount { get; set; }
    public int TotalCharCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<string, int> HeatmapWord { get; set; }
    public Dictionary<string, int> HeatmapChar { get; set; }
    

    public override string ToString()
    {
        return @$"
            Scan time: {ScanTime}
            Source name: {SourceName}
            Total word count: {TotalWordCount} 
            Total char count: {TotalCharCount}
            Longest word: {LongestWord}
            Word Heatmap: {ToStringHeatmap(HeatmapWord)} 
            Char Heatmap: {ToStringHeatmap(HeatmapChar)}
        ";
    }
    
    private static string ToStringHeatmap(Dictionary<string, int> heatMap) {
        var highestValue = 0;
        var strValue = "";

        foreach (var pair in heatMap) {
            if (pair.Value <= highestValue) continue;
            highestValue += pair.Value;
            strValue = pair.Key;
        }

        return ($" {strValue} | Counted {highestValue} times,"); 
    }
    
    public static AnalyzerResult operator +(AnalyzerResult a, AnalyzerResult b) {
        var newResult = new AnalyzerResult();

        newResult.TotalWordCount = a.TotalWordCount + b.TotalWordCount;
        newResult.TotalCharCount = a.TotalCharCount + b.TotalCharCount;

        if (a.LongestWord.Length > b.LongestWord.Length) newResult.LongestWord = a.LongestWord;
        else newResult.LongestWord = b.LongestWord;

        newResult.HeatmapWord = b.HeatmapWord;
        foreach (var word in a.HeatmapWord) {
            if (newResult.HeatmapWord.ContainsKey(word.Key)) {
                newResult.HeatmapWord[word.Key] += word.Value;
                
            } else {
                newResult.HeatmapWord.Add(word.Key, word.Value);
            }
        }
        
        newResult.HeatmapChar = b.HeatmapChar;
        foreach (var word in a.HeatmapChar) {
            if (newResult.HeatmapChar.ContainsKey(word.Key)) {
                newResult.HeatmapChar[word.Key] += word.Value;
                
            } else {
                newResult.HeatmapChar.Add(word.Key, word.Value);
            }
        }
            
        return newResult;
        }

    }