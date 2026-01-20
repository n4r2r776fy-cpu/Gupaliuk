using System;

class ProductManager
{
    public void AddProduct(string name, decimal price, int stock)
    {
        Console.WriteLine("Validating product...");
        if (string.IsNullOrEmpty(name) || price <= 0)
        {
            Console.WriteLine("Invalid product data");
            return;
        }

        Console.WriteLine("Saving product to database...");
        Console.WriteLine("Logging product creation...");

        if (stock < 5)
        {
            Console.WriteLine("Warning: Low stock notification sent!");
        }
    }

    public void UpdatePrice(string name, decimal newPrice)
    {
        Console.WriteLine("Updating price...");
        Console.WriteLine("Logging price update...");
    }
}
