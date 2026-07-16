namespace task17;

public class ServerThread
{
    private readonly Queue<ICommand> queue = new Queue<ICommand>();
    private readonly object locked = new object();
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
            if (command != null)
            {
                try
                {
                    command.Execute();
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
