using TextAnalyzer.Analyzer;
using TextAnalyzer.Db;
using TextAnalyzer.FileReader;
using TextAnalyzer.UI;
using TextAnalyzer.Logging;
using TextAnalyzer.Actions;
using TextAnalyzer.ActionUI;
using Action = TextAnalyzer.Actions.Action;

namespace TextAnalyzer;

/// <summary>
/// Used as a facade, creates and instance of many classes in the project and make them cooperate.
/// </summary>
public class MainManager
{
    internal MainMenuAction mainMenu;

    public MainManager()
    {
        mainMenu = new MainMenuAction(new List<Action>()
        {
            new EmptyAction("Empty Action 1"),
            new EmptyAction("Empty Action 2"),
            new MenuLoopAction(new List<Action>()
            {
                new EmptyAction("Empty sub-action 1"),
                new MenuLoopAction(new List<Action>()
                {
                    new EmptyAction("empty sub-sub-action"),
                    new MenuAction(new List<Action>()
                    {
                        new EmptyAction("Yes"),
                        new EmptyAction("No")
                    }, "yes/no menu")
                })
            }, "Loopable menu")
        });
    }

    public void Start()
    {
        mainMenu.Act();
    }
    
    public static void Main(string[] args)
    {
        Logger.SetLevel(LogLevel.Info);
        Logger.Info("Initializing Application");
        MainManager mainManager = new();

        Logger.Info("Running main loop");
        mainManager.Start();
    }
}