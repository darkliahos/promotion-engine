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
        private readonly IPromotionRuleEngine promotionRuleEngine;

        public Checkout(IWarehouse warehouse, IPromotionRuleEngine promotionRuleEngine)
        {
            this.warehouse = warehouse;
            this.promotionRuleEngine = promotionRuleEngine;
        }


        public void AddSKUToCart(string skuName, int quantity)
        {
            var sku = warehouse.GetSKUInformation(skuName);
            SKUCart.Add(new CartItem { SKU = sku, Quantity = quantity });
        }

        public int CalculateTotal()
        {
            int total = 0;

            var promotions = promotionRuleEngine.ApplyPromotion(SKUCart);

            var skuGrouped = SKUCart.GroupBy(g => g.SKU.Name).Select(sg => new
            {
                sg.Key,
                Price = sg.First().SKU.Price,
                TotalQuantity = sg.Sum(s => s.Quantity)
            });

            foreach (var skuCartItem in skuGrouped) 
            {
                var multiBuyPromos = promotions.SingleOrDefault(x => x.SKUReference == skuCartItem.Key && x.Type == PromotionType.MultibuyOffer);
                if(multiBuyPromos != null)
                {
                    total += multiBuyPromos.PromoPrice;
                    total += skuCartItem.Price * (skuCartItem.TotalQuantity - multiBuyPromos.QuantityApplied);
                }

                var specificItemsPromotions = promotions.SingleOrDefault(x =>  x.Type == PromotionType.HaveCertainProductsInYourBasket && x.SKUReference.Split(",").Any(x=> x.Trim(',') == skuCartItem.Key));

                if (specificItemsPromotions != null)
                {
                    total += skuCartItem.Price * (skuCartItem.TotalQuantity - specificItemsPromotions.QuantityApplied);
                }
                
                if(specificItemsPromotions == null && multiBuyPromos == null)
                {
                    total += skuCartItem.Price * skuCartItem.TotalQuantity;
                }

            }

            foreach(var specificPromos in promotions.Where(x=> x.Type == PromotionType.HaveCertainProductsInYourBasket))
            {
                total += specificPromos.PromoPrice;
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
