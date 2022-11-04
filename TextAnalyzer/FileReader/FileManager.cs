using TextAnalyzer.UI;

namespace TextAnalyzer;

public class FileManager {
    private List<string>? _textFileArrayList;
    private List<string>? _textFileNames;
    private int _selectedFile;
    private Boolean _notValidInput = true;

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

    internal void DisplayStoredFiles() {
        _textFileArrayList = new List<string>();
        _textFileNames = new List<string>();

        IOManager.ClearConsole();
        IOManager.Write("Display texts that arent analysed.");
        var directoryInfo = new DirectoryInfo("Resources"); //Insert directory
        FileInfo[] files = directoryInfo.GetFiles("*.txt"); //Get files the end with .txt
        var counter = 0;
        foreach (FileInfo file in files) {
            counter++;
            Console.WriteLine(counter + ". " + file.Name);
            _textFileNames.Add(file.Name);
            _textFileArrayList.Add(file.FullName);
        }

        IOManager.Write("\nType in menu option number and press <Enter> to analyse text");
        var inputInt = Convert.ToInt32(Console.ReadLine());
        while (_notValidInput) {
            if (inputInt > _textFileArrayList.Count || inputInt <= 0) {
                Console.WriteLine("Input to high, try again:");
                DisplayStoredFiles();
                inputInt = Convert.ToInt32(Console.ReadLine());
            }
            else {
                _notValidInput = false;
                _selectedFile = inputInt;
            }
        }
        
    }

    /**
     * 
     */
    public string GetSelectedFile() {
        if (_textFileArrayList != null) return _textFileArrayList[_selectedFile - 1];
        return "No files are stored on disk";
    }

    public string RetriveAllFileNames() {
        if (_textFileNames != null) return _textFileNames[_selectedFile - 1];
        return "No files are stored on disk";
    }
}