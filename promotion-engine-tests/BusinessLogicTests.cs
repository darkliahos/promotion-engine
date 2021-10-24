using promotion_engine;
using System;
using Xunit;

namespace promotion_engine_tests
{
    public class BusinessLogicTests
    {
        [Fact]
        public void ScenarioA_Totals100()
        {
            // Arrange
            var checkout = new Checkout();
            checkout.AddSKUToCart("A", 1);
            checkout.AddSKUToCart("B", 1);
            checkout.AddSKUToCart("C", 1);


            // Act
            var result = checkout.CalculateTotal();

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public void ScenarioB_Totals370()
        {
            // Arrange
            var checkout = new Checkout();
            checkout.AddSKUToCart("A", 5);
            checkout.AddSKUToCart("B", 5);
            checkout.AddSKUToCart("C", 1);


            // Act
            var result = checkout.CalculateTotal();

            // Assert
            Assert.Equal(370, result);
        }

        [Fact]
        public void ScenarioC_Totals280()
        {
            // Arrange
            var checkout = new Checkout();
            checkout.AddSKUToCart("A", 3);
            checkout.AddSKUToCart("B", 5);
            checkout.AddSKUToCart("C", 1);
            checkout.AddSKUToCart("D", 1);


            // Act
            var result = checkout.CalculateTotal();

            // Assert
            Assert.Equal(280, result);
        }

    }
}
