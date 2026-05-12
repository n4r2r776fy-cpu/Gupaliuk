using System;

namespace ECommerceApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================");
            Console.WriteLine("   ПРОЄКТ ЗАПУЩЕНО УСПІШНО! ");
            Console.WriteLine("========================================");
            Console.WriteLine("\nЦе консоль вашого інтернет-магазину.");
            Console.WriteLine("Натисніть будь-яку клавішу, щоб вийти...");
            Console.ReadKey();
        }
    }
}