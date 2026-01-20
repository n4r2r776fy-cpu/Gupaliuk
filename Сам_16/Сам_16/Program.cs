
using System;

class Program
{
    static void Main()
    {
        IProductValidator validator = new ProductValidator();
        IProductRepository repository = new ProductRepository();
        ILogger logger = new ConsoleLogger();
        IStockNotifier notifier = new StockNotifier();

        ProductService service = new ProductService(
            validator,
            repository,
            logger,
            notifier
        );

        service.AddProduct("Laptop", 25000, 3);

        Console.WriteLine("Натисніть любу кнопку для завершення програми: ");
        Console.ReadLine();
    }
}
     