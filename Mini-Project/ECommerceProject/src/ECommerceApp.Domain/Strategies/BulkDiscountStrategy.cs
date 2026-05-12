namespace ECommerceApp.Domain.Strategies
{
    public class BulkDiscountStrategy : IDiscountStrategy
    {
        public decimal Apply(decimal total) => total > 1000 ? total * 0.9m : total;
    }
}