# Звіт з аналізу SOLID принципів (SRP, OCP) в Open-Source проєкті

---

## 1. Аналіз SRP (Single Responsibility Principle)

### 2.1. Приклади дотримання SRP

#### Клас: ProductController
- **Відповідальність:** Обробка HTTP-запитів для продуктів.
- **Обґрунтування:** Контролер займається лише маршрутизацією та викликом сервісу.
```csharp
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
```

#### Клас: ProductService
- **Відповідальність:** Логіка бізнес-операцій над продуктами.
- **Обґрунтування:** Реалізує лише логіку роботи з продуктами, не займаючись контролерами чи іншими функціями.
```csharp
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
```

### 2.2. Приклади порушення SRP

#### Клас: OrderManager
- **Множинні відповідальності:** Валідація, оплата, збереження у базу, email-повідомлення.
- **Проблеми:** Важко тестувати, підтримувати, розширювати.
```csharp
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
```

---

## 3. Аналіз OCP (Open/Closed Principle)

### 3.1. Приклади дотримання OCP

#### Стратегія оплати (Strategy Pattern)
- **Механізм розширення:** Інтерфейси `IPaymentProcessor` для різних способів оплати.
- **Обґрунтування:** Нові способи додаються без зміни існуючого коду.
```csharp
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
```

### 3.2. Приклади порушення OCP

#### Клас: DiscountCalculator
- **Проблема:** Додавання нової категорії потребує зміни коду.
- **Наслідки:** Знижує підтримуваність та розширюваність.
```csharp
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
```

---

## 4. Допоміжні класи для демонстрації
```csharp
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