using task05;
namespace task05tests;

public class TestClass
{
    public int PublicField;
    private string _privateField = "";
    public int Property { get; set; }

    public void Method() { }
    public void Method_Params(string name, int age, double salary) { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();
        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetMethodParams_ReturnsType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("Method");
        Assert.Contains("Void", parameters);
        Assert.Single(parameters);
    }

    [Fact]
    public void GetMethodParams_ReturnsTypeAndParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("Method_Params");
        Assert.Contains("Void", parameters); 
        Assert.Contains("name", parameters); 
        Assert.Contains("age", parameters); 
        Assert.Contains("salary", parameters); 
        Assert.Equal(4, parameters.Count());     
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();
        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();
        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void HasAttribute_ReturnsFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
}
