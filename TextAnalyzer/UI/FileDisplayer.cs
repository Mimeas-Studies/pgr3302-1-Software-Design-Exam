using System.Collections;

namespace TextAnalyzer.UI; 

public class FileDisplayer {
    private List<string>? textFileArrayList;
    private int selectedFile;
    
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
            textFileArrayList.Add(file.Name);
            
        }
        Console.WriteLine("\nType in menu option number and press <Enter> to analyse text");
        selectedFile = Convert.ToInt32(Console.ReadLine());
    }


    public string getSelectedFile() {
        return textFileArrayList[selectedFile - 1];
    }
}