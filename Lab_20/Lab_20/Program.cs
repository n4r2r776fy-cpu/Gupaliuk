using System;

class Program
{
    static void Main()
    {
        IOrderValidator validator = new OrderValidator();
        IOrderRepository repository = new InMemoryOrderRepository();
        IEmailService emailService = new ConsoleEmailService();

        OrderService orderService = new OrderService(
            validator,
            repository,
            emailService
        );

        // Валідне замовлення
        Order validOrder = new Order(1, "Ivan Petrenko", 1500);
        orderService.ProcessOrder(validOrder);
        Console.WriteLine($"Final status: {validOrder.Status}\n");

        // Невалідне замовлення
        Order invalidOrder = new Order(2, "Oleh Shevchenko", -200);
        orderService.ProcessOrder(invalidOrder);
        Console.WriteLine($"Final status: {invalidOrder.Status}");

        Console.WriteLine("Натисніть любу кнопку для завершення роболти:");
        Console.ReadLine();
    }
}
