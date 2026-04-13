using System;
using System.Diagnostics;
using Lab24.Core;

namespace Lab24.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Інтеграція Composite + Decorator + Proxy ===\n");

            // 1. Створюємо складну композитну структуру (наприклад, 50 фігур)
            var mainGroup = new GraphicGroup();
            for (int i = 1; i <= 50; i++)
            {
                // Деякі фігури декоруємо
                IGraphic shape = new Shape($"Складна Деталь #{i}");
                if (i % 2 == 0) shape = new ColorDecorator(shape, "Синій");

                mainGroup.Add(shape);
            }

            Console.WriteLine($"Створено групу з 50 елементів. Рендеринг займає час...\n");

            // --- БАЗОВИЙ СЦЕНАРІЙ (Без проксі) ---
            Console.WriteLine("--- БАЗОВИЙ РЕНДЕРИНГ (Завжди обчислюється) ---");
            MeasureTime(() => mainGroup.Render(), "Спроба 1 (Без кешу)");
            MeasureTime(() => mainGroup.Render(), "Спроба 2 (Без кешу)");

            // --- ОПТИМІЗОВАНИЙ СЦЕНАРІЙ (З проксі) ---
            Console.WriteLine("\n--- РЕНДЕРИНГ ЧЕРЕЗ PROXY (Кешування) ---");
            var smartProxy = new CachingGraphicProxy(mainGroup);

            MeasureTime(() => smartProxy.Render(), "Спроба 1 (Генеруємо кеш)");
            MeasureTime(() => smartProxy.Render(), "Спроба 2 (Беремо з кешу)");
            MeasureTime(() => smartProxy.Render(), "Спроба 3 (Беремо з кешу)");

            Console.WriteLine("\nНатисніть Enter для виходу...");
            Console.ReadLine(); // Щоб вікно не закрилося!
        }

        static void MeasureTime(Action action, string label)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            Console.WriteLine($"[{label}] Час виконання: {sw.ElapsedMilliseconds} мс.");
        }
    }
}