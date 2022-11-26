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

        IOManager.ClearConsole();
        IOManager.Write("Display texts that arent analysed.");
        DirectoryInfo directoryInfo = new DirectoryInfo("Resources"); //Insert directory
        FileInfo[] files = directoryInfo.GetFiles("*.txt"); //Get files the end with .txt
        int counter = 0;
        foreach (FileInfo file in files)
        {
            counter++;
            IOManager.Write(counter + ". " + file.Name);
            _textFileNames.Add(file.Name);
            _textFileArrayList.Add(file.FullName);
        }

        IOManager.Write("\nType in menu option number and press <Enter> to analyse text");
        IOManager.Write("Type in <B> to go back and press <Enter>");

        do
        {
            string? input = IOManager.Input();

            if (input is null || input.Any(char.IsLetter))
            {
                return null;
            }

            int intInput = int.Parse(input);

            if (intInput > _textFileArrayList.Count || intInput <= 0) IOManager.Write("Input to high, try again:");
            else return _textFileArrayList[intInput - 1];

        } while (true);
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