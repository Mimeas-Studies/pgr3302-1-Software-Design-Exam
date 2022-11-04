using TextAnalyzer.Analyzer;

namespace TextAnalyzer;

public class Program
{
    internal static bool IsProgramRunning = true;

    public static void Main(String[] args) {
        //Infinite while loop of the main menu switch case, while isProgramRunning set to true,
        //false value set to five in switch case, exiting program 
        while (IsProgramRunning)
        {
            Ui.PrintMenu();
            MenuHandler.menuHandler();
        }
    }
}
