using System;

namespace ECommerceApp.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        public Product(string name, decimal price, int stockQuantity)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Назва не може бути порожньою.");
            if (price <= 0) throw new ArgumentException("Ціна має бути більшою за нуль.");
            if (stockQuantity < 0) throw new ArgumentException("Кількість не може бути від'ємною.");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Кількість для списання має бути > 0.");
            if (StockQuantity < quantity) throw new InvalidOperationException($"Недостатньо товару '{Name}' на складі.");

            StockQuantity -= quantity;
        }
    }
}