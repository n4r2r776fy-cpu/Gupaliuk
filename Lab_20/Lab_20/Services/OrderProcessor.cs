using System;

public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        if (order.TotalAmount <= 0)
        {
            order.Status = OrderStatus.Cancelled;
            return;
        }

        Console.WriteLine("Saving order...");
        Console.WriteLine("Sending email...");
        order.Status = OrderStatus.Processed;
    }
}
