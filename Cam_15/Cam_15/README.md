# SOLID: SRP и OCP (с примерами и анализом)

## 1. Принцип единой ответственности (SRP)

**SRP (Single Responsibility Principle)** — класс должен иметь только одну причину для изменения, то есть выполнять одну задачу.

---

### 1.1 Нарушение SRP

**Класс: ProductController**

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