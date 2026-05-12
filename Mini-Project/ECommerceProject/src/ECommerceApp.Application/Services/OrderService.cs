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

        // Приклад LINQ: Пошук дорогих товарів, що є в наявності
        public IEnumerable<Product> GetPremiumAvailableProducts(decimal minPrice)
        {
            return _productRepo.GetAll()
                .Where(p => p.Price >= minPrice && p.StockQuantity > 0)
                .OrderByDescending(p => p.Price);
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
            _productRepo.SaveChanges();
            _orderRepo.SaveChanges();
        }
        // Метод показує топ-3 найдорожчих замовлень клієнта (Демонстрація LINQ)
        public IEnumerable<Order> GetTopExpensiveOrders(string customerName)
        {
            return _orderRepo.GetAll()
                .Where(o => o.CustomerName == customerName)
                .OrderByDescending(o => o.TotalAmount)
                .Take(3);
        }
    }
}