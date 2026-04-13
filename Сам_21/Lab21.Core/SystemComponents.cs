using System;

namespace Lab21.Core
{
    // --- ПАТЕРН STRATEGY (Стратегія знижок) ---
    public interface IDiscountStrategy { decimal ApplyDiscount(decimal amount); }
    public class NoDiscountStrategy : IDiscountStrategy { public decimal ApplyDiscount(decimal amount) => amount; }
    public class HalfPriceStrategy : IDiscountStrategy { public decimal ApplyDiscount(decimal amount) => amount * 0.5m; }

    // --- ПАТЕРН FACTORY (Фабрика сповіщень) ---
    public interface INotification { string Send(string message); }
    public class EmailNotification : INotification { public string Send(string msg) => $"Email sent: {msg}"; }
    public class SmsNotification : INotification { public string Send(string msg) => $"SMS sent: {msg}"; }

    public static class NotificationFactory
    {
        public static INotification Create(string type) => type.ToLower() switch
        {
            "email" => new EmailNotification(),
            "sms" => new SmsNotification(),
            _ => throw new ArgumentException("Unknown notification type")
        };
    }

    // --- ПАТЕРНИ SINGLETON та OBSERVER (Центр обробки) ---
    public class ProcessingHub
    {
        private static ProcessingHub _instance;
        public static ProcessingHub Instance => _instance ??= new ProcessingHub();

        // Observer event
        public event Action<decimal> OnOrderProcessed;

        // Поточні залежності
        public IDiscountStrategy CurrentStrategy { get; set; }
        public INotification CurrentNotification { get; set; }

        private ProcessingHub()
        {
            // Дефолтні значення
            CurrentStrategy = new NoDiscountStrategy();
            CurrentNotification = new EmailNotification();
        }

        // Скидання Singleton для тестів (щоб тести не впливали один на одного)
        public static void ResetForTesting() => _instance = null;

        public decimal ProcessOrder(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            if (CurrentStrategy == null) throw new InvalidOperationException("Strategy is missing.");
            if (CurrentNotification == null) throw new InvalidOperationException("Notification is missing.");

            // 1. Використовуємо Strategy
            decimal finalAmount = CurrentStrategy.ApplyDiscount(amount);

            // 2. Використовуємо Factory об'єкт
            CurrentNotification.Send($"Order processed for {finalAmount}");

            // 3. Використовуємо Observer
            OnOrderProcessed?.Invoke(finalAmount);

            return finalAmount;
        }
    }
}