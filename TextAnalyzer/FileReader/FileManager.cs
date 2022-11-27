using TextAnalyzer.Logging;
using TextAnalyzer.UI;

namespace TextAnalyzer.FileReader;

/// <summary>
/// Handles txt files by storing them in Queue's, displays them from directories and return them
/// </summary>
public class FileManager
{
    private List<string>? _textFileArrayList;
    private List<string>? _textFileNames;

    public FileManager()
    {
        // MainManager assumes a 'Resources' folder exists
        DirectoryInfo resources = new("Resources");
        
        if (resources.Exists) return;
        Logger.Warn("Resources folder not found, creating a new one");
        resources.Create();
    }

    /// <summary>
    /// Reads .txt files and adds strings separated by an empty space to a queue.
    /// </summary>
    /// <param name="filepath">takes in a file from  bin/debug/net6.0/resources</param>
    /// <returns>a list of strings in a queue</returns>
    public static IEnumerator<string> GetText(string filepath)
    {
    Logger.Debug($"Reading from: {filepath}");
        IEnumerable<string> text = File.ReadLines(filepath);
        return text.GetEnumerator();
    }

    /// <summary>
    /// Displays files that are located in the resources directory
    /// checks in user input is valid based on files from directory
    /// </summary>
    internal string? ChooseStoredFile()
    {
        _textFileArrayList = new List<string>();
        _textFileNames = new List<string>();

        IoManager.ClearConsole();
        IoManager.Write("Display texts that arent analysed.");
        DirectoryInfo directoryInfo = new ("Resources"); //Insert directory
        FileInfo[] files = directoryInfo.GetFiles("*.txt"); //Get files the end with .txt

        (int, FileInfo)[] fileList = files.Select((file, i) => (i, file)).ToArray();
        foreach ((int _, FileInfo file) in fileList)
        {
            _textFileNames.Add(file.Name);
            _textFileArrayList.Add(file.FullName);
        }

        while (true)
        {
            IoManager.ClearConsole();
            foreach ((int index, FileInfo file) in fileList)
            {
                IoManager.Write($"{index+1}. {file.Name}");
            }

            IoManager.Write("\nType in menu option number and press <Enter> to analyse text");
            IoManager.Write("Type in <B> to go back and press <Enter>");
            
            string? input = IoManager.Input();
            if (string.IsNullOrWhiteSpace(input) || input.Any(c => !char.IsNumber(c))) return null;

            int selected = int.Parse(input);
            if (selected <= 0 || selected > _textFileArrayList.Count)
            {
                continue;
            }
            return _textFileArrayList[selected -1];
        }
    }


    /// <summary>
    /// Checks if there are existing file names added previously
    /// </summary>
    /// <returns> returns selected file name with user input </returns>
    public string[] RetrieveAllFileNames()
    {
        return _textFileNames is null ? Array.Empty<string>() : _textFileNames.ToArray();
    }
}