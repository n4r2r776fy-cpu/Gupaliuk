using System;


namespace Lab23_ISP_DIP
{

    // "Товстий" інтерфейс
    public interface IMachine
    {
        void Print();
        void Scan();
        void Fax();
    }

    // Конкретні модулі
    public class PrinterModule
    {
        public void Print() => Console.WriteLine("Printing document...");
    }

    public class ScannerModule
    {
        public void Scan() => Console.WriteLine("Scanning document...");
    }

    public class FaxModule
    {
        public void Fax() => Console.WriteLine("Sending fax...");
    }

    // Клас, що ПОРУШУЄ ISP і DIP
    public class SmartMachine : IMachine
    {
        // ЖОРСТКЕ створення залежностей → порушення DIP
        private readonly PrinterModule _printer = new PrinterModule();
        private readonly ScannerModule _scanner = new ScannerModule();
        private readonly FaxModule _fax = new FaxModule();

        public void Print() => _printer.Print();
        public void Scan() => _scanner.Scan();
        public void Fax() => _fax.Fax();
    }


    public interface IPrinter
    {
        void Print();
    }

    public interface IScanner
    {
        void Scan();
    }

    public interface IFax
    {
        void Fax();
    }

    // Конкретні реалізації
    public class BasicPrinter : IPrinter
    {
        public void Print() => Console.WriteLine("Basic printer: printing...");
    }

    public class BasicScanner : IScanner
    {
        public void Scan() => Console.WriteLine("Basic scanner: scanning...");
    }

    public class BasicFax : IFax
    {
        public void Fax() => Console.WriteLine("Basic fax: sending...");
    }



    public class SmartMachineRefactored
    {
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;
        private readonly IFax _fax;

        // DI через конструктор → дотримання DIP
        public SmartMachineRefactored(IPrinter printer, IScanner scanner, IFax fax)
        {
            _printer = printer;
            _scanner = scanner;
            _fax = fax;
        }

        public void Print() => _printer.Print();
        public void Scan() => _scanner.Scan();
        public void Fax() => _fax.Fax();
    }



    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== BEFORE REFACTORING (ISP & DIP violation) ===\n");

            IMachine oldMachine = new SmartMachine();
            oldMachine.Print();
            oldMachine.Scan();
            oldMachine.Fax();


            Console.WriteLine("\n=== AFTER REFACTORING (ISP & DIP satisfied) ===\n");

            // Конфігурація залежностей (manual DI)
            IPrinter printer = new BasicPrinter();
            IScanner scanner = new BasicScanner();
            IFax fax = new BasicFax();

            var newMachine = new SmartMachineRefactored(printer, scanner, fax);

            newMachine.Print();
            newMachine.Scan();
            newMachine.Fax();

            Console.WriteLine("\nProgram finished successfully.");
        }
    }
}

