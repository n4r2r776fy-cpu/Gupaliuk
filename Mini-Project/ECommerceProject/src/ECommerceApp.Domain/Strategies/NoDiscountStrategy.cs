namespace ECommerceApp.Domain.Strategies
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal Apply(decimal total) => total;
    }
}