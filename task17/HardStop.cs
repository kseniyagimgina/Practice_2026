namespace task17;

public class HardStopCommand : ICommand
{
    private readonly ServerThread server;
    public HardStopCommand(ServerThread serv)
    {
        server = serv;
    }
    public void Execute()
    {
        if (Thread.CurrentThread != server.GetThread())
        {
            throw new InvalidOperationException("HardStop выполняется только в потоке");
        }
        server.HardStopInternal();
    }
}
