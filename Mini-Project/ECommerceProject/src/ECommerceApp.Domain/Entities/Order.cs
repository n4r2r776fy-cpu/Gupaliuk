using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceApp.Domain.Entities
{
    public enum OrderStatus { New, Paid, Shipped, Cancelled }

    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal DiscountAmount { get; private set; }

        public Order(string customerName)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
            Status = OrderStatus.New;
            DiscountAmount = 0;
        }

        public void AddItem(Product product, int quantity)
        {
            if (Status != OrderStatus.New) throw new InvalidOperationException("Можна змінювати тільки нові замовлення.");

            _items.Add(new OrderItem(product, quantity));
        }

        public void ApplyDiscount(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Знижка має бути від 0 до 100.");

            DiscountAmount = SubTotal * (discountPercentage / 100m);
        }

        public decimal SubTotal => _items.Sum(i => i.TotalPrice);
        public decimal TotalAmount => SubTotal - DiscountAmount;

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.New) throw new InvalidOperationException("Замовлення не може бути оплачене в поточному статусі.");
            if (!_items.Any()) throw new InvalidOperationException("Не можна оплатити порожнє замовлення.");

            Status = OrderStatus.Paid;
        }
    }
}