using task18;
namespace task17;

public class ServerThread
{
    private readonly Queue<ICommand> queue = new Queue<ICommand>();
    private readonly object locked = new object();
    private readonly IScheduler scheduler;
    private Thread thread;
    private bool hardstop;
    private bool softstop;
    private readonly Action<Exception, ICommand> ExceptionHandler;
    public ServerThread(Action<Exception, ICommand>? error = null)
    {
        if (error != null)
        {
            ExceptionHandler = error;
        }
        else
        {
            ExceptionHandler = (exception, command) => { };
        }
    }
    public ServerThread(IScheduler scheduler, Action<Exception, ICommand>? error = null)
    {
        this.scheduler = scheduler;
        if (error != null)
        {
            ExceptionHandler = error;
        }
        else
        {
            ExceptionHandler = (exception, command) => { };
        }
    }
    public void Start()
    {
        thread = new Thread(Work);
        thread.Start();
    }
    public void Enqueue(ICommand command)
    {
        lock (queue)
        {
            queue.Enqueue(command);
            Monitor.Pulse(queue);
        }
    }
    private void Work()
    {
        while (true)
        {
            ICommand? command = null;
            if (scheduler != null && scheduler.HasCommand())
            {
                command = scheduler.Select();
            }
            else
            {
                lock (queue)
                {
                    while (queue.Count == 0 && !hardstop && !softstop)
                    {
                        Monitor.Wait(queue);
                    }
                    if (hardstop)
                        break;
                    if (softstop && queue.Count == 0)
                        break;
                    if (queue.Count > 0)
                        command = queue.Dequeue();
                }
            }
            if (command != null)
            {
                try
                {
                    command.Execute();
                    if (scheduler != null && command is ILongCommand longCommand && !longCommand.IsCompleted)
                    {
                        scheduler.Add(command);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandler(exception, command);
                }
            }
        }
    }
    public void HardStopInternal()
    {
        lock (queue)
        {
            queue.Clear();
            hardstop = true;
            Monitor.Pulse(queue);
        }
    }
    public void SoftStopInternal()
    {
        lock (queue)
        {
            softstop = true;
            Monitor.Pulse(queue);
        }
    }
    public Thread GetThread()
    {
        return thread;
    }
}
