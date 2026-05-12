using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(Guid id);
        void Update(Product product);
        void SaveChanges();
    }
}