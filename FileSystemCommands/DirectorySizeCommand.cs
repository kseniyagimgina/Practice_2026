using task07;
using CommandLib;
namespace FileSystemCommands;

[DisplayName("Определение размера каталога")]
[Version(1,0)]
public class DirectorySizeCommand: ICommand
{
    public string Filepath {get;}
    public long Size {get; private set;}
    public List<FileInfo> Files {get;} = new List<FileInfo>();
    public DirectorySizeCommand(string filepath)
    {
        Filepath = filepath;
    }
    public void Execute()
    {
        Size = 0;
        Files.Clear();
        if (!Directory.Exists(Filepath))
        {
            return;
        }
        var dir = new DirectoryInfo(Filepath);
        foreach (var file in dir.EnumerateFiles(".", SearchOption.AllDirectories))
        {
            Size += file.Length;
            Files.Add(file);
        }
    }
}
