using System.Reflection;
namespace CommandRunner
{
    public class Program
    {
        public static void Main()
        {
            string Dir = AppDomain.CurrentDomain.BaseDirectory;
            string FilePath = Path.Combine(Dir, "FileSystemCommands.dll");
            Assembly assembly = Assembly.LoadFrom(FilePath);

            string dir = Path.Combine(Path.GetTempPath(), "Dir");
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "file1.txt"), "Hello World");
            File.WriteAllText(Path.Combine(dir, "file2.doc"), "Document");
            File.WriteAllText(Path.Combine(dir, "file3.txt"), "README");

            Type size = assembly.GetType("FileSystemCommands.DirectorySizeCommand")!;
            if (size != null)
            {
                object filesize= Activator.CreateInstance(size, dir)!;
                dynamic command = filesize;
                command.Execute(); 
                Console.WriteLine($"Размер всех файлов составляет {command.Size} байт");
            }
            Type findmask = assembly.GetType("FileSystemCommands.FindFilesCommand")!;
            if (findmask != null)
            {
                object findfile= Activator.CreateInstance(findmask, dir, "*.txt")!;
                dynamic command = findfile;
                command.Execute(); 
                Console.WriteLine($"Количество файлов с маской {command.Mask} равно {command.Files.Count}:");
                for (int i = 0; i < command.Files.Count; i++)
                {
                    Console.WriteLine(command.Files[i].Name);
                }
            }
        }
    }
}
