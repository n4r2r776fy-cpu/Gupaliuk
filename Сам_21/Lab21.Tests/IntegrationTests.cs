using System;
using Xunit;
using Lab21.Core;

namespace Lab21.Tests
{
    public class IntegrationTests : IDisposable
    {
        public IntegrationTests()
        {
            // Скидаємо Singleton перед кожним тестом для чистоти експерименту
            ProcessingHub.ResetForTesting();
        }

        public void Dispose()
        {
            ProcessingHub.ResetForTesting();
        }

        // ==========================================
        // ПОЗИТИВНІ СЦЕНАРІЇ (3 шт)
        // ==========================================

        [Fact]
        public void Scenario1_FullSystemIntegration_WorksCorrectly()
        {
            // Arrange
            var hub = ProcessingHub.Instance;
            hub.CurrentStrategy = new HalfPriceStrategy(); // Strategy
            hub.CurrentNotification = NotificationFactory.Create("sms"); // Factory

            decimal receivedAmount = 0;
            hub.OnOrderProcessed += (amount) => receivedAmount = amount; // Observer підписка

            // Act
            decimal result = hub.ProcessOrder(100m);

            // Assert
            Assert.Equal(50m, result); // Strategy відпрацювала
            Assert.Equal(50m, receivedAmount); // Observer відпрацював
        }

        [Fact]
        public void Scenario2_SingletonState_IsPreservedAcrossCalls()
        {
            // Arrange
            var hub1 = ProcessingHub.Instance;
            var hub2 = ProcessingHub.Instance;

            // Act
            hub1.CurrentStrategy = new HalfPriceStrategy();

            // Assert
            Assert.Same(hub1, hub2); // Це той самий об'єкт
            Assert.IsType<HalfPriceStrategy>(hub2.CurrentStrategy); // Зміна в hub1 вплинула на hub2
        }

        [Fact]
        public void Scenario3_StrategyChangeAtRuntime_ChangesOutcome()
        {
            // Arrange
            var hub = ProcessingHub.Instance;

            // Act 1
            hub.CurrentStrategy = new NoDiscountStrategy();
            decimal result1 = hub.ProcessOrder(100m);

            // Act 2 (зміна в runtime)
            hub.CurrentStrategy = new HalfPriceStrategy();
            decimal result2 = hub.ProcessOrder(100m);

            // Assert
            Assert.Equal(100m, result1);
            Assert.Equal(50m, result2);
        }

        // ==========================================
        // НЕГАТИВНІ/ГРАНИЧНІ СЦЕНАРІЇ (2 шт)
        // ==========================================

        [Fact]
        public void Scenario4_MissingDependencies_ThrowsInvalidOperationException()
        {
            // Arrange
            var hub = ProcessingHub.Instance;
            hub.CurrentStrategy = null; // Штучно ламаємо стан

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => hub.ProcessOrder(100m));
            Assert.Equal("Strategy is missing.", ex.Message);
        }

        [Fact]
        public void Scenario5_UnsubscribedObserver_DoesNotReceiveNotifications()
        {
            // Arrange
            var hub = ProcessingHub.Instance;
            decimal receivedAmount = 0;

            Action<decimal> handler = (amount) => receivedAmount = amount;
            hub.OnOrderProcessed += handler; // Підписались
            hub.OnOrderProcessed -= handler; // Одразу відписались

            // Act
            hub.ProcessOrder(100m);

            // Assert
            Assert.Equal(0, receivedAmount); // Значення не змінилося, бо ми відписались
        }
    }
}