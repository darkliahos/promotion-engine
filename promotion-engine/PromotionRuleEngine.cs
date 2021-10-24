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

                        if(eligableItems.First().TotalQuantity >= discount.QualifyingQuantity)
                        {
                            var timesApplied = eligableItems.First().TotalQuantity / discount.QualifyingQuantity;
                            var quanityAffected = timesApplied * discount.QualifyingQuantity;
                            var subractionPercentage = (100M - discount.Percentage) / 100;
                            var promoPrice = (eligableItems.First().Price * quanityAffected) * subractionPercentage;
                            promotionApplications.Add(new PromotionApplication { QuantityApplied = quanityAffected, 
                                SKUReference = eligableItems.First().Key, 
                                PromoPrice = (int)promoPrice, 
                                Type = discount.Type });
                        }
                        break;
                    case PromotionType.HaveCertainProductsInYourBasket:
                        var discountEligable = true;
                        var groupedCart = Cart.GroupBy(gci => gci.SKU.Name)
                            .Select(gcis => new { gcis.Key, TotalQuantity = gcis.Sum(g => g.Quantity), Price = gcis.First().SKU.Price });

                        var itemsToApplyPromosTo = new List<GroupedCartItem>();
                        foreach (var skuRef in discount.SKUReference)
                        {
                            var eligibleProduct = groupedCart.FirstOrDefault(c => c.Key == skuRef);

                            if (eligibleProduct == null)
                            {
                                discountEligable = false; 
                            }
                            else
                            {
                                itemsToApplyPromosTo.Add(new GroupedCartItem
                                {
                                    SKUReference = eligibleProduct.Key,
                                    Price = eligibleProduct.Price,
                                    Quantity = eligibleProduct.TotalQuantity
                                });
                            }

                        }

                        if (discountEligable)
                        {
                            var lowestCommonQuantity = itemsToApplyPromosTo.First().Quantity;
                            var skuReferences = "";

                            foreach(var itapt in itemsToApplyPromosTo)
                            {
                                skuReferences += $"{itapt.SKUReference},";
                                if (itapt.Quantity < lowestCommonQuantity)
                                {
                                    lowestCommonQuantity = itapt.Quantity;
                                }
                            }

                            var subractionPercentage = (100M - discount.Percentage) / 100;
                            var withoutDiscount = itemsToApplyPromosTo.Select(x=> new {
                                TotalPrice = x.Price * lowestCommonQuantity
                            });

                            var totalPrice = decimal.Ceiling((withoutDiscount.Sum(x => x.TotalPrice) * subractionPercentage));

                            promotionApplications.Add(new PromotionApplication
                            {

                                Type = discount.Type, QuantityApplied = lowestCommonQuantity, SKUReference = skuReferences, PromoPrice = (int)totalPrice

                            }); ;
                        }


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
