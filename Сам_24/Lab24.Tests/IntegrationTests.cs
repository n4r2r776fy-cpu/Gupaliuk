using System;
using System.Diagnostics;
using Xunit;
using Lab24.Core;

namespace Lab24.Tests
{
    public class IntegrationTests
    {
        // ТЕСТ 1: Перевірка Composite
        [Fact]
        public void Composite_RendersAllChildrenCorrectly()
        {
            var group = new GraphicGroup();
            group.Add(new Shape("Коло"));
            group.Add(new Shape("Квадрат"));

            var result = group.Render();

            Assert.Contains("Фігура: Коло", result);
            Assert.Contains("Фігура: Квадрат", result);
            Assert.Contains("<Група>", result);
        }

        // ТЕСТ 2: Перевірка Composite + Decorator
        [Fact]
        public void Decorator_WrapsCompositeCorrectly()
        {
            var shape = new Shape("Трикутник");
            var coloredShape = new ColorDecorator(shape, "Червоний");

            var result = coloredShape.Render();

            Assert.StartsWith("[Колір: Червоний]", result);
            Assert.Contains("Фігура: Трикутник", result);
        }

        // ТЕСТ 3: Перевірка Proxy (Кешування працює швидше)
        [Fact]
        public void Proxy_CachesResult_AndImprovesPerformance()
        {
            // Arrange
            var complexGroup = new GraphicGroup();
            for (int i = 0; i < 5; i++) complexGroup.Add(new Shape($"Точка {i}"));

            var proxy = new CachingGraphicProxy(complexGroup);

            // Act 1: Перший рендер (повинен зайняти > 50 мс через Thread.Sleep)
            var sw = Stopwatch.StartNew();
            var firstResult = proxy.Render();
            sw.Stop();
            var firstTime = sw.ElapsedMilliseconds;

            // Act 2: Другий рендер (з кешу, має бути миттєвим, ~0 мс)
            sw.Restart();
            var secondResult = proxy.Render();
            sw.Stop();
            var secondTime = sw.ElapsedMilliseconds;

            // Assert
            Assert.Equal(firstResult, secondResult); // Результат однаковий
            Assert.True(firstTime > 30); // Перший раз довго
            Assert.True(secondTime < 5); // Другий раз дуже швидко (кеш)
        }

        // ТЕСТ 4: Негативний / Граничний сценарій
        [Fact]
        public void Proxy_NullSubject_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CachingGraphicProxy(null));
        }
    }
}