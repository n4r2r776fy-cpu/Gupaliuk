using System;

class ProductValidator : IProductValidator
{
    public bool Validate(string name, decimal price)
    {
        return !string.IsNullOrEmpty(name) && price > 0;
    }
}

class ProductRepository : IProductRepository
{
    public void Save(string name, decimal price, int stock)
    {
        Console.WriteLine($"Product '{name}' saved to database.");
    }
}

class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"LOG: {message}");
    }
}

class StockNotifier : IStockNotifier
{
    public void NotifyLowStock(string name, int stock)
    {
        if (stock < 5)
        {
            Console.WriteLine($"⚠ Low stock for product '{name}'");
        }
    }
}
