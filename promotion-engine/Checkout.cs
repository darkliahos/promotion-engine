using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotion_engine
{
    public class Checkout : ICheckout
    {
        // Could seperate this into another dependency
        private List<CartItem> SKUCart = new List<CartItem>();

        private readonly IWarehouse warehouse;

        public Checkout(IWarehouse warehouse)
        {
            this.warehouse = warehouse;
        }


        public void AddSKUToCart(string skuName, int quantity)
        {
            var sku = warehouse.GetSKUInformation(skuName);
            SKUCart.Add(new CartItem { SKU = sku, Quantity = quantity });
        }

        public int CalculateTotal()
        {
            int total = 0;

            foreach (var skuCartItem in SKUCart) 
            {
                total += skuCartItem.SKU.Price * skuCartItem.Quantity;
            }
            return total;
        }
    }

    public interface ICheckout
    {
        void AddSKUToCart(string skuName, int quantity);

        int CalculateTotal();
    }
}
