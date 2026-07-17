using task17;
namespace task18;

public interface ILongCommand : ICommand
{
    bool IsCompleted { get; }
}
