using System;

namespace lab24.Observers
{
    public class ConsoleLoggerObserver
    {
        public void OnResultCalculated(double result, string operation)
        {
            Console.WriteLine($"[Console] Operation: {operation} | Result: {result}");
        }
    }
}
