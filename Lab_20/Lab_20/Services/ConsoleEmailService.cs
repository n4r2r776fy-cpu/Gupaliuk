using System;

public class ConsoleEmailService : IEmailService
{
    public void SendOrderConfirmation(Order order)
    {
        Console.WriteLine($"Email sent to {order.CustomerName}");
    }
}
