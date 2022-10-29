namespace TextAnalyzer.UI;

public class CreateNewFiles
{
    public void CreateTxtFile()
    {
        Console.WriteLine("Enter file name: ");
        string fileName = Console.ReadLine();

            if (fileName != null)
        {
            string path = Path.Combine ("Resources", fileName + ".txt");

            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(
                    path
                );
                Console.WriteLine("Enter text: ");
                string text = Console.ReadLine();
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
        }
    }
    }
 