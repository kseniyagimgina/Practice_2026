using task17;
using task18;
using task19;
using ScottPlot;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace task19tests;


public class Task19CombinedTest
{
    [Fact]
    public void FiveCommandsThreeTimes()
    {
        var scheduler = new Scheduler();
        var server = new ServerThread(scheduler);
        var progressData = new ConcurrentDictionary<int, List<(double TimeMs, double Progress)>>();
        var sw = Stopwatch.StartNew();
        const int CommandCount = 5;
        const int StepsPeriodCommand = 3;
        for (int i = 1; i <= CommandCount; i++)
        {
            progressData[i] = new List<(double, double)>();
            int id = i;
            
            var testcmd = new TestCommand(id);
            var repeatedCmd = new RepeatedCommand(testcmd, StepsPeriodCommand);
            var statCmd = new Statistics(repeatedCmd, id, StepsPeriodCommand, 
                (time, prog) => progressData[id].Add((time, prog)));
                
            scheduler.Add(statCmd);
        }
        server.Start();
        int timeout = 3000;
        while (sw.ElapsedMilliseconds < timeout)
        {
            bool allDone = true;
            for (int i = 1; i <= CommandCount; i++)
            {
                if (progressData[i].Count < StepsPeriodCommand)
                {
                    allDone = false;
                    break;
                }
            }
            if (allDone) break;
            Thread.Sleep(10);
        }
        server.Enqueue(new HardStopCommand(server));
        server.GetThread()?.Join();
        Assert.All(progressData.Values, list => Assert.Equal(StepsPeriodCommand, list.Count));
        Graphic(progressData, CommandCount);
    }
    private void Graphic(ConcurrentDictionary<int, List<(double TimeMs, double Progress)>> data, int count)
    {
        var plt = new Plot();
        var colors = new[] { "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728", "#9467bd" };
        for (int i = 1; i <= count; i++)
        {
            var points = data[i].OrderBy(x => x.TimeMs).ToList();
            double[] xs = points.Select(p => p.TimeMs).ToArray();
            double[] ys = points.Select(p => p.Progress).ToArray();
            var sig = plt.Add.SignalXY(xs, ys);
            sig.Color = Color.FromHex(colors[i - 1]);
            sig.LineWidth = 2;
            sig.MarkerSize = 8;
            sig.MarkerShape = MarkerShape.FilledCircle;
            sig.LegendText = $"Команда {i}";
        }
        plt.Title("Выполнение 5 экземпляров TestCommand 3 раза", 16);
        plt.XLabel("Время выполнения", 14);
        plt.YLabel("Процесс выполнения", 14);
        plt.Axes.AutoScale();
        plt.ShowLegend();
        plt.Grid.MajorLineColor = Color.FromHex("#e0e0e0");
        string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string outputPath = Path.Combine(projectPath, "GraphicTestCommand.png");
        plt.SavePng(outputPath, 1000, 600);
        Assert.True(File.Exists(outputPath));
    }
}

public class RepeatedCommand : ILongCommand
{
    private readonly ICommand Inner;
    private readonly int MaxExecutions;
    private int CurrentExecutions;

    public bool IsCompleted => CurrentExecutions  >= MaxExecutions;

    public RepeatedCommand(ICommand inner, int maxExecutions)
    {
        Inner = inner;
        MaxExecutions = maxExecutions;
    }

    public void Execute()
    {
        Inner.Execute();
        CurrentExecutions++;
    }
}

public class Statistics : ILongCommand
{
    private readonly ILongCommand Inner;
    private readonly int Id;
    private readonly int MaxSteps;
    private readonly Action<double, double> Progress;
    private int CurrentStep;

    public bool IsCompleted => Inner.IsCompleted;

    public Statistics(ILongCommand inner, int id, int maxSteps, Action<double, double> progress)
    {
        Inner = inner;
        Id = id;
        MaxSteps = maxSteps;
        Progress = progress;
    }

    public void Execute()
    {
        Inner.Execute();
        CurrentStep++;
        double progress = (CurrentStep / (double)MaxSteps) * 100.0;
        Progress(Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency * 1000.0, progress);
    }
}
