using task17;
namespace task18;
public class Scheduler : IScheduler
{
    private readonly Queue<ICommand> commands = new Queue<ICommand>();
    private readonly object locked = new object();
    public bool HasCommand()
    {
        lock (locked)
        {
            return commands.Count > 0;
        }
    }
    public ICommand Select()
    {
        lock (locked)
        {
            if (commands.Count == 0)
                return null;
            
            var command = commands.Dequeue();
            return command;
        }
    }
    public void Add(ICommand cmd)
    {
        lock (locked)
        {
            commands.Enqueue(cmd);
        }
    }
}
