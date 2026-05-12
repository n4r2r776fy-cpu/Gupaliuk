using System;
using System.Collections.Generic;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Strategies;
using Xunit;

namespace ECommerceApp.Tests
{
    public class FinalTests
    {
        // === ÒÅÑÒÈ ÑÓÒÍÎÑÒÅÉ (Domain) ===
        [Fact]
        public void Product_Price_ShouldBePositive() =>
            Assert.True(new Product("Òîâàð", 100m, 10).Price > 0);

        [Fact]
        public void Product_ReduceStock_Works()
        {
            var p = new Product("A", 10m, 5);
            p.ReduceStock(2);
            Assert.Equal(3, p.StockQuantity);
        }

        [Fact]
        public void Order_InitialStatus_IsNew() =>
            Assert.Equal(OrderStatus.New, new Order("Êë³ºíò").Status);

        // === ÒÅÑÒÈ ÑÒÐÀÒÅÃ²É ÇÍÈÆÎÊ (Pattern Strategy) ===
        [Fact]
        public void NoDiscount_DoesNothing() =>
            Assert.Equal(100, new NoDiscountStrategy().Apply(100));

        [Fact]
        public void BulkDiscount_AppliesOver1000() =>
            Assert.Equal(900, new BulkDiscountStrategy().Apply(1000.01m));

        [Fact]
        public void BulkDiscount_NoApplyUnder1000() =>
            Assert.Equal(500, new BulkDiscountStrategy().Apply(500));

        // === ÒÅÑÒÈ Á²ÇÍÅÑ-ÏÐÀÂÈË ===
        [Fact]
        public void Cannot_ReduceStock_BelowZero() =>
            Assert.Throws<InvalidOperationException>(() => new Product("A", 10m, 1).ReduceStock(5));

        [Fact]
        public void Order_Cannot_BeEmpty_OnPay() =>
            Assert.Throws<InvalidOperationException>(() => new Order("A").MarkAsPaid());

        [Fact]
        public void Product_Name_CannotBeEmpty() =>
            Assert.Throws<ArgumentException>(() => new Product("", 10m, 1));

        // === ÒÅÑÒÈ ËÎÃ²ÊÈ ÒÀ LINQ ===
        [Fact]
        public void TotalAmount_CalculatesCorrectly()
        {
            var o = new Order("A");
            o.AddItem(new Product("A", 100m, 10), 1);
            Assert.Equal(100, o.TotalAmount);
        }

        [Fact]
        public void OrderItem_Quantity_MustBePositive() =>
            Assert.Throws<ArgumentException>(() => new OrderItem(new Product("A", 10m, 1), 0));

        [Fact]
        public void Order_Status_Transition_Works()
        {
            var o = new Order("A");
            o.AddItem(new Product("A", 10m, 10), 1);
            o.MarkAsPaid();
            Assert.Equal(OrderStatus.Paid, o.Status);
        }
    }
}