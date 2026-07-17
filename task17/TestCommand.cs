using task17;
namespace task19;

public class TestCommand(int id) : ICommand
{
    int counter = 0;

    public void Execute()
    {
        Thread.Sleep(10);
        Console.WriteLine($"Поток {id} вызов {++counter}");
    }
}
