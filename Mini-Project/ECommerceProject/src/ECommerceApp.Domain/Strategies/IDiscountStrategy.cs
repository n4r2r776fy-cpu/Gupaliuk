namespace ECommerceApp.Domain.Strategies
{
    public interface IDiscountStrategy
    {
        decimal Apply(decimal total);
    }
}