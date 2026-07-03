using System.Reflection;
using FileSystemCommands;
using task07;
namespace task09tests;

public class Console_Test
{
    [Fact]
    public void GetMethodsFromDirectorySizeCommand()
    {
        Type type = typeof(DirectorySizeCommand);
        MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.Contains(methods, a => a.Name == "Execute");
        Assert.Contains(methods, b => b.Name == "get_Size");
        Assert.Contains(methods, c => c.Name == "get_Filepath");
    }

    [Fact]
    public void GetMethodsFromFindFilesCommand() 
    {
        Type type = typeof(FindFilesCommand);
        MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.Contains(methods, a => a.Name == "Execute");
        Assert.Contains(methods, b => b.Name == "get_Mask");
        Assert.Contains(methods, c => c.Name == "get_Filepath");
    }

    [Fact]
    public void GetConstructorsFromDirectorySizeCommandWithParameters()
    {
        Type type = typeof(DirectorySizeCommand);
        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.Single(constructors[0].GetParameters());
        Assert.Equal("String", constructors[0].GetParameters()[0].ParameterType.Name);
    }

    [Fact]
    public void GetConstructorsFromFindFilesCommandWithParameters()
    {
        Type type = typeof(FindFilesCommand);
        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.Equal(2, constructors[0].GetParameters().Length); 
        Assert.Equal("String", constructors[0].GetParameters()[0].ParameterType.Name);
        Assert.Equal("String", constructors[0].GetParameters()[1].ParameterType.Name);
    }

    [Fact]
    public void GetAttributesFromDirectorySizeCommandWithParameters()
    {
        Type type = typeof(DirectorySizeCommand);
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(displayNameAttr);
        Assert.Equal("Команда определения размера каталога", displayNameAttr.DisplayName);
        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(versionAttr);
        Assert.Equal(1, versionAttr.Major);
        Assert.Equal(0, versionAttr.Minor);
    }

    [Fact]
    public void GetAttributesFromFindFilesCommand()
    {
        Type type = typeof(FindFilesCommand);
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(displayNameAttr);
        Assert.Equal("Команда поиска файлов по маске", displayNameAttr.DisplayName);
        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(versionAttr);
        Assert.Equal(1, versionAttr.Major);
        Assert.Equal(0, versionAttr.Minor);
    }
}
