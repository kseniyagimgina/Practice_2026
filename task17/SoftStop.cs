namespace task17;

public class SoftStopCommand : ICommand
{
    private readonly ServerThread server;
    public string Name => "SoftStop";
    public bool IsCompleted { get; private set; }
    public SoftStopCommand(ServerThread serv)
    {
        server = serv;
    }
    public bool Execute()
    {
        if (Thread.CurrentThread != server.GetThread())
        {
            throw new InvalidOperationException("SoftStop выполняется только в потоке");
        }
        server.SoftStopInternal();
        IsCompleted = true;
        return true;
    }
}
