using System;
using System.IO;
using System.Text;

namespace lab25
{
    // ================================
    // LOGGER (Factory Method)
    // ================================

    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[Console Logger] {message}");
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string path = "log.txt";

        public void Log(string message)
        {
            File.AppendAllText(path,
                $"[File Logger] {message}\n");
        }
    }

    public abstract class LoggerFactory
    {
        public abstract ILogger CreateLogger();
    }

    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger()
            => new ConsoleLogger();
    }

    public class FileLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger()
            => new FileLogger();
    }

    // ================================
    // SINGLETON — LoggerManager
    // ================================

    public class LoggerManager
    {
        private static LoggerManager instance;
        private ILogger logger;

        private LoggerManager() { }

        public static LoggerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoggerManager();

                return instance;
            }
        }

        public void Initialize(LoggerFactory factory)
        {
            logger = factory.CreateLogger();
        }

        public void ChangeFactory(LoggerFactory factory)
        {
            logger = factory.CreateLogger();
        }

        public void Log(string message)
        {
            logger.Log(message);
        }
    }

    // ================================
    // STRATEGY — Data Processing
    // ================================

    public interface IDataProcessorStrategy
    {
        string Process(string data);
        string Name { get; }
    }

    public class EncryptDataStrategy : IDataProcessorStrategy
    {
        public string Name => "Encryption";

        public string Process(string data)
        {
            var sb = new StringBuilder();

            foreach (char c in data)
                sb.Append((char)(c + 1)); // проста шифрація

            return sb.ToString();
        }
    }

    public class CompressDataStrategy : IDataProcessorStrategy
    {
        public string Name => "Compression";

        public string Process(string data)
        {
            return data.Replace(" ", "");
        }
    }

    public class DataContext
    {
        private IDataProcessorStrategy strategy;

        public DataContext(IDataProcessorStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void SetStrategy(IDataProcessorStrategy strategy)
        {
            this.strategy = strategy;
        }

        public string Execute(string data)
        {
            return strategy.Process(data);
        }

        public string GetStrategyName()
        {
            return strategy.Name;
        }
    }

    // ================================
    // OBSERVER
    // ================================

    public class DataPublisher
    {
        public event Action<string, string> DataProcessed;

        public void Publish(string data, string strategy)
        {
            DataProcessed?.Invoke(data, strategy);
        }
    }

    public class ProcessingLoggerObserver
    {
        public void OnDataProcessed(string data, string strategy)
        {
            LoggerManager.Instance.Log(
                $"Processed with {strategy}: {data}");
        }
    }

    // ================================
    // MAIN — DEMO SCENARIOS
    // ================================

    class Program
    {
        static void Main()
        {
            // -----------------------------
            Console.WriteLine(
                "===== SCENARIO 1: FULL INTEGRATION =====");

            LoggerManager.Instance.Initialize(
                new ConsoleLoggerFactory());

            var context =
                new DataContext(new EncryptDataStrategy());

            var publisher = new DataPublisher();
            var observer = new ProcessingLoggerObserver();

            publisher.DataProcessed +=
                observer.OnDataProcessed;

            Run(context, publisher, "Hello World");

            // -----------------------------
            Console.WriteLine(
                "\n===== SCENARIO 2: CHANGE LOGGER =====");

            LoggerManager.Instance.ChangeFactory(
                new FileLoggerFactory());

            Run(context, publisher, "Factory Changed");

            Console.WriteLine(
                "Check log.txt for file logs.");

            // -----------------------------
            Console.WriteLine(
                "\n===== SCENARIO 3: CHANGE STRATEGY =====");

            context.SetStrategy(
                new CompressDataStrategy());

            Run(context, publisher,
                "Observer Strategy Changed");

            Console.WriteLine("\nDone.");
        }

        static void Run(
            DataContext context,
            DataPublisher publisher,
            string data)
        {
            var result = context.Execute(data);

            publisher.Publish(
                result,
                context.GetStrategyName());
        }
    }
}
