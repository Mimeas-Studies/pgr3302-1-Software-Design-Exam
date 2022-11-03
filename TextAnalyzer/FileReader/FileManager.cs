namespace TextAnalyzer;

public class FileManager {
    
    private List<string>? textFileArrayList;
    private int selectedFile;
    
    //  Returns a Queue of words from a filepath
    public static Queue<string> GetText(string filepath) {
        var queue = new Queue<string>();

        foreach (string line in File.ReadLines(filepath)) {
            foreach (var word in line.Split(" ")) {
                queue.Enqueue(word);
            }
        }
        return queue;
    }
    
    internal void displayStoredFiles() {
        textFileArrayList = new List<string>();
        Console.Clear();
        Console.WriteLine("Display texts that arent analysed.");
        DirectoryInfo d = new DirectoryInfo("Resources"); //Insert directory
        FileInfo[] Files = d.GetFiles("*.txt"); //Get files the end with .txt
        var counter = 0;
        foreach(FileInfo file in Files ) {
            counter++;
            Console.WriteLine(counter+". "+file.Name);
            textFileArrayList.Add(file.FullName);
            
        }
        Console.WriteLine("\nType in menu option number and press <Enter> to analyse text");
        selectedFile = Convert.ToInt32(Console.ReadLine());
    }
    
    public string getSelectedFile() {
        return textFileArrayList[selectedFile - 1];
    }

}