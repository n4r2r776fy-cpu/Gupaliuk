using System;
using System.Threading.Tasks;

// ===============================
// Приклади дотримання SRP
// ===============================

// Контролер (дотримання SRP)
public class ProductController
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
            return new NotFoundResult();

        return new OkObjectResult(product);
    }
}

// Сервіс (дотримання SRP)
public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<ProductDto> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }
}

// ===============================
// Приклади порушення SRP
// ===============================

// "God Object" клас (порушення SRP)
public class OrderManager
{
    public void Create(Order order)
    {
        ValidateOrder(order);
        ProcessPayment(order);
        SaveOrderToDatabase(order);
        SendConfirmationEmail(order);
    }

    private void ValidateOrder(Order order) { /* … */ }
    private void ProcessPayment(Order order) { /* … */ }
    private void SaveOrderToDatabase(Order order) { /* … */ }
    private void SendConfirmationEmail(Order order) { /* … */ }
}

// ===============================
// Приклади дотримання OCP
// ===============================

public interface IPaymentProcessor
{
    void ProcessPayment(Payment payment);
}

public class StripeProcessor : IPaymentProcessor
{
    public void ProcessPayment(Payment payment)
    {
        // Логіка Stripe
    }
}

public class PaypalProcessor : IPaymentProcessor
{
    public void ProcessPayment(Payment payment)
    {
        // Логіка PayPal
    }
}

// ===============================
// Приклади порушення OCP
// ===============================

public enum CustomerType { Regular, Vip, Employee }

public class DiscountCalculator
{
    public decimal Calculate(Order order)
    {
        if (order.CustomerType == CustomerType.Regular)
            return order.Total * 0.05M;
        else if (order.CustomerType == CustomerType.Vip)
            return order.Total * 0.15M;
        else if (order.CustomerType == CustomerType.Employee)
            return order.Total * 0.25M;

        return 0;
    }
}

// ===============================
// Допоміжні класи для демонстрації
// ===============================

public class Order
{
    public CustomerType CustomerType { get; set; }
    public decimal Total { get; set; }
}

public class Payment { }

public class ProductDto { }

public interface IProductRepository
{
    Task<ProductDto> GetByIdAsync(int id);
}

// Імітація результатів контролера
public class IActionResult { }
public class OkObjectResult : IActionResult
{
    public OkObjectResult(object value) { }
}
public class NotFoundResult : IActionResult { }
