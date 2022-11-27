using Action = TextAnalyzer.Actions.Action;

namespace TextAnalyzer.ActionUI;

public class MainMenuAction: MenuAction
{
    public MainMenuAction(List<Action> actions) : base(MainMenuAction.WithExit(actions))
    {
    }

    private static List<Action> WithExit(List<Action> actions)
    {
        actions.Add(new ExitAction());
        return actions;
    }
    
    private class ExitAction : Action
    {
        public override string Message { get; internal set; } = "Exit program";

        public override void Act()
        {
            Logger.Info("Exiting program");
            Environment.Exit(0);
        }
    }

    public override void Act()
    {
        while (true)
        {
            base.Act();
        }
    }
}