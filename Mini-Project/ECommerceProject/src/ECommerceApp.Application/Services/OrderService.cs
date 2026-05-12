using System;
using System.Collections.Generic;
using System.Linq; // Перенесли сюди
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Services
{
    public class OrderService
    {
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;

        public OrderService(IProductRepository productRepo, IOrderRepository orderRepo)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        // 1. Пошук преміальних товарів
        public IEnumerable<Product> GetPremiumAvailableProducts(decimal minPrice)
        {
            return _productRepo.GetAll()
                .Where(p => p.Price >= minPrice && p.StockQuantity > 0)
                .OrderByDescending(p => p.Price);
        }

        // 2. Товари, що закінчуються (менше 5 шт)
        public IEnumerable<Product> GetLowStock() => 
            _productRepo.GetAll().Where(p => p.StockQuantity < 5);

        // 3. Сортування за ціною (від дорогих)
        public IEnumerable<Product> GetByPrice() => 
            _productRepo.GetAll().OrderByDescending(p => p.Price);

        // 4. Загальна вартість всього складу (Агрегація)
        public decimal GetTotalValue() => 
            _productRepo.GetAll().Sum(p => p.Price * p.StockQuantity);

        // 5. Пошук товару за назвою
        public Product? FindByName(string name) => 
            _productRepo.GetAll().FirstOrDefault(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        // 6. Топ-3 найдорожчих замовлень клієнта
        public IEnumerable<Order> GetTopExpensiveOrders(string customerName)
        {
            return _orderRepo.GetAll()
                .Where(o => o.CustomerName == customerName)
                .OrderByDescending(o => o.TotalAmount)
                .Take(3);
        }

        public void PlaceOrder(string customerName, Dictionary<Guid, int> items)
        {
            var order = new Order(customerName);
            foreach (var item in items)
            {
                var product = _productRepo.GetById(item.Key);
                if (product != null)
                {
                    product.ReduceStock(item.Value);
                    order.AddItem(product, item.Value);
                }
            }
            _orderRepo.Add(order);
            
            // Зберігаємо зміни (якщо методи SaveChanges є в інтерфейсах)
            _productRepo.SaveChanges();
            _orderRepo.SaveChanges();
        }
    }
}