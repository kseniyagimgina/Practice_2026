using task17;
using ScottPlot;
namespace task17tests;

public class TestCommand : ICommand
{
    private readonly Action action;
    public string Name => "TestCommand";
    public bool IsCompleted { get; private set; }

    public TestCommand(Action act)
    {
        action = act;
    }

    public bool Execute()
    {
        action();
        IsCompleted = true;
        return true;
    }
}
public class LongCommand : ICommand
{
    private readonly int TotalSteps;
    private int currstep;

    public string Name { get; }
    public bool IsCompleted { get; private set; }
    public List<int> ExecuteSteps { get; } = new List<int>();
    private static int StepCounter = 0;

    public LongCommand(string name, int totalsteps)
    {
        Name = name;
        TotalSteps = totalsteps;
        currstep = 0;
        IsCompleted = false;
    }
    public bool Execute()
    {
         lock (typeof(LongCommand))
        {
            StepCounter++;
            ExecuteSteps.Add(StepCounter);
        }
        currstep++;
        Thread.Sleep(5);

        if (currstep >= TotalSteps)
        {
            IsCompleted = true;
            return true;
        }
        return false;
    }
}
public class ServerThreadTests
{
    [Fact]
    public void SoftStopWorkTest()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        server.Start();
        var res = new List<int>();
        server.Enqueue(new TestCommand(() => res.Add(1)));
        server.Enqueue(new TestCommand(() => res.Add(2)));
        server.Enqueue(new TestCommand(() => res.Add(3)));
        server.Enqueue(new SoftStopCommand(server));
        while (scheduler.HasCommand())
        {
            Thread.Sleep(10);
        }
        server.GetThread().Join();
        Assert.Equal(3, res.Count);
    }

    [Fact]
    public void HardStopWorkTest()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        server.Start();
        var res = new List<int>();
        server.Enqueue(new TestCommand(() => res.Add(1)));
        server.Enqueue(new HardStopCommand(server));
        server.Enqueue(new TestCommand(() => res.Add(2)));
        server.GetThread().Join(1000);
        Assert.Single(res);
        Assert.Equal(1, res[0]);
        Assert.DoesNotContain(2, res);
    }

    [Fact]
    public void HardStopThrowsException()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
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
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
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
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        server.Start();
        Thread.Sleep(500);
        Assert.True(server.GetThread().IsAlive);
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
    }

    [Fact]
    public void LongCommandsExecuteConcurrently()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        server.Start();
        var command1 = new LongCommand("1", 5);
        var command2 = new LongCommand("2", 5);
        server.Enqueue(command1);
        server.Enqueue(command2);
        Thread.Sleep(300); 
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
        Assert.False(scheduler.HasCommand());
        Assert.True(command1.IsCompleted);
        Assert.True(command2.IsCompleted);
    }

    [Fact]
    public void ServerThreadWhenQueueIsEmpty()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        server.Start();
        Thread.Sleep(100); 
        var command = new LongCommand("1", 2);
        server.Enqueue(command);
        Thread.Sleep(100);
        Assert.True(command.IsCompleted);
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
    }
}

public class SchedulerTests
{
    [Fact]
    public void SelectReturnsCommandsInOrder()
    {
        var scheduler = new Scheduler();
        var command1 = new TestCommand(() => { });
        var command2 = new TestCommand(() => { });
        var command3 = new TestCommand(() => { });
        scheduler.Add(command1);
        scheduler.Add(command2);
        scheduler.Add(command3);
        Assert.Same(command1, scheduler.Select());
        Assert.Same(command2, scheduler.Select());
        Assert.Same(command3, scheduler.Select());
        Assert.Same(command1, scheduler.Select()); 
    }

    [Fact]
    public void RemoveCompletedCommands()
    {
        var scheduler = new Scheduler();
        var command1 = new TestCommand(() => { });
        var command2 = new TestCommand(() => { });
        scheduler.Add(command1);
        scheduler.Add(command2);
        command1.Execute();
        var selected = scheduler.Select();
        Assert.Same(command2, selected);
        selected = scheduler.Select();
        Assert.Same(command2, selected);
    }

    [Fact]
    public void GraphicScheduler()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        server.Start();
        var commands = new List<LongCommand>();
        for (int i = 1; i <= 4; i++)
        {
            commands.Add(new LongCommand($"cmd {i}", 3));
        }
        foreach (var command in commands)
        {
            server.Enqueue(command);
        }
        while (scheduler.HasCommand())
        {
            Thread.Sleep(10);
        }
        server.Enqueue(new SoftStopCommand(server));
        server.GetThread().Join();
        var plot = new Plot();
        plot.Title("Псевдопараллельная обработка");
        plot.XLabel("Номер вызова Execute");
        plot.YLabel("Команда");
        var yAxis = plot.Axes.Left;
        var manual = new ScottPlot.TickGenerators.NumericManual();
        for (int i = 0; i < commands.Count; i++)
        {
            manual.AddMajor(i, commands[i].Name);
        }
        yAxis.TickGenerator = manual;
        yAxis.Min = -0.5;
        yAxis.Max = commands.Count - 0.5;
        int maxStep = commands.Max(cmd => cmd.ExecuteSteps.Max());
        var allSteps = new List<(int step, int cmd)>();
        for (int step = 1; step <= maxStep; step++)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].ExecuteSteps.Contains(step))
                {
                    allSteps.Add((step, i));
                    break;
                }
            }
        }
        var X = new List<double>();
        var Y = new List<double>();
        for (int i = 0; i < allSteps.Count; i++)
        {
            var (step, cmd) = allSteps[i];
            
            if (i == 0)
            {
                X.Add(step);
                Y.Add(cmd);
            }
            else
            {
                var prevStep = allSteps[i - 1].step;
                var prevPlayer = allSteps[i - 1].cmd;
                X.Add(prevStep);
                Y.Add(cmd);
                X.Add(step);
                Y.Add(cmd);
            }
        }
        if (X.Count > 0)
        {
            var graph = plot.Add.Scatter(X.ToArray(), Y.ToArray());
            graph.MarkerSize = 6;
            graph.MarkerShape = MarkerShape.FilledCircle;
            graph.Color = Colors.Black;
            graph.LineWidth = 2;
            graph.LegendText = "Последовательность выполнения";
        }
        var colors = new[] 
        {
            Colors.Blue, Colors.Pink, Colors.Green, Colors.Purple
        };
        for (int i = 0; i < commands.Count; i++)
        {
            var cmd = commands[i];
            double[] xs = cmd.ExecuteSteps.Select(x => (double)x).ToArray();
            double[] ys = Enumerable.Repeat((double)i, xs.Length).ToArray();
            var scatter = plot.Add.Scatter(xs, ys);
            scatter.MarkerSize = 15;
            scatter.MarkerShape = MarkerShape.FilledCircle;
            scatter.Color = colors[i % colors.Length];
            scatter.LineWidth = 0;
            scatter.LegendText = cmd.Name;
        }
        plot.ShowLegend();
        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
        string filePath = Path.Combine(path, "graphic_scheduler.png");
        plot.SavePng(filePath, 1000, 500);
    }
}