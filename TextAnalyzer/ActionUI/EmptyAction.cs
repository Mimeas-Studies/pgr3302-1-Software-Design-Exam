using Action = TextAnalyzer.Actions.Action;

namespace TextAnalyzer.ActionUI;

public class EmptyAction : Action
{
    public override string Message { get; internal set; }
    public override void Act()
    {
        return;
    }

    public EmptyAction(string message = "Empty Action")
    {
        Message = message;
    }
}