using System;
using System.Collections.Generic;

namespace IndependentWork22
{
    // ==========================================
    // 1. СПІЛЬНИЙ ІНТЕРФЕЙС (IComponent)
    // ==========================================
    public interface IComponent
    {
        void Draw();
    }

    // ==========================================
    // 2. ПАТЕРН COMPOSITE: Leaf (Окремі елементи)
    // ==========================================
    public class Point : IComponent
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) { X = x; Y = y; }

        public void Draw()
        {
            Console.WriteLine($"Малюємо Точку з координатами ({X}, {Y})");
        }
    }

    public class Line : IComponent
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }

        public Line(int startX, int startY, int endX, int endY)
        {
            StartX = startX; StartY = startY; EndX = endX; EndY = endY;
        }

        public void Draw()
        {
            Console.WriteLine($"Малюємо Лінію від ({StartX}, {StartY}) до ({EndX}, {EndY})");
        }
    }

    // ==========================================
    // 3. ПАТЕРН COMPOSITE: Composite (Група)
    // ==========================================
    public class GraphicGroup : IComponent
    {
        private List<IComponent> _children = new List<IComponent>();

        public void Add(IComponent component)
        {
            _children.Add(component);
        }

        public void Remove(IComponent component)
        {
            _children.Remove(component);
        }

        public void Draw()
        {
            Console.WriteLine("--- Початок малювання групи ---");
            foreach (var child in _children)
            {
                child.Draw();
            }
            Console.WriteLine("--- Кінець малювання групи ---");
        }
    }

    // ==========================================
    // 4. ПАТЕРН DECORATOR: Базовий декоратор
    // ==========================================
    public abstract class GraphicDecorator : IComponent
    {
        protected IComponent _component;

        public GraphicDecorator(IComponent component)
        {
            _component = component;
        }

        public virtual void Draw()
        {
            if (_component != null)
            {
                _component.Draw();
            }
        }
    }

    // ==========================================
    // 5. ПАТЕРН DECORATOR: Конкретні декоратори
    // ==========================================
    public class ColorDecorator : GraphicDecorator
    {
        private string _color;

        public ColorDecorator(IComponent component, string color) : base(component)
        {
            _color = color;
        }

        public override void Draw()
        {
            Console.Write($"[Колір: {_color}] ");
            base.Draw();
        }
    }

    public class BorderDecorator : GraphicDecorator
    {
        private string _borderStyle;

        public BorderDecorator(IComponent component, string borderStyle) : base(component)
        {
            _borderStyle = borderStyle;
        }

        public override void Draw()
        {
            Console.Write($"[Рамка: {_borderStyle}] ");
            base.Draw();
        }
    }

    // ==========================================
    // 6. ДЕМОНСТРАЦІЯ (Main)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Патерни Composite та Decorator ===\n");

            // 1. Створюємо базові елементи
            IComponent point1 = new Point(10, 20);
            IComponent line1 = new Line(0, 0, 100, 100);
            IComponent point2 = new Point(50, 50);

            // 2. Декоруємо їх
            IComponent redPoint = new ColorDecorator(point1, "Червоний");
            // Можемо накладати декоратори один на одний (матрьошка)
            IComponent fancyLine = new BorderDecorator(new ColorDecorator(line1, "Синій"), "Пунктирна");

            // 3. Створюємо підгрупу і додаємо туди елементи
            GraphicGroup subGroup = new GraphicGroup();
            subGroup.Add(redPoint);
            subGroup.Add(fancyLine);

            // 4. Створюємо головну групу і ДЕКОРУЄМО її всю!
            GraphicGroup mainGroup = new GraphicGroup();
            mainGroup.Add(point2);   // звичайна точка без декорацій
            mainGroup.Add(subGroup); // додаємо підгрупу

            IComponent borderedMainGroup = new BorderDecorator(mainGroup, "Суцільна товста");

            // 5. Виклик єдиного методу Draw() запустить весь ланцюжок
            Console.WriteLine("Вивід загальної складної структури:\n");
            borderedMainGroup.Draw();

            Console.ReadLine();
        }
    }
}