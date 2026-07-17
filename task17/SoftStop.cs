namespace task17;

public class SoftStopCommand : ICommand
{
    private readonly ServerThread server;
    public SoftStopCommand(ServerThread serv)
    {
        server = serv;
    }
    public void Execute()
    {
        if (Thread.CurrentThread != server.GetThread())
        {
            throw new InvalidOperationException("SoftStop выполняется только в потоке");
        }
        server.SoftStopInternal();
    }
}

