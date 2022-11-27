namespace TextAnalyzer.UI;

public static class CreateNewFiles
{
    internal static void CreateTxtFiles()
    {
        string? fileName = null;
        while (string.IsNullOrEmpty(fileName))
        {
            IOManager.ClearConsole();
            fileName = IOManager.Input("Enter file name: ");
        }
        
        string path = Path.Combine("Resources", fileName + ".txt");
        
        if (File.Exists(path)) return;
        
        StreamWriter sw = new StreamWriter(
            path
        );

        IOManager.Write("Enter text and press enter 3 times to continue");
        while (true)
        {
            string? line = IOManager.Input();
            if (line is null or "")
            {
                string? confirm = IOManager.Input();
                if (confirm is null or "") break;
                sw.WriteLine($"\n{confirm}");
                continue;
            }
            sw.WriteLine(line);
        }
        
        sw.Flush();
        sw.Close();
        Ui.PrintSaveOrDiscard();
        while (true)
        {
            string? input = IOManager.Input();
            if (string.IsNullOrEmpty(input)) continue;
            if (input == "2") File.Delete(path);
            
            return;
        }
    }
}