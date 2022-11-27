namespace TextAnalyzer.ActionUI;

using TextAnalyzer.Actions;

public class MenuAction: Action
{
    public List<Action> actions;

    public MenuAction(List<Action> actionList, string name="Menu action")
    {
        if (actionList.Count < 1) throw new Exception("Missing actions to form a meaningful menu");
        Message = name;
        actions = actionList;
    }

    public override string Message { get; internal set; }
    
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
        return actions[highlighted];
    }

    private void DrawMenu(int highlight)
    {
        Console.Clear();
        Console.WriteLine("Choose an action");
        foreach ((int, Action) action in actions
                     .Select((action, i) => (i, action))
                 )
        {
            if (highlight == action.Item1)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"{action.Item1}. {action.Item2.Message}");
            Console.ResetColor();
        }
    }
}