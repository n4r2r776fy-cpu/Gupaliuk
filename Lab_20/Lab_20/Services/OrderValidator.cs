public class OrderValidator : IOrderValidator
{
    public bool IsValid(Order order)
    {
        return order.TotalAmount > 0;
    }
}
