public interface IOrderRepository
{
    void Save(Order order);
    Order GetById(int id);
}
