using promotion_engine;
using System;

namespace promotion_engine_console
{
    class Program
    {
        private static Checkout checkout = new Checkout(new Warehouse(), new PromotionRuleEngine());
        static void Main(string[] args)
        {
            checkout.AddSKUToCart("A", 5);
            checkout.AddSKUToCart("B", 5);
            checkout.AddSKUToCart("C", 1);
            Console.WriteLine(checkout.CalculateTotal());
        }
    }
}
