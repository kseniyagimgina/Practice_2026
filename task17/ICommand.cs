namespace task17;

public interface ICommand
{
    bool Execute();
    bool IsCompleted { get; }
    string Name { get; }
}
