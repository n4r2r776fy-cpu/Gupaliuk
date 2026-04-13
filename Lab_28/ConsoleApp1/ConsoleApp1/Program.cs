using System;
using System.Threading.Tasks;

namespace lab28v2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string filename = "products.json";

            Console.WriteLine("=== 1. Створення об'єктів та збереження у JSON ===");

            var repoForSave = new ProductRepository();

            var categoryElectronics = new Category { Id = 1, Name = "Електроніка" };
            var categoryClothing = new Category { Id = 2, Name = "Одяг" };

            repoForSave.Add(new Product { Id = 101, Name = "Ноутбук Dell", Price = 25000m, Category = categoryElectronics });
            repoForSave.Add(new Product { Id = 102, Name = "Смартфон Samsung", Price = 15000m, Category = categoryElectronics });
            repoForSave.Add(new Product { Id = 103, Name = "Футболка чорна", Price = 500m, Category = categoryClothing });

            // Зберігаємо у файл
            await repoForSave.SaveToFileAsync(filename);
            Console.WriteLine($"Дані успішно збережено у файл: {filename}\n");

            Console.WriteLine("=== 2. Завантаження даних з JSON ===");

            // Створюємо новий репозиторій, щоб довести, що дані беруться саме з файлу, а не з пам'яті
            var repoForLoad = new ProductRepository();
            await repoForLoad.LoadFromFileAsync(filename);

            var loadedProducts = repoForLoad.GetAll();

            foreach (var product in loadedProducts)
            {
                Console.WriteLine($"[ID: {product.Id}] {product.Name} - {product.Price} грн (Категорія: {product.Category?.Name})");
            }

            Console.WriteLine("\n=== 3. Перевірка методу GetById ===");
            var singleProduct = repoForLoad.GetById(102);
            if (singleProduct != null)
            {
                Console.WriteLine($"Знайдено продукт з ID 102: {singleProduct.Name}");
            }
        }
    }
}