namespace TextAnalyzer.Actions;

public abstract class Action
{
    public abstract string Message { get; internal set; }
    
    public abstract void Act();
}