namespace TextAnalyzer.Analyzer; 

public class AnalyzerResult {
    
    public AnalyzerResult() {
        TotalWordCount = 0;
        TotalCharCount = 0;
        LongestWord = "";
        HeatmapWord = new Dictionary<string, int>();
        HeatmapChar = new Dictionary<char, int>();
    }
    
    public int TotalWordCount { get; set; }
    public int TotalCharCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<string, int> HeatmapWord { get; set; }
    public Dictionary<char, int> HeatmapChar { get; set; }

}