namespace TextAnalyzer.UI;

public static class CreateNewFiles
{
    internal static void CreateTxtFiles()
    {
        IOManager.ClearConsole();
        IOManager.Write("\nEnter file name: ");
        string fileName = Console.ReadLine();


        if (fileName != null)
        {
            string path = Path.Combine("Resources", fileName + ".txt");

            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(
                    path
                );

                sw.Write(IOManager.Input("Enter text: "));
                sw.Flush();
                sw.Close();
                Ui.PrintSaveOrDiscard();
                if (IOManager.Input() == "2")
                {
                    File.Delete(path);
                }
            }
        }
    }
}