using System.Collections.Generic;

namespace lab24.Observers
{
    public class HistoryLoggerObserver
    {
        public List<string> History { get; } = new();

        public void OnResultCalculated(double result, string operation)
        {
            History.Add($"{operation} → {result}");
        }

        public void PrintHistory()
        {
            System.Console.WriteLine("\n--- History ---");
            foreach (var record in History)
            {
                System.Console.WriteLine(record);
            }
        }
    }
}
