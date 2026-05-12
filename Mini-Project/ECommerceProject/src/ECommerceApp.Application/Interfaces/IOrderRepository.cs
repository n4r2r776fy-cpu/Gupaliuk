using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        IEnumerable<Order> GetAll();
        void SaveChanges();
    }
}