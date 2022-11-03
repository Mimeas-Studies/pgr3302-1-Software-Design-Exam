namespace TextAnalyzer.UI;

public class CreateNewFiles
{
    internal static void CreateTxtFiles()
    {
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
                IOManager.Input("Enter text: ");
                sw.Flush();
                sw.Close();
            }
        }
    }
}
 