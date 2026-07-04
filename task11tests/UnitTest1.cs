namespace task11tests;
using task11;
public class UnitTest1
{
    [Fact]
    public void TestCalculator()
    {
        var calculator = GenerationOfClass.CreateCalculator();

        Assert.NotNull(calculator);
        Assert.Equal(4, calculator.Add(2, 2));
        Assert.Equal(1, calculator.Minus(10, 9));
        Assert.Equal(21, calculator.Mul(7, 3));
        Assert.Equal(4, calculator.Div(8, 2));
    }

    [Fact]
    public void TestCalculatorWithNegative()
    {
        var calculator = GenerationOfClass.CreateCalculator();
        
        Assert.Equal(-12, calculator.Add(-9, -3));
        Assert.Equal(2, calculator.Minus(-7, -9));
        Assert.Equal(-36, calculator.Mul(-12, 3));
        Assert.Equal(1, calculator.Div(-6, -6));
    }

    [Fact]
    public void DivideByZero()
    {
        var calculator = GenerationOfClass.CreateCalculator();

        Assert.Throws<DivideByZeroException>(() => calculator.Div(10, 0));
    }
}
