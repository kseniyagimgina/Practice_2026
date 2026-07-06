namespace FileSystemCommands;

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
