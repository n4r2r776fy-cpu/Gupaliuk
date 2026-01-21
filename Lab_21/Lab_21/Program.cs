using System;

namespace lab21
{
    // ===== Strategy interface =====
    public interface IShippingStrategy
    {
        decimal CalculateCost(decimal distance, decimal weight);
    }

    // ===== Concrete strategies =====
    public class StandardShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            return distance * 1.5m + weight * 0.5m;
        }
    }

    public class ExpressShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            return distance * 2.5m + weight * 1.0m + 50m;
        }
    }

    public class InternationalShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            decimal baseCost = distance * 5.0m + weight * 2.0m;
            decimal tax = baseCost * 0.15m;
            return baseCost + tax;
        }
    }

    // ===== OCP extension =====
    public class NightShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            decimal standardCost = distance * 1.5m + weight * 0.5m;
            return standardCost + 30m;
        }
    }

    // ===== Factory Method =====
    public static class ShippingStrategyFactory
    {
        public static IShippingStrategy CreateStrategy(string deliveryType)
        {
            switch (deliveryType.ToLower())
            {
                case "standard":
                    return new StandardShippingStrategy();

                case "express":
                    return new ExpressShippingStrategy();

                case "international":
                    return new InternationalShippingStrategy();

                case "night":
                    return new NightShippingStrategy();

                default:
                    throw new ArgumentException("Невідомий тип доставки");
            }
        }
    }

    // ===== Context =====
    public class DeliveryService
    {
        public decimal CalculateDeliveryCost(
            decimal distance,
            decimal weight,
            IShippingStrategy strategy)
        {
            return strategy.CalculateCost(distance, weight);
        }
    }

    // ===== Program =====
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Типи доставки: Standard, Express, International, Night");

            Console.Write("Введіть тип доставки: ");
            string type = Console.ReadLine();

            Console.Write("Введіть відстань (км): ");
            decimal distance = decimal.Parse(Console.ReadLine());

            Console.Write("Введіть вагу (кг): ");
            decimal weight = decimal.Parse(Console.ReadLine());

            try
            {
                IShippingStrategy strategy =
                    ShippingStrategyFactory.CreateStrategy(type);

                DeliveryService service = new DeliveryService();
                decimal cost =
                    service.CalculateDeliveryCost(distance, weight, strategy);

                Console.WriteLine("Вартість доставки: " + cost + " грн");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
