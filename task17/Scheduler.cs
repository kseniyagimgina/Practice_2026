namespace task17;

public class Scheduler : IScheduler
{
    private readonly List<ICommand> commands = new List<ICommand>();
    private int current_index = 0;
    private readonly object locked = new object();

    public void Add(ICommand command)
    {
        lock (locked)
        {
            commands.Add(command);
        }
    }
    public bool HasCommand()
    {
        lock (locked)
        {
            CleanupCompleteCmd();
            return commands.Count > 0;
        }
    }
    public ICommand Select()
    {
        lock (locked)
        {
            CleanupCompleteCmd();
            if (commands.Count == 0)
            {
                return null;
            }
            var command = commands[current_index];
            current_index = (current_index + 1) % commands.Count;
            return command;
        }
    }
    private void CleanupCompleteCmd()
    {
        commands.RemoveAll(command => command.IsCompleted);

        if (commands.Count == 0)
        {
            current_index = 0;
        }
        else if (current_index >= commands.Count)
        {
            current_index = 0;
        }
    }
}