using promotion_engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotion_engine
{
    public class Warehouse : IWarehouse
    {
        // This could be a database or json file,
        // Might be worth adding this as a repo layer if there more operations
        // for time purposes, I have used an in memory list and just plastered it here
        private List<SKU> skuList = new List<SKU>{
        new SKU{Name = "A", Price = 50},
        new SKU{Name = "B", Price = 30},
        new SKU{Name = "C", Price = 20},
        new SKU{Name = "D", Price = 15}
        };
        public SKU GetSKUInformation(string skuReference)
        {
            var skuSearch = skuList.SingleOrDefault(s => s.Name == skuReference);
            
            if(skuSearch == null)
            {
                throw new KeyNotFoundException($"SKU {skuReference} not found or there were multiple SKUs found");
            }

            return skuSearch;
        }
    }

    public interface IWarehouse
    {
        SKU GetSKUInformation(string skuReference);
    }
}
