using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace lab29v2
{
    class Program
    {
        const string InputFile = "sales_data.csv";
        const string OutputFileAsync = "high_sales_async.csv";
        const string OutputFileSync = "high_sales_sync.csv";
        const int RowCount = 1_000_000; // Мільйон рядків!

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна 29: Асинхронне читання файлів ===");

            // 1. Генерація файлу
            Console.WriteLine($"\n[1] Генерація файлу на {RowCount:N0} рядків (це може зайняти кілька секунд)...");
            await GenerateLargeFileAsync(InputFile, RowCount);
            Console.WriteLine("Генерацію завершено!");

            // 2. Асинхронне читання та фільтрація
            Console.WriteLine("\n[2] Запуск АСИНХРОННОЇ обробки...");
            var stopwatchAsync = Stopwatch.StartNew();
            var asyncResult = await ProcessSalesAsync(InputFile, OutputFileAsync);
            stopwatchAsync.Stop();

            Console.WriteLine($"Результат (Асинхронно):");
            Console.WriteLine($"- Загальна сума продажів: {asyncResult.TotalSum:C2}");
            Console.WriteLine($"- Знайдено великих продажів (> 1000): {asyncResult.FilteredCount:N0}");
            Console.WriteLine($"- Час виконання: {stopwatchAsync.ElapsedMilliseconds} мс");

            // 3. Синхронне читання (для порівняння)
            Console.WriteLine("\n[3] Запуск СИНХРОННОЇ обробки...");
            var stopwatchSync = Stopwatch.StartNew();
            var syncResult = ProcessSalesSync(InputFile, OutputFileSync);
            stopwatchSync.Stop();

            Console.WriteLine($"Результат (Синхронно):");
            Console.WriteLine($"- Загальна сума продажів: {syncResult.TotalSum:C2}");
            Console.WriteLine($"- Знайдено великих продажів (> 1000): {syncResult.FilteredCount:N0}");
            Console.WriteLine($"- Час виконання: {stopwatchSync.ElapsedMilliseconds} мс");

            Console.WriteLine("\n=== Готово! Перевір папку проєкту на наявність CSV-файлів. ===");
        }

        // ==========================================
        // МЕТОДИ ДЛЯ РОБОТИ З ФАЙЛАМИ
        // ==========================================

        static async Task GenerateLargeFileAsync(string filename, int rows)
        {
            using var writer = new StreamWriter(filename);
            await writer.WriteLineAsync("TransactionId,Date,Amount"); // Заголовок CSV

            var rnd = new Random();
            for (int i = 1; i <= rows; i++)
            {
                // Генеруємо суму від 10 до 5000
                decimal amount = (decimal)(rnd.NextDouble() * 4990) + 10;

                // Записуємо рядок у файл
                string line = $"{i},2023-10-15,{amount.ToString("F2", CultureInfo.InvariantCulture)}";
                await writer.WriteLineAsync(line);
            }
        }

        static async Task<(decimal TotalSum, int FilteredCount)> ProcessSalesAsync(string input, string output)
        {
            decimal totalSum = 0;
            int filteredCount = 0;

            using var reader = new StreamReader(input);
            using var writer = new StreamWriter(output);

            // Читаємо і записуємо заголовок
            string header = await reader.ReadLineAsync();
            if (header != null) await writer.WriteLineAsync(header);

            string line;
            // Читаємо ПО РЯДКУ (файл не завантажується в пам'ять цілком)
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length == 3 && decimal.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                {
                    totalSum += amount; // Агрегуємо статистику

                    // Фільтруємо (наприклад, шукаємо продажі > 1000) і одразу записуємо результат
                    if (amount > 1000m)
                    {
                        await writer.WriteLineAsync(line);
                        filteredCount++;
                    }
                }
            }

            return (totalSum, filteredCount);
        }

        static (decimal TotalSum, int FilteredCount) ProcessSalesSync(string input, string output)
        {
            // Цей метод робить те саме, але синхронно (блокуючи потік)
            decimal totalSum = 0;
            int filteredCount = 0;

            using var reader = new StreamReader(input);
            using var writer = new StreamWriter(output);

            string header = reader.ReadLine();
            if (header != null) writer.WriteLine(header);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length == 3 && decimal.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                {
                    totalSum += amount;

                    if (amount > 1000m)
                    {
                        writer.WriteLine(line);
                        filteredCount++;
                    }
                }
            }

            return (totalSum, filteredCount);
        }
    }
}