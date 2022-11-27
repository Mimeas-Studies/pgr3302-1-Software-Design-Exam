namespace TextAnalyzer.Actions;

public abstract class Action
{
    public abstract string Message { get; }
    
    public abstract void Act();
}