using System;

namespace IndependentWork20
{
    // ==========================================
    // ПАТЕРН STRATEGY (Стратегія)
    // ==========================================

    // 1. Спільний інтерфейс для всіх стратегій
    public interface IDataProcessorStrategy
    {
        void Process(string data);
    }

    // 2. Конкретна стратегія: Додати 10
    public class AddTenStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            if (double.TryParse(data, out double number))
                Console.WriteLine($"[Strategy: Add 10] {number} + 10 = {number + 10}");
            else
                Console.WriteLine("[Strategy: Add 10] Помилка: Неможливо перетворити рядок у число.");
        }
    }

    // 3. Конкретна стратегія: Помножити на 2
    public class MultiplyByTwoStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            if (double.TryParse(data, out double number))
                Console.WriteLine($"[Strategy: Multiply by 2] {number} * 2 = {number * 2}");
            else
                Console.WriteLine("[Strategy: Multiply by 2] Помилка: Неможливо перетворити рядок у число.");
        }
    }

    // 4. Конкретна стратегія: Піднести до квадрата
    public class SquareStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            if (double.TryParse(data, out double number))
                Console.WriteLine($"[Strategy: Square] {number} ^ 2 = {Math.Pow(number, 2)}");
            else
                Console.WriteLine("[Strategy: Square] Помилка: Неможливо перетворити рядок у число.");
        }
    }

    // 5. Контекст, який використовує стратегію
    public class DataContext
    {
        private IDataProcessorStrategy _strategy;

        public DataContext(IDataProcessorStrategy strategy)
        {
            _strategy = strategy;
        }

        // Дозволяє змінити стратегію в рантаймі
        public void SetStrategy(IDataProcessorStrategy strategy)
        {
            _strategy = strategy;
            Console.WriteLine("\n--- Стратегію змінено ---");
        }

        public void ExecuteProcessing(string data)
        {
            _strategy.Process(data);
        }
    }

    // ==========================================
    // ПАТЕРН OBSERVER (Спостерігач)
    // ==========================================

    // 1. Видавець (Subject)
    public class DataPublisher
    {
        // Подія, на яку будуть підписуватися спостерігачі
        public event Action<string> DataProcessed;

        public void PublishDataProcessed(string data)
        {
            Console.WriteLine($"\n[Publisher] Публікація нових даних: {data}");
            // Викликаємо подію, якщо є хоча б один підписник
            DataProcessed?.Invoke(data);
        }
    }

    // 2. Спостерігач 1: Вивід у консоль
    public class ConsoleOutputObserver
    {
        // Метод-обробник події
        public void OnDataReceived(string data)
        {
            Console.WriteLine($"[ConsoleOutputObserver] Отримано сигнал про дані: {data}");
        }
    }

    // 3. Спостерігач 2: Калькулятор суми
    public class SumCalculatorObserver
    {
        private double _totalSum = 0;

        // Метод-обробник події
        public void AccumulateSum(string data)
        {
            if (double.TryParse(data, out double number))
            {
                _totalSum += number;
                Console.WriteLine($"[SumCalculatorObserver] Додано {number}. Загальна сума тепер: {_totalSum}");
            }
        }
    }

    // ==========================================
    // ДЕМОНСТРАЦІЯ РОБОТИ (Main)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Патерни Strategy та Observer ===");

            // --- Налаштування Observer ---
            var publisher = new DataPublisher();
            var consoleObserver = new ConsoleOutputObserver();
            var sumObserver = new SumCalculatorObserver();

            // Підписуємо спостерігачів на подію
            publisher.DataProcessed += consoleObserver.OnDataReceived;
            publisher.DataProcessed += sumObserver.AccumulateSum;

            // --- Налаштування Strategy ---
            // Початкова стратегія - додавання 10
            var context = new DataContext(new AddTenStrategy());

            // Виконуємо обробку і публікуємо результат
            context.ExecuteProcessing("5");
            publisher.PublishDataProcessed("15");

            // Змінюємо стратегію на множення
            context.SetStrategy(new MultiplyByTwoStrategy());
            context.ExecuteProcessing("10");
            publisher.PublishDataProcessed("20");

            // Змінюємо стратегію на квадрат
            context.SetStrategy(new SquareStrategy());
            context.ExecuteProcessing("4");
            publisher.PublishDataProcessed("16");

            // Демонстрація відписки
            Console.WriteLine("\n--- Відписуємо ConsoleOutputObserver ---");
            publisher.DataProcessed -= consoleObserver.OnDataReceived;
            publisher.PublishDataProcessed("100"); // Тепер відпрацює тільки SumCalculator
        }
    }
}