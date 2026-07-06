using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}
public static class GenerationOfClass
{
    public static ICalculator CreateCalculator()
    {
        string code = @"
            using task11;
            public class Calculator : ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }";
        try
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
            };
            var compilation = CSharpCompilation.Create("Calculator", new[] {tree}, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using var memory = new MemoryStream();
            var res = compilation.Emit(memory);
            if (!res.Success)
            {
                var errors = res.Diagnostics.Where(a => a.Severity == DiagnosticSeverity.Error).Select(b => b.GetMessage());
                throw new Exception($"Ошибка компиляции: {string.Join(", ", errors)}");
            }
            var assembly = Assembly.Load(memory.ToArray());
            var type = assembly.GetType("Calculator");
            var instance = Activator.CreateInstance(type);
            return (ICalculator)instance;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            throw;
        }
    }
}
