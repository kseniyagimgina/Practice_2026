using System.Reflection;
using FileSystemCommands;
using task07;
using task09;
namespace task09tests;

public class Console_Test
{
    public class ConsoleOutputTest
    {
        public string ConsoleRun()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            string path = Path.GetFullPath("../../../../FileSystemCommands/bin/Debug/net10.0/FileSystemCommands.dll");
            string[] args = new string[] {path};
            Program.Main(args);
            return output.ToString();
        }
        [Fact]
        public void MetaDataOfDirectorySizeCommand()
        {
            string output = ConsoleRun();
            Assert.Contains("Библиотека: FileSystemCommands", output);

            Assert.Contains("Класс DirectorySizeCommand:", output);

            Assert.Contains("public String get_Filepath()", output);
            Assert.Contains("public Int64 get_Size()", output);
            Assert.Contains("private Void set_Size(Int64 value)", output);
            Assert.Contains("public List`1 get_Files()", output);
            Assert.Contains("public Void Execute()", output);

            Assert.Contains("DisplayNameAttribute", output);
            Assert.Contains("VersionAttribute", output);

            Assert.Contains("public DirectorySizeCommand(String filepath)", output);

            
        }
        [Fact]
        public void MetaDataOfFindFilesCommand()
        {
            string output = ConsoleRun();
            
            Assert.Contains("Библиотека: FileSystemCommands", output);

            Assert.Contains("Класс FindFilesCommand:", output);

            Assert.Contains("public String get_Filepath()", output);
            Assert.Contains("public String get_Mask()", output);
            Assert.Contains("public List`1 get_Files()", output);
            Assert.Contains("public Void Execute()", output);

            Assert.Contains("DisplayNameAttribute", output);
            Assert.Contains("VersionAttribute", output);


            Assert.Contains("public FindFilesCommand(String filepath, String mask)", output);

        }
        [Fact]
        public void MetaDataOfAttributeClasses()
        {
            string output = ConsoleRun();

            Assert.Contains("Библиотека: FileSystemCommands", output);

            Assert.Contains("Класс DisplayNameAttribute:", output);
            Assert.Contains("public String get_DisplayName()", output);
            Assert.Contains("public DisplayNameAttribute(String displayname)", output);

            Assert.Contains("AttributeUsageAttribute", output);

            Assert.Contains("Класс VersionAttribute:", output);
            Assert.Contains("public Int32 get_Major()", output);
            Assert.Contains("public VersionAttribute(Int32 major, Int32 minor)", output);
        }
        [Fact]
        public void MetaDataOfSampleClassAndReflectionHelper()
        {
            string output = ConsoleRun();

            Assert.Contains("Библиотека: FileSystemCommands", output);

            Assert.Contains("Класс SampleClass:", output);
            Assert.Contains("public Void TestMethod()", output);
            Assert.Contains("public Int32 get_Number()", output);
            Assert.Contains("public SampleClass()", output);

            Assert.Contains("Класс ReflectionHelper:", output);
            Assert.Contains("public Void PrintTypeInfo(Type type)", output);
        }
    }
}
