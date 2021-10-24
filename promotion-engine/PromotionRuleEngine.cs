using promotion_engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotion_engine
{
    public class PromotionRuleEngine : IPromotionRuleEngine
    {
        private List<Discount> DiscountStore = new List<Discount>
        {
            new Discount
            {
                SKUReference = new string[] {"A"},
                QualifyingQuantity = 3,
                Percentage = 13.33M,
                Type = PromotionType.MultibuyOffer
            },
            new Discount
            {
                SKUReference = new string [] {"B" },
                QualifyingQuantity = 2,
                Percentage = 25.00M,
                Type = PromotionType.MultibuyOffer
            },
            new Discount
            {
                SKUReference = new string [] {"C", "D" },
                QualifyingQuantity = 1,
                Percentage = 14.29M,
                Type = PromotionType.HaveCertainProductsInYourBasket
            }
        };

        public IEnumerable<PromotionApplication> ApplyPromotion(List<CartItem> Cart)
        {
            var promotionApplications = new List<PromotionApplication>();
            foreach(var discount in DiscountStore)
            {
                switch (discount.Type)
                {
                    case PromotionType.MultibuyOffer:
                        var eligableItems = Cart.Where(ci => ci.SKU.Name == discount.SKUReference.First()).GroupBy(gci => gci.SKU.Name)
                            .Select(gcis => new { gcis.Key, TotalQuantity = gcis.Sum(g=> g.Quantity), Price = gcis.First().SKU.Price});

                        if(eligableItems.First().TotalQuantity > discount.QualifyingQuantity)
                        {
                            var timesApplied = eligableItems.First().TotalQuantity / discount.QualifyingQuantity;
                            var quanityAffected = timesApplied * discount.QualifyingQuantity;
                            var subractionPercentage = (100M - discount.Percentage) / 100;
                            var promoPrice = (eligableItems.First().Price * quanityAffected) * subractionPercentage;
                            promotionApplications.Add(new PromotionApplication { QuantityApplied = quanityAffected, SKUReference = eligableItems.First().Key, PromoPrice = (int)promoPrice, Type = discount.Type });
                        }
                        break;
                    case PromotionType.HaveCertainProductsInYourBasket:



                        break;
                }
            }

            return promotionApplications;
        }
    }

    public interface IPromotionRuleEngine
    {
        IEnumerable<PromotionApplication> ApplyPromotion(List<CartItem> Cart);
    }
}
