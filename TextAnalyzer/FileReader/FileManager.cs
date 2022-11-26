using TextAnalyzer.UI;

namespace TextAnalyzer;

/// <summary>
/// Handles txt files by storing them in Queue's, displays them from directories and return them
/// </summary>
public class FileManager
{
    private List<string>? _textFileArrayList;
    private List<string>? _textFileNames;
    private int _selectedFile;
    private bool _notValidInput;
    private bool _displayingFiles = false;

    public FileManager()
    {
        // MainManager assumes a 'Resources' folder exists
        DirectoryInfo resources = new("Resources");
        if (!resources.Exists)
        {
            Logger.Warn("Resources folder not found, creating a new one");
            resources.Create();
        }
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
    internal bool DisplayStoredFiles()
    {
        _notValidInput = true;
        _textFileArrayList = new List<string>();
        _textFileNames = new List<string>();

        IOManager.ClearConsole();
        IOManager.Write("Display texts that arent analysed.");
        var directoryInfo = new DirectoryInfo("Resources"); //Insert directory
        FileInfo[] files = directoryInfo.GetFiles("*.txt"); //Get files the end with .txt
        var counter = 0;
        foreach (FileInfo file in files)
        {
            counter++;
            IOManager.Write(counter + ". " + file.Name);
            _textFileNames.Add(file.Name);
            _textFileArrayList.Add(file.FullName);
        }

        IOManager.Write("\nType in menu option number and press <Enter> to analyse text");
        IOManager.Write("Type in <B> to go back and press <Enter>");

        var input = IOManager.Input();

        if (input.Any((x) => char.IsLetter(x)))
        {
            return _displayingFiles = false;
        }

        var intInput = Convert.ToInt32(input);
        while (_notValidInput)
        {
            if (intInput > _textFileArrayList.Count || intInput <= 0)
            {
                IOManager.Write("Input to high, try again:");
                intInput = Convert.ToInt32(IOManager.Input());
            }
            else
            {
                _notValidInput = false;
            }

            _selectedFile = intInput;
        }

        return _displayingFiles = true;
    }

    /// <summary>
    /// Checks if there are existing files added previously
    /// </summary>
    /// <returns> returns selected file path with user input </returns>
    public string GetSelectedFile()
    {
        if (_textFileArrayList != null) return _textFileArrayList[_selectedFile - 1];
        return "No files are stored on disk";
    }


    /// <summary>
    /// Checks if there are existing file names added previously
    /// </summary>
    /// <returns> returns selected file name with user input </returns>
    public string RetrieveAllFileNames() 
    {
        if (_textFileNames != null) return _textFileNames[_selectedFile - 1];
        return "No files are stored on disk";
    }
}