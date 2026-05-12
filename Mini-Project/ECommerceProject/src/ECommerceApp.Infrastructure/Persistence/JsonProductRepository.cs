using System.Text.Json;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Infrastructure.Persistence
{
    public class JsonProductRepository : IProductRepository
    {
        private readonly string _filePath;
        private List<Product> _products;

        public JsonProductRepository(string filePath)
        {
            _filePath = filePath;
            // якщо файлу немаЇ, створюЇмо пустий список, €кщо Ї Ч читаЇмо
            if (!File.Exists(_filePath))
            {
                _products = new List<Product>();
            }
            else
            {
                var json = File.ReadAllText(_filePath);
                _products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
        }

        public IEnumerable<Product> GetAll() => _products;

        public Product GetById(Guid id) => _products.FirstOrDefault(p => p.Id == id);

        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
        public async Task SaveAsync()
        {
            var json = JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task LoadAsync()
        {
            if (!File.Exists(_filePath)) return;
            var json = await File.ReadAllTextAsync(_filePath);
            _products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
    }
}