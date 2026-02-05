using System;


namespace Lab22_LSP
{


    // Базовий клас
    public class Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("Bird is flying");
        }
    }

    // Похідний клас
    public class Penguin : Bird
    {
        public override void Fly()
        {
            // Пінгвіни не літають → порушення контракту базового класу
            throw new NotImplementedException("Penguins cannot fly!");
        }
    }

    // Клієнтський код, який очікує, що КОЖЕН Bird може літати
    public static class BirdClient
    {
        public static void MakeBirdFly(Bird bird)
        {
            Console.WriteLine("Trying to make bird fly...");
            bird.Fly(); // Тут виникає проблема з Penguin
        }
    }


    // Новий базовий клас — просто птах
    public abstract class BirdBase
    {
        public abstract void Move();
    }

    // Інтерфейс лише для тих, хто ВМІЄ літати
    public interface IFlyingBird
    {
        void Fly();
    }

    // Звичайний літаючий птах
    public class Sparrow : BirdBase, IFlyingBird
    {
        public override void Move()
        {
            Console.WriteLine("Sparrow hops on the ground");
        }

        public void Fly()
        {
            Console.WriteLine("Sparrow is flying");
        }
    }

    // Пінгвін — НЕ літає, але все ще коректний птах
    public class PenguinFixed : BirdBase
    {
        public override void Move()
        {
            Console.WriteLine("Penguin is swimming");
        }
    }

    // Новий клієнт працює лише з тими, хто реально літає
    public static class FlyingBirdClient
    {
        public static void MakeBirdFly(IFlyingBird bird)
        {
            Console.WriteLine("Flying bird client call:");
            bird.Fly(); // БЕЗПЕЧНО — тут лише ті, хто може літати
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== DEMO: LSP VIOLATION ===\n");

            Bird normalBird = new Bird();
            Bird penguin = new Penguin();

            BirdClient.MakeBirdFly(normalBird);

            try
            {
                BirdClient.MakeBirdFly(penguin); // аварія
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }


            Console.WriteLine("\n=== DEMO: LSP FIX ===\n");

            Sparrow sparrow = new Sparrow();
            PenguinFixed fixedPenguin = new PenguinFixed();

            // Рух доступний для всіх птахів
            sparrow.Move();
            fixedPenguin.Move();

            // Літати можуть тільки літаючі
            FlyingBirdClient.MakeBirdFly(sparrow);

            Console.WriteLine("\nProgram finished successfully.");
        }
    }
}
