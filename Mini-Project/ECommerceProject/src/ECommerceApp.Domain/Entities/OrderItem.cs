namespace ECommerceApp.Domain.Entities
{
    public class OrderItem
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public OrderItem(Product product, int quantity)
        {
            Product = product ?? throw new System.ArgumentNullException(nameof(product));
            Quantity = quantity > 0 ? quantity : throw new System.ArgumentException("Quantity must be > 0");
            UnitPrice = product.Price;
        }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}