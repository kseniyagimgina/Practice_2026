using static task10.PluginLib;
namespace task10tests;

public class UnitTest1
{
    public static class TestLog
    {
        public static List<string> Sortlist = new();
    }
    [PluginLoad]
    public class Plugin1 : IPlugin
    {
        public void Execute()
        {
            TestLog.Sortlist.Add("1");
        }
    }
    [PluginLoad(Deps = new[] { "PluginA" })]
    public class Plugin2 : IPlugin
    {
        public void Execute()
        {
            TestLog.Sortlist.Add("2");
        }
    }
    [PluginLoad(Deps = new[] { "PluginA", "PluginB" })]
    public class Plugin3 : IPlugin
    {
        public void Execute()
        {
            TestLog.Sortlist.Add("3");
        }
    }
    public class PluginTests
    {
        [Fact]
        public void TestPlugin()
        {
            TestLog.Sortlist.Clear();
            var host = new PluginHost();
            var testpath = Path.GetDirectoryName(typeof(PluginTests).Assembly.Location);
            host.LoadFromDirectory(testpath);
            host.ExecutePlugin();
            Assert.Equal(new[] { "1", "2", "3" }, TestLog.Sortlist);
        }

        [Fact]
        public void EmptyDirWithoutErrors()
        {
            var empty = Path.Combine(Path.GetTempPath(), "emptyplugins" + Path.GetRandomFileName());
            Directory.CreateDirectory(empty);
            var host = new PluginHost();
            host.LoadFromDirectory(empty);
            host.ExecutePlugin();
            Directory.Delete(empty, true);
        }
    }
}

