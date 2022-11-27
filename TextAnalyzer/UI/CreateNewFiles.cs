namespace TextAnalyzer.UI;

public static class CreateNewFiles
{
    internal static void CreateTxtFiles()
    {
        IoManager.ClearConsole();
        IoManager.Write("\nEnter file name: ");
        string fileName = Console.ReadLine();


        if (fileName != null)
        {
            string path = Path.Combine("Resources", fileName + ".txt");

            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(
                    path
                );

                sw.Write(IoManager.Input("Enter text: "));
                sw.Flush();
                sw.Close();
                Ui.PrintSaveOrDiscard();
                if (IoManager.Input() == "2")
                {
                    File.Delete(path);
                }
            }
        }
    }
}