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

    private string retrieveHeatmap(Dictionary<string, int> heatMap) {
        
        var highestValue = 0;
        var strValue = "";

        foreach (var i in heatMap) {
            if (i.Value > highestValue) {
                highestValue += i.Value;
                strValue = i.Key;
            }
        }

        return " \""+strValue + "\" | Counted: " + highestValue +" times.";
    }
    
    private string retrieveHeatmap(Dictionary<char, int> heatMap) {
        
        var highestValue = 0;
        var charValue = 's';

        foreach (var i in heatMap) {
            if (i.Value > highestValue) {
                highestValue += i.Value;
                charValue = i.Key;
            }
        }

        return " \""+charValue + "\" | Counted: " + highestValue +" times.";
        
    }
    

    public override string ToString() {
        return "Total word count: " + TotalWordCount + ",\n" +
               "Total char count: " + TotalCharCount + ",\n" +
               "Longest word: " + LongestWord + ",\n" +
               "Word Heatmap: " + retrieveHeatmap(HeatmapWord) + "\n" + 
               "Char Heatmap:" + retrieveHeatmap(HeatmapChar) + "\n";

    }
}