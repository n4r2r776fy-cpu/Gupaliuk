using System;

namespace IndependentWork19
{
    // ==========================================
    // 1. ІНТЕРФЕЙС ТА КОНКРЕТНІ ПРОДУКТИ
    // ==========================================
    public interface IPaymentGateway
    {
        void ProcessPayment(decimal amount);
    }

    public class PayPalGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"[PayPal] Обробка платежу на суму {amount:C}");
        }
    }

    public class StripeGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"[Stripe] Обробка платежу на суму {amount:C}");
        }
    }

    public class BankTransferGateway : IPaymentGateway
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"[BankTransfer] Обробка банківського переказу на суму {amount:C}");
        }
    }

    // ==========================================
    // 2. АБСТРАКТНА ФАБРИКА ТА ФАБРИЧНИЙ МЕТОД
    // ==========================================
    public abstract class PaymentGatewayFactory
    {
        // Factory Method (Фабричний метод), який підкласи мають реалізувати
        protected abstract IPaymentGateway CreateGateway();

        // Метод, який використовує створений продукт
        public void ExecutePayment(decimal amount)
        {
            IPaymentGateway gateway = CreateGateway();
            gateway.ProcessPayment(amount);
        }
    }

    // ==========================================
    // 3. КОНКРЕТНІ ФАБРИКИ
    // ==========================================
    public class PayPalFactory : PaymentGatewayFactory
    {
        protected override IPaymentGateway CreateGateway() => new PayPalGateway();
    }

    public class StripeFactory : PaymentGatewayFactory
    {
        protected override IPaymentGateway CreateGateway() => new StripeGateway();
    }

    public class BankTransferFactory : PaymentGatewayFactory
    {
        protected override IPaymentGateway CreateGateway() => new BankTransferGateway();
    }

    // ==========================================
    // 4. SINGLETON (Одинак) - Менеджер платежів
    // ==========================================
    public class PaymentProcessor
    {
        private static PaymentProcessor _instance;
        private PaymentGatewayFactory _currentFactory;

        // Приватний конструктор забороняє створення об'єктів через 'new'
        private PaymentProcessor() { }

        // Глобальна точка доступу до єдиного екземпляра
        public static PaymentProcessor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PaymentProcessor();
                }
                return _instance;
            }
        }

        // Динамічна заміна фабрики "на льоту"
        public void SetFactory(PaymentGatewayFactory factory)
        {
            _currentFactory = factory;
        }

        // Делегування виклику поточній фабриці
        public void Process(decimal amount)
        {
            if (_currentFactory == null)
            {
                Console.WriteLine("Помилка: Платіжна фабрика не встановлена!");
                return;
            }
            _currentFactory.ExecutePayment(amount);
        }
    }

    // ==========================================
    // 5. ДЕМОНСТРАЦІЯ (Main)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Система обробки платежів (Factory + Singleton) ===\n");

            // Отримуємо єдиний екземпляр менеджера
            PaymentProcessor processor = PaymentProcessor.Instance;

            // 1. Встановлюємо PayPal
            Console.WriteLine("--- Клієнт обрав PayPal ---");
            processor.SetFactory(new PayPalFactory());
            processor.Process(150.50m);
            processor.Process(25.00m);

            // 2. Змінюємо на Stripe (сам об'єкт processor не змінюється)
            Console.WriteLine("\n--- Клієнт обрав Stripe ---");
            processor.SetFactory(new StripeFactory());
            processor.Process(99.99m);

            // 3. Змінюємо на BankTransfer
            Console.WriteLine("\n--- Клієнт обрав Банківський переказ ---");
            processor.SetFactory(new BankTransferFactory());
            processor.Process(5000.00m);
        }
    }
}