using System;

namespace IndependentWork23
{
    // ==========================================
    // 1. ПАТЕРН ADAPTER (Адаптер)
    // ==========================================

    // Adaptee: Старий клас, який ми не можемо/не хочемо змінювати
    public class LegacyApiHandler
    {
        public void ProcessRequest(object request)
        {
            Console.WriteLine($"[LegacyApiHandler] Обробка запиту старого формату: {request}");
        }
    }

    // Target: Сучасний інтерфейс, з яким хоче працювати наш новий код
    public interface IExternalApiProcessor
    {
        void Process(string data);
    }

    // Adapter: Перехідник, що з'єднує новий інтерфейс зі старим класом
    public class LegacyApiAdapter : IExternalApiProcessor
    {
        private readonly LegacyApiHandler _legacyHandler = new LegacyApiHandler();

        public void Process(string data)
        {
            Console.WriteLine("[LegacyApiAdapter] Адаптація рядка 'string' у тип 'object'...");
            _legacyHandler.ProcessRequest((object)data); // Перетворення форматів
        }
    }

    // ==========================================
    // 2. ПАТЕРН FACADE (Фасад)
    // ==========================================

    // Складні класи підсистеми
    public class OrderService { public void CreateOrder(string item) => Console.WriteLine($"[OrderService] Замовлення на '{item}' створено."); }
    public class InventoryService { public void UpdateStock(string item) => Console.WriteLine($"[InventoryService] Залишки на складі для '{item}' оновлено."); }
    public class PaymentService { public void ProcessPayment(decimal amount) => Console.WriteLine($"[PaymentService] Оплату в розмірі {amount} грн успішно проведено."); }

    // Facade: Надає єдиний простий метод замість ручного виклику трьох сервісів
    public class ECommerceFacade
    {
        private readonly OrderService _orderService = new OrderService();
        private readonly InventoryService _inventoryService = new InventoryService();
        private readonly PaymentService _paymentService = new PaymentService();

        public void PlaceOrder(string item, decimal amount)
        {
            Console.WriteLine("\n--- [ECommerceFacade] Початок оформлення замовлення ---");
            _orderService.CreateOrder(item);
            _inventoryService.UpdateStock(item);
            _paymentService.ProcessPayment(amount);
            Console.WriteLine("--- [ECommerceFacade] Замовлення успішно оформлено ---\n");
        }
    }

    // ==========================================
    // 3. ПАТЕРН PROXY (Заступник)
    // ==========================================

    // Subject: Спільний інтерфейс
    public interface IImageLoader
    {
        void LoadImage();
    }

    // RealSubject: Важкий об'єкт, який "довго" вантажиться
    public class RealImageLoader : IImageLoader
    {
        private readonly string _filename;

        public RealImageLoader(string filename)
        {
            _filename = filename;
            SimulateHeavyLoading();
        }

        private void SimulateHeavyLoading()
        {
            Console.WriteLine($"[RealImageLoader] Завантаження файлу '{_filename}' з жорсткого диска... (Займає 3 секунди)");
        }

        public void LoadImage()
        {
            Console.WriteLine($"[RealImageLoader] Відображення картинки '{_filename}' на екрані.");
        }
    }

    // Proxy: Заступник, який реалізує "Ледаче завантаження" (Lazy Loading)
    public class LazyImageLoaderProxy : IImageLoader
    {
        private RealImageLoader _realImageLoader;
        private readonly string _filename;

        public LazyImageLoaderProxy(string filename)
        {
            _filename = filename;
            Console.WriteLine($"[LazyProxy] Створено проксі для '{_filename}'. Сама картинка ще НЕ завантажена в пам'ять.");
        }

        public void LoadImage()
        {
            if (_realImageLoader == null)
            {
                Console.WriteLine("[LazyProxy] Клієнт запросив картинку! Створюю реальний об'єкт зараз...");
                _realImageLoader = new RealImageLoader(_filename);
            }
            _realImageLoader.LoadImage();
        }
    }

    // ==========================================
    // 4. ДЕМОНСТРАЦІЯ (Main)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Патерни Adapter, Facade та Proxy ===\n");

            // --- Тест Adapter ---
            Console.WriteLine("1. Тестування ADAPTER:");
            IExternalApiProcessor api = new LegacyApiAdapter();
            api.Process("DataPayload_123");

            // --- Тест Facade ---
            Console.WriteLine("\n2. Тестування FACADE:");
            ECommerceFacade store = new ECommerceFacade();
            store.PlaceOrder("Ноутбук Dell", 35000m);

            // --- Тест Proxy ---
            Console.WriteLine("3. Тестування PROXY (Lazy Loading):");
            IImageLoader image1 = new LazyImageLoaderProxy("photo_high_res.png");
            IImageLoader image2 = new LazyImageLoaderProxy("background.jpg");

            Console.WriteLine("\n> Натисніть Enter, щоб реально завантажити першу картинку...");
            Console.ReadLine(); // Чекаємо реакції

            image1.LoadImage(); // Тільки тут іде реальне завантаження

            Console.WriteLine("\n> Викликаємо LoadImage() для першої картинки ще раз (кеш):");
            image1.LoadImage(); // Об'єкт вже створений, просто малюємо

            Console.WriteLine("\nУсі тести завершено. Натисніть Enter для виходу.");
            Console.ReadLine(); // Щоб консоль не закривалась миттєво!
        }
    }
}
