using System.Collections.Generic;
using System;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = new Dictionary<int, Order>();

    public void Save(Order order)
    {
        _orders[order.Id] = order;
        Console.WriteLine($"Order {order.Id} saved.");
    }

    public Order GetById(int id)
    {
        return _orders.ContainsKey(id) ? _orders[id] : null;
    }
}
