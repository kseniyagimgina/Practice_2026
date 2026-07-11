namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (threadsnumber <= 0)
        {
            throw new ArgumentException("Число потоков должно быть положительным", nameof(threadsnumber));
        }
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
        double[] localResults = new double[threadsnumber];  
        Task[] tasks = new Task[threadsnumber]; 
        double lengthsegment = b - a;
        if (lengthsegment == 0)
        {
            return 0.0;
        } 
        double sublengthsegment = lengthsegment / threadsnumber;

        for (int i = 0; i < threadsnumber; i++)
        {
            int threadIndex = i;
            tasks[i] = Task.Run(() =>
            {
                double A = a + threadIndex * sublengthsegment;
                double B = A + sublengthsegment;
                int n = (int)Math.Ceiling((B - A) / step);
                double h = (B - A) / n;
                double localSum = 0.5 * (function(A) + function(B));
                for (int j = 1; j < n; j++)
                {
                    double x = A + j * h;
                    localSum += function(x);
                }
                localSum = localSum * h;
                localResults[threadIndex] = localSum;
            });  
        }
        Task.WaitAll(tasks);
        double sum = 0;
        for (int i = 0; i < threadsnumber; i++)
        {
            sum += localResults[i];
        }
         if (ReversedInterval)
            {
                return -sum;
            }
            else
            {
                return sum;
            }
    }
}
