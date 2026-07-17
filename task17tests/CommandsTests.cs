using task17;
namespace task17tests;

public class TestCommand : ICommand
{
    private readonly Action action;
    public TestCommand(Action act) 
    { 
        action = act;
    }
    public void Execute() 
    { 
        action(); 
    }
}
public class ServerThreadTests
{
    [Fact]
    public void SoftStopWorkTest()
    {
        var server = new ServerThread();
        server.Start();
        var res = new List<int>();
        server.Enqueue(new TestCommand(() => res.Add(1)));
        server.Enqueue(new TestCommand(() => res.Add(2)));
        server.Enqueue(new TestCommand(() => res.Add(3)));
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
        Assert.Equal(3, res.Count);
    }

    [Fact]
    public void HardStopWorkTest()
    {
        var server = new ServerThread();
        server.Start();
        var res = new List<int>();
        server.Enqueue(new TestCommand(() => res.Add(1)));
        server.Enqueue(new HardStopCommand(server));
        server.Enqueue(new TestCommand(() => res.Add(2)));
        server.GetThread().Join();
        Assert.Single(res);
        Assert.Equal(1, res[0]);
        Assert.DoesNotContain(2, res);
    }

    [Fact]
    public void HardStopThrowsException()
    {
        var server = new ServerThread();
        server.Start();
        var command = new HardStopCommand(server);
        var exception = Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Contains("HardStop выполняется только в потоке", exception.Message);
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
    }

    [Fact]
    public void SoftStopThrowsException()
    {
        var server = new ServerThread();
        server.Start();
        var command = new SoftStopCommand(server);
        var exception = Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Contains("SoftStop выполняется только в потоке", exception.Message);
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
    }

    [Fact]
    public void ThreadWaitsForCommands()
    {
        var server = new ServerThread();
        server.Start();
        Thread.Sleep(500);
        Assert.True(server.GetThread().IsAlive);
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
    }
}
