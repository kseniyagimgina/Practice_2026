using System.Reflection;

namespace task09;
public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }
        Assembly MetaData = Assembly.LoadFrom(args[0]);
        Console.WriteLine($"Библиотека: {MetaData.GetName().Name}");
        Type[] Types = MetaData.GetTypes();
        foreach (Type type in Types)
        {
            if (!type.IsClass)
            {
                continue;
            }
            Console.WriteLine($"Класс {type.Name}:");
            Console.WriteLine("Методы:");
            MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods)
            {
                string modificator = "";
                if (method.IsPublic == true)
                {
                    modificator = "public";
                }
                else
                {
                    modificator = "private";
                }
                string typemethod = method.ReturnType.Name;
                string namemethod = method.Name;
                ParameterInfo[] Parameters = method.GetParameters();
                string parameterstr = string.Join(", ", Parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"{modificator} {typemethod} {namemethod}({parameterstr})");
            }
            Console.WriteLine("Атрибуты:");
            object[] attributes = type.GetCustomAttributes(false);
            foreach (object attribute in attributes)
            {
                Type attr = attribute.GetType();
                string attrName = attr.Name;
                Console.WriteLine($"{attrName}");
            }
            Console.WriteLine("Конструкторы:");
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (ConstructorInfo constr in constructors)
            {
                string modificator = "";
                if (constr.IsPublic == true)
                {
                    modificator = "public";
                }
                else
                {
                    modificator = "private";
                }
                ParameterInfo[] Parameters = constr.GetParameters();
                string parameterstr = string.Join(", ", Parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"{modificator} {type.Name}({parameterstr})");
            }
        } 
    }
}
