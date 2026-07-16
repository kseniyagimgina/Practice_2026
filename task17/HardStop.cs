namespace task17;

public class HardStopCommand : ICommand
{
    private readonly ServerThread server;
    public string Name => "HardStop";
    public bool IsCompleted { get; private set; }
    public HardStopCommand(ServerThread serv)
    {
        server = serv;
    }
    public bool Execute()
    {
        if (Thread.CurrentThread != server.GetThread())
        {
            throw new InvalidOperationException("HardStop выполняется только в потоке");
        }
        server.HardStopInternal();
        IsCompleted = true;
        return true;
    }
}
