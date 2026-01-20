interface IProductValidator
{
    bool Validate(string name, decimal price);
}

interface IProductRepository
{
    void Save(string name, decimal price, int stock);
}

interface ILogger
{
    void Log(string message);
}

interface IStockNotifier
{
    void NotifyLowStock(string name, int stock);
}
