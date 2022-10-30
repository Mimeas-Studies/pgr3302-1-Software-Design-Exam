namespace TextAnalyzer.Analyzer; 

public class AnalyzerResult {
    
    //Reason for initialise is to have zero null values and to have guard values 
    public AnalyzerResult() {
        TotalWordCount = 0;
        TotalCharCount = 0;
        LongestWord = "";
        HeatmapWord = new Dictionary<string, int>();
        HeatmapChar = new Dictionary<string, int>();
    }
    
    public int TotalWordCount { get; set; }
    public int TotalCharCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<string, int> HeatmapWord { get; set; }
    public Dictionary<string, int> HeatmapChar { get; set; }
    

    public override string ToString() {
        return "Total word count: " + TotalWordCount + ",\n" +
               "Total char count: " + TotalCharCount + ",\n" +
               "Longest word: " + LongestWord + ",\n" +
               "Word Heatmap: " + ToStringHeatmap(HeatmapWord) + "\n" + 
               "Char Heatmap:" + ToStringHeatmap(HeatmapChar) + "\n";
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

}