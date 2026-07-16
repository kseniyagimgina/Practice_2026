using System.Collections.Concurrent;

namespace task17;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> Queue = new BlockingCollection<ICommand>();
    private readonly IScheduler Scheduler;
    private Thread? thread;
    private volatile bool hardstop;
    private volatile bool softstop;
    private readonly Action<Exception, ICommand?> ExceptionHandler;
    private const int WaitTimeOut = 10;
    public ServerThread(IScheduler scheduler, Action<Exception, ICommand?>? error = null)
    {
        if (scheduler != null)
        {
            ExceptionHandler = error;
        }
        else
        {
            ExceptionHandler = (exception, command) => { };
        }
        if (scheduler == null)
        {
            throw new ArgumentNullException(nameof(scheduler));
        }
        Scheduler = scheduler;
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
        thread = new Thread(Work)
        {
            IsBackground = true,
            Name = "ServerThread"
        };
        thread.Start();
    }
    public void Enqueue(ICommand command)
    {
        if (!Queue.IsAddingCompleted)
        {
            Queue.Add(command);
        }
    }
    private void Work()
    {
        while (true)
        {
            ICommand? newcommand = null;
            bool getcommand = false;
            try
            {
                getcommand = Queue.TryTake(out newcommand, WaitTimeOut);
            }
            catch (InvalidOperationException)
            {
                break;
            }
            if (getcommand && newcommand != null)
            {
                Scheduler.Add(newcommand);
            }
            if (hardstop)
                break;
            if (softstop && !Scheduler.HasCommand() && Queue.Count == 0)
                break;
            if (Scheduler.HasCommand())
            {
                var command_execute = Scheduler.Select();
                if (command_execute != null)
                {
                    try
                    {
                        command_execute.Execute();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler(exception, command_execute);
                    }
                }
            }
        }
    }
    public void SoftStopInternal()
    {
        softstop = true;
    }
    public void HardStopInternal()
    {
        hardstop = true;
        Queue.CompleteAdding();
    }
    public Thread GetThread()
    {
        return thread;
    }
}
