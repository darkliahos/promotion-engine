using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotion_engine
{
    public class Checkout : ICheckout
    {
        public void AddSKUToCart(string skuName, int quantity)
        {
            throw new NotImplementedException();
        }

        public int CalculateTotal()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICheckout
    {
        void AddSKUToCart(string skuName, int quantity);

        int CalculateTotal();
    }
}
