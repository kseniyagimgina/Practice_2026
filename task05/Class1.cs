using System;
using System.Reflection;
using System.Collections.Generic;
namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }
    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods().Select(a => a.Name);
    }
    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var public_method = _type.GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        if (public_method == null)
        {
            return Enumerable.Empty<string>();
        }
    
        return new [] {public_method.ReturnType.Name}.Concat(public_method.GetParameters().Select(a => a.Name!));
    }
    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Select(c => c.Name);
    }
    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Select(d => d.Name);
    }
    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.GetCustomAttributes(typeof(T), false).Any();
    }
}
