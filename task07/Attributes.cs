using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class DisplayNameAttribute: Attribute
{
    public string DisplayName {get;}
    public DisplayNameAttribute(string displayname)
    {
        DisplayName = displayname;
    }
}
[AttributeUsage(AttributeTargets.Class,Inherited = true, AllowMultiple = false)]
public class VersionAttribute: Attribute
{
    public int Major {get;}
    public int Minor {get;}
    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}
[DisplayName("Пример класса")]
[Version(1,0)]
public class SampleClass
{
   [DisplayName("Тестовый метод")]
   public void TestMethod() {}
   [DisplayName("Числовое свойство")]
   public int Number {get; set;}
}
public static class ReflectionHelper 
{
    public static void PrintTypeInfo(Type type)
    {
        var Name = type.GetCustomAttribute<DisplayNameAttribute>();
        if (Name != null)
        {
            Console.WriteLine($"{type.Name} - {Name.DisplayName}");
        }
        var Vers = type.GetCustomAttribute<VersionAttribute>();
        if (Vers != null)
        {
            Console.WriteLine($"Version: {Vers.Major}.{Vers.Minor}");
        }
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(a => a.GetCustomAttribute<DisplayNameAttribute>() != null).ToList();
        Console.WriteLine("Methods:");
        if (methods.Count() != 0)
        {
            foreach (var method in methods)
            {
                Console.WriteLine($"{method.Name} - {method.GetCustomAttribute<DisplayNameAttribute>()!.DisplayName}");
            }
        
        }
        else
        {
            Console.WriteLine("No methods");
        }
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(a => a.GetCustomAttribute<DisplayNameAttribute>() != null).ToList();
        Console.WriteLine("Properties:");
        if (properties.Count() != 0)
        {
            foreach (var property in properties)
            {
                Console.WriteLine($"{property.Name}, {property.PropertyType.Name} - {property.GetCustomAttribute<DisplayNameAttribute>()!.DisplayName}");
            }
        }
        else
        {
            Console.WriteLine("No properties");
        }
    } 
}
