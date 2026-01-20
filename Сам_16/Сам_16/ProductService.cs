using System;

class ProductService
{
    private readonly IProductValidator _validator;
    private readonly IProductRepository _repository;
    private readonly ILogger _logger;
    private readonly IStockNotifier _notifier;

    public ProductService(
        IProductValidator validator,
        IProductRepository repository,
        ILogger logger,
        IStockNotifier notifier)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
        _notifier = notifier;
    }

    public void AddProduct(string name, decimal price, int stock)
    {
        if (!_validator.Validate(name, price))
        {
            Console.WriteLine("Product validation failed.");
            return;
        }

        _repository.Save(name, price, stock);
        _logger.Log("Product added.");
        _notifier.NotifyLowStock(name, stock);
    }
}
