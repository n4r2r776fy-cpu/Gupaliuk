
using lab24.Core;
using lab24.Observers;
using lab24.Strategies;
using System;

namespace lab24
{
    class Program
    {
        static void Main()
        {
            // Processor + Publisher
            var processor = new NumericProcessor(new SquareOperationStrategy());
            var publisher = new ResultPublisher();

            // Observers
            var consoleLogger = new ConsoleLoggerObserver();
            var historyLogger = new HistoryLoggerObserver();
            var thresholdNotifier = new ThresholdNotifierObserver(50);

            // Subscriptions
            publisher.ResultCalculated += consoleLogger.OnResultCalculated;
            publisher.ResultCalculated += historyLogger.OnResultCalculated;
            publisher.ResultCalculated += thresholdNotifier.OnResultCalculated;

            // Test numbers
            double[] numbers = { 2, 3, 5, 8 };

            // --- Square ---
            Console.WriteLine("=== Square ===");
            processor.SetStrategy(new SquareOperationStrategy());
            Run(processor, publisher, numbers);

            // --- Cube ---
            Console.WriteLine("\n=== Cube ===");
            processor.SetStrategy(new CubeOperationStrategy());
            Run(processor, publisher, numbers);

            // --- Square Root ---
            Console.WriteLine("\n=== Square Root ===");
            processor.SetStrategy(new SquareRootOperationStrategy());
            Run(processor, publisher, numbers);

            // Print history
            historyLogger.PrintHistory();
        }

        static void Run(
            NumericProcessor processor,
            ResultPublisher publisher,
            double[] numbers)
        {
            foreach (var n in numbers)
            {
                var result = processor.Process(n);
                publisher.PublishResult(result, processor.GetOperationName());
            }
        }
    }
}
