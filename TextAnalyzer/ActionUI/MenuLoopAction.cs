using Action = TextAnalyzer.Actions.Action;

namespace TextAnalyzer.ActionUI;

public class MenuLoopAction: MenuAction
{
    private static bool _done = false;
    
    public MenuLoopAction(List<Action> actionList, string name = "Menu action") : base(actionList, name)
    {
        actions.Add(new BackAction());
    }

    private class BackAction : Action
    {
        public override string Message { get; internal set; } = "Back";

        public override void Act()
        {
            _done = true;
        }
    }

    public override void Act()
    {
        _done = false;
        
        while (!_done)
        {
            base.Act();
        }

        _done = false;
    }
}