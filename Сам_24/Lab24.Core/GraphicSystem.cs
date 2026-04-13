using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab24.Core
{
    // Спільний інтерфейс (Component)
    public interface IGraphic
    {
        string Render();
    }

    // ==========================================
    // 1. COMPOSITE (Листок та Група)
    // ==========================================
    public class Shape : IGraphic
    {
        private readonly string _name;

        public Shape(string name)
        {
            _name = name;
        }

        public string Render()
        {
            // Імітація "важкого" процесу малювання (10 мілісекунд на кожну фігуру)
            Thread.Sleep(10);
            return $"Фігура: {_name}";
        }
    }

    public class GraphicGroup : IGraphic
    {
        private readonly List<IGraphic> _children = new List<IGraphic>();

        public void Add(IGraphic component) => _children.Add(component);

        public string Render()
        {
            var results = _children.Select(c => c.Render());
            return $"<Група>\n  {string.Join("\n  ", results)}\n</Група>";
        }
    }

    // ==========================================
    // 2. DECORATOR (Декоратор)
    // ==========================================
    public class ColorDecorator : IGraphic
    {
        private readonly IGraphic _component;
        private readonly string _color;

        public ColorDecorator(IGraphic component, string color)
        {
            _component = component ?? throw new ArgumentNullException(nameof(component));
            _color = color;
        }

        public string Render()
        {
            return $"[Колір: {_color}] {_component.Render()}";
        }
    }

    // ==========================================
    // 3. PROXY (Кешуючий заступник)
    // ==========================================
    public class CachingGraphicProxy : IGraphic
    {
        private readonly IGraphic _realSubject;
        private string _cachedRender;

        public CachingGraphicProxy(IGraphic realSubject)
        {
            // Негативний кейс: не можна створити проксі для порожнього об'єкта
            _realSubject = realSubject ?? throw new ArgumentNullException(nameof(realSubject), "Реальний об'єкт не може бути null");
        }

        public string Render()
        {
            // Якщо кеш порожній - малюємо по-справжньому і зберігаємо
            if (_cachedRender == null)
            {
                _cachedRender = _realSubject.Render();
            }

            return _cachedRender;
        }

        // Метод для примусового очищення кешу (якщо фігура змінилася)
        public void ClearCache()
        {
            _cachedRender = null;
        }
    }
}