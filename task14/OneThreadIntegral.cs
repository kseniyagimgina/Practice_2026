namespace task14;

public class OneThreadIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step)
    {
        if (step <= 0)
        {
            throw new ArgumentException("Размер шага должен быть положительным", nameof(step));
        }
        bool ReversedInterval = false;
        if (a > b)
        {
            (a, b) = (b, a);
            ReversedInterval = true;
        }
        double lengthsegment = b - a;
        if (lengthsegment == 0)
        {
            return 0.0;
        }

        int n = (int)Math.Ceiling(lengthsegment / step);
        double h = lengthsegment / n;
        
        double sum = 0.5 * (function(a) + function(b));

        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            sum += function(x);
        }

        double res = sum * h;

        if (ReversedInterval)
        {
            return -res;
        }
        else
        {
            return res;
        }
    }
}
