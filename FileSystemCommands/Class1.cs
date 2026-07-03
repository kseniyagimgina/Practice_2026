using CommandLib;
namespace FileSystemCommands;

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
public class FindFilesCommand: ICommand
{
    public string Filepath {get;}
    public string Mask {get;}
    public List<FileInfo> Files {get;} = new List<FileInfo>();
    public FindFilesCommand(string filepath, string mask)
    {
        Filepath = filepath;
        Mask = mask;
    }
    public void Execute()
    {
        Files.Clear();
        if (!Directory.Exists(Filepath))
        {
            return;
        }
       foreach (var file in Directory.GetFiles(Filepath, Mask, SearchOption.AllDirectories))
        {
        Files.Add(new FileInfo(file));
        }
    }
}
