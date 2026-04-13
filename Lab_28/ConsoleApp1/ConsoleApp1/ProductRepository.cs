using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace lab28v2
{
    public class ProductRepository
    {
        private List<Product> _products = new List<Product>();

        // Опції для красивого форматування JSON (з відступами)
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void Add(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            _products.Add(product);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        // Асинхронне збереження у файл
        public async Task SaveToFileAsync(string filename)
        {
            // Відкриваємо потік для запису у файл
            using FileStream createStream = File.Create(filename);
            await JsonSerializer.SerializeAsync(createStream, _products, _jsonOptions);
        }

        // Асинхронне завантаження з файлу
        public async Task LoadFromFileAsync(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"Файл {filename} не знайдено. Створено порожній репозиторій.");
                _products = new List<Product>();
                return;
            }

            // Відкриваємо потік для читання з файлу
            using FileStream openStream = File.OpenRead(filename);
            _products = await JsonSerializer.DeserializeAsync<List<Product>>(openStream, _jsonOptions)
                        ?? new List<Product>();
        }
    }
}