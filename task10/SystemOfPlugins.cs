using System.Reflection;
using Microsoft.VisualBasic;

namespace task10;

public class PluginLib
{
    public interface IPlugin
    {
        void Execute();
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginLoadAttribute : Attribute
    {
        public string[] Deps { get; set; } = [];
    }
    public class PluginHost
    {
        public List<Type> Plugins {get;} = [];
        public void LoadFromDirectory(string filepath)
        {
            if (!Directory.Exists(filepath))
            {
                throw new DirectoryNotFoundException($"Директории {filepath} нет");
            }

            foreach (var file in Directory.GetFiles(filepath, "*.dll"))
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(file);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Ошибка {exception.GetType().Name}: {exception.Message}", exception);
                }
                var types = assembly.GetTypes().Where(a => a.GetCustomAttribute<PluginLoadAttribute>() != null && typeof(IPlugin).IsAssignableFrom(a) && !a.IsAbstract);
                Plugins.AddRange(types);
            }
        }
        public void ExecutePlugin()
        {
            foreach (var type in SortedDeps())
                ((IPlugin)Activator.CreateInstance(type)).Execute();
        }
        private List<Type> SortedDeps()
        {
            var res = new List<Type>();
            var visited = new HashSet<Type>();
            void Visit(Type type)
            {
                if (!visited.Add(type))
                {
                    return;
                }
                var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
                string[] deps;
                if (attribute != null)
                {
                    deps = attribute.Deps;
                }
                else
                {
                    deps = [];
                }
                foreach (var name in deps)
                {
                    var dep = Plugins.FirstOrDefault(a => a.Name == name);
                    if (dep != null)
                    {
                        Visit(dep);
                    }
                }
                res.Add(type);
            }
            foreach (var plugin in Plugins)
            {
                Visit(plugin);
            }
            return res;
        }
    }
}
