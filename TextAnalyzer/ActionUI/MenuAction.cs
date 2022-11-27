namespace TextAnalyzer.ActionUI;

using TextAnalyzer.Actions;

public class MenuAction: Action
{
    private List<(int, Action)> actions;

    public MenuAction(IEnumerable<Action> actions)
    {
        this.actions = actions.Select((action, i) => (i, action)).ToList();

    }

    public override string Message { get; } = "Choose an action";
    
    public override void Act()
    {
        Action action = GetAction();
        action.Act();
    }

    private Action GetAction()
    {
        int highlighted = 0;
        while (true)
        {
            DrawMenu(highlighted);
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    highlighted = Math.Max(0, highlighted - 1);
                    break;
                case ConsoleKey.DownArrow:
                    highlighted = Math.Min(highlighted + 1, actions.Count - 1);
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    goto done;
                default:
                    continue;
            }
        }

        done: ;
        return actions[highlighted].Item2;
    }

    private void DrawMenu(int highlight)
    {
        foreach ((int, Action) action in actions)
        {
            if (highlight == action.Item1)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"{action.Item1}. ${action.Item2}");
            Console.ResetColor();
        }
    }
}